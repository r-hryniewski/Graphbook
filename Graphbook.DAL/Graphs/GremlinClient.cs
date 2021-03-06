﻿using Microsoft.Azure;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Graphbook.DAL.Graphs
{
    public class GremlinClient
    {
        private static readonly string CosmosDBEndpoint = CloudConfigurationManager.GetSetting("CosmosDBEndpoint");
        private static readonly string CosmosDBAuthKey = CloudConfigurationManager.GetSetting("CosmosDBKey");
        private static readonly string CosmosDBDatabaseId = CloudConfigurationManager.GetSetting("CosmosDBDatabaseId");
        private static readonly string DBDocsCollectionId = CloudConfigurationManager.GetSetting("CosmosDBCollectionId");

        private static DocumentClient client;
        private static Database database;
        private static DocumentCollection graph;

        public GremlinClient()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Init();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public static async Task Init()
        {
            await GetGraph();
        }

        private static async Task<DocumentCollection> GetGraph()
        {
            if (client == null)
                client = new DocumentClient(
                    new Uri(CosmosDBEndpoint),
                    CosmosDBAuthKey,
                    new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });

            if (database == null)
                database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = CosmosDBDatabaseId });

            if (graph == null)
                graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                    databaseUri: UriFactory.CreateDatabaseUri(CosmosDBDatabaseId),
                    documentCollection: new DocumentCollection { Id = DBDocsCollectionId },
                    options: new RequestOptions { OfferThroughput = 400 }); //TODO: Extract this to some kind of settings

            return graph;
        }

        public async Task<IEnumerable<TResult>> Execute<TResult>(string gremlinQuery, Func<Vertex, TResult> vertexSelector, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var query = client.CreateGremlinQuery<Vertex>(await GetGraph(), gremlinQuery);
                var results = new List<TResult>();
                while (query.HasMoreResults)
                {
                    foreach (var vertex in await query.ExecuteNextAsync<Vertex>())
                    {
                        results.Add(vertexSelector(vertex));
                    }
                }
                return results;
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }
        }

        public async Task<dynamic> ExecuteDynamic(string gremlinQuery, CancellationToken ct = default(CancellationToken))
        {
            var query = client.CreateGremlinQuery<Vertex>(await GetGraph(), gremlinQuery, new FeedOptions() { MaxItemCount = null });
            return await query.ExecuteNextAsync<dynamic>();
        }

        public async Task<FeedResponse<object>> ExecuteFeedResponse(string gremlinQuery, CancellationToken ct = default(CancellationToken))
        {
            var query = client.CreateGremlinQuery<Vertex>(await GetGraph(), gremlinQuery, new FeedOptions() { MaxItemCount = null });
            return await query.ExecuteNextAsync<object>();
        }

        public async Task Execute(string gremlinQuery, CancellationToken ct = default(CancellationToken))
        {
            var query = client.CreateGremlinQuery<Vertex>(await GetGraph(), gremlinQuery);
            while (query.HasMoreResults)
            {
                await query.ExecuteNextAsync<Vertex>();
            }
        }
    }
}
