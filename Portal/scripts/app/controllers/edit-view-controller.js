(function () {
    'use strict';
    app.controller('editViewController', ["$scope", "apiService", "$location", EditViewController]);

    function EditViewController(scope, service, location) {

        function init() {
            var s = $location.search();
            if (s !== null && s !== undefined && s.hasOwnProperty("name")) {
                //s.id
            }
        }
    }

})();