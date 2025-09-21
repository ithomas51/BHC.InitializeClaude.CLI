// BHC.InitializedClaude.CLI/Program.cs
using System.CommandLine;
using BHC.InitializedClaude.CLI.Commands;

namespace BHC.InitializedClaude.CLI;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("BHC Initialize Claude CLI Tool - Manage Claude initialization in .NET projects");
        
        // Register all commands using Action-based handlers
        RegisterCommands(rootCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
    
    private static void RegisterCommands(RootCommand rootCommand)
    {
        // Register each command with its Action handler
        var searchCommand = SearchInitialized.Create();
        var installCommand = InstallClaude.Create();
        
        rootCommand.AddCommand(searchCommand);
        rootCommand.AddCommand(installCommand);
    }
}