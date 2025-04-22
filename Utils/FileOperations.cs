public class FileOperations
{
    static string RUN_ARTIFACTS_PATH = "./RunArtifacts";

    private static void _CheckAndCreateArtifactsDirectory()
    {
        try {
            if (!Directory.Exists(RUN_ARTIFACTS_PATH)) {
                DirectoryInfo di = Directory.CreateDirectory(RUN_ARTIFACTS_PATH);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(RUN_ARTIFACTS_PATH));
            }
        } catch (Exception ex) {
            Console.WriteLine($"there was a problem checking for/creating the dictionary at {RUN_ARTIFACTS_PATH}:\n{ex}");
        }
    }

    public static async Task WriteJsonToFile(string filePath, string jsonResponse)
    {
        try
        {
            _CheckAndCreateArtifactsDirectory();
            await File.WriteAllTextAsync($"{RUN_ARTIFACTS_PATH}/{filePath}", jsonResponse);
            Console.WriteLine($"JSON response saved to: {RUN_ARTIFACTS_PATH}/{filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save response to: {RUN_ARTIFACTS_PATH}/{filePath}:\n{ex}");
        }
    }


}