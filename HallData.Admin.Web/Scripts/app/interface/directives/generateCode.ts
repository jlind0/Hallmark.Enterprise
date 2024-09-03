module HallData.EMS.Interface {
	"use strict";

	export function generateCode(): ng.IDirective {
		var watchString = "interface.DisplayName";
		var jqInput = "#inputName";

		return {
			restrict: "A",
			link: ($scope: EMS.Interface.IInterfaceAddScope, element: JQuery, attrs: any) => {
				console.log("generateCodeDirective");
				$scope.$watch(watchString, function () {
					if ($(jqInput).hasClass("ng-pristine")) {
						$scope.interfaceObj.DisplayName = $scope.interfaceObj.Name;
					}
				});
			}
		};
	}
};