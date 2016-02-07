(function () {
    'use strict';
    app.controller('gridStyleController', ["$scope", "serverAddressService", GridStyleController]);

    function GridStyleController(scope, addressService) {
        scope.getRgbaColor = function (c) {
            return "rgba(" + c.red + "," + c.green + "," + c.blue + "," + c.alpha + ")";
        }

        scope.getBorder = function (o) {
            return o.size + "px " + o.style + " " + scope.getRgbaColor(o.color);
        }

        scope.getShadow = function (o) {
            var out = "";
            if (o.inset)
                out += "inset ";
            out += o.horizontal + "px ";
            out += o.vertical + "px ";
            out += o.blur + "px ";
            out += o.spread + "px ";
            out += scope.getRgbaColor(o.color);
            return out;
        }

        scope.tableStyle = function () {
            if (scope.inputConfiguration === null)
                return;

            var style = {};
            var settings = scope.inputConfiguration.gridSettings;
            var backgroundColor = scope.getRgbaColor(settings.color);
            var prefixes = ["", "-ms-", "-o-", "-moz-", "-webkit-"];
            style["border-spacing"] = settings.cellSpacing + "px";
            style["background"] = backgroundColor;
            if (settings.showImageBackground) {
                style["background-image"] = "url(" + addressService.serverHost + "media/" + scope.inputConfiguration.trimmedName + "/" + settings.mediaItem.fileName + ")";
                for (var i = 0; i < prefixes.length; i++) {
                    style[prefixes[i] + "background-size"] = settings.mediaItem.size;
                }
                style["background-repeat"] = settings.mediaItem.repeat;
                style["background-position"] = settings.mediaItem.positionLeft + " " + settings.mediaItem.positionTop;
            }
            return style;
        }

        scope.cellStyle = function (action) {
            if (!action.isVisible)
                return;

            var style = {};
            var settings = scope.inputConfiguration.gridSettings;
            var heightSpacing = 100 / (settings.cellSpacing * (settings.rows * 2));
            var prefixes = ["", "-ms-", "-o-", "-moz-", "-webkit-"];

            style["background-color"] = action.color;
            style["color"] = action.textColor;
            style["width"] = 100 / settings.columns + '%';
            style["height"] = ((100 / settings.rows) - heightSpacing) + '%';
            style["opacity"] = action.opacity;

            if (settings.showBorder)
                style["border"] = scope.getBorder(settings.cellBorder);

            if (settings.showShadow)
                style["box-shadow"] = scope.getShadow(settings.cellShadow);
            
            if (settings.hideOverflowText) {
                for (var i = 0; i < prefixes.length; i++) {
                    style[prefixes[i] + "text-overflow"] = "clip";
                }
                style["overflow"] = "hidden";
                style["white-space"] =  "nowrap";
            }
            if (action.mediaItem.fileName.length > 0) {
                style["background-image"] = "url(" + addressService.serverHost + "media/" + scope.inputConfiguration.trimmedName + "/" + action.mediaItem.fileName + ")";
                for (var i = 0; i < prefixes.length; i++) {
                    style[prefixes[i] + "background-size"] = action.mediaItem.size;
                }
                style["background-repeat"] = action.mediaItem.repeat;
                style["background-position"] = action.mediaItem.positionLeft + " " + action.mediaItem.positionTop;
            }
            return style;
        }
    }

})();