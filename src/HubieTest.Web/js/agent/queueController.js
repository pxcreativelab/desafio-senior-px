/* =====================================================================
   queueController (AGENT) — TODO (candidate area)
   List the ticket queue (method 'listQueue'), with an optional status filter.
   ===================================================================== */
angular.module('hubieTest').controller('queueController',
    ['$scope', '$state', 'apiService',
    function ($scope, $state, apiService) {

        $scope.tickets = [];
        $scope.statusFilter = '';

        $scope.load = function () {
            // TODO: apiService.request('ashx/process/ticket.ashx', 'listQueue',
            //       JSON.stringify({ status: $scope.statusFilter })).then(...)
        };

        $scope.handle = function (ticket) {
            $state.go('app.handle', { id: ticket.TICKET_ID });
        };

        $scope.load();
    }
]);
