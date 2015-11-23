app.directive('onReadFile', function ($parse) {
    return {
        restrict: 'A',
        scope: false,
        link: function (scope, element, attrs) {
            var fn = $parse(attrs.onReadFile);

            element.on('change', function (onChangeEvent) {
                //var reader = new FileReader();

                //reader.onload = function (onLoadEvent) {
                //    scope.$apply(function () {
                //        fn(scope, { $fileContent: onLoadEvent.target.result });
                //    });
                //};
                scope.dataFromFile((onChangeEvent.srcElement || onChangeEvent.target).files[0]);
                //reader.readAsArrayBuffer((onChangeEvent.srcElement || onChangeEvent.target).files[0]);
            });
        }
    };
});
