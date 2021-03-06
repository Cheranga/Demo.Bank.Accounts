@description('Name for the container group')
param name string

@description('The DNS name')
param dnsName string

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Container image to deploy')
param image string = 'mcr.microsoft.com/azuredocs/aci-helloworld'

@description('Port to open on the container and the public IP address.')
param port int = 80

@description('The number of CPU cores to allocate to the container.')
param cpuCores int = 1

@description('The amount of memory to allocate to the container in gigabytes.')
param memoryInGb int = 1

@description('The behavior of Azure runtime if container has stopped.')
@allowed([
  'Always'
  'Never'
  'OnFailure'
])
param restartPolicy string = 'OnFailure'

@secure()
param storageAccount string

param databaseSetupImage string
param databaseServerName string
param databaseName string
param databaseUserName string
param newBankAccountsQueue string
param visibilityInSeconds int = 3000
param pollingSeconds int = 60

@secure()
param databasePassword string

@secure()
param databaseConnectionString string

var aspNetCoreEnvironment = 'Production'

resource containerGroup 'Microsoft.ContainerInstance/containerGroups@2021-09-01' = {
  name: name
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    initContainers: [
      {
        name: 'databasesetup'
        properties: {
          image: databaseSetupImage
          environmentVariables:[
            {
              name: 'SERVER_NAME'
              value: databaseServerName
            }
            {
              name: 'USERNAME'
              value: databaseUserName
            }            
            {
              name: 'PASSWORD'
              value: databasePassword
            }            
            {
              name: 'DATABASE_NAME'
              value: databaseName
            }            
          ]
        }
      }
    ]
    containers: [
      {
        name: name
        properties: {
          image: image
          ports: [
            {
              port: port
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: cpuCores
              memoryInGB: memoryInGb
            }
          }
          environmentVariables: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: aspNetCoreEnvironment
            }
            {
              name: 'StorageAccount'
              value: storageAccount
            }
            {
              name: 'BankAccountConfig__DatabaseConnectionString'
              value: databaseConnectionString
            }   
            {
              name: 'BankAccountConfig__NewBankAccountsQueue'
              value: newBankAccountsQueue
            } 
            {
              name: 'BankAccountConfig__VisibilityInSeconds'
              value: string(visibilityInSeconds)
            } 
            {
              name: 'BankAccountConfig__PollingSeconds'
              value: string(pollingSeconds)
            }                          
          ]
        }
      }
    ]
    osType: 'Linux'
    restartPolicy: restartPolicy
    ipAddress: {
      type: 'Public'
      ports: [
        {
          port: port
          protocol: 'TCP'
        }
      ]
      dnsNameLabel: dnsName
    }
  }
}

output containerIPv4Address string = containerGroup.properties.ipAddress.ip
output managedId string = containerGroup.identity.principalId
