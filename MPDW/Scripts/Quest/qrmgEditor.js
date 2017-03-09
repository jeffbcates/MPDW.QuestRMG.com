// qrmgEditor.js
function qrmgEditor(model) {
    var _self = this;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._model = model;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._cb = model.callback ? model.callback : null;
    _self._options = [];
    _self._pkey = null;
    _self._bROS = false;
    _self._bChanges = false;
    _self._bMasking = _self._model.mask || !_self._model.bNoMasking;
    _self._defopers = [
        { name: 'Save', classes: "btn btn-success", Save: true },
        { name: 'Clear', classes: "btn btn-secondary", Clear: true },
        { name: 'Cancel', label: 'Back', classes: "btn btn-warning", Cancel: true },
        {
            name: 'Delete', classes: "btn btn-danger", Delete: true,
            attrs: [
                { name: 'data-toggle', value: 'modal' },  
                { name: 'data-target', value: '#mdlgConfirmDelete' },
            ]
        },
    ];

    _self._init = function () {
        _self._initctx()
        _self._initee();
        _self._initff();
        _self._initops();
        _self._initacts();
        _self._initopts();
        _self._render();
        _self._bind();
        _self._mask = _self._model.mask || _self._e;
        _self.Mask();
        if (!_self._model.bNoLoad) {
            _self.GetOptions();
        }
    }
    _self._initctx = function () {
        _self._ctx = new qrmgctx(_self._model.ctx);
    }
    _self._initee = function() {
        if (!_self._model.events) {
            _self._model.events = [];
        }
    }
    _self._initff = function () {
        $.each(_self._model.fields, function (i, f) {
            f._id = _self._pfx + f.name;
            if (f.key) {
                _self._pkey = f;
            }
            if (f.type && f.type == 'labelOnly') {
                f.labelOnly = true;
            }
        });
    }
    _self._initops = function () {
        _self._initdefoo();
        if (!_self._model.operations) {
            _self._model.operations = [];
        }
        $.each(_self._model.operations, function (i, o) {
            o.id = _self._pfx + o.name;
            o.label = o.label || o.name;
            if (o.type) {
                if (o.type == 'View') {
                    o.view = true;
                }
            }
            var _do = _self._getdefo(o.name);
            if (_do) {
                if (!_self._model.bNoDefaultOptions) {
                    $.extend(_do, o);
                }
            }
        });
        if (!_self._model.bNoDefaultOptions) {
            var _oo = [];
            $.each(_self._model.operations, function (i, o) {
                var _do = _self._getdefo(o.name);
                if (! _do) {
                    _oo.push(o);
                }
            });
            _self._model.operations = _oo;
            _self._model.operations = _self._defopers.concat(_self._model.operations);
        }
    }
    _self._initdefoo = function () {
        $.each(_self._defopers, function (i, o) {
            o.id = _self._pfx + o.name;
            o.label = o.label || o.name;
        });
    }
    _self._getdefo = function (n) {
        var _o;
        $.each(_self._defopers, function (i, o) {
            if (n == o.name) {
                _o = o;
                return (false);
            }
        })
        return (_o);
    }

    _self._initacts = function () {
        if (!_self._model.actions) {
            _self._model.actions = [];
        }
        $.each(_self._model.actions, function (i, a) {
            a.id = _self._pfx + a.name;
        });
    }
    _self._initopts = function () {
        $.each(_self._model.fields, function (i, f) {
            if (f.type && f.type == 'select') {
                var _o = new Object();
                _o.id = _self._pfx + f.name;
                _o.uri = _self._uri + f.name + 'Options';
                _o.required = f.required;
                $.extend(_o, f);
                _self._options.push(_o);
            }
        });
        _self._model.options = _self._model.options || [];
        $.each(_self._model.options, function (i, o) {
            var _o = new Object();
            _o.id = _self._pfx + o.name;
            _o.uri = _self._uri + o.name + 'Options';
            _o.args = o.args;
            _o.bNoLoad = o.bNoLoad;
            _o.callback = o.callback;
            _o.mask = o.mask;
            _self._options.push(_o);
        });
    }

    _self._render = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div role="form" method="post" class="form-horizontal ' + (_self._model.classes ? _self._model.classes : '') + '">';
        _h[_i++] = _self._rdrff();
        _h[_i++] = '</div>';
        _h[_i++] = _self._rdroas();
        $(_self._e).empty().append(_h.join(''));
    }
    _self._rdrff = function () {
        var _h = [], _i = 0;
        if (_self._model.ShortFields) { // TEMP: ALL ONE ROW
            _h[_i++] = '<div class="row-fluid">';
            $.each(_self._model.fields, function (i, f) {
                if (f.type && f.type == 'hidden') {
                }
                else {
                    _h[_i++] = _self._rdrlbl(f);
                }
                _h[_i++] = _self._rdrf(f);
            });
            _h[_i++] = '</div>';
        }
        else {
            $.each(_self._model.fields, function (i, f) {
                if (f.type && f.type == 'hidden') {
                }
                else {
                    _h[_i++] = '<div class="row-fluid">';
                    _h[_i++] = _self._rdrlbl(f);
                }
                _h[_i++] = _self._rdrf(f);
                if (f.type && f.type == 'hidden') {
                }
                else {
                    _h[_i++] = '</div>';
                }
            });
        }
        return (_h.join(''));
    }
    _self._rdrlbl = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<label for="' + f._id + '" class="' + (_self._model.ShortFields ? 'short-label ' : 'control-label ');
        if (f.labelOnly) {
            _h[_i++] = ' ' + f.classes + ' ';
        }
        _h[_i++] = '">';

        if (f.labelOnly) {
            _h[_i++] = f.label;
        }
        else if (f.label) {
            if (f.required && f.required == true) {
                _h[_i++] = f.label + '<span class="required">*</span>';
            }
            else {
                _h[_i++] = f.label + ': ';
            }
        }
        else {
            _h[_i++] = '&nbsp;';
        }
        _h[_i++] = '</label >';
        return (_h.join(''));
    }
    _self._rdrf = function (f) {
        var _h = [], _i = 0;
        if (!f.type || f.type == 'text') {
            _h[_i++] = '<input type="text" id="' + f._id + '" name="' + f._id + '" class="' + _self._rndrfcc(f) + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        if (f.type == 'hidden') {
            _h[_i++] = '<input type="hidden" id="' + f._id + '" name="' + f._id + '" class="' + _self._rndrfcc(f) + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'checkbox') {
            _h[_i++] = '<input type="checkbox" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'select') {
            _h[_i++] = '<select type="checkbox" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="' + _self._rndrfcc(f) + '"></select>';
        }
        else if (f.type == 'radio') {
            _h[_i++] = '<input type="radio" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'textarea') {
            _h[_i++] = '    <textarea id="' + f._id + '" class="' + _self._rndrfcc(f) + '" rows="' + (f.rows ? f.rows : '4') + '" ' + (f.readonly ? ' readonly="readonly" ' : '') + '></textarea>';
        }
        else if (f.type == 'date') {
            _h[_i++] = '<input type="text" id="' + f._id + '" name="' + f._id + '" class="' + _self._rndrfcc(f) + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'email') {
            _h[_i++] = '<input type="email" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="' + _self._rndrfcc(f) + '" />';
        }
        else if (f.type == 'phone') {
            _h[_i++] = '<input type="phone" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="' + _self._rndrfcc(f) + '" />';
        }
        else if (f.type == 'password' || f.type == 'confirmpassword') {
            _h[_i++] = '<input type="password" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="' + _self._rndrfcc(f) + '" />';
        }
        else if (f.type == 'file') {
            _h[_i++] = '<input type="file" id="' + f._id + '" name="' + f._id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        return (_h.join(''));
    }
    _self._rndrfcc = function (f) {
        return((_self._model.ShortFields ? 'short-control' : 'form-control ') + (f.classes ? f.classes : ''));
    }
    _self._rdrfo = function (f) {
        var _h = [], _i = 0;
        if (f.width) {
            _h[_i++] = ' width="' + f.width + '" ';
        }
        if (f.height) {
            _h[_i++] = ' height="' + f.height + '" ';
        }
        if (f.size) {
            _h[_i++] = ' size="' + f.size + '" ';
        }
        if (f.maxlength) {
            _h[_i++] = ' maxlength="' + f.maxlength + '" ';
        }
        if (f.min) {
            _h[_i++] = ' min="' + f.min + '" ';
        }
        if (f.max) {
            _h[_i++] = ' max="' + f.max + '" ';
        }
        if (f.step) {
            _h[_i++] = ' step="' + f.step + '" ';
        }
        if (f.multiple) {
            _h[_i++] = ' multiple="' + f.multiple + '" ';
        }
        if (f.pattern) {
            _h[_i++] = ' pattern="' + f.pattern + '" ';
        }
        if (f.placeholder) {
            _h[_i++] = ' placeholder="' + f.placeholder + '" ';
        }
        if (f.readonly && f.readonly == true) {
            _h[_i++] = ' readonly="' + f.readonly + '" ';
        }
        if (f.disabled && f.disabled == true) {
            _h[_i++] = ' disabled="' + f.disabled + '" ';
        }
        if (f.required && f.required == true) {
            _h[_i++] = ' required="' + f.required + '" ';
        }
        if (f.checked && f.checked == true) {
            _h[_i++] = ' checked="checked" ';
        }
        if (f.align) {
            _h[_i++] = ' align="' + f.align + '" ';
        }
        return (_h.join(''));
    }
    _self._rdroas = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="' + (_self._model.opframe ? _self._model.opframe : 'avopsacts') + '">';
        $.each(_self._model.operations, function (i, o) {
            _h[_i++] = _self._rdroa(o);
        });
        $.each(_self._model.actions, function (i, a) {
            _h[_i++] = _self._rdroa(a);
        });
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rdroa = function (oa) {
        var _h = [], _i = 0;
        _h[_i++] = '<td>';
        _h[_i++] = '<button id="' + oa.id + '" ';
        if (oa.classes) {
            _h[_i++] = ' class="' + oa.classes + '" ';
        }
        if (oa.attrs) {
            $.each(oa.attrs, function (j, a) {
                _h[_i++] = ' ' + a.name + '="' + a.value + '" ';
            });
        }
        _h[_i++] = '>' + oa.label + '</button>';
        _h[_i++] = '</td>';
        return (_h.join(''));
    }

    _self._bind = function () {
        _self._bindbtns();
        _self._bindff();
    }
    _self._bindopts = function () {
        $.each(_self._model.options, function (i, o) {
            _self._bindopt(o);
        });
    }
    _self._bindopt = function (o) {
        var _e = $('#' + o.id, _self._e);
        $(_e).on('change', null, o, function (e) {
            var _v = $(this).find(':selected').val();
            if (o.callback) {
                o.callback({ Change: true, event: e }, _v);
            }
            _self._bChanges = true;
            var coo = _self._getmoo(o);
            $.each(coo, function (i, _o) {
                var d = {};
                d[o.name] = _v;
                _self.GetOption(_o, d);
            });
        });
    }
    _self._bindbtns = function () {
        $.each(_self._model.operations, function (i, o) {
            $(('#' + o.id), _self._e).on('click', null, o, function (e) {
                if ($(this).is('input')) { return; } // TODO: namespace input from buttons
                _self._dooper(o, e);
                e.stopPropagation();
                e.preventDefault();
            });
            if (o.Save) {
                $('input', _self._e).keypress(function (e) {
                    if (e.which == 13) {
                        _self._dooper(o, e);
                    }
                });
            }
        });
        $.each(_self._model.actions, function (i, a) {
            $(('#' + a.id), _self._e).on('click', null, a, function (e) {
                _self._doaction(a);
                e.stopPropagation();
                e.preventDefault();
            });
        });
    }
    _self._bindff = function () {
        $.each(_self._model.fields, function (i, f) {
            $('#' + f._id, _self._e).attr('data-toggle', 'tooltip');
            if (f.master) {
                $('#' + _self._pfx + f.master, _self._e).on('blur', function (e) {
                    $('#' + f._id, _self._e).val($(this).val());
                });

            }
            if (f.type && f.type == 'date') {
                $('#' + f._id, _self._e).datepicker({ format: "mm/dd/yyyy", autoclose: true });
            }
        });
        $('input', _self._e).on('change', function (e) {
            _self._bChanges = true;
        });
        $('textarea', _self._e).keypress(function (e) {
            _self._bChanges = true;
        });
        if (_self._model.bAutoFocus) {
            $('input', _self._e).focus(function (e) {
                _self.ScrollTo(this.id);
            });
            $('textarea', _self._e).focus(function (e) {
                _self.ScrollTo(this.id);
            });
        }
    }
    _self._binderrs = function () {
        var _vs = _self.ViewState();
        if (_vs) {
            if (_vs.InvalidFields) {
                $.each(_vs.InvalidFields, function (i, f) {
                    $('#' + _self._pfx + f, _self._e).attr('title', _vs.UserMessages[i]).addClass('validationError').tooltip();
                });
            }
        }
    }

    _self.ScrollTo = function (n) {
        $(window).scrollTop(($(_self._pfx + '#' + n, _self._e).position().top - 200));
    }

    _self.GetOptions = function (bForce) {
        _self._optcnt = 0;
        if (_self._options.length) {
            $(_self._e).on("qrmgEditor:optsdone", function (e) {
                _self.Load();
            });
            $.each(_self._options, function (i, o) {
                if ((!o.master) && (! o.NoLoad)) {
                    _self.GetOption(o);
                }
            });
        }
        else {
            _self.Load();
        }
    }
    _self.GetOption = function (o, d) {
        o.bLoading = true;
        _self._optcnt += 1;
        var _ud = o;
        var _d = _self._optargs(o);
        var _ctx = _self._getctx();
        $.extend(_d, _ctx);
        if (d) { $.extend(_d, d); }
        $('#' + o.id).empty().prop('disabled', 'disabled').text('Loading ...').append('<option value="-1"><span class="italic">Loading ...</span></option>');
        var _questio = new qrmgio(_self._ropt, _ud);
        if (o.mask) {
            Mask(o.mask);
        }
        _questio._ajax('get', 'json', o.uri, _d);
    }
    _self._optargs = function (o) {
        var _d = {};
        if (o.args) {
            $.each(o.args, function (i, a) {
                if ($.isPlainObject(a)) {
                    $.extend(_d, a);
                }
                else {
                    _d[a.name] = $('#' + a.element).val();
                }
            });
        }
        else {
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
        }
        return (_d);
    }
    _self._ropt = function (ud, Data) {
        if (IsUserMessage(Data)) {
            _self._optcnt -= 1;
            if (!_self._optcnt) {
                $(_self._e).trigger("qrmgEditor:optsdone");
            }
            _self._docallback(ud, Data)
            if (ud.mask) {
                _self.Unmask(ud.mask);
            }
            return;
        }
        if (ud.bNoLoad) {
            if (ud.callback) {
                ud.callback(ud, Data);
                if (ud.mask) {
                    _self.Unmask(ud.mask);
                }
                return;
            }
        }
        _self._loadOptions(ud, Data);
        _self._optcnt -= 1;
        if (!_self._optcnt) {
            $(_self._e).trigger("qrmgEditor:optsdone");
        }
        if (ud.mask) {
            _self.Unmask(ud.mask);
        }
    }
    _self._loadOptions = function (opt, Data) {
        var _s = $('select[id="' + opt.id + '"]', _self._e).val();
        $('select[id="' + opt.id + '"]', _self._e).unbind().empty();
        var i = 0, h = [];
        $.each(Data, function (i, d) {
            if (d.Image) {
                h[i++] = '<option value="' + d.Id + '" style="background-image:url(' + d.Image + ');">' + d.Label + '</option>';
            }
            else {
                h[i++] = '<option value="' + d.Id + '">' + d.Label + '</option>';
            }
        });
        $('select[id="' + opt.id + '"]', _self._e).append(h.join(''));

        if (_s !== undefined) {
            $('select[id="' + opt.id + '"]', _self._e).val(_s);
        }
        if (_self._optdetails(opt).length > 0) {
            $('#' + opt.id).trigger("change");
        }
        if (opt.callback) {
            if (opt.callback({ PostLoad: true }, Data)) {
                return;
            }
        }
        $('#' + opt.id).removeProp('disabled');
        _self._bindopt(opt);
        opt.bLoading = false;
    }
    _self._optdetails = function (opt) {
        var _detopts = [];
        $.each(_self._options, function (i, o) {
            if (o.master && o.master == opt.name) {
                _detopts.push(o);
            }
        });
        return (_detopts);
    }
    _self._getop = function (name) {
        var _o = null;
        $.each(_self._model.operations, function (i, o) {
            if (o.name == name) {
                _o = o;
                return (false);
            }
        });
        return (_o);
    }
    _self._getopt = function (name) {
        var _o = null;
        $.each(_self._options, function (i, o) {
            if (o.name == name) {
                _o = o;
                return (false);
            }
        });
        return (_o);
    }
    _self._getmoo = function (o) {
        var _oo = [];
        $.each(_self._options, function (i, _o) {
            if (_o.master && _o.master == o.name) { _oo.push(_o); }
        });
        return (_oo);
    }

    _self._dooper = function (o, e) {
        _self.Mask(_self._mask);
        if (o.Validate) {
            if (!_self.Validate()) {
                _self.Unmask();
                return;
            }
        }
        var _d = _self.GetData();
        if (_self._docallback(o, _d)) {
            _self.Unmask();
            return;
        }
        if (o.callback) {
            if (o.callback(o, _d)) {
                _self.Unmask();
                return;
            }
        }
        if (o.Save) {
            _self.Save(o);
        }
        else if (o.Clear) {
            _self.Clear();
            ClearUserMessage();
            RemoveQSParam("Id");
            _self.Unmask();
        }
        else if (o.Delete) {
            _self.Delete();
        }
        else if (o.Cancel) {
            _self.Cancel(o);
        }
        else {
            ClearUserMessage();
            bUnmask = false;
            try {
                var _url = (o.url ? o.url : null);
                if (! _url) {
                    _url = (o.uri ? o.uri : _self._model.uri) + '/' + o.name;
                }
                var _cxt = _self._getctx();
                var _io = new qrmgio(_self._roper);
                if (o.action) {
                    _io.GetJSON(_url, _cxt, o);
                }
                else if (o.view) {
                    _io.ShowView(_url, _cxt, o);
                }
                else {
                    var _d = _self.GetData();
                    _io.PostJSON(_url, _d, o);
                }
            }
            catch (e) {
                alert('F|Error doing operation ' + o.name);
            }
        }
    }
    _self.Save = function (o, bNV) {
        if (!_self._bChanges) {
            _self.Validate();
            _self.Unmask();
            return;
        }
        var bV = true;
        if (!bNV) {
            bV = _self.Validate()
        }
        if (!bV) {
            _self.Unmask();
            return;
        }
        var _d = _self.GetData();
        var _evt = _self._getevt("BeforeSave");
        if (_evt != null) {
            if (_evt.callback({ BeforeSave: true }, _d)) {
                _self.Unmask();
                return;
            }
        }

        var _url = (o.uri ? o.uri : _self._model.uri) + '/Save';
        var _io = new qrmgio(_self._roper);
        if (o.type == 'submit') {
            _io.PostView(_url, _d, o);
        }
        else {
            ////_io.PostJSON(_url, _d, o);
            _io._postJSON(_url, _d, o);
        }
    }


    _self.Clear = function (bUM, bForce) {
        if (!bForce && _self._bChanges) {
            if (!confirm('Are you sure?  You will lose your change')) {
                return;
            }
        }
        $(_self._e).find('.validationError').removeClass('validationError');
        var _d = _self.GetData();
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
                $(f).val(-1);
            }
            else if ($(f).prop('type') == 'select-multiple') {
                $(f).val([]);
            }
            else if ($(f).is(':radio')) {
                $(f).prop('checked', false);
            }
            else {
                $(f).val('');
            }
        });
        $(_ff).removeAttr('title');
        _self._bChanges = false;
        _self._bROS = false;

        $(':input:enabled:visible:not([readonly]):first', _self._e).focus();
        if (bUM) {
            ClearUserMessage();
        }
        _self._docallback({ PostClear: true }, _d);
    }
    _self.Cancel = function (o) {
        if (_self._bChanges) {
            $('#btnCancel', '#mdlgLoseChanges').unbind('click').on('click', function (e) {
                return;
            });
            $('#btnDiscard', '#mdlgLoseChanges').unbind('click').on('click', function (e) {
                _self._cancel(o);
            });
            $('#mdlgLoseChanges').modal('show');
        }
        else {
            _self._cancel(o);
        }
    }
    _self._cancel = function (o) {
        _self.Mask();
        try {
            var _url = (o.uri ? o.uri : _self._model.uri) + '/' + o.name;
            var _ctx = _self._getctx();
            var _io = new qrmgio(_self._roper);
            _io.ShowView(_url, _ctx, o);
        }
        catch (e) {
            DisplayUserMessage('F|Error doing operation ' + o.name);
            _self.Unmask();
        }
    }
    _self.Read = function (id, bRO) {
        _self.Mask();
        var _d = {};
        var _ud = { Read: true };
        if (bRO) { _ud.bRO = true }
        var _ctx = _self._getctx();
        $.extend(_d, _ctx);
        _d[_self._pkey.name] = id;
        var _url = _self._model.uri + '/Read';
        var _io = new qrmgio(_self._roper);
        _io.GetJSON(_url, _d, _ud);
    }
    _self.Delete = function () {
        if (_self._bChanges) {
            $('#btnCancel', '#mdlgDelChanges').unbind('click').on('click', function (e) {
                return;
            });
            $('#btnRefresh', '#mdlgDelChanges').unbind('click').on('click', function (e) {
                var id = _self._getkeyv();
                _self.Mask();
                _self.Read(id, true);
            });
            $('#mdlgDelChanges').modal('show');
        }
        else {
            $('#btnCancel', '#mdlgConfirmDelete').unbind('click').on('click', function (e) {
                return;
            });
            $('#btnConfirm', '#mdlgConfirmDelete').unbind('click').on('click', function (e) {
                _self._delete();
            });
            $('#mdlgConfirmDelete').modal('show');
        }
    }
    _self._delete = function () {
        if (!_self._bROS) {
            return;
        }
        var _d = _self.GetData();
        var _url = _self._model.uri + '/Delete';
        var _io = new qrmgio(_self._roper);
        _io.PostJSON(_url, _d, { Delete: true });
    }
    _self._roper = function (ud, d) {
        if (ud.bRO && IsAppSuccess(d)) { }
        else {
            DisplayUserMessage(d, _self);
        }
        if (_self._docallback(ud, d)) {
            _self.Unmask();
            return;
        }
        if (ud.Save) {
            if (IsAppSuccess(d)) {
                var _evt = _self._getevt("AfterSave");
                if (_evt != null) {
                    if (_evt.callback({ AfterSave: true }, d)) {
                        return;
                    }
                }
                _self.Read(d.Id, true);
            }
        }
        else if (ud.Read) {
            if (IsAppSuccess(d)) {
                _self._bChanges = false;
                _self._load(d, false, (ud.bRO ? false : true));
                _self._bROS = true;
            }
        }
        else if (ud.Delete) {
            if (IsAppSuccess(d)) {
                _self._bROS = false;
                _self._bChanges = false;
                var _url = window.location.href;
                RemoveQSParam("Id");
                _self.Clear();
                $('#' + _self._pkey._id, _self._e).val('');
            }
        }
        _self.Unmask();
    }

    _self._doaction = function (a) {
        _self.Mask();
        try {
            var _url = (a.uri ? a.uri : _self._model.uri) + '/' + a.name;
            // TODO: args
            var _io = new qrmgio(_self._raction, a);
            _io.ShowView(_url);
        }
        catch (e) {
            DisplayUserMessage('F|EXCEPTION: performing action ' + a.name);
            _self.Unmask();
        }
    }
    _self._raction = function () {
        DisplayUserMessage('F|Error retruning from action ' + a.name);
    }

    _self.GetData = function () {
        var _d = {};
        var _ii = $(_self._e).find('input');
        $.each(_ii, function (i, _i) {
            var n = $(_i).attr('id').substr(_self._pfx.length);
            var _t = $(_i).attr('type');
            var _v = null;
            if (_t == 'checkbox') {
                _v = $(_i).is(':checked') ? 'True' : 'False';
            }
            else if (_t == 'radio') {
                // TODO
            }
            else {
                _v = $(_i).val();
            }
            _d[n] = _v;
        });
        var _ss = $(_self._e).find('select');
        $.each(_ss, function (i, _s) {
            var _n = $(_s).attr('id').substr(_self._pfx.length);
            var _v = $(_s).val();
            _d[_n] = _v;
        });
        var ttaa = $(_self._e).find('textarea');
        $.each(ttaa, function (i, ta) {
            var n = $(ta).attr('id').substr(_self._pfx.length);
            var _v = $(ta).val();
            _d[n] = _v;
        });
        var _ctx = _self._getctx();
        $.extend(_d, _ctx);
        return (_d);
    }
    _self.Validate = function () {
        $(_self._e).find('.validationError').removeClass('validationError');
        $.each(_self._model.fields, function (i, f) {
            if (f.required && f.required == true) {
                if (_self._mt(f)) {
                    $('#' + f._id, _self._e).addClass('validationError');
                }
            }
            if (f.type) {
                if (f.type == 'email') {
                    if (f.required && f.required == true) {
                        if (!bValidEmailAddress($('#' + f._id, _self._e).val())) {
                            $('#' + f._id, _self._e).addClass('validationError');
                        }
                    }
                    else {
                        if ($('#' + f._id, _self._e).val().trim().length > 0) {
                            if (!bValidEmailAddress($('#' + f._id, _self._e).val())) {
                                $('#' + f._id, _self._e).addClass('validationError');
                            }
                        }
                    }
                }
                else if (f.type == 'number') {
                    if (!bValidNumber($('#' + f._id, _self._e).val())) {
                        if ($('#' + f._id, _self._e).val().trim().length > 0) {
                            $('#' + f._id, _self._e).addClass('validationError');
                        }
                    }
                }
            }
        });
        $.each(_self._options, function (i, o) {
            if (o.required && o.required == true) {
                if (_self._mto(o)) {
                    $('#' + o.id, _self._e).addClass('validationError');
                }
            }
        });
        var _pcf = _self._getfbt('confirmpassword');
        if (_pcf) {
            var _pf = _self._getfbt('password');
            if (_pf) {
                if ($('#' + _pf.id, _self._e).val() != $('#' + _pcf.id, _self._e).val()) {
                    $('#' + _pf.id, _self._e).addClass('validationError');
                    $('#' + _pcf.id, _self._e).addClass('validationError');
                }
            }
        }
        return ($(_self._e).find('.validationError').length == 0);
    }
    _self._mt = function (f) {
        var _e = $('#' + f._id, _self._e);
        if (_e.length == 0) { return; }
        var _v = $('#' + f._id, _self._e).val();
        return (_v ? ($('#' + f._id, _self._e).val().trim().length == 0) : true);
    }
    _self._mto = function (o) {
        var _e = $('#' + o.id, _self._e);
        if (_e.length == 0) { return; }
        return ($('#' + o.id + ' option:selected', _self._e).length == 0 || $('#' + o.id + ' option:selected', _self._e).attr('value') == -1);
    }
    _self._getfbt = function (ft) {
        var _f;
        $.each(_self._model.fields, function (i, f) {
            if (f.type == ft) {
                _f = f;
                return (false);
            }
        });
        return (_f);
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

    _self.SetField = function (n, v, t) {
        var f = _self.GetField(n);
        if (!f) { return; }
        var _f = $('#'+f._id, _self._e);
        _self._setf(_f, v, t);
    }
    _self._setf = function (f, v, t) {
        if ($(f).is(':text')) {
            $(f).val(v);
        }
        else if ($(f).is(':checkbox')) {
            $(f).prop('checked', v);
        }
        else if ($(f).prop('type') == 'select-one') {
            if (t) { 
                $('#' + $(f).attr('id') + ' option').filter(function () { return $(this).html() == t; }).prop('selected', 'selected');
            }
            else {
                $(f).find('option[value="' + v + '"]').prop('selected', true);
            }
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

    _self._docallback = function (ud, d) {
        if (_self._cb) {
            return (_self._cb(ud, d));
        }
    }

    _self.ViewState = function () {
        var _n = '__' + $(_self._e).attr('id') + 'VIEW_STATE';
        if ($('#' + _n).length != 1) { return (null); }
        var _vs = $.parseJSON($('#' + _n).val());
        if (_vs.questStatus && _vs.questStatus.Severity == -1) { return (null); }
        return (_vs);
    }
    _self.Load = function (Data, bForce) {
        var _dd;
        _self.Mask();
        if (!Data) {
            var _vs = _self.ViewState();
            if (!_vs) {
                _self._docallback({ PostLoad: true }, Data);
                _self.Unmask();
                return;
            }
            if (_vs) {
                if (_vs.questStatus && _vs.questStatus.Severity == 2 && _vs.Id > 0) {
                    _self.Read(_vs.Id, true);
                }
                else {
                    _self._load(_vs);
                    _self.Unmask();
                }
            }
        }
        else {
            _self._load(Data, bForce);
            _self.Unmask();
        }
    }
    _self._load = function (Data, bForce, bUM) {
        _self.Clear(bUM, bForce);
        for (var f in Data) {
            var _v = Data[f] || '';
            var _e = $('#' + _self._pfx + f, _self._e);
            if ($(_e).length == 1) {
                if ($(_e).is(':checkbox')) {
                    _self._afot(_v) ? $(_e).prop('checked', 'checked') : $(_e).removeAttr('checked');
                }
                else if ($(_e).is('select')) {
                    $(_e).find('option[value="' + (_v ? _v : -1) + '"]').prop('selected', true);
                }
                else {
                    $(_e).val(_v);
                }
                if (f == 'Id') {
                    _self._bROS = f > 0;
                }
            }
        }
        _self._bChanges = false;
        ////ClearMessage();

        var _evt = _self._getevt("AfterLoaded");
        if (_evt != null) {
            _evt.callback({ AfterLoaded: true }, Data);
        }
        _self._binderrs();
    }
    _self._afot = function (v) {
        var __afot = ['True', 'true', 'Yes', 'yes', '1', true];
        return ($.inArray(v, __afot) > -1);
    }

    _self.DisplayFormMessages = function () {
        if (!__umsg) { return; }
        __umsg.DisplayFormMessages(_self);
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
    _self._getkeyv = function () {
        return($('#' + _self._pkey._id, _self._e).val());
    }
    _self._getevt = function (evt) {
        var _e = null;
        $.each(_self._model.events, function (i, e) {
            if (e.name === evt) {
                _e = e;
                return (false);
            }
        });
        return (_e);
    }

    _self.Change = function(v) {
        _self._bChanges = v;
        var _evt = _self._getevt("OnChange");
        if (_evt != null) {
            if (_evt.callback({ OnChange: true }, _self._bChanges)) {
                return;
            }
        }
    }
    _self.bChanges = function() {
        return (_self._bChanges);
    }
    _self.LoadOptions = function (o, oo) {
        var _o = _self._getopt(o.name);
        if (!_o) { return; }
        return(_self._loadOptions(_o, oo));
    }

    _self.Mask = function (e, msg, bForce) {
        if (!bForce) {
            if (!_self._bMasking) { return; }
        }
        Mask(e || _self._mask, null, msg);
        _self.Buttons(false);
    }
    _self.Unmask = function (e, bCM, bForce) {
        if (!bForce) {
            if (!_self._bMasking) { return; }
        }
        Unmask(e || _self._mask, null, bCM);
        _self.Buttons(true);
    }
    _self.Buttons = function (bDisable) {
        bDisable ? $('#frmTableset .avopsacts button').removeAttr('disabled') : $('#frmTableset .avopsacts button').attr('disabled', 'disabled');
    }

    _self._init();
}
