(function () {
    'use strict';

    angular
        .module('webapi')
        .controller('usersController', usersController);

    usersController.$inject = ['$scope', 'Users'];
    function usersController($scope, Users) {
        $scope.users = Users.query();
    }
})();