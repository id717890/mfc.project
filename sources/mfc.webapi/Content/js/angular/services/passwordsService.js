(function () {
    'use strict';

    var usersServices = angular.module('password.service', ['ngResource']);

    usersServices.factory('PasswordFactory', function ($resource) {
        return $resource('/api/password/:id', {}, {
            update: { method: 'PUT', params: { id: '@id' } }
        });
    });
})();