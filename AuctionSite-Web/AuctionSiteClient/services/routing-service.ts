class RoutingService {
    public static $inject: string[] = ['$window', '$rootScope', '$location'];

    listeners: RouteServiceListener[] = [];

    public constructor(private $window: ng.IWindowService,
        private $rootScope: ng.IRootScopeService,
        private $location: ng.ILocationService) {

        $rootScope.$on('routChangedEvent',
            (ev, paramName, param) => {
                for (var listener of this.listeners) {
                    if (paramName === listener.paramName) {
                        listener.callback(param);
                    }
                }
            });

        $rootScope.$watch(() => { return $location.path() },
            (newPath: string) => {
                if (newPath !== '') {
                    var paramName = newPath.split('/')[1].split('=')[0];
                    var param = newPath.split('/')[1].split('=')[1];
                    $rootScope.$broadcast('routChangedEvent', paramName, param);
                }
            });
    }

    public register(listener: RouteServiceListener) {
        this.listeners.push(listener);
    }

    public moveToUrl(paramName:string, param: string) {
        this.$location.path(paramName + '=' + param);
        this.$location.replace();
        this.$rootScope.$broadcast('routChangedEvent', param);

    }

    public clearUrl() {
        this.$location.url('');
    }
}

class RouteServiceListener {
    paramName: string;
    callback: Function;
}

app.service('routingService', RoutingService);