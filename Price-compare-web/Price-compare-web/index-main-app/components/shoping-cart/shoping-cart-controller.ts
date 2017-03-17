class ShopingCartController {

    constructor(private shopingCartService: ShopingCartService, private reportService: ReportService) {
    }

    public products: product[] = [];
    public shopingCartItems: product[] = [];
    public storesToCompare: string[] = [];

    public selectedProduct: product = undefined;
    public shopingCartItemsAmount: number = 0;

    public addProductToShopingCart(selectedProduct: product) {
        if (selectedProduct !== undefined) {
            if (this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, selectedProduct.name) === undefined) {
                this.shopingCartItems.push(selectedProduct);
            };
        }
        else {
            if (this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedProduct.name) === undefined) {
                this.shopingCartItems.push(this.selectedProduct);
            };
        }

        this.shopingCartItemsAmount = this.shopingCartItems.length;
        this.selectedProduct = undefined;
    }

    public removeProductFromShopingCart(selectedProduct: product) {
        if (selectedProduct === undefined) {
            this.shopingCartItems.splice(this.shopingCartItems.indexOf(this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, this.selectedProduct.name)), 1);
        }
        else {
            this.shopingCartItems.splice(this.shopingCartItems.indexOf(this.shopingCartService.getProductItemInShopingCart(this.shopingCartItems, selectedProduct.name)), 1);
        }

        this.shopingCartItemsAmount = this.shopingCartItems.length;
    }

    public compareStores() {
        if (this.shopingCartItems.length >= 1 && this.storesToCompare.length >= 2) {
            this.reportService.compareStores(this.storesToCompare, this.shopingCartItems);
        }
    }
}

ShopingCartController.$inject = ['shopingCartService', 'reportService'];