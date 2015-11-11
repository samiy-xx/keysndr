(function () {
    'use strict';
    app.controller('playGridController', ["$scope", "apiService", "$location", PlayGridController]);

    function PlayGridController(scope, service, location) {

        function init() {
            var s = $location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("name")) {
                //s.id
            }
        }
    }

})();