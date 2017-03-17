storeSelectionModule.component("storeSelection", {
    templateUrl: "index-main-app/components/store-selection-form/store-selection.html",
    controller: StoreSelectionController,
    bindings: {
        products: "=",
        storesToCompare: "="
    }
});