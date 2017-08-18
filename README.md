# TextTemplating
## Goal
This project's goal is to bring the old T4 text templating and Razor code generating approach to the new ASP.NET 5 projects.

### Update 
The Visual Studio 2015 Update 1 now supports to process *.tt files in desing time, but this repo is maybe still useful who wants to process T4 templates in a dnx project outside visual studio (eg. in Mac with Visual Studio Code)

## How to use
### As a command line tool
Add the following to your `project.json`.

```JSON
{
    "dependencies": {
        "TextTemplating": "1.0.0-rc1-final"
    },
    "commands": {
        "tt": "TextTemplating"
    }
}
```

Now you can use the `tt` command as a command line tool to transform templates at design-time, with the specified command line arguments   

- `--t4-template` or `-t4` flag: Enable to process t4 template files (*.tt) and generate outputs.
- `--razor` or `-r` flag: Enable to process razor files (*.cshtml) and generate outputs.
- `--preprocess` or `-p` flag: If you use this with `-t4` flag the output will be a runtime text template.
- `--dir` or `-d` optional single value argument: Specify the working directory. The default is the project's root folder, and the processing is recursive in the current working directory.

Example:
```Batchfile
dnx tt --t4-template --razor --dir ./Templates
```

### As a design time tool
You can also bring the old CTRL + S behavior for the template files with Gulp, so you can generate code immediately if you save the file.

Add the following commands to the `project.json`:

```JSON
{
    "dependencies": {
        "TextTemplating": "1.0.0-rc1-final"
    },
    "commands": {
        "t4": "TextTemplating --t4-template --dir ./Templates",
        "razor": "TextTemplating --razor --dir ./Templates"
    }
}
```

Add the following packages to the project's Node package manager config file. (`package.json`)

```JSON
"dateformat": "~1.0.11",
"gulp": "~3.9.0",
"gulp-dnx": "git+https://github.com/totht91/gulp-dnx.git#dev"
```
With the `gulp-dnx` you can execute a dnx command as a gulp task, so we can create `gulp.watch` tasks for the template files. This is also my forked repo what can you find there: https://github.com/totht91/gulp-dnx

Add the following to the project's `gulpfile.js`:
```JavaScript
/// <binding ProjectOpened='default' />

var gulp = require('gulp');
dnx = require("gulp-dnx");

gulp.task('default', ['watch']);

var options = {
    restore: false
};

gulp.task('t4', dnx('t4', options));
gulp.task('razor', dnx('razor', options));

gulp.task('watch', function (cb) {
    gulp.watch('./Templates/*.tt', ['t4']);
    gulp.watch('./Templates/*.cshtml', ['razor']);
});
```

After that the template files will be automatically processed in save. (also in visual studio) 


### As a library
To transform templates at runtime, you can also use the `Engine` class.

*Sample is work in progres*

### As a service
*Work in progres*

# License
MIT
