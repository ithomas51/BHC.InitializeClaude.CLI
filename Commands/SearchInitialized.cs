// BHC.InitializedClaude.CLI/Commands/SearchInitialized.cs

using System.CommandLine;
using System.Diagnostics;

namespace BHC.InitializedClaude.CLI.Commands;

public static class SearchInitialized
{
    public static Command Create()
    {
        var searchCommand = new Command("search", "Search for .sln files and check if Claude is initialized in them");
        
        // Add aliases for the main command
        searchCommand.AddAlias("find");
        searchCommand.AddAlias("lookup");
        
        var directoryArgument = new Argument<string>(
            "directory",
            () => Environment.CurrentDirectory,
            "Directory to search for .sln files (defaults to current directory)"
        );
        
        var recursiveOption = new Option<bool>(
            "--recursive",
            () => true,
            "Search subdirectories recursively"
        );
        recursiveOption.AddAlias("-r");
        
        var includeNpmOption = new Option<bool>(
            "--check-npm",
            () => true,
            "Check if Claude is installed via npm globally"
        );
        includeNpmOption.AddAlias("--npm");
        
        searchCommand.AddArgument(directoryArgument);
        searchCommand.AddOption(recursiveOption);
        searchCommand.AddOption(includeNpmOption);
        
        // Action-based handler
        searchCommand.SetHandler((string directory, bool recursive, bool checkNpm) =>
        {
            SearchInitializedAction(directory, recursive, checkNpm);
        }, directoryArgument, recursiveOption, includeNpmOption);
        
        return searchCommand;
    }
    
    private static void SearchInitializedAction(string directory, bool recursive, bool checkNpm)
    {
        try
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Error: Directory '{directory}' does not exist.");
                return;
            }
            
            Console.WriteLine($"Searching for .sln files in: {Path.GetFullPath(directory)}");
            Console.WriteLine($"Recursive search: {recursive}");
            Console.WriteLine();
            
            // Search for .sln files
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var slnFiles = Directory.GetFiles(directory, "*.sln", searchOption);
            
            if (!slnFiles.Any())
            {
                Console.WriteLine("No .sln files found in the specified directory.");
                return;
            }
            
            Console.WriteLine($"Found {slnFiles.Length} solution file(s):");
            foreach (var slnFile in slnFiles)
            {
                Console.WriteLine($"  {Path.GetRelativePath(directory, slnFile)}");
            }
            Console.WriteLine();
            
            // Check if Claude is installed globally via npm
            if (checkNpm)
            {
                Console.WriteLine("Checking if Claude is installed globally via npm...");
                bool claudeInstalled = IsClaudeInstalledGlobally();
                
                if (claudeInstalled)
                {
                    Console.WriteLine("✓ Claude is installed globally via npm");
                }
                else
                {
                    Console.WriteLine("✗ Claude is not installed globally via npm");
                    Console.WriteLine("  Use 'init-claude install' to install Claude globally");
                }
                Console.WriteLine();
            }
            
            // For each solution, check if Claude is initialized
            Console.WriteLine("Checking Claude initialization status for each solution:");
            foreach (var slnFile in slnFiles)
            {
                CheckClaudeInitialization(slnFile);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: An unexpected error occurred during search");
            Console.WriteLine($"Details: {ex.Message}");
        }
    }
    
    private static bool IsClaudeInstalledGlobally()
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "list -g @anthropic-ai/claude-code --depth=0",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            using var process = Process.Start(processInfo);
            if (process == null) return false;
            
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            return output.Contains("@anthropic-ai/claude-code");
        }
        catch
        {
            return false;
        }
    }
    
    private static void CheckClaudeInitialization(string slnFile)
    {
        var slnDirectory = Path.GetDirectoryName(slnFile);
        var slnName = Path.GetFileNameWithoutExtension(slnFile);
        
        if (slnDirectory == null)
        {
            Console.WriteLine($"  {slnName}: Error - Could not determine directory");
            return;
        }
        
        // Check for common Claude initialization indicators
        var claudeConfigFile = Path.Combine(slnDirectory, ".claude_config");
        var claudeProject = Path.Combine(slnDirectory, ".claude");
        var packageJson = Path.Combine(slnDirectory, "package.json");
        
        var indicators = new List<string>();
        
        if (File.Exists(claudeConfigFile))
            indicators.Add(".claude_config file");
        
        if (Directory.Exists(claudeProject))
            indicators.Add(".claude directory");
        
        if (File.Exists(packageJson))
        {
            try
            {
                var packageContent = File.ReadAllText(packageJson);
                if (packageContent.Contains("@anthropic-ai/claude-code"))
                    indicators.Add("package.json with Claude dependency");
            }
            catch { }
        }
        
        if (indicators.Any())
        {
            Console.WriteLine($"  ✓ {slnName}: Claude appears to be initialized");
            foreach (var indicator in indicators)
            {
                Console.WriteLine($"    - {indicator}");
            }
        }
        else
        {
            Console.WriteLine($"  ✗ {slnName}: Claude not initialized");
        }
    }
}