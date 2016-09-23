(function () {
    'use strict';

    var usersServices = angular.module('user.service', ['ngResource']);

    usersServices.factory('UsersFactory', function ($resource) {
        return $resource('/api/user', {}, {
            query: { method: 'GET', params: {}, isArray: true },
            create: { method: 'POST' }
        });
    });

    usersServices.factory('UserFactory', function ($resource) {
        return $resource('/api/user/:id', {}, {
            show: { method: 'GET' },
            update: { method: 'PUT', params: { id: '@id' } },
            delete: { method: 'DELETE', params: { id: '@id' } }
        });
    });
})();