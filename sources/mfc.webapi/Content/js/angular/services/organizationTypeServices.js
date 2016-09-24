(function () {
    'use strict';

    var organizationTypesServices = angular.module('organizationType.service', ['ngResource']);

    organizationTypesServices.factory('organizationTypesFactory', function ($resource) {
        return $resource('/api/organizationTypeApi', {}, {
            query: { method: 'GET', params: {}, isArray: true },
            create: { method: 'POST' }
        });
    });

    organizationTypesServices.factory('organizationTypeFactory', function ($resource) {
        return $resource('/api/organizationTypeApi/:id', {}, {
            show: { method: 'GET' },
            update: { method: 'PUT', params: { id: '@id' } },
            delete: { method: 'DELETE', params: { id: '@id' } }
        });
    });
})();