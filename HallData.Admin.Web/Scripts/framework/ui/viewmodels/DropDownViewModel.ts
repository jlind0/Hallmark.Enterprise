module HallData.UI.ViewModels {
    export enum DropDownState {
        Loading,
        Loaded,
        Unloading,
        Unloaded,
        Error
    }
    enum WorkerQueueInstructions {
        Load,
        Unload
    }
    interface WorkerQueueInstruction<TLoadParameter> {
        instruction: WorkerQueueInstructions;
        loadParam?: TLoadParameter;
    }
    export interface ILoadParameterModel<TView> {
        valueSelector?: ($: TView) => boolean;
        defaultValueSelector?: ($: TView) => boolean;
    }
    export class DropDownViewModel<TView, TLoadParameter extends ILoadParameterModel<any>, TService extends Service.IService>{
        public items: KnockoutObservableArray<TView> = ko.observableArray<TView>();
        public selectedItem: KnockoutObservable<TView> = ko.observable<TView>();
        public dropDownState: KnockoutObservable<DropDownState> = ko.observable(DropDownState.Loading);
        public isLoading: KnockoutComputed<boolean> = ko.computed(() => this.dropDownState() == DropDownState.Loading);
        public isLoaded: KnockoutComputed<boolean> = ko.computed(() => this.dropDownState() == DropDownState.Loaded);
        public isUnloading: KnockoutComputed<boolean> = ko.computed(() => this.dropDownState() == DropDownState.Unloading);
        public isUnloaded: KnockoutComputed<boolean> = ko.computed(() => this.dropDownState() == DropDownState.Unloaded);
        public isError: KnockoutComputed<boolean> = ko.computed(() => this.dropDownState() == DropDownState.Error);
        public errorMessage: KnockoutObservable<string> = ko.observable<string>();
        private workerQueue: AsyncQueue<WorkerQueueInstruction<TLoadParameter>> = async.queue((s, c) => {
            var i = <WorkerQueueInstruction<TLoadParameter>>s;
            switch (i.instruction) {
                case WorkerQueueInstructions.Load:
                    this.doLoad(i.loadParam).then(() => {
                        this.dropDownState(DropDownState.Loaded);
                        c();
                    },() => {
                        this.dropDownState(DropDownState.Error);
                        if (Util.String.isNullOrWhitespace(this.errorMessage()))
                            this.errorMessage("Dropdown load failed");
                        c();
                    });
                    break;
                case WorkerQueueInstructions.Unload:
                    this.doUnload().then(() => {
                        this.dropDownState(DropDownState.Unloaded);
                        c();
                    },() => {
                        this.dropDownState(DropDownState.Error);
                        if (Util.String.isNullOrWhitespace(this.errorMessage()))
                            this.errorMessage("Dropdown unload failed");
                    });
                    break;
            }
        }, 1);
        constructor(protected service: TService, public displayMemberPath : string) {
        }
        public load(param?: TLoadParameter): JQueryPromise<TView[]> {
            var d = $.Deferred<TView[]>();
            if (this.isLoaded())
                this.unload();
            this.workerQueue.push({ instruction: WorkerQueueInstructions.Load, loadParam: param },() => {
                if (this.isError())
                    d.fail();
                else
                    d.resolve(this.items());
            });
            return d.promise();
        }
        private doLoad(param?: TLoadParameter): JQueryPromise<void> {
            this.dropDownState(DropDownState.Loading);
            return this.getItems(param).then(items => {
                for (var i = 0; i < items.length; i++) {
                    this.items.push(items[i]);
                }
                var typedParam = <ILoadParameterModel<TView>>param;
                var data = Enumerable.From(this.items());
                var selectItem: TView = null;
                if (!Util.Obj.isNullOrUndefined(param)) {
                    if (!Util.Obj.isNullOrUndefined(param.valueSelector))
                        selectItem = data.FirstOrDefault(null, typedParam.valueSelector);
                    if (Util.Obj.isNullOrUndefined(selectItem) && !Util.Obj.isNullOrUndefined(param.defaultValueSelector))
                        selectItem = data.FirstOrDefault(null, typedParam.defaultValueSelector);
                }
                if (Util.Obj.isNullOrUndefined(selectItem))
                    selectItem = data.FirstOrDefault(null);
                this.selectedItem(selectItem);
            }, error => {
                if(!Util.Obj.isNullOrUndefined(error) && !Util.Obj.isNullOrUndefined(error.responseText))
                    this.errorMessage(error.responseText);
            });
        }
        protected getItems(param?: TLoadParameter): JQueryPromise<TView[]> {
            var d = $.Deferred<TView[]>();
            d.fail();
            return d.promise();
        }
        public unload(): JQueryPromise<void> {
            var d = $.Deferred<void>();
            this.workerQueue.push({ instruction: WorkerQueueInstructions.Unload },() => {
                if (this.isError())
                    d.fail();
                else
                    d.resolve();
            });
            return d.promise();
        }
        protected doUnload(): JQueryPromise<void> {
            this.dropDownState(DropDownState.Unloading);
            var d = $.Deferred<void>();
            this.items.removeAll();
            this.selectedItem(null);
            d.resolve();
            return d.promise();
        }
    }
}