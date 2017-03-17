app.component("placeBid",
    {
        templateUrl: "components/Place-bid/place-bid-template.html",
        controller: PlaceBidController,
        bindings: {
            auctionId: "="
        }
    });