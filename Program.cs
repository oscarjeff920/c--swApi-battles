using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

using ResponseModels;
class Program

{
    static async Task Main(string[] args)
    {
        string apiUrl = "https://www.swapi.tech/api/people/"; // Example: Get information about the first person

        List<PersonProperties> scrapedPeople = new List<PersonProperties>();

        int personId = 1;
        while (true)
        {

            Console.WriteLine($"current personId: {personId}");
            PersonProperties? person = await CallPersonEndpoint(personId, apiUrl);
            if (person != null)
            {
                Console.WriteLine($"Scraped {person.Name}!");
                scrapedPeople.Add(person);
                Console.WriteLine($"scrapedPeople length: {scrapedPeople.Count}");
                personId++;
            }
            else
            {
                break;
            }
        }

        await FileOperations.WriteJsonToFile("scrapedPeople.json", JsonSerializer.Serialize(scrapedPeople));
    }

    static async public Task<PersonProperties?> CallPersonEndpoint(int personId, string baseUrl)
    {
        string apiUrl = $"{baseUrl}{personId}/";
        HttpClient client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            string responseAsString = await response.Content.ReadAsStringAsync();

            await FileOperations.WriteJsonToFile("SWapi-response.json", responseAsString);

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                SWapiResponse swapiResponse = JsonSerializer.Deserialize<SWapiResponse>(jsonString)!;

                PersonProperties person = swapiResponse.Result.Properties;

                // PersonProperties person = JsonSerializer.Deserialize<PersonProperties>(jsonString);

                await FileOperations.WriteJsonToFile("deserialized-response.json", JsonSerializer.Serialize(person));


                Console.WriteLine($"Name: {person.Name}");
                Console.WriteLine($"Height: {person.Height}");
                Console.WriteLine($"Mass: {person.Mass}");
                Console.WriteLine($"DoB: {person.BirthYear}");
                // Output other properties as needed
                return person;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request Exception: {e.Message}");
            return null;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"JSON Deserialization Exception: {e.Message}");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            return null;
        }

    }
}