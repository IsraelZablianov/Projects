var AuctionsController = (function () {
    function AuctionsController(routingService, auctionsService, categoriesService, $uibModal, $scope, $window) {
        var _this = this;
        this.routingService = routingService;
        this.auctionsService = auctionsService;
        this.categoriesService = categoriesService;
        this.$uibModal = $uibModal;
        this.$scope = $scope;
        this.$window = $window;
        this.userName = window.localStorage.getItem('UserName');
        routingService.register({
            paramName: "category",
            callback: function (categoryId) {
                if (_this.categoriesService.categories.length === 0) {
                    _this.categoriesService.initCategories()
                        .then(function () { _this.matchCategory(parseInt(categoryId)); });
                }
                else {
                    _this.matchCategory(parseInt(categoryId));
                }
            }
        });
    }
    AuctionsController.prototype.onCategoryChange = function (category) {
        if (category === undefined) {
            this.currentCategory = undefined;
            this.routingService.clearUrl();
        }
        else {
            this.routingService.moveToUrl("category", category.id.toString());
        }
    };
    AuctionsController.prototype.refreshPage = function () {
        this.$window.location.reload();
    };
    AuctionsController.prototype.openSellPopup = function () {
        var modalInstance = this.$uibModal.open({
            templateUrl: 'components/Sell-popup/sell-popup-template.html',
            controller: SellPopupController,
            controllerAs: '$ctrl',
            size: 'sell-popup-size'
        });
    };
    AuctionsController.prototype.openPlaceBid = function (auction) {
        var modalInstance = this.$uibModal.open({
            templateUrl: "components/Place-bid/place-bid-template.html",
            controller: PlaceBidController,
            controllerAs: '$ctrl',
            resolve: {
                auction: function () { return auction; }
            },
            size: 'place-bid-size'
        });
    };
    AuctionsController.prototype.logout = function () {
        this.$window.localStorage.setItem('Authorization', '');
        this.refreshPage();
    };
    AuctionsController.prototype.areAuctionsAvalable = function () {
        if (this.currentCategory === undefined)
            return this.auctionsService.auctions.length !== 0;
        for (var _i = 0, _a = this.auctionsService.auctions; _i < _a.length; _i++) {
            var auction = _a[_i];
            if (auction.category.id === this.currentCategory.id)
                return true;
        }
        return false;
    };
    AuctionsController.prototype.routingServiceCallback = function (categoryId) {
        var _this = this;
        if (this.categoriesService.categories.length === 0) {
            this.categoriesService.initCategories()
                .then(function () { _this.matchCategory(parseInt(categoryId)); });
        }
        else {
            this.matchCategory(parseInt(categoryId));
        }
    };
    AuctionsController.prototype.matchCategory = function (categoryId) {
        for (var _i = 0, _a = this.categoriesService.categories; _i < _a.length; _i++) {
            var category = _a[_i];
            if (category.id === categoryId) {
                this.currentCategory = { id: category.id, name: category.name };
            }
        }
    };
    AuctionsController.$inject = ['routingService', 'auctionsService', 'categoriesService', '$uibModal', '$scope', '$window'];
    return AuctionsController;
}());
//# sourceMappingURL=auctions-controller.js.map