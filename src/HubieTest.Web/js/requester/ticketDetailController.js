/* =====================================================================
   ticketDetailController (REQUESTER) — TODO (candidate area)
   Ticket detail + interaction thread + attachments. The requester can also
   reply (interact) and attach files.
   ===================================================================== */
angular.module('hubieTest').controller('ticketDetailController',
    ['$scope', '$stateParams', 'apiService',
    function ($scope, $stateParams, apiService) {

        $scope.ticketId = parseInt($stateParams.id, 10);
        $scope.ticket = null;
        $scope.interactions = [];
        $scope.attachments = [];
        $scope.newMessage = '';

        $scope.load = function () {
            // TODO: get ('get'), listInteractions, listAttachments
        };

        $scope.reply = function () {
            // TODO: 'addInteraction' with $scope.newMessage and reload the thread
            alert('TODO: implement reply (addInteraction).');
        };

        $scope.load();
    }
]);
