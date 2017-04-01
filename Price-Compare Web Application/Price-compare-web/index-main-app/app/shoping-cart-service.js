var ShopingCartService = (function () {
    function ShopingCartService($http) {
        this.$http = $http;
    }
    ShopingCartService.prototype.getChainNames = function () {
        return this.$http.get("api/serverPriceCompare/getChainNames").then(function (data) {
            return data.data;
        });
    };
    ShopingCartService.prototype.getStoreNames = function (fileIdentifiers, optionalAeraFilter) {
        var info = fileIdentifiers.DirName + ',' + fileIdentifiers.PartialFileName + ',' + optionalAeraFilter;
        return this.$http.get("api/serverPriceCompare/getStoreNames", {
            params: {
                info: info
            }
        }).then(function (data) {
            return data.data;
        });
    };
    ShopingCartService.prototype.getProductNames = function (fileIdentifiers) {
        var info = fileIdentifiers.DirName + ',' + fileIdentifiers.PartialFileName;
        return this.$http.get("api/serverPriceCompare/getProductItems", {
            params: {
                info: info
            }
        }).then(function (data) {
            return data.data;
        });
    };
    ShopingCartService.prototype.getProductItemInShopingCart = function (products, name) {
        for (var i = 0; i < products.length; i++) {
            if (products[i].name === name) {
                return products[i];
            }
        }
        return undefined;
    };
    ShopingCartService.prototype.getStringsArray = function (objectArray) {
        var stringArray = [];
        for (var i = 0; i < objectArray.length; i++) {
            stringArray.push(objectArray[i].toString());
        }
        return stringArray;
    };
    return ShopingCartService;
}());
ShopingCartService.$inject = ['$http'];
app.service("shopingCartService", ShopingCartService);
//# sourceMappingURL=shoping-cart-service.js.map