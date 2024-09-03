/// <reference path="../../libraries/jquery/jquery.d.ts" />
/// <reference path="../../libraries/knockout/knockout.d.ts" />
module HallData.UI.Grid {
    export class Page {
        constructor(public pageNumber: number, public isSelected: boolean, private grid: IGridBase) {
        }
        public select(): void {
            this.grid.currentPage(this.pageNumber);
            this.grid.load();
        }
    }
    export class Pager {
        public pages: KnockoutObservableArray<Page>;
        public isNextAvailable: KnockoutComputed<boolean>;
        public isPreviousAvailable: KnockoutComputed<boolean>;
        public isFirstAvailable: KnockoutComputed<boolean>;
        public isLastAvailable: KnockoutComputed<boolean>;
        public pageDisplayCount: number = 5;
        public pageCount: KnockoutComputed<number> = ko.computed(() => Math.ceil(this.grid.totalResultCount() / this.grid.currentPageSize()));
        constructor(private grid: IGridBase) {
            this.pages = ko.observableArray<Page>();
            this.isNextAvailable = ko.computed(() => (this.grid.currentPage() * this.grid.currentPageSize()) < this.grid.totalResultCount());
            this.isPreviousAvailable = ko.computed(() => this.grid.currentPage() > 1);
            this.isFirstAvailable = ko.computed(() => this.grid.currentPage() != 1);
            this.isLastAvailable = ko.computed(() => this.grid.currentPage() != Math.ceil(this.grid.totalResultCount() / this.grid.currentPageSize()));
        }
        public next(): void {
            this.grid.currentPage(this.grid.currentPage() + 1);
            this.grid.load();
        }
        public previous(): void {
            this.grid.currentPage(this.grid.currentPage() - 1);
            this.grid.load();
        }
        public first(): void {
            this.grid.currentPage(1);
            this.grid.load();
        }
        public last(): void {
            this.grid.currentPage(Math.ceil(this.grid.totalResultCount() / this.grid.currentPageSize()));
            this.grid.load();
        }
        public buildPages(): void {
            var pages = this.pageCount();
            var startPage = this.grid.currentPage() <= this.pageDisplayCount ? 1 : this.grid.currentPage() - this.pageDisplayCount + 1;
            this.pages.removeAll();
            var k = 1;
            for (var i = startPage; i <= pages && k <= this.pageDisplayCount; i++) {
                this.pages.push(new Page(i, i == this.grid.currentPage(), this.grid));
                k++;
            }
        }
    }
}