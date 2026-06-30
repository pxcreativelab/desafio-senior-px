/* =====================================================================
   openTicketController (REQUESTER)
   - Load categories: IMPLEMENTED as a reference (consumes categories.ashx).
   - Save the ticket + attach a file: TODO (candidate area).
   ===================================================================== */
angular.module('hubieTest').controller('openTicketController',
    ['$scope', '$state', 'apiService',
    function ($scope, $state, apiService) {

        $scope.ticket = { TICKET_TITLE: '', TICKET_DESCRIPTION: '', CATEGORY_ID: null };
        $scope.categories = [];
        $scope.file = null;     // set by the file-input (see open-ticket.html)
        $scope.saving = false;
        $scope.error = null;

        // ---- REFERENCE: populate the category dropdown ----
        apiService.listCategories().then(function (response) {
            $scope.categories = response.data;
        });

        // ---- TODO (candidate): submit the ticket ----
        $scope.save = function () {
            $scope.error = null;

            // Expected flow:
            //  1. validate title/description/category
            //  2. POST 'open' to ticket.ashx via apiService (add apiService.openTicket)
            //  3. if $scope.file is set, upload it (multipart) to attachment.ashx
            //  4. redirect to 'app.ticketDetail' with the returned id
            //
            // Example JSON submit (without attachment):
            //   apiService.request('ashx/process/ticket.ashx', 'open',
            //       JSON.stringify($scope.ticket)).then(...);

            alert('TODO: implement the ticket submission (openTicketController.save).');
        };
    }
]);
