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
