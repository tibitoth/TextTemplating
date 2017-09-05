# TextTemplating

[中文说明](README.CHS.md)

## Goal
This project's goal is to bring the old T4 text templating code generating approach to the new ASP.NET Core 2.0 projects.

### Update 
The Visual Studio 2017 and Xamarin Studio now supports to process *.tt files in desing time, but this repo is maybe still useful who wants to process T4 templates in a dotnet core(netstandard2.0) project outside IDE (eg. in Linux or macOS with Visual Studio Code)

## How to use
### As a command line tool
Add the following to  `YourProject.csproj`.

```xml
<ItemGroup>
    <PackageReference Include="TextTemplating" Version="2.0.0-alpha4" />
</ItemGroup>
<ItemGroup>
    <DotNetCliToolReference Include="TextTemplating.Tools" Version="2.0.0-alpha5" />
</ItemGroup>
```

Now you can use the `dotnet t4` command as a command line tool to transform templates at design-time, with the specified command line arguments   

Run `dotnet t4 -h` to see the usage.

Example:
```Batchfile
dotnet t4 proc -f DbBase.tt
```

### As a design time tool (Not Implemented)
*Work in progres*


### As a library
To transform templates at runtime, you can also use the `Engine` class.

*Sample is work in progres*

### As a service (Not Implemented)
*Work in progres*

# License
MIT
