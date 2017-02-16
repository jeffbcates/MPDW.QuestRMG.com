function qrmgctx(model) {
    var _self = this;
    _self._model = model;
    _self._e;
    _self._model.objects;
    _self._model.elements;

    _self._init = function () {
        if (_self._model.element) {
            if ($.isPlainObject(_self._model.element)) {
                _self._e = _self._model.element;
            }
            else {
                _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
            }
        }
        _self._model.objects = _self._model.objects ? _self._model.objects : [];
        _self._model.elements = _self._model.elements ? _self._model.elements : [];
    }
    _self.Context = function (n, v) {
        var ctx = _self._getctx();
        if (n) {
            if (v) {
                $('#' + n).val(v);
                return ($('#' + n).val(v));
            }
            else {
                return (_self._getctxv(n));
            }
        }
        else {
            return (ctx);
        }
    }
    _self._getctx = function () {
        var _ctx = {};
        var _ee = {};
        if (_self._e) {
            if ($.isPlainObject(_self._model.element)) {
                _ctx[_self._model.element.value] = $('#'+_self._model.element.name).val();
            }
            else {
                _ee = $('input', _self._e);
                $.each(_ee, function (i, e) {
                    _ctx[$(e).attr('id')] = $(e).val();
                });
            }
        }
        $.each(_self._model.objects, function (i, o) {
            if (o) {
                var _c = o.Context();
                $.extend(_ctx, _c);
            }
        });
        $.each(_self._model.elements, function (i, e) {
            if (e) {
                if ($.isPlainObject(e)) {
                    if (e.id) {
                        var _c = _self._getectx(e.id);
                        if (e.name) {
                            _ctx[e.name] = _c[e.id];
                        }
                        else {
                            $.extend(_ctx, _c);
                        }
                    }
                }
                else {
                    var _c = _self._getectx(e);
                    $.extend(_ctx, _c);
                }
            }
        });
        return (_ctx);
    }
    _self._getctxv = function (n) {
        var _v;
        var ctx = _self._getctx();
        for (var f in ctx) {
            if (f === n) {
                _v = ctx[f];
                return (false);
            }
        }
        return (_v);
    }
    _self._getectx = function (n) {
        var _ectx = {};
        if ($('#' + n).is('input')) {
            _ectx[$('#' + n).attr('id')] = $('#' + n).val();
        }
        else {
            var _ee = $('input', '#' + n);
            $.each(_ee, function (i, e) {
                _ectx[$(e).attr('id')] = $(e).val();
            });
        }
        return (_ectx);
    }

    _self._init();
}

