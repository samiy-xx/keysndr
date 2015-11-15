(function () {
    'use strict';
    app.directive('bindConfiguration', bindConfiguration);

    function bindConfiguration() {
        function link(scope, element, attributes) {
            var configuration = attributes.bindConfiguration;
            scope.loadConfiguration(configuration);
            for (var i = 0; i < element[0].children.length; i++) {
                var child = element[0].children[i];
                child.onclick = function () {
                    var dataValue = this.dataset.action;
                    if (dataValue !== undefined)
                        scope.execute(dataValue);
                }
            }
        }

        return {
            link: link
        }
    }

})();