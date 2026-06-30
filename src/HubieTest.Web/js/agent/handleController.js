/* =====================================================================
   handleController (AGENT) — TODO (candidate area)
   Open the ticket, allow assigning, replying (interact), attaching and
   changing status.
   ===================================================================== */
angular.module('hubieTest').controller('handleController',
    ['$scope', '$stateParams', 'apiService',
    function ($scope, $stateParams, apiService) {

        $scope.ticketId = parseInt($stateParams.id, 10);
        $scope.ticket = null;
        $scope.interactions = [];
        $scope.attachments = [];
        $scope.newMessage = '';

        $scope.load = function () {
            // TODO: 'get' + 'listInteractions' + 'listAttachments'
        };

        $scope.assign = function () {
            // TODO: 'assign' and reload
            alert('TODO: implement assign.');
        };

        $scope.reply = function () {
            // TODO: 'addInteraction' (authorship = logged-in agent) and reload the thread
            alert('TODO: implement reply.');
        };

        $scope.changeStatus = function (newStatus) {
            // TODO: 'changeStatus'
            alert('TODO: implement change status to ' + newStatus + '.');
        };

        $scope.load();
    }
]);
