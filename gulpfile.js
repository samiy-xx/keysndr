var gulp = require('gulp');
var connect = require('gulp-connect');

gulp.task('build', function () {
	connect.server();
});
 
gulp.task('default', ['build']);