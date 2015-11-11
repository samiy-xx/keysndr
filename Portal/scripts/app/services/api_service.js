app.factory('apiService', function ($http) {
    var rootUrl = "http://192.168.0.112:45889/api/";

    return {
        getAllConfigurations: function () {
            return $http.get(rootUrl + 'action/getallconfigurations');
        },

        getLegacyConfigurations: function() {
            return $http.get(rootUrl + 'action/getlegacyconfigurations');
        },

        getViewConfigurations: function() {
            return $http.get(rootUrl + 'action/getviewconfigurations');
        },

        getConfiguration: function (name) {
            return $http.get(rootUrl + 'action/getconfiguration?name='+ name);
        },

        getSettings: function() {
            return $http.get(rootUrl + 'settings/getappsettings');
        },

        saveSettings: function(settings) {
            return $http.post(rootUrl + 'settings/storeappsettings', settings);
        }
    }
});
