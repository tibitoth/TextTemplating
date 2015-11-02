# OUTDATED, UPDATE IN PROGRESS

# TextTemplating
T4 for ASP.NET 5

To use, add the following to your `project.json`.

```JSON
{
    "dependencies": {
        "Bricelam.TextTemplating": "1.0.0-*"
    },
    "commands": {
        "t4": "Bricelam.TextTemplating"
    }
}
```

To transform templates at design-time, use the following command.

```Batchfile
k t4
```

By default, all text template files under the project directory will be transformed. You can specify explicit file
patterns.

```Batchfile
k t4 Changelog.tt **\GeneratedCode\*.tt
```
To create a runtime text template, use the corresponding command line option.

```Batchfile
k t4 --preprocess
```
To transform templates at runtime, you can also use the `Engine` class.

```C#
var engine = new Engine();
var host = MyCustomEngineHost();
var output = engine.ProcessTemplate(inputTemplate, host);
```
