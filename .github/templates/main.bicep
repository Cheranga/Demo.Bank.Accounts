param buildNumber string
param appName string
param environmentName string
param location string = resourceGroup().location
param containerImage string
param databaseSetupImage string
param sqlServerLocation string
param databaseServerName string
param databaseName string
param databaseUserName string
@secure()
param databasePassword string

param pollingSeconds int
param visibilityInSeconds int

var storageName = 'sg${appName}${environmentName}'
var aciName = 'aci-${appName}-${environmentName}'
var newBankAccountsQueue = 'newbankaccounts'
var tableNames = ''

module storageAccount 'storageaccount/template.bicep' = {
  name: '${buildNumber}-storage-account'
  params: {
    location: location
    name: storageName
    queues: newBankAccountsQueue
    tables: tableNames
  }
}

module database 'sqlserver/template.bicep' = {
  name: '${buildNumber}-database-setup'
  params: {
    location: sqlServerLocation
    serverName: databaseServerName
    databaseName: databaseName
    adminUserName: databaseUserName
    adminPassword: databasePassword
  }
}

module containerInstance 'aci/template.bicep' = {
  name: '${buildNumber}-container-instance'
  params: {
    location: location
    name: aciName
    databaseConnectionString: database.outputs.connectionString
    databaseServerName: database.outputs.databaseServerUrl
    databaseName: databaseName
    databaseUserName: databaseUserName
    databasePassword: databasePassword
    databaseSetupImage: databaseSetupImage
    dnsName: '${appName}-${environmentName}'
    storageAccount: storageName
    image: containerImage
    newBankAccountsQueue: newBankAccountsQueue
    visibilityInSeconds: visibilityInSeconds
    pollingSeconds: pollingSeconds
  }
  dependsOn: [
    storageAccount
    database
  ]
}

module rbacqueue 'rbac/template.bicep'= {
  name: '${buildNumber}-rbac-queues'
  params: {    
    accessibility: 'queue_read_write'
    friendlyName: '${appName}queueaccess'
    principalId: containerInstance.outputs.managedId
    storageAccountName: storageName
  }
  dependsOn:[
    containerInstance
    storageAccount
  ]
}
