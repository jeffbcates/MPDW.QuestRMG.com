// qrmgForm.js
function qrmgForm(model) {
    var _self = this;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._model = model;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._cb = model.callback ? model.callback : null;
    _self._options = [];

    _self._init = function () {
        _self._initff();
        _self._initops();
        _self._initacts();
        _self._initopts();
        _self._render();
        _self._bind();
        _self.GetOptions();
    }
    _self._initff = function () {
        $.each(_self._model.fields, function (i, f) {
            f.id = _self._pfx + f.name;
        });
    }
    _self._initops = function () {
        $.each(_self._model.operations, function (i, o) {
            o.id = _self._pfx + o.name;
            if (o.submit) {
                _self._action = _self._uri + o.name;
            }
        });
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
    }
    _self._bindopts = function () {
        $.each(_self._model.options, function (i, o) {
            _self._bindopt(o);
        });
    }
    _self._bindopt = function (o) {
        var _e = $('#' + o.id, _self._e);
        $(_e).on('change', null, o, function (e) {
            _self._bChanges = true;
            var coo = _self._getmoo(o);
            var _v = $(this).find(':selected').val();
            $.each(coo, function (i, _o) {
                var d = {};
                d[o.name] = _v;
                _self.GetOption(_o, d);
            });
        });
    }
    _self._render = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<form role="form" method="post" action="' + _self._action + '" class="form-horizontal">';
        _h[_i++] = _self._rdrff();
        _h[_i++] = '</form>';
        _h[_i++] = _self._rdroas();
        $(_self._e).empty().append(_h.join(''));
        $(':input:enabled:visible:not([readonly]):first', _self._e).focus();
    }
    _self._rdrff = function () {
        var _h = [], _i = 0;
        $.each(_self._model.fields, function (i, f) {
            if (f.type && f.type == 'hidden') {
            }
            else {
                _h[_i++] = '<div class="form-group">';
                _h[_i++] = _self._rdrlbl(f);
            }
            _h[_i++] = _self._rdrf(f);
            if (f.type && f.type == 'hidden') {
            }
            else {
                _h[_i++] = '</div>';
            }
        });
        return (_h.join(''));
    }
    _self._rdrlbl = function (f) {
        var _h = [], _i = 0;
        _h[_i++] = '<label for="' + f.id + '" class="control-label">';
        if (f.label) {
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
            _h[_i++] = '<input type="text" id="' + f.id + '" name="' + f.id + '" class="form-control" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        if (f.type == 'hidden') {
            _h[_i++] = '<input type="hidden" id="' + f.id + '" name="' + f.id + '" class="form-control" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'checkbox') {
            _h[_i++] = '<input type="checkbox" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'select') {
            _h[_i++] = '<select type="checkbox" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="form-control"></select>';
        }
        else if (f.type == 'radio') {
            _h[_i++] = '<input type="radio" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'textarea') {
            _h[_i++] = '    <textarea id="' + f._id + '" class="form-control" rows="' + (f.rows ? f.rows : '4') + '" ' + (f.readonly ? ' readonly="readonly" ' : '') + '></textarea>';
        }
        else if (f.type == 'date') {
            _h[_i++] = '<input type="text" id="' + f.id + '" name="' + f.id + '" class="form-control" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        else if (f.type == 'email') {
            _h[_i++] = '<input type="email" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="form-control" />';
        }
        else if (f.type == 'phone') {
            _h[_i++] = '<input type="phone" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="form-control" />';
        }
        else if (f.type == 'password' || f.type == 'confirmpassword') {
            _h[_i++] = '<input type="password" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' class="form-control" />';
        }
        else if (f.type == 'file') {
            _h[_i++] = '<input type="file" id="' + f.id + '" name="' + f.id + '" ';
            _h[_i++] = _self._rdrfo(f);
            _h[_i++] = ' />';
        }
        return (_h.join(''));
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
        _h[_i++] = '<div class="avopsacts">';
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
        _h[_i++] = '>' + oa.name + '</button>';
        _h[_i++] = '</td>';
        return (_h.join(''));
    }

    _self._bind = function () {
        _self._bindbtns();
        _self._bindff();
    }
    _self._bindbtns = function () {
        $.each(_self._model.operations, function (i, o) {
            $(('#' + o.id), _self._e).on('click', null, o, function (e) {
                _self._dooper(o);
                e.stopPropagation();
                e.preventDefault();
            });
            if (o.submit) {
                $('input', _self._e).keypress(function (e) {
                    if (e.which == 13) {
                        _self._dooper(o);
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
            $('#' + f.id, _self._e).attr('data-toggle', 'tooltip');
            if (f.master) {
                $('#' + _self._pfx + f.master, _self._e).on('blur', function (e) {
                    $('#' + f.id, _self._e).val($(this).val());
                });

            }
            if (f.type && f.type == 'date') {
                $('#' + f.id, _self._e).datepicker({ format: "mm/dd/yyyy", autoclose: true });
            }
        });
        $('input', _self._e).on('change', function (e) {
            _self._bChanges = true;
        });
        $('textarea', _self._e).keypress(function (e) {
            _self._bChanges = true;
        });
        $('input', _self._e).focus(function (e) {
            _self.ScrollTo(this.id);
        });
        $('textarea', _self._e).focus(function (e) {
            _self.ScrollTo(this.id);
        });
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
            $(_self._e).on("qrmgForm:optsdone", function (e) {
                _self.Load();
            });
            $.each(_self._options, function (i, o) {
                if (!o.master) {
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
        if (d) { $.extend(_d, d); }
        $('#' + o.id).empty().prop('disabled', 'disabled').text('Loading ...').append('<option value="-1"><span class="italic">Loading ...</span></option>');
        var _questio = new qrmgio(_self._ropt, _ud);
        _questio._ajax('get', 'json', o.uri, _d);
    }
    _self._optargs = function (o) {
        var _d = {};
        if (o.args) {
            $.each(o.args, function (i, a) {
                _d[a.name] = $('#' + a.value).val();
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
            return (_self._docallback(ud, Data));
            ////var _o = _self._getopt(ud.name);
            ////$('#' + _o.id).removeProp('disabled');
            ////$(_self._e).hasClass('editable') ? $('#' + _o.id).removeProp('disabled') : $('#' + _o.id).prop('disabled', 'disabled');
        }
        _self._loadOptions(ud, Data);
        _self._optcnt -= 1;
        if (!_self._optcnt) {
            $(_self._e).trigger("qrmgForm:optsdone");
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
            if (opt.callback(Data)) {
                return;
            }
        }
        $('#' + opt.id).removeProp('disabled');
        _self._bindopt(opt);
        opt.bLoading = false; _self._optcnt -= 1;
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
    _self._getmoo = function (o) {
        var _oo = [];
        $.each(_self._options, function (i, _o) {
            if (_o.master && _o.master == o.name) { _oo.push(_o); }
        });
        return (_oo);
    }
    _self._dooper = function (o) {
        var bUnmask = true;
        qrmgmvc.Global.Mask(_self._e);
        if (o.submit || (o.validate && o.validate == true)) {
            if (!_self.Validate()) {
                qrmgmvc.Global.Unmask(_self._e);
                return;
            }
        }
        if (o.submit) {
            $(_self._e).find('form').submit();
        }
        else {
            ClearUserMessage();
            bUnmask = false;
            try {
                var _url = (o.uri ? o.uri : _self._model.uri) + '/' + o.name;
                var _d = _self.GetData();
                var _io = new qrmgio(_self._roper);
                if (o.action) {
                    _io.GetSON(_url, _d, o);
                }
                else if (o.view) {
                    _io.ShowView(_url, _d, o);
                }
                else {
                    _io.PostJSON(_url, _d, o);
                }
            }
            catch (e) {
                alert('F|Error doing operation ' + o.name);
            }
        }
        if (bUnmask) {
            qrmgmvc.Global.Unmask(_self._e);
        }
    }
    _self._roper = function (ud, d) {
        DisplayUserMessage(d, _self);
        if (_self._docallback(ud, d)) {
            return;
        }
        // TODO: unmask
    }

    _self._doaction = function (a) {
        // TODO: mask
        try {
            var _url = (a.uri ? a.uri : _self._model.uri) + '/' + a.name;
            // TODO: args
            var _io = new qrmgio(_self._raction);
            _io.ShowView(_url);
        }
        catch (e) {
            alert('F|Error doing action ' + o.name);
        }
    }
    _self._raction = function () {
        alert('F|Error retruning from action ' + o.name);
    }

    _self.GetData = function () {
        var _d = {};
        var _ii = $(_self._e).find('input');
        $.each(_ii, function (i, _i) {
            var n = $(_i).attr('id').substr(_self._pfx.length);
            var _t = $(_i).attr('type');
            var _v = null;
            if (_t == 'checkbox') {
                _v = $(_i).is(':checked') ? $(_i).val() : 'false';
            }
            else {
                _v = $(_i).val();
            }
            _d[n] = _v;
        });
        var _ss = $(_self._e).find('select');
        $.each(_ss, function (i, _s) {
            var _n = $(_s).attr('id').substr(_self._pfx.length);
            var _v = $(_s).find(":selected").attr('id');
            _d[_n] = _v;
        });
        var ttaa = $('#' + _self._model.id).find('textarea');
        $.each(ttaa, function (i, ta) {
            var n = $(ta).attr('id').substr(_self._pfx.length);
            var _v = $(ta).text();
            _d[n] = _v;
        });
        return (_d);
    }
    _self.Validate = function () {
        $(_self._e).find('.validationError').removeClass('validationError');
        $.each(_self._model.fields, function (i, f) {
            if (f.required && f.required == true) {
                if (_self._mt(f)) {
                    $('#' + f.id, _self._e).addClass('validationError');
                }
            }
            if (f.type) {
                if (f.type == 'email') {
                    if (f.required && f.required == true) {
                        if (!bValidEmailAddress($('#' + f.id, _self._e).val())) {
                            $('#' + f.id, _self._e).addClass('validationError');
                        }
                    }
                    else {
                        if ($('#' + f.id, _self._e).val().trim().length > 0) {
                            if (!bValidEmailAddress($('#' + f.id, _self._e).val())) {
                                $('#' + f.id, _self._e).addClass('validationError');
                            }
                        }
                    }
                }
                else if (f.type == 'number') {
                    if (!bValidNumber($('#' + f.id, _self._e).val())) {
                        if ($('#' + f.id, _self._e).val().trim().length > 0) {
                            $('#' + f.id, _self._e).addClass('validationError');
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
        var _e = $('#' + f.id, _self._e);
        if (_e.length == 0) { return; }
        var _v = $('#' + f.id, _self._e).val();
        return (_v ? ($('#' + f.id, _self._e).val().trim().length == 0) : true);
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
        return (_f.label);
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
    _self.Load = function (Data) {
        var _dd;
        if (!Data) {
            var _vs = _self.ViewState();
            if (! _vs) { return; }
            if (_vs.questStatus && _vs.questStatus.Severity == 2 && _vs.Id > 0) {
                _self.Read(_vs.Id);
            }
            else {
                _self._load(_vs);
            }
        }
        else {

        }
    }
    _self._load = function (Data) {
        _self.Clear();
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
        ClearMessage();
        DisplayUserMessage(Data);

        _self._binderrs();
    }
    _self._afot = function (v) {
        var __afot = ['True', 'true', 'Yes', 'yes', '1'];
        return ($.inArray(v, __afot) > -1);
    }

    _self.Clear = function () {
        $(_self._e).find('.validationError').removeClass('validationError');
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
                $(f).val('');
            }
        });
        $(_ff).removeAttr('title');
        $(':input:enabled:visible:not([readonly]):first', _self._e).focus();
    }

    _self.DisplayFormMessages = function () {
        if (!__umsg) { return (false); }
        __umsg.DisplayFormMessages(_self);
    }

    _self._bldurl = function (x) {
        var _url = (x.uri ? x.uri : _self._model.uri) + '/' + x.name;
        var _ctx = _self._getctx();
    }
    _self._getctx = function () {
        if (!_self._model.ctx) { return; }
        var _ctx = {};
        if ($.isArray(_self._model.ctx)) {
            $.each(_self._model.ctx, function (i, c) {
                _ctx[c.name] = $('#' + c.value).val();
            });
        }
        else {
            var _v = $('#' + _self._model.ctx.value);
            _ctx[_self._model.ctx.name] = (_v.length == 1 ? $('#' + _self._model.ctx.value).val() : _self._model.ctx.value);
        }
        return (_ctx);
    }
    _self.bChanges = function()
    {
        return (_self._bChanges);
    }

    _self._init();
}

