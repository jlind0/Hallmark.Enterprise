// CONSTANTS
// ---------

var GROWL_NOTICE_TITLE = "Success!";
var GROWL_ERROR_TITLE = "Error!";
var GROWL_WARNING_TITLE = "Note:";
var GROWL_DEFAULT_TITLE = "???";


// GROWL NOTIFICATIONS
// -------------------

function growl(type, title, message) {
	console.log("growl");

	switch(type) {
		case "notice":
			$.growl.notice({
				title: title,
				message: message,
				size: "medium"
			});
			break;

		case "error":
			$.growl.error({
				title: title,
				message: message,
				size: "large"
			});
			break;

		case "warning":
			$.growl.warning({
				title: title,
				message: message,
				size: "medium"
			});
			break;

		default:
			$.growl({
				title: title,
				message: message,
				size: "medium"
			});
			break;
	}
}