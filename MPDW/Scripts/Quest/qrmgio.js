// qrmgio.js
function qrmgio(cb, ud, tmo) {
    var _self = this;
    this._cb = cb;
    this._ud = ud;
    this._tmo = tmo == undefined ? 600000 : tmo;
    this._url;
    this._path;
    this._r = null;

    this._init = function () {
        _self._path = GetCurrentPath();
    }

    this.ShowView = function (url, d) {
        try {
            var _url = _self._path + url;
            if (d && (url.indexOf('?') < 0)) {
                _url += '?' + $.param(d);
            }
            document.location.href = _url;
        }
        catch (e) {
            alert('EXCEPTION: retrieving ' + url);
        }
    }
    this.PostView = function (url, d, ud) {
        if (ud !== undefined) { this._ud = ud; }
        var _url = _self._path + url;
        _self._r = $.ajax({
            timeout: _self._tmo,
            type: 'POST',
            method: 'POST',
            url: _url,
            data: JSON.stringify(d),
            processData: false,
            contentType: "application/json; charset=UTF-8",
            cache: false,
            error: function (xhr, st, err) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                var _e = _self._bldE(_url, xhr.status, err);
                if (_e) {
                    _self._docallback('F|' + _e);
                }
            },
            success: function (Data) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                _self._docallback(Data);
            }
        });
        _self._r = _rr.Add(_self._r);
    }
    this.GetJSON = function (url, d, ud) {
        return (_self._json('get', url, d, ud));
    }
    this.PostJSON = function (url, d, ud) {
        return (_self._json('post', url, d, ud));
    }
    this._json = function (m, url, d, ud) {
        return (_self._ajax(m, 'json', url, d, ud));
    }
    this._jsonp = function (m, url, d, ud) {
        return (_self._ajax(m, 'jsonp', url, d, ud));
    }
    this._ajax = function (m, dt, url, d, ud, ct) {
        if (ud !== undefined) { this._ud = ud; }
        var _url = _self._path + url;
        _self._r = $.ajax({
            timeout: _self._tmo,
            type: m,
            dataType: dt,
            url: _url,
            data: d,
            cache: false,
            error: function (xhr, st, err) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                var _e = _self._bldE(_url, xhr.status, err);
                if (_e) {
                    _self._docallback('F|' + _e);
                }
            },
            success: function (Data) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                _self._docallback(Data);
            }
        });
        _self._r = _rr.Add(_self._r);
    }
    this._postJSON = function (url, d, ud) {
        if (ud !== undefined) { this._ud = ud; }
        var _url = _self._path + url;
        _self._r = $.ajax({
            timeout: _self._tmo,
            type: "POST",
            dataType: "json",
            url: _url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(d),
            cache: false,
            accept: 'application/json',
            error: function (xhr, st, err) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                var _e = _self._bldE(_url, xhr.status, err);
                if (_e) {
                    _self._docallback('F|' + _e);
                }
            },
            success: function (Data) {
                if (_rr && _rr.bAborting) { return; }
                _rr.Remove(_self._r);
                _self._docallback(Data);
            }
        });
        _self._r = _rr.Add(_self._r);
    }
    this._docallback = function (data) {
        if (_self._cb) {
            _self._cb(_self._ud, data);
        }
    }
    // TODO: REMOVE THIS FROM HERE.
    this._bldE = function (url, st, txt) {
        if ((url === undefined) || (url === null)) { return (null) };
        if (st === null || st == 0) { return; }
        var _m = [], i = 0;
        _m[i++] = 'Server Request Error: (';
        _m[i++] = url ? url : 'null';
        _m[i++] = ')  Status: (';
        _m[i++] = st ? st : 'null';
        _m[i++] = ')  Error: (';
        _m[i++] = txt ? txt : 'null';
        _m[i++] = ')|' + st;
        return (_m.join(''));
    }

    this.Abort = function () {
        _self._r.Abort();
    }

    _self._init();
}

function _questreqs() {
    var _self = this;
    this._rr = [];
    this._id = 0;
    this.bAborting = false;

    this._init = function () {
    }

    this.Add = function (r) {
        _self._id = _self._id + 1;
        r.Id = _self._id;
        _self._rr.push(r);
        return (r);
    }
    this.Remove = function (r) {
        if (_self._rr.length == 1) {
            var x = 0;
            x = 4;
        }
        var _rr = [];
        $.each(_self._rr, function (i, _r) {
            if (_r.Id != r.Id) {
                _rr.push(_r);
            }
        });
        _self._rr = _rr;
    }
    this.Abort = function (r) {
        if (!r) { return (_self._abort()); }
        var _r = _self._find(r);
        if (_r) {
            _r.abort();
            return (true);
        }
        return (false);
    }
    this._find = function (r) {
        var _idx = $.inArray(r.Id, _self._rr);
        if (_idx < 0) { return (null); };
        return (_self._rr[_idx]);
    }
    this._abort = function () {
        _self.bAborting = true;
        $.each(_self._rr, function (i, r) {
            r.abort();
        });
        _self._rr = [];
    }
    _self._init();
}
var _rr = new _questreqs();
