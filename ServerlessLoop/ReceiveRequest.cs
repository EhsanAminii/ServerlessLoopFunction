using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServerlessLoop.Domain;

namespace ServerlessLoop
{
    public static class ReceiveRequest
    {
        [FunctionName("ReceiveRequest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [ServiceBus("messagequeue", Connection = "Serverless.ServiceBusQueue.Connection")] IAsyncCollector<ServerlessMessage> message,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var serverlessMessage = JsonConvert.DeserializeObject<ServerlessMessage>(requestBody);

            await message.AddAsync(serverlessMessage);

            return new OkObjectResult($"Process started with Amount {serverlessMessage.Amount}");
        }
    }
}