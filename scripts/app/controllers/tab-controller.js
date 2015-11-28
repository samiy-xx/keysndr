(function () {
    'use strict';
    app.requires.push('ui.bootstrap');
    app.controller('tabController', ["$scope", TabController]);

    function TabController(scope) {
        scope.activeTab = 0;

        scope.setActive = function(i) {
            scope.activeTab = i;
        }
    }

})();