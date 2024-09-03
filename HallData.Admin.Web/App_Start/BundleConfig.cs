using System.Web;
using System.Web.Optimization;

namespace HallData.Admin.Web
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			// INTERNAL CODE

			bundles.Add(new ScriptBundle("~/bundles/framework").Include(
				"~/Scripts/framework/Dictionary.js",
				"~/Scripts/framework/Events.js",
				"~/Scripts/framework/Util.js",
				"~/Scripts/framework/Services/Interfaces.js",
				"~/Scripts/framework/Services/DataClasses.js",
				"~/Scripts/framework/Services/Services.js",
				"~/Scripts/framework/authentication/service/AuthenticationService.js",
				"~/Scripts/framework/authentication/provider/AuthenticationProvider.js",
				"~/Scripts/framework/controllers/ControllerBase.js",
				"~/Scripts/framework/Grid/Interfaces.js",
				"~/Scripts/framework/Grid/Filter.js",
				"~/Scripts/framework/Grid/Column.js",
				"~/Scripts/framework/Grid/Pager.js",
				"~/Scripts/framework/Grid/GridBase.js",
				"~/Scripts/framework/Grid/ViewGrid.js",
				"~/Scripts/framework/ui/data/filteroperationoption/interfaces/FilterOperationOption.js",
				"~/Scripts/framework/ui/data/applicationviewcolumn/interfaces/ApplicationViewColumn.js",
				"~/Scripts/framework/ui/services/personalization/PersonalizationService.js",
				"~/Scripts/framework/KnockoutDateBinding.js",
				"~/Scripts/framework/KnockoutDragAndDropBinding.js",
				"~/Scripts/framework/KnockoutMultiselectBinding.js",
				"~/Scripts/shared/interfaces/IKeyValuePair.js"));

			bundles.Add(new ScriptBundle("~/bundles/app/shared").Include(
				"~/Scripts/app/services/shared/classes/EnumerationsService.js",
				"~/Scripts/app/services/shared/classes/EnumerationsServiceFactory.js",
				"~/Scripts/app/shared/enumerable/ControllerAction.js",
				"~/Scripts/app/shared/enumerable/ControllerState.js",
				"~/Scripts/app/shared/controllers/ViewController.js",
				"~/Scripts/app/shared/services/RouteDictionaryService.js",
				"~/Scripts/app/shared/services/PerspectiveDictionaryService.js",
				"~/Scripts/app/shared/services/RedirectService.js",
				"~/Scripts/app/shared/controllers/SharedController.js",
				"~/Scripts/app/shared/modules/Globals.js"));

			bundles.Add(new ScriptBundle("~/bundles/app/menu").Include(
				"~/Scripts/app/menu/controllers/MenuController.js",
				"~/Scripts/app/menu/initializers/Menu.js"));

			bundles.Add(new ScriptBundle("~/bundles/app/perspectives").Include(
				"~/Scripts/app/perspectives/controllers/PerspectivesController.js",
				"~/Scripts/app/perspectives/initializers/Perspectives.js"));

			bundles.Add(new ScriptBundle("~/bundles/hdsScripts").Include(
				"~/Scripts/app/common.js"));

			// EXTERNAL LIBRARIES

			bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
				"~/Scripts/libraries/knockout/knockout-3.3.0.js"));

			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
				"~/Scripts/libraries/angular/angular.d.js",
				"~/Scripts/libraries/angular/angular-messages.js",
				"~/Scripts/libraries/angular-xeditable/xeditable.js",
				"~/Scripts/libraries/angular-ui-select/select.js",
				"~/Scripts/libraries/angular-multi-select/isteven-multi-select.js",
				"~/Scripts/libraries/angular/angular-multiselect.js"));

			bundles.Add(new ScriptBundle("~/bundles/linq").Include(
				"~/Scripts/libraries/linq/linq.js"));

			bundles.Add(new ScriptBundle("~/bundles/gridRequirements").Include(
				"~/Scripts/libraries/async/async.js"));

			bundles.Add(new ScriptBundle("~/bundles/base-js-1").Include(
				"~/Content/plugins/pace/pace.js",
				"~/Content/plugins/jquery/jquery-1.9.1.js",
				"~/Content/plugins/jquery/jquery-migrate-1.1.0.js",
				"~/Content/plugins/jquery-ui/ui/jquery-ui.js",
				"~/Content/plugins/bootstrap/js/bootstrap.js",
				"~/Content/plugins/growl/js/jquery.growl.js",
				"~/Content/plugins/bootstrap-multiselect/js/bootstrap-multiselect.js"));

			bundles.Add(new ScriptBundle("~/bundles/crossbrowserjs").Include(
				"~/Content/crossbrowserjs/html5shiv.js",
				"~/Content/crossbrowserjs/respond.min.js",
				"~/Content/crossbrowserjs/excanvas.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/base-js-2").Include(
				"~/Content/plugins/slimscroll/jquery.slimscroll.js",
				"~/Content/plugins/jquery-cookie/jquery.cookie.js",
				"~/Content/plugins/select2/dist/js/select2.js"));

			bundles.Add(new ScriptBundle("~/bundles/backbone").Include(
				"~/Scripts/libraries/underscore/underscore.js",
				"~/Scripts/libraries/backbone/backbone.js",
				"~/Scripts/libraries/backbone/backbone.babysitter.js",
				"~/Scripts/libraries/frameworks/backbone.wreqr.js",
				"~/Scripts/libraries/frameworks/backbone.marionette.js"));


			// STYLES

			bundles.Add(new StyleBundle("~/bundles/css").Include(
				"~/Content/plugins/jquery-ui/themes/base/minified/jquery-ui.min.css",
				"~/Content/plugins/bootstrap/css/bootstrap.css",
				"~/Content/css/animate.css",
				"~/Content/css/ui.multiselect.standalone.css",
				"~/Content/css/style.css",
				"~/Content/css/style-responsive.css",
				"~/Content/css/theme/hds.css",
				"~/Content/plugins/growl/css/jquery.growl.css",
				"~/Content/plugins/select2/dist/css/select2.css",
				"~/Content/css/hds-icons.css",
				"~/Content/plugins/simple-line-icons/simple-line-icons.css",
				"~/Content/plugins/ionicons/css/ionicons.css",
				"~/Scripts/libraries/angular-multi-select/isteven-multi-select.css",
				"~/Scripts/libraries/angular-ui-select/select.css"));

			BundleTable.EnableOptimizations = false;
		}
	}
}
