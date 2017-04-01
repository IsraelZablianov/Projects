app.directive('fallbackSrc', function () {
    var fallbackSrc = {
        link: function (scope, iElement, iAttrs) {
            iElement.bind('error', function () {
                angular.element(this).attr("src", iAttrs.fallbackSrc);
            });
        }
    };
    return fallbackSrc;
});
//# sourceMappingURL=fallback-src.js.map