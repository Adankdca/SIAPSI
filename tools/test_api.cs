using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://localhost:59914/api/Adecuaciones");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.StatusCode);
        if (content.Length > 200) {
            Console.WriteLine(content.Substring(0, 200) + "...");
        } else {
            Console.WriteLine(content);
        }
    }
}
