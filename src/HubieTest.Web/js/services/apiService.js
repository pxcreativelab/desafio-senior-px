/* =====================================================================
   apiService — mirrors the Hubie ticketService.
   - builds the headers (form-urlencoded Content-Type + Bearer Authorization);
   - POSTs { method, data } to the given .ashx.

   AUTH and CATEGORIES are IMPLEMENTED as a reference. The TICKET endpoints are
   TODO (candidate area) — the "method" names must match the switch in ticket.ashx.cs.
   ===================================================================== */
angular.module('hubieTest').factory('apiService',
    ['$http', '$httpParamSerializerJQLike', '$sessionStorage',
    function ($http, $httpParamSerializerJQLike, $sessionStorage) {

        // backend base (.ashx). Change it if you host the backend on another port.
        var WEB_SERVER = $sessionStorage.webServer || '/';

        function headers() {
            var h = { 'Content-Type': 'application/x-www-form-urlencoded;' };
            if ($sessionStorage.X_User_Token) {
                h['Authorization'] = 'Bearer ' + $sessionStorage.X_User_Token;
            }
            return h;
        }

        // generic request in the Hubie style
        function request(url, method, data) {
            var params = { method: method, data: data };
            return $http({
                method: 'POST',
                url: WEB_SERVER + url,
                data: $httpParamSerializerJQLike(params),
                headers: headers()
            });
        }

        return {
            // ---------------- AUTH (reference) ----------------
            login: function (login, password) {
                // the token comes back in the X-User-Token header (see loginController)
                return request('ashx/auth/starter.ashx', 'authlogin',
                    JSON.stringify({ login: login, password: password }));
            },

            // ---------------- CATEGORIES (reference) ----------------
            listCategories: function () {
                return request('ashx/process/categories.ashx', 'list', null);
            },

            // ---------------- TICKET (candidate area) ----------------
            // Hint: they all follow the same request(...) pattern above.
            // Implement them as you need in the controllers:
            //
            // openTicket: function (ticket) {
            //     return request('ashx/process/ticket.ashx', 'open', JSON.stringify(ticket));
            // },
            // listMyTickets: function () { ... 'listMine' ... },
            // listQueue: function (status) { ... 'listQueue' ... },
            // getTicket: function (id) { ... 'get' ... },
            // assign: function (id) { ... 'assign' ... },
            // changeStatus: function (id, status) { ... 'changeStatus' ... },
            // addInteraction: function (id, message) { ... 'addInteraction' ... },
            // listInteractions: function (id) { ... 'listInteractions' ... },
            // listAttachments: function (id) { ... 'listAttachments' ... },
            //
            // Attachment upload is multipart (FormData) — see open-ticket.html / handle.html.

            // exposes the generic request so the candidate can use it freely
            request: request,
            webServer: WEB_SERVER
        };
    }
]);
