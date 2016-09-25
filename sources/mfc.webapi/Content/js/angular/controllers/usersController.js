(function () {
    'use strict';

    angular.module('user.ctrl', []).controller('usersController', usersController);

    usersController.$inject = ['$scope', '$http', '$log', 'UsersFactory', 'UserFactory', 'PasswordFactory'];
    function usersController($scope, $http, $log, UsersFactory, UserFactory, PasswordFactory) {
        $scope.users = UsersFactory.query();

        $scope.mainUser = {};
        $scope.password = {
            new_password: '',
            confirm_password: ''
        };

        //Добавление пользователя
        $scope.createUser = function () {
            UsersFactory.create($scope.mainUser).$promise.then(function (response) {
                $scope.users = UsersFactory.query();
                $scope.clearForms();
            }, function (response) {
                if (response.status == 409) alert('Данный логин уже используется');
                else {
                    alert('Ошибка при добавлении пользователя');
                }
            });
        }

        //Удаления пользователя
        $scope.deleteUser = function (user_id) {
            if (confirm('Удалить пользователя "' + $scope.users[user_id].user_name + ' | ' + $scope.users[user_id].description + '" ?')) {
                UserFactory.delete({ id: $scope.users[user_id].id });
                $scope.users.splice(user_id, 1);
            };
        }

        //Обновление пользователя
        $scope.updateUser = function () {
            UserFactory.update({ id: $scope.mainUser.id }, $scope.mainUser).$promise.then(function (responce) {
                $scope.users = UsersFactory.query();
                $scope.clearForms();
            }, function (response) {
                if (response.status === 409) alert('Данный логин уже используется');
                else {
                    alert('Ошибка при добавлении пользователя');
                    $log.log(response);
                }
            });
        }

        //обработчик кнопки "Редактировать пользователя"
        $scope.editUser = function (index) {
            UserFactory.show({ id: $scope.users[index].id }).$promise.then(function (response) {
                $scope.mainUser = response;
                $scope.updateUserForm.$setPristine();
            }, function (response) {
                $log.log(response);
            });
        }

        //Чистка форм после успешного добавления/обновления
        $scope.clearForms = function () {
            $("#CreateUser").find("input[type=text], textarea").val("");
            $("#CreateUser").modal('hide');
            $("#UpdateUser").modal('hide');
            $scope.mainUser = angular.copy({});
            $scope.newUserForm.$setPristine();
        }

        $scope.find_user = function (index) {
            UserFactory.show({ id: $scope.users[index].id }).$promise.then(function (response) {
                $scope.mainUser = response;
            }, function (response) {
                $log.log(response);
            });
        }

        $scope.changePassword = function () {
            $log.log($scope.password.new_password);
            PasswordFactory.update({ id: $scope.mainUser.id }, '"'+$scope.password.new_password+'"');
        }
    }

    angular.module('password.ctrl', []).controller('passwordController', passwordController);

    passwordController.$inject = ['$scope', '$http', '$log', 'PasswordFactory'];
    function passwordController($scope, $http, $log, PasswordFactory) {






    }
})();