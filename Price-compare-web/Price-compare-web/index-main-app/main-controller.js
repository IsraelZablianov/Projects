var MainController = (function () {
    function MainController(shopingCartService) {
        var _this = this;
        this.shopingCartService = shopingCartService;
        this.chainNames = [];
        this.storeNames = [];
        this.productNames = [];
        this.selectedStoresToCompare = [];
        this.shopingCartItems = [];
        this.report = [];
        this.selectedChain = undefined;
        this.selectedStore = undefined;
        this.selectedItem = undefined;
        this.selectedStoreToCompare = undefined;
        this.selectedShopingCartItem = undefined;
        this.selectedStoreToReport = undefined;
        this.checkBoxSelectBranch = false;
        this.checkBoxselectItem = false;
        this.amountOfItem = 1;
        this.filterAreaName = "";
        this.selectedStoreReport = "";
        this.shopingCartService.getChainNames().then(function (chainNames) { _this.chainNames = chainNames; });
    }
    MainController.prototype.addStores = function () {
        var _this = this;
        if (this.selectedChain != undefined) {
            var fileIdentifiers = new FileIdentifiers();
            fileIdentifiers.DirName = this.selectedChain.toString();
            fileIdentifiers.PartialFileName = "Stores";
            this.shopingCartService.getStoreNames(fileIdentifiers, this.filterAreaName).then(function (storeNames) { _this.storeNames = storeNames; });
        }
        this.filterAreaName = "";
    };
    MainController.prototype.addProducts = function () {
        var _this = this;
        var fileIdentifiers = new FileIdentifiers();
        if (this.selectedStore != undefined) {
            fileIdentifiers.DirName = this.selectedChain.toString();
            fileIdentifiers.PartialFileName = this.selectedStore.toString();
            this.shopingCartService.getProductNames(fileIdentifiers).then(function (productNames) {
                _this.productNames = productNames;
            });
        }
        var selectedBranch = fileIdentifiers.DirName + "," + fileIdentifiers.PartialFileName;
        if (this.checkBoxSelectBranch === true && this.selectedStoresToCompare.indexOf(selectedBranch) < 0) {
            this.selectedStoresToCompare.push(selectedBranch);
        }
    };
    MainController.prototype.removeSelectedStore = function () {
        if (this.selectedStoreToCompare != undefined) {
            this.selectedStoresToCompare.splice(this.selectedStoresToCompare.indexOf(this.selectedStoreToCompare), 1);
        }
    };
    MainController.prototype.addProductToShopingCart = function () {
        if (this.selectedItem != undefined && this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedItem.toString()) === undefined) {
            var product_1 = { name: this.selectedItem.toString(), quantity: 1 };
            this.shopingCartItems.push(product_1);
        }
    };
    MainController.prototype.shopingCartItemSelected = function () {
        if (this.checkBoxselectItem == true) {
            this.shopingCartItems.splice(this.shopingCartItems.indexOf(this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedShopingCartItem.toString())), 1);
        }
        else {
            this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedShopingCartItem.toString()).quantity = this.amountOfItem;
        }
    };
    MainController.prototype.compareStores = function () {
        var _this = this;
        if (this.shopingCartItems.length > 0 && this.selectedStoresToCompare.length > 0) {
            this.shopingCartService.getReport(this.selectedStoresToCompare, this.shopingCartItems).then(function (reportReturned) {
                _this.report = reportReturned;
            });
        }
    };
    MainController.prototype.selectedStoreToReportChanged = function () {
        for (var i = 0; i < this.report.length; i++) {
            if (this.report[i].storeName === this.selectedStoreToReport.toString()) {
                this.selectedStoreReport = this.report[i].storeReport;
            }
        }
    };
    return MainController;
}());
app.controller("mainAppController", MainController);
