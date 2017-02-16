// qrmgumsg.js
function qrmgumsg(model) {
    var _self = this;
    this._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    this._model = model;
    this._pfx = model.prefix ? model.prefix : '';
    this._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    this._cb = model.callback ? model.callback : null;

    this._init = function () {
    }

    this.DisplayFormMessages = function (_questForm) {
        _self.Clear();
        var _vs = _questForm.ViewState();
        if (!_vs) { return; }
        var _h = [], _i = 0;
        if (_vs.UserMessages) {
            $.each(_vs.UserMessages, function (i, u) {
                var _id = _questForm._e.substr(1) + '_' + (i + 1);
                $(_self._e).append('<div id="' + _id + '" class="umsg"></div>');
                DisplayUserMessageEx($('#' + _id, _self._e), u);
            });
        }
        $.each(_vs.InvalidFields, function (i, f) {
            var _f = _questForm.GetField(f);
            _h[_i++] = '<a href="javascript:$(#' + _f.id + ',' + _questForm._e + ').scrollTo(#' + _f.id + ').focus()">' + _vs.UserMessages[i] + '</a>';
        });

        $(_self._e).append(_h.join(''));
        _self.Expand();
    }

    this.Expand = function () {
        $(_self._e).css('padding', '10px');
        $(_self._e).animate({ height: 'auto' });
        $(_self._e).mouseenter();
    }
    this.Collapse = function () {
        ////$(_self._e).css('padding', '0').animate({ height: '0' });
    }
    this.Clear = function () {
        $(_self._e).empty();
    }
    _self._init();
}

