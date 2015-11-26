(function () {
    'use strict';
    app.controller('settingsController', ["$scope", "apiService", SettingsController]);

    function SettingsController(scope, service) {
        scope.appSettings = null;

        scope.getAppSettings = function() {
            service.getSettings().then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to load settings", result.errorMessage, 5000);
                    return;
                }
                scope.appSettings = result.content;
            });
        };

        scope.saveSettings = function() {
            service.saveSettings(scope.appSettings).then(function(response) {
                var result = response.data;
                if (!result.success) {
                    scope.displayErrorMessage("Failed to save settings", result.errorMessage, 5000);
                    return;
                }
                scope.displaySuccessMessage("Settings saved", result.errorMessage, 3000);
            });
        };
        scope.getAppSettings();
    }

})();