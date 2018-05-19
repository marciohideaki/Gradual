function progress(a, b) {
    var c = a * b.width() / 100;
    b.find(".progressbar-value").animate({
        width: c
    }, 1200)
}

function themefromCookie() {
    var a = $.cookie("set-layout-theme"),
        b = $.cookie("set-framework-theme");
    null != a && $("#layout-color").attr("href", "/Resc/Skin/Supina/assets-minified/themes/supina/" + a + "/layout-color.css"), null != b && $("#framework-color").attr("href", "/Resc/Skin/Supina/assets-minified/themes/supina/" + b + "/framework-color.css")
}

function layoutFormatter() {
    setTimeout(function () {
        var a = $(window).height(),
            b = $("#page-header").height(),
            c = $("#page-nav").height(),
            d = a - b,
            e = a - b - c; /*$("#page-sidebar").height(d), $("#page-sidebar-wrapper").height(d),*/
        $("#page-content").css("min-height", e - 2)
    }, 499)
}

function init_page_transitions() {
    var a = [".pt-page-moveFromLeft", "pt-page-moveFromRight", "pt-page-moveFromTop", "pt-page-moveFromBottom", "pt-page-fade", "pt-page-moveFromLeftFade", "pt-page-moveFromRightFade", "pt-page-moveFromTopFade", "pt-page-moveFromBottomFade", "pt-page-scaleUp", "pt-page-scaleUpCenter", "pt-page-flipInLeft", "pt-page-flipInRight", "pt-page-flipInBottom", "pt-page-flipInTop", "pt-page-rotatePullRight", "pt-page-rotatePullLeft", "pt-page-rotatePullTop", "pt-page-rotatePullBottom", "pt-page-rotateUnfoldLeft", "pt-page-rotateUnfoldRight", "pt-page-rotateUnfoldTop", "pt-page-rotateUnfoldBottom"];
    for (var b in a) {
        var c = a[b];
        if ($(".add-transition").hasClass(c)) return $(".add-transition").addClass(c + "-init page-transition"), void setTimeout(function () {
            $(".add-transition").removeClass(c + " " + c + "-init page-transition")
        }, 1200)
    }
} + function (a) {
    "use strict";

    function b(b) {
        b && 3 === b.which || (a(e).remove(), a(f).each(function () {
            var d = a(this),
                e = c(d),
                f = {
                    relatedTarget: this
                };
            e.hasClass("open") && (e.trigger(b = a.Event("hide.bs.dropdown", f)), b.isDefaultPrevented() || (d.attr("aria-expanded", "false"), e.removeClass("open").trigger("hidden.bs.dropdown", f)))
        }))
    }

    function c(b) {
        var c = b.attr("data-target");
        c || (c = b.attr("href"), c = c && /#[A-Za-z]/.test(c) && c.replace(/.*(?=#[^\s]*$)/, ""));
        var d = c && a(c);
        return d && d.length ? d : b.parent()
    }

    function d(b) {
        return this.each(function () {
            var c = a(this),
                d = c.data("bs.dropdown");
            d || c.data("bs.dropdown", d = new g(this)), "string" == typeof b && d[b].call(c)
        })
    }
    var e = ".dropdown-backdrop",
        f = '[data-toggle="dropdown"]',
        g = function (b) {
            a(b).on("click.bs.dropdown", this.toggle)
        };
    g.VERSION = "3.2.0", g.prototype.toggle = function (d) {
        var e = a(this);
        if (!e.is(".disabled, :disabled")) {
            var f = c(e),
                g = f.hasClass("open");
            if (b(), !g) {
                "ontouchstart" in document.documentElement && !f.closest(".navbar-nav").length && a('<div class="dropdown-backdrop"/>').insertAfter(a(this)).on("click", b);
                var h = {
                    relatedTarget: this
                };
                if (f.trigger(d = a.Event("show.bs.dropdown", h)), d.isDefaultPrevented()) return;
                e.trigger("focus").attr("aria-expanded", "true"), f.toggleClass("open").trigger("shown.bs.dropdown", h)
            }
            return !1
        }
    }, g.prototype.keydown = function (b) {
        if (/(38|40|27)/.test(b.keyCode)) {
            var d = a(this);
            if (b.preventDefault(), b.stopPropagation(), !d.is(".disabled, :disabled")) {
                var e = c(d),
                    g = e.hasClass("open");
                if (!g || g && 27 == b.keyCode) return 27 == b.which && e.find(f).trigger("focus"), d.trigger("click");
                var h = " li:not(.divider):visible a",
                    i = e.find('[role="menu"]' + h + ', [role="listbox"]' + h);
                if (i.length) {
                    var j = i.index(i.filter(":focus"));
                    38 == b.keyCode && j > 0 && j--, 40 == b.keyCode && j < i.length - 1 && j++, ~j || (j = 0), i.eq(j).trigger("focus")
                }
            }
        }
    };
    var h = a.fn.dropdown;
    a.fn.dropdown = d, a.fn.dropdown.Constructor = g, a.fn.dropdown.noConflict = function () {
        return a.fn.dropdown = h, this
    }, a(document).on("click.bs.dropdown.data-api", b).on("click.bs.dropdown.data-api", ".dropdown form", function (a) {
        a.stopPropagation()
    }).on("click.bs.dropdown.data-api", f, g.prototype.toggle).on("keydown.bs.dropdown.data-api", f + ', [role="menu"], [role="listbox"]', g.prototype.keydown)
}(jQuery),
function (a) {
    return a.easyPieChart = function (b, c) {
        var d, e, f, g, h, i, j, k, l = this;
        return this.el = b, this.$el = a(b), this.$el.data("easyPieChart", this), this.init = function () {
            var b, d;
            return l.options = a.extend({}, a.easyPieChart.defaultOptions, c), b = parseInt(l.$el.data("percent"), 10), l.percentage = 0, l.canvas = a("<canvas width='" + l.options.size + "' height='" + l.options.size + "'></canvas>").get(0), l.$el.append(l.canvas), "undefined" != typeof G_vmlCanvasManager && null !== G_vmlCanvasManager && G_vmlCanvasManager.initElement(l.canvas), l.ctx = l.canvas.getContext("2d"), window.devicePixelRatio > 1 && (d = window.devicePixelRatio, a(l.canvas).css({
                width: l.options.size,
                height: l.options.size
            }), l.canvas.width *= d, l.canvas.height *= d, l.ctx.scale(d, d)), l.ctx.translate(l.options.size / 2, l.options.size / 2), l.ctx.rotate(l.options.rotate * Math.PI / 180), l.$el.addClass("easyPieChart"), l.$el.css({
                width: l.options.size,
                height: l.options.size,
                lineHeight: "" + l.options.size + "px"
            }), l.update(b), l
        }, this.update = function (a) {
            return a = parseFloat(a) || 0, l.options.animate === !1 ? f(a) : e(l.percentage, a), l
        }, j = function () {
            var a, b, c;
            for (l.ctx.fillStyle = l.options.scaleColor, l.ctx.lineWidth = 1, c = [], a = b = 0; 24 >= b; a = ++b) c.push(d(a));
            return c
        }, d = function (a) {
            var b;
            b = a % 6 === 0 ? 0 : .017 * l.options.size, l.ctx.save(), l.ctx.rotate(a * Math.PI / 12), l.ctx.fillRect(l.options.size / 2 - b, 0, .05 * -l.options.size + b, 1), l.ctx.restore()
        }, k = function () {
            var a;
            a = l.options.size / 2 - l.options.lineWidth / 2, l.options.scaleColor !== !1 && (a -= .08 * l.options.size), l.ctx.beginPath(), l.ctx.arc(0, 0, a, 0, 2 * Math.PI, !0), l.ctx.closePath(), l.ctx.strokeStyle = l.options.trackColor, l.ctx.lineWidth = l.options.lineWidth, l.ctx.stroke()
        }, i = function () {
            l.options.scaleColor !== !1 && j(), l.options.trackColor !== !1 && k()
        }, f = function (b) {
            var c;
            i(), l.ctx.strokeStyle = a.isFunction(l.options.barColor) ? l.options.barColor(b) : l.options.barColor, l.ctx.lineCap = l.options.lineCap, l.ctx.lineWidth = l.options.lineWidth, c = l.options.size / 2 - l.options.lineWidth / 2, l.options.scaleColor !== !1 && (c -= .08 * l.options.size), l.ctx.save(), l.ctx.rotate(-Math.PI / 2), l.ctx.beginPath(), l.ctx.arc(0, 0, c, 0, 2 * Math.PI * b / 100, !1), l.ctx.stroke(), l.ctx.restore()
        }, h = function () {
            return window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || function (a) {
                return window.setTimeout(a, 1e3 / 60)
            }
        }(), e = function (a, b) {
            var c, d;
            l.options.onStart.call(l), l.percentage = b, d = Date.now(), c = function () {
                var e, j;
                return j = Date.now() - d, j < l.options.animate && h(c), l.ctx.clearRect(-l.options.size / 2, -l.options.size / 2, l.options.size, l.options.size), i.call(l), e = [g(j, a, b - a, l.options.animate)], l.options.onStep.call(l, e), f.call(l, e), j >= l.options.animate ? l.options.onStop.call(l) : void 0
            }, h(c)
        }, g = function (a, b, c, d) {
            var e, f;
            return e = function (a) {
                return Math.pow(a, 2)
            }, f = function (a) {
                return 1 > a ? e(a) : 2 - e(a / 2 * -2 + 2)
            }, a /= d / 2, c / 2 * f(a) + b
        }, this.init()
    }, a.easyPieChart.defaultOptions = {
        barColor: "#ef1e25",
        trackColor: "#f2f2f2",
        scaleColor: "#dfe0e0",
        lineCap: "round",
        rotate: 0,
        size: 110,
        lineWidth: 3,
        animate: !1,
        onStart: a.noop,
        onStop: a.noop,
        onStep: a.noop
    }, void (a.fn.easyPieChart = function (b) {
        return a.each(this, function (c, d) {
            var e, f;
            return e = a(d), e.data("easyPieChart") ? void 0 : (f = a.extend({}, b, e.data()), e.data("easyPieChart", new a.easyPieChart(d, f)))
        })
    })
}(jQuery);
var initPieChart = function () {
    $(".chart").easyPieChart({
        barColor: function (a) {
            return a /= 100, "rgb(" + Math.round(254 * (1 - a)) + ", " + Math.round(255 * a) + ", 0)"
        },
        animate: 1e3,
        scaleColor: "#ccc",
        lineWidth: 3,
        size: 100,
        lineCap: "cap",
        onStep: function (a) {
            this.$el.find("span").text(~~a)
        }
    }), $(".chart-alt").easyPieChart({
        barColor: function (a) {
            return a /= 100, "rgb(" + Math.round(255 * (1 - a)) + ", " + Math.round(255 * a) + ", 0)"
        },
        trackColor: "#333",
        scaleColor: !1,
        lineCap: "butt",
        rotate: -90,
        lineWidth: 20,
        animate: 1500,
        onStep: function (a) {
            this.$el.find("span").text(~~a)
        }
    }), $(".chart-alt-1").easyPieChart({
        barColor: function (a) {
            return a /= 100, "rgb(" + Math.round(255 * (1 - a)) + ", " + Math.round(255 * a) + ", 0)"
        },
        trackColor: "#e1ecf1",
        scaleColor: "#c4d7e0",
        lineCap: "cap",
        rotate: -90,
        lineWidth: 10,
        size: 80,
        animate: 2500,
        onStep: function (a) {
            this.$el.find("span").text(~~a)
        }
    }), $(".chart-alt-2").easyPieChart({
        barColor: function (a) {
            return a /= 100, "rgb(" + Math.round(255 * (1 - a)) + ", " + Math.round(255 * a) + ", 0)"
        },
        trackColor: "#fff",
        scaleColor: !1,
        lineCap: "butt",
        rotate: -90,
        lineWidth: 4,
        size: 50,
        animate: 1500,
        onStep: function (a) {
            this.$el.find("span").text(~~a)
        }
    }), $(".chart-alt-3").easyPieChart({
        barColor: function (a) {
            return a /= 100, "rgb(" + Math.round(255 * (1 - a)) + ", " + Math.round(255 * a) + ", 0)"
        },
        trackColor: "#333",
        scaleColor: !0,
        lineCap: "butt",
        rotate: -90,
        lineWidth: 4,
        size: 50,
        animate: 1500,
        onStep: function (a) {
            this.$el.find("span").text(~~a)
        }
    }), $(".updateEasyPieChart").on("click", function (a) {
        a.preventDefault(), $(".chart, .chart-alt, .chart-alt-1, .chart-alt-2, .chart-alt-3").each(function () {
            $(this).data("easyPieChart").update(Math.round(100 * Math.random()))
        })
    })
};
$(document).ready(function () {
    initPieChart()
}), + function (a) {
    "use strict";

    function b(b) {
        return this.each(function () {
            var d = a(this),
                e = d.data("bs.tooltip"),
                f = "object" == typeof b && b;
            (e || "destroy" != b) && (e || d.data("bs.tooltip", e = new c(this, f)), "string" == typeof b && e[b]())
        })
    }
    var c = function (a, b) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null, this.init("tooltip", a, b)
    };
    c.VERSION = "3.2.0", c.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1,
        viewport: {
            selector: "body",
            padding: 0
        }
    }, c.prototype.init = function (b, c, d) {
        this.enabled = !0, this.type = b, this.$element = a(c), this.options = this.getOptions(d), this.$viewport = this.options.viewport && a(this.options.viewport.selector || this.options.viewport);
        for (var e = this.options.trigger.split(" "), f = e.length; f--;) {
            var g = e[f];
            if ("click" == g) this.$element.on("click." + this.type, this.options.selector, a.proxy(this.toggle, this));
            else if ("manual" != g) {
                var h = "hover" == g ? "mouseenter" : "focusin",
                    i = "hover" == g ? "mouseleave" : "focusout";
                this.$element.on(h + "." + this.type, this.options.selector, a.proxy(this.enter, this)), this.$element.on(i + "." + this.type, this.options.selector, a.proxy(this.leave, this))
            }
        }
        this.options.selector ? this._options = a.extend({}, this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    }, c.prototype.getDefaults = function () {
        return c.DEFAULTS
    }, c.prototype.getOptions = function (b) {
        return b = a.extend({}, this.getDefaults(), this.$element.data(), b), b.delay && "number" == typeof b.delay && (b.delay = {
            show: b.delay,
            hide: b.delay
        }), b
    }, c.prototype.getDelegateOptions = function () {
        var b = {},
            c = this.getDefaults();
        return this._options && a.each(this._options, function (a, d) {
            c[a] != d && (b[a] = d)
        }), b
    }, c.prototype.enter = function (b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget).data("bs." + this.type);
        return c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c)), clearTimeout(c.timeout), c.hoverState = "in", c.options.delay && c.options.delay.show ? void (c.timeout = setTimeout(function () {
            "in" == c.hoverState && c.show()
        }, c.options.delay.show)) : c.show()
    }, c.prototype.leave = function (b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget).data("bs." + this.type);
        return c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c)), clearTimeout(c.timeout), c.hoverState = "out", c.options.delay && c.options.delay.hide ? void (c.timeout = setTimeout(function () {
            "out" == c.hoverState && c.hide()
        }, c.options.delay.hide)) : c.hide()
    }, c.prototype.show = function () {
        var b = a.Event("show.bs." + this.type);
        if (this.hasContent() && this.enabled) {
            this.$element.trigger(b);
            var c = a.contains(document.documentElement, this.$element[0]);
            if (b.isDefaultPrevented() || !c) return;
            var d = this,
                e = this.tip(),
                f = this.getUID(this.type);
            this.setContent(), e.attr("id", f), this.$element.attr("aria-describedby", f), this.options.animation && e.addClass("fade");
            var g = "function" == typeof this.options.placement ? this.options.placement.call(this, e[0], this.$element[0]) : this.options.placement,
                h = /\s?auto?\s?/i,
                i = h.test(g);
            i && (g = g.replace(h, "") || "top"), e.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(g).data("bs." + this.type, this), this.options.container ? e.appendTo(this.options.container) : e.insertAfter(this.$element);
            var j = this.getPosition(),
                k = e[0].offsetWidth,
                l = e[0].offsetHeight;
            if (i) {
                var m = g,
                    n = this.$element.parent(),
                    o = this.getPosition(n);
                g = "bottom" == g && j.top + j.height + l - o.scroll > o.height ? "top" : "top" == g && j.top - o.scroll - l < 0 ? "bottom" : "right" == g && j.right + k > o.width ? "left" : "left" == g && j.left - k < o.left ? "right" : g, e.removeClass(m).addClass(g)
            }
            var p = this.getCalculatedOffset(g, j, k, l);
            this.applyPlacement(p, g);
            var q = function () {
                d.$element.trigger("shown.bs." + d.type), d.hoverState = null
            };
            a.support.transition && this.$tip.hasClass("fade") ? e.one("bsTransitionEnd", q).emulateTransitionEnd(150) : q()
        }
    }, c.prototype.applyPlacement = function (b, c) {
        var d = this.tip(),
            e = d[0].offsetWidth,
            f = d[0].offsetHeight,
            g = parseInt(d.css("margin-top"), 10),
            h = parseInt(d.css("margin-left"), 10);
        isNaN(g) && (g = 0), isNaN(h) && (h = 0), b.top = b.top + g, b.left = b.left + h, a.offset.setOffset(d[0], a.extend({
            using: function (a) {
                d.css({
                    top: Math.round(a.top),
                    left: Math.round(a.left)
                })
            }
        }, b), 0), d.addClass("in");
        var i = d[0].offsetWidth,
            j = d[0].offsetHeight;
        "top" == c && j != f && (b.top = b.top + f - j);
        var k = this.getViewportAdjustedDelta(c, b, i, j);
        k.left ? b.left += k.left : b.top += k.top;
        var l = k.left ? 2 * k.left - e + i : 2 * k.top - f + j,
            m = k.left ? "left" : "top",
            n = k.left ? "offsetWidth" : "offsetHeight";
        d.offset(b), this.replaceArrow(l, d[0][n], m)
    }, c.prototype.replaceArrow = function (a, b, c) {
        this.arrow().css(c, a ? 50 * (1 - a / b) + "%" : "")
    }, c.prototype.setContent = function () {
        var a = this.tip(),
            b = this.getTitle();
        a.find(".tooltip-inner")[this.options.html ? "html" : "text"](b), a.removeClass("fade in top bottom left right")
    }, c.prototype.hide = function () {
        function b() {
            "in" != c.hoverState && d.detach(), c.$element.trigger("hidden.bs." + c.type)
        }
        var c = this,
            d = this.tip(),
            e = a.Event("hide.bs." + this.type);
        return this.$element.removeAttr("aria-describedby"), this.$element.trigger(e), e.isDefaultPrevented() ? void 0 : (d.removeClass("in"), a.support.transition && this.$tip.hasClass("fade") ? d.one("bsTransitionEnd", b).emulateTransitionEnd(150) : b(), this.hoverState = null, this)
    }, c.prototype.fixTitle = function () {
        var a = this.$element;
        (a.attr("title") || "string" != typeof a.attr("data-original-title")) && a.attr("data-original-title", a.attr("title") || "").attr("title", "")
    }, c.prototype.hasContent = function () {
        return this.getTitle()
    }, c.prototype.getPosition = function (b) {
        b = b || this.$element;
        var c = b[0],
            d = "BODY" == c.tagName,
            e = window.SVGElement && c instanceof window.SVGElement,
            f = c.getBoundingClientRect ? c.getBoundingClientRect() : null,
            g = d ? {
                top: 0,
                left: 0
            } : b.offset(),
            h = {
                scroll: d ? document.documentElement.scrollTop || document.body.scrollTop : b.scrollTop()
            },
            i = e ? {} : {
                width: d ? a(window).width() : b.outerWidth(),
                height: d ? a(window).height() : b.outerHeight()
            };
        return a.extend({}, f, h, i, g)
    }, c.prototype.getCalculatedOffset = function (a, b, c, d) {
        return "bottom" == a ? {
            top: b.top + b.height,
            left: b.left + b.width / 2 - c / 2
        } : "top" == a ? {
            top: b.top - d,
            left: b.left + b.width / 2 - c / 2
        } : "left" == a ? {
            top: b.top + b.height / 2 - d / 2,
            left: b.left - c
        } : {
            top: b.top + b.height / 2 - d / 2,
            left: b.left + b.width
        }
    }, c.prototype.getViewportAdjustedDelta = function (a, b, c, d) {
        var e = {
            top: 0,
            left: 0
        };
        if (!this.$viewport) return e;
        var f = this.options.viewport && this.options.viewport.padding || 0,
            g = this.getPosition(this.$viewport);
        if (/right|left/.test(a)) {
            var h = b.top - f - g.scroll,
                i = b.top + f - g.scroll + d;
            h < g.top ? e.top = g.top - h : i > g.top + g.height && (e.top = g.top + g.height - i)
        } else {
            var j = b.left - f,
                k = b.left + f + c;
            j < g.left ? e.left = g.left - j : k > g.width && (e.left = g.left + g.width - k)
        }
        return e
    }, c.prototype.getTitle = function () {
        var a, b = this.$element,
            c = this.options;
        return a = b.attr("data-original-title") || ("function" == typeof c.title ? c.title.call(b[0]) : c.title)
    }, c.prototype.getUID = function (a) {
        do a += ~~(1e6 * Math.random()); while (document.getElementById(a));
        return a
    }, c.prototype.tip = function () {
        return this.$tip = this.$tip || a(this.options.template)
    }, c.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    }, c.prototype.validate = function () {
        this.$element[0].parentNode || (this.hide(), this.$element = null, this.options = null)
    }, c.prototype.enable = function () {
        this.enabled = !0
    }, c.prototype.disable = function () {
        this.enabled = !1
    }, c.prototype.toggleEnabled = function () {
        this.enabled = !this.enabled
    }, c.prototype.toggle = function (b) {
        var c = this;
        b && (c = a(b.currentTarget).data("bs." + this.type), c || (c = new this.constructor(b.currentTarget, this.getDelegateOptions()), a(b.currentTarget).data("bs." + this.type, c))), c.tip().hasClass("in") ? c.leave(c) : c.enter(c)
    }, c.prototype.destroy = function () {
        clearTimeout(this.timeout), this.hide().$element.off("." + this.type).removeData("bs." + this.type)
    };
    var d = a.fn.tooltip;
    a.fn.tooltip = b, a.fn.tooltip.Constructor = c, a.fn.tooltip.noConflict = function () {
        return a.fn.tooltip = d, this
    }
}(jQuery), + function (a) {
    "use strict";

    function b(b) {
        return this.each(function () {
            var d = a(this),
                e = d.data("bs.popover"),
                f = "object" == typeof b && b;
            (e || "destroy" != b) && (e || d.data("bs.popover", e = new c(this, f)), "string" == typeof b && e[b]())
        })
    }
    var c = function (a, b) {
        this.init("popover", a, b)
    };
    if (!a.fn.tooltip) throw new Error("Popover requires tooltip.js");
    c.VERSION = "3.2.0", c.DEFAULTS = a.extend({}, a.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    }), c.prototype = a.extend({}, a.fn.tooltip.Constructor.prototype), c.prototype.constructor = c, c.prototype.getDefaults = function () {
        return c.DEFAULTS
    }, c.prototype.setContent = function () {
        var a = this.tip(),
            b = this.getTitle(),
            c = this.getContent();
        a.find(".popover-title")[this.options.html ? "html" : "text"](b), a.find(".popover-content").empty()[this.options.html ? "string" == typeof c ? "html" : "append" : "text"](c), a.removeClass("fade top bottom left right in"), a.find(".popover-title").html() || a.find(".popover-title").hide()
    }, c.prototype.hasContent = function () {
        return this.getTitle() || this.getContent()
    }, c.prototype.getContent = function () {
        var a = this.$element,
            b = this.options;
        return a.attr("data-content") || ("function" == typeof b.content ? b.content.call(a[0]) : b.content)
    }, c.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    }, c.prototype.tip = function () {
        return this.$tip || (this.$tip = a(this.options.template)), this.$tip
    };
    var d = a.fn.popover;
    a.fn.popover = b, a.fn.popover.Constructor = c, a.fn.popover.noConflict = function () {
        return a.fn.popover = d, this
    }
}(jQuery), + function (a) {
    "use strict";

    function b(b) {
        return this.each(function () {
            var d = a(this),
                e = d.data("bs.button"),
                f = "object" == typeof b && b;
            e || d.data("bs.button", e = new c(this, f)), "toggle" == b ? e.toggle() : b && e.setState(b)
        })
    }
    var c = function (b, d) {
        this.$element = a(b), this.options = a.extend({}, c.DEFAULTS, d), this.isLoading = !1
    };
    c.VERSION = "3.2.0", c.DEFAULTS = {
        loadingText: "loading..."
    }, c.prototype.setState = function (b) {
        var c = "disabled",
            d = this.$element,
            e = d.is("input") ? "val" : "html",
            f = d.data();
        b += "Text", null == f.resetText && d.data("resetText", d[e]()), d[e](null == f[b] ? this.options[b] : f[b]), setTimeout(a.proxy(function () {
            "loadingText" == b ? (this.isLoading = !0, d.addClass(c).attr(c, c)) : this.isLoading && (this.isLoading = !1, d.removeClass(c).removeAttr(c))
        }, this), 0)
    }, c.prototype.toggle = function () {
        var a = !0,
            b = this.$element.closest('[data-toggle="buttons"]');
        if (b.length) {
            var c = this.$element.find("input");
            "radio" == c.prop("type") && (c.prop("checked") && this.$element.hasClass("active") ? a = !1 : b.find(".active").removeClass("active")), a && c.prop("checked", !this.$element.hasClass("active")).trigger("change")
        }
        a && this.$element.toggleClass("active")
    };
    var d = a.fn.button;
    a.fn.button = b, a.fn.button.Constructor = c, a.fn.button.noConflict = function () {
        return a.fn.button = d, this
    }, a(document).on("click.bs.button.data-api", '[data-toggle^="button"]', function (c) {
        var d = a(c.target);
        d.hasClass("btn") || (d = d.closest(".btn")), b.call(d, "toggle"), c.preventDefault()
    }).on("focus.bs.button.data-api blur.bs.button.data-api", '[data-toggle^="button"]', function (b) {
        a(b.target).closest(".btn").toggleClass("focus", "focus" == b.type)
    })
}(jQuery), + function (a) {
    "use strict";

    function b(b) {
        return this.each(function () {
            var d = a(this),
                e = d.data("bs.collapse"),
                f = a.extend({}, c.DEFAULTS, d.data(), "object" == typeof b && b);
            !e && f.toggle && "show" == b && (b = !b), e || d.data("bs.collapse", e = new c(this, f)), "string" == typeof b && e[b]()
        })
    }
    var c = function (b, d) {
        this.$element = a(b), this.options = a.extend({}, c.DEFAULTS, d), this.transitioning = null, this.options.parent && (this.$parent = a(this.options.parent)), this.options.toggle && this.toggle()
    };
    c.VERSION = "3.2.0", c.DEFAULTS = {
        toggle: !0
    }, c.prototype.dimension = function () {
        var a = this.$element.hasClass("width");
        return a ? "width" : "height"
    }, c.prototype.show = function () {
        if (!this.transitioning && !this.$element.hasClass("in")) {
            var c = a.Event("show.bs.collapse");
            if (this.$element.trigger(c), !c.isDefaultPrevented()) {
                var d = this.$parent && this.$parent.find("> .panel > .in");
                if (d && d.length) {
                    var e = d.data("bs.collapse");
                    if (e && e.transitioning) return;
                    b.call(d, "hide"), e || d.data("bs.collapse", null)
                }
                var f = this.dimension();
                this.$element.removeClass("collapse").addClass("collapsing")[f](0), this.transitioning = 1;
                var g = function () {
                    this.$element.removeClass("collapsing").addClass("collapse in")[f](""), this.transitioning = 0, this.$element.trigger("shown.bs.collapse")
                };
                if (!a.support.transition) return g.call(this);
                var h = a.camelCase(["scroll", f].join("-"));
                this.$element.one("bsTransitionEnd", a.proxy(g, this)).emulateTransitionEnd(350)[f](this.$element[0][h])
            }
        }
    }, c.prototype.hide = function () {
        if (!this.transitioning && this.$element.hasClass("in")) {
            var b = a.Event("hide.bs.collapse");
            if (this.$element.trigger(b), !b.isDefaultPrevented()) {
                var c = this.dimension();
                this.$element[c](this.$element[c]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse in"), this.transitioning = 1;
                var d = function () {
                    this.transitioning = 0, this.$element.trigger("hidden.bs.collapse").removeClass("collapsing").addClass("collapse")
                };
                return a.support.transition ? void this.$element[c](0).one("bsTransitionEnd", a.proxy(d, this)).emulateTransitionEnd(350) : d.call(this)
            }
        }
    }, c.prototype.toggle = function () {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    };
    var d = a.fn.collapse;
    a.fn.collapse = b, a.fn.collapse.Constructor = c, a.fn.collapse.noConflict = function () {
        return a.fn.collapse = d, this
    }, a(document).on("click.bs.collapse.data-api", '[data-toggle="collapse"]', function (c) {
        var d, e = a(this),
            f = e.attr("data-target") || c.preventDefault() || (d = e.attr("href")) && d.replace(/.*(?=#[^\s]+$)/, ""),
            g = a(f),
            h = g.data("bs.collapse"),
            i = h ? "toggle" : e.data(),
            j = e.attr("data-parent"),
            k = j && a(j);
        h && h.transitioning || (k && k.find('[data-toggle="collapse"][data-parent="' + j + '"]').not(e).addClass("collapsed"), e.toggleClass("collapsed", g.hasClass("in"))), b.call(g, i)
    })
}(jQuery), $(document).on("ready", function () {
    $(".progressbar").each(function () {
        var a = $(this),
            b = $(this).attr("data-value");
        progress(b, a)
    })
}), $(function () {
    $("#header-right, .updateEasyPieChart, .complete-user-profile, #progress-dropdown, .sb-slidebar .scrollable-content").hover(function () {
        $(".progressbar").each(function () {
            var a = $(this),
                b = $(this).attr("data-value");
            progress(b, a)
        })
    })
}),
    function (a, b) {
        "use strict";

        function c(a) {
            var b = Array.prototype.slice.call(arguments, 1);
            return a.prop ? a.prop.apply(a, b) : a.attr.apply(a, b)
        }

        function d(a, b, c) {
            var d, e;
            for (d in c) c.hasOwnProperty(d) && (e = d.replace(/ |$/g, b.eventNamespace), a.bind(e, c[d]))
        }

        function e(a, b, c) {
            d(a, c, {
                focus: function () {
                    b.addClass(c.focusClass)
                },
                blur: function () {
                    b.removeClass(c.focusClass), b.removeClass(c.activeClass)
                },
                mouseenter: function () {
                    b.addClass(c.hoverClass)
                },
                mouseleave: function () {
                    b.removeClass(c.hoverClass), b.removeClass(c.activeClass)
                },
                "mousedown touchbegin": function () {
                    a.is(":disabled") || b.addClass(c.activeClass)
                },
                "mouseup touchend": function () {
                    b.removeClass(c.activeClass)
                }
            })
        }

        function f(a, b) {
            a.removeClass(b.hoverClass + " " + b.focusClass + " " + b.activeClass)
        }

        function g(a, b, c) {
            c ? a.addClass(b) : a.removeClass(b)
        }

        function h(a, b, c) {
            var d = "checked",
                e = b.is(":" + d);
            b.prop ? b.prop(d, e) : e ? b.attr(d, d) : b.removeAttr(d), g(a, c.checkedClass, e)
        }

        function i(a, b, c) {
            g(a, c.disabledClass, b.is(":disabled"))
        }

        function j(a, b, c) {
            switch (c) {
                case "after":
                    return a.after(b), a.next();
                case "before":
                    return a.before(b), a.prev();
                case "wrap":
                    return a.wrap(b), a.parent()
            }
            return null
        }

        function k(b, d, e) {
            var f, g, h;
            return e || (e = {}), e = a.extend({
                bind: {},
                divClass: null,
                divWrap: "wrap",
                spanClass: null,
                spanHtml: null,
                spanWrap: "wrap"
            }, e), f = a("<div />"), g = a("<span />"), d.autoHide && b.is(":hidden") && "none" === b.css("display") && f.hide(), e.divClass && f.addClass(e.divClass), d.wrapperClass && f.addClass(d.wrapperClass), e.spanClass && g.addClass(e.spanClass), h = c(b, "id"), d.useID && h && c(f, "id", d.idPrefix + "-" + h), e.spanHtml && g.html(e.spanHtml), f = j(b, f, e.divWrap), g = j(b, g, e.spanWrap), i(f, b, d), {
                div: f,
                span: g
            }
        }

        function l(b, c) {
            var d;
            return c.wrapperClass ? (d = a("<span />").addClass(c.wrapperClass), d = j(b, d, "wrap")) : null
        }

        function m() {
            var b, c, d, e;
            return e = "rgb(120,2,153)", c = a('<div style="width:0;height:0;color:' + e + '">'), a("body").append(c), d = c.get(0), b = window.getComputedStyle ? window.getComputedStyle(d, "").color : (d.currentStyle || d.style || {}).color, c.remove(), b.replace(/ /g, "") !== e
        }

        function n(b) {
            return b ? a("<span />").text(b).html() : ""
        }

        function o() {
            return navigator.cpuClass && !navigator.product
        }

        function p() {
            return "undefined" != typeof window.XMLHttpRequest ? !0 : !1
        }

        function q(a) {
            var b;
            return a[0].multiple ? !0 : (b = c(a, "size"), !b || 1 >= b ? !1 : !0)
        }

        function r() {
            return !1
        }

        function s(a, b) {
            var c = "none";
            d(a, b, {
                "selectstart dragstart mousedown": r
            }), a.css({
                MozUserSelect: c,
                msUserSelect: c,
                webkitUserSelect: c,
                userSelect: c
            })
        }

        function t(a, b, c) {
            var d = a.val();
            "" === d ? d = c.fileDefaultHtml : (d = d.split(/[\/\\]+/), d = d[d.length - 1]), b.text(d)
        }

        function u(a, b, c) {
            var d, e;
            for (d = [], a.each(function () {
                var a;
                for (a in b) Object.prototype.hasOwnProperty.call(b, a) && (d.push({
                el: this,
                name: a,
                old: this.style[a]
            }), this.style[a] = b[a])
            }), c() ; d.length;) e = d.pop(), e.el.style[e.name] = e.old
        }

        function v(a, b) {
            var c;
            c = a.parents(), c.push(a[0]), c = c.not(":visible"), u(c, {
                visibility: "hidden",
                display: "block",
                position: "absolute"
            }, b)
        }

        function w(a, b) {
            return function () {
                a.unwrap().unwrap().unbind(b.eventNamespace)
            }
        }
        var x = !0,
            y = !1,
            z = [{
                match: function (a) {
                    return a.is("a, button, :submit, :reset, input[type='button']")
                },
                apply: function (a, b) {
                    var g, h, j, l, m;
                    return h = b.submitDefaultHtml, a.is(":reset") && (h = b.resetDefaultHtml), l = a.is("a, button") ? function () {
                        return a.html() || h
                    } : function () {
                        return n(c(a, "value")) || h
                    }, j = k(a, b, {
                        divClass: b.buttonClass,
                        spanHtml: l()
                    }), g = j.div, e(a, g, b), m = !1, d(g, b, {
                        "click touchend": function () {
                            var b, d, e, f;
                            m || a.is(":disabled") || (m = !0, a[0].dispatchEvent ? (b = document.createEvent("MouseEvents"), b.initEvent("click", !0, !0), d = a[0].dispatchEvent(b), a.is("a") && d && (e = c(a, "target"), f = c(a, "href"), e && "_self" !== e ? window.open(f, e) : document.location.href = f)) : a.click(), m = !1)
                        }
                    }), s(g, b), {
                        remove: function () {
                            return g.after(a), g.remove(), a.unbind(b.eventNamespace), a
                        },
                        update: function () {
                            f(g, b), i(g, a, b), a.detach(), j.span.html(l()).append(a)
                        }
                    }
                }
            }, {
                match: function (a) {
                    return a.is(":checkbox")
                },
                apply: function (a, b) {
                    var c, g, j;
                    return c = k(a, b, {
                        divClass: b.checkboxClass
                    }), g = c.div, j = c.span, e(a, g, b), d(a, b, {
                        "click touchend": function () {
                            h(j, a, b)
                        }
                    }), h(j, a, b), {
                        remove: w(a, b),
                        update: function () {
                            f(g, b), j.removeClass(b.checkedClass), h(j, a, b), i(g, a, b)
                        }
                    }
                }
            }, {
                match: function (a) {
                    return a.is(":file")
                },
                apply: function (b, g) {
                    function h() {
                        t(b, n, g)
                    }
                    var l, m, n, p;
                    return l = k(b, g, {
                        divClass: g.fileClass,
                        spanClass: g.fileButtonClass,
                        spanHtml: g.fileButtonHtml,
                        spanWrap: "after"
                    }), m = l.div, p = l.span, n = a("<span />").html(g.fileDefaultHtml), n.addClass(g.filenameClass), n = j(b, n, "after"), c(b, "size") || c(b, "size", m.width() / 10), e(b, m, g), h(), o() ? d(b, g, {
                        click: function () {
                            b.trigger("change"), setTimeout(h, 0)
                        }
                    }) : d(b, g, {
                        change: h
                    }), s(n, g), s(p, g), {
                        remove: function () {
                            return n.remove(), p.remove(), b.unwrap().unbind(g.eventNamespace)
                        },
                        update: function () {
                            f(m, g), t(b, n, g), i(m, b, g)
                        }
                    }
                }
            }, {
                match: function (a) {
                    if (a.is("input")) {
                        var b = (" " + c(a, "type") + " ").toLowerCase(),
                            d = " color date datetime datetime-local email month number password search tel text time url week ";
                        return d.indexOf(b) >= 0
                    }
                    return !1
                },
                apply: function (a, b) {
                    var d, f;
                    return d = c(a, "type"), a.addClass(b.inputClass), f = l(a, b), e(a, a, b), b.inputAddTypeAsClass && a.addClass(d), {
                        remove: function () {
                            a.removeClass(b.inputClass), b.inputAddTypeAsClass && a.removeClass(d), f && a.unwrap()
                        },
                        update: r
                    }
                }
            }, {
                match: function (a) {
                    return a.is(":radio")
                },
                apply: function (b, g) {
                    var j, l, m;
                    return j = k(b, g, {
                        divClass: g.radioClass
                    }), l = j.div, m = j.span, e(b, l, g), d(b, g, {
                        "click touchend": function () {
                            a.uniform.update(a(':radio[name="' + c(b, "name") + '"]'))
                        }
                    }), h(m, b, g), {
                        remove: w(b, g),
                        update: function () {
                            f(l, g), h(m, b, g), i(l, b, g)
                        }
                    }
                }
            }, {
                match: function (a) {
                    return a.is("select") && !q(a) ? !0 : !1
                },
                apply: function (b, c) {
                    var g, h, j, l;
                    return c.selectAutoWidth && v(b, function () {
                        l = b.width()
                    }), g = k(b, c, {
                        divClass: c.selectClass,
                        spanHtml: (b.find(":selected:first") || b.find("option:first")).html(),
                        spanWrap: "before"
                    }), h = g.div, j = g.span, c.selectAutoWidth ? v(b, function () {
                        u(a([j[0], h[0]]), {
                            display: "block"
                        }, function () {
                            var a;
                            a = j.outerWidth() - j.width(), h.width(l + a), j.width(l)
                        })
                    }) : h.addClass("fixedWidth"), e(b, h, c), d(b, c, {
                        change: function () {
                            j.html(b.find(":selected").html()), h.removeClass(c.activeClass)
                        },
                        "click touchend": function () {
                            var a = b.find(":selected").html();
                            j.html() !== a && b.trigger("change")
                        },
                        keyup: function () {
                            j.html(b.find(":selected").html())
                        }
                    }), s(j, c), {
                        remove: function () {
                            return j.remove(), b.unwrap().unbind(c.eventNamespace), b
                        },
                        update: function () {
                            c.selectAutoWidth ? (a.uniform.restore(b), b.uniform(c)) : (f(h, c), j.html(b.find(":selected").html()), i(h, b, c))
                        }
                    }
                }
            }, {
                match: function (a) {
                    return a.is("select") && q(a) ? !0 : !1
                },
                apply: function (a, b) {
                    var c;
                    return a.addClass(b.selectMultiClass), c = l(a, b), e(a, a, b), {
                        remove: function () {
                            a.removeClass(b.selectMultiClass), c && a.unwrap()
                        },
                        update: r
                    }
                }
            }, {
                match: function (a) {
                    return a.is("textarea")
                },
                apply: function (a, b) {
                    var c;
                    return a.addClass(b.textareaClass), c = l(a, b), e(a, a, b), {
                        remove: function () {
                            a.removeClass(b.textareaClass), c && a.unwrap()
                        },
                        update: r
                    }
                }
            }];
        o() && !p() && (x = !1), a.uniform = {
            defaults: {
                activeClass: "active",
                autoHide: !0,
                buttonClass: "button",
                checkboxClass: "checker",
                checkedClass: "checked",
                disabledClass: "disabled",
                eventNamespace: ".uniform",
                fileButtonClass: "action",
                fileButtonHtml: "Choose File",
                fileClass: "uploader",
                fileDefaultHtml: "No file selected",
                filenameClass: "filename",
                focusClass: "focus",
                hoverClass: "hover",
                idPrefix: "uniform",
                inputAddTypeAsClass: !0,
                inputClass: "uniform-input",
                radioClass: "radio",
                resetDefaultHtml: "Reset",
                resetSelector: !1,
                selectAutoWidth: !0,
                selectClass: "selector",
                selectMultiClass: "uniform-multiselect",
                submitDefaultHtml: "Submit",
                textareaClass: "uniform",
                useID: !0,
                wrapperClass: null
            },
            elements: []
        }, a.fn.uniform = function (b) {
            var c = this;
            return b = a.extend({}, a.uniform.defaults, b), y || (y = !0, m() && (x = !1)), x ? (b.resetSelector && a(b.resetSelector).mouseup(function () {
                window.setTimeout(function () {
                    a.uniform.update(c)
                }, 10)
            }), this.each(function () {
                var c, d, e, f = a(this);
                if (f.data("uniformed")) return void a.uniform.update(f);
                for (c = 0; c < z.length; c += 1)
                    if (d = z[c], d.match(f, b)) return e = d.apply(f, b), f.data("uniformed", e), void a.uniform.elements.push(f.get(0))
            })) : this
        }, a.uniform.restore = a.fn.uniform.restore = function (c) {
            c === b && (c = a.uniform.elements), a(c).each(function () {
                var b, c, d = a(this);
                c = d.data("uniformed"), c && (c.remove(), b = a.inArray(this, a.uniform.elements), b >= 0 && a.uniform.elements.splice(b, 1), d.removeData("uniformed"))
            })
        }, a.uniform.update = a.fn.uniform.update = function (c) {
            c === b && (c = a.uniform.elements), a(c).each(function () {
                var b, c = a(this);
                b = c.data("uniformed"), b && b.update(c, b.options)
            })
        }
    }(jQuery),
    function () {
        var a, b, c, d, e, f = {}.hasOwnProperty,
            g = function (a, b) {
                function c() {
                    this.constructor = a
                }
                for (var d in b) f.call(b, d) && (a[d] = b[d]);
                return c.prototype = b.prototype, a.prototype = new c, a.__super__ = b.prototype, a
            };
        d = function () {
            function a() {
                this.options_index = 0, this.parsed = []
            }
            return a.prototype.add_node = function (a) {
                return "OPTGROUP" === a.nodeName.toUpperCase() ? this.add_group(a) : this.add_option(a)
            }, a.prototype.add_group = function (a) {
                var b, c, d, e, f, g;
                for (b = this.parsed.length, this.parsed.push({
                    array_index: b,
                    group: !0,
                    label: this.escapeExpression(a.label),
                    children: 0,
                    disabled: a.disabled
                }), f = a.childNodes, g = [], d = 0, e = f.length; e > d; d++) c = f[d], g.push(this.add_option(c, b, a.disabled));
                return g
            }, a.prototype.add_option = function (a, b, c) {
                return "OPTION" === a.nodeName.toUpperCase() ? ("" !== a.text ? (null != b && (this.parsed[b].children += 1), this.parsed.push({
                    array_index: this.parsed.length,
                    options_index: this.options_index,
                    value: a.value,
                    text: a.text,
                    html: a.innerHTML,
                    selected: a.selected,
                    disabled: c === !0 ? c : a.disabled,
                    group_array_index: b,
                    classes: a.className,
                    style: a.style.cssText
                })) : this.parsed.push({
                    array_index: this.parsed.length,
                    options_index: this.options_index,
                    empty: !0
                }), this.options_index += 1) : void 0
            }, a.prototype.escapeExpression = function (a) {
                var b, c;
                return null == a || a === !1 ? "" : /[\&\<\>\"\'\`]/.test(a) ? (b = {
                    "<": "&lt;",
                    ">": "&gt;",
                    '"': "&quot;",
                    "'": "&#x27;",
                    "`": "&#x60;"
                }, c = /&(?!\w+;)|[\<\>\"\'\`]/g, a.replace(c, function (a) {
                    return b[a] || "&amp;"
                })) : a
            }, a
        }(), d.select_to_array = function (a) {
            var b, c, e, f, g;
            for (c = new d, g = a.childNodes, e = 0, f = g.length; f > e; e++) b = g[e], c.add_node(b);
            return c.parsed
        }, b = function () {
            function a(b, c) {
                this.form_field = b, this.options = null != c ? c : {}, a.browser_is_supported() && (this.is_multiple = this.form_field.multiple, this.set_default_text(), this.set_default_values(), this.setup(), this.set_up_html(), this.register_observers())
            }
            return a.prototype.set_default_values = function () {
                var a = this;
                return this.click_test_action = function (b) {
                    return a.test_active_click(b)
                }, this.activate_action = function (b) {
                    return a.activate_field(b)
                }, this.active_field = !1, this.mouse_on_container = !1, this.results_showing = !1, this.result_highlighted = null, this.allow_single_deselect = null != this.options.allow_single_deselect && null != this.form_field.options[0] && "" === this.form_field.options[0].text ? this.options.allow_single_deselect : !1, this.disable_search_threshold = this.options.disable_search_threshold || 0, this.disable_search = this.options.disable_search || !1, this.enable_split_word_search = null != this.options.enable_split_word_search ? this.options.enable_split_word_search : !0, this.group_search = null != this.options.group_search ? this.options.group_search : !0, this.search_contains = this.options.search_contains || !1, this.single_backstroke_delete = null != this.options.single_backstroke_delete ? this.options.single_backstroke_delete : !0, this.max_selected_options = this.options.max_selected_options || 1 / 0, this.inherit_select_classes = this.options.inherit_select_classes || !1, this.display_selected_options = null != this.options.display_selected_options ? this.options.display_selected_options : !0, this.display_disabled_options = null != this.options.display_disabled_options ? this.options.display_disabled_options : !0
            }, a.prototype.set_default_text = function () {
                return this.default_text = this.form_field.getAttribute("data-placeholder") ? this.form_field.getAttribute("data-placeholder") : this.is_multiple ? this.options.placeholder_text_multiple || this.options.placeholder_text || a.default_multiple_text : this.options.placeholder_text_single || this.options.placeholder_text || a.default_single_text, this.results_none_found = this.form_field.getAttribute("data-no_results_text") || this.options.no_results_text || a.default_no_result_text
            }, a.prototype.mouse_enter = function () {
                return this.mouse_on_container = !0
            }, a.prototype.mouse_leave = function () {
                return this.mouse_on_container = !1
            }, a.prototype.input_focus = function () {
                var a = this;
                if (this.is_multiple) {
                    if (!this.active_field) return setTimeout(function () {
                        return a.container_mousedown()
                    }, 50)
                } else if (!this.active_field) return this.activate_field()
            }, a.prototype.input_blur = function () {
                var a = this;
                return this.mouse_on_container ? void 0 : (this.active_field = !1, setTimeout(function () {
                    return a.blur_test()
                }, 100))
            }, a.prototype.results_option_build = function (a) {
                var b, c, d, e, f;
                for (b = "", f = this.results_data, d = 0, e = f.length; e > d; d++) c = f[d], b += c.group ? this.result_add_group(c) : this.result_add_option(c), (null != a ? a.first : void 0) && (c.selected && this.is_multiple ? this.choice_build(c) : c.selected && !this.is_multiple && this.single_set_selected_text(c.text));
                return b
            }, a.prototype.result_add_option = function (a) {
                var b, c;
                return a.search_match && this.include_option_in_results(a) ? (b = [], a.disabled || a.selected && this.is_multiple || b.push("active-result"), !a.disabled || a.selected && this.is_multiple || b.push("disabled-result"), a.selected && b.push("result-selected"), null != a.group_array_index && b.push("group-option"), "" !== a.classes && b.push(a.classes), c = document.createElement("li"), c.className = b.join(" "), c.style.cssText = a.style, c.setAttribute("data-option-array-index", a.array_index), c.innerHTML = a.search_text, this.outerHTML(c)) : ""
            }, a.prototype.result_add_group = function (a) {
                var b;
                return (a.search_match || a.group_match) && a.active_options > 0 ? (b = document.createElement("li"), b.className = "group-result", b.innerHTML = a.search_text, this.outerHTML(b)) : ""
            }, a.prototype.results_update_field = function () {
                return this.set_default_text(), this.is_multiple || this.results_reset_cleanup(), this.result_clear_highlight(), this.results_build(), this.results_showing ? this.winnow_results() : void 0
            }, a.prototype.reset_single_select_options = function () {
                var a, b, c, d, e;
                for (d = this.results_data, e = [], b = 0, c = d.length; c > b; b++) a = d[b], e.push(a.selected ? a.selected = !1 : void 0);
                return e
            }, a.prototype.results_toggle = function () {
                return this.results_showing ? this.results_hide() : this.results_show()
            }, a.prototype.results_search = function () {
                return this.results_showing ? this.winnow_results() : this.results_show()
            }, a.prototype.winnow_results = function () {
                var a, b, c, d, e, f, g, h, i, j, k, l, m;
                for (this.no_results_clear(), e = 0, g = this.get_search_text(), a = g.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&"), d = this.search_contains ? "" : "^", c = new RegExp(d + a, "i"), j = new RegExp(a, "i"), m = this.results_data, k = 0, l = m.length; l > k; k++) b = m[k], b.search_match = !1, f = null, this.include_option_in_results(b) && (b.group && (b.group_match = !1, b.active_options = 0), null != b.group_array_index && this.results_data[b.group_array_index] && (f = this.results_data[b.group_array_index], 0 === f.active_options && f.search_match && (e += 1), f.active_options += 1), (!b.group || this.group_search) && (b.search_text = b.group ? b.label : b.html, b.search_match = this.search_string_match(b.search_text, c), b.search_match && !b.group && (e += 1), b.search_match ? (g.length && (h = b.search_text.search(j), i = b.search_text.substr(0, h + g.length) + "</em>" + b.search_text.substr(h + g.length), b.search_text = i.substr(0, h) + "<em>" + i.substr(h)), null != f && (f.group_match = !0)) : null != b.group_array_index && this.results_data[b.group_array_index].search_match && (b.search_match = !0)));
                return this.result_clear_highlight(), 1 > e && g.length ? (this.update_results_content(""), this.no_results(g)) : (this.update_results_content(this.results_option_build()), this.winnow_results_set_highlight())
            }, a.prototype.search_string_match = function (a, b) {
                var c, d, e, f;
                if (b.test(a)) return !0;
                if (this.enable_split_word_search && (a.indexOf(" ") >= 0 || 0 === a.indexOf("[")) && (d = a.replace(/\[|\]/g, "").split(" "), d.length))
                    for (e = 0, f = d.length; f > e; e++)
                        if (c = d[e], b.test(c)) return !0
            }, a.prototype.choices_count = function () {
                var a, b, c, d;
                if (null != this.selected_option_count) return this.selected_option_count;
                for (this.selected_option_count = 0, d = this.form_field.options, b = 0, c = d.length; c > b; b++) a = d[b], a.selected && (this.selected_option_count += 1);
                return this.selected_option_count
            }, a.prototype.choices_click = function (a) {
                return a.preventDefault(), this.results_showing || this.is_disabled ? void 0 : this.results_show()
            }, a.prototype.keyup_checker = function (a) {
                var b, c;
                switch (b = null != (c = a.which) ? c : a.keyCode, this.search_field_scale(), b) {
                    case 8:
                        if (this.is_multiple && this.backstroke_length < 1 && this.choices_count() > 0) return this.keydown_backstroke();
                        if (!this.pending_backstroke) return this.result_clear_highlight(), this.results_search();
                        break;
                    case 13:
                        if (a.preventDefault(), this.results_showing) return this.result_select(a);
                        break;
                    case 27:
                        return this.results_showing && this.results_hide(), !0;
                    case 9:
                    case 38:
                    case 40:
                    case 16:
                    case 91:
                    case 17:
                        break;
                    default:
                        return this.results_search()
                }
            }, a.prototype.clipboard_event_checker = function () {
                var a = this;
                return setTimeout(function () {
                    return a.results_search()
                }, 50)
            }, a.prototype.container_width = function () {
                return null != this.options.width ? this.options.width : "" + this.form_field.offsetWidth + "px"
            }, a.prototype.include_option_in_results = function (a) {
                return this.is_multiple && !this.display_selected_options && a.selected ? !1 : !this.display_disabled_options && a.disabled ? !1 : a.empty ? !1 : !0
            }, a.prototype.search_results_touchstart = function (a) {
                return this.touch_started = !0, this.search_results_mouseover(a)
            }, a.prototype.search_results_touchmove = function (a) {
                return this.touch_started = !1, this.search_results_mouseout(a)
            }, a.prototype.search_results_touchend = function (a) {
                return this.touch_started ? this.search_results_mouseup(a) : void 0
            }, a.prototype.outerHTML = function (a) {
                var b;
                return a.outerHTML ? a.outerHTML : (b = document.createElement("div"), b.appendChild(a), b.innerHTML)
            }, a.browser_is_supported = function () {
                return "Microsoft Internet Explorer" === window.navigator.appName ? document.documentMode >= 8 : /iP(od|hone)/i.test(window.navigator.userAgent) ? !1 : /Android/i.test(window.navigator.userAgent) && /Mobile/i.test(window.navigator.userAgent) ? !1 : !0
            }, a.default_multiple_text = "Select Some Options", a.default_single_text = "Select an Option", a.default_no_result_text = "No results match", a
        }(), a = jQuery, a.fn.extend({
            chosen: function (d) {
                return b.browser_is_supported() ? this.each(function () {
                    var b, e;
                    b = a(this), e = b.data("chosen"), "destroy" === d && e ? e.destroy() : e || b.data("chosen", new c(this, d))
                }) : this
            }
        }), c = function (b) {
            function c() {
                return e = c.__super__.constructor.apply(this, arguments)
            }
            return g(c, b), c.prototype.setup = function () {
                return this.form_field_jq = a(this.form_field), this.current_selectedIndex = this.form_field.selectedIndex, this.is_rtl = this.form_field_jq.hasClass("chosen-rtl")
            }, c.prototype.set_up_html = function () {
                var b, c;
                return b = ["chosen-container"], b.push("chosen-container-" + (this.is_multiple ? "multi" : "single")), this.inherit_select_classes && this.form_field.className && b.push(this.form_field.className), this.is_rtl && b.push("chosen-rtl"), c = {
                    "class": b.join(" "),
                    style: "width: " + this.container_width() + ";",
                    title: this.form_field.title
                }, this.form_field.id.length && (c.id = this.form_field.id.replace(/[^\w]/g, "_") + "_chosen"), this.container = a("<div />", c), this.container.html(this.is_multiple ? '<ul class="chosen-choices"><li class="search-field"><input type="text" value="' + this.default_text + '" class="default" autocomplete="off" style="width:25px;" /></li></ul><div class="chosen-drop"><ul class="chosen-results"></ul></div>' : '<a class="chosen-single chosen-default" tabindex="-1"><span>' + this.default_text + '</span><div><b></b></div></a><div class="chosen-drop"><div class="chosen-search"><input type="text" autocomplete="off" /></div><ul class="chosen-results"></ul></div>'), this.form_field_jq.hide().after(this.container), this.dropdown = this.container.find("div.chosen-drop").first(), this.search_field = this.container.find("input").first(), this.search_results = this.container.find("ul.chosen-results").first(), this.search_field_scale(), this.search_no_results = this.container.find("li.no-results").first(), this.is_multiple ? (this.search_choices = this.container.find("ul.chosen-choices").first(), this.search_container = this.container.find("li.search-field").first()) : (this.search_container = this.container.find("div.chosen-search").first(), this.selected_item = this.container.find(".chosen-single").first()), this.results_build(), this.set_tab_index(), this.set_label_behavior(), this.form_field_jq.trigger("chosen:ready", {
                    chosen: this
                })
            }, c.prototype.register_observers = function () {
                var a = this;
                return this.container.bind("mousedown.chosen", function (b) {
                    a.container_mousedown(b)
                }), this.container.bind("mouseup.chosen", function (b) {
                    a.container_mouseup(b)
                }), this.container.bind("mouseenter.chosen", function (b) {
                    a.mouse_enter(b)
                }), this.container.bind("mouseleave.chosen", function (b) {
                    a.mouse_leave(b)
                }), this.search_results.bind("mouseup.chosen", function (b) {
                    a.search_results_mouseup(b)
                }), this.search_results.bind("mouseover.chosen", function (b) {
                    a.search_results_mouseover(b)
                }), this.search_results.bind("mouseout.chosen", function (b) {
                    a.search_results_mouseout(b)
                }), this.search_results.bind("mousewheel.chosen DOMMouseScroll.chosen", function (b) {
                    a.search_results_mousewheel(b)
                }), this.search_results.bind("touchstart.chosen", function (b) {
                    a.search_results_touchstart(b)
                }), this.search_results.bind("touchmove.chosen", function (b) {
                    a.search_results_touchmove(b)
                }), this.search_results.bind("touchend.chosen", function (b) {
                    a.search_results_touchend(b)
                }), this.form_field_jq.bind("chosen:updated.chosen", function (b) {
                    a.results_update_field(b)
                }), this.form_field_jq.bind("chosen:activate.chosen", function (b) {
                    a.activate_field(b)
                }), this.form_field_jq.bind("chosen:open.chosen", function (b) {
                    a.container_mousedown(b)
                }), this.form_field_jq.bind("chosen:close.chosen", function (b) {
                    a.input_blur(b)
                }), this.search_field.bind("blur.chosen", function (b) {
                    a.input_blur(b)
                }), this.search_field.bind("keyup.chosen", function (b) {
                    a.keyup_checker(b)
                }), this.search_field.bind("keydown.chosen", function (b) {
                    a.keydown_checker(b)
                }), this.search_field.bind("focus.chosen", function (b) {
                    a.input_focus(b)
                }), this.search_field.bind("cut.chosen", function (b) {
                    a.clipboard_event_checker(b)
                }), this.search_field.bind("paste.chosen", function (b) {
                    a.clipboard_event_checker(b)
                }), this.is_multiple ? this.search_choices.bind("click.chosen", function (b) {
                    a.choices_click(b)
                }) : this.container.bind("click.chosen", function (a) {
                    a.preventDefault()
                })
            }, c.prototype.destroy = function () {
                return a(this.container[0].ownerDocument).unbind("click.chosen", this.click_test_action), this.search_field[0].tabIndex && (this.form_field_jq[0].tabIndex = this.search_field[0].tabIndex), this.container.remove(), this.form_field_jq.removeData("chosen"), this.form_field_jq.show()
            }, c.prototype.search_field_disabled = function () {
                return this.is_disabled = this.form_field_jq[0].disabled, this.is_disabled ? (this.container.addClass("chosen-disabled"), this.search_field[0].disabled = !0, this.is_multiple || this.selected_item.unbind("focus.chosen", this.activate_action), this.close_field()) : (this.container.removeClass("chosen-disabled"), this.search_field[0].disabled = !1, this.is_multiple ? void 0 : this.selected_item.bind("focus.chosen", this.activate_action))
            }, c.prototype.container_mousedown = function (b) {
                return this.is_disabled || (b && "mousedown" === b.type && !this.results_showing && b.preventDefault(), null != b && a(b.target).hasClass("search-choice-close")) ? void 0 : (this.active_field ? this.is_multiple || !b || a(b.target)[0] !== this.selected_item[0] && !a(b.target).parents("a.chosen-single").length || (b.preventDefault(), this.results_toggle()) : (this.is_multiple && this.search_field.val(""), a(this.container[0].ownerDocument).bind("click.chosen", this.click_test_action), this.results_show()), this.activate_field())
            }, c.prototype.container_mouseup = function (a) {
                return "ABBR" !== a.target.nodeName || this.is_disabled ? void 0 : this.results_reset(a)
            }, c.prototype.search_results_mousewheel = function (a) {
                var b;
                return a.originalEvent && (b = -a.originalEvent.wheelDelta || a.originalEvent.detail), null != b ? (a.preventDefault(), "DOMMouseScroll" === a.type && (b = 40 * b), this.search_results.scrollTop(b + this.search_results.scrollTop())) : void 0
            }, c.prototype.blur_test = function () {
                return !this.active_field && this.container.hasClass("chosen-container-active") ? this.close_field() : void 0
            }, c.prototype.close_field = function () {
                return a(this.container[0].ownerDocument).unbind("click.chosen", this.click_test_action), this.active_field = !1, this.results_hide(), this.container.removeClass("chosen-container-active"), this.clear_backstroke(), this.show_search_field_default(), this.search_field_scale()
            }, c.prototype.activate_field = function () {
                return this.container.addClass("chosen-container-active"), this.active_field = !0, this.search_field.val(this.search_field.val()), this.search_field.focus()
            }, c.prototype.test_active_click = function (b) {
                var c;
                return c = a(b.target).closest(".chosen-container"), c.length && this.container[0] === c[0] ? this.active_field = !0 : this.close_field()
            }, c.prototype.results_build = function () {
                return this.parsing = !0, this.selected_option_count = null, this.results_data = d.select_to_array(this.form_field), this.is_multiple ? this.search_choices.find("li.search-choice").remove() : this.is_multiple || (this.single_set_selected_text(), this.disable_search || this.form_field.options.length <= this.disable_search_threshold ? (this.search_field[0].readOnly = !0, this.container.addClass("chosen-container-single-nosearch")) : (this.search_field[0].readOnly = !1, this.container.removeClass("chosen-container-single-nosearch"))), this.update_results_content(this.results_option_build({
                    first: !0
                })), this.search_field_disabled(), this.show_search_field_default(), this.search_field_scale(), this.parsing = !1
            }, c.prototype.result_do_highlight = function (a) {
                var b, c, d, e, f;
                if (a.length) {
                    if (this.result_clear_highlight(), this.result_highlight = a, this.result_highlight.addClass("highlighted"), d = parseInt(this.search_results.css("maxHeight"), 10), f = this.search_results.scrollTop(), e = d + f, c = this.result_highlight.position().top + this.search_results.scrollTop(), b = c + this.result_highlight.outerHeight(), b >= e) return this.search_results.scrollTop(b - d > 0 ? b - d : 0);
                    if (f > c) return this.search_results.scrollTop(c)
                }
            }, c.prototype.result_clear_highlight = function () {
                return this.result_highlight && this.result_highlight.removeClass("highlighted"), this.result_highlight = null
            }, c.prototype.results_show = function () {
                return this.is_multiple && this.max_selected_options <= this.choices_count() ? (this.form_field_jq.trigger("chosen:maxselected", {
                    chosen: this
                }), !1) : (this.container.addClass("chosen-with-drop"), this.results_showing = !0, this.search_field.focus(), this.search_field.val(this.search_field.val()), this.winnow_results(), this.form_field_jq.trigger("chosen:showing_dropdown", {
                    chosen: this
                }))
            }, c.prototype.update_results_content = function (a) {
                return this.search_results.html(a)
            }, c.prototype.results_hide = function () {
                return this.results_showing && (this.result_clear_highlight(), this.container.removeClass("chosen-with-drop"), this.form_field_jq.trigger("chosen:hiding_dropdown", {
                    chosen: this
                })), this.results_showing = !1
            }, c.prototype.set_tab_index = function () {
                var a;
                return this.form_field.tabIndex ? (a = this.form_field.tabIndex, this.form_field.tabIndex = -1, this.search_field[0].tabIndex = a) : void 0
            }, c.prototype.set_label_behavior = function () {
                var b = this;
                return this.form_field_label = this.form_field_jq.parents("label"), !this.form_field_label.length && this.form_field.id.length && (this.form_field_label = a("label[for='" + this.form_field.id + "']")), this.form_field_label.length > 0 ? this.form_field_label.bind("click.chosen", function (a) {
                    return b.is_multiple ? b.container_mousedown(a) : b.activate_field()
                }) : void 0
            }, c.prototype.show_search_field_default = function () {
                return this.is_multiple && this.choices_count() < 1 && !this.active_field ? (this.search_field.val(this.default_text), this.search_field.addClass("default")) : (this.search_field.val(""), this.search_field.removeClass("default"))
            }, c.prototype.search_results_mouseup = function (b) {
                var c;
                return c = a(b.target).hasClass("active-result") ? a(b.target) : a(b.target).parents(".active-result").first(), c.length ? (this.result_highlight = c, this.result_select(b), this.search_field.focus()) : void 0
            }, c.prototype.search_results_mouseover = function (b) {
                var c;
                return c = a(b.target).hasClass("active-result") ? a(b.target) : a(b.target).parents(".active-result").first(), c ? this.result_do_highlight(c) : void 0
            }, c.prototype.search_results_mouseout = function (b) {
                return a(b.target).hasClass("active-result") ? this.result_clear_highlight() : void 0
            }, c.prototype.choice_build = function (b) {
                var c, d, e = this;
                return c = a("<li />", {
                    "class": "search-choice"
                }).html("<span>" + b.html + "</span>"), b.disabled ? c.addClass("search-choice-disabled") : (d = a("<a />", {
                    "class": "search-choice-close",
                    "data-option-array-index": b.array_index
                }), d.bind("click.chosen", function (a) {
                    return e.choice_destroy_link_click(a)
                }), c.append(d)), this.search_container.before(c)
            }, c.prototype.choice_destroy_link_click = function (b) {
                return b.preventDefault(), b.stopPropagation(), this.is_disabled ? void 0 : this.choice_destroy(a(b.target))
            }, c.prototype.choice_destroy = function (a) {
                return this.result_deselect(a[0].getAttribute("data-option-array-index")) ? (this.show_search_field_default(), this.is_multiple && this.choices_count() > 0 && this.search_field.val().length < 1 && this.results_hide(), a.parents("li").first().remove(), this.search_field_scale()) : void 0
            }, c.prototype.results_reset = function () {
                return this.reset_single_select_options(), this.form_field.options[0].selected = !0, this.single_set_selected_text(), this.show_search_field_default(), this.results_reset_cleanup(), this.form_field_jq.trigger("change"), this.active_field ? this.results_hide() : void 0
            }, c.prototype.results_reset_cleanup = function () {
                return this.current_selectedIndex = this.form_field.selectedIndex, this.selected_item.find("abbr").remove()
            }, c.prototype.result_select = function (a) {
                var b, c;
                return this.result_highlight ? (b = this.result_highlight, this.result_clear_highlight(), this.is_multiple && this.max_selected_options <= this.choices_count() ? (this.form_field_jq.trigger("chosen:maxselected", {
                    chosen: this
                }), !1) : (this.is_multiple ? b.removeClass("active-result") : this.reset_single_select_options(), c = this.results_data[b[0].getAttribute("data-option-array-index")], c.selected = !0, this.form_field.options[c.options_index].selected = !0, this.selected_option_count = null, this.is_multiple ? this.choice_build(c) : this.single_set_selected_text(c.text), (a.metaKey || a.ctrlKey) && this.is_multiple || this.results_hide(), this.search_field.val(""), (this.is_multiple || this.form_field.selectedIndex !== this.current_selectedIndex) && this.form_field_jq.trigger("change", {
                    selected: this.form_field.options[c.options_index].value
                }), this.current_selectedIndex = this.form_field.selectedIndex, this.search_field_scale())) : void 0
            }, c.prototype.single_set_selected_text = function (a) {
                return null == a && (a = this.default_text), a === this.default_text ? this.selected_item.addClass("chosen-default") : (this.single_deselect_control_build(), this.selected_item.removeClass("chosen-default")), this.selected_item.find("span").text(a)
            }, c.prototype.result_deselect = function (a) {
                var b;
                return b = this.results_data[a], this.form_field.options[b.options_index].disabled ? !1 : (b.selected = !1, this.form_field.options[b.options_index].selected = !1, this.selected_option_count = null, this.result_clear_highlight(), this.results_showing && this.winnow_results(), this.form_field_jq.trigger("change", {
                    deselected: this.form_field.options[b.options_index].value
                }), this.search_field_scale(), !0)
            }, c.prototype.single_deselect_control_build = function () {
                return this.allow_single_deselect ? (this.selected_item.find("abbr").length || this.selected_item.find("span").first().after('<abbr class="search-choice-close"></abbr>'), this.selected_item.addClass("chosen-single-with-deselect")) : void 0
            }, c.prototype.get_search_text = function () {
                return this.search_field.val() === this.default_text ? "" : a("<div/>").text(a.trim(this.search_field.val())).html()
            }, c.prototype.winnow_results_set_highlight = function () {
                var a, b;
                return b = this.is_multiple ? [] : this.search_results.find(".result-selected.active-result"), a = b.length ? b.first() : this.search_results.find(".active-result").first(), null != a ? this.result_do_highlight(a) : void 0
            }, c.prototype.no_results = function (b) {
                var c;
                return c = a('<li class="no-results">' + this.results_none_found + ' "<span></span>"</li>'), c.find("span").first().html(b), this.search_results.append(c), this.form_field_jq.trigger("chosen:no_results", {
                    chosen: this
                })
            }, c.prototype.no_results_clear = function () {
                return this.search_results.find(".no-results").remove()
            }, c.prototype.keydown_arrow = function () {
                var a;
                return this.results_showing && this.result_highlight ? (a = this.result_highlight.nextAll("li.active-result").first()) ? this.result_do_highlight(a) : void 0 : this.results_show()
            }, c.prototype.keyup_arrow = function () {
                var a;
                return this.results_showing || this.is_multiple ? this.result_highlight ? (a = this.result_highlight.prevAll("li.active-result"), a.length ? this.result_do_highlight(a.first()) : (this.choices_count() > 0 && this.results_hide(), this.result_clear_highlight())) : void 0 : this.results_show()
            }, c.prototype.keydown_backstroke = function () {
                var a;
                return this.pending_backstroke ? (this.choice_destroy(this.pending_backstroke.find("a").first()), this.clear_backstroke()) : (a = this.search_container.siblings("li.search-choice").last(), a.length && !a.hasClass("search-choice-disabled") ? (this.pending_backstroke = a, this.single_backstroke_delete ? this.keydown_backstroke() : this.pending_backstroke.addClass("search-choice-focus")) : void 0)
            }, c.prototype.clear_backstroke = function () {
                return this.pending_backstroke && this.pending_backstroke.removeClass("search-choice-focus"), this.pending_backstroke = null
            }, c.prototype.keydown_checker = function (a) {
                var b, c;
                switch (b = null != (c = a.which) ? c : a.keyCode, this.search_field_scale(), 8 !== b && this.pending_backstroke && this.clear_backstroke(), b) {
                    case 8:
                        this.backstroke_length = this.search_field.val().length;
                        break;
                    case 9:
                        this.results_showing && !this.is_multiple && this.result_select(a), this.mouse_on_container = !1;
                        break;
                    case 13:
                        a.preventDefault();
                        break;
                    case 38:
                        a.preventDefault(), this.keyup_arrow();
                        break;
                    case 40:
                        a.preventDefault(), this.keydown_arrow()
                }
            }, c.prototype.search_field_scale = function () {
                var b, c, d, e, f, g, h, i, j;
                if (this.is_multiple) {
                    for (d = 0, h = 0, f = "position:absolute; left: -1000px; top: -1000px; display:none;", g = ["font-size", "font-style", "font-weight", "font-family", "line-height", "text-transform", "letter-spacing"], i = 0, j = g.length; j > i; i++) e = g[i], f += e + ":" + this.search_field.css(e) + ";";
                    return b = a("<div />", {
                        style: f
                    }), b.text(this.search_field.val()), a("body").append(b), h = b.width() + 25, b.remove(), c = this.container.outerWidth(), h > c - 10 && (h = c - 10), this.search_field.css({
                        width: h + "px"
                    })
                }
            }, c
        }(b)
    }.call(this),
    function (a) {
        "use strict";
        var b = function () {
            var b = {
                bcClass: "sf-breadcrumb",
                menuClass: "sf-js-enabled",
                anchorClass: "sf-with-ul",
                menuArrowClass: "sf-arrows"
            },
                c = function () {
                    var b = /iPhone|iPad|iPod/i.test(navigator.userAgent);
                    return b && a(window).load(function () {
                        a("body").children().on("click", a.noop)
                    }), b
                }(),
                d = function () {
                    var a = document.documentElement.style;
                    return "behavior" in a && "fill" in a && /iemobile/i.test(navigator.userAgent)
                }(),
                e = function (a, c) {
                    var d = b.menuClass;
                    c.cssArrows && (d += " " + b.menuArrowClass), a.toggleClass(d)
                },
                f = function (c, d) {
                    return c.find("li." + d.pathClass).slice(0, d.pathLevels).addClass(d.hoverClass + " " + b.bcClass).filter(function () {
                        return a(this).children(d.popUpSelector).hide().show().length
                    }).removeClass(d.pathClass)
                },
                g = function (a) {
                    a.children("a").toggleClass(b.anchorClass)
                },
                h = function (a) {
                    var b = a.css("ms-touch-action");
                    b = "pan-y" === b ? "auto" : "pan-y", a.css("ms-touch-action", b)
                },
                i = function (b, e) {
                    var f = "li:has(" + e.popUpSelector + ")";
                    a.fn.hoverIntent && !e.disableHI ? b.hoverIntent(k, l, f) : b.on("mouseenter.superfish", f, k).on("mouseleave.superfish", f, l);
                    var g = "MSPointerDown.superfish";
                    c || (g += " touchend.superfish"), d && (g += " mousedown.superfish"), b.on("focusin.superfish", "li", k).on("focusout.superfish", "li", l).on(g, "a", e, j)
                },
                j = function (b) {
                    var c = a(this),
                        d = c.siblings(b.data.popUpSelector);
                    d.length > 0 && d.is(":hidden") && (c.one("click.superfish", !1), "MSPointerDown" === b.type ? c.trigger("focus") : a.proxy(k, c.parent("li"))())
                },
                k = function () {
                    var b = a(this),
                        c = o(b);
                    clearTimeout(c.sfTimer), b.siblings().superfish("hide").end().superfish("show")
                },
                l = function () {
                    var b = a(this),
                        d = o(b);
                    c ? a.proxy(m, b, d)() : (clearTimeout(d.sfTimer), d.sfTimer = setTimeout(a.proxy(m, b, d), d.delay))
                },
                m = function (b) {
                    b.retainPath = a.inArray(this[0], b.$path) > -1, this.superfish("hide"), this.parents("." + b.hoverClass).length || (b.onIdle.call(n(this)), b.$path.length && a.proxy(k, b.$path)())
                },
                n = function (a) {
                    return a.closest("." + b.menuClass)
                },
                o = function (a) {
                    return n(a).data("sf-options")
                };
            return {
                hide: function (b) {
                    if (this.length) {
                        var c = this,
                            d = o(c);
                        if (!d) return this;
                        var e = d.retainPath === !0 ? d.$path : "",
                            f = c.find("li." + d.hoverClass).add(this).not(e).removeClass(d.hoverClass).children(d.popUpSelector),
                            g = d.speedOut;
                        b && (f.show(), g = 0), d.retainPath = !1, d.onBeforeHide.call(f), f.stop(!0, !0).animate(d.animationOut, g, function () {
                            var b = a(this);
                            d.onHide.call(b)
                        })
                    }
                    return this
                },
                show: function () {
                    var a = o(this);
                    if (!a) return this;
                    var b = this.addClass(a.hoverClass),
                        c = b.children(a.popUpSelector);
                    return a.onBeforeShow.call(c), c.stop(!0, !0).animate(a.animation, a.speed, function () {
                        a.onShow.call(c)
                    }), this
                },
                destroy: function () {
                    return this.each(function () {
                        var c, d = a(this),
                            f = d.data("sf-options");
                        return f ? (c = d.find(f.popUpSelector).parent("li"), clearTimeout(f.sfTimer), e(d, f), g(c), h(d), d.off(".superfish").off(".hoverIntent"), c.children(f.popUpSelector).attr("style", function (a, b) {
                            return b.replace(/display[^;]+;?/g, "")
                        }), f.$path.removeClass(f.hoverClass + " " + b.bcClass).addClass(f.pathClass), d.find("." + f.hoverClass).removeClass(f.hoverClass), f.onDestroy.call(d), void d.removeData("sf-options")) : !1
                    })
                },
                init: function (c) {
                    return this.each(function () {
                        var d = a(this);
                        if (d.data("sf-options")) return !1;
                        var j = a.extend({}, a.fn.superfish.defaults, c),
                            k = d.find(j.popUpSelector).parent("li");
                        j.$path = f(d, j), d.data("sf-options", j), e(d, j), g(k), h(d), i(d, j), k.not("." + b.bcClass).superfish("hide", !0), j.onInit.call(this)
                    })
                }
            }
        }();
        a.fn.superfish = function (c) {
            return b[c] ? b[c].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof c && c ? a.error("Method " + c + " does not exist on jQuery.fn.superfish") : b.init.apply(this, arguments)
        }, a.fn.superfish.defaults = {
            popUpSelector: "ul,.sf-mega",
            hoverClass: "sfHover",
            pathClass: "overrideThisToUse",
            pathLevels: 1,
            delay: 800,
            animation: {
                opacity: "show"
            },
            animationOut: {
                opacity: "hide"
            },
            speed: "normal",
            speedOut: "fast",
            cssArrows: !0,
            disableHI: !1,
            onInit: a.noop,
            onBeforeShow: a.noop,
            onShow: a.noop,
            onBeforeHide: a.noop,
            onHide: a.noop,
            onIdle: a.noop,
            onDestroy: a.noop
        }, a.fn.extend({
            hideSuperfishUl: b.hide,
            showSuperfishUl: b.show
        })
    }(jQuery),
    function (a) {
        var b = function () {
            var b = {
                bcClass: "sf-breadcrumb",
                menuClass: "sf-js-enabled",
                anchorClass: "sf-with-ul",
                menuArrowClass: "sf-arrows"
            },
                c = (function () {
                    a(window).load(function () {
                        a("body").children().on("click.superclick", function () {
                            var b = a(".sf-js-enabled");
                            b.superclick("reset")
                        })
                    })
                }(), function (a, c) {
                    var d = b.menuClass;
                    c.cssArrows && (d += " " + b.menuArrowClass), a.toggleClass(d)
                }),
                d = function (c, d) {
                    return c.find("li." + d.pathClass).slice(0, d.pathLevels).addClass(d.activeClass + " " + b.bcClass).filter(function () {
                        return a(this).children("ul").hide().show().length
                    }).removeClass(d.pathClass)
                },
                e = function (a) {
                    a.children("a").toggleClass(b.anchorClass)
                },
                f = function (a) {
                    var b = a.css("ms-touch-action");
                    b = "pan-y" === b ? "auto" : "pan-y", a.css("ms-touch-action", b)
                },
                g = function () {
                    var b, c = a(this),
                        d = c.siblings("ul");
                    return d.length ? (b = d.is(":hidden") ? h : i, a.proxy(b, c.parent("li"))(), !1) : void 0
                },
                h = function () {
                    {
                        var b = a(this);
                        l(b)
                    }
                    b.siblings().superclick("hide").end().superclick("show")
                },
                i = function () {
                    var b = a(this),
                        c = l(b);
                    a.proxy(j, b, c)()
                },
                j = function (b) {
                    b.retainPath = a.inArray(this[0], b.$path) > -1, this.superclick("hide"), this.parents("." + b.activeClass).length || (b.onIdle.call(k(this)), b.$path.length && a.proxy(h, b.$path)())
                },
                k = function (a) {
                    return a.closest("." + b.menuClass)
                },
                l = function (a) {
                    return k(a).data("sf-options")
                };
            return {
                hide: function (b) {
                    if (this.length) {
                        var c = this,
                            d = l(c);
                        if (!d) return this;
                        var e = d.retainPath === !0 ? d.$path : "",
                            f = c.find("li." + d.activeClass).add(this).not(e).removeClass(d.activeClass).children("ul"),
                            g = d.speedOut;
                        b && (f.show(), g = 0), d.retainPath = !1, d.onBeforeHide.call(f), f.stop(!0, !0).animate(d.animationOut, g, function () {
                            var b = a(this);
                            d.onHide.call(b)
                        })
                    }
                    return this
                },
                show: function () {
                    var a = l(this);
                    if (!a) return this;
                    var b = this.addClass(a.activeClass),
                        c = b.children("ul");
                    return a.onBeforeShow.call(c), c.stop(!0, !0).animate(a.animation, a.speed, function () {
                        a.onShow.call(c)
                    }), this
                },
                destroy: function () {
                    return this.each(function () {
                        var d = a(this),
                            g = d.data("sf-options"),
                            h = d.find("li:has(ul)");
                        return g ? (c(d, g), e(h), f(d), d.off(".superclick"), h.children("ul").attr("style", function (a, b) {
                            return b.replace(/display[^;]+;?/g, "")
                        }), g.$path.removeClass(g.activeClass + " " + b.bcClass).addClass(g.pathClass), d.find("." + g.activeClass).removeClass(g.activeClass), g.onDestroy.call(d), void d.removeData("sf-options")) : !1
                    })
                },
                reset: function () {
                    return this.each(function () {
                        var b = a(this),
                            c = l(b),
                            d = a(b.find("." + c.activeClass).toArray().reverse());
                        d.children("a").trigger("click")
                    })
                },
                init: function (h) {
                    return this.each(function () {
                        var i = a(this);
                        if (i.data("sf-options")) return !1;
                        var j = a.extend({}, a.fn.superclick.defaults, h),
                            k = i.find("li:has(ul)");
                        j.$path = d(i, j), i.data("sf-options", j), c(i, j), e(k), f(i), i.on("click.superclick", "a", g), k.not("." + b.bcClass).superclick("hide", !0), j.onInit.call(this)
                    })
                }
            }
        }();
        a.fn.superclick = function (c) {
            return b[c] ? b[c].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof c && c ? a.error("Method " + c + " does not exist on jQuery.fn.superclick") : b.init.apply(this, arguments)
        }, a.fn.superclick.defaults = {
            activeClass: "sfHover",
            pathClass: "overrideThisToUse",
            pathLevels: 1,
            animation: {
                opacity: "show"
            },
            animationOut: {
                opacity: "hide"
            },
            speed: "normal",
            speedOut: "fast",
            cssArrows: !0,
            onInit: a.noop,
            onBeforeShow: a.noop,
            onShow: a.noop,
            onBeforeHide: a.noop,
            onHide: a.noop,
            onIdle: a.noop,
            onDestroy: a.noop
        }
    }(jQuery),
    function (b) {
        function c() {
            var a = document.getElementsByTagName("script"),
                b = a[a.length - 1].src.split("?")[0];
            return b.split("/").length > 0 ? b.split("/").slice(0, -1).join("/") + "/" : ""
        }

        function d(a, b, c) {
            for (var d = 0; d < b.length; d++) c(a, b[d])
        }
        var e = !1,
            f = !1,
            g = 5e3,
            h = 2e3,
            i = 0,
            j = b,
            k = c(),
            l = ["ms", "moz", "webkit", "o"],
            m = window.requestAnimationFrame || !1,
            n = window.cancelAnimationFrame || !1;
        if (!m)
            for (var o in l) {
                var p = l[o];
                m || (m = window[p + "RequestAnimationFrame"]), n || (n = window[p + "CancelAnimationFrame"] || window[p + "CancelRequestAnimationFrame"])
            }
        var q = window.MutationObserver || window.WebKitMutationObserver || !1,
            r = {
                zindex: "auto",
                cursoropacitymin: 0,
                cursoropacitymax: 1,
                cursorcolor: "#424242",
                cursorwidth: "5px",
                cursorborder: "1px solid #fff",
                cursorborderradius: "5px",
                scrollspeed: 60,
                mousescrollstep: 24,
                touchbehavior: !1,
                hwacceleration: !0,
                usetransition: !0,
                boxzoom: !1,
                dblclickzoom: !0,
                gesturezoom: !0,
                grabcursorenabled: !0,
                autohidemode: !0,
                background: "",
                iframeautoresize: !0,
                cursorminheight: 32,
                preservenativescrolling: !0,
                railoffset: !1,
                bouncescroll: !0,
                spacebarenabled: !0,
                railpadding: {
                    top: 0,
                    right: 0,
                    left: 0,
                    bottom: 0
                },
                disableoutline: !0,
                horizrailenabled: !0,
                railalign: "right",
                railvalign: "bottom",
                enabletranslate3d: !0,
                enablemousewheel: !0,
                enablekeyboard: !0,
                smoothscroll: !0,
                sensitiverail: !0,
                enablemouselockapi: !0,
                cursorfixedheight: !1,
                directionlockdeadzone: 6,
                hidecursordelay: 400,
                nativeparentscrolling: !0,
                enablescrollonselection: !0,
                overflowx: !0,
                overflowy: !0,
                cursordragspeed: .3,
                rtlmode: !1,
                cursordragontouch: !1,
                oneaxismousemode: "auto"
            },
            s = !1,
            t = function () {
                function a() {
                    var a = ["-moz-grab", "-webkit-grab", "grab"];
                    (c.ischrome && !c.ischrome22 || c.isie) && (a = []);
                    for (var d = 0; d < a.length; d++) {
                        var e = a[d];
                        if (b.style.cursor = e, b.style.cursor == e) return e
                    }
                    return "url(http://www.google.com/intl/en_ALL/mapfiles/openhand.cur),n-resize"
                }
                if (s) return s;
                var b = document.createElement("DIV"),
                    c = {};
                c.haspointerlock = "pointerLockElement" in document || "mozPointerLockElement" in document || "webkitPointerLockElement" in document, c.isopera = "opera" in window, c.isopera12 = c.isopera && "getUserMedia" in navigator, c.isoperamini = "[object OperaMini]" === Object.prototype.toString.call(window.operamini), c.isie = "all" in document && "attachEvent" in b && !c.isopera, c.isieold = c.isie && !("msInterpolationMode" in b.style), c.isie7 = !(!c.isie || c.isieold || "documentMode" in document && 7 != document.documentMode), c.isie8 = c.isie && "documentMode" in document && 8 == document.documentMode, c.isie9 = c.isie && "performance" in window && document.documentMode >= 9, c.isie10 = c.isie && "performance" in window && document.documentMode >= 10, c.isie9mobile = /iemobile.9/i.test(navigator.userAgent), c.isie9mobile && (c.isie9 = !1), c.isie7mobile = !c.isie9mobile && c.isie7 && /iemobile/i.test(navigator.userAgent), c.ismozilla = "MozAppearance" in b.style, c.iswebkit = "WebkitAppearance" in b.style, c.ischrome = "chrome" in window, c.ischrome22 = c.ischrome && c.haspointerlock, c.ischrome26 = c.ischrome && "transition" in b.style, c.cantouch = "ontouchstart" in document.documentElement || "ontouchstart" in window, c.hasmstouch = window.navigator.msPointerEnabled || !1, c.ismac = /^mac$/i.test(navigator.platform), c.isios = c.cantouch && /iphone|ipad|ipod/i.test(navigator.platform), c.isios4 = c.isios && !("seal" in Object), c.isandroid = /android/i.test(navigator.userAgent), c.trstyle = !1, c.hastransform = !1, c.hastranslate3d = !1, c.transitionstyle = !1, c.hastransition = !1, c.transitionend = !1;
                for (var d = ["transform", "msTransform", "webkitTransform", "MozTransform", "OTransform"], e = 0; e < d.length; e++)
                    if ("undefined" != typeof b.style[d[e]]) {
                        c.trstyle = d[e];
                        break
                    }
                c.hastransform = 0 != c.trstyle, c.hastransform && (b.style[c.trstyle] = "translate3d(1px,2px,3px)", c.hastranslate3d = /translate3d/.test(b.style[c.trstyle])), c.transitionstyle = !1, c.prefixstyle = "", c.transitionend = !1;
                for (var d = ["transition", "webkitTransition", "MozTransition", "OTransition", "OTransition", "msTransition", "KhtmlTransition"], f = ["", "-webkit-", "-moz-", "-o-", "-o", "-ms-", "-khtml-"], g = ["transitionend", "webkitTransitionEnd", "transitionend", "otransitionend", "oTransitionEnd", "msTransitionEnd", "KhtmlTransitionEnd"], e = 0; e < d.length; e++)
                    if (d[e] in b.style) {
                        c.transitionstyle = d[e], c.prefixstyle = f[e], c.transitionend = g[e];
                        break
                    }
                return c.ischrome26 && (c.prefixstyle = f[1]), c.hastransition = c.transitionstyle, c.cursorgrabvalue = a(), c.hasmousecapture = "setCapture" in b, c.hasMutationObserver = q !== !1, b = null, s = c, c
            },
            u = function (a, b) {
                function c() {
                    var a = s.doc.css(w.trstyle);
                    return a && "matrix" == a.substr(0, 6) ? a.replace(/^.*\((.*)\)$/g, "$1").replace(/px/g, "").split(/, +/) : !1
                }

                function d() {
                    var a = s.win;
                    if ("zIndex" in a) return a.zIndex();
                    for (; a.length > 0;) {
                        if (9 == a[0].nodeType) return !1;
                        var b = a.css("zIndex");
                        if (!isNaN(b) && 0 != b) return parseInt(b);
                        a = a.parent()
                    }
                    return !1
                }

                function l(a, b, c) {
                    var d = a.css(b),
                        e = parseFloat(d);
                    if (isNaN(e)) {
                        e = x[d] || 0;
                        var f = 3 == e ? c ? s.win.outerHeight() - s.win.innerHeight() : s.win.outerWidth() - s.win.innerWidth() : 1;
                        return s.isie8 && e && (e += 1), f ? e : 0
                    }
                    return e
                }

                function o(a, b, c, d) {
                    s._bind(a, b, function (d) {
                        var d = d ? d : window.event,
                            e = {
                                original: d,
                                target: d.target || d.srcElement,
                                type: "wheel",
                                deltaMode: "MozMousePixelScroll" == d.type ? 0 : 1,
                                deltaX: 0,
                                deltaZ: 0,
                                preventDefault: function () {
                                    return d.preventDefault ? d.preventDefault() : d.returnValue = !1, !1
                                },
                                stopImmediatePropagation: function () {
                                    d.stopImmediatePropagation ? d.stopImmediatePropagation() : d.cancelBubble = !0
                                }
                            };
                        return "mousewheel" == b ? (e.deltaY = -1 / 40 * d.wheelDelta, d.wheelDeltaX && (e.deltaX = -1 / 40 * d.wheelDeltaX)) : e.deltaY = d.detail, c.call(a, e)
                    }, d)
                }

                function p(a, b, c) {
                    var d, e;
                    if (0 == a.deltaMode ? (d = -Math.floor(a.deltaX * (s.opt.mousescrollstep / 54)), e = -Math.floor(a.deltaY * (s.opt.mousescrollstep / 54))) : 1 == a.deltaMode && (d = -Math.floor(a.deltaX * s.opt.mousescrollstep), e = -Math.floor(a.deltaY * s.opt.mousescrollstep)), b && s.opt.oneaxismousemode && 0 == d && e && (d = e, e = 0), d && (s.scrollmom && s.scrollmom.stop(), s.lastdeltax += d, s.debounced("mousewheelx", function () {
                        var a = s.lastdeltax;
                        s.lastdeltax = 0, s.rail.drag || s.doScrollLeftBy(a)
                    }, 120)), e) {
                        if (s.opt.nativeparentscrolling && c && !s.ispage && !s.zoomactive)
                            if (0 > e) {
                                if (s.getScrollTop() >= s.page.maxh) return !0
                            } else if (s.getScrollTop() <= 0) return !0;
                        s.scrollmom && s.scrollmom.stop(), s.lastdeltay += e, s.debounced("mousewheely", function () {
                            var a = s.lastdeltay;
                            s.lastdeltay = 0, s.rail.drag || s.doScrollBy(a)
                        }, 120)
                    }
                    return a.stopImmediatePropagation(), a.preventDefault()
                }
                var s = this;
                if (this.version = "3.5.1", this.name = "nicescroll", this.me = b, this.opt = {
                    doc: j("body"),
                    win: !1
                }, j.extend(this.opt, r), this.opt.snapbackspeed = 80, a)
                    for (var u in s.opt) "undefined" != typeof a[u] && (s.opt[u] = a[u]);
                this.doc = s.opt.doc, this.iddoc = this.doc && this.doc[0] ? this.doc[0].id || "" : "", this.ispage = /BODY|HTML/.test(s.opt.win ? s.opt.win[0].nodeName : this.doc[0].nodeName), this.haswrapper = s.opt.win !== !1, this.win = s.opt.win || (this.ispage ? j(window) : this.doc), this.docscroll = this.ispage && !this.haswrapper ? j(window) : this.win, this.body = j("body"), this.viewport = !1, this.isfixed = !1, this.iframe = !1, this.isiframe = "IFRAME" == this.doc[0].nodeName && "IFRAME" == this.win[0].nodeName, this.istextarea = "TEXTAREA" == this.win[0].nodeName, this.forcescreen = !1, this.canshowonmouseevent = "scroll" != s.opt.autohidemode, this.onmousedown = !1, this.onmouseup = !1, this.onmousemove = !1, this.onmousewheel = !1, this.onkeypress = !1, this.ongesturezoom = !1, this.onclick = !1, this.onscrollstart = !1, this.onscrollend = !1, this.onscrollcancel = !1, this.onzoomin = !1, this.onzoomout = !1, this.view = !1, this.page = !1, this.scroll = {
                    x: 0,
                    y: 0
                }, this.scrollratio = {
                    x: 0,
                    y: 0
                }, this.cursorheight = 20, this.scrollvaluemax = 0, this.checkrtlmode = !1, this.scrollrunning = !1, this.scrollmom = !1, this.observer = !1, this.observerremover = !1;
                do this.id = "ascrail" + h++; while (document.getElementById(this.id));
                this.rail = !1, this.cursor = !1, this.cursorfreezed = !1, this.selectiondrag = !1, this.zoom = !1, this.zoomactive = !1, this.hasfocus = !1, this.hasmousefocus = !1, this.visibility = !0, this.locked = !1, this.hidden = !1, this.cursoractive = !0, this.overflowx = s.opt.overflowx, this.overflowy = s.opt.overflowy, this.nativescrollingarea = !1, this.checkarea = 0, this.events = [], this.saved = {}, this.delaylist = {}, this.synclist = {}, this.lastdeltax = 0, this.lastdeltay = 0, this.detected = t();
                var w = j.extend({}, this.detected);
                this.canhwscroll = w.hastransform && s.opt.hwacceleration, this.ishwscroll = this.canhwscroll && s.haswrapper, this.istouchcapable = !1, w.cantouch && w.ischrome && !w.isios && !w.isandroid && (this.istouchcapable = !0, w.cantouch = !1), w.cantouch && w.ismozilla && !w.isios && !w.isandroid && (this.istouchcapable = !0, w.cantouch = !1), s.opt.enablemouselockapi || (w.hasmousecapture = !1, w.haspointerlock = !1), this.delayed = function (a, b, c, d) {
                    var e = s.delaylist[a],
                        f = (new Date).getTime();
                    return !d && e && e.tt ? !1 : (e && e.tt && clearTimeout(e.tt), void (e && e.last + c > f && !e.tt ? s.delaylist[a] = {
                        last: f + c,
                        tt: setTimeout(function () {
                            s.delaylist[a].tt = 0, b.call()
                        }, c)
                    } : e && e.tt || (s.delaylist[a] = {
                        last: f,
                        tt: 0
                    }, setTimeout(function () {
                        b.call()
                    }, 0))))
                }, this.debounced = function (a, b, c) {
                    {
                        var d = s.delaylist[a];
                        (new Date).getTime()
                    }
                    s.delaylist[a] = b, d || setTimeout(function () {
                        var b = s.delaylist[a];
                        s.delaylist[a] = !1, b.call()
                    }, c)
                }, this.synched = function (a, b) {
                    function c() {
                        s.onsync || (m(function () {
                            s.onsync = !1;
                            for (a in s.synclist) {
                                var b = s.synclist[a];
                                b && b.call(s), s.synclist[a] = !1
                            }
                        }), s.onsync = !0)
                    }
                    return s.synclist[a] = b, c(), a
                }, this.unsynched = function (a) {
                    s.synclist[a] && (s.synclist[a] = !1)
                }, this.css = function (a, b) {
                    for (var c in b) s.saved.css.push([a, c, a.css(c)]), a.css(c, b[c])
                }, this.scrollTop = function (a) {
                    return "undefined" == typeof a ? s.getScrollTop() : s.setScrollTop(a)
                }, this.scrollLeft = function (a) {
                    return "undefined" == typeof a ? s.getScrollLeft() : s.setScrollLeft(a)
                }, BezierClass = function (a, b, c, d, e, f, g) {
                    this.st = a, this.ed = b, this.spd = c, this.p1 = d || 0, this.p2 = e || 1, this.p3 = f || 0, this.p4 = g || 1, this.ts = (new Date).getTime(), this.df = this.ed - this.st
                }, BezierClass.prototype = {
                    B2: function (a) {
                        return 3 * a * a * (1 - a)
                    },
                    B3: function (a) {
                        return 3 * a * (1 - a) * (1 - a)
                    },
                    B4: function (a) {
                        return (1 - a) * (1 - a) * (1 - a)
                    },
                    getNow: function () {
                        var a = (new Date).getTime(),
                            b = 1 - (a - this.ts) / this.spd,
                            c = this.B2(b) + this.B3(b) + this.B4(b);
                        return 0 > b ? this.ed : this.st + Math.round(this.df * c)
                    },
                    update: function (a, b) {
                        return this.st = this.getNow(), this.ed = a, this.spd = b, this.ts = (new Date).getTime(), this.df = this.ed - this.st, this
                    }
                }, this.ishwscroll ? (this.doc.translate = {
                    x: 0,
                    y: 0,
                    tx: "0px",
                    ty: "0px"
                }, w.hastranslate3d && w.isios && this.doc.css("-webkit-backface-visibility", "hidden"), this.getScrollTop = function (a) {
                    if (!a) {
                        var b = c();
                        if (b) return 16 == b.length ? -b[13] : -b[5];
                        if (s.timerscroll && s.timerscroll.bz) return s.timerscroll.bz.getNow()
                    }
                    return s.doc.translate.y
                }, this.getScrollLeft = function (a) {
                    if (!a) {
                        var b = c();
                        if (b) return 16 == b.length ? -b[12] : -b[4];
                        if (s.timerscroll && s.timerscroll.bh) return s.timerscroll.bh.getNow()
                    }
                    return s.doc.translate.x
                }, this.notifyScrollEvent = document.createEvent ? function (a) {
                    var b = document.createEvent("UIEvents");
                    b.initUIEvent("scroll", !1, !0, window, 1), a.dispatchEvent(b)
                } : document.fireEvent ? function (a) {
                    var b = document.createEventObject();
                    a.fireEvent("onscroll"), b.cancelBubble = !0
                } : function () { }, w.hastranslate3d && s.opt.enabletranslate3d ? (this.setScrollTop = function (a, b) {
                    s.doc.translate.y = a, s.doc.translate.ty = -1 * a + "px", s.doc.css(w.trstyle, "translate3d(" + s.doc.translate.tx + "," + s.doc.translate.ty + ",0px)"), b || s.notifyScrollEvent(s.win[0])
                }, this.setScrollLeft = function (a, b) {
                    s.doc.translate.x = a, s.doc.translate.tx = -1 * a + "px", s.doc.css(w.trstyle, "translate3d(" + s.doc.translate.tx + "," + s.doc.translate.ty + ",0px)"), b || s.notifyScrollEvent(s.win[0])
                }) : (this.setScrollTop = function (a, b) {
                    s.doc.translate.y = a, s.doc.translate.ty = -1 * a + "px", s.doc.css(w.trstyle, "translate(" + s.doc.translate.tx + "," + s.doc.translate.ty + ")"), b || s.notifyScrollEvent(s.win[0])
                }, this.setScrollLeft = function (a, b) {
                    s.doc.translate.x = a, s.doc.translate.tx = -1 * a + "px", s.doc.css(w.trstyle, "translate(" + s.doc.translate.tx + "," + s.doc.translate.ty + ")"), b || s.notifyScrollEvent(s.win[0])
                })) : (this.getScrollTop = function () {
                    return s.docscroll.scrollTop()
                }, this.setScrollTop = function (a) {
                    return s.docscroll.scrollTop(a)
                }, this.getScrollLeft = function () {
                    return s.docscroll.scrollLeft()
                }, this.setScrollLeft = function (a) {
                    return s.docscroll.scrollLeft(a)
                }), this.getTarget = function (a) {
                    return a ? a.target ? a.target : a.srcElement ? a.srcElement : !1 : !1
                }, this.hasParent = function (a, b) {
                    if (!a) return !1;
                    for (var c = a.target || a.srcElement || a || !1; c && c.id != b;) c = c.parentNode || !1;
                    return c !== !1
                };
                var x = {
                    thin: 1,
                    medium: 3,
                    thick: 5
                };
                this.getOffset = function () {
                    if (s.isfixed) return {
                        top: parseFloat(s.win.css("top")),
                        left: parseFloat(s.win.css("left"))
                    };
                    if (!s.viewport) return s.win.offset();
                    var a = s.win.offset(),
                        b = s.viewport.offset();
                    return {
                        top: a.top - b.top + s.viewport.scrollTop(),
                        left: a.left - b.left + s.viewport.scrollLeft()
                    }
                }, this.updateScrollBar = function (a) {
                    if (s.ishwscroll) s.rail.css({
                        height: s.win.innerHeight()
                    }), s.railh && s.railh.css({
                        width: s.win.innerWidth()
                    });
                    else {
                        var b = s.getOffset(),
                            c = {
                                top: b.top,
                                left: b.left
                            };
                        c.top += l(s.win, "border-top-width", !0); {
                            (s.win.outerWidth() - s.win.innerWidth()) / 2
                        }
                        c.left += s.rail.align ? s.win.outerWidth() - l(s.win, "border-right-width") - s.rail.width : l(s.win, "border-left-width");
                        var d = s.opt.railoffset;
                        if (d && (d.top && (c.top += d.top), s.rail.align && d.left && (c.left += d.left)), s.locked || s.rail.css({
                            top: c.top,
                            left: c.left,
                            height: a ? a.h : s.win.innerHeight()
                        }), s.zoom && s.zoom.css({
                            top: c.top + 1,
                            left: 1 == s.rail.align ? c.left - 20 : c.left + s.rail.width + 4
                        }), s.railh && !s.locked) {
                            var c = {
                                top: b.top,
                                left: b.left
                            },
                                e = s.railh.align ? c.top + l(s.win, "border-top-width", !0) + s.win.innerHeight() - s.railh.height : c.top + l(s.win, "border-top-width", !0),
                                f = c.left + l(s.win, "border-left-width");
                            s.railh.css({
                                top: e,
                                left: f,
                                width: s.railh.width
                            })
                        }
                    }
                }, this.doRailClick = function (a, b, c) {
                    var d, e, f, g;
                    s.locked || (s.cancelEvent(a), b ? (d = c ? s.doScrollLeft : s.doScrollTop, f = c ? (a.pageX - s.railh.offset().left - s.cursorwidth / 2) * s.scrollratio.x : (a.pageY - s.rail.offset().top - s.cursorheight / 2) * s.scrollratio.y, d(f)) : (d = c ? s.doScrollLeftBy : s.doScrollBy, f = c ? s.scroll.x : s.scroll.y, g = c ? a.pageX - s.railh.offset().left : a.pageY - s.rail.offset().top, e = c ? s.view.w : s.view.h, d(f >= g ? e : -e)))
                }, s.hasanimationframe = m, s.hascancelanimationframe = n, s.hasanimationframe ? s.hascancelanimationframe || (n = function () {
                    s.cancelAnimationFrame = !0
                }) : (m = function (a) {
                    return setTimeout(a, 15 - Math.floor(+new Date / 1e3) % 16)
                }, n = clearInterval), this.init = function () {
                    function a(b) {
                        if (s.selectiondrag) {
                            if (b) {
                                var c = s.win.outerHeight(),
                                    d = b.pageY - s.selectiondrag.top;
                                d > 0 && c > d && (d = 0), d >= c && (d -= c), s.selectiondrag.df = d
                            }
                            if (0 != s.selectiondrag.df) {
                                var e = 2 * -Math.floor(s.selectiondrag.df / 6);
                                s.doScrollBy(e), s.debounced("doselectionscroll", function () {
                                    a()
                                }, 50)
                            }
                        }
                    }

                    function b() {
                        s.iframexd = !1;
                        try {
                            {
                                var a = "contentDocument" in this ? this.contentDocument : this.contentWindow.document;
                                a.domain
                            }
                        } catch (b) {
                            s.iframexd = !0, a = !1
                        }
                        if (s.iframexd) return "console" in window && console.log("NiceScroll error: policy restriced iframe"), !0;
                        if (s.forcescreen = !0, s.isiframe && (s.iframe = {
                            doc: j(a),
                            html: s.doc.contents().find("html")[0],
                            body: s.doc.contents().find("body")[0]
                        }, s.getContentSize = function () {
                            return {
                            w: Math.max(s.iframe.html.scrollWidth, s.iframe.body.scrollWidth),
                            h: Math.max(s.iframe.html.scrollHeight, s.iframe.body.scrollHeight)
                        }
                        }, s.docscroll = j(s.iframe.body)), !w.isios && s.opt.iframeautoresize && !s.isiframe) {
                            s.win.scrollTop(0), s.doc.height("");
                            var c = Math.max(a.getElementsByTagName("html")[0].scrollHeight, a.body.scrollHeight);
                            s.doc.height(c)
                        }
                        s.lazyResize(30), w.isie7 && s.css(j(s.iframe.html), {
                            "overflow-y": "hidden"
                        }), s.css(j(s.iframe.body), {
                            "overflow-y": "hidden"
                        }), w.isios && s.haswrapper && s.css(j(a.body), {
                            "-webkit-transform": "translate3d(0,0,0)"
                        }), "contentWindow" in this ? s.bind(this.contentWindow, "scroll", s.onscroll) : s.bind(a, "scroll", s.onscroll), s.opt.enablemousewheel && s.bind(a, "mousewheel", s.onmousewheel), s.opt.enablekeyboard && s.bind(a, w.isopera ? "keypress" : "keydown", s.onkeypress), (w.cantouch || s.opt.touchbehavior) && (s.bind(a, "mousedown", s.ontouchstart), s.bind(a, "mousemove", function (a) {
                            s.ontouchmove(a, !0)
                        }), s.opt.grabcursorenabled && w.cursorgrabvalue && s.css(j(a.body), {
                            cursor: w.cursorgrabvalue
                        })), s.bind(a, "mouseup", s.ontouchend), s.zoom && (s.opt.dblclickzoom && s.bind(a, "dblclick", s.doZoom), s.ongesturezoom && s.bind(a, "gestureend", s.ongesturezoom))
                    }
                    if (s.saved.css = [], w.isie7mobile) return !0;
                    if (w.isoperamini) return !0;
                    if (w.hasmstouch && s.css(s.ispage ? j("html") : s.win, {
                        "-ms-touch-action": "none"
                    }), s.zindex = "auto", s.zindex = s.ispage || "auto" != s.opt.zindex ? s.opt.zindex : d() || "auto", s.ispage || "auto" == s.zindex || s.zindex > i && (i = s.zindex), s.isie && 0 == s.zindex && "auto" == s.opt.zindex && (s.zindex = "auto"), !s.ispage || !w.cantouch && !w.isieold && !w.isie9mobile) {
                        var c = s.docscroll;
                        s.ispage && (c = s.haswrapper ? s.win : s.doc), w.isie9mobile || s.css(c, {
                            "overflow-y": "hidden"
                        }), s.ispage && w.isie7 && ("BODY" == s.doc[0].nodeName ? s.css(j("html"), {
                            "overflow-y": "hidden"
                        }) : "HTML" == s.doc[0].nodeName && s.css(j("body"), {
                            "overflow-y": "hidden"
                        })), !w.isios || s.ispage || s.haswrapper || s.css(j("body"), {
                            "-webkit-overflow-scrolling": "touch"
                        });
                        var h = j(document.createElement("div"));
                        h.css({
                            position: "relative",
                            top: 0,
                            "float": "right",
                            width: s.opt.cursorwidth,
                            height: "0px",
                            "background-color": s.opt.cursorcolor,
                            border: s.opt.cursorborder,
                            "background-clip": "padding-box",
                            "-webkit-border-radius": s.opt.cursorborderradius,
                            "-moz-border-radius": s.opt.cursorborderradius,
                            "border-radius": s.opt.cursorborderradius
                        }), h.hborder = parseFloat(h.outerHeight() - h.innerHeight()), s.cursor = h;
                        var l = j(document.createElement("div"));
                        l.attr("id", s.id), l.addClass("nicescroll-rails");
                        var m, n, o = ["left", "right"];
                        for (var p in o) n = o[p], m = s.opt.railpadding[n], m ? l.css("padding-" + n, m + "px") : s.opt.railpadding[n] = 0;
                        l.append(h), l.width = Math.max(parseFloat(s.opt.cursorwidth), h.outerWidth()) + s.opt.railpadding.left + s.opt.railpadding.right, l.css({
                            width: l.width + "px",
                            zIndex: s.zindex,
                            background: s.opt.background,
                            cursor: "default"
                        }), l.visibility = !0, l.scrollable = !0, l.align = "left" == s.opt.railalign ? 0 : 1, s.rail = l, s.rail.drag = !1;
                        var r = !1;
                        if (!s.opt.boxzoom || s.ispage || w.isieold || (r = document.createElement("div"), s.bind(r, "click", s.doZoom), s.zoom = j(r), s.zoom.css({
                            cursor: "pointer",
                            "z-index": s.zindex,
                            backgroundImage: "url(" + k + "zoomico.png)",
                            height: 18,
                            width: 18,
                            backgroundPosition: "0px 0px"
                        }), s.opt.dblclickzoom && s.bind(s.win, "dblclick", s.doZoom), w.cantouch && s.opt.gesturezoom && (s.ongesturezoom = function (a) {
                            return a.scale > 1.5 && s.doZoomIn(a), a.scale < .8 && s.doZoomOut(a), s.cancelEvent(a)
                        }, s.bind(s.win, "gestureend", s.ongesturezoom))), s.railh = !1, s.opt.horizrailenabled) {
                            s.css(c, {
                                "overflow-x": "hidden"
                            });
                            var h = j(document.createElement("div"));
                            h.css({
                                position: "relative",
                                top: 0,
                                height: s.opt.cursorwidth,
                                width: "0px",
                                "background-color": s.opt.cursorcolor,
                                border: s.opt.cursorborder,
                                "background-clip": "padding-box",
                                "-webkit-border-radius": s.opt.cursorborderradius,
                                "-moz-border-radius": s.opt.cursorborderradius,
                                "border-radius": s.opt.cursorborderradius
                            }), h.wborder = parseFloat(h.outerWidth() - h.innerWidth()), s.cursorh = h;
                            var t = j(document.createElement("div"));
                            t.attr("id", s.id + "-hr"), t.addClass("nicescroll-rails"), t.height = Math.max(parseFloat(s.opt.cursorwidth), h.outerHeight()), t.css({
                                height: t.height + "px",
                                zIndex: s.zindex,
                                background: s.opt.background
                            }), t.append(h), t.visibility = !0, t.scrollable = !0, t.align = "top" == s.opt.railvalign ? 0 : 1, s.railh = t, s.railh.drag = !1
                        }
                        if (s.ispage) l.css({
                            position: "fixed",
                            top: "0px",
                            height: "100%"
                        }), l.css(l.align ? {
                            right: "0px"
                        } : {
                            left: "0px"
                        }), s.body.append(l), s.railh && (t.css({
                            position: "fixed",
                            left: "0px",
                            width: "100%"
                        }), t.css(t.align ? {
                            bottom: "0px"
                        } : {
                            top: "0px"
                        }), s.body.append(t));
                        else {
                            if (s.ishwscroll) {
                                "static" == s.win.css("position") && s.css(s.win, {
                                    position: "relative"
                                });
                                var u = "HTML" == s.win[0].nodeName ? s.body : s.win;
                                s.zoom && (s.zoom.css({
                                    position: "absolute",
                                    top: 1,
                                    right: 0,
                                    "margin-right": l.width + 4
                                }), u.append(s.zoom)), l.css({
                                    position: "absolute",
                                    top: 0
                                }), l.css(l.align ? {
                                    right: 0
                                } : {
                                    left: 0
                                }), u.append(l), t && (t.css({
                                    position: "absolute",
                                    left: 0,
                                    bottom: 0
                                }), t.css(t.align ? {
                                    bottom: 0
                                } : {
                                    top: 0
                                }), u.append(t))
                            } else {
                                s.isfixed = "fixed" == s.win.css("position");
                                var x = s.isfixed ? "fixed" : "absolute";
                                s.isfixed || (s.viewport = s.getViewport(s.win[0])), s.viewport && (s.body = s.viewport, 0 == /fixed|relative|absolute/.test(s.viewport.css("position")) && s.css(s.viewport, {
                                    position: "relative"
                                })), l.css({
                                    position: x
                                }), s.zoom && s.zoom.css({
                                    position: x
                                }), s.updateScrollBar(), s.body.append(l), s.zoom && s.body.append(s.zoom), s.railh && (t.css({
                                    position: x
                                }), s.body.append(t))
                            }
                            w.isios && s.css(s.win, {
                                "-webkit-tap-highlight-color": "rgba(0,0,0,0)",
                                "-webkit-touch-callout": "none"
                            }), w.isie && s.opt.disableoutline && s.win.attr("hideFocus", "true"), w.iswebkit && s.opt.disableoutline && s.win.css({
                                outline: "none"
                            })
                        } if (s.opt.autohidemode === !1 ? (s.autohidedom = !1, s.rail.css({
                            opacity: s.opt.cursoropacitymax
                        }), s.railh && s.railh.css({
                            opacity: s.opt.cursoropacitymax
                        })) : s.opt.autohidemode === !0 || "leave" === s.opt.autohidemode ? (s.autohidedom = j().add(s.rail), w.isie8 && (s.autohidedom = s.autohidedom.add(s.cursor)), s.railh && (s.autohidedom = s.autohidedom.add(s.railh)), s.railh && w.isie8 && (s.autohidedom = s.autohidedom.add(s.cursorh))) : "scroll" == s.opt.autohidemode ? (s.autohidedom = j().add(s.rail), s.railh && (s.autohidedom = s.autohidedom.add(s.railh))) : "cursor" == s.opt.autohidemode ? (s.autohidedom = j().add(s.cursor), s.railh && (s.autohidedom = s.autohidedom.add(s.cursorh))) : "hidden" == s.opt.autohidemode && (s.autohidedom = !1, s.hide(), s.locked = !1), w.isie9mobile) {
                            s.scrollmom = new v(s), s.onmangotouch = function () {
                                var a = s.getScrollTop(),
                                    b = s.getScrollLeft();
                                if (a == s.scrollmom.lastscrolly && b == s.scrollmom.lastscrollx) return !0;
                                var c = a - s.mangotouch.sy,
                                    d = b - s.mangotouch.sx,
                                    e = Math.round(Math.sqrt(Math.pow(d, 2) + Math.pow(c, 2)));
                                if (0 != e) {
                                    var f = 0 > c ? -1 : 1,
                                        g = 0 > d ? -1 : 1,
                                        h = +new Date;
                                    if (s.mangotouch.lazy && clearTimeout(s.mangotouch.lazy), h - s.mangotouch.tm > 80 || s.mangotouch.dry != f || s.mangotouch.drx != g) s.scrollmom.stop(), s.scrollmom.reset(b, a), s.mangotouch.sy = a, s.mangotouch.ly = a, s.mangotouch.sx = b, s.mangotouch.lx = b, s.mangotouch.dry = f, s.mangotouch.drx = g, s.mangotouch.tm = h;
                                    else {
                                        s.scrollmom.stop(), s.scrollmom.update(s.mangotouch.sx - d, s.mangotouch.sy - c); {
                                            h - s.mangotouch.tm
                                        }
                                        s.mangotouch.tm = h;
                                        var i = Math.max(Math.abs(s.mangotouch.ly - a), Math.abs(s.mangotouch.lx - b));
                                        s.mangotouch.ly = a, s.mangotouch.lx = b, i > 2 && (s.mangotouch.lazy = setTimeout(function () {
                                            s.mangotouch.lazy = !1, s.mangotouch.dry = 0, s.mangotouch.drx = 0, s.mangotouch.tm = 0, s.scrollmom.doMomentum(30)
                                        }, 100))
                                    }
                                }
                            };
                            var y = s.getScrollTop(),
                                z = s.getScrollLeft();
                            s.mangotouch = {
                                sy: y,
                                ly: y,
                                dry: 0,
                                sx: z,
                                lx: z,
                                drx: 0,
                                lazy: !1,
                                tm: 0
                            }, s.bind(s.docscroll, "scroll", s.onmangotouch)
                        } else {
                            if (w.cantouch || s.istouchcapable || s.opt.touchbehavior || w.hasmstouch) {
                                s.scrollmom = new v(s), s.ontouchstart = function (a) {
                                    if (a.pointerType && 2 != a.pointerType) return !1;
                                    if (s.hasmoving = !1, !s.locked) {
                                        if (w.hasmstouch)
                                            for (var b = a.target ? a.target : !1; b;) {
                                                var c = j(b).getNiceScroll();
                                                if (c.length > 0 && c[0].me == s.me) break;
                                                if (c.length > 0) return !1;
                                                if ("DIV" == b.nodeName && b.id == s.id) break;
                                                b = b.parentNode ? b.parentNode : !1
                                            }
                                        s.cancelScroll();
                                        var b = s.getTarget(a);
                                        if (b) {
                                            var d = /INPUT/i.test(b.nodeName) && /range/i.test(b.type);
                                            if (d) return s.stopPropagation(a)
                                        }
                                        if (!("clientX" in a) && "changedTouches" in a && (a.clientX = a.changedTouches[0].clientX, a.clientY = a.changedTouches[0].clientY), s.forcescreen) {
                                            var e = a,
                                                a = {
                                                    original: a.original ? a.original : a
                                                };
                                            a.clientX = e.screenX, a.clientY = e.screenY
                                        }
                                        if (s.rail.drag = {
                                            x: a.clientX,
                                            y: a.clientY,
                                            sx: s.scroll.x,
                                            sy: s.scroll.y,
                                            st: s.getScrollTop(),
                                            sl: s.getScrollLeft(),
                                            pt: 2,
                                            dl: !1
                                        }, s.ispage || !s.opt.directionlockdeadzone) s.rail.drag.dl = "f";
                                        else {
                                            var f = {
                                                w: j(window).width(),
                                                h: j(window).height()
                                            },
                                                g = {
                                                    w: Math.max(document.body.scrollWidth, document.documentElement.scrollWidth),
                                                    h: Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                                                },
                                                h = Math.max(0, g.h - f.h),
                                                i = Math.max(0, g.w - f.w);
                                            s.rail.drag.ck = !s.rail.scrollable && s.railh.scrollable ? h > 0 ? "v" : !1 : s.rail.scrollable && !s.railh.scrollable && i > 0 ? "h" : !1, s.rail.drag.ck || (s.rail.drag.dl = "f")
                                        } if (s.opt.touchbehavior && s.isiframe && w.isie) {
                                            var k = s.win.position();
                                            s.rail.drag.x += k.left, s.rail.drag.y += k.top
                                        }
                                        if (s.hasmoving = !1, s.lastmouseup = !1, s.scrollmom.reset(a.clientX, a.clientY), !w.cantouch && !this.istouchcapable && !w.hasmstouch) {
                                            var l = b ? /INPUT|SELECT|TEXTAREA/i.test(b.nodeName) : !1;
                                            if (!l) return !s.ispage && w.hasmousecapture && b.setCapture(), s.opt.touchbehavior ? (b.onclick && !b._onclick && (b._onclick = b.onclick, b.onclick = function (a) {
                                                +new Date - s.scrollmom.lasttime;
                                                return s.hasmoving ? !1 : void b._onclick.call(this, a)
                                            }), s.cancelEvent(a)) : s.stopPropagation(a);
                                            /SUBMIT|CANCEL|BUTTON/i.test(j(b).attr("type")) && (pc = {
                                                tg: b,
                                                click: !1
                                            }, s.preventclick = pc)
                                        }
                                    }
                                }, s.ontouchend = function (a) {
                                    return a.pointerType && 2 != a.pointerType ? !1 : s.rail.drag && 2 == s.rail.drag.pt && (s.scrollmom.doMomentum(), s.rail.drag = !1, s.hasmoving && (s.lastmouseup = !0, s.hideCursor(), w.hasmousecapture && document.releaseCapture(), !w.cantouch)) ? s.cancelEvent(a) : void 0
                                };
                                var A = s.opt.touchbehavior && s.isiframe && !w.hasmousecapture;
                                s.ontouchmove = function (a, b) {
                                    if (a.pointerType && 2 != a.pointerType) return !1;
                                    if (s.rail.drag && 2 == s.rail.drag.pt) {
                                        if (w.cantouch && "undefined" == typeof a.original) return !0;
                                        s.hasmoving = !0, s.preventclick && !s.preventclick.click && (s.preventclick.click = s.preventclick.tg.onclick || !1, s.preventclick.tg.onclick = s.onpreventclick);
                                        var c = j.extend({
                                            original: a
                                        }, a);
                                        if (a = c, "changedTouches" in a && (a.clientX = a.changedTouches[0].clientX, a.clientY = a.changedTouches[0].clientY), s.forcescreen) {
                                            var d = a,
                                                a = {
                                                    original: a.original ? a.original : a
                                                };
                                            a.clientX = d.screenX, a.clientY = d.screenY
                                        }
                                        var e = ofy = 0;
                                        if (A && !b) {
                                            var f = s.win.position();
                                            e = -f.left, ofy = -f.top
                                        }
                                        var g = a.clientY + ofy,
                                            h = g - s.rail.drag.y,
                                            i = a.clientX + e,
                                            k = i - s.rail.drag.x,
                                            l = s.rail.drag.st - h;
                                        if (s.ishwscroll && s.opt.bouncescroll ? 0 > l ? l = Math.round(l / 2) : l > s.page.maxh && (l = s.page.maxh + Math.round((l - s.page.maxh) / 2)) : (0 > l && (l = 0, g = 0), l > s.page.maxh && (l = s.page.maxh, g = 0)), s.railh && s.railh.scrollable) {
                                            var m = s.rail.drag.sl - k;
                                            s.ishwscroll && s.opt.bouncescroll ? 0 > m ? m = Math.round(m / 2) : m > s.page.maxw && (m = s.page.maxw + Math.round((m - s.page.maxw) / 2)) : (0 > m && (m = 0, i = 0), m > s.page.maxw && (m = s.page.maxw, i = 0))
                                        }
                                        var n = !1;
                                        if (s.rail.drag.dl) n = !0, "v" == s.rail.drag.dl ? m = s.rail.drag.sl : "h" == s.rail.drag.dl && (l = s.rail.drag.st);
                                        else {
                                            var o = Math.abs(h),
                                                p = Math.abs(k),
                                                q = s.opt.directionlockdeadzone;
                                            if ("v" == s.rail.drag.ck) {
                                                if (o > q && .3 * o >= p) return s.rail.drag = !1, !0;
                                                p > q && (s.rail.drag.dl = "f", j("body").scrollTop(j("body").scrollTop()))
                                            } else if ("h" == s.rail.drag.ck) {
                                                if (p > q && .3 * p >= o) return s.rail.drag = !1, !0;
                                                o > q && (s.rail.drag.dl = "f", j("body").scrollLeft(j("body").scrollLeft()))
                                            }
                                        } if (s.synched("touchmove", function () {
                                            s.rail.drag && 2 == s.rail.drag.pt && (s.prepareTransition && s.prepareTransition(0), s.rail.scrollable && s.setScrollTop(l), s.scrollmom.update(i, g), s.railh && s.railh.scrollable ? (s.setScrollLeft(m), s.showCursor(l, m)) : s.showCursor(l), w.isie10 && document.selection.clear())
                                        }), w.ischrome && s.istouchcapable && (n = !1), n) return s.cancelEvent(a)
                                    }
                                }
                            }
                            s.onmousedown = function (a, b) {
                                if (!s.rail.drag || 1 == s.rail.drag.pt) {
                                    if (s.locked) return s.cancelEvent(a);
                                    s.cancelScroll(), s.rail.drag = {
                                        x: a.clientX,
                                        y: a.clientY,
                                        sx: s.scroll.x,
                                        sy: s.scroll.y,
                                        pt: 1,
                                        hr: !!b
                                    };
                                    var c = s.getTarget(a);
                                    return !s.ispage && w.hasmousecapture && c.setCapture(), s.isiframe && !w.hasmousecapture && (s.saved.csspointerevents = s.doc.css("pointer-events"), s.css(s.doc, {
                                        "pointer-events": "none"
                                    })), s.cancelEvent(a)
                                }
                            }, s.onmouseup = function (a) {
                                if (s.rail.drag) {
                                    if (w.hasmousecapture && document.releaseCapture(), s.isiframe && !w.hasmousecapture && s.doc.css("pointer-events", s.saved.csspointerevents), 1 != s.rail.drag.pt) return;
                                    return s.rail.drag = !1, s.cancelEvent(a)
                                }
                            }, s.onmousemove = function (a) {
                                if (s.rail.drag) {
                                    if (1 != s.rail.drag.pt) return;
                                    if (w.ischrome && 0 == a.which) return s.onmouseup(a);
                                    if (s.cursorfreezed = !0, s.rail.drag.hr) {
                                        s.scroll.x = s.rail.drag.sx + (a.clientX - s.rail.drag.x), s.scroll.x < 0 && (s.scroll.x = 0);
                                        var b = s.scrollvaluemaxw;
                                        s.scroll.x > b && (s.scroll.x = b)
                                    } else {
                                        s.scroll.y = s.rail.drag.sy + (a.clientY - s.rail.drag.y), s.scroll.y < 0 && (s.scroll.y = 0);
                                        var c = s.scrollvaluemax;
                                        s.scroll.y > c && (s.scroll.y = c)
                                    }
                                    return s.synched("mousemove", function () {
                                        s.rail.drag && 1 == s.rail.drag.pt && (s.showCursor(), s.rail.drag.hr ? s.doScrollLeft(Math.round(s.scroll.x * s.scrollratio.x), s.opt.cursordragspeed) : s.doScrollTop(Math.round(s.scroll.y * s.scrollratio.y), s.opt.cursordragspeed))
                                    }), s.cancelEvent(a)
                                }
                            }, w.cantouch || s.opt.touchbehavior ? (s.onpreventclick = function (a) {
                                return s.preventclick ? (s.preventclick.tg.onclick = s.preventclick.click, s.preventclick = !1, s.cancelEvent(a)) : void 0
                            }, s.bind(s.win, "mousedown", s.ontouchstart), s.onclick = w.isios ? !1 : function (a) {
                                return s.lastmouseup ? (s.lastmouseup = !1, s.cancelEvent(a)) : !0
                            }, s.opt.grabcursorenabled && w.cursorgrabvalue && (s.css(s.ispage ? s.doc : s.win, {
                                cursor: w.cursorgrabvalue
                            }), s.css(s.rail, {
                                cursor: w.cursorgrabvalue
                            }))) : (s.hasTextSelected = "getSelection" in document ? function () {
                                return document.getSelection().rangeCount > 0
                            } : "selection" in document ? function () {
                                return "None" != document.selection.type
                            } : function () {
                                return !1
                            }, s.onselectionstart = function () {
                                s.ispage || (s.selectiondrag = s.win.offset())
                            }, s.onselectionend = function () {
                                s.selectiondrag = !1
                            }, s.onselectiondrag = function (b) {
                                s.selectiondrag && s.hasTextSelected() && s.debounced("selectionscroll", function () {
                                    a(b)
                                }, 250)
                            }), w.hasmstouch && (s.css(s.rail, {
                                "-ms-touch-action": "none"
                            }), s.css(s.cursor, {
                                "-ms-touch-action": "none"
                            }), s.bind(s.win, "MSPointerDown", s.ontouchstart), s.bind(document, "MSPointerUp", s.ontouchend), s.bind(document, "MSPointerMove", s.ontouchmove), s.bind(s.cursor, "MSGestureHold", function (a) {
                                a.preventDefault()
                            }), s.bind(s.cursor, "contextmenu", function (a) {
                                a.preventDefault()
                            })), this.istouchcapable && (s.bind(s.win, "touchstart", s.ontouchstart), s.bind(document, "touchend", s.ontouchend), s.bind(document, "touchcancel", s.ontouchend), s.bind(document, "touchmove", s.ontouchmove)), s.bind(s.cursor, "mousedown", s.onmousedown), s.bind(s.cursor, "mouseup", s.onmouseup), s.railh && (s.bind(s.cursorh, "mousedown", function (a) {
                                s.onmousedown(a, !0)
                            }), s.bind(s.cursorh, "mouseup", function (a) {
                                return s.rail.drag && 2 == s.rail.drag.pt ? void 0 : (s.rail.drag = !1, s.hasmoving = !1, s.hideCursor(), w.hasmousecapture && document.releaseCapture(), s.cancelEvent(a))
                            })), (s.opt.cursordragontouch || !w.cantouch && !s.opt.touchbehavior) && (s.rail.css({
                                cursor: "default"
                            }), s.railh && s.railh.css({
                                cursor: "default"
                            }), s.jqbind(s.rail, "mouseenter", function () {
                                s.canshowonmouseevent && s.showCursor(), s.rail.active = !0
                            }), s.jqbind(s.rail, "mouseleave", function () {
                                s.rail.active = !1, s.rail.drag || s.hideCursor()
                            }), s.opt.sensitiverail && (s.bind(s.rail, "click", function (a) {
                                s.doRailClick(a, !1, !1)
                            }), s.bind(s.rail, "dblclick", function (a) {
                                s.doRailClick(a, !0, !1)
                            }), s.bind(s.cursor, "click", function (a) {
                                s.cancelEvent(a)
                            }), s.bind(s.cursor, "dblclick", function (a) {
                                s.cancelEvent(a)
                            })), s.railh && (s.jqbind(s.railh, "mouseenter", function () {
                                s.canshowonmouseevent && s.showCursor(), s.rail.active = !0
                            }), s.jqbind(s.railh, "mouseleave", function () {
                                s.rail.active = !1, s.rail.drag || s.hideCursor()
                            }), s.opt.sensitiverail && (s.bind(s.railh, "click", function (a) {
                                s.doRailClick(a, !1, !0)
                            }), s.bind(s.railh, "dblclick", function (a) {
                                s.doRailClick(a, !0, !0)
                            }), s.bind(s.cursorh, "click", function (a) {
                                s.cancelEvent(a)
                            }), s.bind(s.cursorh, "dblclick", function (a) {
                                s.cancelEvent(a)
                            })))), w.cantouch || s.opt.touchbehavior ? (s.bind(w.hasmousecapture ? s.win : document, "mouseup", s.ontouchend), s.bind(document, "mousemove", s.ontouchmove), s.onclick && s.bind(document, "click", s.onclick), s.opt.cursordragontouch && (s.bind(s.cursor, "mousedown", s.onmousedown), s.bind(s.cursor, "mousemove", s.onmousemove), s.cursorh && s.bind(s.cursorh, "mousedown", function (a) {
                                s.onmousedown(a, !0)
                            }), s.cursorh && s.bind(s.cursorh, "mousemove", s.onmousemove))) : (s.bind(w.hasmousecapture ? s.win : document, "mouseup", s.onmouseup), s.bind(document, "mousemove", s.onmousemove), s.onclick && s.bind(document, "click", s.onclick), !s.ispage && s.opt.enablescrollonselection && (s.bind(s.win[0], "mousedown", s.onselectionstart), s.bind(document, "mouseup", s.onselectionend), s.bind(s.cursor, "mouseup", s.onselectionend), s.cursorh && s.bind(s.cursorh, "mouseup", s.onselectionend), s.bind(document, "mousemove", s.onselectiondrag)), s.zoom && (s.jqbind(s.zoom, "mouseenter", function () {
                                s.canshowonmouseevent && s.showCursor(), s.rail.active = !0
                            }), s.jqbind(s.zoom, "mouseleave", function () {
                                s.rail.active = !1, s.rail.drag || s.hideCursor()
                            }))), s.opt.enablemousewheel && (s.isiframe || s.bind(w.isie && s.ispage ? document : s.win, "mousewheel", s.onmousewheel), s.bind(s.rail, "mousewheel", s.onmousewheel), s.railh && s.bind(s.railh, "mousewheel", s.onmousewheelhr)), s.ispage || w.cantouch || /HTML|BODY/.test(s.win[0].nodeName) || (s.win.attr("tabindex") || s.win.attr({
                                tabindex: g++
                            }), s.jqbind(s.win, "focus", function (a) {
                                e = s.getTarget(a).id || !0, s.hasfocus = !0, s.canshowonmouseevent && s.noticeCursor()
                            }), s.jqbind(s.win, "blur", function () {
                                e = !1, s.hasfocus = !1
                            }), s.jqbind(s.win, "mouseenter", function (a) {
                                f = s.getTarget(a).id || !0, s.hasmousefocus = !0, s.canshowonmouseevent && s.noticeCursor()
                            }), s.jqbind(s.win, "mouseleave", function () {
                                f = !1, s.hasmousefocus = !1, s.rail.drag || s.hideCursor()
                            }))
                        } if (s.onkeypress = function (a) {
                            if (s.locked && 0 == s.page.maxh) return !0;
                            a = a ? a : window.e;
                            var b = s.getTarget(a);
                            if (b && /INPUT|TEXTAREA|SELECT|OPTION/.test(b.nodeName)) {
                                var c = b.getAttribute("type") || b.type || !1;
                                if (!c || !/submit|button|cancel/i.tp) return !0
                        }
                            if (s.hasfocus || s.hasmousefocus && !e || s.ispage && !e && !f) {
                                var d = a.keyCode;
                                if (s.locked && 27 != d) return s.cancelEvent(a);
                                var g = a.ctrlKey || !1,
                                    h = a.shiftKey || !1,
                                    i = !1;
                                switch (d) {
                                    case 38:
                                    case 63233:
                                        s.doScrollBy(72), i = !0;
                                        break;
                                    case 40:
                                    case 63235:
                                        s.doScrollBy(-72), i = !0;
                                        break;
                                    case 37:
                                    case 63232:
                                        s.railh && (g ? s.doScrollLeft(0) : s.doScrollLeftBy(72), i = !0);
                                        break;
                                    case 39:
                                    case 63234:
                                        s.railh && (g ? s.doScrollLeft(s.page.maxw) : s.doScrollLeftBy(-72), i = !0);
                                        break;
                                    case 33:
                                    case 63276:
                                        s.doScrollBy(s.view.h), i = !0;
                                        break;
                                    case 34:
                                    case 63277:
                                        s.doScrollBy(-s.view.h), i = !0;
                                        break;
                                    case 36:
                                    case 63273:
                                        s.railh && g ? s.doScrollPos(0, 0) : s.doScrollTo(0), i = !0;
                                        break;
                                    case 35:
                                    case 63275:
                                        s.railh && g ? s.doScrollPos(s.page.maxw, s.page.maxh) : s.doScrollTo(s.page.maxh), i = !0;
                                        break;
                                    case 32:
                                        s.opt.spacebarenabled && (s.doScrollBy(h ? s.view.h : -s.view.h), i = !0);
                                        break;
                                    case 27:
                                        s.zoomactive && (s.doZoom(), i = !0)
                        }
                                if (i) return s.cancelEvent(a)
                        }
                        }, s.opt.enablekeyboard && s.bind(document, w.isopera && !w.isopera12 ? "keypress" : "keydown", s.onkeypress), s.bind(window, "resize", s.lazyResize), s.bind(window, "orientationchange", s.lazyResize), s.bind(window, "load", s.lazyResize), w.ischrome && !s.ispage && !s.haswrapper) {
                            var B = s.win.attr("style"),
                                C = parseFloat(s.win.css("width")) + 1;
                            s.win.css("width", C), s.synched("chromefix", function () {
                                s.win.attr("style", B)
                            })
                        }
                        s.onAttributeChange = function () {
                            s.lazyResize(250)
                        }, s.ispage || s.haswrapper || (q !== !1 ? (s.observer = new q(function (a) {
                            a.forEach(s.onAttributeChange)
                        }), s.observer.observe(s.win[0], {
                            childList: !0,
                            characterData: !1,
                            attributes: !0,
                            subtree: !1
                        }), s.observerremover = new q(function (a) {
                            a.forEach(function (a) {
                                if (a.removedNodes.length > 0)
                                    for (var b in a.removedNodes)
                                        if (a.removedNodes[b] == s.win[0]) return s.remove()
                            })
                        }), s.observerremover.observe(s.win[0].parentNode, {
                            childList: !0,
                            characterData: !1,
                            attributes: !1,
                            subtree: !1
                        })) : (s.bind(s.win, w.isie && !w.isie9 ? "propertychange" : "DOMAttrModified", s.onAttributeChange), w.isie9 && s.win[0].attachEvent("onpropertychange", s.onAttributeChange), s.bind(s.win, "DOMNodeRemoved", function (a) {
                            a.target == s.win[0] && s.remove()
                        }))), !s.ispage && s.opt.boxzoom && s.bind(window, "resize", s.resizeZoom), s.istextarea && s.bind(s.win, "mouseup", s.lazyResize), s.checkrtlmode = !0, s.lazyResize(30)
                    }
                    "IFRAME" == this.doc[0].nodeName && (this.doc[0].readyState && "complete" == this.doc[0].readyState && setTimeout(function () {
                        b.call(s.doc[0], !1)
                    }, 500), s.bind(this.doc, "load", b))
                }, this.showCursor = function (a, b) {
                    s.cursortimeout && (clearTimeout(s.cursortimeout), s.cursortimeout = 0), s.rail && (s.autohidedom && (s.autohidedom.stop().css({
                        opacity: s.opt.cursoropacitymax
                    }), s.cursoractive = !0), s.rail.drag && 1 == s.rail.drag.pt || ("undefined" != typeof a && a !== !1 && (s.scroll.y = Math.round(1 * a / s.scrollratio.y)), "undefined" != typeof b && (s.scroll.x = Math.round(1 * b / s.scrollratio.x))), s.cursor.css({
                        height: s.cursorheight,
                        top: s.scroll.y
                    }), s.cursorh && (s.cursorh.css(!s.rail.align && s.rail.visibility ? {
                        width: s.cursorwidth,
                        left: s.scroll.x + s.rail.width
                    } : {
                        width: s.cursorwidth,
                        left: s.scroll.x
                    }), s.cursoractive = !0), s.zoom && s.zoom.stop().css({
                        opacity: s.opt.cursoropacitymax
                    }))
                }, this.hideCursor = function (a) {
                    s.cursortimeout || s.rail && s.autohidedom && (s.hasmousefocus && "leave" == s.opt.autohidemode || (s.cursortimeout = setTimeout(function () {
                        s.rail.active && s.showonmouseevent || (s.autohidedom.stop().animate({
                            opacity: s.opt.cursoropacitymin
                        }), s.zoom && s.zoom.stop().animate({
                            opacity: s.opt.cursoropacitymin
                        }), s.cursoractive = !1), s.cursortimeout = 0
                    }, a || s.opt.hidecursordelay)))
                }, this.noticeCursor = function (a, b, c) {
                    s.showCursor(b, c), s.rail.active || s.hideCursor(a)
                }, this.getContentSize = s.ispage ? function () {
                    return {
                        w: Math.max(document.body.scrollWidth, document.documentElement.scrollWidth),
                        h: Math.max(document.body.scrollHeight, document.documentElement.scrollHeight)
                    }
                } : s.haswrapper ? function () {
                    return {
                        w: s.doc.outerWidth() + parseInt(s.win.css("paddingLeft")) + parseInt(s.win.css("paddingRight")),
                        h: s.doc.outerHeight() + parseInt(s.win.css("paddingTop")) + parseInt(s.win.css("paddingBottom"))
                    }
                } : function () {
                    return {
                        w: s.docscroll[0].scrollWidth,
                        h: s.docscroll[0].scrollHeight
                    }
                }, this.onResize = function (a, b) {
                    if (!s || !s.win) return !1;
                    if (!s.haswrapper && !s.ispage) {
                        if ("none" == s.win.css("display")) return s.visibility && s.hideRail().hideRailHr(), !1;
                        s.hidden || s.visibility || s.showRail().showRailHr()
                    }
                    var c = s.page.maxh,
                        d = s.page.maxw,
                        e = {
                            h: s.view.h,
                            w: s.view.w
                        };
                    if (s.view = {
                        w: s.ispage ? s.win.width() : parseInt(s.win[0].clientWidth),
                        h: s.ispage ? s.win.height() : parseInt(s.win[0].clientHeight)
                    }, s.page = b ? b : s.getContentSize(), s.page.maxh = Math.max(0, s.page.h - s.view.h), s.page.maxw = Math.max(0, s.page.w - s.view.w), s.page.maxh == c && s.page.maxw == d && s.view.w == e.w) {
                        if (s.ispage) return s;
                        var f = s.win.offset();
                        if (s.lastposition) {
                            var g = s.lastposition;
                            if (g.top == f.top && g.left == f.left) return s
                        }
                        s.lastposition = f
                    }
                    if (0 == s.page.maxh ? (s.hideRail(), s.scrollvaluemax = 0, s.scroll.y = 0, s.scrollratio.y = 0, s.cursorheight = 0, s.setScrollTop(0), s.rail.scrollable = !1) : s.rail.scrollable = !0, 0 == s.page.maxw ? (s.hideRailHr(), s.scrollvaluemaxw = 0, s.scroll.x = 0, s.scrollratio.x = 0, s.cursorwidth = 0, s.setScrollLeft(0), s.railh.scrollable = !1) : s.railh.scrollable = !0, s.locked = 0 == s.page.maxh && 0 == s.page.maxw, s.locked) return s.ispage || s.updateScrollBar(s.view), !1;
                    s.hidden || s.visibility ? s.hidden || s.railh.visibility || s.showRailHr() : s.showRail().showRailHr(), s.istextarea && s.win.css("resize") && "none" != s.win.css("resize") && (s.view.h -= 20), s.cursorheight = Math.min(s.view.h, Math.round(s.view.h * (s.view.h / s.page.h))), s.cursorheight = s.opt.cursorfixedheight ? s.opt.cursorfixedheight : Math.max(s.opt.cursorminheight, s.cursorheight), s.cursorwidth = Math.min(s.view.w, Math.round(s.view.w * (s.view.w / s.page.w))), s.cursorwidth = s.opt.cursorfixedheight ? s.opt.cursorfixedheight : Math.max(s.opt.cursorminheight, s.cursorwidth), s.scrollvaluemax = s.view.h - s.cursorheight - s.cursor.hborder, s.railh && (s.railh.width = s.page.maxh > 0 ? s.view.w - s.rail.width : s.view.w, s.scrollvaluemaxw = s.railh.width - s.cursorwidth - s.cursorh.wborder), s.checkrtlmode && s.railh && (s.checkrtlmode = !1, s.opt.rtlmode && 0 == s.scroll.x && s.setScrollLeft(s.page.maxw)), s.ispage || s.updateScrollBar(s.view), s.scrollratio = {
                        x: s.page.maxw / s.scrollvaluemaxw,
                        y: s.page.maxh / s.scrollvaluemax
                    };
                    var h = s.getScrollTop();
                    return h > s.page.maxh ? s.doScrollTop(s.page.maxh) : (s.scroll.y = Math.round(s.getScrollTop() * (1 / s.scrollratio.y)), s.scroll.x = Math.round(s.getScrollLeft() * (1 / s.scrollratio.x)), s.cursoractive && s.noticeCursor()), s.scroll.y && 0 == s.getScrollTop() && s.doScrollTo(Math.floor(s.scroll.y * s.scrollratio.y)), s
                }, this.resize = s.onResize, this.lazyResize = function (a) {
                    return a = isNaN(a) ? 30 : a, s.delayed("resize", s.resize, a), s
                }, this._bind = function (a, b, c, d) {
                    s.events.push({
                        e: a,
                        n: b,
                        f: c,
                        b: d,
                        q: !1
                    }), a.addEventListener ? a.addEventListener(b, c, d || !1) : a.attachEvent ? a.attachEvent("on" + b, c) : a["on" + b] = c
                }, this.jqbind = function (a, b, c) {
                    s.events.push({
                        e: a,
                        n: b,
                        f: c,
                        q: !0
                    }), j(a).bind(b, c)
                }, this.bind = function (a, b, c, d) {
                    var e = "jquery" in a ? a[0] : a;
                    if ("mousewheel" == b)
                        if ("onwheel" in s.win) s._bind(e, "wheel", c, d || !1);
                        else {
                            var f = "undefined" != typeof document.onmousewheel ? "mousewheel" : "DOMMouseScroll";
                            o(e, f, c, d || !1), "DOMMouseScroll" == f && o(e, "MozMousePixelScroll", c, d || !1)
                        } else if (e.addEventListener) {
                            if (w.cantouch && /mouseup|mousedown|mousemove/.test(b)) {
                                var g = "mousedown" == b ? "touchstart" : "mouseup" == b ? "touchend" : "touchmove";
                                s._bind(e, g, function (a) {
                                    if (a.touches) {
                                        if (a.touches.length < 2) {
                                            var b = a.touches.length ? a.touches[0] : a;
                                            b.original = a, c.call(this, b)
                                        }
                                    } else if (a.changedTouches) {
                                        var b = a.changedTouches[0];
                                        b.original = a, c.call(this, b)
                                    }
                                }, d || !1)
                            }
                            s._bind(e, b, c, d || !1), w.cantouch && "mouseup" == b && s._bind(e, "touchcancel", c, d || !1)
                        } else s._bind(e, b, function (a) {
                            return a = a || window.event || !1, a && a.srcElement && (a.target = a.srcElement), "pageY" in a || (a.pageX = a.clientX + document.documentElement.scrollLeft, a.pageY = a.clientY + document.documentElement.scrollTop), c.call(e, a) === !1 || d === !1 ? s.cancelEvent(a) : !0
                        })
                }, this._unbind = function (a, b, c, d) {
                    a.removeEventListener ? a.removeEventListener(b, c, d) : a.detachEvent ? a.detachEvent("on" + b, c) : a["on" + b] = !1
                }, this.unbindAll = function () {
                    for (var a = 0; a < s.events.length; a++) {
                        var b = s.events[a];
                        b.q ? b.e.unbind(b.n, b.f) : s._unbind(b.e, b.n, b.f, b.b)
                    }
                }, this.cancelEvent = function (a) {
                    var a = a.original ? a.original : a ? a : window.event || !1;
                    return a ? (a.preventDefault && a.preventDefault(), a.stopPropagation && a.stopPropagation(), a.preventManipulation && a.preventManipulation(), a.cancelBubble = !0, a.cancel = !0, a.returnValue = !1, !1) : !1
                }, this.stopPropagation = function (a) {
                    var a = a.original ? a.original : a ? a : window.event || !1;
                    return a ? a.stopPropagation ? a.stopPropagation() : (a.cancelBubble && (a.cancelBubble = !0), !1) : !1
                }, this.showRail = function () {
                    return 0 == s.page.maxh || !s.ispage && "none" == s.win.css("display") || (s.visibility = !0, s.rail.visibility = !0, s.rail.css("display", "block")), s
                }, this.showRailHr = function () {
                    return s.railh ? (0 == s.page.maxw || !s.ispage && "none" == s.win.css("display") || (s.railh.visibility = !0, s.railh.css("display", "block")), s) : s
                }, this.hideRail = function () {
                    return s.visibility = !1, s.rail.visibility = !1, s.rail.css("display", "none"), s
                }, this.hideRailHr = function () {
                    return s.railh ? (s.railh.visibility = !1, s.railh.css("display", "none"), s) : s
                }, this.show = function () {
                    return s.hidden = !1, s.locked = !1, s.showRail().showRailHr()
                }, this.hide = function () {
                    return s.hidden = !0, s.locked = !0, s.hideRail().hideRailHr()
                }, this.toggle = function () {
                    return s.hidden ? s.show() : s.hide()
                }, this.remove = function () {
                    s.stop(), s.cursortimeout && clearTimeout(s.cursortimeout), s.doZoomOut(), s.unbindAll(), w.isie9 && s.win[0].detachEvent("onpropertychange", s.onAttributeChange), s.observer !== !1 && s.observer.disconnect(), s.observerremover !== !1 && s.observerremover.disconnect(), s.events = null, s.cursor && s.cursor.remove(), s.cursorh && s.cursorh.remove(), s.rail && s.rail.remove(), s.railh && s.railh.remove(), s.zoom && s.zoom.remove();
                    for (var a = 0; a < s.saved.css.length; a++) {
                        var b = s.saved.css[a];
                        b[0].css(b[1], "undefined" == typeof b[2] ? "" : b[2])
                    }
                    s.saved = !1, s.me.data("__nicescroll", "");
                    var c = j.nicescroll;
                    c.each(function (a) {
                        if (this && this.id === s.id) {
                            delete c[a];
                            for (var b = ++a; b < c.length; b++, a++) c[a] = c[b];
                            c.length--, c.length && delete c[c.length]
                        }
                    });
                    for (var d in s) s[d] = null, delete s[d];
                    s = null
                }, this.scrollstart = function (a) {
                    return this.onscrollstart = a, s
                }, this.scrollend = function (a) {
                    return this.onscrollend = a, s
                }, this.scrollcancel = function (a) {
                    return this.onscrollcancel = a, s
                }, this.zoomin = function (a) {
                    return this.onzoomin = a, s
                }, this.zoomout = function (a) {
                    return this.onzoomout = a, s
                }, this.isScrollable = function (a) {
                    var b = a.target ? a.target : a;
                    if ("OPTION" == b.nodeName) return !0;
                    for (; b && 1 == b.nodeType && !/BODY|HTML/.test(b.nodeName) ;) {
                        var c = j(b),
                            d = c.css("overflowY") || c.css("overflowX") || c.css("overflow") || "";
                        if (/scroll|auto/.test(d)) return b.clientHeight != b.scrollHeight;
                        b = b.parentNode ? b.parentNode : !1
                    }
                    return !1
                }, this.getViewport = function (a) {
                    for (var b = a && a.parentNode ? a.parentNode : !1; b && 1 == b.nodeType && !/BODY|HTML/.test(b.nodeName) ;) {
                        var c = j(b);
                        if (/fixed|absolute/.test(c.css("position"))) return c;
                        var d = c.css("overflowY") || c.css("overflowX") || c.css("overflow") || "";
                        if (/scroll|auto/.test(d) && b.clientHeight != b.scrollHeight) return c;
                        if (c.getNiceScroll().length > 0) return c;
                        b = b.parentNode ? b.parentNode : !1
                    }
                    return !1
                }, this.onmousewheel = function (a) {
                    if (s.locked) return s.debounced("checkunlock", s.resize, 250), !0;
                    if (s.rail.drag) return s.cancelEvent(a);
                    if ("auto" == s.opt.oneaxismousemode && 0 != a.deltaX && (s.opt.oneaxismousemode = !1), s.opt.oneaxismousemode && 0 == a.deltaX && !s.rail.scrollable) return s.railh && s.railh.scrollable ? s.onmousewheelhr(a) : !0;
                    var b = +new Date,
                        c = !1;
                    if (s.opt.preservenativescrolling && s.checkarea + 600 < b && (s.nativescrollingarea = s.isScrollable(a), c = !0), s.checkarea = b, s.nativescrollingarea) return !0;
                    var d = p(a, !1, c);
                    return d && (s.checkarea = 0), d
                }, this.onmousewheelhr = function (a) {
                    if (s.locked || !s.railh.scrollable) return !0;
                    if (s.rail.drag) return s.cancelEvent(a);
                    var b = +new Date,
                        c = !1;
                    return s.opt.preservenativescrolling && s.checkarea + 600 < b && (s.nativescrollingarea = s.isScrollable(a), c = !0), s.checkarea = b, s.nativescrollingarea ? !0 : s.locked ? s.cancelEvent(a) : p(a, !0, c)
                }, this.stop = function () {
                    return s.cancelScroll(), s.scrollmon && s.scrollmon.stop(), s.cursorfreezed = !1, s.scroll.y = Math.round(s.getScrollTop() * (1 / s.scrollratio.y)), s.noticeCursor(), s
                }, this.getTransitionSpeed = function (a) {
                    var b = Math.round(10 * s.opt.scrollspeed),
                        c = Math.min(b, Math.round(a / 20 * s.opt.scrollspeed));
                    return c > 20 ? c : 0
                }, s.opt.smoothscroll ? s.ishwscroll && w.hastransition && s.opt.usetransition ? (this.prepareTransition = function (a, b) {
                    var c = b ? a > 20 ? a : 0 : s.getTransitionSpeed(a),
                        d = c ? w.prefixstyle + "transform " + c + "ms ease-out" : "";
                    return s.lasttransitionstyle && s.lasttransitionstyle == d || (s.lasttransitionstyle = d, s.doc.css(w.transitionstyle, d)), c
                }, this.doScrollLeft = function (a, b) {
                    var c = s.scrollrunning ? s.newscrolly : s.getScrollTop();
                    s.doScrollPos(a, c, b)
                }, this.doScrollTop = function (a, b) {
                    var c = s.scrollrunning ? s.newscrollx : s.getScrollLeft();
                    s.doScrollPos(c, a, b)
                }, this.doScrollPos = function (a, b, c) {
                    var d = s.getScrollTop(),
                        e = s.getScrollLeft();
                    return ((s.newscrolly - d) * (b - d) < 0 || (s.newscrollx - e) * (a - e) < 0) && s.cancelScroll(), 0 == s.opt.bouncescroll && (0 > b ? b = 0 : b > s.page.maxh && (b = s.page.maxh), 0 > a ? a = 0 : a > s.page.maxw && (a = s.page.maxw)), s.scrollrunning && a == s.newscrollx && b == s.newscrolly ? !1 : (s.newscrolly = b, s.newscrollx = a, s.newscrollspeed = c || !1, s.timer ? !1 : void (s.timer = setTimeout(function () {
                        var c = s.getScrollTop(),
                            d = s.getScrollLeft(),
                            e = {};
                        e.x = a - d, e.y = b - c, e.px = d, e.py = c;
                        var f = Math.round(Math.sqrt(Math.pow(e.x, 2) + Math.pow(e.y, 2))),
                            g = s.newscrollspeed && s.newscrollspeed > 1 ? s.newscrollspeed : s.getTransitionSpeed(f);
                        if (s.newscrollspeed && s.newscrollspeed <= 1 && (g *= s.newscrollspeed), s.prepareTransition(g, !0), s.timerscroll && s.timerscroll.tm && clearInterval(s.timerscroll.tm), g > 0) {
                            if (!s.scrollrunning && s.onscrollstart) {
                                var h = {
                                    type: "scrollstart",
                                    current: {
                                        x: d,
                                        y: c
                                    },
                                    request: {
                                        x: a,
                                        y: b
                                    },
                                    end: {
                                        x: s.newscrollx,
                                        y: s.newscrolly
                                    },
                                    speed: g
                                };
                                s.onscrollstart.call(s, h)
                            }
                            w.transitionend ? s.scrollendtrapped || (s.scrollendtrapped = !0, s.bind(s.doc, w.transitionend, s.onScrollEnd, !1)) : (s.scrollendtrapped && clearTimeout(s.scrollendtrapped), s.scrollendtrapped = setTimeout(s.onScrollEnd, g));
                            var i = c,
                                j = d;
                            s.timerscroll = {
                                bz: new BezierClass(i, s.newscrolly, g, 0, 0, .58, 1),
                                bh: new BezierClass(j, s.newscrollx, g, 0, 0, .58, 1)
                            }, s.cursorfreezed || (s.timerscroll.tm = setInterval(function () {
                                s.showCursor(s.getScrollTop(), s.getScrollLeft())
                            }, 60))
                        }
                        s.synched("doScroll-set", function () {
                            s.timer = 0, s.scrollendtrapped && (s.scrollrunning = !0), s.setScrollTop(s.newscrolly), s.setScrollLeft(s.newscrollx), s.scrollendtrapped || s.onScrollEnd()
                        })
                    }, 50)))
                }, this.cancelScroll = function () {
                    if (!s.scrollendtrapped) return !0;
                    var a = s.getScrollTop(),
                        b = s.getScrollLeft();
                    return s.scrollrunning = !1, w.transitionend || clearTimeout(w.transitionend), s.scrollendtrapped = !1, s._unbind(s.doc, w.transitionend, s.onScrollEnd), s.prepareTransition(0), s.setScrollTop(a), s.railh && s.setScrollLeft(b), s.timerscroll && s.timerscroll.tm && clearInterval(s.timerscroll.tm), s.timerscroll = !1, s.cursorfreezed = !1, s.showCursor(a, b), s
                }, this.onScrollEnd = function () {
                    s.scrollendtrapped && s._unbind(s.doc, w.transitionend, s.onScrollEnd), s.scrollendtrapped = !1, s.prepareTransition(0), s.timerscroll && s.timerscroll.tm && clearInterval(s.timerscroll.tm), s.timerscroll = !1;
                    var a = s.getScrollTop(),
                        b = s.getScrollLeft();
                    if (s.setScrollTop(a), s.railh && s.setScrollLeft(b), s.noticeCursor(!1, a, b), s.cursorfreezed = !1, 0 > a ? a = 0 : a > s.page.maxh && (a = s.page.maxh), 0 > b ? b = 0 : b > s.page.maxw && (b = s.page.maxw), a != s.newscrolly || b != s.newscrollx) return s.doScrollPos(b, a, s.opt.snapbackspeed);
                    if (s.onscrollend && s.scrollrunning) {
                        var c = {
                            type: "scrollend",
                            current: {
                                x: b,
                                y: a
                            },
                            end: {
                                x: s.newscrollx,
                                y: s.newscrolly
                            }
                        };
                        s.onscrollend.call(s, c)
                    }
                    s.scrollrunning = !1
                }) : (this.doScrollLeft = function (a, b) {
                    var c = s.scrollrunning ? s.newscrolly : s.getScrollTop();
                    s.doScrollPos(a, c, b)
                }, this.doScrollTop = function (a, b) {
                    var c = s.scrollrunning ? s.newscrollx : s.getScrollLeft();
                    s.doScrollPos(c, a, b)
                }, this.doScrollPos = function (a, b, c) {
                    function d() {
                        if (s.cancelAnimationFrame) return !0;
                        if (s.scrollrunning = !0, k = 1 - k) return s.timer = m(d) || 1;
                        var a = 0,
                            b = sy = s.getScrollTop();
                        if (s.dst.ay) {
                            b = s.bzscroll ? s.dst.py + s.bzscroll.getNow() * s.dst.ay : s.newscrolly;
                            var c = b - sy;
                            (0 > c && b < s.newscrolly || c > 0 && b > s.newscrolly) && (b = s.newscrolly), s.setScrollTop(b), b == s.newscrolly && (a = 1)
                        } else a = 1;
                        var e = sx = s.getScrollLeft();
                        if (s.dst.ax) {
                            e = s.bzscroll ? s.dst.px + s.bzscroll.getNow() * s.dst.ax : s.newscrollx;
                            var c = e - sx;
                            (0 > c && e < s.newscrollx || c > 0 && e > s.newscrollx) && (e = s.newscrollx), s.setScrollLeft(e), e == s.newscrollx && (a += 1)
                        } else a += 1; if (2 == a) {
                            if (s.timer = 0, s.cursorfreezed = !1, s.bzscroll = !1, s.scrollrunning = !1, 0 > b ? b = 0 : b > s.page.maxh && (b = s.page.maxh), 0 > e ? e = 0 : e > s.page.maxw && (e = s.page.maxw), e != s.newscrollx || b != s.newscrolly) s.doScrollPos(e, b);
                            else if (s.onscrollend) {
                                var f = {
                                    type: "scrollend",
                                    current: {
                                        x: sx,
                                        y: sy
                                    },
                                    end: {
                                        x: s.newscrollx,
                                        y: s.newscrolly
                                    }
                                };
                                s.onscrollend.call(s, f)
                            }
                        } else s.timer = m(d) || 1
                    }
                    var b = "undefined" == typeof b || b === !1 ? s.getScrollTop(!0) : b;
                    if (s.timer && s.newscrolly == b && s.newscrollx == a) return !0;
                    s.timer && n(s.timer), s.timer = 0;
                    var e = s.getScrollTop(),
                        f = s.getScrollLeft();
                    ((s.newscrolly - e) * (b - e) < 0 || (s.newscrollx - f) * (a - f) < 0) && s.cancelScroll(), s.newscrolly = b, s.newscrollx = a, s.bouncescroll && s.rail.visibility || (s.newscrolly < 0 ? s.newscrolly = 0 : s.newscrolly > s.page.maxh && (s.newscrolly = s.page.maxh)), s.bouncescroll && s.railh.visibility || (s.newscrollx < 0 ? s.newscrollx = 0 : s.newscrollx > s.page.maxw && (s.newscrollx = s.page.maxw)), s.dst = {}, s.dst.x = a - f, s.dst.y = b - e, s.dst.px = f, s.dst.py = e;
                    var g = Math.round(Math.sqrt(Math.pow(s.dst.x, 2) + Math.pow(s.dst.y, 2)));
                    s.dst.ax = s.dst.x / g, s.dst.ay = s.dst.y / g;
                    var h = 0,
                        i = g;
                    0 == s.dst.x ? (h = e, i = b, s.dst.ay = 1, s.dst.py = 0) : 0 == s.dst.y && (h = f, i = a, s.dst.ax = 1, s.dst.px = 0);
                    var j = s.getTransitionSpeed(g);
                    if (c && 1 >= c && (j *= c), s.bzscroll = j > 0 ? s.bzscroll ? s.bzscroll.update(i, j) : new BezierClass(h, i, j, 0, 1, 0, 1) : !1, !s.timer) {
                        (e == s.page.maxh && b >= s.page.maxh || f == s.page.maxw && a >= s.page.maxw) && s.checkContentSize();
                        var k = 1;
                        if (s.cancelAnimationFrame = !1, s.timer = 1, s.onscrollstart && !s.scrollrunning) {
                            var l = {
                                type: "scrollstart",
                                current: {
                                    x: f,
                                    y: e
                                },
                                request: {
                                    x: a,
                                    y: b
                                },
                                end: {
                                    x: s.newscrollx,
                                    y: s.newscrolly
                                },
                                speed: j
                            };
                            s.onscrollstart.call(s, l)
                        }
                        d(), (e == s.page.maxh && b >= e || f == s.page.maxw && a >= f) && s.checkContentSize(), s.noticeCursor()
                    }
                }, this.cancelScroll = function () {
                    return s.timer && n(s.timer), s.timer = 0, s.bzscroll = !1, s.scrollrunning = !1, s
                }) : (this.doScrollLeft = function (a, b) {
                    var c = s.getScrollTop();
                    s.doScrollPos(a, c, b)
                }, this.doScrollTop = function (a, b) {
                    var c = s.getScrollLeft();
                    s.doScrollPos(c, a, b)
                }, this.doScrollPos = function (a, b) {
                    var c = a > s.page.maxw ? s.page.maxw : a;
                    0 > c && (c = 0);
                    var d = b > s.page.maxh ? s.page.maxh : b;
                    0 > d && (d = 0), s.synched("scroll", function () {
                        s.setScrollTop(d), s.setScrollLeft(c)
                    })
                }, this.cancelScroll = function () { }), this.doScrollBy = function (a, b) {
                    var c = 0;
                    if (b) c = Math.floor((s.scroll.y - a) * s.scrollratio.y);
                    else {
                        var d = s.timer ? s.newscrolly : s.getScrollTop(!0);
                        c = d - a
                    } if (s.bouncescroll) {
                        var e = Math.round(s.view.h / 2); -e > c ? c = -e : c > s.page.maxh + e && (c = s.page.maxh + e)
                    }
                    return s.cursorfreezed = !1, py = s.getScrollTop(!0), 0 > c && 0 >= py ? s.noticeCursor() : c > s.page.maxh && py >= s.page.maxh ? (s.checkContentSize(), s.noticeCursor()) : void s.doScrollTop(c)
                }, this.doScrollLeftBy = function (a, b) {
                    var c = 0;
                    if (b) c = Math.floor((s.scroll.x - a) * s.scrollratio.x);
                    else {
                        var d = s.timer ? s.newscrollx : s.getScrollLeft(!0);
                        c = d - a
                    } if (s.bouncescroll) {
                        var e = Math.round(s.view.w / 2); -e > c ? c = -e : c > s.page.maxw + e && (c = s.page.maxw + e)
                    }
                    return s.cursorfreezed = !1, px = s.getScrollLeft(!0), 0 > c && 0 >= px ? s.noticeCursor() : c > s.page.maxw && px >= s.page.maxw ? s.noticeCursor() : void s.doScrollLeft(c)
                }, this.doScrollTo = function (a, b) {
                    var c = b ? Math.round(a * s.scrollratio.y) : a;
                    0 > c ? c = 0 : c > s.page.maxh && (c = s.page.maxh), s.cursorfreezed = !1, s.doScrollTop(a)
                }, this.checkContentSize = function () {
                    var a = s.getContentSize();
                    (a.h != s.page.h || a.w != s.page.w) && s.resize(!1, a)
                }, s.onscroll = function () {
                    s.rail.drag || s.cursorfreezed || s.synched("scroll", function () {
                        s.scroll.y = Math.round(s.getScrollTop() * (1 / s.scrollratio.y)), s.railh && (s.scroll.x = Math.round(s.getScrollLeft() * (1 / s.scrollratio.x))), s.noticeCursor()
                    })
                }, s.bind(s.docscroll, "scroll", s.onscroll), this.doZoomIn = function (a) {
                    if (!s.zoomactive) {
                        s.zoomactive = !0, s.zoomrestore = {
                            style: {}
                        };
                        var b = ["position", "top", "left", "zIndex", "backgroundColor", "marginTop", "marginBottom", "marginLeft", "marginRight"],
                            c = s.win[0].style;
                        for (var d in b) {
                            var e = b[d];
                            s.zoomrestore.style[e] = "undefined" != typeof c[e] ? c[e] : ""
                        }
                        s.zoomrestore.style.width = s.win.css("width"), s.zoomrestore.style.height = s.win.css("height"), s.zoomrestore.padding = {
                            w: s.win.outerWidth() - s.win.width(),
                            h: s.win.outerHeight() - s.win.height()
                        }, w.isios4 && (s.zoomrestore.scrollTop = j(window).scrollTop(), j(window).scrollTop(0)), s.win.css({
                            position: w.isios4 ? "absolute" : "fixed",
                            top: 0,
                            left: 0,
                            "z-index": i + 100,
                            margin: "0px"
                        });
                        var f = s.win.css("backgroundColor");
                        return ("" == f || /transparent|rgba\(0, 0, 0, 0\)|rgba\(0,0,0,0\)/.test(f)) && s.win.css("backgroundColor", "#fff"), s.rail.css({
                            "z-index": i + 101
                        }), s.zoom.css({
                            "z-index": i + 102
                        }), s.zoom.css("backgroundPosition", "0px -18px"), s.resizeZoom(), s.onzoomin && s.onzoomin.call(s), s.cancelEvent(a)
                    }
                }, this.doZoomOut = function (a) {
                    return s.zoomactive ? (s.zoomactive = !1, s.win.css("margin", ""), s.win.css(s.zoomrestore.style), w.isios4 && j(window).scrollTop(s.zoomrestore.scrollTop), s.rail.css({
                        "z-index": s.zindex
                    }), s.zoom.css({
                        "z-index": s.zindex
                    }), s.zoomrestore = !1, s.zoom.css("backgroundPosition", "0px 0px"), s.onResize(), s.onzoomout && s.onzoomout.call(s), s.cancelEvent(a)) : void 0
                }, this.doZoom = function (a) {
                    return s.zoomactive ? s.doZoomOut(a) : s.doZoomIn(a)
                }, this.resizeZoom = function () {
                    if (s.zoomactive) {
                        var a = s.getScrollTop();
                        s.win.css({
                            width: j(window).width() - s.zoomrestore.padding.w + "px",
                            height: j(window).height() - s.zoomrestore.padding.h + "px"
                        }), s.onResize(), s.setScrollTop(Math.min(s.page.maxh, a))
                    }
                }, this.init(), j.nicescroll.push(this)
            },
            v = function (a) {
                var b = this;
                this.nc = a, this.lastx = 0, this.lasty = 0, this.speedx = 0, this.speedy = 0, this.lasttime = 0, this.steptime = 0, this.snapx = !1, this.snapy = !1, this.demulx = 0, this.demuly = 0, this.lastscrollx = -1, this.lastscrolly = -1, this.chkx = 0, this.chky = 0, this.timer = 0, this.time = function () {
                    return +new Date
                }, this.reset = function (a, c) {
                    b.stop();
                    var d = b.time();
                    b.steptime = 0, b.lasttime = d, b.speedx = 0, b.speedy = 0, b.lastx = a, b.lasty = c, b.lastscrollx = -1, b.lastscrolly = -1
                }, this.update = function (a, c) {
                    var d = b.time();
                    b.steptime = d - b.lasttime, b.lasttime = d;
                    var e = c - b.lasty,
                        f = a - b.lastx,
                        g = b.nc.getScrollTop(),
                        h = b.nc.getScrollLeft(),
                        i = g + e,
                        j = h + f;
                    b.snapx = 0 > j || j > b.nc.page.maxw, b.snapy = 0 > i || i > b.nc.page.maxh, b.speedx = f, b.speedy = e, b.lastx = a, b.lasty = c
                }, this.stop = function () {
                    b.nc.unsynched("domomentum2d"), b.timer && clearTimeout(b.timer), b.timer = 0, b.lastscrollx = -1, b.lastscrolly = -1
                }, this.doSnapy = function (a, c) {
                    var d = !1;
                    0 > c ? (c = 0, d = !0) : c > b.nc.page.maxh && (c = b.nc.page.maxh, d = !0), 0 > a ? (a = 0, d = !0) : a > b.nc.page.maxw && (a = b.nc.page.maxw, d = !0), d && b.nc.doScrollPos(a, c, b.nc.opt.snapbackspeed)
                }, this.doMomentum = function (a) {
                    var c = b.time(),
                        d = a ? c + a : b.lasttime,
                        e = b.nc.getScrollLeft(),
                        f = b.nc.getScrollTop(),
                        g = b.nc.page.maxh,
                        h = b.nc.page.maxw;
                    b.speedx = h > 0 ? Math.min(60, b.speedx) : 0, b.speedy = g > 0 ? Math.min(60, b.speedy) : 0;
                    var i = d && 60 >= c - d;
                    (0 > f || f > g || 0 > e || e > h) && (i = !1);
                    var j = b.speedy && i ? b.speedy : !1,
                        k = b.speedx && i ? b.speedx : !1;
                    if (j || k) {
                        var l = Math.max(16, b.steptime);
                        if (l > 50) {
                            var m = l / 50;
                            b.speedx *= m, b.speedy *= m, l = 50
                        }
                        b.demulxy = 0, b.lastscrollx = b.nc.getScrollLeft(), b.chkx = b.lastscrollx, b.lastscrolly = b.nc.getScrollTop(), b.chky = b.lastscrolly;
                        var n = b.lastscrollx,
                            o = b.lastscrolly,
                            p = function () {
                                var a = b.time() - c > 600 ? .04 : .02;
                                b.speedx && (n = Math.floor(b.lastscrollx - b.speedx * (1 - b.demulxy)), b.lastscrollx = n, (0 > n || n > h) && (a = .1)), b.speedy && (o = Math.floor(b.lastscrolly - b.speedy * (1 - b.demulxy)), b.lastscrolly = o, (0 > o || o > g) && (a = .1)), b.demulxy = Math.min(1, b.demulxy + a), b.nc.synched("domomentum2d", function () {
                                    if (b.speedx) {
                                        var a = b.nc.getScrollLeft();
                                        a != b.chkx && b.stop(), b.chkx = n, b.nc.setScrollLeft(n)
                                    }
                                    if (b.speedy) {
                                        var c = b.nc.getScrollTop();
                                        c != b.chky && b.stop(), b.chky = o, b.nc.setScrollTop(o)
                                    }
                                    b.timer || (b.nc.hideCursor(), b.doSnapy(n, o))
                                }), b.demulxy < 1 ? b.timer = setTimeout(p, l) : (b.stop(), b.nc.hideCursor(), b.doSnapy(n, o))
                            };
                        p()
                    } else b.doSnapy(b.nc.getScrollLeft(), b.nc.getScrollTop())
                }
            },
            w = b.fn.scrollTop;
        b.cssHooks.pageYOffset = {
            get: function (a) {
                var b = j.data(a, "__nicescroll") || !1;
                return b && b.ishwscroll ? b.getScrollTop() : w.call(a)
            },
            set: function (a, b) {
                var c = j.data(a, "__nicescroll") || !1;
                return c && c.ishwscroll ? c.setScrollTop(parseInt(b)) : w.call(a, b), this
            }
        }, b.fn.scrollTop = function (a) {
            if ("undefined" == typeof a) {
                var b = this[0] ? j.data(this[0], "__nicescroll") || !1 : !1;
                return b && b.ishwscroll ? b.getScrollTop() : w.call(this)
            }
            return this.each(function () {
                var b = j.data(this, "__nicescroll") || !1;
                b && b.ishwscroll ? b.setScrollTop(parseInt(a)) : w.call(j(this), a)
            })
        };
        var x = b.fn.scrollLeft;
        j.cssHooks.pageXOffset = {
            get: function (a) {
                var b = j.data(a, "__nicescroll") || !1;
                return b && b.ishwscroll ? b.getScrollLeft() : x.call(a)
            },
            set: function (a, b) {
                var c = j.data(a, "__nicescroll") || !1;
                return c && c.ishwscroll ? c.setScrollLeft(parseInt(b)) : x.call(a, b), this
            }
        }, b.fn.scrollLeft = function (a) {
            if ("undefined" == typeof a) {
                var b = this[0] ? j.data(this[0], "__nicescroll") || !1 : !1;
                return b && b.ishwscroll ? b.getScrollLeft() : x.call(this)
            }
            return this.each(function () {
                var b = j.data(this, "__nicescroll") || !1;
                b && b.ishwscroll ? b.setScrollLeft(parseInt(a)) : x.call(j(this), a)
            })
        };
        var y = function (b) {
            var c = this;
            if (this.length = 0, this.name = "nicescrollarray", this.each = function (a) {
                for (var b = 0, d = 0; b < c.length; b++) a.call(c[b], d++);
                return c
            }, this.push = function (a) {
                c[c.length] = a, c.length++
            }, this.eq = function (a) {
                return c[a]
            }, b)
                for (a = 0; a < b.length; a++) {
                    var d = j.data(b[a], "__nicescroll") || !1;
                    d && (this[this.length] = d, this.length++)
                }
            return this
        };
        d(y.prototype, ["show", "hide", "toggle", "onResize", "resize", "remove", "stop", "doScrollPos"], function (a, b) {
            a[b] = function () {
                var a = arguments;
                return this.each(function () {
                    this[b].apply(this, a)
                })
            }
        }), b.fn.getNiceScroll = function (a) {
            if ("undefined" == typeof a) return new y(this);
            var b = this[a] && j.data(this[a], "__nicescroll") || !1;
            return b
        }, b.extend(b.expr[":"], {
            nicescroll: function (a) {
                return j.data(a, "__nicescroll") ? !0 : !1
            }
        }), j.fn.niceScroll = function (a, b) {
            "undefined" == typeof b && ("object" != typeof a || "jquery" in a || (b = a, a = !1));
            var c = new y;
            "undefined" == typeof b && (b = {}), a && (b.doc = j(a), b.win = j(this));
            var d = !("doc" in b);
            return d || "win" in b || (b.win = j(this)), this.each(function () {
                var a = j(this).data("__nicescroll") || !1;
                a || (b.doc = d ? j(this) : b.doc, a = new u(b, j(this)), j(this).data("__nicescroll", a)), c.push(a)
            }), 1 == c.length ? c[0] : c
        }, window.NiceScroll = {
            getjQuery: function () {
                return b
            }
        }, j.nicescroll || (j.nicescroll = new y, j.nicescroll.options = r)
    }(jQuery), $(document).ready(function () {
        $(".overlay-button").click(function () {
            var a = $(this).attr("data-theme"),
                b = $(this).attr("data-opacity"),
                c = $(this).attr("data-style"),
                d = '<div id="loader-overlay" class="ui-front loader ui-widget-overlay ' + a + " opacity-" + b + '"><img src="../assets/images/spinner/loader-' + c + '.gif" alt="" /></div>';
            $("#loader-overlay").length && $("#loader-overlay").remove(), $("body").append(d), $("#loader-overlay").fadein("fast"), setTimeout(function () {
                $("#loader-overlay").fadeout("fast")
            }, 5e3)
        }), $(".refresh-button").click(function (a) {
            $(".glyph-icon", this).addClass("icon-spin"), a.preventDefault();
            var b = $(this).parents(".content-box"),
                c = $(this).attr("data-theme"),
                d = $(this).attr("data-opacity"),
                e = $(this).attr("data-style"),
                f = '<div id="refresh-overlay" class="ui-front loader ui-widget-overlay ' + c + " opacity-" + d + '"><img src="../assets/images/spinner/loader-' + e + '.gif" alt="" /></div>';
            $("#refresh-overlay").length && $("#refresh-overlay").remove(), $(b).append(f), $("#refresh-overlay").fadeIn("fast"), setTimeout(function () {
                $("#refresh-overlay").fadeout("fast")
            }, 1500)
        })
    }),
    function (a) {
        "function" == typeof define && define.amd ? define(["jquery", "./core", "./widget", "./position", "./menu"], a) : a(jQuery)
    }(function (a) {
        return a.widget("ui.autocomplete", {
            version: "@VERSION",
            defaultElement: "<input>",
            options: {
                appendTo: null,
                autoFocus: !1,
                delay: 300,
                minLength: 1,
                position: {
                    my: "left top",
                    at: "left bottom",
                    collision: "none"
                },
                source: null,
                change: null,
                close: null,
                focus: null,
                open: null,
                response: null,
                search: null,
                select: null
            },
            requestIndex: 0,
            pending: 0,
            _create: function () {
                var b, c, d, e = this.element[0].nodeName.toLowerCase(),
                    f = "textarea" === e,
                    g = "input" === e;
                this.isMultiLine = f ? !0 : g ? !1 : this.element.prop("isContentEditable"), this.valueMethod = this.element[f || g ? "val" : "text"], this.isNewMenu = !0, this.element.addClass("ui-autocomplete-input").attr("autocomplete", "off"), this._on(this.element, {
                    keydown: function (e) {
                        if (this.element.prop("readOnly")) return b = !0, d = !0, void (c = !0);
                        b = !1, d = !1, c = !1;
                        var f = a.ui.keyCode;
                        switch (e.keyCode) {
                            case f.PAGE_UP:
                                b = !0, this._move("previousPage", e);
                                break;
                            case f.PAGE_DOWN:
                                b = !0, this._move("nextPage", e);
                                break;
                            case f.UP:
                                b = !0, this._keyEvent("previous", e);
                                break;
                            case f.DOWN:
                                b = !0, this._keyEvent("next", e);
                                break;
                            case f.ENTER:
                                this.menu.active && (b = !0, e.preventDefault(), this.menu.select(e));
                                break;
                            case f.TAB:
                                this.menu.active && this.menu.select(e);
                                break;
                            case f.ESCAPE:
                                this.menu.element.is(":visible") && (this.isMultiLine || this._value(this.term), this.close(e), e.preventDefault());
                                break;
                            default:
                                c = !0, this._searchTimeout(e)
                        }
                    },
                    keypress: function (d) {
                        if (b) return b = !1, void ((!this.isMultiLine || this.menu.element.is(":visible")) && d.preventDefault());
                        if (!c) {
                            var e = a.ui.keyCode;
                            switch (d.keyCode) {
                                case e.PAGE_UP:
                                    this._move("previousPage", d);
                                    break;
                                case e.PAGE_DOWN:
                                    this._move("nextPage", d);
                                    break;
                                case e.UP:
                                    this._keyEvent("previous", d);
                                    break;
                                case e.DOWN:
                                    this._keyEvent("next", d)
                            }
                        }
                    },
                    input: function (a) {
                        return d ? (d = !1, void a.preventDefault()) : void this._searchTimeout(a)
                    },
                    focus: function () {
                        this.selectedItem = null, this.previous = this._value()
                    },
                    blur: function (a) {
                        return this.cancelBlur ? void delete this.cancelBlur : (clearTimeout(this.searching), this.close(a), void this._change(a))
                    }
                }), this._initSource(), this.menu = a("<ul>").addClass("ui-autocomplete ui-front").appendTo(this._appendTo()).menu({
                    role: null
                }).hide().menu("instance"), this._on(this.menu.element, {
                    mousedown: function (b) {
                        b.preventDefault(), this.cancelBlur = !0, this._delay(function () {
                            delete this.cancelBlur
                        });
                        var c = this.menu.element[0];
                        a(b.target).closest(".ui-menu-item").length || this._delay(function () {
                            var b = this;
                            this.document.one("mousedown", function (d) {
                                d.target === b.element[0] || d.target === c || a.contains(c, d.target) || b.close()
                            })
                        })
                    },
                    menufocus: function (b, c) {
                        var d, e;
                        return this.isNewMenu && (this.isNewMenu = !1, b.originalEvent && /^mouse/.test(b.originalEvent.type)) ? (this.menu.blur(), void this.document.one("mousemove", function () {
                            a(b.target).trigger(b.originalEvent)
                        })) : (e = c.item.data("ui-autocomplete-item"), !1 !== this._trigger("focus", b, {
                            item: e
                        }) && b.originalEvent && /^key/.test(b.originalEvent.type) && this._value(e.value), d = c.item.attr("aria-label") || e.value, void (d && jQuery.trim(d).length && (this.liveRegion.children().hide(), a("<div>").text(d).appendTo(this.liveRegion))))
                    },
                    menuselect: function (a, b) {
                        var c = b.item.data("ui-autocomplete-item"),
                            d = this.previous;
                        this.element[0] !== this.document[0].activeElement && (this.element.focus(), this.previous = d, this._delay(function () {
                            this.previous = d, this.selectedItem = c
                        })), !1 !== this._trigger("select", a, {
                            item: c
                        }) && this._value(c.value), this.term = this._value(), this.close(a), this.selectedItem = c
                    }
                }), this.liveRegion = a("<span>", {
                    role: "status",
                    "aria-live": "assertive",
                    "aria-relevant": "additions"
                }).addClass("ui-helper-hidden-accessible").appendTo(this.document[0].body), this._on(this.window, {
                    beforeunload: function () {
                        this.element.removeAttr("autocomplete")
                    }
                })
            },
            _destroy: function () {
                clearTimeout(this.searching), this.element.removeClass("ui-autocomplete-input").removeAttr("autocomplete"), this.menu.element.remove(), this.liveRegion.remove()
            },
            _setOption: function (a, b) {
                this._super(a, b), "source" === a && this._initSource(), "appendTo" === a && this.menu.element.appendTo(this._appendTo()), "disabled" === a && b && this.xhr && this.xhr.abort()
            },
            _appendTo: function () {
                var b = this.options.appendTo;
                return b && (b = b.jquery || b.nodeType ? a(b) : this.document.find(b).eq(0)), b && b[0] || (b = this.element.closest(".ui-front")), b.length || (b = this.document[0].body), b
            },
            _initSource: function () {
                var b, c, d = this;
                a.isArray(this.options.source) ? (b = this.options.source, this.source = function (c, d) {
                    d(a.ui.autocomplete.filter(b, c.term))
                }) : "string" == typeof this.options.source ? (c = this.options.source, this.source = function (b, e) {
                    d.xhr && d.xhr.abort(), d.xhr = a.ajax({
                        url: c,
                        data: b,
                        dataType: "json",
                        success: function (a) {
                            e(a)
                        },
                        error: function () {
                            e([])
                        }
                    })
                }) : this.source = this.options.source
            },
            _searchTimeout: function (a) {
                clearTimeout(this.searching), this.searching = this._delay(function () {
                    var b = this.term === this._value(),
                        c = this.menu.element.is(":visible"),
                        d = a.altKey || a.ctrlKey || a.metaKey || a.shiftKey;
                    (!b || b && !c && !d) && (this.selectedItem = null, this.search(null, a))
                }, this.options.delay)
            },
            search: function (a, b) {
                return a = null != a ? a : this._value(), this.term = this._value(), a.length < this.options.minLength ? this.close(b) : this._trigger("search", b) !== !1 ? this._search(a) : void 0
            },
            _search: function (a) {
                this.pending++, this.element.addClass("ui-autocomplete-loading"), this.cancelSearch = !1, this.source({
                    term: a
                }, this._response())
            },
            _response: function () {
                var b = ++this.requestIndex;
                return a.proxy(function (a) {
                    b === this.requestIndex && this.__response(a), this.pending--, this.pending || this.element.removeClass("ui-autocomplete-loading")
                }, this)
            },
            __response: function (a) {
                a && (a = this._normalize(a)), this._trigger("response", null, {
                    content: a
                }), !this.options.disabled && a && a.length && !this.cancelSearch ? (this._suggest(a), this._trigger("open")) : this._close()
            },
            close: function (a) {
                this.cancelSearch = !0, this._close(a)
            },
            _close: function (a) {
                this.menu.element.is(":visible") && (this.menu.element.hide(), this.menu.blur(), this.isNewMenu = !0, this._trigger("close", a))
            },
            _change: function (a) {
                this.previous !== this._value() && this._trigger("change", a, {
                    item: this.selectedItem
                })
            },
            _normalize: function (b) {
                return b.length && b[0].label && b[0].value ? b : a.map(b, function (b) {
                    return "string" == typeof b ? {
                        label: b,
                        value: b
                    } : a.extend({}, b, {
                        label: b.label || b.value,
                        value: b.value || b.label
                    })
                })
            },
            _suggest: function (b) {
                var c = this.menu.element.empty();
                this._renderMenu(c, b), this.isNewMenu = !0, this.menu.refresh(), c.show(), this._resizeMenu(), c.position(a.extend({
                    of: this.element
                }, this.options.position)), this.options.autoFocus && this.menu.next()
            },
            _resizeMenu: function () {
                var a = this.menu.element;
                a.outerWidth(Math.max(a.width("").outerWidth() + 1, this.element.outerWidth()))
            },
            _renderMenu: function (b, c) {
                var d = this;
                a.each(c, function (a, c) {
                    d._renderItemData(b, c)
                })
            },
            _renderItemData: function (a, b) {
                return this._renderItem(a, b).data("ui-autocomplete-item", b)
            },
            _renderItem: function (b, c) {
                return a("<li>").text(c.label).appendTo(b)
            },
            _move: function (a, b) {
                return this.menu.element.is(":visible") ? this.menu.isFirstItem() && /^previous/.test(a) || this.menu.isLastItem() && /^next/.test(a) ? (this.isMultiLine || this._value(this.term), void this.menu.blur()) : void this.menu[a](b) : void this.search(null, b)
            },
            widget: function () {
                return this.menu.element
            },
            _value: function () {
                return this.valueMethod.apply(this.element, arguments)
            },
            _keyEvent: function (a, b) {
                (!this.isMultiLine || this.menu.element.is(":visible")) && (this._move(a, b), b.preventDefault())
            }
        }), a.extend(a.ui.autocomplete, {
            escapeRegex: function (a) {
                return a.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&")
            },
            filter: function (b, c) {
                var d = new RegExp(a.ui.autocomplete.escapeRegex(c), "i");
                return a.grep(b, function (a) {
                    return d.test(a.label || a.value || a)
                })
            }
        }), a.widget("ui.autocomplete", a.ui.autocomplete, {
            options: {
                messages: {
                    noResults: "No search results.",
                    results: function (a) {
                        return a + (a > 1 ? " results are" : " result is") + " available, use up and down arrow keys to navigate."
                    }
                }
            },
            __response: function (b) {
                var c;
                this._superApply(arguments), this.options.disabled || this.cancelSearch || (c = b && b.length ? this.options.messages.results(b.length) : this.options.messages.noResults, this.liveRegion.children().hide(), a("<div>").text(c).appendTo(this.liveRegion))
            }
        }), a.ui.autocomplete
    }),
    function (a) {
        "function" == typeof define && define.amd ? define(["jquery", "./core", "./widget", "./position"], a) : a(jQuery)
    }(function (a) {
        return a.widget("ui.menu", {
            version: "@VERSION",
            defaultElement: "<ul>",
            delay: 300,
            options: {
                icons: {
                    submenu: "ui-icon-carat-1-e"
                },
                items: "> *",
                menus: "ul",
                position: {
                    my: "left-1 top",
                    at: "right top"
                },
                role: "menu",
                blur: null,
                focus: null,
                select: null
            },
            _create: function () {
                this.activeMenu = this.element, this.mouseHandled = !1, this.element.uniqueId().addClass("ui-menu ui-widget ui-widget-content").toggleClass("ui-menu-icons", !!this.element.find(".ui-icon").length).attr({
                    role: this.options.role,
                    tabIndex: 0
                }), this.options.disabled && this.element.addClass("ui-state-disabled").attr("aria-disabled", "true"), this._on({
                    "mousedown .ui-menu-item": function (a) {
                        a.preventDefault()
                    },
                    "click .ui-menu-item": function (b) {
                        var c = a(b.target);
                        !this.mouseHandled && c.not(".ui-state-disabled").length && (this.select(b), b.isPropagationStopped() || (this.mouseHandled = !0), c.has(".ui-menu").length ? this.expand(b) : !this.element.is(":focus") && a(this.document[0].activeElement).closest(".ui-menu").length && (this.element.trigger("focus", [!0]), this.active && 1 === this.active.parents(".ui-menu").length && clearTimeout(this.timer)))
                    },
                    "mouseenter .ui-menu-item": function (b) {
                        var c = a(b.currentTarget);
                        c.siblings(".ui-state-active").removeClass("ui-state-active"), this.focus(b, c)
                    },
                    mouseleave: "collapseAll",
                    "mouseleave .ui-menu": "collapseAll",
                    focus: function (a, b) {
                        var c = this.active || this.element.find(this.options.items).eq(0);
                        b || this.focus(a, c)
                    },
                    blur: function (b) {
                        this._delay(function () {
                            a.contains(this.element[0], this.document[0].activeElement) || this.collapseAll(b)
                        })
                    },
                    keydown: "_keydown"
                }), this.refresh(), this._on(this.document, {
                    click: function (a) {
                        this._closeOnDocumentClick(a) && this.collapseAll(a), this.mouseHandled = !1
                    }
                })
            },
            _destroy: function () {
                this.element.removeAttr("aria-activedescendant").find(".ui-menu").addBack().removeClass("ui-menu ui-widget ui-widget-content ui-menu-icons ui-front").removeAttr("role").removeAttr("tabIndex").removeAttr("aria-labelledby").removeAttr("aria-expanded").removeAttr("aria-hidden").removeAttr("aria-disabled").removeUniqueId().show(), this.element.find(".ui-menu-item").removeClass("ui-menu-item").removeAttr("role").removeAttr("aria-disabled").removeUniqueId().removeClass("ui-state-hover").removeAttr("tabIndex").removeAttr("role").removeAttr("aria-haspopup").children().each(function () {
                    var b = a(this);
                    b.data("ui-menu-submenu-carat") && b.remove()
                }), this.element.find(".ui-menu-divider").removeClass("ui-menu-divider ui-widget-content")
            },
            _keydown: function (b) {
                function c(a) {
                    return a.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&")
                }
                var d, e, f, g, h, i = !0;
                switch (b.keyCode) {
                    case a.ui.keyCode.PAGE_UP:
                        this.previousPage(b);
                        break;
                    case a.ui.keyCode.PAGE_DOWN:
                        this.nextPage(b);
                        break;
                    case a.ui.keyCode.HOME:
                        this._move("first", "first", b);
                        break;
                    case a.ui.keyCode.END:
                        this._move("last", "last", b);
                        break;
                    case a.ui.keyCode.UP:
                        this.previous(b);
                        break;
                    case a.ui.keyCode.DOWN:
                        this.next(b);
                        break;
                    case a.ui.keyCode.LEFT:
                        this.collapse(b);
                        break;
                    case a.ui.keyCode.RIGHT:
                        this.active && !this.active.is(".ui-state-disabled") && this.expand(b);
                        break;
                    case a.ui.keyCode.ENTER:
                    case a.ui.keyCode.SPACE:
                        this._activate(b);
                        break;
                    case a.ui.keyCode.ESCAPE:
                        this.collapse(b);
                        break;
                    default:
                        i = !1, e = this.previousFilter || "", f = String.fromCharCode(b.keyCode), g = !1, clearTimeout(this.filterTimer), f === e ? g = !0 : f = e + f, h = new RegExp("^" + c(f), "i"), d = this.activeMenu.find(this.options.items).filter(function () {
                            return h.test(a(this).text())
                        }), d = g && -1 !== d.index(this.active.next()) ? this.active.nextAll(".ui-menu-item") : d, d.length || (f = String.fromCharCode(b.keyCode), h = new RegExp("^" + c(f), "i"), d = this.activeMenu.find(this.options.items).filter(function () {
                            return h.test(a(this).text())
                        })), d.length ? (this.focus(b, d), d.length > 1 ? (this.previousFilter = f, this.filterTimer = this._delay(function () {
                            delete this.previousFilter
                        }, 1e3)) : delete this.previousFilter) : delete this.previousFilter
                }
                i && b.preventDefault()
            },
            _activate: function (a) {
                this.active.is(".ui-state-disabled") || (this.active.is("[aria-haspopup='true']") ? this.expand(a) : this.select(a))
            },
            refresh: function () {
                var b, c, d = this,
                    e = this.options.icons.submenu,
                    f = this.element.find(this.options.menus);
                this.element.toggleClass("ui-menu-icons", !!this.element.find(".ui-icon").length), f.filter(":not(.ui-menu)").addClass("ui-menu ui-widget ui-widget-content ui-front").hide().attr({
                    role: this.options.role,
                    "aria-hidden": "true",
                    "aria-expanded": "false"
                }).each(function () {
                    var b = a(this),
                        c = b.parent(),
                        d = a("<span>").addClass("ui-menu-icon ui-icon " + e).data("ui-menu-submenu-carat", !0);
                    c.attr("aria-haspopup", "true").prepend(d), b.attr("aria-labelledby", c.attr("id"))
                }), b = f.add(this.element), c = b.find(this.options.items), c.not(".ui-menu-item").each(function () {
                    var b = a(this);
                    d._isDivider(b) && b.addClass("ui-widget-content ui-menu-divider")
                }), c.not(".ui-menu-item, .ui-menu-divider").addClass("ui-menu-item").uniqueId().attr({
                    tabIndex: -1,
                    role: this._itemRole()
                }), c.filter(".ui-state-disabled").attr("aria-disabled", "true"), this.active && !a.contains(this.element[0], this.active[0]) && this.blur()
            },
            _itemRole: function () {
                return {
                    menu: "menuitem",
                    listbox: "option"
                }[this.options.role]
            },
            _setOption: function (a, b) {
                "icons" === a && this.element.find(".ui-menu-icon").removeClass(this.options.icons.submenu).addClass(b.submenu), "disabled" === a && this.element.toggleClass("ui-state-disabled", !!b).attr("aria-disabled", b), this._super(a, b)
            },
            focus: function (a, b) {
                var c, d;
                this.blur(a, a && "focus" === a.type), this._scrollIntoView(b), this.active = b.first(), d = this.active.addClass("ui-state-focus").removeClass("ui-state-active"), this.options.role && this.element.attr("aria-activedescendant", d.attr("id")), this.active.parent().closest(".ui-menu-item").addClass("ui-state-active"), a && "keydown" === a.type ? this._close() : this.timer = this._delay(function () {
                    this._close()
                }, this.delay), c = b.children(".ui-menu"), c.length && a && /^mouse/.test(a.type) && this._startOpening(c), this.activeMenu = b.parent(), this._trigger("focus", a, {
                    item: b
                })
            },
            _scrollIntoView: function (b) {
                var c, d, e, f, g, h;
                this._hasScroll() && (c = parseFloat(a.css(this.activeMenu[0], "borderTopWidth")) || 0, d = parseFloat(a.css(this.activeMenu[0], "paddingTop")) || 0, e = b.offset().top - this.activeMenu.offset().top - c - d, f = this.activeMenu.scrollTop(), g = this.activeMenu.height(), h = b.outerHeight(), 0 > e ? this.activeMenu.scrollTop(f + e) : e + h > g && this.activeMenu.scrollTop(f + e - g + h))
            },
            blur: function (a, b) {
                b || clearTimeout(this.timer), this.active && (this.active.removeClass("ui-state-focus"), this.active = null, this._trigger("blur", a, {
                    item: this.active
                }))
            },
            _startOpening: function (a) {
                clearTimeout(this.timer), "true" === a.attr("aria-hidden") && (this.timer = this._delay(function () {
                    this._close(), this._open(a)
                }, this.delay))
            },
            _open: function (b) {
                var c = a.extend({
                    of: this.active
                }, this.options.position);
                clearTimeout(this.timer), this.element.find(".ui-menu").not(b.parents(".ui-menu")).hide().attr("aria-hidden", "true"), b.show().removeAttr("aria-hidden").attr("aria-expanded", "true").position(c)
            },
            collapseAll: function (b, c) {
                clearTimeout(this.timer), this.timer = this._delay(function () {
                    var d = c ? this.element : a(b && b.target).closest(this.element.find(".ui-menu"));
                    d.length || (d = this.element), this._close(d), this.blur(b), this.activeMenu = d
                }, this.delay)
            },
            _close: function (a) {
                a || (a = this.active ? this.active.parent() : this.element), a.find(".ui-menu").hide().attr("aria-hidden", "true").attr("aria-expanded", "false").end().find(".ui-state-active").not(".ui-state-focus").removeClass("ui-state-active")
            },
            _closeOnDocumentClick: function (b) {
                return !a(b.target).closest(".ui-menu").length
            },
            _isDivider: function (a) {
                return !/[^\-\u2014\u2013\s]/.test(a.text())
            },
            collapse: function (a) {
                var b = this.active && this.active.parent().closest(".ui-menu-item", this.element);
                b && b.length && (this._close(), this.focus(a, b))
            },
            expand: function (a) {
                var b = this.active && this.active.children(".ui-menu ").find(this.options.items).first();
                b && b.length && (this._open(b.parent()), this._delay(function () {
                    this.focus(a, b)
                }))
            },
            next: function (a) {
                this._move("next", "first", a)
            },
            previous: function (a) {
                this._move("prev", "last", a)
            },
            isFirstItem: function () {
                return this.active && !this.active.prevAll(".ui-menu-item").length
            },
            isLastItem: function () {
                return this.active && !this.active.nextAll(".ui-menu-item").length
            },
            _move: function (a, b, c) {
                var d;
                this.active && (d = "first" === a || "last" === a ? this.active["first" === a ? "prevAll" : "nextAll"](".ui-menu-item").eq(-1) : this.active[a + "All"](".ui-menu-item").eq(0)), d && d.length && this.active || (d = this.activeMenu.find(this.options.items)[b]()), this.focus(c, d)
            },
            nextPage: function (b) {
                var c, d, e;
                return this.active ? void (this.isLastItem() || (this._hasScroll() ? (d = this.active.offset().top, e = this.element.height(), this.active.nextAll(".ui-menu-item").each(function () {
                    return c = a(this), c.offset().top - d - e < 0
                }), this.focus(b, c)) : this.focus(b, this.activeMenu.find(this.options.items)[this.active ? "last" : "first"]()))) : void this.next(b)
            },
            previousPage: function (b) {
                var c, d, e;
                return this.active ? void (this.isFirstItem() || (this._hasScroll() ? (d = this.active.offset().top, e = this.element.height(), this.active.prevAll(".ui-menu-item").each(function () {
                    return c = a(this), c.offset().top - d + e > 0
                }), this.focus(b, c)) : this.focus(b, this.activeMenu.find(this.options.items).first()))) : void this.next(b)
            },
            _hasScroll: function () {
                return this.element.outerHeight() < this.element.prop("scrollHeight")
            },
            select: function (b) {
                this.active = this.active || a(b.target).closest(".ui-menu-item");
                var c = {
                    item: this.active
                };
                this.active.has(".ui-menu").length || this.collapseAll(b, !0), this._trigger("select", b, c)
            }
        })
    }),
    function (a) {
        "use strict";

        function b(a, b, c, d) {
            a.beginPath(), a.arc(b, c, d, 0, s, !1), a.fill()
        }

        function c(a, b, c, d, e) {
            a.beginPath(), a.moveTo(b, c), a.lineTo(d, e), a.stroke()
        }

        function d(a, c, d, e, f, g, h, i) {
            var j = Math.cos(c * s),
                k = Math.sin(c * s);
            i -= h, b(a, d - k * f, e + j * g + .5 * i, h + (1 - .5 * j) * i)
        }

        function e(a, b, c, e, f, g, h, i) {
            var j;
            for (j = 5; j--;) d(a, b + j / 5, c, e, f, g, h, i)
        }

        function f(a, b, c, d, f, g, h) {
            b /= 3e4;
            var i = .21 * f,
                j = .12 * f,
                k = .24 * f,
                l = .28 * f;
            a.fillStyle = h, e(a, b, c, d, i, j, k, l), a.globalCompositeOperation = "destination-out", e(a, b, c, d, i, j, k - g, l - g), a.globalCompositeOperation = "source-over"
        }

        function g(a, b, d, e, f, g, h) {
            b /= 12e4;
            var i, j, k, l, m = .25 * f - .5 * g,
                n = .32 * f + .5 * g,
                o = .5 * f - .5 * g;
            for (a.strokeStyle = h, a.lineWidth = g, a.lineCap = "round", a.lineJoin = "round", a.beginPath(), a.arc(d, e, m, 0, s, !1), a.stroke(), i = 8; i--;) j = (b + i / 8) * s, k = Math.cos(j), l = Math.sin(j), c(a, d + k * n, e + l * n, d + k * o, e + l * o)
        }

        function h(a, b, c, d, e, f, g) {
            b /= 15e3;
            var h = .29 * e - .5 * f,
                i = .05 * e,
                j = Math.cos(b * s),
                k = j * s / -16;
            a.strokeStyle = g, a.lineWidth = f, a.lineCap = "round", a.lineJoin = "round", c += j * i, a.beginPath(), a.arc(c, d, h, k + s / 8, k + 7 * s / 8, !1), a.arc(c + Math.cos(k) * h * t, d + Math.sin(k) * h * t, h, k + 5 * s / 8, k + 3 * s / 8, !0), a.closePath(), a.stroke()
        }

        function i(a, b, c, d, e, f, g) {
            b /= 1350;
            var h, i, j, k, l = .16 * e,
                m = 11 * s / 12,
                n = 7 * s / 12;
            for (a.fillStyle = g, h = 4; h--;) i = (b + h / 4) % 1, j = c + (h - 1.5) / 1.5 * (1 === h || 2 === h ? -1 : 1) * l, k = d + i * i * e, a.beginPath(), a.moveTo(j, k - 1.5 * f), a.arc(j, k, .75 * f, m, n, !1), a.fill()
        }

        function j(a, b, d, e, f, g, h) {
            b /= 750;
            var i, j, k, l, m = .1875 * f;
            for (a.strokeStyle = h, a.lineWidth = .5 * g, a.lineCap = "round", a.lineJoin = "round", i = 4; i--;) j = (b + i / 4) % 1, k = Math.floor(d + (i - 1.5) / 1.5 * (1 === i || 2 === i ? -1 : 1) * m) + .5, l = e + j * f, c(a, k, l - 1.5 * g, k, l + 1.5 * g)
        }

        function k(a, b, d, e, f, g, h) {
            b /= 3e3;
            var i, j, k, l, m = .16 * f,
                n = .75 * g,
                o = b * s * .7,
                p = Math.cos(o) * n,
                q = Math.sin(o) * n,
                r = o + s / 3,
                t = Math.cos(r) * n,
                u = Math.sin(r) * n,
                v = o + 2 * s / 3,
                w = Math.cos(v) * n,
                x = Math.sin(v) * n;
            for (a.strokeStyle = h, a.lineWidth = .5 * g, a.lineCap = "round", a.lineJoin = "round", i = 4; i--;) j = (b + i / 4) % 1, k = d + Math.sin((j + i / 4) * s) * m, l = e + j * f, c(a, k - p, l - q, k + p, l + q), c(a, k - t, l - u, k + t, l + u), c(a, k - w, l - x, k + w, l + x)
        }

        function l(a, b, c, d, f, g, h) {
            b /= 3e4;
            var i = .21 * f,
                j = .06 * f,
                k = .21 * f,
                l = .28 * f;
            a.fillStyle = h, e(a, b, c, d, i, j, k, l), a.globalCompositeOperation = "destination-out", e(a, b, c, d, i, j, k - g, l - g), a.globalCompositeOperation = "source-over"
        }

        function m(a, b, c, d, e, f, g) {
            var h = e / 8,
                i = h / 3,
                j = 2 * i,
                k = b % 1 * s,
                l = Math.cos(k),
                m = Math.sin(k);
            a.fillStyle = g, a.strokeStyle = g, a.lineWidth = f, a.lineCap = "round", a.lineJoin = "round", a.beginPath(), a.arc(c, d, h, k, k + Math.PI, !1), a.arc(c - i * l, d - i * m, j, k + Math.PI, k, !1), a.arc(c + j * l, d + j * m, i, k + Math.PI, k, !0), a.globalCompositeOperation = "destination-out", a.fill(), a.globalCompositeOperation = "source-over", a.stroke()
        }

        function n(a, b, c, d, e, f, g, h, i) {
            b /= 2500;
            var j, k, l, n, o = u[g],
                p = (b + g - v[g].start) % h,
                q = (b + g - v[g].end) % h,
                r = (b + g) % h;
            if (a.strokeStyle = i, a.lineWidth = f, a.lineCap = "round", a.lineJoin = "round", 1 > p) {
                if (a.beginPath(), p *= o.length / 2 - 1, j = Math.floor(p), p -= j, j *= 2, j += 2, a.moveTo(c + (o[j - 2] * (1 - p) + o[j] * p) * e, d + (o[j - 1] * (1 - p) + o[j + 1] * p) * e), 1 > q) {
                    for (q *= o.length / 2 - 1, k = Math.floor(q), q -= k, k *= 2, k += 2, n = j; n !== k; n += 2) a.lineTo(c + o[n] * e, d + o[n + 1] * e);
                    a.lineTo(c + (o[k - 2] * (1 - q) + o[k] * q) * e, d + (o[k - 1] * (1 - q) + o[k + 1] * q) * e)
                } else
                    for (n = j; n !== o.length; n += 2) a.lineTo(c + o[n] * e, d + o[n + 1] * e);
                a.stroke()
            } else if (1 > q) {
                for (a.beginPath(), q *= o.length / 2 - 1, k = Math.floor(q), q -= k, k *= 2, k += 2, a.moveTo(c + o[0] * e, d + o[1] * e), n = 2; n !== k; n += 2) a.lineTo(c + o[n] * e, d + o[n + 1] * e);
                a.lineTo(c + (o[k - 2] * (1 - q) + o[k] * q) * e, d + (o[k - 1] * (1 - q) + o[k + 1] * q) * e), a.stroke()
            }
            1 > r && (r *= o.length / 2 - 1, l = Math.floor(r), r -= l, l *= 2, l += 2, m(a, b, c + (o[l - 2] * (1 - r) + o[l] * r) * e, d + (o[l - 1] * (1 - r) + o[l + 1] * r) * e, e, f, i))
        }
        var o, p;
        ! function () {
            var b = a.requestAnimationFrame || a.webkitRequestAnimationFrame || a.mozRequestAnimationFrame || a.oRequestAnimationFrame || a.msRequestAnimationFrame,
                c = a.cancelAnimationFrame || a.webkitCancelAnimationFrame || a.mozCancelAnimationFrame || a.oCancelAnimationFrame || a.msCancelAnimationFrame;
            b && c ? (o = function (a) {
                function c() {
                    d.value = b(c), a()
                }
                var d = {
                    value: null
                };
                return c(), d
            }, p = function (a) {
                c(a.value)
            }) : (o = setInterval, p = clearInterval)
        }();
        var q = 500,
            r = .08,
            s = 2 * Math.PI,
            t = 2 / Math.sqrt(2),
            u = [
                [-.75, -.18, -.7219, -.1527, -.6971, -.1225, -.6739, -.091, -.6516, -.0588, -.6298, -.0262, -.6083, .0065, -.5868, .0396, -.5643, .0731, -.5372, .1041, -.5033, .1259, -.4662, .1406, -.4275, .1493, -.3881, .153, -.3487, .1526, -.3095, .1488, -.2708, .1421, -.2319, .1342, -.1943, .1217, -.16, .1025, -.129, .0785, -.1012, .0509, -.0764, .0206, -.0547, -.012, -.0378, -.0472, -.0324, -.0857, -.0389, -.1241, -.0546, -.1599, -.0814, -.1876, -.1193, -.1964, -.1582, -.1935, -.1931, -.1769, -.2157, -.1453, -.229, -.1085, -.2327, -.0697, -.224, -.0317, -.2064, .0033, -.1853, .0362, -.1613, .0672, -.135, .0961, -.1051, .1213, -.0706, .1397, -.0332, .1512, .0053, .158, .0442, .1624, .0833, .1636, .1224, .1615, .1613, .1565, .1999, .15, .2378, .1402, .2749, .1279, .3118, .1147, .3487, .1015, .3858, .0892, .4236, .0787, .4621, .0715, .5012, .0702, .5398, .0766, .5768, .089, .6123, .1055, .6466, .1244, .6805, .144, .7147, .163, .75, .18],
                [-.75, 0, -.7033, .0195, -.6569, .0399, -.6104, .06, -.5634, .0789, -.5155, .0954, -.4667, .1089, -.4174, .1206, -.3676, .1299, -.3174, .1365, -.2669, .1398, -.2162, .1391, -.1658, .1347, -.1157, .1271, -.0661, .1169, -.017, .1046, .0316, .0903, .0791, .0728, .1259, .0534, .1723, .0331, .2188, .0129, .2656, -.0064, .3122, -.0263, .3586, -.0466, .4052, -.0665, .4525, -.0847, .5007, -.1002, .5497, -.113, .5991, -.124, .6491, -.1325, .6994, -.138, .75, -.14]
            ],
            v = [{
                start: .36,
                end: .11
            }, {
                start: .56,
                end: .16
            }],
            w = function (a) {
                this.list = [], this.interval = null, this.color = a && a.color ? a.color : "black", this.resizeClear = !(!a || !a.resizeClear)
            };
        w.CLEAR_DAY = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                f = Math.min(d, e);
            g(a, b, .5 * d, .5 * e, f, f * r, c)
        }, w.CLEAR_NIGHT = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                f = Math.min(d, e);
            h(a, b, .5 * d, .5 * e, f, f * r, c)
        }, w.PARTLY_CLOUDY_DAY = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                h = Math.min(d, e);
            g(a, b, .625 * d, .375 * e, .75 * h, h * r, c), f(a, b, .375 * d, .625 * e, .75 * h, h * r, c)
        }, w.PARTLY_CLOUDY_NIGHT = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                g = Math.min(d, e);
            h(a, b, .667 * d, .375 * e, .75 * g, g * r, c), f(a, b, .375 * d, .625 * e, .75 * g, g * r, c)
        }, w.CLOUDY = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                g = Math.min(d, e);
            f(a, b, .5 * d, .5 * e, g, g * r, c)
        }, w.RAIN = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                g = Math.min(d, e);
            i(a, b, .5 * d, .37 * e, .9 * g, g * r, c), f(a, b, .5 * d, .37 * e, .9 * g, g * r, c)
        }, w.SLEET = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                g = Math.min(d, e);
            j(a, b, .5 * d, .37 * e, .9 * g, g * r, c), f(a, b, .5 * d, .37 * e, .9 * g, g * r, c)
        }, w.SNOW = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                g = Math.min(d, e);
            k(a, b, .5 * d, .37 * e, .9 * g, g * r, c), f(a, b, .5 * d, .37 * e, .9 * g, g * r, c)
        }, w.WIND = function (a, b, c) {
            var d = a.canvas.width,
                e = a.canvas.height,
                f = Math.min(d, e);
            n(a, b, .5 * d, .5 * e, f, f * r, 0, 2, c), n(a, b, .5 * d, .5 * e, f, f * r, 1, 2, c)
        }, w.FOG = function (a, b, d) {
            var e = a.canvas.width,
                f = a.canvas.height,
                g = Math.min(e, f),
                h = g * r;
            l(a, b, .5 * e, .32 * f, .75 * g, h, d), b /= 5e3;
            var i = Math.cos(b * s) * g * .02,
                j = Math.cos((b + .25) * s) * g * .02,
                k = Math.cos((b + .5) * s) * g * .02,
                m = Math.cos((b + .75) * s) * g * .02,
                n = .936 * f,
                o = Math.floor(n - .5 * h) + .5,
                p = Math.floor(n - 2.5 * h) + .5;
            a.strokeStyle = d, a.lineWidth = h, a.lineCap = "round", a.lineJoin = "round", c(a, i + .2 * e + .5 * h, o, j + .8 * e - .5 * h, o), c(a, k + .2 * e + .5 * h, p, m + .8 * e - .5 * h, p)
        }, w.prototype = {
            add: function (a, b) {
                var c;
                "string" == typeof a && (a = document.getElementById(a)), null !== a && ("string" == typeof b && (b = b.toUpperCase().replace(/-/g, "_"), b = w.hasOwnProperty(b) ? w[b] : null), "function" == typeof b && (c = {
                    element: a,
                    context: a.getContext("2d"),
                    drawing: b
                }, this.list.push(c), this.draw(c, q)))
            },
            set: function (a, b) {
                var c;
                for ("string" == typeof a && (a = document.getElementById(a)), c = this.list.length; c--;)
                    if (this.list[c].element === a) return this.list[c].drawing = b, void this.draw(this.list[c], q);
                this.add(a, b)
            },
            remove: function (a) {
                var b;
                for ("string" == typeof a && (a = document.getElementById(a)), b = this.list.length; b--;)
                    if (this.list[b].element === a) return void this.list.splice(b, 1)
            },
            draw: function (a, b) {
                var c = a.context.canvas;
                this.resizeClear ? c.width = c.width : a.context.clearRect(0, 0, c.width, c.height), a.drawing(a.context, b, this.color)
            },
            play: function () {
                var a = this;
                this.pause(), this.interval = o(function () {
                    var b, c = Date.now();
                    for (b = a.list.length; b--;) a.draw(a.list[b], c)
                }, 1e3 / 60)
            },
            pause: function () {
                this.interval && (p(this.interval), this.interval = null)
            }
        }, a.Skycons = w
    }(this), $(function () {
        var a, b = new Skycons,
            c = ["clear-day", "clear-night", "partly-cloudy-day", "partly-cloudy-night", "cloudy", "rain", "sleet", "snow", "wind", "fog"];
        for (a = c.length; a--;) b.set(c[a], c[a]);
        b.add(document.getElementById("icon-cloud"), Skycons.PARTLY_CLOUDY_DAY);
        var d = new Skycons({
            color: "white"
        });
        d.add(document.getElementById("icon-clear-2"), Skycons.PARTLY_CLOUDY_NIGHT), b.play()
    }), $(document).ready(function () {
        $(".switch-button").click(function (a) {
            a.preventDefault();
            var b = $(this).attr("switch-parent"),
                c = $(this).attr("switch-target");
            $(b).slideToggle(), $(c).slideToggle()
        }), $(".hidden-button").hover(function () {
            $(".btn-hide", this).fadeIn("fast")
        }, function () {
            $(".btn-hide", this).fadeOut("normal")
        }), $(".toggle-button").click(function (a) {
            a.preventDefault(), $(".glyph-icon", this).toggleClass("icon-rotate-180"), $(this).parents(".content-box:first").find(".content-box-wrapper").slideToggle()
        }), $(".remove-button").click(function (a) {
            a.preventDefault();
            var b = $(this).attr("data-animation"),
                c = $(this).parents(".content-box:first");
            $(c).addClass("animated"), $(c).addClass(b);
            window.setTimeout(function () {
                $(c).slideUp()
            }, 500), window.setTimeout(function () {
                $(c).removeClass(b).fadeIn()
            }, 2500)
        }), $(function () {
            $(".infobox-close").click(function (a) {
                a.preventDefault(), $(this).parent().fadeOut()
            })
        })
    }), + function (a) {
        "use strict";

        function b(b) {
            return this.each(function () {
                var d = a(this),
                    e = d.data("bs.tab");
                e || d.data("bs.tab", e = new c(this)), "string" == typeof b && e[b]()
            })
        }
        var c = function (b) {
            this.element = a(b)
        };
        c.VERSION = "3.2.0", c.prototype.show = function () {
            var b = this.element,
                c = b.closest("ul:not(.dropdown-menu)"),
                d = b.data("target");
            if (d || (d = b.attr("href"), d = d && d.replace(/.*(?=#[^\s]*$)/, "")), !b.parent("li").hasClass("active")) {
                var e = c.find(".active:last a")[0],
                    f = a.Event("show.bs.tab", {
                        relatedTarget: e
                    });
                if (b.trigger(f), !f.isDefaultPrevented()) {
                    var g = a(d);
                    this.activate(b.closest("li"), c), this.activate(g, g.parent(), function () {
                        b.trigger({
                            type: "shown.bs.tab",
                            relatedTarget: e
                        })
                    })
                }
            }
        }, c.prototype.activate = function (b, c, d) {
            function e() {
                f.removeClass("active").find("> .dropdown-menu > .active").removeClass("active"), b.addClass("active"), g ? (b[0].offsetWidth, b.addClass("in")) : b.removeClass("fade"), b.parent(".dropdown-menu") && b.closest("li.dropdown").addClass("active"), d && d()
            }
            var f = c.find("> .active"),
                g = d && a.support.transition && (f.length && f.hasClass("fade") || !!c.find("> .fade").length);
            f.length && g ? f.one("bsTransitionEnd", e).emulateTransitionEnd(150) : e(), f.removeClass("in")
        };
        var d = a.fn.tab;
        a.fn.tab = b, a.fn.tab.Constructor = c, a.fn.tab.noConflict = function () {
            return a.fn.tab = d, this
        }, a(document).on("click.bs.tab.data-api", '[data-toggle="tab"], [data-toggle="pill"]', function (c) {
            c.preventDefault(), b.call(a(this), "show")
        })
    }(jQuery),
    function (a) {
        a.slidebars = function (b) {
            function c() {
                !j.disableOver || "number" == typeof j.disableOver && j.disableOver >= w ? (v = !0, a("html").addClass("sb-init"), j.hideControlClasses && x.removeClass("sb-hide"), d()) : "number" == typeof j.disableOver && j.disableOver < w && (v = !1, a("html").removeClass("sb-init"), j.hideControlClasses && x.addClass("sb-hide"), q.css("minHeight", ""), (s || u) && g())
            }

            function d() {
                q.css("minHeight", ""), q.css("minHeight", a("html").height() + "px"), r && r.hasClass("sb-width-custom") && r.css("width", r.attr("data-sb-width")), t && t.hasClass("sb-width-custom") && t.css("width", t.attr("data-sb-width")), r && (r.hasClass("sb-style-push") || r.hasClass("sb-style-overlay")) && r.css("marginLeft", "-" + r.css("width")), t && (t.hasClass("sb-style-push") || t.hasClass("sb-style-overlay")) && t.css("marginRight", "-" + t.css("width")), j.scrollLock && a("html").addClass("sb-scroll-lock")
            }

            function e(a, b, c) {
                var e;
                if (e = a.hasClass("sb-style-push") ? q.add(a).add(y) : a.hasClass("sb-style-overlay") ? a : q.add(y), "translate" === z) e.css("transform", "translate(" + b + ")");
                else if ("side" === z) "-" === b[0] && (b = b.substr(1)), "0px" !== b && e.css(c, "0px"), setTimeout(function () {
                    e.css(c, b)
                }, 1);
                else if ("jQuery" === z) {
                    "-" === b[0] && (b = b.substr(1));
                    var f = {};
                    f[c] = b, e.stop().animate(f, 400)
                }
                setTimeout(function () {
                    "0px" === b && (e.removeAttr("style"), d())
                }, 400)
            }

            function f(b, c) {
                function d() {
                    v && "left" === b && r ? (a("html").addClass("sb-active sb-active-left"), r.addClass("sb-active"), e(r, r.css("width"), "left"), setTimeout(function () {
                        s = !0, "function" == typeof c && c()
                    }, 400)) : v && "right" === b && t && (a("html").addClass("sb-active sb-active-right"), t.addClass("sb-active"), e(t, "-" + t.css("width"), "right"), setTimeout(function () {
                        u = !0, "function" == typeof c && c()
                    }, 400))
                }
                "left" === b && r && u || "right" === b && t && s ? (g(), setTimeout(d, 400)) : d()
            }

            function g(b) {
                (s || u) && (s && (e(r, "0px", "left"), s = !1), u && (e(t, "0px", "right"), u = !1), setTimeout(function () {
                    a("html").removeClass("sb-active sb-active-left sb-active-right"), r && r.removeClass("sb-active"), t && t.removeClass("sb-active"), "function" == typeof b && b()
                }, 400))
            }

            function h(a, b) {
                "left" === a && r && (s ? g(null, b) : f("left", b)), "right" === a && t && (u ? g(null, b) : f("right", b))
            }

            function i(a, b) {
                a.stopPropagation(), a.preventDefault(), "touchend" === a.type && b.off("click")
            }
            var j = a.extend({
                siteClose: !0,
                scrollLock: !1,
                disableOver: !1,
                hideControlClasses: !1
            }, b),
                k = document.createElement("div").style,
                l = !1,
                m = !1;
            ("" === k.MozTransition || "" === k.WebkitTransition || "" === k.OTransition || "" === k.transition) && (l = !0), ("" === k.MozTransform || "" === k.WebkitTransform || "" === k.OTransform || "" === k.transform) && (m = !0);
            var n = navigator.userAgent,
                o = !1,
                p = !1;
            /Android/.test(n) ? o = n.substr(n.indexOf("Android") + 8, 3) : /(iPhone|iPod|iPad)/.test(n) && (p = n.substr(n.indexOf("OS ") + 3, 3).replace("_", ".")), (o && 3 > o || p && 5 > p) && a("html").addClass("sb-static");
            var q = a("#sb-site, .sb-site-container");
            if (a(".sb-left").length) var r = a(".sb-left"),
                s = !1;
            if (a(".sb-right").length) var t = a(".sb-right"),
                u = !1;
            var v = !1,
                w = a(window).width(),
                x = a(".sb-toggle-left, .sb-toggle-right, .sb-open-left, .sb-open-right, .sb-close"),
                y = a(".sb-slide");
            c(), a(window).resize(function () {
                var b = a(window).width();
                w !== b && (w = b, c(), s && f("left"), u && f("right"))
            });
            var z;
            l && m ? (z = "translate", o && 4.4 > o && (z = "side")) : z = "jQuery", this.slidebars = {
                open: f,
                close: g,
                toggle: h,
                init: function () {
                    return v
                },
                reInit: c,
                resetCSS: d,
                active: function (a) {
                    return "left" === a && r ? s : "right" === a && t ? u : void 0
                },
                destroy: function (a) {
                    "left" === a && r && (s && g(), setTimeout(function () {
                        r.remove(), r = !1
                    }, 400)), "right" === a && t && (u && g(), setTimeout(function () {
                        t.remove(), t = !1
                    }, 400))
                }
            }, a(".sb-toggle-left").on("touchend click", function (b) {
                i(b, a(this)), h("left")
            }), a(".sb-toggle-right").on("touchend click", function (b) {
                i(b, a(this)), h("right")
            }), a(".sb-open-left").on("touchend click", function (b) {
                i(b, a(this)), f("left")
            }), a(".sb-open-right").on("touchend click", function (b) {
                i(b, a(this)), f("right")
            }), a(".sb-close").on("touchend click", function (b) {
                if (a(this).is("a") || a(this).children().is("a")) {
                    if ("click" === b.type) {
                        b.preventDefault();
                        var c = a(this).is("a") ? a(this).attr("href") : a(this).find("a").attr("href");
                        g(function () {
                            window.location = c
                        })
                    }
                } else i(b, a(this)), g()
            }), q.on("touchend click", function (b) {
                j.siteClose && (s || u) && (i(b, a(this)), g())
            })
        }
    }(jQuery), $(function () {
        $('a[href="#"]').click(function (a) {
            a.preventDefault()
        })
    }), $(function () {
        $(".todo-box li input").on("click", function () {
            $(this).parent().toggleClass("todo-done")
        })
    }), $(function () {
        var a = 0;
        $(".timeline-scroll .tl-row").each(function (b, c) {
            var d = $(c);
            a += d.outerWidth() + parseInt(d.css("margin-left"), 10) + parseInt(d.css("margin-right"), 10)
        }), $(".timeline-horizontal", this).width(a)
    }), $(document).on("ready", function () {
        $(".scrollable-nice").niceScroll({
            horizrailenabled: !1,
            cursorborder: "0",
            cursorwidth: "6px",
            cursorcolor: "#363636",
            zindex: "5555",
            autohidemode: !0,
            bouncescroll: !0,
            mousescrollstep: "40",
            scrollspeed: "100",
            background: "#cdcdcd",
            cursoropacitymax: "0.6",
            cursorborderradius: "0"
        }), $(".scrollable-nice").getNiceScroll().resize()
    }), $(function () {
        $(".loading-button").click(function () {
            var a = $(this);
            a.button("loading")
        })
    }), $(function () {
        $('input[type="checkbox"].custom-checkbox').uniform(), $('input[type="radio"].custom-radio').uniform(), $(".custom-select").uniform(), $(".selector").append('<i class="glyph-icon icon-caret-down"></i>'), $(".checker span").append('<i class="glyph-icon icon-check"></i>'), $(".radio span").append('<i class="glyph-icon icon-circle"></i>')
    }), $(function () {
        $(".chosen-select").chosen(), $(".chosen-search").append('<i class="glyph-icon icon-search"></i>'), $(".chosen-single div").html('<i class="glyph-icon icon-caret-down"></i>')
    }), $(function () {
        var a = ["ActionScript", "AppleScript", "Asp", "BASIC", "C", "C++", "Clojure", "COBOL", "ColdFusion", "Erlang", "Fortran", "Groovy", "Haskell", "Java", "JavaScript", "Lisp", "Perl", "PHP", "Python", "Ruby", "Scala", "Scheme"];
        $(".autocomplete-input").autocomplete({
            source: a
        })
    }), $(function () {
        $(".thumbnail-box").hover(function () {
            $(".thumbnail-overlay", this).fadeIn("fast"), $(".thumbnail-content", this).slideDown("fast")
        }, function () {
            $(".thumbnail-overlay", this).fadeOut("fast"), $(".thumbnail-content", this).slideUp("fast")
        })
    }), $(function () {
        $(".tooltip-button").tooltip({
            container: "body"
        })
    }), $(function () {
        $(".alert-close-btn").click(function () {
            $(this).parent().addClass("animated fadeOutDown")
        })
    }), $(function () {
        $(".popover-button").popover({
            container: "body",
            html: !0,
            animation: !0,
            content: function () {
                var a = $(this).attr("data-id");
                return $(a).html()
            }
        }).click(function (a) {
            a.preventDefault()
        })
    }), $(function () {
        $(".popover-button-default").popover({
            container: "body",
            html: !0,
            animation: !0
        }).click(function (a) {
            a.preventDefault()
        })
    }),
    function (a) {
        a(document).ready(function () {
            a.slidebars()
        })
    }(jQuery), $(document).ready(function () {
        $(".choose-theme").click(function () {
            var a = $(this).attr("layout-color"),
                b = $(this).attr("framework-color");
            $("#loading").slideDown({
                complete: function () {
                    if ("" != a) {
                        $("#layout-color").attr("href", "/Resc/Skin/Supina/assets-minified/themes/supina/" + a + "/layout-color.css"); {
                            $.cookie("set-layout-theme", a)
                        }
                    }
                    if ("" != b) {
                        $("#framework-color").attr("href", "/Resc/Skin/Supina/assets-minified/themes/supina/" + b + "/framework-color.css"); {
                            $.cookie("set-framework-theme", b)
                        }
                    }
                }
            }), $("#loading").delay(1500).slideUp()
        }), themefromCookie()
    }), $(window).resize(function () {
        layoutFormatter()
    }),
/* // para box de busca nao redimensionar
$(function () { $("#sidebar-search input").focus(function () { $(this).stop().animate({ width: 200 }, "slow") }).blur(function () { $(this).stop().animate({ width: 100 }, "slow") }) }),
*/
    $(document).on("ready", function () {
        $("#page-sidebar-wrapper").niceScroll({
            horizrailenabled: !1,
            cursorborder: "0",
            cursorwidth: "6px",
            cursorcolor: "#000000",
            zindex: "5555",
            autohidemode: !0,
            bouncescroll: !0,
            mousescrollstep: "40",
            scrollspeed: "100",
            background: "#2A2A2A",
            cursoropacitymax: "0.6",
            cursorborderradius: "0"
        }), $("#page-sidebar-wrapper").getNiceScroll().resize()
    }), $(document).ready(function () {
        layoutFormatter(), $(function () {
            $("#responsive-open-menu").click(function () {
                $("#page-sidebar").toggle()
            })
        }), $(function () {
            $("#sidebar-menu > ul").superclick({
                animation: {
                    height: "show"
                },
                animationOut: {
                    height: "hide"
                }
            })
        }), $(function () {
            $(".main-nav > ul").superclick({
                animation: {
                    height: "show"
                },
                animationOut: {
                    height: "hide"
                }
            })
        }), $(function () {
            $("#collapse-sidebar").click(function () {
                $("#page-sidebar, #page-content-wrapper, #header-logo").removeClass("rm-transition"),
                $("body").toggleClass("sidebar-collapsed"),
                //$(".glyph-icon", this).toggleClass("icon-chevron-right").toggleClass("icon-chevron-left");
                $(this).toggleClass("icon-chevron-right").toggleClass("icon-chevron-left");

                if ($(".carrossel").length > 0) {
                    setTimeout(function () {
                        carrossel.onResize();
                    }, 500);
                }

            })
        })
    }), $(document).ready(function () {
        init_page_transitions()
    }); 