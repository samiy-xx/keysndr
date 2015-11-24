(function () {
    'use strict';
    app.directive('blinkElement', BlinkElement);

    function BlinkElement() {
        function link(scope, element, attributes) {
            var ms = attributes.blinkElement;
            $(element).click(function () {
                var originalColor = $(element).css('backgroundColor');
                $(this).animate({
                    backgroundColor: "rgba(81, 203, 238, 1)"
                }, 100, function() {
                    $(this).animate({
                        backgroundColor: originalColor
                    }, 100);
                });
            });
        }

        return {
            link: link
        }
    }

})();