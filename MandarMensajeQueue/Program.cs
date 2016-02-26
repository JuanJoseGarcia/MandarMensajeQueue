using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace MandarMensajeQueue
{
    class Program
    {
        public class Message
        {
            public string Asunto { get; set; }
            public string Contenido { get; set; }
        }

        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["ConexionAzure"].ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("messages");
            queue.CreateIfNotExists();

            Console.WriteLine("Enviar mensajes.... (ESC para sali)");

            Message mess = new Message();

            do
            {
                Console.Write("Asunto: ");
                mess.Asunto = Console.ReadLine();
                Console.Write("Contenido: ");
                mess.Contenido = Console.ReadLine();

                queue.AddMessage(
                        new CloudQueueMessage(JsonConvert.SerializeObject(mess)),
                        TimeSpan.FromDays(5.0),
                        TimeSpan.FromDays(0.0)
                    );


            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);


        }
    }
}
