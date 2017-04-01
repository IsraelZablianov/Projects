class AuctionsController {
    currentCategory: Category;
    userName: string = window.localStorage.getItem('UserName');

    public static $inject: string[] = ['routingService', 'auctionsService', 'categoriesService', '$uibModal', '$scope', '$window'];

    constructor(private routingService: RoutingService,
        private auctionsService: AuctionsService,
        private categoriesService: CategoriesService,
        private $uibModal,
        private $scope,
        private $window: ng.IWindowService) {

        routingService.register({
            paramName: "category",
            callback: (categoryId) => {
                if (this.categoriesService.categories.length === 0) {
                    this.categoriesService.initCategories()
                        .then(() => { this.matchCategory(parseInt(categoryId)); });
                }
                else { this.matchCategory(parseInt(categoryId)); }  
            }

        });     
    }

    public onCategoryChange(category: Category) {
        if (category === undefined) {
            this.currentCategory = undefined;
            this.routingService.clearUrl();
        } else {
            this.routingService.moveToUrl("category", category.id.toString());
        }

    }

    public refreshPage() {
        this.$window.location.reload();
    }

    public openSellPopup() {
        var modalInstance = this.$uibModal.open({
            templateUrl: 'components/Sell-popup/sell-popup-template.html',
            controller: SellPopupController,
            controllerAs: '$ctrl',
            size: 'sell-popup-size'
        });

    }

    public openPlaceBid(auction: Auction) {
        var modalInstance = this.$uibModal.open({
            templateUrl: "components/Place-bid/place-bid-template.html",
            controller: PlaceBidController,
            controllerAs: '$ctrl',
            resolve: {
                auction: () => auction
            },
            size: 'place-bid-size'
        });

    }

    public logout(): void {
        this.$window.localStorage.setItem('Authorization', '');
        this.refreshPage();
    }

    public areAuctionsAvalable() {
        if (this.currentCategory === undefined) return this.auctionsService.auctions.length !== 0;
        for (var auction of this.auctionsService.auctions) {
            if (auction.category.id === this.currentCategory.id) return true;
        }
        return false;
    }

    private routingServiceCallback(categoryId: string) {
        if (this.categoriesService.categories.length === 0) {
            this.categoriesService.initCategories()
                .then(() => {this.matchCategory(parseInt(categoryId));});
        }
        else {this.matchCategory(parseInt(categoryId));}  
    }

    private matchCategory(categoryId:number) {
        for (var category of this.categoriesService.categories) {
            if (category.id === categoryId) {
                this.currentCategory = { id: category.id, name: category.name };
            }
        }
    }

}
