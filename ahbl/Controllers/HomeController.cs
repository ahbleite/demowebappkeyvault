using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace ahbl.Controllers
{
    public class HomeController : Controller
    {
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            string headers = string.Empty;

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            try
            {
                var client = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                var secret = await client.GetSecretAsync("https://senpkev.vault.azure.net/secrets/mysecret").ConfigureAwait(false);

                ViewBag.Secret = secret.Value;

            }
            catch (Exception exp)
            {
                ViewBag.Error = exp.Message;
            }

            ViewBag.Principal = azureServiceTokenProvider.PrincipalUsed != null ? $"Is Authenticated: {azureServiceTokenProvider.PrincipalUsed.IsAuthenticated.ToString()}, " +
                $"App Id: {azureServiceTokenProvider.PrincipalUsed.AppId}" : string.Empty;            


            foreach (string strKey in Request.Headers.AllKeys)
                headers += $"<div class='divTableRow'>\n<div class='divTableCell'>{strKey}</div>\n<div class='divTableCell'>{Request.Headers[strKey] }\n</div>\n</div>\n";

            ViewBag.Headers = headers;

            return View();

        }
    }
}