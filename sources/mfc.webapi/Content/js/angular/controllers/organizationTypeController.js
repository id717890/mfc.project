(function () {
    'use strict';

    angular.module('organizationType.ctrl', []).controller('organizationTypeController', organizationTypeController);

    organizationTypeController.$inject = ['$scope', '$http', '$log', 'organizationTypesFactory', 'organizationTypeFactory'];
    function organizationTypeController($scope, $http, $log, OrganizationTypesFactory, OrganizationTypeFactory) {
        $scope.organizationTypes = OrganizationTypesFactory.query();
        $scope.selected = {};

        $scope.CreateOrganizationType = function () {
            OrganizationTypesFactory.Create($scope.selected).$promise.then(function (response) {
                $scope.organizationTypes = OrganizationTypesFactory.query();
                $scope.clearForms();
            }, function (response) {
                if (response.status == 409) alert('Данный логин уже используется');
                else {
                    alert('Ошибка при добавлении типа организаций ОГВ');
                }
            });
        }


        //Чистка форм после успешного добавления/обновления
        $scope.clearForms = function () {
            $("#CreateOrganizationType").find("input[type=text], textarea").val("");
            $("#CreateOrganizationType").modal('hide');
            $("#UpdateOrganizationType").modal('hide');
            $scope.mainUser = angular.copy({});
            $scope.newUserForm.$setPristine();
        }
    }
})();