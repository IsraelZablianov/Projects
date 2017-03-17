var ShopingCartController = (function () {
    function ShopingCartController(shopingCartService, reportService) {
        this.shopingCartService = shopingCartService;
        this.reportService = reportService;
        this.products = [];
        this.shopingCartItems = [];
        this.storesToCompare = [];
        this.selectedProduct = undefined;
        this.shopingCartItemsAmount = 0;
    }
    ShopingCartController.prototype.addProductToShopingCart = function (selectedProduct) {
        if (selectedProduct !== undefined) {
            if (this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, selectedProduct.name) === undefined) {
                this.shopingCartItems.push(selectedProduct);
            }
            ;
        }
        else {
            if (this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedProduct.name) === undefined) {
                this.shopingCartItems.push(this.selectedProduct);
            }
            ;
        }
        this.shopingCartItemsAmount = this.shopingCartItems.length;
        this.selectedProduct = undefined;
    };
    ShopingCartController.prototype.removeProductFromShopingCart = function (selectedProduct) {
        if (selectedProduct === undefined) {
            this.shopingCartItems.splice(this.shopingCartItems.indexOf(this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedProduct.name)), 1);
        }
        else {
            this.shopingCartItems.splice(this.shopingCartItems.indexOf(this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, selectedProduct.name)), 1);
        }
        this.shopingCartItemsAmount = this.shopingCartItems.length;
    };
    ShopingCartController.prototype.compareStores = function () {
        if (this.shopingCartItems.length >= 1 && this.storesToCompare.length >= 2) {
            this.reportService.compareStores(this.storesToCompare, this.shopingCartItems);
        }
    };
    return ShopingCartController;
}());
ShopingCartController.$inject = ['shopingCartService', 'reportService'];
//# sourceMappingURL=shoping-cart-controller.js.map