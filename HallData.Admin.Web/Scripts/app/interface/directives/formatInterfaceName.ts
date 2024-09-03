module HallData.EMS.Interface {
	"use strict";

	export function formatInterfaceName(): ng.IDirective {
		var watchString = "interfaceObj.Name";

		return {
			restrict: "A",
			link: ($scope: EMS.Interface.IInterfaceAddScope, element: JQuery, attrs: any) => {
				console.log("formatInterfaceNameDirective");
				$scope.$watch(watchString, function () {
					if ($scope.interfaceObj.Name !== null && $scope.interfaceObj.Name !== undefined) {
						$scope.interfaceObj.Name = $scope.interfaceObj.Name.replace(/[\W_]/g, "");
					}
				});
			}
		};
	}
};