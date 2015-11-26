var app = angular.module("keysndrsite", []);

app.config(['$locationProvider',
    function ($locationProvider) {
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
]);

// Prototypes
Array.prototype.insertAt = function (index, item) {
    this.splice(index, 0, item);
};

Array.prototype.removeAt = function (index) {
    this.splice(index, 1);
};