(function () {
    'use strict';
    app.directive('blinkElement', BlinkElement);

    function BlinkElement() {
        function link(scope, element, attributes) {
            var ms = attributes.blinkElement;
            var originalColor = $(element).css('backgroundColor');

            $(element).click(function() {
                $(this).animate({
                    backgroundColor: "#aa0000"
                }, 200, function() {
                    $(this).animate({
                        backgroundColor: originalColor
                    }, 200);
                });
            });
        }

        return {
            link: link
        }
    }

})();