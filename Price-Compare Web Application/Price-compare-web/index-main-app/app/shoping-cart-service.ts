class ShopingCartService {

    constructor(private $http: ng.IHttpService) {
    }

    public getChainNames(): ng.IPromise<Object[]> {
        return this.$http.get("api/serverPriceCompare/getChainNames").then(data => {
            return data.data;
        });
    }

    public getStoreNames(fileIdentifiers: FileIdentifiers, optionalAeraFilter: string): ng.IPromise<Object[]> {
        let info = fileIdentifiers.DirName + ',' + fileIdentifiers.PartialFileName + ',' + optionalAeraFilter;

        return this.$http.get("api/serverPriceCompare/getStoreNames", {
            params: {
                info: info
            }
        }).then(data => {
            return data.data;
        });
    }

    public getProductNames(fileIdentifiers: FileIdentifiers): ng.IPromise<Object[]> {
        let info = fileIdentifiers.DirName + ',' + fileIdentifiers.PartialFileName;

        return this.$http.get("api/serverPriceCompare/getProductItems", {
            params: {
                info: info
            }
        }).then(data => {
            return data.data;
        });
    }

    public getProductItemInShopingCart(products: product[], name: string): product {
        for (let i = 0; i < products.length; i++) {
            if (products[i].name === name) {
                return products[i];
            }
        }

        return undefined;
    }

    public getStringsArray(objectArray: Object[]): string[]{
        let stringArray: string[] = [];
        for (let i = 0; i < objectArray.length; i++)
        {
            stringArray.push(objectArray[i].toString());
        }

        return stringArray;
    }
}
ShopingCartService.$inject = ['$http'];
app.service("shopingCartService", ShopingCartService);
