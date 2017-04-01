class PriceCompareController {
    constructor(private reportService: ReportService) {
    }

    public shopingCartItemsAmount: number = 0;
    public products: product[] = [];
    storesToCompare: string[] = [];

}

PriceCompareController.$inject = ['reportService'];
app.controller("PriceCompareController", PriceCompareController); 
