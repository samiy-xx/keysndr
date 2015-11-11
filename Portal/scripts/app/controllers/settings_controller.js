(function () {
    'use strict';
    app.controller('settingsController', ["$scope", "apiService", SettingsController]);

    function SettingsController(scope, service) {
        scope.appSettings = null;

        scope.errorMessage = null;
        scope.message = null;
        scope.hasError = false;

        scope.getAppSettings = function() {
            service.getSettings().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.errorMessage = result.errorMessage;
                    scope.hasError = true;
                    return;
                }
                scope.appSettings = result.content;
                scope.hasError = false;
            });
        };

        scope.saveSettings = function() {
            service.saveSettings(scope.appSettings).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.errorMessage = result.errorMessage;
                    scope.message = result.message;
                    scope.hasError = true;
                    return;
                }
                scope.message = "Settings saved";
                scope.hasError = false;
            });
        };
        scope.getAppSettings();
    }

})();