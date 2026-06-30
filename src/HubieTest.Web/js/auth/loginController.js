/* =====================================================================
   loginController — IMPLEMENTED as a reference.
   Authenticates via apiService.login, stores the token (X-User-Token header)
   and the user data in $sessionStorage, then routes by profile.
   ===================================================================== */
angular.module('hubieTest').controller('loginController',
    ['$scope', '$state', '$sessionStorage', 'apiService',
    function ($scope, $state, $sessionStorage, apiService) {

        $scope.credentials = { login: '', password: '' };
        $scope.error = null;
        $scope.loading = false;

        // shortcuts to make testing easier
        $scope.fill = function (login) {
            $scope.credentials.login = login;
            $scope.credentials.password = '123456';
        };

        $scope.signIn = function () {
            $scope.error = null;
            $scope.loading = true;

            apiService.login($scope.credentials.login, $scope.credentials.password)
                .then(function (response) {
                    var token = response.headers('X-User-Token');
                    var user = response.data;

                    if (!token) {
                        $scope.error = 'Authentication failed.';
                        return;
                    }

                    $sessionStorage.X_User_Token = token;
                    $sessionStorage.USER = user;

                    if (user.USER_PROFILE === 'AGENT') $state.go('app.queue');
                    else $state.go('app.myTickets');
                })
                .catch(function (response) {
                    var code = response.headers ? response.headers('X-User-ErrorMessage') : null;
                    $scope.error = translateError(code);
                })
                .finally(function () {
                    $scope.loading = false;
                });
        };

        function translateError(code) {
            switch (code) {
                case 'USER_NOT_FOUND': return 'User not found.';
                case 'INVALID_PASSWORD': return 'Invalid password.';
                default: return 'Could not sign in. Check your login and password.';
            }
        }
    }
]);
