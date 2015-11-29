(function () {
    'use strict';

    app.controller('newsController', ["$scope", "$location", "apiService",  NewsController]);

    function NewsController(scope, $location, service) {
        scope.feed = null;
        
        
        scope.init = function() {
            service.getFeed().then(function(response) {
                scope.feed = response.data;
            });
        } 
        
        scope.init();
    }
})(app);