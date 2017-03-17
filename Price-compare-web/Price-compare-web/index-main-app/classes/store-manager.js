var StoreManager = (function () {
    function StoreManager(shopingCartService) {
        var _this = this;
        this.shopingCartService = shopingCartService;
        this.chainNames = [];
        this.storeNames = [];
        this.products = [];
        this.onStoreSelectGetProductsIsTrue = undefined;
        this.onSelect = undefined;
        this.selectedChain = undefined;
        this.selectedStore = undefined;
        this.storeToCompare = undefined;
        this.shopingCartService.getChainNames().then(function (chainNames) {
            _this.chainNames = _this.shopingCartService.getStringsArray(chainNames);
        });
    }
    StoreManager.prototype.onChainSelect = function () {
        var _this = this;
        if (this.selectedChain != undefined) {
            var fileIdentifiers = new FileIdentifiers();
            fileIdentifiers.DirName = this.selectedChain.toString();
            fileIdentifiers.PartialFileName = "Stores";
            this.shopingCartService.getStoreNames(fileIdentifiers, "").then(function (storeNames) {
                _this.storeNames = _this.shopingCartService.getStringsArray(storeNames);
                _this.selectedStore = undefined;
                _this.storeToCompare = undefined;
                if (_this.onSelect !== undefined) {
                    _this.onSelect();
                }
            });
        }
    };
    StoreManager.prototype.onStoreSelect = function (getProducts) {
        var _this = this;
        var fileIdentifiers = new FileIdentifiers();
        if (this.selectedStore != undefined) {
            this.storeToCompare = this.selectedChain + "," + this.selectedStore;
            if (this.onSelect !== undefined) {
                this.onSelect();
            }
            if (getProducts === true) {
                if (this.onStoreSelectGetProductsIsTrue !== undefined) {
                    this.onStoreSelectGetProductsIsTrue();
                }
                this.products.length = 0;
                fileIdentifiers.DirName = this.selectedChain;
                fileIdentifiers.PartialFileName = this.selectedStore;
                this.shopingCartService.getProductNames(fileIdentifiers).then(function (products) {
                    products.forEach(function (productReturned) {
                        var pr = new product();
                        pr.name = productReturned.toString();
                        _this.products.push(pr);
                    });
                });
            }
        }
    };
    return StoreManager;
}());
//# sourceMappingURL=store-manager.js.map