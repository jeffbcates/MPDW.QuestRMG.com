// qrmgMainMenu.js
function qrmgMainMenu(model) {
    var _self = this;
    _self._model = model;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._pfx = model.prefix ? model.prefix : '';

    _self._init = function () {
        _self._initctx();
        _self._render();
        _self._bind();
    }
    _self._initctx = function() {
        _self._ctx = new qrmgctx(_self._model.ctx);
        var ctx = _self._getctx();
    }

    _self._render = function () {
        var _h = [], _i = 0;
        var _flt = _self._model.float ? _self._model.float : '';
        _h[_i++] = '<ul id="mainMenu" class="nav navbar-nav navbar-' + _flt + '">';
        $.each(_self._model.options, function (i, o) {
            _h[_i++] = _self._rndro(_self._model, o);
        });
        _h[_i++] = '</ul>';
        $(_self._e).append(_h.join('')).removeClass().addClass('collapse navbar-collapse');
    }
    _self._rndro = function (p, o) {
        o._id = _self._pfx + o.Name;
        o.Label = o.Label ? o.Label : o.Name;
        var _h = [], _i = 0;
        _h[_i++] = '<li>';
        _h[_i++] = '<a id="' + o._id + '" href="' + o.Uri + '" class="page-scroll">' + o.Label + '</a>';

        _h[_i++] = '</li>';
        return (_h.join(''));
    }

    _self._bind = function () {
        $('a', _self._e).on('click', null, null, function (e) {
            e.preventDefault();
            e.stopPropagation();

            var _o = _self._getopt(this.id);
            var _d = _self._getctx();
            var _url = _o.Uri;
            var _io = new qrmgio(_self._ropt, _o);
            if (_o.MenuOptionType == 4) {
                _io.PostView(_url, _d, _o);
            }
            if (!_o.method) {
                _io.ShowView(_url, _d);
            }
        });
    }
    _self._getopt = function (id) {
        var _o;
        $.each(_self._model.options, function (i, o) {
            if (id == o._id) {
                _o = o;
                return (false);
            }
        });
        return (_o)
    }
    _self._ropt = function (ud, d) {
        DisplayUserMessage(d);
    }

    _self._getctx = function () {
        var _ctx = _self._ctx.Context();
        return (_ctx);
    }

    _self._init();
}

