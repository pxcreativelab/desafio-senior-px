/* =====================================================================
   myTicketsController (REQUESTER) — TODO (candidate area)
   List the logged-in requester's tickets (method 'listMine').
   ===================================================================== */
angular.module('hubieTest').controller('myTicketsController',
    ['$scope', '$state', 'apiService',
    function ($scope, $state, apiService) {

        $scope.tickets = [];
        $scope.loading = false;

        $scope.load = function () {
            // TODO: apiService.request('ashx/process/ticket.ashx', 'listMine', null)
            //       .then(function (r) { $scope.tickets = r.data; });
        };

        $scope.openDetail = function (ticket) {
            $state.go('app.ticketDetail', { id: ticket.TICKET_ID });
        };

        $scope.load();
    }
]);
