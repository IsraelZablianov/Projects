var StoreSelectionController = (function () {
    function StoreSelectionController(shopingCartService) {
        var _this = this;
        this.shopingCartService = shopingCartService;
        this.stores = [];
        this.products = [];
        this.storesToCompare = [];
        this.stores.push(new StoreManager(this.shopingCartService)); //1
        this.stores.push(new StoreManager(this.shopingCartService)); //2
        this.stores[0].onStoreSelectGetProductsIsTrue = function () { _this.products = _this.stores[0].products; };
        var callbackUpdateStoresToCompare = function () {
            _this.storesToCompare.length = 0;
            _this.stores.forEach(function (storeManagre) {
                if (storeManagre.storeToCompare !== undefined) {
                    _this.storesToCompare.push(storeManagre.storeToCompare);
                }
            });
        };
        this.stores[0].onSelect = callbackUpdateStoresToCompare;
        this.stores[1].onSelect = callbackUpdateStoresToCompare;
    }
    StoreSelectionController.prototype.addStoreManager = function () {
        var _this = this;
        var newStoreManager = new StoreManager(this.shopingCartService);
        this.stores.push(newStoreManager);
        newStoreManager.onSelect = function () {
            _this.storesToCompare.length = 0;
            _this.stores.forEach(function (storeManagre) {
                if (storeManagre.storeToCompare !== undefined) {
                    _this.storesToCompare.push(storeManagre.storeToCompare);
                }
            });
        };
    };
    return StoreSelectionController;
}());
StoreSelectionController.$inject = ['shopingCartService'];
//# sourceMappingURL=store-selection-controller.js.map