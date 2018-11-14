using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ServerlessLoop
{
    public static class StoreMessage
    {
        [FunctionName("StoreMessage")]
        public static void Run(
            [ServiceBusTrigger("messagequeue", Connection = "Serverless.ServiceBusQueue.Connection")]string serverlessMessage,
            [Blob("messages/{MessageId}", Connection = "Serverless.Blobstorage.Connection")] out string blobMessage,
            ILogger log)
        {
            try
            {
                blobMessage = serverlessMessage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}