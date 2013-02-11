using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace MvcWebRole
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            CreateStorage();
        }

        /// <summary>
        /// Creates the tables/queues/blobs.
        /// </summary>
        private static void CreateStorage()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            //Tables
            var tableCLient = cloudStorageAccount.CreateCloudTableClient();            
            var mailingListTable = tableCLient.GetTableReference("MailingListTable");
            mailingListTable.CreateIfNotExists();
            var messageTable = tableCLient.GetTableReference("MessageTable ");
            messageTable.CreateIfNotExists();

            //Blob
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("azuremailblobcontainer");
            blobContainer.CreateIfNotExists();

            //Queue
            var queueClient = cloudStorageAccount.CreateCloudQueueClient();
            var subscribeQueue = queueClient.GetQueueReference("azuremailsubscribequeue");
            subscribeQueue.CreateIfNotExists();
        }
    }
}