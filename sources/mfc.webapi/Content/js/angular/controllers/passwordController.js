(function () {
    'use strict';

    angular.module('password.ctrl', []).controller('passwordController', passwordController);

    passwordController.$inject = ['$scope', '$http', '$log', 'PasswordFactory'];
    function passwordController($scope, $http, $log, PasswordFactory) {

        $scope.password = {
            new_password:'123',
            confirm_password:'1234'
        };

        $scope.changePassword = function (index) {
            $log.log($scope.password);
            PasswordFactory.update({ id: 1 }, $scope.password);
        }

        


    }
})();