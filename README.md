<div align="center">

# Environment Variable Explorer
##### Manage Environment Variables Easily Across Platforms

<img alt="Environment Variable Explorer logo" height="280" src="/assets/environment-variable-explorer-logo.png" />

![Build Status](https://img.shields.io/github/actions/workflow/status/Dwarf1er/environment-variable-explorer/publish.yml?branch=master&style=for-the-badge)
![License](https://img.shields.io/github/license/Dwarf1er/environment-variable-explorer?style=for-the-badge)
![Version](https://img.shields.io/github/v/release/Dwarf1er/environment-variable-explorer?style=for-the-badge)
![Issues](https://img.shields.io/github/issues/Dwarf1er/environment-variable-explorer?style=for-the-badge)
![PRs](https://img.shields.io/github/issues-pr/Dwarf1er/environment-variable-explorer?style=for-the-badge)
![Contributors](https://img.shields.io/github/contributors/Dwarf1er/environment-variable-explorer?style=for-the-badge)
![Stars](https://img.shields.io/github/stars/Dwarf1er/environment-variable-explorer?style=for-the-badge)

# Demo

![](/assets/environment-variable-explorer-demo.gif)

</div>

# Project Description

Environment Variable Explorer is a C# application designed to view, edit, add, and delete environment variables on your system, with support for different environment variable scopes (Process, User, Machine). It provides a clean and intuitive UI to help developers and sysadmins:

- Easily browse environment variables by target (Process, User, Machine).
- Edit values safely with undo/redo functionality.
- Add and delete variables, with platform-aware restrictions.

This tool is ideal for developers, administrators, and power users who want a simple way to manage environment variables without relying on command-line tools or registry editors. Whether debugging application configs or adjusting system-wide environment settings, Environment Variable Explorer streamlines the process.

If you find this tool helpful, please star the repo! ⭐

# Why Environment Variable Explorer?

No more hunting through OS settings or remembering complex commands! This app offers:

- ✅ Cross-platform support with platform-specific restrictions.
- ✅ Clear separation of environment variable targets.
- ✅ Safe editing with inline validation
- ✅ Quick access to environment variables for troubleshooting and configuration.

# Table of Contents

- [Environment Variable Explorer](#environment-variable-explorer)
        - [Manage Environment Variables Easily Across Platforms](#manage-environment-variables-easily-across-platforms)
- [Project Description](#project-description)
- [Why Environment Variable Explorer?](#why-environment-variable-explorer)
- [Table of Contents](#table-of-contents)
  - [Features](#features)
    - [Core Functionality](#core-functionality)
    - [Platform Support](#platform-support)
  - [Installation](#installation)
    - [Option 1: Download Pre-built Binary](#option-1-download-pre-built-binary)
    - [Option 2: Build from source](#option-2-build-from-source)
  - [Quick Start](#quick-start)
  - [Troubleshooting](#troubleshooting)
    - [Common Issues](#common-issues)
  - [Get Involved](#get-involved)
- [License](#license)

## Features

### Core Functionality
- 🔍 **Smart Filtering** - Browse variables by scope (Process/User/Machine)
- ✏️ **Inline Editing** - Click to edit with real-time validation
- ➕ **Easy Addition** - Add new variables with guided input
- 🗑️ **Safe Deletion** - Remove variables with confirmation prompts

### Platform Support
- 🪟 **Windows** - Full support for all variable scopes
- 🐧 **Linux/macOS** - Process-level variables only
- ⚡ **Cross-platform** - Built with .NET for maximum compatibility

## Installation

### Option 1: Download Pre-built Binary

1. Go to [Releases](https://github.com/Dwarf1er/environment-variable-explorer/releases)
2. Download the latest version for your platform (Windows/Linux/macOS)
3. Extract and run the executable

### Option 2: Build from source

```bash
git clone https://github.com/Dwarf1er/environment-variable-explorer.git
cd environment-variable-explorer
publish.sh # .\publish.ps1 on Windows
cd environment-variable-explorer/EnvironmentVariableExplorer/bin/Release/net9.0/{your-platform}/publish/
# run EnvironmentVariableExplorer binary
```

## Quick Start

1. **Launch** the application
2. **Select scope** - Choose between Process, User, or Machine variables
3. **Browse** existing variables in the table
4. **Add new** variables using the "Add Variable" button
5. **Edit inline** by clicking any value cell
6. **Delete** by selecting a row and clicking the delete button

> **💡 Tip:** On non-Windows platforms, only Process-level variables can be modified

## Troubleshooting

### Common Issues

**"Access Denied" when editing User/Machine variables**
- Run as Administrator on Windows
- Machine scopes require elevated permissions

**Variables not appearing**
- Restart the application to refresh the view
- Check if variables exist in the correct scope

## Get Involved

- 💡 Have ideas or feature requests? Open an issue or start a discussion!
- 🔧 Want to contribute? Fork the repo, submit PRs, and help improve the tool!
- 📢 Using Environment Variable Explorer? Let us know! Share your use case, report issues, or give feedback.
- 🚀 Spread the word! ⭐ Star the repo and share with others!

# License

This software is licensed under the [MIT license](LICENSE)