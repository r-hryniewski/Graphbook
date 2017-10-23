# Graphbook
Simple Cosmos DB Graph API sample for my talk (link to slides soon).

Sample should be deployed on azure App Service with configured Azure App Service Authentication with Facebook (http://hryniewski.net/2017/04/20/azure-app-services-authentication-with-fb-google-and-others-in-5-minutes-or-so/).

In Application Settings you should include those settings from Cosmos DB (key // description):
* CosmosDBEndpoint - Endpoint for your Cosmos DB
* CosmosDBKey - Key for accessing your Cosmos DB
* CosmosDBDatabaseId - Id for your Database in Cosmos DB (it will be created automatically if not exists)
* CosmosDBCollectionId - Id for your Collection in Cosmos DB (it will be created automatically if not exists)


