(function () {
    'use strict';
    var usersServices = angular.module('usersServices', ['ngResource']);

    usersServices.factory('Users', ['$resource',
      function ($resource) {
          return $resource('/api/user', {}, {
              query: { method: 'GET', params: {}, isArray: true }
          });
      }]);
})();