app.directive('fallbackSrc', () => {
    var fallbackSrc = {
        link: (scope, iElement, iAttrs) => {
            iElement.bind('error', function () {
                angular.element(this).attr("src", iAttrs.fallbackSrc);
            });
        }
    }
    return fallbackSrc;
});