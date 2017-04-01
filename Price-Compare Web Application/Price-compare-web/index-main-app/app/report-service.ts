class ReportService {
    public constructor(private $http: ng.IHttpService) {
    }

    private report: Report[] = [];
    private storesToCompare: Object[] = [];
    private cheapestProductsFromAllStores: product[] = [];
    public totalCheapestPrice = Number.MAX_VALUE;
    public inReportMode: boolean = false;
    public stores: StoreDTO[] = [];

    public compareStores(storesToCompare: Object[], products: product[]) {
        this.report = [];
        this.cheapestProductsFromAllStores = [];
        this.stores = [];
        this.storesToCompare = [];

        this.storesToCompare = storesToCompare;
        this.inReportMode = true;
        this.getReport(storesToCompare, products).then(reportReturned => {
            this.report = reportReturned;
            this.parseReportOfAllStores();
            this.calculateCheapestPrice();
            });
    }

    public isProductPriceCheapest(product: product) {
        let foundProduct = false;
        let isCheaper = false;

        for (let i = 0; i < this.cheapestProductsFromAllStores.length && !foundProduct; i++) {
            if (this.cheapestProductsFromAllStores[i].name == product.name) {
                isCheaper = this.cheapestProductsFromAllStores[i].price === product.price ? true : false;
                foundProduct = true;
            }
        }

        return isCheaper;
    }

    private parseReportOfAllStores() {
        this.parseStoresNames();

        this.stores.forEach((store) => {
            let reportOfStore = this.getReportOfStoreByFullName(store.fullStoreName);
            if (reportOfStore !== undefined) {
                let parsedReport = reportOfStore.split('\n');
                store.productsTotalPrice = Number(parsedReport[0].split('=')[1]);
                for (let i = 1; i < parsedReport.length; i++) {
                    if (parsedReport[i] !== undefined && parsedReport[i].length > 1) {
                        let pr = new product();
                        pr.name = parsedReport[i].split('=')[1];
                        pr.price = parsedReport[i].split('=')[0];
                        store.products.push(pr)
                    }
                }

                if (store.products.length !== 0) {
                    this.updateCheapestPrices(store.products);
                }
            }

        });
    }

    private updateCheapestPrices(products: product[]) {
        if (this.cheapestProductsFromAllStores.length === 0) {
            this.initialProductsForCheapestPrices(products);
        }
        else {
            products.forEach((product) => {
                let foundProduct = false;
                for (let i = 0; i < this.cheapestProductsFromAllStores.length && !foundProduct; i++) {
                    if (this.cheapestProductsFromAllStores[i].name === product.name) {
                        foundProduct = true;
                        if (Number(this.cheapestProductsFromAllStores[i].price) !== NaN
                            && Number(product.price) !== NaN) {
                            if (Number(product.price) < Number(this.cheapestProductsFromAllStores[i].price)) {
                                this.cheapestProductsFromAllStores[i].price = product.price;
                            }
                        }
                        else if (Number(this.cheapestProductsFromAllStores[i].price) === NaN
                            && Number(product.price) !== NaN) {
                            this.cheapestProductsFromAllStores[i].price = product.price;
                        }
                    }
                }
            });
        }
    }

    private initialProductsForCheapestPrices(products: product[]) {
        if (this.cheapestProductsFromAllStores.length === 0) {
            products.forEach((pr) => {
                let newProduct = new product();
                newProduct.name = pr.name;
                newProduct.price = pr.price;
                this.cheapestProductsFromAllStores.push(newProduct);
            });
        }
    }

    private parseStoresNames() {
        this.storesToCompare.forEach((comparedStore) => {
            let FullstoreName: string[] = comparedStore.toString().split(',');
            let storeDTO: StoreDTO = new StoreDTO();
            storeDTO.fullStoreName = comparedStore.toString();
            storeDTO.chainName = FullstoreName[0];
            storeDTO.storeName = FullstoreName[1];
            this.stores.push(storeDTO);
        });
    }

    private calculateCheapestPrice() {
        this.stores.forEach((store) => {
            if (store.productsTotalPrice < this.totalCheapestPrice) {
                this.totalCheapestPrice = store.productsTotalPrice;
            }
        });
    }

    private getReportOfStoreByFullName(storeName: string): string {
        let storeReport: string = undefined;
        for (let i = 0; i < this.report.length && storeReport === undefined; i++) {
            if (this.report[i].storeName === storeName) {
                storeReport = this.report[i].storeReport;
            }
        }

        return storeReport;
    }

    private getReport(storesToCompare: Object[], products: product[]): ng.IPromise<Report[]> {
        return this.$http.post("api/serverPriceCompare/getReport", {
            storesToCompare: storesToCompare,
            products: products
        }).then(data => {
            return data.data;
        });
    }
}
ReportService.$inject = ['$http'];
app.service("reportService", ReportService);