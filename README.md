# BHC.InitializeClaude.CLI



A .NET global tool for managing Claude initialization in .NET projects.# Install Instructions



## Installation## Local Tool Installation



Install globally as a dotnet tool:1. Build and pack the tool:

   ```sh

```bash   dotnet pack -c Release

dotnet tool install --global BHC.InitializeClaude.CLI   ```

```2. Copy the generated `.nupkg` file from `bin/Release` to your local NuGet feed directory (e.g., `C:\.bhc-nuget-feed`).

3. Install the tool globally from your local feed:

## Usage   ```sh

   dotnet tool install --global BHC.EnvironmentVars.CLI --add-source C:\.bhc-nuget-feed

### Commands   ```

4. Run the tool using:

The tool provides two main commands:   ```sh

   envvars [command] [options]

#### `search` - Search for .sln files and check Claude initialization status   ```



```bash## Project Structure

# Search current directory for .sln files and check Claude status

init-claude search```

BHC.EnvironmentVars.CLI/

# Search specific directory│   BHC.EnvironmentVars.CLI.csproj

init-claude search C:\MyProjects│   Program.cs

│   README.md

# Search without recursion└── Commands/

init-claude search --recursive false    │   ExportEnvironVariables.cs

    │   GetEnvironVariables.cs

# Skip npm Claude check    └── SearchEnvironVariables.cs

init-claude search --check-npm false```

```

## Features

**Options:**

- `directory` - Directory to search (defaults to current directory)- **Export Variables**: Export environment variables to CSV file with customizable output path

- `--recursive`, `-r` - Search subdirectories recursively (default: true)- **List Variables**: Print all environment variables to console in multiple formats

- `--check-npm`, `--npm` - Check if Claude is installed via npm globally (default: true)- **Search Variables**: Find environment variables by partial name matching (case-insensitive)

- **Action-based Handlers**: Each command uses Action delegates for clean separation

**Aliases:** `find`, `lookup`- **Command Aliases**: Multiple aliases for improved user experience

- **Flexible Output**: Support for table, list, and JSON formats

#### `install` - Install Claude globally and optionally initialize in target directory

## Prerequisites

```bash

# Install Claude globally and initialize in current directory- .NET 9 SDK

init-claude install- System.CommandLine package (included in project)



# Install and initialize in specific directory## Building the Application

init-claude install C:\MyProject

```bash

# Force reinstallationcd BHC.EnvironmentVars.CLI

init-claude install --forcedotnet build

```

# Install globally only (don't initialize in directory)

init-claude install --global-only## Running the Application



# Install and auto-start Claude### Development

init-claude install --auto-start

```bash

# Show verbose installation outputdotnet run -- [command] [options]

init-claude install --verbose```

```

### Published Binary

**Options:**

- `target-directory` - Target directory to initialize Claude in (defaults to current directory)```bash

- `--force`, `-f` - Force reinstallation even if Claude is already installeddotnet publish -c Release

- `--skip-navigation`, `--no-nav` - Skip navigating to target directory after installation./bin/Release/net9.0/envvars [command] [options]

- `--auto-start`, `-s`, `--start` - Automatically start Claude after installation```

- `--verbose`, `-v` - Show detailed installation output

- `--global-only`, `-g` - Only install globally, don't initialize in target directory## Usage Examples



**Aliases:** `setup`, `init`### Export Commands



## Features```bash

# Export to default location (c:\temp\exported-vars.csv)

- **Solution Detection**: Automatically finds all .sln files in specified directoriesenvvars export

- **Claude Status Check**: Verifies if Claude is installed globally via npmenvvars exp                           # Using alias

- **Initialization Detection**: Checks for Claude initialization indicators in solution directories

- **Automated Installation**: Installs Claude globally via npm with customizable options# Export to custom file

- **Cross-platform**: Works on Windows, macOS, and Linuxenvvars export --output "c:\data\my-vars.csv"

envvars export -o "c:\data\my-vars.csv"

## Prerequisites

# Export only user variables (exclude system)

- .NET 9.0 or laterenvvars export --include-system false

- Node.js and npm (for Claude installation)

# Export unsorted

## Examplesenvvars export --sort false

```

### Check Claude status in current directory

```bash### Get/List Commands

init-claude search

``````bash

# List all environment variables (table format)

### Install Claude and start it automaticallyenvvars get

