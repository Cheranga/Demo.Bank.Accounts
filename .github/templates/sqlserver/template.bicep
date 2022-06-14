@description('The name of the SQL logical server.')
param serverName string

@description('The name of the SQL Database.')
param databaseName string

@description('Location for all resources.')
param serverLocation string = resourceGroup().location

@description('The administrator username of the SQL logical server.')
param adminUserName string

@description('The administrator password of the SQL logical server.')
@secure()
param adminPassword string

resource sqlServer 'Microsoft.Sql/servers@2021-11-01-preview' existing = {
  name: serverName
}

resource serverName_databaseName 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {  
  name: '${sqlServer.name}/${databaseName}'
  location: serverLocation
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 1073741824
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
  }
}

output connectionString string = 'Server=tcp:${serverName}.database.windows.net,1433;Initial Catalog=${databaseName};Persist Security Info=False;User ID=${adminUserName};Password=${adminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
