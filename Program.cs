using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace QueueConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Config Builder
            var build = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            
            //Set config
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            CloudStorageAccount myClient = CloudStorageAccount.Parse(config["connectionstring"]);

            //Set communication with the service
            CloudQueueClient queueClient = myClient.CreateCloudQueueClient();

            //Its like containr in blob storage
            CloudQueue queue = queueClient.GetQueueReference("rowprocess");
            queue.CreateIfNotExists();

            for (int i = 0; i < 500; i++)
            {
                CloudQueueMessage message = new CloudQueueMessage(string.Format("Operation: {0}", i));
                queue.AddMessage(message);

                Console.WriteLine(i.ToString() + "New message is publish");
            }

            Console.ReadLine();

        }
    }
}