```bashenvvars list                          # Using alias

init-claude install --auto-startenvvars show                          # Using alias

```

# List in different formats

### Force reinstall Claude with verbose outputenvvars get --format table           # Default table format

```bashenvvars get --format list            # Simple Name=Value format

init-claude install --force --verboseenvvars get --format json            # JSON format

```

# List with filtering

### Search for solutions recursively in a specific directoryenvvars get --filter "PATH"          # Show only variables containing "PATH"

```bashenvvars get -p "TEMP"                # Using alias

init-claude search C:\Projects --recursive

```# List only user variables

envvars get --include-system false

## Output Examples

# List unsorted

### Search Outputenvvars get --sort false

``````

Searching for .sln files in: C:\MyProject

Recursive search: true### Search Commands



Found 2 solution file(s):```bash

  MyProject.sln# Search for variables containing "meraki" (case-insensitive)

  SubProject\SubProject.slnenvvars search meraki

envvars find meraki                   # Using alias

Checking if Claude is installed globally via npm...envvars lookup meraki                 # Using alias

✓ Claude is installed globally via npm

# Expected output if MERAKI_API_KEY exists:

Checking Claude initialization status for each solution:# MERAKI_API_KEY=your_api_key_value

  ✓ MyProject: Claude appears to be initialized

    - .claude_config file# Exact match search

    - package.json with Claude dependencyenvvars search "PATH" --exact

  ✗ SubProject: Claude not initializedenvvars search "PATH" -e

```

# Case-sensitive search

### Install Outputenvvars search "Path" --case-sensitive

```envvars search "Path" -c

Claude Installation and Setup

=============================# Limit results

envvars search "TEMP" --limit 5

Claude is not installed globally. Installing now...envvars search "TEMP" -l 5

Installing @anthropic-ai/claude-code globally...

✓ Claude installed successfully!# Search only user variables

envvars search "MY" --include-system false

Target directory: C:\MyProject```

Setting working directory to: C:\MyProject

✓ Successfully navigated to target directory## Command Aliases



Installation complete!### Main Commands

Navigate to 'C:\MyProject' and run 'claude' to start.

```- `export` → `exp`, `save`

- `get` → `list`, `show`, `print`

## Error Handling- `search` → `find`, `lookup`, `query`



The tool provides comprehensive error handling and user-friendly messages for common scenarios:### Options



- Missing npm/Node.js installation- `--output` → `-o`, `--file`, `-f`

- Invalid directory paths- `--sort` → `-s`

- Network issues during installation- `--include-system` → `--sys`

- Permission problems- `--format` → `-f`

- Missing solution files- `--filter` → `--pattern`, `-p`

- `--exact` → `-e`, `--exact-match`

## Development- `--case-sensitive` → `-c`, `--case`

- `--limit` → `-l`, `--max`

This tool is part of the BHC.Commands suite and follows the same architectural patterns as other BHC CLI tools.

## Output Formats

### Building from Source

### Table Format (Default)

```bash

git clone <repository>```

cd BHC.EnvironmentVars.CLI/src/BHC.InitializeClaude.CLIName                          | Value

dotnet build------------------------------|----------------------------------

dotnet packCOMPUTERNAME                  | DESKTOP-ABC123

dotnet tool install --global --add-source ./bin/Debug BHC.InitializeClaude.CLIPATH                          | C:\Windows\System32;C:\Program...

```TEMP                          | C:\Users\Username\AppData\Local\Temp

```

## License

### List Format

[Add your license information here]
```
COMPUTERNAME=DESKTOP-ABC123
PATH=C:\Windows\System32;C:\Program Files;...
TEMP=C:\Users\Username\AppData\Local\Temp
```

### JSON Format

```json
{
  "COMPUTERNAME": "DESKTOP-ABC123",
  "PATH": "C:\\Windows\\System32;C:\\Program Files;...",
  "TEMP": "C:\\Users\\Username\\AppData\\Local\\Temp"
}
```

## Search Examples

### Partial Matching (Default)

```bash
# Find variables containing "java"
envvars search java

# Possible matches:
# JAVA_HOME=C:\Program Files\Java\jdk-17
# JAVA_TOOL_OPTIONS=-Xmx2g
```

### Exact Matching

```bash
# Find variable with exact name "PATH"
envvars search PATH --exact

