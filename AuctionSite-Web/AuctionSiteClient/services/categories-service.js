var CategoriesService = (function () {
    function CategoriesService($http) {
        this.$http = $http;
        this.categories = [];
        this.initCategories();
    }
    CategoriesService.prototype.initCategories = function () {
        var _this = this;
        return this.$http.get("api/categories").then(function (data) {
            _this.categories = data.data;
            return _this.categories;
        });
    };
    CategoriesService.prototype.matchIcon = function (category, selected) {
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
        }
        else {
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
    };
    CategoriesService.prototype.matchTemplate = function (category, selected) {
        if (selected === undefined || selected === false) {
            if (category === undefined) {
                return 'all-categories-template';
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
        }
        else {
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
    };
    CategoriesService.$inject = ['$http'];
    return CategoriesService;
}());
app.service('categoriesService', CategoriesService);
//# sourceMappingURL=categories-service.js.map