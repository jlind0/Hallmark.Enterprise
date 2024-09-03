interface KnockoutBindingHandlers {
    multiselect: KnockoutBindingHandler
}
ko.bindingHandlers.multiselect = {
    init: element => {
        $(element).bind("multiselectclick",() => {
            $.data(element, "donotrefresh", true);
        });
    },
    update: element => {
        var doNotRefresh = !!($.data(element, 'donotrefresh'));
        if (!doNotRefresh) { $(element).multiselect("refresh"); }
        $.data(element, 'donotrefresh', false);
    }
}