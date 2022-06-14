param buildNumber string
param appName string
param environmentName string
param location string = resourceGroup().location
param containerImage string
param hotelCancellationQueue string
param pollingInSeconds int
param visibilityInSeconds int
param cancellationsTable string

var storageName = 'sg${appName}${environmentName}'
var aciName = 'aci-${appName}-${environmentName}'
var tableNames = ''

module storageAccount 'storageaccount/template.bicep' = {
  name: '${buildNumber}-storage-account'
  params: {
    location: location
    name: storageName
    queues: newbankaccounts
    tables: tableNames
  }
}