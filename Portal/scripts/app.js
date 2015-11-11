var app = angular.module("keysndr", []);

app.config(['$locationProvider',
    function ($locationProvider) {
        $locationProvider.html5Mode({
            enabled: false,
            requireBase: false
        });
    }
]);
