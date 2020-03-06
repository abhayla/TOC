(function (t, e, n, r) { function i(r) { if (!n[r]) { if (!e[r]) { if (t) return t(r); throw Error("Cannot find module '" + r + "'") } var o = n[r] = { exports: {} }; e[r][0](function (t) { var n = e[r][1][t]; return i(n ? n : t) }, o, o.exports) } return n[r].exports } for (var o = 0; r.length > o; o++)i(r[o]); return i })("undefined" != typeof require && require, { 1: [function (t, e) { var n, r, i, o, s, u = [].indexOf || function (t) { for (var e = 0, n = this.length; n > e; e++)if (e in this && this[e] === t) return e; return -1 }; s = t("../lib/lodash.js"), o = t("./helpers"), r = t("./context"), i = {}, i.render = function (t, e, n, u) { var l, a; return null == e && (e = []), null == n && (n = {}), null == u && (u = {}), l = u.debug && console ? o.consoleLogger : o.nullLogger, l("Transparency.render:", t, e, n, u), t ? (s.isArray(e) || (e = [e]), t = (a = o.data(t)).context || (a.context = new r(t, i)), t.render(e, n, u).el) : void 0 }, i.matcher = function (t, e) { return t.el.id === e || u.call(t.classNames, e) >= 0 || t.el.name === e || t.el.getAttribute("data-bind") === e }, i.clone = function (t) { return n(t).clone()[0] }, i.jQueryPlugin = o.chainable(function (t, e, n) { var r, o, s, u; for (u = [], o = 0, s = this.length; s > o; o++)r = this[o], u.push(i.render(r, t, e, n)); return u }), ("undefined" != typeof jQuery && null !== jQuery || "undefined" != typeof Zepto && null !== Zepto) && (n = jQuery || Zepto, null != n && (n.fn.render = i.jQueryPlugin)), (e !== void 0 && null !== e ? e.exports : void 0) && (e.exports = i), "undefined" != typeof window && null !== window && (window.Transparency = i), ("undefined" != typeof define && null !== define ? define.amd : void 0) && define(function () { return i }) }, { "../lib/lodash.js": 2, "./helpers": 3, "./context": 4 }], 2: [function (t, e, n) { var r = {}; r.toString = Object.prototype.toString, r.toArray = function (t) { for (var e = Array(t.length), n = 0; t.length > n; n++)e[n] = t[n]; return e }, r.isString = function (t) { return "[object String]" == r.toString.call(t) }, r.isNumber = function (t) { return "[object Number]" == r.toString.call(t) }, r.isArray = Array.isArray || function (t) { return "[object Array]" === r.toString.call(t) }, r.isDate = function (t) { return "[object Date]" === r.toString.call(t) }, r.isElement = function (t) { return !(!t || 1 !== t.nodeType) }, r.isPlainValue = function (t) { var e; return e = typeof t, "object" !== e && "function" !== e || n.isDate(t) }, r.isBoolean = function (t) { return t === !0 || t === !1 }, e.exports = r }, {}], 3: [function (t, e, n) { var r, i, o, s; r = t("./elementFactory"), n.before = function (t) { return function (e) { return function () { return t.apply(this, arguments), e.apply(this, arguments) } } }, n.after = function (t) { return function (e) { return function () { return e.apply(this, arguments), t.apply(this, arguments) } } }, n.chainable = n.after(function () { return this }), n.onlyWith$ = function (t) { return "undefined" != typeof jQuery && null !== jQuery || "undefined" != typeof Zepto && null !== Zepto ? function () { return t(arguments) }(jQuery || Zepto) : void 0 }, n.getElements = function (t) { var e; return e = [], s(t, e), e }, s = function (t, e) { var i, o; for (i = t.firstChild, o = []; i;)i.nodeType === n.ELEMENT_NODE && (e.push(new r.createElement(i)), s(i, e)), o.push(i = i.nextSibling); return o }, n.ELEMENT_NODE = 1, n.TEXT_NODE = 3, o = function () { return "<:nav></:nav>" !== document.createElement("nav").cloneNode(!0).outerHTML }, n.cloneNode = "undefined" == typeof document || null === document || o() ? function (t) { return t.cloneNode(!0) } : function (t) { var e, r, o, s, u; if (e = Transparency.clone(t), e.nodeType === n.ELEMENT_NODE) for (e.removeAttribute(i), u = e.getElementsByTagName("*"), o = 0, s = u.length; s > o; o++)r = u[o], r.removeAttribute(i); return e }, i = "transparency", n.data = function (t) { return t[i] || (t[i] = {}) }, n.nullLogger = function () { }, n.consoleLogger = function () { return console.log(arguments) }, n.log = n.nullLogger }, { "./elementFactory": 5 }], 4: [function (t, e) { var n, r, i, o, s, u, l; l = t("./helpers"), o = l.before, i = l.after, s = l.chainable, u = l.cloneNode, r = t("./instance"), e.exports = n = function () { function t(t, e) { this.el = t, this.Transparency = e, this.template = u(this.el), this.instances = [new r(this.el, this.Transparency)], this.instanceCache = [] } var e, n; return n = s(function () { return this.parent = this.el.parentNode, this.parent ? (this.nextSibling = this.el.nextSibling, this.parent.removeChild(this.el)) : void 0 }), e = s(function () { return this.parent ? this.nextSibling ? this.parent.insertBefore(this.el, this.nextSibling) : this.parent.appendChild(this.el) : void 0 }), t.prototype.render = o(n)(i(e)(s(function (t, e, n) { for (var i, o, s, l, a, h, c; t.length < this.instances.length;)this.instanceCache.push(this.instances.pop().remove()); for (; t.length > this.instances.length;)s = this.instanceCache.pop() || new r(u(this.template), this.Transparency), this.instances.push(s.appendTo(this.el)); for (c = [], o = a = 0, h = t.length; h > a; o = ++a)l = t[o], s = this.instances[o], i = [], c.push(s.prepare(l, i).renderValues(l, i).renderDirectives(l, o, e).renderChildren(l, i, e, n)); return c }))), t }() }, { "./helpers": 3, "./instance": 6 }], 5: [function (t, e) { var n, r, i, o, s, u, l, a, h, c, p, f, d, m, y, g, v = {}.hasOwnProperty, b = function (t, e) { function n() { this.constructor = t } for (var r in e) v.call(e, r) && (t[r] = e[r]); return n.prototype = e.prototype, t.prototype = new n, t.__super__ = e.prototype, t }; p = t("../lib/lodash.js"), c = t("./helpers"), n = t("./attributeFactory"), e.exports = o = { Elements: { input: {} }, createElement: function (t) { var e, n; return e = "input" === (n = t.nodeName.toLowerCase()) ? o.Elements[n][t.type.toLowerCase()] || s : o.Elements[n] || i, new e(t) } }, i = function () { function t(t) { this.el = t, this.attributes = {}, this.childNodes = p.toArray(this.el.childNodes), this.nodeName = this.el.nodeName.toLowerCase(), this.classNames = this.el.className.split(" "), this.originalAttributes = {} } return t.prototype.empty = function () { for (var t; t = this.el.firstChild;)this.el.removeChild(t); return this }, t.prototype.reset = function () { var t, e, n, r; n = this.attributes, r = []; for (e in n) t = n[e], r.push(t.set(t.templateValue)); return r }, t.prototype.render = function (t) { return this.attr("text", t) }, t.prototype.attr = function (t, e) { var r, i; return r = (i = this.attributes)[t] || (i[t] = n.createAttribute(this.el, t, e)), null != e && r.set(e), r }, t.prototype.renderDirectives = function (t, e, n) { var r, i, o, s; s = []; for (i in n) v.call(n, i) && (r = n[i], "function" == typeof r && (o = r.call(t, { element: this.el, index: e, value: this.attr(i).templateValue }), null != o ? s.push(this.attr(i, o)) : s.push(void 0))); return s }, t }(), l = function (t) { function e(t) { e.__super__.constructor.call(this, t), this.elements = c.getElements(t) } return b(e, t), o.Elements.select = e, e.prototype.render = function (t) { var e, n, r, i, o; for (t = "" + t, i = this.elements, o = [], n = 0, r = i.length; r > n; n++)e = i[n], "option" === e.nodeName && o.push(e.attr("selected", e.el.value === t)); return o }, e }(i), h = function (t) { function e() { return f = e.__super__.constructor.apply(this, arguments) } var n, r, i, s; for (b(e, t), n = ["area", "base", "br", "col", "command", "embed", "hr", "img", "input", "keygen", "link", "meta", "param", "source", "track", "wbr"], i = 0, s = n.length; s > i; i++)r = n[i], o.Elements[r] = e; return e.prototype.attr = function (t, n) { return "text" !== t && "html" !== t ? e.__super__.attr.call(this, t, n) : void 0 }, e }(i), s = function (t) { function e() { return d = e.__super__.constructor.apply(this, arguments) } return b(e, t), e.prototype.render = function (t) { return this.attr("value", t) }, e }(h), a = function (t) { function e() { return m = e.__super__.constructor.apply(this, arguments) } return b(e, t), o.Elements.textarea = e, e }(s), r = function (t) { function e() { return y = e.__super__.constructor.apply(this, arguments) } return b(e, t), o.Elements.input.checkbox = e, e.prototype.render = function (t) { return this.attr("checked", Boolean(t)) }, e }(s), u = function (t) { function e() { return g = e.__super__.constructor.apply(this, arguments) } return b(e, t), o.Elements.input.radio = e, e }(r) }, { "../lib/lodash.js": 2, "./helpers": 3, "./attributeFactory": 7 }], 6: [function (t, e) { var n, r, i, o, s = {}.hasOwnProperty; o = t("../lib/lodash.js"), r = (i = t("./helpers")).chainable, e.exports = n = function () { function t(t, e) { this.Transparency = e, this.queryCache = {}, this.childNodes = o.toArray(t.childNodes), this.elements = i.getElements(t) } return t.prototype.remove = r(function () { var t, e, n, r, i; for (r = this.childNodes, i = [], e = 0, n = r.length; n > e; e++)t = r[e], i.push(t.parentNode.removeChild(t)); return i }), t.prototype.appendTo = r(function (t) { var e, n, r, i, o; for (i = this.childNodes, o = [], n = 0, r = i.length; r > n; n++)e = i[n], o.push(t.appendChild(e)); return o }), t.prototype.prepare = r(function (t) { var e, n, r, o, s; for (o = this.elements, s = [], n = 0, r = o.length; r > n; n++)e = o[n], e.reset(), s.push(i.data(e.el).model = t); return s }), t.prototype.renderValues = r(function (t, e) { var n, r, i, u; if (o.isElement(t) && (n = this.elements[0])) return n.empty().el.appendChild(t); if ("object" == typeof t) { u = []; for (r in t) s.call(t, r) && (i = t[r], null != i && (o.isString(i) || o.isNumber(i) || o.isBoolean(i) || o.isDate(i) ? u.push(function () { var t, e, o, s; for (o = this.matchingElements(r), s = [], t = 0, e = o.length; e > t; t++)n = o[t], s.push(n.render(i)); return s }.call(this)) : "object" == typeof i ? u.push(e.push(r)) : u.push(void 0))); return u } }), t.prototype.renderDirectives = r(function (t, e, n) { var r, i, o, u; u = []; for (o in n) s.call(n, o) && (r = n[o], "object" == typeof r && ("object" != typeof t && (t = { value: t }), u.push(function () { var n, s, u, l; for (u = this.matchingElements(o), l = [], n = 0, s = u.length; s > n; n++)i = u[n], l.push(i.renderDirectives(t, e, r)); return l }.call(this)))); return u }), t.prototype.renderChildren = r(function (t, e, n, r) { var i, o, s, u, l; for (l = [], s = 0, u = e.length; u > s; s++)o = e[s], l.push(function () { var e, s, u, l; for (u = this.matchingElements(o), l = [], e = 0, s = u.length; s > e; e++)i = u[e], l.push(this.Transparency.render(i.el, t[o], n[o], r)); return l }.call(this)); return l }), t.prototype.matchingElements = function (t) { var e, n, r; return n = (r = this.queryCache)[t] || (r[t] = function () { var n, r, i, o; for (i = this.elements, o = [], n = 0, r = i.length; r > n; n++)e = i[n], this.Transparency.matcher(e, t) && o.push(e); return o }.call(this)), i.log("Matching elements for '" + t + "':", n), n }, t }() }, { "../lib/lodash.js": 2, "./helpers": 3 }], 7: [function (t, e) { var n, r, i, o, s, u, l, a, h = {}.hasOwnProperty, c = function (t, e) { function n() { this.constructor = t } for (var r in e) h.call(e, r) && (t[r] = e[r]); return n.prototype = e.prototype, t.prototype = new n, t.__super__ = e.prototype, t }; a = t("../lib/lodash"), l = t("./helpers"), e.exports = r = { Attributes: {}, createAttribute: function (t, e) { var i; return i = r.Attributes[e] || n, new i(t, e) } }, n = function () { function t(t, e) { this.el = t, this.name = e, this.templateValue = this.el.getAttribute(this.name) || "" } return t.prototype.set = function (t) { return this.el[this.name] = t, this.el.setAttribute(this.name, "" + t) }, t }(), i = function (t) { function e(t, e) { this.el = t, this.name = e, this.templateValue = this.el.getAttribute(this.name) || !1 } var n, i, o, s; for (c(e, t), n = ["hidden", "async", "defer", "autofocus", "formnovalidate", "disabled", "autofocus", "formnovalidate", "multiple", "readonly", "required", "checked", "scoped", "reversed", "selected", "loop", "muted", "autoplay", "controls", "seamless", "default", "ismap", "novalidate", "open", "typemustmatch", "truespeed"], o = 0, s = n.length; s > o; o++)i = n[o], r.Attributes[i] = e; return e.prototype.set = function (t) { return this.el[this.name] = t, t ? this.el.setAttribute(this.name, this.name) : this.el.removeAttribute(this.name) }, e }(n), u = function (t) { function e(t, e) { var n; this.el = t, this.name = e, this.templateValue = function () { var t, e, r, i; for (r = this.el.childNodes, i = [], t = 0, e = r.length; e > t; t++)n = r[t], n.nodeType === l.TEXT_NODE && i.push(n.nodeValue); return i }.call(this).join(""), this.children = a.toArray(this.el.children), (this.textNode = this.el.firstChild) ? this.textNode.nodeType !== l.TEXT_NODE && (this.textNode = this.el.insertBefore(this.el.ownerDocument.createTextNode(""), this.textNode)) : this.el.appendChild(this.textNode = this.el.ownerDocument.createTextNode("")) } return c(e, t), r.Attributes.text = e, e.prototype.set = function (t) { for (var e, n, r, i, o; e = this.el.firstChild;)this.el.removeChild(e); for (this.textNode.nodeValue = t, this.el.appendChild(this.textNode), i = this.children, o = [], n = 0, r = i.length; r > n; n++)e = i[n], o.push(this.el.appendChild(e)); return o }, e }(n), s = function (t) { function e(t) { this.el = t, this.templateValue = "", this.children = a.toArray(this.el.children) } return c(e, t), r.Attributes.html = e, e.prototype.set = function (t) { for (var e, n, r, i, o; e = this.el.firstChild;)this.el.removeChild(e); for (this.el.innerHTML = t + this.templateValue, i = this.children, o = [], n = 0, r = i.length; r > n; n++)e = i[n], o.push(this.el.appendChild(e)); return o }, e }(n), o = function (t) { function e(t) { e.__super__.constructor.call(this, t, "class") } return c(e, t), r.Attributes["class"] = e, e }(n) }, { "../lib/lodash": 2, "./helpers": 3 }] }, {}, [1]);


