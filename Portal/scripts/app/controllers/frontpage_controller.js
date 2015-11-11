(function () {
    'use strict';
    app.controller('frontPageController', ["$scope", "apiService", FrontPageController]);

    function FrontPageController(scope, service) {
        scope.availableViewConfigurations = null;
        scope.availableLegacyConfigurations = null;

        scope.errorMessage = null;
        scope.message = null;
        scope.hasError = false;

        scope.getLegacyConfigurations = function() {
            service.getLegacyConfigurations().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.errorMessage = result.errorMessage;
                    scope.hasError = true;
                    return;
                }
                scope.hasError = false;
                scope.availableLegacyConfigurations = result.content;
            });
        }

        scope.getViewConfigurations = function() {
            service.getViewConfigurations().then(function (response) {
                var result = response.data;
                if (!result.success) {
                    scope.errorMessage = result.errorMessage;
                    scope.hasError = true;
                    return;
                }
                scope.hasError = false;
                scope.availableViewConfigurations = result.content;
            });
        }

        scope.removeGrid = function(index) {
            scope.availableLegacyConfigurations.splice(index, 1);
            
        }

        scope.editGrid = function (index) {
            var name = scope.availableLegacyConfigurations[index];
            window.location.href = "edit-grid.html?name=" + name;
        }

        scope.openGrid = function (index) {
            var name = scope.availableLegacyConfigurations[index];
            window.location.href = "play-grid.html?name=" + name;
        }

        scope.removeView = function (index) {
            scope.availableViewConfigurations.splice(index, 1);
        }

        scope.editView = function (index) {
            var name = scope.availableViewConfigurations[index];
            window.location.href = "edit-view.html?name=" + name;
        }

        scope.openView = function (index) {
            var name = scope.availableViewConfigurations[index];
            service.getConfiguration(name).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    return;
                }
                var url = result.content.view;
                window.location.href = "/" + url + "/index.html";
            });
            
        }
        scope.getLegacyConfigurations();
        scope.getViewConfigurations();
    }

})();