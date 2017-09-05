# TextTemplating
## 目标
这个项目致力于把原先的 T4 模板带到全新的 ASP.NET Core 世界。

### Update 
到目前为之，微软手下的 IDE：Visual Studio 2017、Xamarin Studio 均对 T4 模板有较好的支持，但是在持续集成环境以及 Linux 下，这个项目还是非常有用的。你可以完全的脱离 IDE 来使用 T4 模板。

## 快速上手
### 使用命令行工具
把下面的片段加入到  `YourProject.csproj`.

```xml
<ItemGroup>
    <PackageReference Include="TextTemplating" Version="2.0.0-alpha4" />
</ItemGroup>
<ItemGroup>
    <DotNetCliToolReference Include="TextTemplating.Tools" Version="2.0.0-alpha5" />
</ItemGroup>
```

现在，你就可以使用 `dotnet t4` 这个命令行工具来转换 T4 模板了，使用 `dotnet t4 -h` 这个命令来查看目前可用的操作。


举个例子:
```Batchfile
dotnet t4 proc -f DbBase.tt
```

### 使用设计时工具 (尚未实现)
*Work in progres*


### 引用模板引擎库
你可以使用 `Engine` 类来做运行时的转换。

*Sample is work in progres*

### 作为一个服务 (尚未实现)
*Work in progres*

# License
MIT
