var RoutingService = (function () {
    function RoutingService($window, $rootScope, $location) {
        var _this = this;
        this.$window = $window;
        this.$rootScope = $rootScope;
        this.$location = $location;
        this.listeners = [];
        $rootScope.$on('routChangedEvent', function (ev, paramName, param) {
            for (var _i = 0, _a = _this.listeners; _i < _a.length; _i++) {
                var listener = _a[_i];
                if (paramName === listener.paramName) {
                    listener.callback(param);
                }
            }
        });
        $rootScope.$watch(function () { return $location.path(); }, function (newPath) {
            if (newPath !== '') {
                var paramName = newPath.split('/')[1].split('=')[0];
                var param = newPath.split('/')[1].split('=')[1];
                $rootScope.$broadcast('routChangedEvent', paramName, param);
            }
        });
    }
    RoutingService.prototype.register = function (listener) {
        this.listeners.push(listener);
    };
    RoutingService.prototype.moveToUrl = function (paramName, param) {
        this.$location.path(paramName + '=' + param);
        this.$location.replace();
        this.$rootScope.$broadcast('routChangedEvent', param);
    };
    RoutingService.prototype.clearUrl = function () {
        this.$location.url('');
    };
    RoutingService.$inject = ['$window', '$rootScope', '$location'];
    return RoutingService;
}());
var RouteServiceListener = (function () {
    function RouteServiceListener() {
    }
    return RouteServiceListener;
}());
app.service('routingService', RoutingService);
//# sourceMappingURL=routing-service.js.map