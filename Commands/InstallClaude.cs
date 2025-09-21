// BHC.InitializedClaude.CLI/Commands/InstallClaude.cs

using System.CommandLine;
using System.Diagnostics;

namespace BHC.InitializedClaude.CLI.Commands;

public static class InstallClaude
{
    public static Command Create()
    {
        var installCommand = new Command("install", "Install Claude globally via npm with optional configuration");
        
        // Add aliases for the main command
        installCommand.AddAlias("setup");
        installCommand.AddAlias("init");
        
        var targetDirectoryArgument = new Argument<string>(
            "target-directory", 
            () => Environment.CurrentDirectory,
            "Target directory to initialize Claude in (defaults to current directory)"
        );
        
        var forceOption = new Option<bool>(
            "--force",
            () => false,
            "Force reinstallation even if Claude is already installed"
        );
        forceOption.AddAlias("-f");
        
        var skipNavigationOption = new Option<bool>(
            "--skip-navigation",
            () => false,
            "Skip navigating to target directory after installation"
        );
        skipNavigationOption.AddAlias("--no-nav");
        
        var autoStartOption = new Option<bool>(
            "--auto-start",
            () => false,
            "Automatically start Claude after installation"
        );
        autoStartOption.AddAlias("-s");
        autoStartOption.AddAlias("--start");
        
        var verboseOption = new Option<bool>(
            "--verbose",
            () => false,
            "Show detailed installation output"
        );
        verboseOption.AddAlias("-v");
        
        var globalOnlyOption = new Option<bool>(
            "--global-only",
            () => false,
            "Only install globally, don't initialize in target directory"
        );
        globalOnlyOption.AddAlias("-g");
        
        installCommand.AddArgument(targetDirectoryArgument);
        installCommand.AddOption(forceOption);
        installCommand.AddOption(skipNavigationOption);
        installCommand.AddOption(autoStartOption);
        installCommand.AddOption(verboseOption);
        installCommand.AddOption(globalOnlyOption);
        
        // Action-based handler
        installCommand.SetHandler((string targetDirectory, bool force, bool skipNavigation, bool autoStart, bool verbose, bool globalOnly) =>
        {
            InstallClaudeAction(targetDirectory, force, skipNavigation, autoStart, verbose, globalOnly);
        }, targetDirectoryArgument, forceOption, skipNavigationOption, autoStartOption, verboseOption, globalOnlyOption);
        
        return installCommand;
    }
    
    private static void InstallClaudeAction(string targetDirectory, bool force, bool skipNavigation, bool autoStart, bool verbose, bool globalOnly)
    {
        try
        {
            Console.WriteLine("Claude Installation and Setup");
            Console.WriteLine("=============================");
            Console.WriteLine();
            
            // Validate target directory
            if (!globalOnly && !Directory.Exists(targetDirectory))
            {
                Console.WriteLine($"Error: Target directory '{targetDirectory}' does not exist.");
                return;
            }
            
            // Check if npm is available
            if (!IsNpmAvailable())
            {
                Console.WriteLine("Error: npm is not installed or not available in PATH.");
                Console.WriteLine("Please install Node.js and npm first.");
                return;
            }
            
            // Check current Claude installation status
            bool claudeInstalled = IsClaudeInstalledGlobally();
            
            if (claudeInstalled && !force)
            {
                Console.WriteLine("✓ Claude is already installed globally.");
                
                if (!globalOnly)
                {
                    Console.WriteLine($"Target directory: {Path.GetFullPath(targetDirectory)}");
                    
                    if (!skipNavigation)
                    {
                        NavigateToDirectory(targetDirectory);
                    }
                    
                    if (autoStart)
                    {
                        StartClaude(targetDirectory);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Run 'claude' to start Claude in this directory.");
                    }
                }
                
                return;
            }
            
            if (claudeInstalled && force)
            {
                Console.WriteLine("Claude is already installed, but --force flag specified. Reinstalling...");
            }
            else
            {
                Console.WriteLine("Claude is not installed globally. Installing now...");
            }
            
            // Install Claude globally
            bool installSuccess = InstallClaudeGlobally(verbose);
            
            if (!installSuccess)
            {
                Console.WriteLine("Failed to install Claude. Please check your npm configuration and try again.");
                return;
            }
            
            Console.WriteLine("✓ Claude installed successfully!");
            
            if (!globalOnly)
            {
                Console.WriteLine();
                Console.WriteLine($"Target directory: {Path.GetFullPath(targetDirectory)}");
                
                if (!skipNavigation)
                {
                    NavigateToDirectory(targetDirectory);
                }
                
                if (autoStart)
                {
                    Console.WriteLine();
                    StartClaude(targetDirectory);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Installation complete!");
                    Console.WriteLine($"Navigate to '{targetDirectory}' and run 'claude' to start.");
                }
            }
            else
            {
                Console.WriteLine("Global installation complete!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: An unexpected error occurred during installation");
            Console.WriteLine($"Details: {ex.Message}");
        }
    }
    
    private static bool IsNpmAvailable()
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            using var process = Process.Start(processInfo);
            if (process == null) return false;
            
            process.WaitForExit();
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
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
    
    private static bool InstallClaudeGlobally(bool verbose)
    {
        try
        {
            Console.WriteLine("Installing @anthropic-ai/claude-code globally...");
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "npm",
                Arguments = "install -g @anthropic-ai/claude-code",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = !verbose
            };
            
            using var process = Process.Start(processInfo);
            if (process == null) return false;
            
            if (verbose)
            {
                // Show real-time output
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine(e.Data);
                };
                
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"Error: {e.Data}");
                };
                
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            
            process.WaitForExit();
            
            if (!verbose && process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                Console.WriteLine($"Installation failed with exit code {process.ExitCode}");
                Console.WriteLine($"Error output: {error}");
            }
            
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during installation: {ex.Message}");
            return false;
        }
    }
    
    private static void NavigateToDirectory(string targetDirectory)
    {
        try
        {
            Console.WriteLine($"Setting working directory to: {Path.GetFullPath(targetDirectory)}");
            Environment.CurrentDirectory = targetDirectory;
            Console.WriteLine("✓ Successfully navigated to target directory");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Failed to navigate to directory: {ex.Message}");
        }
    }
    
    private static void StartClaude(string targetDirectory)
    {
        try
        {
            Console.WriteLine("Starting Claude...");
            Console.WriteLine("Note: You'll be prompted to log in on first use.");
            
            var processInfo = new ProcessStartInfo
            {
                FileName = "claude",
                UseShellExecute = true,
                WorkingDirectory = targetDirectory
            };
            
            Process.Start(processInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start Claude: {ex.Message}");
            Console.WriteLine("You can manually start Claude by running 'claude' in your terminal.");
        }
    }
}