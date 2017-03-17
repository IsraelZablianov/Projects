class StoreManager {

    constructor(private shopingCartService: ShopingCartService) {
        this.shopingCartService.getChainNames().then(chainNames => {
            this.chainNames = this.shopingCartService.getStringsArray(chainNames);
        });
    }

    public chainNames: string[] = [];
    public storeNames: string[] = [];
    public products: product[] = [];
    public onStoreSelectGetProductsIsTrue: Function = undefined;
    public onSelect: Function = undefined;

    public selectedChain: string = undefined;
    public selectedStore: string = undefined;
    public storeToCompare: string = undefined;

    public onChainSelect() {
        if (this.selectedChain != undefined) {
            let fileIdentifiers = new FileIdentifiers();
            fileIdentifiers.DirName = this.selectedChain.toString();
            fileIdentifiers.PartialFileName = "Stores";
            this.shopingCartService.getStoreNames(fileIdentifiers, "").then(storeNames => {
                this.storeNames = this.shopingCartService.getStringsArray(storeNames);
                this.selectedStore = undefined;
                this.storeToCompare = undefined;
                if (this.onSelect !== undefined) {
                    this.onSelect();
                }
            });
        }
    }

    public onStoreSelect(getProducts: boolean) {
        let fileIdentifiers = new FileIdentifiers();
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
                this.shopingCartService.getProductNames(fileIdentifiers).then(products => {
                    products.forEach((productReturned) => {
                        let pr = new product();
                        pr.name = productReturned.toString();
                        this.products.push(pr)
                    });
                });
            }
        }
    }
}