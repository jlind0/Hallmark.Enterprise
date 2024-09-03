interface KnockoutBindingHandlers {
    drag: KnockoutBindingHandler;
    drop: KnockoutBindingHandler;
}
var _dragged: any; 
ko.bindingHandlers.drag = {
    init: (element, valueAccessor, allBindingsAccessor, viewModel) => {
        var dragElement = $(element).draggable({
            helper: 'clone', revert: true, revertDuration: 0, cursor: "move",
            start: (event, ui) => {
                _dragged = ko.utils.unwrapObservable(valueAccessor().value);
            }
        });
        dragElement.disableSelection();
    }
}
ko.bindingHandlers.drop = {
    init: (element, valueAccessor, allBindingsAccessor, viewModel) => {
        $(element).droppable({
            drop: (event, ui) => valueAccessor().value(_dragged)
        });
    }
}