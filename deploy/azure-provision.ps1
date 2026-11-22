param(
    [Parameter(Mandatory)] [string] $AppName,
    [Parameter(Mandatory)] [string] $SqlServerName,
    [Parameter(Mandatory)] [string] $SqlAdminPassword,
    [string] $ResourceGroup = "kompas-kompetencji",
    [string] $Location = "polandcentral",
    [string] $SqlAdminLogin = "kompasadmin",
    [string] $DatabaseName = "KompasKompetencji",
    [string] $PlanSku = "B1",
    [string] $AnthropicApiKey = ""
)

$ErrorActionPreference = "Stop"

az group create --name $ResourceGroup --location $Location | Out-Null

az sql server create --name $SqlServerName --resource-group $ResourceGroup --location $Location `
    --admin-user $SqlAdminLogin --admin-password $SqlAdminPassword | Out-Null

az sql server firewall-rule create --resource-group $ResourceGroup --server $SqlServerName `
    --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0 | Out-Null

az sql db create --resource-group $ResourceGroup --server $SqlServerName --name $DatabaseName `
    --edition GeneralPurpose --compute-model Serverless --family Gen5 --capacity 2 `
    --use-free-limit --free-limit-exhaustion-behavior AutoPause | Out-Null

az appservice plan create --name "$AppName-plan" --resource-group $ResourceGroup --location $Location `
    --sku $PlanSku --is-linux | Out-Null

az webapp create --name $AppName --resource-group $ResourceGroup --plan "$AppName-plan" `
    --runtime "DOTNETCORE:10.0" | Out-Null

az webapp config set --name $AppName --resource-group $ResourceGroup `
    --web-sockets-enabled true --always-on true --http20-enabled true `
    --startup-file "chmod +x /home/site/wwwroot/KompasKompetencji.Web && /home/site/wwwroot/KompasKompetencji.Web" | Out-Null

$connectionString = "Server=tcp:$SqlServerName.database.windows.net,1433;Database=$DatabaseName;User ID=$SqlAdminLogin;Password=$SqlAdminPassword;Encrypt=True;TrustServerCertificate=False;Connect Timeout=60;MultipleActiveResultSets=True"

az webapp config connection-string set --name $AppName --resource-group $ResourceGroup `
    --connection-string-type SQLAzure --settings "Default=$connectionString" | Out-Null

$settings = @(
    "ASPNETCORE_ENVIRONMENT=Demo",
    "ASPNETCORE_URLS=http://0.0.0.0:8080",
    "ASPNETCORE_FORWARDEDHEADERS_ENABLED=true",
    "WEBSITES_PORT=8080",
    "SCM_DO_BUILD_DURING_DEPLOYMENT=false"
)
if ($AnthropicApiKey) { $settings += "Assistant__ApiKey=$AnthropicApiKey" }

az webapp config appsettings set --name $AppName --resource-group $ResourceGroup --settings $settings | Out-Null

az webapp update --name $AppName --resource-group $ResourceGroup --https-only true | Out-Null

Write-Host "Gotowe: https://$AppName.azurewebsites.net (teraz uruchom deploy/azure-deploy.ps1)"
