var tsorter = function() {
    "use strict";
    var a, b, c, d = !!document.addEventListener;
    return Object.create || (Object.create = function(a) {
        var b = function() {
            return void 0
        };
        return b.prototype = a, new b
    }), b = function(a, b, c) {
        d ? a.addEventListener(b, c, !1) : a.attachEvent("on" + b, c)
    }, c = function(a, b, c) {
        d ? a.removeEventListener(b, c, !1) : a.detachEvent("on" + b, c)
    }, a = {
        getCell: function(a) {
            var b = this;
            return b.trs[a].cells[b.column]
        },
        sort: function (a) {
            if ($(a.target).hasClass('no-sort')) { return; }
            var b = this,
                c = a.target;
            b.column = c.cellIndex, b.get = b.getAccessor(c.getAttribute("data-tsorter")), b.prevCol === b.column ? (c.className = "ascend" !== c.className ? "ascend" : "descend", b.reverseTable()) : (c.className = "ascend", -1 !== b.prevCol && "exc_cell" !== b.ths[b.prevCol].className && (b.ths[b.prevCol].className = ""), b.quicksort(0, b.trs.length)), b.prevCol = b.column
        },
        getAccessor: function(a) {
            var b = this,
                c = b.accessors;
            if (c && c[a]) return c[a];
            switch (a) {
                case "link":
                    return function(a) {
                        return b.getCell(a).firstChild.firstChild.nodeValue
                    };
                case "input":
                    return function(a) {
                        return b.getCell(a).firstChild.value
                    };
                case "numeric":
                    return function(a) {
                        return parseFloat(b.getCell(a).firstChild.nodeValue, 10)
                    };
                default:
                    return function(a) {
                        return b.getCell(a).firstChild.nodeValue
                    }
            }
        },
        exchange: function(a, b) {
            var c, d = this,
                e = d.tbody,
                f = d.trs;
            a === b + 1 ? e.insertBefore(f[a], f[b]) : b === a + 1 ? e.insertBefore(f[b], f[a]) : (c = e.replaceChild(f[a], f[b]), f[a] ? e.insertBefore(c, f[a]) : e.appendChild(c))
        },
        reverseTable: function() {
            var a, b = this;
            for (a = 1; a < b.trs.length; a++) b.tbody.insertBefore(b.trs[a], b.trs[0])
        },
        quicksort: function(a, b) {
            var c, d, e, f = this;
            if (!(a + 1 >= b)) {
                if (b - a === 2) return void(f.get(b - 1) > f.get(a) && f.exchange(b - 1, a));
                for (c = a + 1, d = b - 1, f.get(a) > f.get(c) && f.exchange(c, a), f.get(d) > f.get(a) && f.exchange(a, d), f.get(a) > f.get(c) && f.exchange(c, a), e = f.get(a);;) {
                    for (d--; e > f.get(d);) d--;
                    for (c++; f.get(c) > e;) c++;
                    if (c >= d) break;
                    f.exchange(c, d)
                }
                f.exchange(a, d), b - d > d - a ? (f.quicksort(a, d), f.quicksort(d + 1, b)) : (f.quicksort(d + 1, b), f.quicksort(a, d))
            }
        },
        init: function(a, c, d) {
            var e, f = this;
            for ("string" == typeof a && (a = document.getElementById(a)),
                f.table = a,
                f.ths = a.getElementsByTagName("th"),
                f.tbody = a.tBodies[0],
                f.trs = f.tbody.getElementsByTagName("tr"),
                f.prevCol = c && c > 0 ? c : -1,
                f.accessors = d,
                f.boundSort = f.sort.bind(f),
                e = 1;  /* Ben Gillis: do not bind first column, it is for table commands */
                e < f.ths.length; e++) b(f.ths[e], "click", f.boundSort)
        },
        destroy: function() {
            var a, b = this;
            if (b.ths)
                for (a = 0; a < b.ths.length; a++) c(b.ths[a], "click", b.boundSort)
        }
    }, {
        create: function(b, c, d) {
            var e = Object.create(a);
            return e.init(b, c, d), e
        }
    }
}();