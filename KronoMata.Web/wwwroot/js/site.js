// Override the jsGrid buttons with Font Awesome Icons. Font Awesome and jQuery should
// be included in styles and scripts.
$(function () {
    // override the styles for the editing buttons. notice the 'fa' before the
    // custom classes.
    jsGrid.ControlField.prototype.buttonClass = "grid-button";
    jsGrid.ControlField.prototype.editButtonClass = "fa grid-button-edit";
    jsGrid.ControlField.prototype.deleteButtonClass = "fa grid-button-delete";
    jsGrid.ControlField.prototype.insertButtonClass = "fa grid-button-save";
    jsGrid.ControlField.prototype.insertModeButtonClass = "fa grid-button-insert";
    jsGrid.ControlField.prototype.updateButtonClass = "fa grid-button-save";
    jsGrid.ControlField.prototype.cancelEditButtonClass = "fa grid-button-cancel";
    jsGrid.ControlField.prototype.cancelInsertButtonClass = "fa grid-button-cancel";

    jsGrid.Grid.prototype.cellClass = "compact-jsgrid-cell";

    // override the buttons; use <button type="reset" instead of input type="button" so we can use
    // a pseudo-element (::after) in the style sheet to set the icon.
    jsGrid.ControlField.prototype._createGridButton = function (cls, tooltip, clickHandler) {
        var grid = this._grid;

        return $("<button>").addClass(this.buttonClass)
            .addClass(cls)
            .attr({
                type: "reset",
                title: tooltip
            })
            .on("click", function (e) {
                clickHandler(grid, e);
            });
    }
});

// utility method for geting ajax data
function GetData(url) {
    var resp = [];
    $.ajax({
        type: "GET",
        url: url,
        async: false,
        contentType: "application/json",
        success: function (data) {
            resp.push(data);
        },
        error: function (req, status, error) {
            // TODO: do something with error
            alert("error");
        }
    });
    return resp;
}
