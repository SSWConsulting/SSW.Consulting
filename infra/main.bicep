
// start API
param projectName string = 'sswrewards'
// and API

// start Admin Portal
//param projectName string = 'sswrewardsadmin'
// end Admin portal

param location string = resourceGroup().location

@allowed([
  'dev'
  'staging'
  'prod'
])
param environment string

// start API
param appServicePlanName string
param appServicePlanResourceGroup string

param sqlAdministratorsLoginName string
param sqlAdministratorsObjectId string

param adminPortalUrl string
param idsUrl string

@secure()
@description('Specifies secrets to connect to apple https://docs.microsoft.com/en-us/azure/templates/microsoft.notificationhubs/namespaces/notificationhubs?tabs=bicep#apnscredentialproperties Sample: {"keyId":"key id","appName":"bundle id","appId":"team id","token":"token"}')
param notificationHubAppleCredential object = {}

@secure()
param notificationHubGoogleApiKey string = ''

param now string = utcNow('yyyyMMddHHmmss')

module keyVault 'modules/keyvault.bicep' = {
  name: 'kv-${now}'
  params: {
    projectName: projectName
    environment: environment
    location: location
  }
}

module sqlServer 'modules/sql.bicep' = {
  name: 'sql-${now}'
  params: {
    projectName: projectName
    environment: environment
    location: location
    sqlAdministratorsLoginName: sqlAdministratorsLoginName
    sqlAdministratorsObjectId: sqlAdministratorsObjectId
    keyVaultName: keyVault.outputs.keyVaultName
    now: now
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' existing = {
  name: appServicePlanName
  scope: resourceGroup(appServicePlanResourceGroup)
}

module appService 'modules/webapp.bicep' = {
  name: 'api-${now}'
  params: {
    appServicePlanId: appServicePlan.id
    environment: environment
    location: location
    projectName: projectName
    keyVaultName: keyVault.outputs.keyVaultName
    adminPortalUrl: adminPortalUrl
    idsUrl: idsUrl
    sqlConnectionStringSecretUriWithVersion: sqlServer.outputs.sqlConnectionStringSecretUriWithVersion
  }
}

// Role = Key Vault Secrets User
// Note the confusion that the role ".name" property IS the GUID we need. The .roleName property is the text name of the role :(
// obtain the role id via: az role definition list --name "Key Vault Secrets User" --query "[].name" -o tsv
var keyVaultSecretsUserRoleId = '4633458b-17de-408a-b874-0445c86b69e6'

module kvWebAppRoleAssignment 'modules/add-kv-role-assignment.bicep' = {
  name: 'kvra-${now}'
  params: {
    keyVaultName: keyVault.outputs.keyVaultName
    principalId: appService.outputs.principalId
    roleDefinitionId: keyVaultSecretsUserRoleId
  }
}

module storage 'modules/storage.bicep' = {
  name: 'st-${now}'
  params: {
    environment: environment
    location: location
    projectName: projectName
    keyVaultName: keyVault.outputs.keyVaultName
    now: now
  }
}

module notificationhub 'modules/notificationhub.bicep' = {
  name: 'nh-${now}'
  params: {
    environment: environment
    location: location
    projectName: projectName
    keyVaultName: keyVault.outputs.keyVaultName
    now: now
    appleCredential: notificationHubAppleCredential
    googleApiKey: notificationHubGoogleApiKey
  }
}

output appServiceName string = appService.outputs.appServiceName
// end API

//start Admin portal
@description('The name of the storage account to use for site hosting.')
param storageAccountName string = 'st${projectName}${environment}${uniqueString(resourceGroup().id)}'

@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_ZRS'
  'Premium_LRS'
])
@description('The storage account sku name.')
param storageSku string = 'Standard_LRS'

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: take(storageAccountName, 24)
  location: location
  kind: 'StorageV2'
  sku: {
    name: storageSku
  }
}

output storageAccountName string = storageAccount.name
output storageAccountUrl string = storageAccount.properties.primaryEndpoints.web
//end Admin Portal
