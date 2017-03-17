class UserService {
    public static $inject: string[] = ['$http', '$window'];
    public isLogedIn: boolean = false;

    public constructor(private $http: ng.IHttpService, private $window: ng.IWindowService) {}

    public addUser(user: UserRegistrationDTO, captcha: string): ng.IPromise<AddUserStatuse> {
        return this.verifyCaptcha(captcha)
            .then((response) => {
                if (response) {
                    this.updateAutentication({ userName: user.id, password: user.password });
                    return this.$http.post("api/users", user)
                        .then(data => {
                            if (data.data) {
                                return AddUserStatuse.SUCCESS;
                            } else {
                                return AddUserStatuse.ALREADY_EXIST;
                            }
                        });
                } else {
                    return AddUserStatuse.CAPTCHA_ERR;
                }
            });
    }

    public verifyCaptcha(captcha: string): ng.IPromise<boolean> {
        return this.$http.post<boolean>("api/captcha", { captcha: captcha }).then(data => {
            return data.data;
        });
    }

    public checkIfLogedIn(): ng.IPromise<boolean> {
        return this.checkAuthorization().then(response => this.isLogedIn = response);
    }

    public login(userlogin: userLogin): ng.IPromise<boolean> {
     
        this.updateAutentication(userlogin);
        return this.checkAuthorization();
    }

    private checkAuthorization(): ng.IPromise<boolean> {
        return this.$http.get("api/users").
            then(data => data.status === 200);
    }

    private updateAutentication(userlogin: userLogin) {
        this.updateLocalStorage(userlogin);
        this.$http.defaults.headers.common['Authorization'] = 'Basic ' + window.btoa(userlogin.userName + ':' + userlogin.password);
    }

    private updateLocalStorage(userlogin: userLogin) {
        this.$window.localStorage.setItem('Authorization', 'Basic ' + window.btoa(userlogin.userName + ':' + userlogin.password));
        this.$window.localStorage.setItem('UserName', userlogin.userName);
    }
}

enum AddUserStatuse{
    SUCCESS, CAPTCHA_ERR, ALREADY_EXIST
}

app.service('userService', UserService);