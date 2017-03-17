class StoreSelectionController {

    constructor(private shopingCartService: ShopingCartService) {
        this.stores.push(new StoreManager(this.shopingCartService));//1
        this.stores.push(new StoreManager(this.shopingCartService));//2
        this.stores[0].onStoreSelectGetProductsIsTrue = () => { this.products = this.stores[0].products; };

        let callbackUpdateStoresToCompare = () => {
            this.storesToCompare.length = 0;
            this.stores.forEach((storeManagre) => {
                if (storeManagre.storeToCompare !== undefined) {
                    this.storesToCompare.push(storeManagre.storeToCompare)
                }
            })
        };

        this.stores[0].onSelect = callbackUpdateStoresToCompare;
        this.stores[1].onSelect = callbackUpdateStoresToCompare;
    }

    public stores: StoreManager[] = [];
    public products: product[] = [];
    public storesToCompare: string[] = [];

    public addStoreManager() {
        let newStoreManager = new StoreManager(this.shopingCartService);
        this.stores.push(newStoreManager);
        newStoreManager.onSelect = () => {
            this.storesToCompare.length = 0;
            this.stores.forEach((storeManagre) => {
                if (storeManagre.storeToCompare !== undefined) {
                    this.storesToCompare.push(storeManagre.storeToCompare)
                }
            })
        };
    }
}

StoreSelectionController.$inject = ['shopingCartService'];