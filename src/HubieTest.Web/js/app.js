/* =====================================================================
   Hubie - .NET Senior Tech Test
   AngularJS module + routes (classic ui-router), mirroring the Hubie pattern
   (service factory + $http POST form-urlencoded to .ashx, JWT token kept in
   $sessionStorage).
   ===================================================================== */
angular.module('hubieTest', ['ui.router', 'ngStorage']);

angular.module('hubieTest').config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/login');

        $stateProvider
            // ---------------- public ----------------
            .state('login', {
                url: '/login',
                templateUrl: 'views/login.html',
                controller: 'loginController'
            })

            // -------------- authenticated area (layout with navbar) --------------
            .state('app', {
                abstract: true,
                templateUrl: 'views/shell.html'
            })

            // ---- REQUESTER ----
            .state('app.myTickets', {
                url: '/my-tickets',
                templateUrl: 'views/requester/my-tickets.html',
                controller: 'myTicketsController',
                data: { profile: 'REQUESTER' }
            })
            .state('app.openTicket', {
                url: '/open-ticket',
                templateUrl: 'views/requester/open-ticket.html',
                controller: 'openTicketController',
                data: { profile: 'REQUESTER' }
            })
            .state('app.ticketDetail', {
                url: '/ticket/:id',
                templateUrl: 'views/requester/ticket-detail.html',
                controller: 'ticketDetailController',
                data: { profile: 'REQUESTER' }
            })

            // ---- AGENT ----
            .state('app.queue', {
                url: '/queue',
                templateUrl: 'views/agent/queue.html',
                controller: 'queueController',
                data: { profile: 'AGENT' }
            })
            .state('app.handle', {
                url: '/handle/:id',
                templateUrl: 'views/agent/handle.html',
                controller: 'handleController',
                data: { profile: 'AGENT' }
            });
    }
]);

/* Authentication + per-profile authorization guard.
   - no token -> redirect to /login
   - route requires a profile different from the user's -> block */
angular.module('hubieTest').run(['$rootScope', '$state', '$sessionStorage',
    function ($rootScope, $state, $sessionStorage) {

        $rootScope.$on('$stateChangeStart', function (event, toState) {
            var loggedIn = !!$sessionStorage.X_User_Token;

            if (toState.name !== 'login' && !loggedIn) {
                event.preventDefault();
                $state.go('login');
                return;
            }

            var requiredProfile = toState.data && toState.data.profile;
            if (requiredProfile && $sessionStorage.USER &&
                $sessionStorage.USER.USER_PROFILE !== requiredProfile) {
                event.preventDefault();
                goHome($state, $sessionStorage);
            }
        });
    }
]);

/* Layout controller for the authenticated area: exposes the user and logout. */
angular.module('hubieTest').controller('shellController',
    ['$scope', '$state', '$sessionStorage',
    function ($scope, $state, $sessionStorage) {
        $scope.user = $sessionStorage.USER;
        $scope.signOut = function () {
            delete $sessionStorage.X_User_Token;
            delete $sessionStorage.USER;
            $state.go('login');
        };
    }
]);

function goHome($state, $sessionStorage) {
    var profile = $sessionStorage.USER ? $sessionStorage.USER.USER_PROFILE : null;
    if (profile === 'AGENT') $state.go('app.queue');
    else if (profile === 'REQUESTER') $state.go('app.myTickets');
    else $state.go('login');
}
