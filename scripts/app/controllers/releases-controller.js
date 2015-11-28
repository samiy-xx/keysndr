(function () {
    'use strict';

    app.controller('releasesController', ["$scope", "$location", "apiService",  ReleasesController]);

    function ReleasesController(scope, $location, service) {
        scope.releases = null;
        scope.currentRelease = null;
        
        scope.init = function() {
            service.getReleases().then(function(response) {
                scope.releases = response.data;
                
            });
        } 
        
        scope.init();
    }
})(app);