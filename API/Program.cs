using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text.Json;
using System.Threading.Tasks;

namespace API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            Console.Write("Api: "); // FOR API = app.exchangerate-api.com
            string api = Console.ReadLine();
            Console.Clear();

            Program program = new Program();
            List<string> ParaBirimleri = new List<string>()
            {
                "USD",
                "EUR",
                "GBP",
                "RUB",
            };

            foreach (var item in ParaBirimleri)
            {
                Console.WriteLine($"1 {item} = {await program.Doviz(api, item)} TL");
            }
        }


        async Task<double> Doviz(string api, string birim)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://v6.exchangerate-api.com/v6/{api}/latest/{birim}");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var jsonDocument = JsonDocument.Parse(responseBody);
                    var conversionRates = jsonDocument.RootElement.GetProperty("conversion_rates");

                    var _Try = conversionRates.GetProperty("TRY").GetDouble();
                    return _Try;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("EROR");
                    Environment.Exit(0);
                    return 00.00;
                }
            }
        }
    }
}