! function (a) {
	"use strict";
	var b = function (a, c) {
		var d = /[^\w\-\.:]/.test(a) ? new Function(b.arg + ",tmpl", "var _e=tmpl.encode" + b.helper + ",_s='" + a.replace(b.regexp, b.func) + "';return _s;") : b.cache[a] = b.cache[a] || b(b.load(a));
		return c ? d(c, b) : function (a) {
			return d(a, b)
		}
	};
	b.cache = {}, b.load = function (a) {
		return document.getElementById(a).innerHTML
	}, b.regexp = /([\s'\\])(?!(?:[^{]|\{(?!%))*%\})|(?:\{%(=|#)([\s\S]+?)%\})|(\{%)|(%\})/g, b.func = function (a, b, c, d, e, f) {
		return b ? {
			"\n": "\\n",
			"\r": "\\r",
			"	": "\\t",
			" ": " "
		}[b] || "\\" + b : c ? "=" === c ? "'+_e(" + d + ")+'" : "'+(" + d + "==null?'':" + d + ")+'" : e ? "';" : f ? "_s+='" : void 0
	}, b.encReg = /[<>&"'\x00]/g, b.encMap = {
		"<": "&lt;",
		">": "&gt;",
		"&": "&amp;",
		'"': "&quot;",
		"'": "&#39;"
	}, b.encode = function (a) {
		return (null == a ? "" : "" + a).replace(b.encReg, function (a) {
			return b.encMap[a] || ""
		})
	}, b.arg = "o", b.helper = ",print=function(s,e){_s+=e?(s==null?'':s):_e(s);},include=function(s,d){_s+=tmpl(s,d);}", "function" == typeof define && define.amd ? define(function () {
		return b
	}) : a.tmpl = b
}(this);
