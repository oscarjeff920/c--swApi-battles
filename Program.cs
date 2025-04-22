using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using ResponseModels;

public class FileOperations
{
    public static async Task WriteJsonToFile(string filePath, string jsonResponse)
    {
        try
        {
            await File.WriteAllTextAsync(filePath, jsonResponse);
            Console.WriteLine($"JSON response saved to: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save response to: {filePath}:\n{ex}");
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        string mockJsonString = "{\"data\": 1}";
        await FileOperations.WriteJsonToFile("test.json", mockJsonString);
        string apiUrl = "https://www.swapi.tech/api/people/1/"; // Example: Get information about the first person

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                string responseAsString = await response.Content.ReadAsStringAsync();

                await FileOperations.WriteJsonToFile("SWapi-response.json", responseAsString);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    SWapiResponse swapiResponse = JsonSerializer.Deserialize<SWapiResponse>(jsonString);

                    PersonProperties person = swapiResponse.Result.Properties;

                    // PersonProperties person = JsonSerializer.Deserialize<PersonProperties>(jsonString);

                    await FileOperations.WriteJsonToFile("deserialized-response.json", JsonSerializer.Serialize(person));


                    Console.WriteLine($"Name: {person.Name}");
                    Console.WriteLine($"Height: {person.Height}");
                    Console.WriteLine($"Mass: {person.Mass}");
                    Console.WriteLine($"DoB: {person.BirthYear}");
                    // Output other properties as needed
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request Exception: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON Deserialization Exception: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error occurred: {e.Message}");
            }
        }
    }
}