using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;

namespace DotNetCore_AzureStorage.Pages
{
    public class returnObject
    {
        public string name {get;set;}
        public string accessTier {get;set;}
        public string hostName {get;set;}
        public string nsLookupIp {get;set;}
    }

    public class IndexModel : PageModel
    {
        [BindProperty]
        public string storageAccountConnectionString {get;set;}
        [BindProperty]
        public string containerName {get;set;}

        public returnObject storageReturn {get;set;}
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            storageReturn = new returnObject();
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            BlobContainerClient client= new BlobContainerClient(storageAccountConnectionString,containerName);
            BlobItem items = client.GetBlobs(BlobTraits.All, BlobStates.All).First();
            IPHostEntry resolver = Dns.GetHostEntry(storageAccountConnectionString.Split("AccountName=")[1].Split(";")[0] + ".blob.core.windows.net");
            storageReturn.name = items.Name;
            storageReturn.accessTier = items.Properties.AccessTier.ToString();
            storageReturn.hostName = storageAccountConnectionString.Split("AccountName=")[1].Split(";")[0] + ".blob.core.windows.net";
            storageReturn.nsLookupIp = resolver.AddressList.First().ToString();
        }
    }
}
