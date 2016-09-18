(function () {
    'use strict';

    angular
        .module('webapi')
        .controller('usersController', usersController);

    usersController.$inject = ['$scope', '$http', 'Users'];
    function usersController($scope, $http, Users) {
        $scope.users = Users.query();

        $scope.mainUser = {
            User_Name: '',
            Description: ''
        };

        $scope.submitForm = function () {
            $http({
                method: 'POST',
                url: '/api/user',
                data: $scope.mainUser
            }).then(function successCallback(response) {
                if (response.status === 201) window.location.href = 'List';
            }, function errorCallback(response) {
                alert("Ошибка : " + response.data);
            });
        };

        $scope.deleteUser = function (index) {
            if (confirm('Удалить пользователя "' + $scope.users[index].user_name + ' | ' + $scope.users[index].description + '" ?')) {
                $http({
                    method: 'DELETE',
                    url: '/api/user/' + $scope.users[index].id,
                    data: $scope.users[index].id
                }).then(function successCallback(response) {
                    if (response.status === 200) $scope.users.splice(index, 1);
                }, function errorCallback(response) {
                    alert("Ошибка при удалении пользователя");
                });
            }


        };
    }
})();