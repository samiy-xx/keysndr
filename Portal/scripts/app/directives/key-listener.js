(function () {
    'use strict';
    app.directive('keyListener', keyListener);

    function keyListener() {
        function link(scope, element, attributes) {
            var index = parseInt(scope.$eval(attributes.keyListener));
            var input = angular.element(element[0].querySelector('.form-control'))[0];
            input.onkeydown = function(e) {
                var charCode = (e.which) ? e.which : event.keyCode;
                if (charCode >= 16 && charCode <= 18)
                    scope.setIntModifierForSequence(index, charCode);
                else {
                    scope.setKey(index, charCode);
                }
                return false;
            }

            
            /*$(input).keydown(function(e) {
                var charCode = (e.which) ? e.which : event.keyCode;
                alert(charCode);
                //if (charCode >= 16 && charCode <= 18)
                //    scope.setIntModifierForSequence(index, charCode);
                e.preventDefault();
                return false;
            });*/
        }

        return {
            link: link
        }
    }

})();