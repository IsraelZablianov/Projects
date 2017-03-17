var PriceCompareController = (function () {
    function PriceCompareController(reportService) {
        this.reportService = reportService;
        this.shopingCartItemsAmount = 0;
        this.products = [];
        this.storesToCompare = [];
    }
    return PriceCompareController;
}());
PriceCompareController.$inject = ['reportService'];
app.controller("PriceCompareController", PriceCompareController);
//# sourceMappingURL=price-compare-controller.js.map