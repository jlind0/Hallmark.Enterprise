module HallData.EMS.InterfaceAttribute {
	"use strict";

	export function formatAttributeName(): ng.IDirective {
		var watchString = "interfaceAttributeObj.Name";

		return {
			restrict: "A",
			link: ($scope: EMS.InterfaceAttribute.IInterfaceAttributeAddScope, element: JQuery, attrs: any) => {
				console.log("formatAttributeNameDirective");
				$scope.$watch(watchString, function () {
					if ($scope.interfaceAttributeObj.Name !== null && $scope.interfaceAttributeObj.Name !== undefined) {
						$scope.interfaceAttributeObj.Name = $scope.interfaceAttributeObj.Name.replace(/[\W_]/g, "");
					}
				});
			}
		};
	}
};