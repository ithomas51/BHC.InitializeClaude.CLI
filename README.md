# BHC.InitializeClaude.CLI

A command-line tool for managing Claude AI code assistant installation and initialization in .NET projects. This tool streamlines the process of setting up Claude for development environments, particularly for Visual Studio solutions and .NET projects.

## Features

- **Global Claude Installation**: Install Claude AI code assistant globally via npm
- **Project Search**: Find and analyze .NET solution files for Claude initialization status
- **Force Reinstallation**: Override existing installations when needed
- **Flexible Configuration**: Multiple options for customizing installation and startup behavior
- **Cross-Platform Support**: Works on Windows, macOS, and Linux

## Prerequisites

- [Node.js](https://nodejs.org/) (with npm) - Required for Claude installation
- [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) or later - For running the CLI tool

## Installation

### As a Global .NET Tool

```bash
dotnet tool install -g BHC.InitializeClaude.CLI
```

### From Source

```bash
git clone <repository-url>
cd BHC.InitializeClaude.CLI
dotnet pack
dotnet tool install -g --add-source ./bin/Release BHC.InitializeClaude.CLI
```

## Usage

The tool is available as `init-claude` command after installation.

### Install Claude

Install Claude AI globally and optionally initialize it in a target directory:

```bash
# Install Claude in current directory
init-claude install

# Install Claude in specific directory
init-claude install "C:\MyProject"

# Force reinstallation even if already installed
init-claude install --force

# Install globally only (don't initialize in directory)
init-claude install --global-only

# Install and auto-start Claude
init-claude install --auto-start

# Verbose installation output
init-claude install --verbose
```

#### Install Command Options

| Option | Alias | Description |
|--------|-------|-------------|
| `--force` | `-f` | Force reinstallation even if Claude is already installed |
| `--skip-navigation` | `--no-nav` | Skip navigating to target directory after installation |
| `--auto-start` | `-s`, `--start` | Automatically start Claude after installation |
| `--verbose` | `-v` | Show detailed installation output |
| `--global-only` | `-g` | Only install globally, don't initialize in target directory |

#### Install Command Aliases

- `init-claude setup`
- `init-claude init`

### Search for Initialized Projects

Search for .NET solution files and check their Claude initialization status:

```bash
# Search current directory
init-claude search

# Search specific directory
init-claude search "C:\Projects"

# Search without recursion
init-claude search --recursive false

# Skip npm global check
init-claude search --check-npm false
```

#### Search Command Options

| Option | Alias | Description |
|--------|-------|-------------|
| `--recursive` | `-r` | Search subdirectories recursively (default: true) |
| `--check-npm` | `--npm` | Check if Claude is installed via npm globally (default: true) |

#### Search Command Aliases

- `init-claude find`
- `init-claude lookup`

## Examples

### Basic Setup

```bash
# Install Claude and set up in current directory
init-claude install

# Search for .NET solutions in current directory tree
init-claude search
```

### Advanced Setup

```bash
# Force reinstall Claude with verbose output and auto-start
init-claude install --force --verbose --auto-start

# Search specific project directory without checking npm
init-claude find "C:\MyProjects" --check-npm false
```

### Development Workflow

```bash
# Check what's already initialized
init-claude search --recursive

# Install Claude for a new project
init-claude install "C:\NewProject" --auto-start

# Force update existing installation
init-claude setup --force --verbose
```

## What the Tool Checks

When searching for initialized projects, the tool looks for:

- `.claude_config` files
- `.claude` directories  
- `package.json` files with Claude dependencies
- Global npm installation of `@anthropic-ai/claude-code`

## PowerShell Script Integration

The tool includes a PowerShell template script (`scripts/powershell-init-claude-template.ps1`) that can be used for manual installation and setup workflows.

## Error Handling

The tool provides comprehensive error handling for common scenarios:

- Missing Node.js/npm installation
- Invalid target directories
- Network issues during npm installation
- Permission problems
- Existing installation conflicts

## Development

### Building from Source

```bash
dotnet restore
dotnet build
dotnet pack
```

### Running Tests

```bash
dotnet test
```

### Local Development

```bash
# Run directly with dotnet
dotnet run -- install --help

# Install as local tool
dotnet tool install --local --add-source ./bin/Release BHC.InitializeClaude.CLI
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is part of the BHC.Commands suite. See LICENSE file for details.

## Support

For issues, questions, or contributions, please use the GitHub repository's issue tracker.

## Version History

- **1.0.0** - Initial release with install and search commands
  - Global Claude installation via npm
  - .NET solution file discovery and analysis
  - Force reinstallation options
  - Comprehensive command-line interface
