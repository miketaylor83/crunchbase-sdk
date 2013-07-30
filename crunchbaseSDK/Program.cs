using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using crunchbase.Models;
namespace AwaitAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = String.Empty;
            Console.Write("Enter your API key: ");
            apiKey = Console.ReadLine();

            if (!String.IsNullOrEmpty(apiKey))
            {
                crunchbase.Models.crunchbaseRequest req = new crunchbase.Models.crunchbaseRequest("1", apiKey);
                crunchbase.Models.crunchbaseResponse<crunchbase.Models.crunchbaseEntity> res = req.getEntity(entityType.company, "google");
                Console.WriteLine(res.result.permalink);
            }
        }
    }
}
