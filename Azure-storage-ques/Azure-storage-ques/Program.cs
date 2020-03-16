using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types
using Newtonsoft.Json;

namespace Azure_storage_ques
{
    class Program
    {

        static void Main(string[] args)
        {
          ProcessIsues().GetAwaiter().GetResult();

        }

        private static async Task ProcessIsues()
        {

            CloudStorageAccount storageAccount = null;
            CloudQueue queue = null;

            string storageConnectionString = "provide connection to your string";
            CloudStorageAccount.TryParse(storageConnectionString, out storageAccount);
            try
                {

                    CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                    queue = cloudQueueClient.GetQueueReference("que nane in all lower case");
                    bool createdQueue = await queue.CreateIfNotExistsAsync();

                     if (createdQueue)
                        {
                            Console.WriteLine("The queue was created.");
                       }

                Dictionary<string, string> value = new Dictionary<string, string>
                {
                    {"yes","no" }
                };
                string json = JsonConvert.SerializeObject(value, Formatting.Indented);
                CloudQueueMessage message = new CloudQueueMessage(json);
                await queue.AddMessageAsync(message, new TimeSpan(0, 0, 1, 0), null, null, null);
                CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();
                string messagedata = retrievedMessage.AsString;
                Console.WriteLine(messagedata);
                Console.WriteLine();

            }

                catch (Exception ex)
                {
                    Console.WriteLine("Error returned from Azure Storage: {0}", ex.Message);


                }
            }

        }



    }
