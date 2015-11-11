(function () {
    'use strict';
    app.controller('editGridController', ["$scope", "apiService", "$location", EditGridController]);

    function EditGridController(scope, service, location) {

        function init() {
            var s = $location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("name")) {
                //s.id
            }
        }
    }

})();