(function ($) {
	$.extend({
		tablesorter: new
			function () {
				var parsers = [], widgets = []; this.defaults = { cssHeader: "header", cssAsc: "headerSortUp", cssDesc: "headerSortDown", cssChildRow: "expand-child", sortInitialOrder: "asc", sortMultiSortKey: "shiftKey", sortForce: null, sortAppend: null, sortLocaleCompare: true, textExtraction: "simple", parsers: {}, widgets: [], widgetZebra: { css: ["even", "odd"] }, headers: {}, widthFixed: false, cancelSelection: true, sortList: [], headerList: [], dateFormat: "us", decimal: '/\.|\,/g', onRenderHeader: null, selectorHeaders: 'thead th', debug: false }; function benchmark(s, d) { log(s + "," + (new Date().getTime() - d.getTime()) + "ms"); } this.benchmark = benchmark; function log(s) { if (typeof console != "undefined" && typeof console.debug != "undefined") { console.log(s); } else { alert(s); } } function buildParserCache(table, $headers) { if (table.config.debug) { var parsersDebug = ""; } if (table.tBodies.length == 0) return; var rows = table.tBodies[0].rows; if (rows[0]) { var list = [], cells = rows[0].cells, l = cells.length; for (var i = 0; i < l; i++) { var p = false; if ($.metadata && ($($headers[i]).metadata() && $($headers[i]).metadata().sorter)) { p = getParserById($($headers[i]).metadata().sorter); } else if ((table.config.headers[i] && table.config.headers[i].sorter)) { p = getParserById(table.config.headers[i].sorter); } if (!p) { p = detectParserForColumn(table, rows, -1, i); } if (table.config.debug) { parsersDebug += "column:" + i + " parser:" + p.id + "\n"; } list.push(p); } } if (table.config.debug) { log(parsersDebug); } return list; }; function detectParserForColumn(table, rows, rowIndex, cellIndex) { var l = parsers.length, node = false, nodeValue = false, keepLooking = true; while (nodeValue == '' && keepLooking) { rowIndex++; if (rows[rowIndex]) { node = getNodeFromRowAndCellIndex(rows, rowIndex, cellIndex); nodeValue = trimAndGetNodeText(table.config, node); if (table.config.debug) { log('Checking if value was empty on row:' + rowIndex); } } else { keepLooking = false; } } for (var i = 1; i < l; i++) { if (parsers[i].is(nodeValue, table, node)) { return parsers[i]; } } return parsers[0]; } function getNodeFromRowAndCellIndex(rows, rowIndex, cellIndex) { return rows[rowIndex].cells[cellIndex]; } function trimAndGetNodeText(config, node) { return $.trim(getElementText(config, node)); } function getParserById(name) { var l = parsers.length; for (var i = 0; i < l; i++) { if (parsers[i].id.toLowerCase() == name.toLowerCase()) { return parsers[i]; } } return false; } function buildCache(table) { if (table.config.debug) { var cacheTime = new Date(); } var totalRows = (table.tBodies[0] && table.tBodies[0].rows.length) || 0, totalCells = (table.tBodies[0].rows[0] && table.tBodies[0].rows[0].cells.length) || 0, parsers = table.config.parsers, cache = { row: [], normalized: [] }; for (var i = 0; i < totalRows; ++i) { var c = $(table.tBodies[0].rows[i]), cols = []; if (c.hasClass(table.config.cssChildRow)) { cache.row[cache.row.length - 1] = cache.row[cache.row.length - 1].add(c); continue; } cache.row.push(c); for (var j = 0; j < totalCells; ++j) { cols.push(parsers[j].format(getElementText(table.config, c[0].cells[j]), table, c[0].cells[j])); } cols.push(cache.normalized.length); cache.normalized.push(cols); cols = null; }; if (table.config.debug) { benchmark("Building cache for " + totalRows + " rows:", cacheTime); } return cache; }; function getElementText(config, node) { var text = ""; if (!node) return ""; if (!config.supportsTextContent) config.supportsTextContent = node.textContent || false; if (config.textExtraction == "simple") { if (config.supportsTextContent) { text = node.textContent; } else { if (node.childNodes[0] && node.childNodes[0].hasChildNodes()) { text = node.childNodes[0].innerHTML; } else { text = node.innerHTML; } } } else { if (typeof (config.textExtraction) == "function") { text = config.textExtraction(node); } else { text = $(node).text(); } } return text; } function appendToTable(table, cache) { if (table.config.debug) { var appendTime = new Date() } var c = cache, r = c.row, n = c.normalized, totalRows = n.length, checkCell = (n[0].length - 1), tableBody = $(table.tBodies[0]), rows = []; for (var i = 0; i < totalRows; i++) { var pos = n[i][checkCell]; rows.push(r[pos]); if (!table.config.appender) { var l = r[pos].length; for (var j = 0; j < l; j++) { tableBody[0].appendChild(r[pos][j]); } } } if (table.config.appender) { table.config.appender(table, rows); } rows = null; if (table.config.debug) { benchmark("Rebuilt table:", appendTime); } applyWidget(table); setTimeout(function () { $(table).trigger("sortEnd"); }, 0); }; function buildHeaders(table) { if (table.config.debug) { var time = new Date(); } var meta = ($.metadata) ? true : false; var header_index = computeTableHeaderCellIndexes(table); $tableHeaders = $(table.config.selectorHeaders, table).each(function (index) { this.column = header_index[this.parentNode.rowIndex + "-" + this.cellIndex]; this.order = formatSortingOrder(table.config.sortInitialOrder); this.count = this.order; if (checkHeaderMetadata(this) || checkHeaderOptions(table, index)) this.sortDisabled = true; if (checkHeaderOptionsSortingLocked(table, index)) this.order = this.lockedOrder = checkHeaderOptionsSortingLocked(table, index); if (!this.sortDisabled) { var $th = $(this).addClass(table.config.cssHeader); if (table.config.onRenderHeader) table.config.onRenderHeader.apply($th); } table.config.headerList[index] = this; }); if (table.config.debug) { benchmark("Built headers:", time); log($tableHeaders); } return $tableHeaders; }; function computeTableHeaderCellIndexes(t) {
					var matrix = []; var lookup = {}; var thead = t.getElementsByTagName('THEAD')[0]; var trs = thead.getElementsByTagName('TR'); for (var i = 0; i < trs.length; i++) {
						var cells = trs[i].cells; for (var j = 0; j < cells.length; j++) {
							var c = cells[j]; var rowIndex = c.parentNode.rowIndex; var cellId = rowIndex + "-" + c.cellIndex; var rowSpan = c.rowSpan || 1; var colSpan = c.colSpan || 1
							var firstAvailCol; if (typeof (matrix[rowIndex]) == "undefined") { matrix[rowIndex] = []; } for (var k = 0; k < matrix[rowIndex].length + 1; k++) { if (typeof (matrix[rowIndex][k]) == "undefined") { firstAvailCol = k; break; } } lookup[cellId] = firstAvailCol; for (var k = rowIndex; k < rowIndex + rowSpan; k++) { if (typeof (matrix[k]) == "undefined") { matrix[k] = []; } var matrixrow = matrix[k]; for (var l = firstAvailCol; l < firstAvailCol + colSpan; l++) { matrixrow[l] = "x"; } }
						}
					} return lookup;
				} function checkCellColSpan(table, rows, row) { var arr = [], r = table.tHead.rows, c = r[row].cells; for (var i = 0; i < c.length; i++) { var cell = c[i]; if (cell.colSpan > 1) { arr = arr.concat(checkCellColSpan(table, headerArr, row++)); } else { if (table.tHead.length == 1 || (cell.rowSpan > 1 || !r[row + 1])) { arr.push(cell); } } } return arr; }; function checkHeaderMetadata(cell) { if (($.metadata) && ($(cell).metadata().sorter === false)) { return true; }; return false; } function checkHeaderOptions(table, i) { if ((table.config.headers[i]) && (table.config.headers[i].sorter === false)) { return true; }; return false; } function checkHeaderOptionsSortingLocked(table, i) { if ((table.config.headers[i]) && (table.config.headers[i].lockedOrder)) return table.config.headers[i].lockedOrder; return false; } function applyWidget(table) { var c = table.config.widgets; var l = c.length; for (var i = 0; i < l; i++) { getWidgetById(c[i]).format(table); } } function getWidgetById(name) { var l = widgets.length; for (var i = 0; i < l; i++) { if (widgets[i].id.toLowerCase() == name.toLowerCase()) { return widgets[i]; } } }; function formatSortingOrder(v) { if (typeof (v) != "Number") { return (v.toLowerCase() == "desc") ? 1 : 0; } else { return (v == 1) ? 1 : 0; } } function isValueInArray(v, a) { var l = a.length; for (var i = 0; i < l; i++) { if (a[i][0] == v) { return true; } } return false; } function setHeadersCss(table, $headers, list, css) { $headers.removeClass(css[0]).removeClass(css[1]); var h = []; $headers.each(function (offset) { if (!this.sortDisabled) { h[this.column] = $(this); } }); var l = list.length; for (var i = 0; i < l; i++) { h[list[i][0]].addClass(css[list[i][1]]); } } function fixColumnWidth(table, $headers) { var c = table.config; if (c.widthFixed) { var colgroup = $('<colgroup>'); $("tr:first td", table.tBodies[0]).each(function () { colgroup.append($('<col>').css('width', $(this).width())); }); $(table).prepend(colgroup); }; } function updateHeaderSortCount(table, sortList) { var c = table.config, l = sortList.length; for (var i = 0; i < l; i++) { var s = sortList[i], o = c.headerList[s[0]]; o.count = s[1]; o.count++; } } function multisort(table, sortList, cache) { if (table.config.debug) { var sortTime = new Date(); } var dynamicExp = "var sortWrapper = function(a,b) {", l = sortList.length; for (var i = 0; i < l; i++) { var c = sortList[i][0]; var order = sortList[i][1]; var s = (table.config.parsers[c].type == "text") ? ((order == 0) ? makeSortFunction("text", "asc", c) : makeSortFunction("text", "desc", c)) : ((order == 0) ? makeSortFunction("numeric", "asc", c) : makeSortFunction("numeric", "desc", c)); var e = "e" + i; dynamicExp += "var " + e + " = " + s; dynamicExp += "if(" + e + ") { return " + e + "; } "; dynamicExp += "else { "; } var orgOrderCol = cache.normalized[0].length - 1; dynamicExp += "return a[" + orgOrderCol + "]-b[" + orgOrderCol + "];"; for (var i = 0; i < l; i++) { dynamicExp += "}; "; } dynamicExp += "return 0; "; dynamicExp += "}; "; if (table.config.debug) { benchmark("Evaling expression:" + dynamicExp, new Date()); } eval(dynamicExp); cache.normalized.sort(sortWrapper); if (table.config.debug) { benchmark("Sorting on " + sortList.toString() + " and dir " + order + " time:", sortTime); } return cache; }; function makeSortFunction(type, direction, index) { var a = "a[" + index + "]", b = "b[" + index + "]"; if (type == 'text' && direction == 'asc') { return "(" + a + " == " + b + " ? 0 : (" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : (" + a + " < " + b + ") ? -1 : 1 )));"; } else if (type == 'text' && direction == 'desc') { return "(" + a + " == " + b + " ? 0 : (" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : (" + b + " < " + a + ") ? -1 : 1 )));"; } else if (type == 'numeric' && direction == 'asc') { return "(" + a + " === null && " + b + " === null) ? 0 :(" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : " + a + " - " + b + "));"; } else if (type == 'numeric' && direction == 'desc') { return "(" + a + " === null && " + b + " === null) ? 0 :(" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : " + b + " - " + a + "));"; } }; function makeSortText(i) { return "((a[" + i + "] < b[" + i + "]) ? -1 : ((a[" + i + "] > b[" + i + "]) ? 1 : 0));"; }; function makeSortTextDesc(i) { return "((b[" + i + "] < a[" + i + "]) ? -1 : ((b[" + i + "] > a[" + i + "]) ? 1 : 0));"; }; function makeSortNumeric(i) { return "a[" + i + "]-b[" + i + "];"; }; function makeSortNumericDesc(i) { return "b[" + i + "]-a[" + i + "];"; }; function sortText(a, b) { if (table.config.sortLocaleCompare) return a.localeCompare(b); return ((a < b) ? -1 : ((a > b) ? 1 : 0)); }; function sortTextDesc(a, b) { if (table.config.sortLocaleCompare) return b.localeCompare(a); return ((b < a) ? -1 : ((b > a) ? 1 : 0)); }; function sortNumeric(a, b) { return a - b; }; function sortNumericDesc(a, b) { return b - a; }; function getCachedSortType(parsers, i) { return parsers[i].type; }; this.construct = function (settings) { return this.each(function () { if (!this.tHead || !this.tBodies) return; var $this, $document, $headers, cache, config, shiftDown = 0, sortOrder; this.config = {}; config = $.extend(this.config, $.tablesorter.defaults, settings); $this = $(this); $.data(this, "tablesorter", config); $headers = buildHeaders(this); this.config.parsers = buildParserCache(this, $headers); cache = buildCache(this); var sortCSS = [config.cssDesc, config.cssAsc]; fixColumnWidth(this); $headers.click(function (e) { var totalRows = ($this[0].tBodies[0] && $this[0].tBodies[0].rows.length) || 0; if (!this.sortDisabled && totalRows > 0) { $this.trigger("sortStart"); var $cell = $(this); var i = this.column; this.order = this.count++ % 2; if (this.lockedOrder) this.order = this.lockedOrder; if (!e[config.sortMultiSortKey]) { config.sortList = []; if (config.sortForce != null) { var a = config.sortForce; for (var j = 0; j < a.length; j++) { if (a[j][0] != i) { config.sortList.push(a[j]); } } } config.sortList.push([i, this.order]); } else { if (isValueInArray(i, config.sortList)) { for (var j = 0; j < config.sortList.length; j++) { var s = config.sortList[j], o = config.headerList[s[0]]; if (s[0] == i) { o.count = s[1]; o.count++; s[1] = o.count % 2; } } } else { config.sortList.push([i, this.order]); } }; setTimeout(function () { setHeadersCss($this[0], $headers, config.sortList, sortCSS); appendToTable($this[0], multisort($this[0], config.sortList, cache)); }, 1); return false; } }).mousedown(function () { if (config.cancelSelection) { this.onselectstart = function () { return false }; return false; } }); $this.bind("update", function () { var me = this; setTimeout(function () { me.config.parsers = buildParserCache(me, $headers); cache = buildCache(me); }, 1); }).bind("updateCell", function (e, cell) { var config = this.config; var pos = [(cell.parentNode.rowIndex - 1), cell.cellIndex]; cache.normalized[pos[0]][pos[1]] = config.parsers[pos[1]].format(getElementText(config, cell), cell); }).bind("sorton", function (e, list) { $(this).trigger("sortStart"); config.sortList = list; var sortList = config.sortList; updateHeaderSortCount(this, sortList); setHeadersCss(this, $headers, sortList, sortCSS); appendToTable(this, multisort(this, sortList, cache)); }).bind("appendCache", function () { appendToTable(this, cache); }).bind("applyWidgetId", function (e, id) { getWidgetById(id).format(this); }).bind("applyWidgets", function () { applyWidget(this); }); if ($.metadata && ($(this).metadata() && $(this).metadata().sortlist)) { config.sortList = $(this).metadata().sortlist; } if (config.sortList.length > 0) { $this.trigger("sorton", [config.sortList]); } applyWidget(this); }); }; this.addParser = function (parser) { var l = parsers.length, a = true; for (var i = 0; i < l; i++) { if (parsers[i].id.toLowerCase() == parser.id.toLowerCase()) { a = false; } } if (a) { parsers.push(parser); }; }; this.addWidget = function (widget) { widgets.push(widget); }; this.formatFloat = function (s) { var i = parseFloat(s); return (isNaN(i)) ? 0 : i; }; this.formatInt = function (s) { var i = parseInt(s); return (isNaN(i)) ? 0 : i; }; this.isDigit = function (s, config) { return /^[-+]?\d*$/.test($.trim(s.replace(/[,.']/g, ''))); }; this.clearTableBody = function (table) { if ($.browser.msie) { function empty() { while (this.firstChild) this.removeChild(this.firstChild); } empty.apply(table.tBodies[0]); } else { table.tBodies[0].innerHTML = ""; } };
			}
	}); $.fn.extend({ tablesorter: $.tablesorter.construct }); var ts = $.tablesorter; ts.addParser({ id: "text", is: function (s) { return true; }, format: function (s) { return $.trim(s.toLocaleLowerCase()); }, type: "text" }); ts.addParser({ id: "digit", is: function (s, table) { var c = table.config; return $.tablesorter.isDigit(s, c); }, format: function (s) { return $.tablesorter.formatFloat(s); }, type: "numeric" }); ts.addParser({ id: "currency", is: function (s) { return /^[£$€?.]/.test(s); }, format: function (s) { return $.tablesorter.formatFloat(s.replace(new RegExp(/[£$€]/g), "")); }, type: "numeric" }); ts.addParser({ id: "ipAddress", is: function (s) { return /^\d{2,3}[\.]\d{2,3}[\.]\d{2,3}[\.]\d{2,3}$/.test(s); }, format: function (s) { var a = s.split("."), r = "", l = a.length; for (var i = 0; i < l; i++) { var item = a[i]; if (item.length == 2) { r += "0" + item; } else { r += item; } } return $.tablesorter.formatFloat(r); }, type: "numeric" }); ts.addParser({ id: "url", is: function (s) { return /^(https?|ftp|file):\/\/$/.test(s); }, format: function (s) { return jQuery.trim(s.replace(new RegExp(/(https?|ftp|file):\/\//), '')); }, type: "text" }); ts.addParser({ id: "isoDate", is: function (s) { return /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(s); }, format: function (s) { return $.tablesorter.formatFloat((s != "") ? new Date(s.replace(new RegExp(/-/g), "/")).getTime() : "0"); }, type: "numeric" }); ts.addParser({ id: "percent", is: function (s) { return /\%$/.test($.trim(s)); }, format: function (s) { return $.tablesorter.formatFloat(s.replace(new RegExp(/%/g), "")); }, type: "numeric" }); ts.addParser({ id: "usLongDate", is: function (s) { return s.match(new RegExp(/^[A-Za-z]{3,10}\.? [0-9]{1,2}, ([0-9]{4}|'?[0-9]{2}) (([0-2]?[0-9]:[0-5][0-9])|([0-1]?[0-9]:[0-5][0-9]\s(AM|PM)))$/)); }, format: function (s) { return $.tablesorter.formatFloat(new Date(s).getTime()); }, type: "numeric" }); ts.addParser({ id: "shortDate", is: function (s) { return /\d{1,2}[\/\-]\d{1,2}[\/\-]\d{2,4}/.test(s); }, format: function (s, table) { var c = table.config; s = s.replace(/\-/g, "/"); if (c.dateFormat == "us") { s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})/, "$3/$1/$2"); } else if (c.dateFormat == "uk") { s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})/, "$3/$2/$1"); } else if (c.dateFormat == "dd/mm/yy" || c.dateFormat == "dd-mm-yy") { s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{2})/, "$1/$2/$3"); } return $.tablesorter.formatFloat(new Date(s).getTime()); }, type: "numeric" }); ts.addParser({ id: "time", is: function (s) { return /^(([0-2]?[0-9]:[0-5][0-9])|([0-1]?[0-9]:[0-5][0-9]\s(am|pm)))$/.test(s); }, format: function (s) { return $.tablesorter.formatFloat(new Date("2000/01/01 " + s).getTime()); }, type: "numeric" }); ts.addParser({ id: "metadata", is: function (s) { return false; }, format: function (s, table, cell) { var c = table.config, p = (!c.parserMetadataName) ? 'sortValue' : c.parserMetadataName; return $(cell).metadata()[p]; }, type: "numeric" }); ts.addWidget({ id: "zebra", format: function (table) { if (table.config.debug) { var time = new Date(); } var $tr, row = -1, odd; $("tr:visible", table.tBodies[0]).each(function (i) { $tr = $(this); if (!$tr.hasClass(table.config.cssChildRow)) row++; odd = (row % 2 == 0); $tr.removeClass(table.config.widgetZebra.css[odd ? 0 : 1]).addClass(table.config.widgetZebra.css[odd ? 1 : 0]) }); if (table.config.debug) { $.tablesorter.benchmark("Applying Zebra widget", time); } } });
})(jQuery);


/*
	tinytooltip
*/
(function (e) { e.fn.tinytooltip = function (t) { function r(t) { var r = t.offset(); var i = e('<span class="tinytooltip' + (n.classes ? " " + n.classes : "") + '">'); i.append('<span class="arrow">'); i.append(e('<span class="message">').append(typeof n.message == "function" ? n.message.call(t, i) : n.message)); i.css("opacity", 0).hide(); e("body").append(i); i.css("left", r.left + t.outerWidth() / 2 - i.outerWidth() / 2); i.css("top", r.top + t.outerHeight()); t.data("tinytooltip", i); i.show().animate({ opacity: 1 }, 200) } function i(t) { var n = t.data("tinytooltip"); if (n) { n.animate({ opacity: 0 }, 200, function () { e(this).remove() }) } } var n = { message: "", hover: true, classes: "" }; if (t) { e.extend(n, t) } return this.each(function () { var t = e(this); t.bind((n.hover ? "mouseover " : "") + "showtooltip", function () { if (t.data("tinyactive")) { clearTimeout(t.timer); return } t.data("tinyactive", 1); r(t) }); t.bind((n.hover ? "mouseout " : "") + "hidetooltip", function () { clearTimeout(t.timer); t.timer = setTimeout(function () { t.data("tinyactive", ""); i(t) }, 400) }) }) }; return this })(jQuery);




jQuery(document).ready(function ($) {
	// ______________________________________________________________

	// ___ sticky header on insider table
	if ($('#table-header').length > 0) {
		var top = $('#table-header').offset().top;
		$(document).scroll(function () {
			var me = $('#table-header');

			if ($(document).scrollTop() > top) {
				if (!window.header_sticky) {
					me.css('width', me.parent().outerWidth() + 'px');
					me.addClass('sticky');
					window.header_sticky = true;
				}
			} else if (window.header_sticky) {
				me.removeClass('sticky');
				me.css('width', '100%');
				window.header_sticky = false;
			}
		});
	}

	if ($('#table').length > 0) {

		$("#table").tablesorter();

		$('#table-header th').click(function () {
			var index = $(this).index();

			if ($(this).data('direction') == 'desc') {
				var sorting = [[index, 1]];
				$(this).data('direction', 'asc');
			} else {
				var sorting = [[index, 0]];
				$(this).data('direction', 'desc');
			}

			$("#table").trigger("sorton", [sorting]);

			return false;
		});
	}

	// tooltips
	$('.tip').each(function () {
		$(this).tinytooltip({ message: $(this).data('tip') });
	});

	// ______________________________________________________________

	Calculator.init();
});



// ______________________________________________________________
var Calculator = new function () {

	// public methods
	this.init = function () {
		Me.init();
	};

	// _______________ core
	var Me = {
		span_query: [],
		spans: [],
		span_total: 0,

		init: function () {

			// search
			$('#q').on('keyup keydown', function () {
				Me.searchScrip($.trim($(this).val()).toUpperCase());
			});
			$('#search').submit(function () {
				return false;
			});

			// show popup
			$('.calculate').click(function () {
				var id = $(this).data('id');

				switch (MODULE) {
					case 'Equity':
						Me.showEquityPopup(id);
						break;
					case 'Futures':
						Me.showFuturesPopup(id);
						break;
					case 'Currency':
						Me.showCDSPopup(id);
						break;
					case 'Commodity':
						Me.showCommodityPopup(id);
						break;
				}
				return false;
			});

			// close popup
			$('.popup .close').click(function () {
				$(this).blur();
				$(this).parent().fadeOut(200);
				return false;
			});
			$(document).keypress(function (e) {
				if (e.keyCode == 27) {
					$('.popup').fadeOut(200);
				}
			});


			// equity calculation
			$('#form-equity').submit(function () {
				Me.calculateEquity();
				return false;
			});

			// futures calculation
			$('#form-futures').submit(function () {
				Me.calculateFutures();
				return false;
			});

			// commodity calculation
			$('#form-commodity').submit(function () {
				Me.calculateCommodity();
				return false;
			});

			// currency calculation
			$('#form-cds').submit(function () {
				Me.calculateCDS();
				return false;
			});

			// __________________


			if (MODULE == "SPAN") {
				Me.initSPAN();

				$('#reset').click(function () {
					document.location.reload();
				});
			}

			if (MODULE == "BracketCover") {
				Me.initBOCO();

				$('#reset').click(function () {
					document.location.reload();
				});
			}
		},

		// __ quick search list of scrips
		searchScrip: function (q) {
			if (!q) {
				$('#table tbody tr').show();
				return false;
			}

			if (q.length < 2 || window.prev && window.prev == q) return false;

			var list = $('#table tbody');
			list.find("tr").hide();
			list.find("tr[data-scrip*='" + q + "']").show();
		},

		// __ show popup calculator
		showEquityPopup: function (id) {
			var cont = $('#entry-' + id);
			var scrip = $(cont).data('scrip'),
				segment = $(cont).data('segment'),
				mis_margin = $(cont).data('mis_margin'),
				mis_multiplier = $(cont).data('mis_multiplier'),
				co_margin = $(cont).data('co_margin'),
				co_multiplier = $(cont).data('co_multiplier');

			var popup = $('#popup-equity').data('id', id);
			popup.find('.scrip').text(scrip);
			popup.find('.mis_multiplier').text(mis_multiplier + 'x');
			popup.find('.co_multiplier').text(co_multiplier + 'x');

			$('#form-equity').submit();
			popup.fadeIn(200);
		},
		calculateEquity: function () {
			var entry = $('#entry-' + $('#popup-equity').data('id'));

			var cash = parseFloat($.trim($('#popup-equity .cash').val())),
				price = parseFloat($.trim($('#popup-equity .price').val())),
				mis_margin = parseFloat(entry.data('mis_margin')),
				co_margin = parseFloat(entry.data('co_margin'));

			if (isNaN(cash)) {
				cash = 100000;
				$('#popup-equity .cash').val(cash);
			}

			if (isNaN(price)) {
				price = 100;
				$('#popup-equity .price').val(price);
			}

			// single share
			var mis_single = (price * mis_margin) / 100,
				mis_power = Math.floor(cash / mis_single),
				co_single = (price * co_margin) / 100,
				co_power = Math.floor(cash / co_single);
			cnc_power = Math.floor(cash / price);

			$('#popup-equity .cnc_power').text(!isNaN(cnc_power) && isFinite(cnc_power) ? cnc_power : 0);
			$('#popup-equity .mis_power').text(!isNaN(mis_power) && isFinite(mis_power) ? mis_power : 0);
			$('#popup-equity .co_power').text(!isNaN(co_power) && isFinite(co_power) ? co_power : 0);

			return false;
		},

		// futures
		// __ show popup calculator
		showFuturesPopup: function (id) {
			var cont = $('#entry-' + id);
			var scrip = $(cont).data('scrip'),
				lot_size = $(cont).data('lot_size'),
				expiry = $(cont).data('expiry'),
				nrml_margin = $(cont).data('nrml_margin'),
				mis_margin = $(cont).data('mis_margin'),
				co_margin = $(cont).data('co_margin'),
				price = $(cont).data('price');

			var popup = $('.popup').data('id', id);
			popup.find('.scrip').text(scrip + ': ' + expiry);
			popup.find('.mis_margin').text(mis_margin);
			popup.find('.co_margin').text(co_margin);
			popup.find('.nrml_margin').text(nrml_margin);
			popup.find('.price').val(price);

			$('#form-futures').submit();
			popup.fadeIn(200);
		},
		calculateFutures: function () {
			var entry = $('#entry-' + $('.popup').data('id'));
			var popup = $('.popup');

			var cash = parseFloat($.trim(popup.find('.cash').val())),
				price = parseFloat($.trim(popup.find('.price').val())),

				margin = parseFloat(entry.data('margin')),
				lot_size = parseInt(entry.data('lot_size')),
				co_trigger = parseFloat(entry.data('co_trigger')),

				expiry = entry.data('expiry');

			if (isNaN(cash)) {
				cash = 100000;
				popup.find('.cash').val(cash);
			}

			if (isNaN(price)) {
				price = 100;
				popup.find('.price').val(price);
			}


			var nrml_margin = (price * lot_size * margin) / 100,
				mis_margin = nrml_margin * 0.45,
				co_margin = ((price * lot_size) * co_trigger / 100);

			if ($('h1.scrip').text().indexOf("NIFTY") != -1) {
				mis_margin = nrml_margin * .35;
			}

			// single share
			var nrml_power = Math.floor(cash / nrml_margin),
				mis_power = Math.floor(cash / mis_margin),
				co_power = Math.floor(cash / co_margin);

			popup.find('.nrml_power').text(!isNaN(nrml_power) && isFinite(nrml_power) ? nrml_power : 'N/A');
			popup.find('.mis_power').text(!isNaN(mis_power) && isFinite(mis_power) ? mis_power : 'N/A');
			popup.find('.co_power').text(!isNaN(co_power) && isFinite(co_power) ? co_power : 'N/A');
			popup.find('.price').text(!isNaN(price) && isFinite(price) ? price : 'N/A');

			popup.find('.mis_margin').text(Math.floor(mis_margin));
			popup.find('.co_margin').text(Math.floor(co_margin) == 0 ? 'N/A' : Math.floor(co_margin));
			popup.find('.nrml_margin').text(Math.floor(nrml_margin));

			return false;
		},

		// cds
		// __ show popup calculator
		showCDSPopup: function (id) {

			var cont = $('#entry-' + id);
			var scrip = $(cont).data('scrip'),
				lot_size = $(cont).data('lot_size'),
				expiry = $(cont).data('expiry'),
				nrml_margin = $(cont).data('nrml_margin'),
				mis_margin = $(cont).data('mis_margin'),
				co_margin = $(cont).data('co_margin'),
				price = $(cont).data('price');

			var popup = $('.popup').data('id', id);
			popup.find('.scrip').text(scrip + ': ' + expiry);
			popup.find('.mis_margin').text(mis_margin);
			popup.find('.co_margin').text(co_margin);
			popup.find('.nrml_margin').text(nrml_margin);
			popup.find('.price').val(price);

			$('#form-cds').submit();
			popup.fadeIn(200);
		},
		calculateCDS: function () {
			var entry = $('#entry-' + $('.popup').data('id'));
			var popup = $('.popup');

			var cash = parseFloat($.trim(popup.find('.cash').val())),
				price = parseFloat($.trim(popup.find('.price').val())),

				margin = parseFloat(entry.data('margin')),
				lot_size = parseInt(entry.data('lot_size')),
				co_trigger = parseFloat(entry.data('co_trigger')),

				expiry = entry.data('expiry');


			if (isNaN(cash)) {
				cash = 100000;
				popup.find('.cash').val(cash);
			}

			if (isNaN(price)) {
				price = 100;
				popup.find('.price').val(price);
			}


			var nrml_margin = (price * lot_size * margin) / 100,
				mis_margin = nrml_margin * 0.5,
				co_margin = ((price * lot_size) * co_trigger / 100);

			// single share
			var nrml_power = Math.floor(cash / nrml_margin),
				mis_power = Math.floor(cash / mis_margin),
				co_power = Math.floor(cash / co_margin);

			popup.find('.nrml_power').text(!isNaN(nrml_power) && isFinite(nrml_power) ? nrml_power : 'N/A');
			popup.find('.mis_power').text(!isNaN(mis_power) && isFinite(mis_power) ? mis_power : 'N/A');
			popup.find('.co_power').text(!isNaN(co_power) && isFinite(co_power) ? co_power : 'N/A');
			popup.find('.price').text(!isNaN(price) && isFinite(price) ? price : 'N/A');

			popup.find('.mis_margin').text(Math.floor(mis_margin));
			popup.find('.co_margin').text(Math.floor(co_margin) == 0 ? 'N/A' : Math.floor(co_margin));
			popup.find('.nrml_margin').text(Math.floor(nrml_margin));

			return false;
		},


		// commodity
		// __ show popup calculator
		showCommodityPopup: function (id) {
			var cont = $('#entry-' + id);
			var scrip = $(cont).data('scrip'),
				lot_size = $(cont).data('lot_size'),
				nrml_margin = $(cont).data('nrml_margin'),
				mis_margin = $(cont).data('mis_margin'),
				price = $(cont).data('price');

			var popup = $('.popup').data('id', id);
			popup.find('.scrip').text(scrip);
			popup.find('.mis_margin').text(mis_margin);
			popup.find('.nrml_margin').text(nrml_margin);
			popup.find('.price').val(price);

			$('#form-commodity').submit();
			popup.fadeIn(200);
		},
		calculateCommodity: function () {
			var entry = $('#entry-' + $('.popup').data('id'));
			var popup = $('.popup');

			var cash = parseFloat($.trim(popup.find('.cash').val())),
				price = parseFloat($.trim(popup.find('.price').val())),

				margin = parseFloat(entry.data('margin')),
				lot_size = parseInt(entry.data('lot_size'));

			if (isNaN(cash)) {
				cash = 100000;
				popup.find('.cash').val(cash);
			}

			if (isNaN(price)) {
				price = 100;
				popup.find('.price').val(price);
			}

			var nrml_margin = (price * lot_size * margin) / 100,
				mis_margin = nrml_margin * 0.5;

			// single share
			var nrml_power = Math.floor(cash / nrml_margin),
				mis_power = Math.floor(cash / mis_margin);

			popup.find('.nrml_power').text(!isNaN(nrml_power) && isFinite(nrml_power) ? nrml_power : 'N/A');
			popup.find('.mis_power').text(!isNaN(mis_power) && isFinite(mis_power) ? mis_power : 'N/A');
			popup.find('.price').text(!isNaN(price) && isFinite(price) ? price : 'N/A');

			popup.find('.mis_margin').text(Math.floor(mis_margin));
			popup.find('.nrml_margin').text(Math.floor(nrml_margin));

			return false;
		},

		// _____________ SPAN calculator
		initSPAN: function () {
			// ui control interactions
			// exchange

			$('.changer').change(function () {
				var show = $(this).find('option:selected').data('show'),
					hide = $(this).find('option:selected').data('hide');
				if (hide) {
					$(hide).hide();
				}

				if (show) {
					$(show).show();
				}
			}).change();

			// exchange data
			$('#exchange').change(function () {
				Me.populateSPANui($('#exchange').val());
				$('#scrip').change();
			}).change();

			// scrip lot size
			$('#scrip').change(function () {
				var lz = $(this).find('option:selected').data('lot_size');
				if (lz) {
					$('#lot_size .val').text(lz);
					$('#qty').val(lz);
				}
			}).change();

			// add span calculation
			$('#form-span').submit(function () {
				Me.addSPAN();
				return false;
			});

			// delete table items
			$('#table-span tbody').on('click', '.x', function () {

				var id = $(this).parents('tr:first').index();

				$('#table-span tbody tr:eq(' + id + ')').hide();

				Me.deleteSpanItem(id);

				if (Me.spans.length != 0) {
					Me.fetchSPAN(Me.spans.length - 1);
				} else {
					$('#tally .val').text('0');
					$('#table-span tfoot').hide();
				}

				return false;
			});

			// table rendering directives
			Me.directives = {
				exchange: {
					html: function (e) {
						return $(e.element).text() + ' <a href="#" class="x">x</a>';
					}
				},
				qty: {
					text: function (e) {
						return this.qty + " " + this.trade[0].toUpperCase();
					}
				},
				strike_price: {
					text: function () {
						if (!this.strike_price) {
							return 'N/A';
						}
						return this.strike_price + ' ' + this.option_type;
					}
				},
				span: {
					html: function (e) {
						return $(e.element).text() ? Me.numberFormat(parseFloat($(e.element).text())) : '<span class="loading"> </span>';
					}
				},
				exposure: {
					html: function (e) {
						return $(e.element).text() ? Me.numberFormat(parseFloat($(e.element).text())) : '<span class="loading"> </span>';
					}
				},
				total: {
					html: function (e) {
						return $(e.element).text() ? Me.numberFormat(parseFloat($(e.element).text())) : '<span class="loading"> </span>';
					}
				}
			};

		},
		populateSPANui: function (field) {
			switch (field) {
				case 'NFO':
					var sym = $('#scrip');
					sym.empty();
					for (var n = 0; n < FUTURES.length; n++) {
						sym.append($('<option>').val(FUTURES[n][0]).append(FUTURES[n][1]).data('lot_size', FUTURES[n][2]));
					}
					$('#lot_size').show();
					break;
				case 'MCX':
					var sym = $('#scrip');
					sym.empty();
					for (var n = 0; n < COMMODITIES.length; n++) {
						sym.append($('<option>').val(COMMODITIES[n][0]).append(COMMODITIES[n][1]).data('lot_size', COMMODITIES[n][2]));
					}
					$('#lot_size').show();
					break;
				case 'CDS':
					var sym = $('#scrip');
					sym.empty();
					for (var n = 0; n < CURRENCIES.length; n++) {
						sym.append($('<option>').val(CURRENCIES[n][0]).append(CURRENCIES[n][1]).data('lot_size', CURRENCIES[n][2]));
					}
					$('#lot_size').hide();
					break;
			}
		},

		// _________________________________

		addNewSPANitem: function (query, exchange, scrip, product, product_name, option_type, strike_price, qty, trade) {
			var strike = '';

			Me.spans.push({
				"exchange": exchange,
				"scrip": scrip,
				"product": product,
				"option_type": option_type,
				"product_name": product_name,
				"strike_price": strike_price,
				"qty": qty,
				"trade": trade,
				"query": query
			});

			$('#table-span tbody').render(Me.spans, Me.directives);

			return Me.spans.length - 1;
		},

		spanItemExists: function (exchange, product, scrip, option_type, strike_price, trade) {
			var hay, needle;
			if (product == 'OPT') {
				var needle = exchange + product + scrip + option_type + strike_price;
			} else {
				var needle = exchange + product + scrip;
			}

			for (var n = 0; n < Me.spans.length; n++) {
				if (product == 'OPT') {
					hay = Me.spans[n].exchange + Me.spans[n].product + Me.spans[n].scrip + Me.spans[n].option_type + Me.spans[n].strike_price;
				} else {
					hay = Me.spans[n].exchange + Me.spans[n].product + Me.spans[n].scrip;
				}

				if (hay == needle) {
					return true;
				}
			}

			return false;
		},

		getSPANqueries: function () {
			var q = [];
			for (var n = 0; n < Me.spans.length; n++) {
				q.push(Me.spans[n].query);
			}
			return q.join('&');
		},

		deleteSpanItem: function (id) {
			Me.spans.splice(id, 1);
		},

		refreshSPAN: function () {
			$('#table-span tbody').render(Me.spans, Me.directives);
			$('#table-span tbody *:hidden').show();
		},

		addSPAN: function () {
			// validation
			var qty = $('#qty').val();
			if (!qty || isNaN(qty * 1)) {
				alert("Please enter a valid quantity");
				$('#qty').focus().select();
				return false;
			}
			qty = parseInt(qty);

			// negative qty
			if (qty < 0) {
				qty = Math.abs(qty);
				$('#qty').val(qty);
				$('#form-span .trade-sell').attr('checked', 1);
			}

			// lot size multiple
			var lot_size = parseInt($('#scrip option:selected').data('lot_size'));
			if (!isNaN(lot_size) && lot_size > 0) {
				qty = Math.ceil(qty / lot_size) * lot_size;
			}
			$('#qty').val(qty);


			// strike price
			var strike_price = 0;
			if ($('#product').val() == 'OPT' && $('#exchange').val() != 'MCX') {
				strike_price = $('#strike_price').val();

				if (!strike_price || isNaN(strike_price * 1)) {
					alert("Please enter a valid strike price");
					$('#strike_price').focus().select();
					return false;
				}
				strike_price = parseInt(strike_price);
			}

			// check if it already exists
			if (Me.spanItemExists($('#exchange').val(),
				$('#product').val(),
				$('#scrip').val(),
				$('#option_type').val(),
				strike_price,
				$('#form-span .trade:checked').val()
			)) {
				alert("You have already added this contract");
				return;
			}

			// ___________________________

			var query = $('#form-span').serialize();

			var strike_price;
			if ($('#product').val() == 'FUT') {
				strike_price = '';
			} else {
				strike_price = $('#strike_price').val();
			}

			var id = Me.addNewSPANitem(
				query,
				$('#exchange').val(),
				$('#scrip').val(),
				$('#product option:selected').val(),
				$('#product option:selected').data('name'),
				$('#option_type').val(),
				strike_price,
				qty,
				$('#form-span .trade:checked').val()
			);


			Me.fetchSPAN(id);
			$('#table-span').removeClass('invis');

			// ___________________________ fetch

		},

		fetchSPAN: function (id) {
			if (!Me.getSPANqueries()) {
				return false;
			}

			(function (id) {
				$.post(ROOT + MODULE, 'action=calculate&' + Me.getSPANqueries(), function (data) {
					if (!data || !data.total || !data.total.span) {
						// an invalid entry, remove it. If not removed, Omnesys API will reject all subsequent calls
						$('#table-span tbody tr:eq(' + id + ') .loading').replaceWith('N/A');
						Me.deleteSpanItem(id);
						//Me.refreshSPAN();
					} else {
						// save the data to our store
						Me.spans[id].span = data.last.span;
						Me.spans[id].exposure = data.last.exposure;
						Me.spans[id].spread = data.last.spread;
						Me.spans[id].total = data.last.total;

						Me.refreshSPAN();

						Me.populateSPAN(data.total.span, data.total.exposure, data.total.spread, data.total.netoptionvalue, data.total.total);
					}
				}, "json");

			})(id);
		},

		populateSPAN: function (span, exposure, spread, netoptionvalue, total) {
			// calculate the raw span total
			var all_total = 0;
			for (var n = 0; n < Me.spans.length; n++) {
				all_total += Me.spans[n].total;
			}

			$('#table-span tfoot').show();
			$('#table-span tfoot .total').text(Me.numberFormat(all_total));


			// populate total box
			$('#tally .span').text('Rs: ' + Me.numberFormat(span));
			$('#tally .exposure').text('Rs: ' + Me.numberFormat(exposure));
			$('#tally .total').text('Rs: ' + Me.numberFormat(total));

			if (netoptionvalue > 0) {
				$('#tally .netoptionvalue .val').text('Rs: ' + Me.numberFormat(netoptionvalue));
				$('#tally .netoptionvalue').show();
			} else {
				$('#tally .netoptionvalue').hide();
			}

			if (spread > 0) {
				$('#tally .spread .val').text('Rs: ' + Me.numberFormat(spread));
				$('#tally .spread').show();
			} else {
				$('#tally .spread').hide();
			}

			// benefit calculation
			var ben = $('#tally .mbenefit');
			if (total < all_total) {
				var benefit = all_total - total;

				ben.find('.benefit').text('Rs: ' + Me.numberFormat(benefit));
				ben.show();
			} else {
				ben.hide();
			}
		},

		// _______________________________________________

		// _____________ BOCO calculator
		initBOCO: function () {
			// ui control interactions
			// exchange

			$('.changer').change(function () {
				var show = $(this).find('option:selected').data('show'),
					hide = $(this).find('option:selected').data('hide');
				if (hide) {
					$(hide).hide();
				}

				if (show) {
					$(show).show();
				}
			}).change();

			// exchange data
			$('#exchange').change(function () {
				Me.populateBOCOui($('#exchange').val());
				$('#scrip').change();
			}).change();

			// scrip lot size
			$('#scrip').change(function () {
				var lz = $(this).find('option:selected').data('lot_size');
				if (lz) {
					$('#lot_size .val').text(lz);
					$('#qty').val(lz);
				}

				// price
				var show_price = true;
				if ($('#product').val() == 'OPT') {
					if ($(this).val().indexOf("NIFTY") != -1) {
						$('#span .buysell .buy').show();
						show_price = false;
					} else {
						$('#span .buysell .buy').hide();
					}
				} else {
					$('#span .buysell .buy').show();
				}

				// show price?
				if (show_price) {
					var price = $(this).find('option:selected').data('price');
					if (price) {
						$('#price').val(price);
					} else {
						$('#price').val(0);
					}
				} else {
					$('#price').val("");
				}
			}).change();

			// add span calculation
			$('#form-boco').submit(function () {
				Me.calculateBOCO();
				return false;
			});

			// option->buy disable
			$('#product').change(function () {
				if ($(this).val() == 'OPT') {
					$('#span .buysell .buy').hide();
					$('#span .buysell .trade-sell').prop('checked', true);
					$('#span #opt-buysell').show();

					$('#span .field.stl').hide();

					$('#span .field.price label').text('Premium');
				} else {
					$('#span .buysell .buy').show();
					$('#span #opt-buysell').hide();
					$('#span .field.stl').show();
					$('#span .field.price label').text('Price');
				}
			});

		},
		populateBOCOui: function (field) {
			var sym = $('#scrip'),
				names = [];
			sym.empty();

			// option buy/sell
			$('#span .buysell .buy').show();
			$('#span #opt-buysell').hide();
			$('#span .field.stl').show();
			$('#span .field.stl').show();
			$('#span .field.price label').text('Price');

			switch (field) {
				case 'EQ':
					// gather names and sort
					for (var nm in EQ) {
						if (EQ.hasOwnProperty(nm)) {
							names.push(nm);
						}
					}
					names.sort();

					for (var n = 0; n < names.length; n++) {
						sym.append($('<option>').val(names[n]).append(names[n]));
					}

					$('#lot_size').hide();
					$('#price').val(0);
					$('#stl').val(0);
					$('#qty').val(1);
					break;
				case 'NFO':
					// gather names and sort
					for (var item in FUTURES) {
						if (FUTURES.hasOwnProperty(item)) {
							names.push(item);
						}
					}
					names.sort();

					for (var n = 0; n < names.length; n++) {
						sym.append(
							$('<option>').val(names[n]).append(FUTURES[names[n]]["scrip"] + " " + FUTURES[names[n]]["expiry"])
								.data('lot_size', FUTURES[names[n]]["lot_size"])
								.data('price', FUTURES[names[n]]["price"])
						);
					}

					$('#lot_size').show();
					break;
				case 'MCX':
					// gather names and sort
					for (var item in COMMODITIES) {
						if (COMMODITIES.hasOwnProperty(item)) {
							names.push(item);
						}
					}
					names.sort();

					for (var n = 0; n < names.length; n++) {
						sym.append(
							$('<option>').val(names[n]).append(COMMODITIES[names[n]]["scrip"])
								.data('lot_size', COMMODITIES[names[n]]["lot_size"])
								.data('price', COMMODITIES[names[n]]["price"])
						);
					}

					$('#lot_size').show();
					break;
				case 'CDS':
					// gather names and sort
					for (var item in CURRENCIES) {
						if (CURRENCIES.hasOwnProperty(item)) {
							names.push(item);
						}
					}
					names.sort();

					for (var n = 0; n < names.length; n++) {
						sym.append(
							$('<option>').val(names[n]).append(CURRENCIES[names[n]]["scrip"] + " " + CURRENCIES[names[n]]["expiry"])
								.data('lot_size', CURRENCIES[names[n]]["lot_size"])
								.data('price', CURRENCIES[names[n]]["price"])
						);
					}

					$('#lot_size').show();
					break;
			}
		},

		// _________________________________

		calculateBOCO: function () {
			var scrip = $('#scrip').val();

			// validation
			var qty = $('#qty').val();
			if (!qty || isNaN(qty * 1)) {
				qty = 1;
				$('#qty').val(1);
			}
			qty = parseInt(qty);

			// negative qty
			if (qty < 0) {
				qty = Math.abs(qty);
				$('#qty').val(qty);
				$('#form-boco .trade-sell').attr('checked', 1);
			}

			var price = $('#price').val();
			if (!price || isNaN(price * 1)) {
				alert("Please enter a valid price");
				$('#price').focus().select();
				return false;
			}
			price = parseFloat(price);

			var stl = $('#stl').val();
			if (!stl || isNaN(stl * 1)) {
				$('#stl').val(0);
				stl = 0;
			}
			stl = parseFloat(stl);

			// lot size multiple
			var lot_size = parseInt($('#scrip option:selected').data('lot_size'));
			if (!isNaN(lot_size) && lot_size > 0) {
				qty = Math.ceil(qty / lot_size) * lot_size;
			}
			$('#qty').val(qty);


			// strike price
			var strike_price = 0;
			if ($('#product').val() == 'OPT' && $('#exchange').val() == 'NFO') {
				strike_price = $('#strike_price').val();

				if (!strike_price || isNaN(strike_price * 1)) {
					alert("Please enter a valid strike price");
					$('#strike_price').focus().select();
					return false;
				}
				strike_price = parseInt(strike_price);
			}

			var margin = "N/A", bors = "buy";
			if ($('.trade-sell:checked').length > 0) {
				bors = "sell";
			}

			switch ($('#exchange').val()) {
				case 'EQ':
					var co_lower = parseFloat(EQ[scrip]["co_lower"]) / 100,
						co_upper = parseFloat(EQ[scrip]["co_upper"]) / 100;

					var trigger = price - (co_upper * price);
					if (stl < trigger) {
						$('#stl').val(Math.ceil(trigger));
					} else {
						trigger = stl;
					}

					var x = 0;
					if (bors == "buy") {
						x = (price - trigger) * qty;
					} else {
						x = (trigger - price) * qty;
					}
					var y = co_lower * price * qty;

					// whichever is the highest is the margin
					margin = x > y ? x : y;
					margin += margin * 0.20;
					break;
				case 'NFO':
					if ($('#product').val() == 'OPT') {
						// nifty and banknifty
						if ($('#scrip').val().indexOf("NIFTY") != -1 && $('input.trade-buy').is(':checked')) {
							margin = price * qty * 0.70;
						} else {
							margin = (strike_price + price) * qty * 0.025;
						}
					} else {
						var co_lower = parseFloat(FUTURES[scrip]["co_lower"]) / 100,
							co_upper = parseFloat(FUTURES[scrip]["co_upper"]) / 100;

						var trigger = price - (co_upper * price);
						if (stl < trigger) {
							$('#stl').val(Math.ceil(trigger));
						} else {
							trigger = stl;
						}

						var x = 0;
						if (bors == "buy") {
							x = (price - trigger) * qty;
						} else {
							x = (trigger - price) * qty;
						}

						var y = co_lower * price * qty;

						// whichever is the highest is the margin
						margin = x > y ? x : y;
						margin += margin * .20;
					}
					break;
				case 'MCX':
					var co_lower = 0.01,
						co_upper = 0.019;

					var trigger = price - (co_upper * price);

					if (stl < trigger) {
						$('#stl').val(Math.ceil(trigger));
					} else {
						trigger = stl;
					}

					var x = 0;
					if (bors == "buy") {
						x = (price - trigger) * qty;
					} else {
						x = (trigger - price) * qty;
					}

					var y = co_lower * price * qty;

					// whichever is the highest is the margin
					margin = x > y ? x : y;
					margin += margin * .4;
					break;
				case 'CDS':
					var co_lower = parseFloat(CURRENCIES[scrip]["co_lower"]) / 100,
						co_upper = parseFloat(CURRENCIES[scrip]["co_upper"]) / 100;

					var trigger = price - (co_upper * price);
					if (stl < trigger) {
						$('#stl').val(Math.ceil(trigger));
					} else {
						trigger = stl;
					}

					var x = 0;
					if (bors == "buy") {
						x = (price - trigger) * qty;
					} else {
						x = (trigger - price) * qty;
					}

					var y = co_lower * price * qty;

					// whichever is the highest is the margin
					margin = x > y ? x : y;
					margin += margin * 0.20;
					break;
			}
			$('#actual-val').text(this.numberFormat(price * qty));
			$('#leverage').text(parseFloat((price * qty) / margin, 1).toFixed(1));
			$('#margin-req').text(this.numberFormat(Math.ceil(margin)));

			return false;

		},

		populateBOCO: function (span, exposure, spread, netoptionvalue, total) {
			// populate total box
			$('#tally .span').text('Rs: ' + Me.numberFormat(span));
			$('#tally .exposure').text('Rs: ' + Me.numberFormat(exposure));
			$('#tally .total').text('Rs: ' + Me.numberFormat(total));
		},


		// _______________________________________________

		numberFormat: function (num) {
			num = Math.round(num, 0);

			num += ''; // converts integer to string
			var explrestunits = '', thecash = null;
			if (num.length > 3) {
				var lastthree = num.substr(-3);
				var restunits = num.substr(0, num.length - 3);

				restunits = (restunits.length % 2 == 1) ? "0" + restunits : restunits;

				var expunit = restunits.match(/.{1,2}/g);
				for (i = 0; i < expunit.length; i++) {
					if (i == 0)
						explrestunits += parseInt(expunit[i]) + ',';
					else
						explrestunits += expunit[i] + ',';

					thecash = explrestunits + lastthree;
				}
			}
			else {
				thecash = num;
			}
			return thecash;
		}
	}

};