// qrmgNavbar.js
function qrmgNavbar(model) {
    var _self = this;
    _self._model = model;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._callback = model.callback;
    _self._model.options = _self._model.options ? _self._model.options : [];
    _self._oi = 0;
    _self._bX = false; // Temporary

    _self._init = function () {
        _self._initctx()
        _self._initops();
        _self._render();
        _self._initpin();
        _self._bind();
        _self._autoexpand(_self._model.options);
        _self._ldopts();
    }
    _self._initctx = function() {
        _self._ctx = new qrmgctx(_self._model.ctx);
        var ctx = _self._getctx();
        if (_self._model.expanded) {
            $('#navbarMainMenu').width(180);
        }
    }
    
    _self._initops = function () {
        $.each(_self._model.options, function (i, o) {
            _self._initop(o);
        });
    }
    _self._initop = function (o, p) {
        o._i = ++_self._oi;
        o._p = p;
        o._id = _self._initoid(o);
        o.label = o.label ? o.label : o.name;
        o.NewTabOption = o.NewTabOption || _self._model.NewTabOption;
        if (!o.uri) {
            o.uri = _self._bldopth(o);
        }
        if (o.options) {
            $.each(o.options, function (i, so) {
                _self._initop(so, o);
            });
        }
    }
    _self._initoid = function (o) {
        var _r = _self._pfx;
        var _pp = [];
        for (var _p = o._p; _p; _p = _p._p) {
            _pp.push(_p.name);
        }
        o._id = _r + _pp.reverse().join('_') + o.name;
        return (o._id);
    }
    _self._bldopth = function (o) {
        var _path = _self._uri;
        var _pp = [];
        for (var _p = o._p; _p; _p = _p._p) {
            _pp.push(_p.uri ? _p.uri : _p.name);
        }
        _path += _pp.reverse().join('/') + (_pp.length ? '/' : '') + o.name;
        return (_path.replace(/\/\/+/g, '/'));
    }

    _self._render = function () {
        var _h = [], _i = 0;
        if (_self._bX) {
            _h[_i++] = '<div class="questNavbarHeader"><span class="questNavbarTack fa fa-thumb-tack " title="Pin navbar open"></span></div>';
            _h[_i++] = '<div class="questNavbarFrame">';
        }
        else {
            _h[_i++] = '<div class="questNavbarHeader"><span class="questNavbarTack fa fa-reorder" title="Pin navbar open"></span></div>';
            _h[_i++] = '<div class="questNavbarFrame" style="display: none;">';
        }
        _h[_i++] = '<ul class="">';
        $.each(_self._model.options, function (i, o) {
            _h[_i++] = _self._rndro(_self._model, o);
        });
        _h[_i++] = '</ul>';
        _h[_i++] = '</div>';
        $(_self._e).empty().append(_h.join('')).addClass('qrmgNavbar');
        _self._rndrco();
    }
    _self._rndro = function (p, o) {
        var _h = [], _i = 0;
        _h[_i++] = '<li id="' + o._id + '" class="nav-item ' + (o.default ? ' start active ' : '') + '">';
        if (p.scrollSpy) {
            _h[_i++] = '    <a href="#section' + (o.section ? o.section : o.name) + '" class="nav-link nav-toggle">';
        }
        else if (o.method) {
            _h[_i++] = '    <a href="javascript:void(0);" class="nav-link nav-toggle method-' + o.method + '">';
        }
        else if (o.autoscroll) {
            _h[_i++] = '    <a href="/' + o.uri + '" class="nav-link nav-toggle navbar-autoscroll">';
        }
        else {
            _h[_i++] = '    <a href="/' + o.uri + '" class="nav-link nav-toggle">';
        }
        if (o.icon) {
            _h[_i++] = '        <i class="fa ' + o.icon + '"></i>';
        }
        _h[_i++] = '        <span class="title">' + o.label + '</span>';
        if (o.default) {
            _h[_i++] = '<span class="selected"></span>';
        }
        if (o.options) {
            _h[_i++] = '        <span class="fa fa-chevron-left xpander"></span>';
        }
        _h[_i++] = '    </a>';
        if (o.options) {
            _h[_i++] = _self._rndrsubm(o);
        }
        _h[_i++] = '</li>';
        return (_h.join(''));
    }
    _self._rndrsubm = function (o) {
        var _h = [], _i = 0;
        _h[_i++] = '<ul id="' + o._id + 'Options" class="sub-menu" style="display: ' + (o.autoExpand ? 'block' : 'none') + ';">';
        _h[_i++] = _self._rndrsoo(o);
        _h[_i++] = '</ul>';
        return (_h.join(''));
    }
    _self._inssubm = function (o) {
        var _h = [], _i = 0;
        _h[_i++] = _self._rndrsoo(o);
        $('ul[Id="' + o._id + 'Options"]', _self._e).empty().append(_h.join(''));
    }
    _self._rndrsoo = function (o) {
        var _h = [], _i = 0;
        $.each(o.options, function (i, _o) {
            _h[_i++] = _self._rndrso(o, _o);
        });
        return (_h.join(''));
    }
    _self._rndrso = function (p, o) {
        var _h = [], _i = 0;
        _h[_i++] = '<li id="' + o._id + '" class="nav-item">';
        if (p.scrollSpy) {
            _h[_i++] = '    <a href="#section' + (o.section ? o.section : o.name) + '" class="nav-link">';
        }
        else if (o.autoscroll) {
            _h[_i++] = '    <a href="/' + o.uri + '" class="nav-link nav-toggle navbar-autoscroll">';
        }
        else if (o.method) {
            _h[_i++] = '    <a href="javascript:void(0);" class="nav-link nav-toggle method-' + o.method + '">';
        }
        else {
            _h[_i++] = '    <a href="' + o.uri + '" class="nav-link">';
        }
        _h[_i++] = '        <span class="title">' + o.label + '</span>';
        _h[_i++] = '    </a>';
        _h[_i++] = '</li>';
        return (_h.join(''));
    }
    _self._rndrco = function () {
        var ctx = _self._getctx();
        var e;
        if (ctx['_nbI'] && $('input[Id="_nbI"]').val().length) {
            e = $('#' + $('input[Id="_nbI"]').val() + ' a:first');
            $(e).addClass('currentNavbarItem');
        }
        _self._xpndco();
    }
    _self._xpndco = function () {
        var e = $('.questNavbarFrame').find('.currentNavbarItem');
        if (e.length) {
            _self._xpnde(e)
        }
    }
    _self._getoe = function (o) {

    }

    _self._bind = function () {
        $(_self._e).find('.xpander').unbind('click').on('click', null, null, function (e) {
            _self._xpndclk(this, e);
        });
        $(_self._e).find('a.nav-link').unbind('dblclick').on('dblclick', null, null, function (e) {
            _self._xpndclk(this, e);
        });
        _self._bindopts();
    }

    _self._xpndclk = function (x, e) {
        if (_self._xmux) { return; }
        _self._xmux = true;
        if (!e) { return; }
        e.preventDefault();
        e.stopImmediatePropagation();
        var _sm = $(x).closest('li').find('ul.sub-menu');
        if (_sm) {
            $(_sm).hasClass('expanded') ? _self._clpse(x) : _self._xpnde(x);
        }
        _self._xmux = false;
    }
    _self._xpnde = function (e) {
        var _sm;
        var _x;
        if ($(e).hasClass('xpander')) {
            _sm = $(e).closest('li.nav-item').find('ul.sub-menu');
            _x = e;
        }
        else {
            _sm = $(e).closest('ul.sub-menu');
            _x = $(_sm).parent().find('.xpander');
        }
        if (_sm.length) {
            $(_sm).addClass('expanded');
            $(_sm).css('display', 'block');
            $(_x).addClass('open fa-chevron-down').removeClass('fa-chevron-left');
        }
        return (_sm);
    }
    _self._clpse = function (e) {
        var _sm;
        var _x;
        if ($(e).hasClass('xpander')) {
            _sm = $(e).closest('li.nav-item').find('ul.sub-menu');
            _x = e;
        }
        else {
            _sm = $(e).closest('ul.sub-menu');
            _x = $(_sm).parent().find('.xpander');
        }
        if (_sm) {
            $(_sm).hide();
            $(_sm).removeClass('expanded');
            $(_x).addClass('open fa-chevron-left').removeClass('fa-chevron-down');
        }
        return (_sm);
    }
    _self._bindopts = function () {
        $.each(_self._model.options, function (i, o) {
            _self._bindo(o, _self._model);
        });
    }
    _self._bindo = function (o, p) {
        $('#' + o._id).find('a').unbind('click').on('click', null, o, function (e) {
            e.preventDefault();
            e.stopPropagation();
            _self._setcur(o, e);
            if (o.autoscroll) {
                _self.ScrollTo(o);
                return;
            }
            qrmgmvc.Global.Mask(_self._model.maskFrame);
            var _url = o.uri;
            var _d = _self._getctx();
            if (o.type) {
                if (o.type == 'window') {
                    _url = _self._bldurl(o);
                    window.open(_url);
                }
                else if (o.type == 'action') {
                    var _tmo = o.timeout == undefined ? null : o.timeout;
                    var _io = new qrmgio(_self._ropex, o, _tmo);
                    _io.GetJSON(_url, _d);
                }
            }
            else {
                var _io = new qrmgio(_self._ropex, o);
                if (o.NewTabOption && e.shiftKey) {
                    _io.OpenView(_url, _d);
                    qrmgmvc.Global.Unmask(_self._model.maskFrame);
                }
                else {
                    _io.ShowView(_url, _d);
                }
            }
        });
        if (o.options) {
            $.each(o.options, function (i, _o) {
                _self._bindo(_o, o);
            });
        }
    }
    _self._autoexpand = function (opts) {
        $.each(opts, function (i, o) {
            if (o.autoExpand) {
                _self._xpnde($('li[id="' + _self._pfx + o.name + '"').find('.xpander'));
            }
            if (o.options) {
                _self._autoexpand(o.options);
            }
        });
    }
    _self._ropex = function (ud, d) {
        if (IsUserMessage(d)) {
            var _st = GetUMessageId(d);
            if (_st == 404) {
                DisplayUserMessage('E|' + ud.label + ' URL not found');
            }
            else {
                DisplayUserMessage(d);
            }
        }
        qrmgmvc.Global.Unmask(_self._model.maskFrame);
    }
    _self._setcur = function (o, e) {
        _self._clrcur();
        if (!$(e.currentTarget).hasClass('nav-toggle')) {
            $(e.currentTarget).closest('li').find('a').addClass('currentNavbarItem');
        }
        o.bCurrent = true;
        _self._ctx.Context('_nbI', o._id);
        _self._xpndco(e);
    }
    _self._clrcur = function () {
        var _o = _self._getcur();
        if (_o) {
            _o.bCurrent = false;
        }
        $('.questNavbarFrame').find('.currentNavbarItem').removeClass('currentNavbarItem');
        return (_o);
    }
    _self._getcur = function (oo) {
        var _o = null;
        var _oo = oo ? oo : _self._model.options;
        $.each(_oo, function (i, o) {
            if (o.bCurrent) { _o = o; return (false); }
            if (o.options) {
                _o = _self._getcur(o.options);
                if (_o) { return (false); }
            }
        });
        return (_o);
    }

    _self._ldopts = function () {
        $.each(_self._model.options, function (i, o) {
            if (o.dynamicOptions) {
                _self._lddo(o);
            }
        });
    }
    _self._lddo = function (o) {
        var _io = new qrmgio(_self._rldopts, o);
        var _d = _self._getctx();
        var _uri = o.dynamicOptions.uri;
        _io._ajax('get', 'json', _uri, _d);
    }
    _self._rldopts = function (ud, dd) {
        if (!IsAppSuccess(dd)) {
            DisplayUserMessage(dd);
            return;
        }
        _self._lddopts(ud, dd);
        _self._bnddyoo(ud, dd);
    }
    _self._lddopts = function (o, dd) {
        var oo = [];
        $.each(dd.Items, function (i, d) {
            var _o = { name: o.name, label: d.Name };
            _o.data = d;
            _o._id = _self._pfx + _o.name + '_' + _o.data.Id
            o.dynamicOptions.method ? _o.method = o.dynamicOptions.method : false;
            oo.push(_o);
        });
        o.options = oo;
        _self._inssubm(o);
        _self._rndrco();
    }
    _self._bnddyoo = function (o, dd) {
        var _gg = $("[class*='method-get']", '#' + o._id);
        $.each(o.options, function (i, _o) {
            $('#' + _self._pfx + _o.name + '_' + _o.data.Id).on('click', null, _o, function (e) {
                qrmgmvc.Global.Mask(_self._model.maskFrame);
                e.stopPropagation();
                e.preventDefault();
                _self._setcur(_o, e);
                if (o.dynamicOptions.click) {
                    if ($.isFunction(o.dynamicOptions.click)) {
                        o.dynamicOptions.click(e, _o);
                    }
                    else {
                        var _io = new qrmgio(_self._rdynoclk, _o);
                        var _d = _self._getctx();
                        if (_o.data) { $.extend(_d, _o.data); }
                        var _uri = o.dynamicOptions.click;
                        _io.ShowView(_uri, _d);
                    }
                }
            });
        });
    }
    _self._rdynoclk = function (ud, d) {
        if (IsUserMessage(d)) {
            DisplayUserMessage(d);
            return;
        }
    }
    _self._bldurl = function (o) {
        var _url = o.uri;
        var _ctx = _self._getctx();
        if (_ctx) {
            _url += '?' + $.param(_ctx);
        }
        return (_url);
    }

    _self._getctx = function () {
        var _ctx = _self._ctx.Context();
        return (_ctx);
    }

    _self._hXp;
    _self._initpin = function () {
        var _hX, _hC;
        $('.questNavbarTack').click(function (e) {
            if ($(this).hasClass('fa-thumb-tack')) {
                $('.questNavbarTack').toggleClass('fa-rotate-90');
                $(this).hasClass('fa-rotate-90') ? $('input[Id="_nbX"]').val('false') : $('input[Id="_nbX"]').val('true');
            }
        });
        $('#navbarMainMenu').hover(function (e) {
            if (_hX) { return; }
            _hX = 1;
            _self.Expand();
            setTimeout(function () {
                _hX = 0;
            }, 1000);
            _self._hXp = 1;
        }, function (e) {
            if (_hC || _self._hXp) { return; }
            _hC = 1;
            _self.Collapse();
            setTimeout(function () {
                _hC = 0;
            }, 1000);
        });
        if ($('input[id="_nbX"]').val() == 'true') { _self.Expand(true, 0); }
    }
    _self.Collapse = function(t) {
        if (!$('.questNavbarTack').hasClass('fa-rotate-90')) { return; }
        $('#navbarMainMenu').animate({ width: '30px' }, 300, function () {
            $('.questNavbarTack').removeClass('fa-thumb-tack fa-rotate-90').addClass('fa-reorder');
            $('.umsg').animate({ left: '31px' }, (t ? t : 250));
            $('.avopsacts').animate({ left: '31px' }, (t ? t : 250));
            $('.questNavbarFrame').animate({ opacity: 0 }, 100, function () {
                $('.questNavbarFrame').hide();
            });
            $('#pageContentFrame').animate({ 'padding-left': '50px' }, (t ? t : 250));
        });
    }
    _self.Expand = function(bPin, t) {
        if (!$('.questNavbarTack').hasClass('fa-reorder')) { if (!$('.questNavbarTack').hasClass('fa-rotate-90')) { return; } }
        $('.questNavbarTack').addClass('fa-thumb-tack fa-rotate-90').removeClass('fa-reorder');
        $('.umsg').animate({ left: '181px' }, (t ? t : 250));
        $('.avopsacts').animate({ left: '181px' }, (t ? t : 250));
        $('#pageContentFrame').animate({ 'padding-left': '200px' }, (t ? t : 250));
        $('#navbarMainMenu').animate({ width: '180px' }, (t ? t : 300), function () {
            if (bPin) {
                $('.questNavbarTack').removeClass('fa-rotate-90');
            }
            $('.questNavbarFrame').animate({ opacity: 1.0 }, 100, function () { $('.questNavbarFrame').show(); });           
        });
        setTimeout(function () {
            _self._hXp = 0;
        }, 500);
    }

    _self.ScrollTo = function (o, t, d) {
        if (o.autoscroll) {
            if ($(o.autoscroll.element).length) {
                $('html, body').animate({ scrollTop: $(o.autoscroll.element).offset().top - (o.autoscroll.offset ? o.autoscroll.offset : 0) }, (o.autoscroll.delay ? o.autoscroll.delay : 0));
            }
        }
        else if ($(o).length) {
            $('html, body').animate({ scrollTop: $(o).offset().top - (t ? t : 0) }, (d ? d : 0));
        }
    }


    _self._init();
}