# Output:
# PATH=C:\Windows\System32;C:\Program Files;...
```

### Case Sensitivity

```bash
# Case-insensitive (default) - finds "PATH", "Path", "path"
envvars search path

# Case-sensitive - finds only exact case
envvars search path --case-sensitive
```

## Export Features

- **Automatic Directory Creation**: Creates output directory if it doesn't exist
- **CSV Escaping**: Properly handles values containing commas, quotes, and newlines
- **UTF-8 Encoding**: Supports international characters
- **Error Handling**: Comprehensive error handling for file permissions and I/O issues

## Architecture Notes

- **Action-based Handlers**: Each command uses Action delegates for clean separation of concerns
- **Modular Design**: Each command is in its own file with self-contained logic
- **Alias Support**: Commands and options support multiple aliases for better UX
- **Extensible**: Easy to add new commands by following the established pattern
- **Error Handling**: Robust error handling for file operations and environment access

## Adding New Commands

1. Create a new command file in the `Commands/` folder
2. Implement the static `Create()` method that returns a `Command`
3. Add aliases within the command file
4. Use Action-based handlers for command logic
5. Register the command in `Program.cs`

## Security Considerations

- Environment variables may contain sensitive information (API keys, passwords)
- Use caution when exporting to files or sharing output
- Consider filtering sensitive variables before export
- Be aware that console output may be logged or visible to other users

## Default Behavior

- **Export**: Saves to `c:\temp\exported-vars.csv` with all variables, sorted alphabetically
- **Get**: Shows all variables in table format, sorted alphabetically
- **Search**: Case-insensitive partial matching, sorted results, max 50 results

---


# Developer Notes

<details><summary>Click to expand</summary>

Ran terminal command: dotnet pack -c Release

Perfect! The project builds and packs successfully. Let me create a summary of what I've accomplished:

## Summary

I have successfully completed the transformation of the `BHC.InitializeClaude.CLI` project based on the PowerShell script template and following the same patterns as the `BHC.EnvironmentVars.CLI` project. Here's what was implemented:

### ✅ Completed Tasks:

1. **Fixed Project Structure**: 
   - Corrected namespace and class names from `SearchEnvironVariables` to `SearchInitialized`
   - Updated Program.cs to reference the correct command classes
   - Removed references to non-existent commands

2. **Implemented SearchInitialized Command**:
   - Searches for `.sln` files in specified directories (recursive by default)
   - Checks if Claude is installed globally via npm
   - Detects Claude initialization indicators in solution directories
   - Provides comprehensive status reporting

3. **Created InstallClaude Command**:
   - Installs Claude globally via npm with various configuration options
   - Supports force reinstallation, verbose output, auto-start
   - Handles directory navigation and error scenarios
   - Implements the core functionality from the PowerShell script

4. **Enhanced User Experience**:
   - Added command aliases (`search`/`find`/`lookup`, `install`/`setup`/`init`)
   - Comprehensive help documentation
   - Rich command-line options with short and long forms
   - Clear, user-friendly output formatting

### 🔧 Key Features Implemented:

- **Solution Detection**: Automatically finds all `.sln` files
- **Claude Status Checking**: Verifies npm global installation status
- **Initialization Detection**: Checks for `.claude_config`, `.claude` directory, and `package.json` dependencies
- **Automated Installation**: Full npm-based Claude installation with error handling
- **Cross-platform Support**: Works on Windows, macOS, and Linux
- **Robust Error Handling**: Comprehensive validation and user-friendly error messages

### 📋 Tool Commands:

1. **`init-claude search [directory]`**: 
   - Search for solution files and check Claude status
   - Options: `--recursive`, `--check-npm`

2. **`init-claude install [target-directory]`**: 
   - Install Claude globally and initialize in target directory
   - Options: `--force`, `--verbose`, `--auto-start`, `--global-only`, `--skip-navigation`

### ✅ Testing Results:

- Project builds successfully with `dotnet build`
- Project packs correctly with `dotnet pack`
- CLI help system works properly
- Search functionality correctly identifies solution files
- Commands are properly registered and accessible

The `BHC.InitializeClaude.CLI` tool is now complete and ready for use as a global .NET tool with the command name `init-claude`. It follows the same architectural patterns as the existing `BHC.EnvironmentVars.CLI` tool while providing Claude-specific functionality for managing Claude installations and project initialization.

</details>
