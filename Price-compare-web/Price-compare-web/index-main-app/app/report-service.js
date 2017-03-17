var ReportService = (function () {
    function ReportService($http) {
        this.$http = $http;
        this.report = [];
        this.storesToCompare = [];
        this.cheapestProductsFromAllStores = [];
        this.totalCheapestPrice = Number.MAX_VALUE;
        this.inReportMode = false;
        this.stores = [];
    }
    ReportService.prototype.compareStores = function (storesToCompare, products) {
        var _this = this;
        this.report = [];
        this.cheapestProductsFromAllStores = [];
        this.stores = [];
        this.storesToCompare = [];
        this.storesToCompare = storesToCompare;
        this.inReportMode = true;
        this.getReport(storesToCompare, products).then(function (reportReturned) {
            _this.report = reportReturned;
            _this.parseReportOfAllStores();
            _this.calculateCheapestPrice();
        });
    };
    ReportService.prototype.isProductPriceCheapest = function (product) {
        var foundProduct = false;
        var isCheaper = false;
        for (var i = 0; i < this.cheapestProductsFromAllStores.length && !foundProduct; i++) {
            if (this.cheapestProductsFromAllStores[i].name == product.name) {
                isCheaper = this.cheapestProductsFromAllStores[i].price === product.price ? true : false;
                foundProduct = true;
            }
        }
        return isCheaper;
    };
    ReportService.prototype.parseReportOfAllStores = function () {
        var _this = this;
        this.parseStoresNames();
        this.stores.forEach(function (store) {
            var reportOfStore = _this.getReportOfStoreByFullName(store.fullStoreName);
            if (reportOfStore !== undefined) {
                var parsedReport = reportOfStore.split('\n');
                store.productsTotalPrice = Number(parsedReport[0].split('=')[1]);
                for (var i = 1; i < parsedReport.length; i++) {
                    if (parsedReport[i] !== undefined && parsedReport[i].length > 1) {
                        var pr = new product();
                        pr.name = parsedReport[i].split('=')[1];
                        pr.price = parsedReport[i].split('=')[0];
                        store.products.push(pr);
                    }
                }
                if (store.products.length !== 0) {
                    _this.updateCheapestPrices(store.products);
                }
            }
        });
    };
    ReportService.prototype.updateCheapestPrices = function (products) {
        var _this = this;
        if (this.cheapestProductsFromAllStores.length === 0) {
            this.initialProductsForCheapestPrices(products);
        }
        else {
            products.forEach(function (product) {
                var foundProduct = false;
                for (var i = 0; i < _this.cheapestProductsFromAllStores.length && !foundProduct; i++) {
                    if (_this.cheapestProductsFromAllStores[i].name === product.name) {
                        foundProduct = true;
                        if (Number(_this.cheapestProductsFromAllStores[i].price) !== NaN
                            && Number(product.price) !== NaN) {
                            if (Number(product.price) < Number(_this.cheapestProductsFromAllStores[i].price)) {
                                _this.cheapestProductsFromAllStores[i].price = product.price;
                            }
                        }
                        else if (Number(_this.cheapestProductsFromAllStores[i].price) === NaN
                            && Number(product.price) !== NaN) {
                            _this.cheapestProductsFromAllStores[i].price = product.price;
                        }
                    }
                }
            });
        }
    };
    ReportService.prototype.initialProductsForCheapestPrices = function (products) {
        var _this = this;
        if (this.cheapestProductsFromAllStores.length === 0) {
            products.forEach(function (pr) {
                var newProduct = new product();
                newProduct.name = pr.name;
                newProduct.price = pr.price;
                _this.cheapestProductsFromAllStores.push(newProduct);
            });
        }
    };
    ReportService.prototype.parseStoresNames = function () {
        var _this = this;
        this.storesToCompare.forEach(function (comparedStore) {
            var FullstoreName = comparedStore.toString().split(',');
            var storeDTO = new StoreDTO();
            storeDTO.fullStoreName = comparedStore.toString();
            storeDTO.chainName = FullstoreName[0];
            storeDTO.storeName = FullstoreName[1];
            _this.stores.push(storeDTO);
        });
    };
    ReportService.prototype.calculateCheapestPrice = function () {
        var _this = this;
        this.stores.forEach(function (store) {
            if (store.productsTotalPrice < _this.totalCheapestPrice) {
                _this.totalCheapestPrice = store.productsTotalPrice;
            }
        });
    };
    ReportService.prototype.getReportOfStoreByFullName = function (storeName) {
        var storeReport = undefined;
        for (var i = 0; i < this.report.length && storeReport === undefined; i++) {
            if (this.report[i].storeName === storeName) {
                storeReport = this.report[i].storeReport;
            }
        }
        return storeReport;
    };
    ReportService.prototype.getReport = function (storesToCompare, products) {
        return this.$http.post("api/serverPriceCompare/getReport", {
            storesToCompare: storesToCompare,
            products: products
        }).then(function (data) {
            return data.data;
        });
    };
    return ReportService;
}());
ReportService.$inject = ['$http'];
app.service("reportService", ReportService);
//# sourceMappingURL=report-service.js.map