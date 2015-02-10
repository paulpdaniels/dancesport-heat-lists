/**
 * Created by Paul on 1/31/2015.
 */

;
(function (root) {
    'use strict';
    var module = angular.module('dancesportHeatLists', ['ui.bootstrap', 'ngRoute']);

    module.config(function ($routeProvider) {
        $routeProvider
            .when('/about', {templateUrl: 'partials/about.html', controller: 'AboutController'})
            .when('/search', {templateUrl: 'partials/search.html', controller: 'CompetitionController'})
            .otherwise('/search');
    });

    module.controller('AboutController', function ($scope) {

    });

    module.controller('NavigationController', function($scope, $rootScope, $location) {

        $scope.activeItem = '/search';


        $rootScope.$on('$routeChangeSuccess', function() {
            $scope.activeItem = $location.url();
        });

    });


    module.controller('CompetitionController', function ($scope, $http, $q) {

        $scope.competitions = [];
        $scope.selectedCompetition = null;
        $scope.events = [];
        $scope.selectedEvent = null;

        $scope.competitors = {};
        $scope.competitorList = [];
        $scope.selectedCompetitor = null;

        function reset() {
            $scope.competitors = {}; $scope.selectedCompetitor = null;
            $scope.competitorList = [];
        }


        function computeEvents() {

            reset();

            for (var i = 0, len = $scope.events.length; i < len; ++i) {
                var event = $scope.events[i];
                if (!event) continue;
                for (var j = 0, jlen = event.dancers.length; j < jlen; j++) {
                    if (!event.dancers[j]) continue;
                    var name = event.dancers[j].name;
                    !$scope.competitors[name] && ($scope.competitors[name] = []) && $scope.competitorList.push(event.dancers[j]);
                    $scope.competitors[name].push(i);
                }
            }
        }

        $http.get('competitions/manifest.json')
            .success(function (data, status, headers, config) {
                if (status === 200) {
                    var manifest = angular.fromJson(data);
                    $scope.competitions = manifest.competitions;
                    $scope.selectedCompetition = $scope.competitions[0];
                }

                if ($scope.selectedCompetition) {
                    $http.get('competitions/' + $scope.selectedCompetition.key + '.json')
                        .success(function (data, status, headers, config) {
                            if (status === 200) {
                                $scope.events = angular.fromJson(data).events;
                                $scope.selectedEvent = $scope.events[0];
                                computeEvents();
                            }
                        });
                }
            });


        $scope.lookup = "";
        $scope.competitorLookup = "";

        $scope.changeCompetition = function(){
            if ($scope.selectedCompetition) {
                $http.get('competitions/' + $scope.selectedCompetition.key + '.json')
                    .success(function(data, status, headers, config){
                        if (status === 200) {
                            $scope.events = angular.fromJson(data).events;
                            $scope.selectedEvent = $scope.events[0];
                            computeEvents();
                        }
                    });
            }
        };

        $scope.select = function (item, model, label) {

            var i = 0;
            for (var len = $scope.events.length; i < len; ++i) {
                if (model == $scope.events[i]) {
                    $scope.selectedEvent = $scope.events[i];
                    break;
                }
            }
        };

        $scope.selectCompetitor = function(item, model, label) {
            $scope.selectedCompetitor = $scope.competitors[item.name];
        };

        $scope.getEventName = function(eventId) {
            return $scope.events[eventId].name;
        };

    });

})(window);


