class CategoriesService {

    public static $inject: string[] = ['$http'];

    public categories: Category[] = [];

    public constructor(private $http: ng.IHttpService) {
       this.initCategories();
    }

    public initCategories(): ng.IPromise<Category[]> {
        return this.$http.get("api/categories").then((data:ng.IHttpPromiseCallbackArg<Category[]>) => {
            this.categories = data.data;
            return this.categories;
        });
    }

    public matchIcon(category: Category, selected: boolean): string {
        if (selected === undefined || selected === false) {
            switch (category.id) {
            case 1:
                return 'resources/images/electricity.png';
            case 2:
                return 'resources/images/fashion.png';
            case 3:
                return 'resources/images/home.png';
            case 4:
                return 'resources/images/books.png';
            case 5:
                return 'resources/images/children.png';
            case 6:
                return 'resources/images/misc.png';
            }
        } else {
            switch (category.id) {
            case 1:
                return 'resources/images/electricity_w.png';
            case 2:
                return 'resources/images/fashion_w.png';
            case 3:
                return 'resources/images/home_w.png';
            case 4:
                return 'resources/images/books_w.png';
            case 5:
                return 'resources/images/children_w.png';
            case 6:
                return 'resources/images/misc_w.png';
            }
        }
    }

    public matchTemplate(category: Category, selected: boolean): string {
        if (selected === undefined || selected === false) {
            if (category === undefined) {
                return'all-categories-template';
            }
            switch (category.id) {
            case 1:
                return 'electronics-template';
            case 2:
                return 'fashion-template';
            case 3:
                return 'home-template';
            case 4:
                return 'books-template';
            case 5:
                return 'children-template';
            case 6:
                return 'misc-template';
            }
        } else {
            if (category === undefined) {
                return 'all-categories-template-selected';
            }
            switch (category.id) {
            case 1:
                return 'electronics-template-selected';
            case 2:
                return 'fashion-template-selected';
            case 3:
                return 'home-template-selected';
            case 4:
                return 'books-template-selected';
            case 5:
                return 'children-template-selected';
            case 6:
                return 'misc-template-selected';
            }
        }
    }
}


app.service('categoriesService', CategoriesService);