(function () {
    'use strict';
    app.directive('fileUpload', FileUpload);

    function FileUpload() {
        function link(scope, element, attributes) {
            $(element).append("<input type='file' style='display:none'>");
            var fButton = $(element).find(">:first-child");
            
            fButton.onClick = function(e) {
                scope.upload(e.files[0]);
            }

            $(element).click(function () {
                fButton.click();
            });
        }

        return {
            link: link
        }
    }

})();