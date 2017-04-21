
function qrmgobj(model) {
    var _self = this;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._cb = model.callback ? model.callback : null;
    _self._model = model;

    _self._init = function () {
    }
    _self._initctx = function () {
        var _ctx = $('div[id="_questctx"]');
        if (_ctx.length != 1) {
            _self._ctx = {};
            _self._mm = {};
            _self._nb = {};
            return;
        }
        _self._ctx = _ctx;
        _self._initmm();
        _self._initnb();
    }
    _self._initquestctx = function () {
        var _ctxee = $('.qrmgctx');
        if (_ctx.length < 1) { return; }
    }
    _self._initmm = function () {
        var _mm = $('#_mm', _self._ctx);
        if (_mm.length != 1) {
            _self._mm = {};
            return;
        }
    }
    _self._initnb = function () {
        var _nb = $('#_nb', _self._ctx);
        if (_nb.length != 1) {
            _self._nb = {};
            return;
        }
    }



    init();
}

