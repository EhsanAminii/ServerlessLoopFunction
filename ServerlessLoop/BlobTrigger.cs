using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessLoop.Domain;

namespace ServerlessLoop
{
    public static class BlobTrigger
    {
        [FunctionName("BlobTrigger")]
        public static void Run(
            [BlobTrigger("messages/{messageId}", Connection = "Serverless.Blobstorage.Connection")]string serverlessBlob, string messageId,
            [ServiceBus("messagequeue", Connection = "Serverless.ServiceBusQueue.Connection")]IAsyncCollector<ServerlessMessage> queueMessage,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{messageId} \n Size: {serverlessBlob.Length} Bytes");

            var serverlessMessage = JsonConvert.DeserializeObject<ServerlessMessage>(serverlessBlob);

            try
            {
                var outputMessages = GenerateNewMessages(serverlessMessage);

                foreach (var outputMessage in outputMessages)
                {
                    queueMessage.AddAsync(outputMessage).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static List<ServerlessMessage> GenerateNewMessages(ServerlessMessage serverlessMessage)
        {
            return new List<ServerlessMessage>
            {
                new ServerlessMessage
                {
                    MessageId = Guid.NewGuid(),
                    Amount = 2 * serverlessMessage.Amount,
                    ParentTotal = serverlessMessage.Amount
                },
                new ServerlessMessage
                {
                    MessageId = Guid.NewGuid(),
                    Amount = 3 *serverlessMessage.Amount,
                    ParentTotal = serverlessMessage.Amount
                }
            };
        }
    }
}