@description('Location for all resources.')
param location string = resourceGroup().location

@description('The name of the SQL logical server.')
param serverName string

@description('The name of the SQL Database.')
param databaseName string

@description('The administrator username of the SQL logical server.')
param adminUserName string

@description('The administrator password of the SQL logical server.')
@secure()
param adminPassword string

var dbServerUrl = '${serverName}.database.windows.net'

resource serverName_resource 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: adminUserName
    administratorLoginPassword: adminPassword
  }
}

resource serverName_databaseName 'Microsoft.Sql/servers/databases@2020-08-01-preview' = {
  parent: serverName_resource
  name: databaseName
  location: location
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
    readScale: 'Disabled'
  }
}

resource serverName_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2020-02-02-preview' = {
  parent: serverName_resource
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource symbolicname 'Microsoft.Sql/servers/connectionPolicies@2021-11-01-preview' = {
  name: 'default'
  parent: serverName_resource
  properties: {
    connectionType: 'Redirect'
  }
  dependsOn: [
    serverName_databaseName
  ]
}

output connectionString string = 'Server=tcp:${dbServerUrl},1433;Initial Catalog=${databaseName};Persist Security Info=False;User ID=${adminUserName};Password=${adminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
