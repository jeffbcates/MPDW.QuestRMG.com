// qrmgPanel
function qrmgPanel(model) {
    var _self = this;
    _self._model = model;
    _self._pfx = model.prefix || '';
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._uri = model.uri ? (model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/') : null;
    _self._pkey = null;
    _self._options = [];
    _self._bChanges = false;

    _self._init = function () {
        _self._initctx();
        _self._inittools();
        _self._initaa();
        _self._initoo();
        _self._initff();
        _self._inittt();
        _self._render();
        _self._bindtitle();
        _self._bindtools();
        _self._bindoo();
        _self._bindaa();
        _self._bindtt();
        _self.GetOptions();
    }
    _self._initctx = function () {
        _self._ctx = new qrmgctx(_self._model.ctx);
    }
    _self._inittools = function () {
        _self._tools = [
            { name: 'options', class: 'config', title: 'Options', callback: _self._options },
            { name: 'collapse', class: 'collapse', title: 'Collapse/Expand', callback: _self._collapse },
            { name: 'reload', class: 'reload', title: 'Reload', callback: _self._reload },
        ];
    }
    _self._initaa = function () {
        _self._model.actions = _self._model.actions || [];
        $.each(_self._model.actions, function (i, a) {
            a._m = a.method ? a.method : 'GET';
            _self.bTabbed = _self.bTabbed || (a.type ? a.type == 'tab' : false);
        });
    }
    _self._initoo = function () {
        _self._model.operations = _self._model.operations || [];
        $.each(_self._model.operations, function (i, o) {
            o._m = o.method ? o.method : undefined;
            _self.bTabbed = _self.bTabbed || (o.type ? o.type == 'tab' : false);
        });
    }
    _self._initff = function () {
        _self._model.fields = _self._model.fields || [];
        $.each(_self._model.fields, function (i, f) {
            f._id = _self._pfx + f.name;
            f.readonly = (_self._model.readonly || f.readonly);
            f._lbl = f.label ? f.label : f.name;
            if (f.key) {
                _self._pkey = f;
            }
            if (f.type && f.type == 'select') {
                _self._options.push(f);
            }
        });
    }
    _self._inittt = function () {
        _self._model.tabs = _self._model.tabs || [];
        $.each(_self._model.tabs, function (i, t) {
            if (t.callback) {
                var _content = t.callback({ Init: true }, t);
                t._content = _content || false;
            }
            if (t.defaultTab) {
                _self._defTab = t;
            }
        })
    }

    _self._bindtitle = function () {
        if (!_self.bTabbed) { return; }
        $('.portlet-title', _self._e).on('click', function (e) {
            _self.Tab();
        });
        $(_self._e).find('.portlet-title').on('dblclick', function (e) {
            _self._collapse();
        });
    }
    _self._bindtools = function () {
        var _tt = $(_self._e).find('.portlet-title').find('.tools a');
        if (_tt) {
            $.each(_tt, function (i, t) {
                $(t).on('click', null, t, function (e) {
                    var n = this.id.substr("_porttool".length);
                    var t = _self._gettool(n);
                    if (!t) { return; }
                    if (t.callback) {
                        t.callback(t);
                    }
                })
            });
        }
    }
    _self._gettool = function (n) {
        var _t = null;
        $.each(_self._tools, function (i, t) {
            if (t.name == n) {
                _t = t;
                return (false);
            }
        });
        return (_t);
    }
    _self._geto = function (n) {
        var _o = null;
        $.each(_self._model.operations, function (i, o) {
            if (o.name == n) {
                _o = o;
                return (false);
            }
        });
        return (_o);
    }
    _self._geta = function (n) {
        var _a = null;
        $.each(_self._model.actions, function (i, a) {
            if (a.name == n) {
                _a = a;
                return (false);
            }
        });
        return (_a);
    }
    _self._bindoo = function () {
        var _oo = $('.form-actions .pull-left', _self._e).find('.btn');
        if (_oo) {
            $.each(_oo, function (i, o) {
                $(o).on('click', null, o, function (e) {
                    _self._activetab(e);
                    var n = this.id.substr(_self._pfx.length + '_btn'.length);
                    _self.DoOper(n);
                })
            });
        }
    }
    _self._bindaa = function () {
        var _aa = $('.form-actions .pull-right', _self._e).find('.btn');
        if (_aa) {
            $.each(_aa, function (i, a) {
                $(a).on('click', null, a, function (e) {
                    _self._activetab(e);
                    var pp = this.id.split('_');
                    var n = pp[pp.length - 1];
                    var _a = _self._geta(n);
                    if (!_a) { return; }
                    if (_a.callback) {
                        var _d = _self.GetData();
                        if (!_a.callback(_a, _d)) { return; }
                    }
                    if (_a.type && _a.type == 'tab') {
                        _self.Tab(_a.name);
                        return;
                    }
                    if (_self[_o._action] !== undefined) {
                        _self[_o._action]();
                    }
                    else {
                        var _d = _self.GetData();
                        _self._docallback(_a, _d);
                    }
                })
            });
        }
    }
    _self._activetab = function (e) {
        $('.btn-set', _self._e).find('button').removeClass('active');
        $(e.currentTarget).addClass('active');
    }
    _self._bindtt = function () {
    }
    _self._bindff = function () {
        $('input', _self._e).on('change', function (e) {
            _self._bChanges = true;
        });
        $('select', _self._e).on('change', function (e) {
            _self._bChanges = true;
        });
        $('textarea', _self._e).on('change', function (e) {
            _self._bChanges = true;
        });
    }

    _self.DoOper = function (n) {
        var _o = _self._geto(n);
        if (!_o) { return; }
        var _d;
        if (_o.callback) {
            _d = _self.GetData();
            if (!_o.callback(_o, _d)) { return; }
        }
        if (_o.type && _o.type == 'tab') {
            _self.Tab(_o.name);
        }
        if (_o._action) {
            _self[_o._action]();
        }
        else {
            _self._dooper(_o, _d);
        }
    }
    _self._dooper = function (o, d) {
        qrmgmvc.Global.Maskit($(_self._e));
        ClearUserMessage();
        if (o.validate) {
            if (_self.Validate()) {
                qrmgmvc.Global.Unmask($(_self._e));
                return;
            }
        }
        var _d = d ? d : _self.GetData();
        var _url = _self._uri + o.name;
        var _io = new qrmgio(_self._roper, o, o.timeout);
        if (o._m == 'POST') {
            ////_io.PostJSON(_url, _d, o);
            _io._postJSON(_url, _d, o);
        }
        else if (o._m == 'GET') {
            _io.GetJSON(_url, _d, o);
        }
        else {
            qrmgmvc.Global.Unmask($(_self._e));
        }
    }
    _self._roper = function (ud, d) {
        if (IsUserMessage(d)) {
            DisplayUserMessage(d);
        }
        if (ud && ud.callback) {
            ud.callback(ud, d);
        }
        qrmgmvc.Global.Unmask($(_self._e));
    }

    _self._render = function () {
        var _e = $(_self._e);
        if (!_e) { alert('csPanel element ' + _self._e + ' not found!'); return; }
        var _wc = (_self._model.colmd ? _self._model.colmd : 'col-md-12');
        $(_e).addClass('editorFrame ' + _wc);

        var _h = [], _i = 0;
        _h[_i++] = '    <div class="portlet box blue">';
        _h[_i++] = '        <div class="portlet-title' + (_self.bTabbed ? ' clickable' : '') + '">';
        _h[_i++] = _self._rdnrcap();
        if (_self._tools) {
            _h[_i++] = _self._rndrtools();
        }
        _h[_i++] = '        </div>';
        _h[_i++] = _self._rndrbody();
        _h[_i++] = '        </div>';
        _h[_i++] = '    </div>';

        $(_e).empty().append(_h.join(''));
        $(_e).find('input.date').datepicker({ format: "mm/dd/yyyy" });
        $.each(_self._model.fields, function (i, f) {
            if (f.richtexteditor) {
                $('#' + f._id).jqte({ 'height': '500px' });
            }
            else if (f.ckeditor) {
                $('#' + f._id).css('height', '1000px');
                CKEDITOR.replace(f._id);
            }
            else if (f.quill) {
                f._quill = new Quill('#' + f._id, { theme: 'snow' });
            }
        });
        if (_self._defTab) {
            _self.Tab(_self._defTab.name);
        }
    }
    _self._rdnrcap = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="caption">';
        if (_self._model.icon) {
            _h[_i++] = '    <i class="fa fa-' + _self._model.icon + '"></i>';
        }
        _h[_i++] = _self._model.title;
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrtools = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="tools">';
        $.each(_self._tools, function (i, t) {
            _h[_i++] = '    <a id="_porttool' + t.class + '" class="' + t.class + '" href="javascript:;" data-original-title="" title="' + t.title + '"></a>';
        });
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrbody = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="portlet-body form" style="display: block; ' + (_self._model.minHeight ? (' min-height: ' + _self._model.minHeight + ';') : '' ) + '">';
        _h[_i++] = _self._rndrtbar();
        _h[_i++] = '    <div class="form-body portlet-container' + (_self._sanshdr ? '-sans-actions' : '') + '">';
        _h[_i++] = _self._rndrff();
        _h[_i++] = _self._rndrtt();
        _h[_i++] = '    </div>';
        _h[_i++] = '</div>';
        if (_self._model.bBottomBar) {
            _h[_i++] = _self._rndrtbar(true);
        }
        return (_h.join(''));
    }
    _self._rndrtbar = function (bBottom) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-actions ' + (bBottom ? 'bottom' : 'top') + '">';
        _h[_i++] = _self._rndrops(bBottom);
        _h[_i++] = _self._rndracts();
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrops = function (bBottom) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="btn-set pull-left">';
        $.each(_self._model.operations, function (i, o) {
            _h[_i++] = _self._rndrop(o, bBottom);
        });
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrop = function (o, bBottom) {
        if (bBottom) {
            if (!o.position || (o.position.indexOf("Bottom") == -1)) { return; }
        }
        o._id = _self._pfx + '_btn' + o.name;
        o._lbl = o.label ? o.label : o.name;
        var _h = [], _i = 0;
        _h[_i++] = '<button id="' + o._id + '" class="btn ' + (o.class ? o.class : 'pnlbtn') + '" type="button"';
        if (o.title) {
            _h[_i++] = ' title="' + o.title + '" ';
        }
        _h[_i++] = ' >' + o._lbl + '</button>';
        return (_h.join(''));
    }
    _self._rndracts = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="btn-set pull-right">';
        $.each(_self._model.actions, function (i, a) {
            _h[_i++] = _self._rndract(a);
        });
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndract = function (a) {
        a._id = _self._pfx + '_act_' + a.name;
        a._lbl = a.label ? a.label : a.name;
        var _h = [], _i = 0;
        if (a.type) {
            if (a.type == 'text') {
                _h[_i++] = '<label class="portlet-toolbar-label" ' + (a.title ? ' title="' + a.title + '" ' : '') + '>' + a._lbl + ':&nbsp;</label>';
                _h[_i++] = '<input id="' + a._id + '" type="text" class="portlet-toolbar-text text-right" ' + (a.title ? ' title="' + a.title + '" ' : '') + ' value="' + (a.value ? a.value : '') + '"/>';
            }
            else if (a.type == 'tab') {
                _h[_i++] = '<button id="' + a._id + '" class="btn ' + (a.class ? a.class : 'grey') + '" type="button" ';
                if (a.title) {
                    _h[_i++] = ' title="' + a.title + '" ';
                }
                _h[_i++] = '>' + a._lbl + '</button>';
            }
        }
        else {
            _h[_i++] = '<button id="' + a._id + '" class="btn ' + (a.class ? a.class : 'grey') + '" type="button" ';
            if (a.title) {
                _h[_i++] = ' title="' + a.title + '" ';
            }
            _h[_i++] = '>' + a._lbl + '</button>';
        }
        return (_h.join(''));
    }
    _self._rndrff = function () {
        var _h = [], _i = 0;
        if (_self.bTabbed) {
            _h[_i++] = '<div id="' + (_self._pfx + '_FieldsTab') + '" class="portlet-tab">';
        }
        $.each(_self._model.fields, function (i, f) {
            _h[_i++] = _self._rndrf(f);
        });
        if (_self.bTabbed) {
            _h[_i++] = '</div>';
        }
        return (_h.join(''));
    }
    _self._rndrf = function (f) {
        if (!f.type) {
            if (f.hidden) {
                return (_self._rndrhid(f));
            }
            else {
                return (_self._rndrftxt(f));
            }
        }
        else if (f.type == 'text') {
            return (_self._rndrftxt(f));
        }
        else if (f.type == 'checkbox') {
            return (_self._rndrchkbx(f));
        }
        else if (f.type == 'number') {
            return (_self._rndrfnum(f));
        }
        else if (f.type == 'email') {
            return (_self._rndremail(f));
        }
        else if (f.type == 'password') {
            return (_self._rndrpwd(f));
        }
        else if (f.type == 'select') {
            return (_self._rndrsel(f));
        }
        else if (f.type == 'multi-select') {
            return (_self._rndrmsel(f));
        }
        else if (f.type == 'textarea') {
            return (_self._rndrftxta(f));
        }
        else if (f.type == 'radio') {
            return (_self._rndrradio(f));
        }
        else if (f.type == 'date') {
            return (_self._rndrdate(f));
        }
    }
    _self._rndrhid = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<input id="' + f._id + '" class="form-control" type="hidden" />';
        return (_h.join(''));
    }
    _self._rndrftxt = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        if (f.nolabel) {
        }
        else {
            _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        }
        if (!f.labelonly) {
            _h[_i++] = '    <input id="' + f._id + '" class="form-control ' + (f.bold ? " bold" : "") + (f.classes ? (" " + f.classes + " ") : "") + '" type="text" placeholder="' + (f.placeholder ? f.placeholder : '') + '" ' + (f.readonly ? ' readonly="readonly" ' : '') + ' ';
            if (f.maxlength) {
                _h[_i++] = ' maxlength="' + f.maxlength + '" ';
            }
            _h[_i++] = '/>';
        }

        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrchkbx = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <br/>&nbsp;&nbsp;';
        _h[_i++] = '    <input id="' + f._id + '" class="" type="checkbox" ' + (f.min ? (' min="' + f.min + '" ') : '') + (f.readonly ? ' readonly="readonly" ' : '') + 'placeholder="' + (f.placeholder ? f.placeholder : '') + '" />';
        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrfnum = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <input id="' + f._id + '" class="form-control" type="number" ' + (f.min ? (' min="' + f.min + '" ') : '') + (f.max ? (' max="' + f.max + '" ') : '') + (f.readonly ? ' readonly="readonly" ' : '') + 'placeholder="' + (f.placeholder ? f.placeholder : '') + '" />';
        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndremail = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <div class="input-group">';
        _h[_i++] = '        <span class="input-group-addon"><i class="fa fa-envelope"></i></span>';
        _h[_i++] = '        <input class="form-control" type="email" placeholder="' + (f.placeholder ? f.placeholder : 'Email Address') + '" />';
        _h[_i++] = '        <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '    </div>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrpwd = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <div class="input-group">';
        _h[_i++] = '        <input id="' + f._id + '" class="form-control" type="password" placeholder="' + (f.placeholder ? f.placeholder : 'Password') + '" />';
        _h[_i++] = '        <span class="input-group-addon"><i class="fa fa-user"></i></span>';
        _h[_i++] = '    </div>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrsel = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <select id="' + f._id + '" class="form-control" ' + (f.readonly ? ' disabled="disabled" ' : '') + '>';
        if (f.options) {
            $.each(f.options, function (i, o) {
                _h[_i++] = '    <option value="' + o.Id + '">' + o.Label + '</option>';
            });
        }
        _h[_i++] = '    </select>';
        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrmsel = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '        <select id="' + f._id + '" class="form-control" multiple="">';
        if (f.options) {
            $.each(f.options, function (i, o) {
                _h[_i++] = '        <option value="' + o.Id + '">' + o.Label + '</option>';
            });
        }
        _h[_i++] = '        </select>';
        _h[_i++] = '        <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '    </div>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrftxta = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <textarea id="' + f._id + '" class="form-control" rows="' + (f.rows ? f.rows : '20') + '" ' + (f.readonly ? ' readonly="readonly" ' : '') + '></textarea>';
        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrradio = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <div class="radio-list">';
        $.each(f.options, function (i, o) {
            _h[_i++] = '        <label class="radio-inline">';
            _h[_i++] = '            <div id="uniform-optionsRadios1" class="radio">';
            _h[_i++] = '                <span class="checked">';
            _h[_i++] = '                    <input id="' + f._id + '" type="radio" ' + (o.default ? 'checked=""' : '') + ' value="' + o.value + '" name="' + o.name + '">';
            _h[_i++] = '                </span>';
            _h[_i++] = '            </div>';
            _h[_i++] = f._lbl;
            _h[_i++] = '        </label>';
        });
        _h[_i++] = '    </div>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrdate = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="form-group">';
        _h[_i++] = '    <label class="control-label">' + f._lbl + '</label>';
        _h[_i++] = '    <input id="' + f._id + '" class="form-control date" type="text" placeholder="' + (f.placeholder ? f.placeholder : '') + '" ' + (f.readonly ? ' readonly="readonly" ' : '') + '/>';
        _h[_i++] = '    <span class="help-block">' + (f.help ? f.help : '') + '</span>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrbdy = function (a) {
        var _h = [], _i = 0;
        _h[_i++] = '<a class="btn btn-default btn-sm" href="javascript:;">';
        _h[_i++] = '</a>';
        return (_h.join(''));
    }
    _self._sanshdr = function () {
        return(_self._model.actions.length + _self._model.operations.length);
    }
    _self._rndrtt = function () {
        if (!_self.bTabbed) { return (''); }
        var _h = [], _i = 0;
        $.each(_self._model.tabs, function (i, t) {         
            _h[_i++] = '<div id="' + (_self._pfx + t.name + 'Tab') + '" class="portlet-tab ' + (t.classes ? t.classes : '') + ' hidden">';
            _h[_i++] = t._content || '';
            _h[_i++] = '</div>';           
        });
        return (_h.join(''));
    }
    

    _self.Read = function (id) {
        if (!id) { return; }
        qrmgmvc.Global.Mask(_self._e);
        _self._read(id);
    }
    _self._read = function (Id) {
        var _questio = new qrmgio(_self._rread, _self._model);
        var _d = { Id: Id };
        if (_self._model.ctx) {
            if ($.isArray(_self._model.ctx)) {
                $.each(_self._model.ctx, function (i, c) {
                    _d[c.name] = $('#' + c.value).val();
                });
            }
            else {
                _d[_self._model.ctx.name] = $('#' + _self._model.ctx.value).val();
            }
        }
        var _uri = _self._model.uri + '/Read';
        _questio._ajax('get', 'json', _uri, _d);
    }
    _self._rread = function (ud, d) {
        if (IsUserMessage(d)) {
            DisplayUserMessage(d);
        }
        else {
            _self._load(d);
        }
        qrmgmvc.Global.Unmask(_self._e);
    }

    _self.Load = function (d) {
        return (_self._load(d));
    }
    _self._load = function (d) {
        _self.Clear();
        $.each(_self._model.fields, function (i, f) {
            if (d[f.name] === undefined) { return; }
            _e = _self._getfe(f);
            if (_e === undefined) { return; }
            if ($(_e).is('input')) {
                var _t = $(_e).attr('type');
                if (_t == 'text') {
                    $(_e).val(d[f.name]);
                }
                else if (_t == 'hidden') {
                    $(_e).val(d[f.name]);
                }
                else if (_t == 'checkbox') {
                    var _v = d[f.name];
                    $(_e).prop('checked', _v);
                }
                else if (_t == 'textarea') {
                    if (f.ckeditor) {
                        CKEDITOR.instances[f._id].setData(d[f.name], {
                            callback: function () {
                                this.checkDirty(); // true
                            }
                        });
                    }
                    else if (f.quill) {

                    }
                    else {
                        $(_e).val(d[f.name]);
                        if (f.richtexteditor) {
                            $('#' + f._id).jqteVal(d[f.name]);
                        }
                    }
                }
                    // TODO: RADIO
                else {
                }
            }
            else {
                if (f.type) {
                    if (f.type === 'hidden') {
                        var _f = $('input[id="' + f.id + '"]', _self._e);
                        if (_f && _f.length > 0) {
                            $(_f).val(d[f.name]);
                        }
                    }
                    else if (f.type === 'select') {
                        var _v = d[f.name];
                        var _s = $(_e).find('option[id="' + _v + '"]');
                        if (_s.length == 0) {
                            _s = $(_e).find('option').filter(function (i) {
                                return _v === $(this).text();
                            });
                        }
                        if (_s.length == 0) {
                            $.each($(_e).find('option'), function (i, o) {
                                if (o.text == _v) {
                                    _s = o;
                                    return (false);
                                }
                            });
                            $(_e).addClass('validationError');
                        }
                        else {
                            $(_s).prop('selected', true);
                        }
                    }
                    else if (f.type === 'checkbox') {
                        var _v = d[f.name];
                        if (_v == true) {
                            $(_e).prop('checked', true);
                        }
                        else {
                            $(_e).removeProp('checked');
                        }
                    }
                    else if (f.type === 'textarea') {
                        if (f.ckeditor) {
                            CKEDITOR.instances[f._id].setData(d[f.name], {
                                callback: function () {
                                    this.checkDirty(); // true
                                }
                            });
                        }
                        else {
                            $(_e).html('<div id="' + f.name + 'Label' + '" class="pnlLabel">' + _self._flbl(f) + '</div>');
                            if (f.html) {
                                $(_e).replaceWith('<div class="htmldiv form-control" contenteditable="false">' + d[f.name] + '</div>');
                            }
                            else {
                                $(_e).val(d[f.name]);
                            }
                            if (f.richtexteditor) {
                                $('#' + f._id).jqteVal(d[f.name]);
                            }
                        }
                    }
                }

            }
        });
    }
    _self._flbl = function (f) {
        return (f.label ? f.label : f.name);
    }
    _self._getf = function (name) {
        var _f = null;
        $.each(_self._model.fields, function (i, f) {
            if (name === f.name) {
                _f = f;
                return (false);
            }
        });
        return (_f);
    }
    _self._getfe = function (f) {
        var _e = $('#' + _self._pfx + f.name, _self._e);
        return (_e);
    }
    _self._gettab = function (n) {
        $.each(_self._model.tabs, function (i, t) {

        });
    }

    _self.GetOptions = function () {
        _self._optcnt = 0;
        if (_self._options.length) {
            $.each(_self._model.fields, function (i, f) {
                if (f.type == 'select') {
                    _self.LoadOptions(f);
                }
            });
        }
        else {
            if (_self._model.callback) {
                var d = _self.GetData();
                _self._docallback({ PostInit: true }, d);
            }
        }
    }
    _self.LoadOptions = function (f) {
        _self._optcnt += 1;
        $('#' + f._id, _self._e).empty().prop('disabled', 'disabled').text('Loading ...').append('<option value="-1"><span class="italic">Loading ...</span></option>');
        var _d = {};
        if (f.ctx) {
            if ($.isArray(f.ctx)) {
                $.each(f.ctx, function (i, c) {
                    _d[c.name] = $('#' + c.value).val();
                });
            }
            else {
                _d[f.ctx.name] = $('#' + f.ctx.value).val();
            }
        }
        var _url = _self._uri + f.name + 'Options';
        var _questio = new qrmgio(_self._ropt, f);
        _questio._ajax('get', 'json', _url, _d);
    }
    _self._ropt = function (f, Data) {
        if (IsUserMessage(Data)) {
            return (_self._docallback(f, Data));
        }
        _self._loadOptions(f, Data);
        _self._optcnt -= 1;
        if (!_self._optcnt) {
            if (_self._model.callback) {
                _self._bChanges = false;
                _self._docallback({ PostInit: true }, Data);
            }
        }
    }
    _self._loadOptions = function (f, Data) {
        var _e = $('select[id="' + f._id + '"]', _self._e);
        var i = 0, h = [];
        $.each(Data, function (i, d) {
            h[i++] = '<option id="' + d.Id + '">' + d.Label + '</option>';
        });
        $(_e).unbind().empty();
        $(_e).append(h.join(''));
        if (!f.readonly) {
            $(_e).removeAttr('disabled');
        }
    }

    _self.Clear = function () {
        var _ff = $(_self._e).find(':input');
        $.each(_ff, function (i, f) {
            if ($(f).is(':button')) {
                return (true);
            }
            if ($(f).is(':text')) {
                $(f).val('');
            }
            else if ($(f).is(':checkbox')) {
                $(f).prop('checked', false);
            }
            else if ($(f).prop('type') == 'select-one') {
                $(f).val([]);
            }
            else if ($(f).prop('type') == 'select-multiple') {
                $(f).val([]);
            }
            else if ($(f).is(':radio')) {
                $(f).prop('checked', false);
            }
            else {
                var _f = _self._getf(f.id.substr(_self._pfx.length));
                if (_f && _f.ckeditor) {
                    ////v = CKEDITOR.instances[f.id].setData('');
                }
                else {
                    $(f).val('');
                }
            }
        });
        $.each(_self._model.fields, function (i, f) {
            if (f.richtexteditor) {
                $('#' + f._id).jqteVal('');
            }
            else if (f.ckeditor) {
                ////CKEDITOR.instances[f._id].setData('');
            }
        });
    }
    _self.Refresh = function () {
        _self.Read(_self._getkeyv());
    }
    _self._getkeyv = function () {
        return ($('#' + _self._pkey._id, _self._e).val());
    }

    _self._docallback = function (ud, data) {
        if (_self._model.callback) {
            return (_self._model.callback(ud, data));
        }
    }

    _self.Hide = function () {
        $(_self._e).hide();
    }
    _self.Show = function () {
        $(_self._e).show();
    }
    _self.IsVisible = function (bBody) {
        if (bBody) {
            return ($(_self._e).find('.portlet-body').is(":visible"));
        }
        return ($(_self._e).is(":visible"));
    }

    _self.Collapse = function () {
        $(_self._e + ' .portlet-body').hide(300);
    }
    _self.Expand = function () {
        $(_self._e + ' .portlet-body').show(300);
    }
    _self._options = function () {
        // TODO: dialog  or expanded menu of options
    }
    _self._collapse = function () {
        _self.IsVisible(true) ? _self.Collapse() : _self.Expand();
    }
    _self._reload = function () {
        _self.Refresh();
    }
    _self.GetData = function () {
        var d = {};
        var _ff = $('form-body', _self._e).find(':input');
        $.each(_ff, function (i, f) {
            if ($(f).is(':button')) {
                return (true);
            }
            var n = $(f).attr('id');
            if (!n || n.substr(0, _self._pfx.length) != _self._pfx) {
                return (true);
            }
            var v = _self._getval(f);
            d[n] = v;
        });
        var _pp = $(_self._e).find(':password');
        $.each(_pp, function (i, p) {
            var n = $(p).attr('id');
            if (!n) {
                return (true);
            }
            n = n.substr(_self._pfx.length);
            v = $(p).val();
            d[n] = v;
        });
        $.each(_self._model.actions, function (i, a) {
            var _a = $('#' + a._id);
            if (!$(_a).is('input')) { return; } // TODO: select etc.
            var v = _self._getval(_a);
            d[a.name] = v;
        });
        var _ctx = _self._getctx();
        $.extend(d, _ctx);
        return d;
    }
    _self._getval = function (e) {
        var n = $(e).attr('id');
        if (!n || n.substr(0, _self._pfx.length) != _self._pfx) {
            return (true);
        }
        n = n.substr(_self._pfx.length);
        var v;
        if ($(e).is(':text')) {
            v = $(e).val();
        }
        else if ($(e).is(':checkbox')) {
            if (d[n] !== undefined) {
                return (true);
            }
            v = $(e).is(':checked');
        }
        else if ($(e).prop('type') == 'select-one') {
            v = $(e).find('option:selected').length == 0 ? null : $(e).find('option:selected')[0].id;
        }
        else if ($(e).prop('type') == 'select-multiple') {
            var _v = [];
            $.each($(e).find('option:selected'), function (i, o) {
                _v.push($(o).attr('id'));
            });
            v = _v;
        }
        else if ($(e).is(':radio')) {
            n = $('#' + e._id).attr('name').substr(_self._pfx.length);
            if (d[n] !== undefined) {
                return (true);
            }
            v = $('input[name=' + e.name + ']:checked').val();
        }
        else {
            var _f = _self._getf(e.id.substr(_self._pfx.length));
            if (_f) {
                if (_f.ckeditor) {
                    v = CKEDITOR.instances[e.id].getData();
                }
            }
            if (!v) {
                v = $(e).val();
            }
        }
        return (v);
    }

    _self.ReadOnly = function (bReadOnly) {
        _self._model.readonly = bReadOnly;
        _self._readOnly(bReadOnly);
    }
    _self._readOnly = function (bReadOnly) {
        if (bReadOnly) {
            $(_self._e).find(':input').attr('disabled', 'disabled');
            $(_self._e).find('select').attr('disabled', 'disabled');
            $(_self._e).find('textarea').attr('disabled', 'disabled');
        }
        else {
            $(_self._e).find(':input').removeAttr('disabled');
            $(_self._e).find('select').removeAttr('disabled');
            $(_self._e).find('textarea').removeAttr('disabled');
        }
    }

    _self.Disable = function () {
        $(_self._e).find('button').attr('disabled', 'disabled');
        $(_self._e).find(':input').attr('disabled', 'disabled');
        $(_self._e).find('select').attr('disabled', 'disabled');
        $(_self._e).find('textarea').attr('disabled', 'disabled');
    }
    _self.Enable = function () {
        $(_self._e).find('button').removeAttr('disabled');
        $(_self._e).find(':input').removeAttr('disabled');
        $(_self._e).find('select').removeAttr('disabled');
        $(_self._e).find('textarea').removeAttr('disabled');
    }

    _self.Validate = function () {
        $(_self._e).find('.validationError').removeClass('validationError');
        $.each(_self._model.fields, function (i, f) {
            if (f.required && f.required == true) {
                if (f.type && f.type == 'select') {
                    if (_self._mto(f)) {
                        $('#' + _self._pfx + f.name, _self._e).addClass('validationError');
                    }
                }
                else {
                    if (_self._mt(f)) {
                        $('#' + _self._pfx + f.name, _self._e).addClass('validationError');
                    }
                }
            }
        });
        return ($(_self._e).find('.validationError').length);
    }
    _self._mt = function (f) {
        var _e = $('#' + _self._pfx + f.name, _self._e);
        if (_e.length == 0) { return; }
        return ($('#' + _self._pfx + f.name, _self._e).val().trim().length == 0);
    }
    _self._mto = function (o) {
        var _e = $('#' + _self._pfx + o.name, _self._e);
        if (_e.length == 0) { return; }
        return ($('#' + _self._pfx + o.name, _self._e).find(":selected").attr('id') == "-1");
    }

    _self._getctx = function () {
        var _ctx = _self._ctx.Context();
        $.extend(_ctx, _self.Context());
        return (_ctx);
    }
    _self.Context = function (n) {
        var ctx = {};
        if (n) {
            var _d = _self.GetData();
            ctx[n] = _d[n];
        }
        else {
            ctx[_self._pkey.name] = _self._getkeyv();
        }
        return (ctx);
    }
    _self.Tab = function (n) {
        if (n) {
            $(_self._e).find('.portlet-tab').addClass('hidden');
            var _n = n || '_Fields';
            $('#' + _self._pfx + _n + 'Tab', _self._e).removeClass('hidden');
        }
        else {
            var id = $(_self._e).find('.portlet-tab:visible').attr('id');
            if (id) {
                var n = id.slice(0, -3).substr(_self._pfx.length);
            }
            return (n);
        }
    }

    _self.GetField = function (fn) {
        var _f;
        $.each(_self._model.fields, function (i, f) {
            if (f.name == fn) {
                _f = f;
                return (false);
            }
        });
        return (_f);
    }
    _self.GetLabel = function (id) {
        var _f;
        $.each(_self._model.fields, function (i, f) {
            if (f._id == id) {
                _f = f;
                return (false);
            }
        });
        return (_f ? _f.label : id);
    }
    _self.SetField = function (n, v) {
        var f = _self.GetField(n);
        if (!f) { return; }
        var _f = $('#' + f._id, _self._e);
        _self._setf(_f, v);
    }
    _self._setf = function (f, v) {
        if ($(f).is(':text')) {
            $(f).val(v);
        }
        else if ($(f).is(':checkbox')) {
            $(f).prop('checked', v);
        }
        else if ($(f).prop('type') == 'select-one') {
            $(f).find('option[value="' + v + '"]').prop('selected', true);
        }
        else if ($(f).prop('type') == 'select-multiple') {
            $(f).val(v);
        }
        else if ($(f).is(':radio')) {
            $(f).prop('checked', v);
        }
        else {
            $(f).val(v);
        }
    }

    _self.GetAction = function (an) {
        var _a;
        $.each(_self._model.actions, function (i, a) {
            if (a.name == an) {
                _a = a;
                return (false);
            }
        });
        return (_a);
    }

    _self.bChanges = function () {
        return (_self._bChanges);
    }

    _self._init();
}