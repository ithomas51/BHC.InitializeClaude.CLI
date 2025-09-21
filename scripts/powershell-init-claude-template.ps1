# PowerShell Script for Installing and Running Claude Code
# Description: Checks if Claude code is installed globally, installs if needed, 
# then navigates to target directory and starts Claude

param(
    [Parameter(Mandatory=$true)]
    [string]$TargetDirectory
)

# Function to check if Claude is installed globally
function Test-ClaudeInstalled {
    try {
        $claudeVersion = npm list -g @anthropic-ai/claude-code --depth=0 2>$null
        if ($claudeVersion -match "@anthropic-ai/claude-code") {
            return $true
        }
        return $false
    }
    catch {
        return $false
    }
}

# Function to check if npm is available
function Test-NpmAvailable {
    try {
        npm --version | Out-Null
        return $true
    }
    catch {
        return $false
    }
}

# Main script execution
Write-Host "Claude Code Installation and Setup Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan

# Check if npm is available
if (-not (Test-NpmAvailable)) {
    Write-Error "npm is not installed or not available in PATH. Please install Node.js and npm first."
    exit 1
}

# Check if target directory exists
if (-not (Test-Path $TargetDirectory)) {
    Write-Error "Target directory '$TargetDirectory' does not exist."
    exit 1
}

# Check if Claude is already installed
Write-Host "Checking if Claude code is installed..." -ForegroundColor Yellow

if (Test-ClaudeInstalled) {
    Write-Host "✓ Claude code is already installed globally." -ForegroundColor Green
} else {
    Write-Host "✗ Claude code is not installed. Installing now..." -ForegroundColor Yellow
    
    try {
        Write-Host "Installing @anthropic-ai/claude-code globally..." -ForegroundColor Blue
        npm install -g @anthropic-ai/claude-code
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Claude code installed successfully!" -ForegroundColor Green
        } else {
            Write-Error "Failed to install Claude code. Exit code: $LASTEXITCODE"
            exit 1
        }
    }
    catch {
        Write-Error "An error occurred during installation: $_"
        exit 1
    }
}

# Navigate to target directory
Write-Host "Navigating to target directory: $TargetDirectory" -ForegroundColor Blue
try {
    Set-Location $TargetDirectory
    Write-Host "✓ Successfully navigated to: $(Get-Location)" -ForegroundColor Green
}
catch {
    Write-Error "Failed to navigate to directory: $_"
    exit 1
}

# Start Claude
Write-Host "Starting Claude code..." -ForegroundColor Blue
Write-Host "Note: You'll be prompted to log in on first use." -ForegroundColor Gray

try {
    claude
}
catch {
    Write-Error "Failed to start Claude: $_"
    exit 1
}