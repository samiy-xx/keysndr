var app = angular.module("keysndr", []);

app.config(['$locationProvider',
    function ($locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
]);

app.factory('serverAddressService', ["$location", function ($location) {
    var url = "";
    var protocol = $location.protocol();
    if (protocol !== "http" && protocol !== "https") {
        url += "http://192.168.0.112:45889/";
    } else {
        var host = $location.host();
        var port = $location.port();

        url += protocol + "://" + host + ":" + port + "/";
    }
    
    return {
        serverHost: url
    };
}]);
