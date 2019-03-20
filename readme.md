![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/TerWoord.CSharp8Support.svg)

# C# 8 support for .NET. 

Improvements, including documentation, are welcome.

Initial focus is on making `await foreach` and `async using` working.

## Getting started

In your .NET 4.5.2 or .NET 4.6 (4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2 have not yet been tested), add a reference to the nuget package `TerWoord.CSharp8Support`. Then set the C# language level support to 8.0. If you don't have the .NET core 3 sdk installed, you will also need to install the current preview of the `Microsoft.Net.Compilers` package, so make sure you have the preview compiler.
