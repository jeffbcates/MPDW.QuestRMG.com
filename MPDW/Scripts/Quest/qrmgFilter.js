// qrmgFilter
function qrmgFilter(model) {
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
        _self._initff();
        _self._render();
        _self.GetOptions();
    }
    _self._initctx = function () {
        _self._ctx = new qrmgctx(_self._model.ctx);
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

    _self._render = function () {
        var _e = $(_self._e);
        var _h = [], _i = 0;
        $(_e).empty().append(_h.join('')).addClass('questFilterPanel');
        _self.Droppable();
    }
    _self._rndritm = function (itm, id) {
        var _id = id || ('fltitm' + itm.name);
        var _h = [], _i = 0;
        _h[_i++] = '<div id="' + _id + '" data-id="' + itm.Id + '" class="questFilterItem ' + (itm.bHidden ? 'questFilterItemHidden' : '') + '">';
        _h[_i++] = _self._rndrcol(itm);
        _h[_i++] = _self._rndritmop(itm);
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndritmop = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="questFilterItemOpFrame">';
        _h[_i++] = _self._rndrioo(itm);
        _h[_i++] = _self._rndrivv(itm);
        _h[_i++] = '<div class="fltitmblk fltitmctrl">';
        _h[_i++] = _self._rndrdel(itm);
        _h[_i++] = _self._rndrnew(itm);
        _h[_i++] = '</div>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrdel = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="fltitmop fltitmX">';
        _h[_i++] = '<a href="#" class="fltitmdel" title="Remove this field operation"><span class="fa fa-remove"></span></a>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrnew = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="fltitmop fltitmPlus">';
        _h[_i++] = '<a href="#" class="fltitmnew" title="Add another operation for this field"><span class="fa fa-plus"></span></a>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrioo = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="fltitmblk fltitmoo">';
        _h[_i++] = '<select class="filtsel form-control" title="Select an operation for the specified value(s)">';
        itm.options = itm.options || _self.Options.OperationId;
        $.each(itm.options, function (i, o) {
            _h[_i++] = '    <option value="' + o.Id + '">' + o.Label + '</option>';
        });
        _h[_i++] = '</select>';
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrivv = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="fltitmblk fltitmvv">';
        _h[_i++] = '<div class="filtvalues">';
        _h[_i++] = '    <input type="text" class="filtinput" title="Enter value(s).  RETURN or TAB between multiple values." />';
        _h[_i++] = '</div>';
        _h[_i++] = '<div class="filttags">';
        _h[_i++] = '</div>';
        itm.values = itm.values || [];
        $.each(itm.values, function (i, v) {
            _h = _self._rndriv(v);
        });
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndriv = function (v) {
        var _h = [], _i = 0;
        _h[_i++] = '    <div class="filtvalue">';
        _h[_i++] = '        <span class="filtvaluelbl">' + v.Label + '</span>';
        _h[_i++] = '        <span class="filtvalueX"></span>';
        _h[_i++] = '    </div>';
        return (_h.join(''));
    }
    _self._rndrcol = function (itm) {
        var _h = [], _i = 0;

        _h[_i++] = '<div class="questFilterItemControls">';
        _h[_i++] = '    <span class="fa fa-cog filterItemMenu" data-toggle="dropdown"></span>';
        _h[_i++] = '    <ul class="filterItemDropdownMenu dropdown-menu">';
        _h[_i++] = '        <li class="filterItemLabelEntry">Label ...</li>';
        _h[_i++] = '        <li class="filterItemParameterNameEntry">Parameter Name ...</li>';
        _h[_i++] = '        <li data-toggle="modal" data-target="#mdlJoin">Join ...</li>';
        _h[_i++] = '        <li data-toggle="modal" data-target="#mdlFilter">Filter/Sub-Select ...</li>';
        _h[_i++] = '        <li data-toggle="modal" data-target="#mdlLookup">Lookup ...</li>';
        _h[_i++] = '        <li data-toggle="modal" data-target="#mdlTypeList">Type List ...</li>';
        if (itm.bHidden) {
            _h[_i++] = '        <li class="filterItemVisibility">Visible</li>';
        }
        else {
            _h[_i++] = '        <li class="filterItemVisibility">Hidden</li>';
        }

        _h[_i++] = '    </ul>';
        _h[_i++] = '</div>';

        _h[_i++] = '<div class="questFilterItemSpecifier">';
        _h[_i++] = '<div class="questFilterColumnEntity">' + itm.Parent.text + '</div>';
        _h[_i++] = '<div id="' + itm.name + '" class="questFilterCogItem questFilterColumn">';
        _h[_i++] = itm.name;
        _h[_i++] = '</div>';
        _h[_i++] = _self._rndrlbl(itm);
        _h[_i++] = _self._rndrpnmnm(itm);
        _h[_i++] = _self._rndrjoins(itm);
        _h[_i++] = _self._rndrLookup(itm);
        _h[_i++] = _self._rndrTypeList(itm);
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrlbl = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="questFilterCogItem questFilterItemLabel">';
        if (itm.Label) {
            _h[_i++] = '<div class="questFilterItemCogLabel">Label:&nbsp;</div>' + '<div class="questFilterItemCogInput questFilterItemLabelText"><input type="text" title="Enter an optional label for results." class="filtinput fltitmlbl" value="' + itm.Label + '"></div>';
        }
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrpnmnm = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="questFilterCogItem questFilterItemParameterName">';
        if (itm.ParameterName) {
            _h[_i++] = '<div class="questFilterItemCogLabel">Param:&nbsp;</div>' + '<div class="questFilterItemCogInput questFilterItemParameterNameText"><input type="text" title="Enter an optional parameter name for stored procedures." class="filtinput fltitmlbl" value="' + itm.ParameterName + '"></div>';
        }
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrjoins = function (itm) {
        var _h = [], _i = 0;
        _h[_i++] = '<div class="questFilterCogItem questFilterJoinFrame">';
        if (itm.Joins) {
            $.each(itm.data.Joins, function (i, j) {
                _h[_i++] = '<div class="questFilterItemJoin" data-id="' + j.ColumnId + '">';
                _h[_i++] = j.JoinType + '&nbsp' + j.Identifier;
                _h[_i++] = '</div>';
            });
        }
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrLookup = function (itm) {
        var _h = [], _i = 0;
        if (!itm.Lookup || itm.Lookup.Id == 0) {
            _h[_i++] = '<div class="questFilterCogItem questFilterItemLookup">';
        }
        else {
            _h[_i++] = '<div class="questFilterCogItem questFilterItemLookup" data-lookupid="' + itm.Lookup.Id + '">';
            _h[_i++] = "Lookup: " + itm.Lookup.Name;
        }
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrTypeList = function (itm) {
        var _h = [], _i = 0;
        if (!itm.TypeList || itm.TypeList.Id == 0) {
            _h[_i++] = '<div class="questFilterItemTypeList">';
        }
        else {
            _h[_i++] = '<div class="questFilterItemTypeList" data-typelistid="' + itm.TypeList.Id + '">';
            _h[_i++] = "TypeList: " + itm.TypeList.Name;
        }
        _h[_i++] = '</div>';
        return (_h.join(''));
    }

    _self._bnditm = function () {
        _self._bndops();
        _self._bndddm();
        _self._bndlbl();
        _self._bndprmnm();
        _self._bnditmvis();
        _self._bnditmctrl();
        _self._bnditmvv();
    }
    _self._bndops = function () {
        $('.filtsel', _fltFilterItems._e).unbind('change').bind('change', function () {
            _self.Change(true);
        });
    }
    _self._bndddm = function () {
        $('.questFilterItemControls', _self._e).on('click', function (e) {
            if ($(this).hasClass('open')) {
                $(this).closest('.questFilterItem').removeAttr('style');
            }
            else if ($(this).closest('.questFilterItem').height() < 210) {
                $(this).closest('.questFilterItem').height(210);
            }
        });
    }
    _self._bndlbl = function () {
        $('.filterItemLabelEntry', _self._e).unbind('click').on('click', function (e) {
            if ($(this).closest('.questFilterItem').find('.questFilterItemSpecifier .questFilterItemLabel').find('input').length == 0) {
                $(this).closest('.questFilterItem').find('.questFilterItemSpecifier .questFilterItemLabel').append('<div class="questFilterItemCogLabel">Label:&nbsp;</div>' + '<div class="questFilterItemCogInput questFilterItemLabelText"><input type="text" title="Enter an optional label for results." class="filtinput fltitmlbl"></div>');
                $('.fltitmlbl', _fltFilterItems._e).unbind('keypress').on('keypress', function () {
                    _self.Change(true);
                });
            }
            else {
                var fi = $(this).closest('.questFilterItem');
                $(fi).find('.questFilterItemSpecifier .questFilterItemLabel').empty();
                var _d = _self._getdata(fi);
                delete _d.Label;
                _self.Change(true);
            }
        });
    }
    _self._bndprmnm = function () {
        $('.filterItemParameterNameEntry', _self._e).unbind('click').on('click', function (e) {
            if ($(this).closest('.questFilterItem').find('.questFilterItemSpecifier .questFilterItemParameterName').find('input').length == 0) {
                $(this).closest('.questFilterItem').find('.questFilterItemSpecifier .questFilterItemParameterName').append('<div class="questFilterItemCogLabel">Param:&nbsp;</div>' + '<div class="questFilterItemCogInput questFilterItemParameterNameText"><input type="text" title="Enter an optional parameter name for stored procedures." class="filtinput fltitmlbl"></div>');
                $('.fltitmlbl', _fltFilterItems._e).unbind('keypress').on('keypress', function () {
                    _self.Change(true);
                });
            }
            else {
                var fi = $(this).closest('.questFilterItem');
                $(fi).find('.questFilterItemSpecifier .questFilterItemParameterName').empty();
                var _d = _self._getdata(fi);
                delete _d.ParameterName;
                _self.Change(true);
            }
        });
    }
    _self._bnditmvis = function () {
        $('.filterItemVisibility', _self._e).unbind('click').on('click', function (e) {
            if ($(this).text() == "Hidden") {
                $(this).text("Visible");
                $(this).closest('.questFilterItem').addClass("questFilterItemHidden");
            }
            else {
                $(this).text("Hidden");
                $(this).closest('.questFilterItem').removeClass("questFilterItemHidden");
            }
            _self.Change(true);
        });
    }
    _self._bnditmctrl = function () {
        $('.fltitmop a', _self._e).unbind('click').on('click', function (e) {
            if ($(e.currentTarget).hasClass('fltitmdel')) {
                _self.Change(true);
                if ($(e.currentTarget).closest('.questFilterItem').find('.questFilterItemOpFrame').length == 1) {
                    $(e.currentTarget).closest('.questFilterItem').remove();
                    return;
                }
                if ($(e.currentTarget).closest('.questFilterItemOpFrame').find('a.fltitmnew').length) {
                    var _h = _self._rndrnew();
                    $(e.currentTarget).closest('.questFilterItemOpFrame').prev('.questFilterItemOpFrame').find('.fltitmctrl').append(_h);
                }
                $(e.currentTarget).closest('.questFilterItemOpFrame').remove();
            }
            else if ($(e.currentTarget).hasClass('fltitmnew')) {
                _self.Change(true);
                _self.NewOperation({ name: $(e.currentTarget).closest('.questFilterItem').attr('id').substr(6) });
            }
        });
    }
    _self._bnditmvv = function () {
        $('.filtinput', _self._e).unbind('focus').on('focus', function (e) {
        });
        $('.filtinput', _self._e).unbind('blur').on('blur', function (e) {
            ClearUserMessage();
        });
        $('.filtinput', _self._e).unbind('keydown').on('keydown', function (e) {
            if (e.which == 9) {
                if (this.value == "") { return; }
                e.stopPropagation();
                e.preventDefault();
                _self._tagv(this);
            }
        });
        $('.filtinput', _self._e).unbind('keyup').on('keyup', function (e) {
            if (e.which == 13) {
                if (this.value == "") { return; }
                e.stopPropagation();
                e.preventDefault();
                _self._tagv(this);
            }
            _self.Change(true);
        });
    }
    _self._tagv = function (e) {
        var _v = $(e).val();
        var _h = [], _i = 0;
        _h[_i++] = '<div class="filtvtag">' + _v + '</div><div class="filtvx">X</div>';
        $(e).closest('.fltitmvv').find('.filttags').append(_h.join(''));
        $('.filtvx', _self._e).unbind('click').on('click', function (e) {
            $(this).prev().remove();
            this.remove();
        });
        $(e).val('');
    }

    _self.Droppable = function (b) {
        if (_self._model.droppable) {
            $(_self._e).droppable({
                drop: function (e, ui) {
                    _self.Change(true);
                    var _evt = _self._getevt("OnDrop");
                    if (_evt != null) {
                        if (_evt.callback("OnDrop", ui)) {
                            return;
                        }
                    }
                }
            });
        }
    }
    _self._getevt = function (evt) {
        var _e = null;
        $.each(_self._model.events, function (i, e) {
            if (e.name == evt) {
                _e = e;
                return (false);
            }
        });
        return (_e);
    }

    _self.Insert = function (itm) {
        if (!itm.key) { return; }
        var _id = 'fltitm' + itm.key;
        var _h = [], _i = 0;
        if (itm.Parent) {
            _h[_i++] = _self._rndritm(itm, _id);
            $(_self._e).append(_h.join(''));
            if (itm.Operations) {
                $.each(itm.Operations, function (i, op) {
                    if (!i) { return; }
                    op.name = itm.key;
                    _self.NewOperation(op);
                });
            }
            if (itm.data) {
                $('#' + _id, _self._e).data(_id, itm.data);
            }
            _self._inititm(itm);
            _self._bnditm();
        }
        else {
            $.each(itm.data.nodes, function (i, n) {
                var pn = _self._model.source.GetNode(n.ParentId);
                var lbl = _self._mklbl(n);
                var _key = pn.text + '.' + lbl;
                _key = _key.replace(/[\[\]']+/g, '').replace(/\./g, '_');
                var itm = { Id: n.Id, key: _key, name: lbl, title: lbl, data: n, Parent: pn, Operations: n.Operations, Joins: n.Joins, Label: n.Label, ParameterName: n.ParameterName, bHidden: n.bHidden };
                _self.Insert(itm);
            });
        }
    }
    _self._mklbl = function (n) {
        var lbl = n.text;
        var char;
        if (lbl.indexOf('.') > -1) {
            char = '.';
        }
        else if (lbl.indexOf(':') > -1) {
            char = ':';
        }
        else {
            return (null); // force exception.
        }
        var pp = lbl.split(char);
        lbl = pp[0].trim();
        return (lbl);
    }

    _self.NewOperation = function (itm) {
        if (!itm.name) { return; }
        var _h = [], _i = 0;
        _h[_i++] = _self._rndritmop(itm);
        var e = _self._getitm(itm.name);
        if (!e) { return; }
        $(e).find('.fa-plus').closest('.fltitmop').remove();
        $(e).append(_h.join(''));
        $(e).find('.questFilterItemOpFrame:last').prev('.questFilterItemOpFrame').find('.fltitmoo').append('<div class="questFilterOR">OR</div>');
        _self._bnditm();
    }
    _self._getitm = function (name) {
        var e = $('div[id="fltitm' + name + '"]', _self._e);
        return (e);
    }
    _self._inititm = function (itm) {
        if (!itm.Operations) { return; }
        $.each(itm.Operations, function (i, op) {
            var oo = $('.questFilterItemOpFrame', '#fltitm' + itm.key);
            $('.fltitmoo select.filtsel', oo[i]).val(op.Operator);
            $.each(op.Values, function (j, v) {
                var _input = $('.fltitmvv .filtvalues input.filtinput', oo[i]);
                $(_input).val(v.Value);
                _self._tagv(_input);
            });
        });
    }

    _self.GetOptions = function () {
        _self._optcnt = 0;
        if (_self._options.length) {
            $.each(_self._options, function (i, f) {
                _self.LoadOptions(f);
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
                _self._docallback({ PostInit: true }, Data);
            }
        }
    }
    _self._loadOptions = function (f, Data) {
        _self.Options = _self.Options || {};
        _self.Options[f.name] = Data;
    }

    _self.GetData = function () {
        var data = {};
        data = [];
        $.each($('.questFilterItem', _fltFilterItems._e), function (i, fi) {
            var item = {};
            item.Table = $(fi).find('.questFilterItemSpecifier .questFilterColumnEntity').text();
            item.Name = $(fi).find('.questFilterItemSpecifier .questFilterColumn').text();
            item.Operations = [];
            item.Entity = _self._getentity(fi);
            $.each($(fi).find('.questFilterItemOpFrame'), function (j, op) {
                var operation = {};
                operation.Operator = $(op).find('select.filtsel').find(':selected').val();
                operation.Values = [];
                $.each($(op).find('.filtvtag'), function (k, v) {
                    var _value = {};
                    _value.Value = $(v).text();
                    operation.Values.push(_value);
                });
                item.Operations.push(operation);
            });
            item.Joins = _self._getjoins(fi);
            var l = _self._getlookup(fi);
            if (l) {
                item.Lookup = l;
            }
            var tl = _self._gettypelist(fi);
            if (tl) {
                item.TypeList = tl;
            }
            var lbl = _self._getlbl(fi);
            if (lbl) {
                item.Label = lbl;
            }
            var pn = _self._getprmnm(fi);
            if (pn) {
                item.ParameterName = pn;
            }
            item.bHidden = $(fi).hasClass('questFilterItemHidden');
            data.push(item);
        });
        return (data);
    }
    _self._getjoins = function (fi) {
        var jj = [];
        var _d = _self._getdata(fi);
        if (_d.Joins) {
            $.each(_d.Joins, function(i, j) {
                jj.push(j);
            });
        }
        return (jj);
    }
    _self._getlookup = function (fi) {
        var l;
        var _d = _self._getdata(fi);
        if (_d.Lookup && _d.Lookup.Id > 0) {
            l = { Id: _d.Lookup.Id, Name: _d.Lookup.text };
        }
        return (l);
    }
    _self._gettypelist = function (fi) {
        var tl;
        var _d = _self._getdata(fi);
        if (_d.TypeList && _d.TypeList.Id > 0) {
            tl = { Id: _d.TypeList.Id, Name: _d.TypeList.text };
        }
        return (tl);
    }
    _self._getlbl = function (fi) {
        var lbl;
        var i = $(fi).find('.questFilterItemLabelText input');
        if (i.length) {
            lbl = $(i).val().trim();
            lbl = lbl.length ? lbl : null;
        }
        else {
            var _d = _self._getdata(fi);
            if (_d.Label && _d.Label.length) {
                lbl = _d.Label;
            }
        }
        return (lbl);
    }
    _self._getprmnm = function (fi) {
        var pn;
        var i = $(fi).find('.questFilterItemParameterNameText input');
        if (i.length) {
            pn = $(i).val().trim();
            pn = pn.length ? pn : null;
        }
        else {
            var _d = _self._getdata(fi);
            if (_d.ParameterName && _d.ParameterName.length) {
                pn = _d.ParameterName;
            }
        }
        return (pn);
    }
    _self._getentity = function (fi) {
        var _data = _self._getdata(fi);
        var entity = {};
        entity.Id = _data.Id;
        entity.type = _data.type;
        return (entity);
    }
    _self._getdata = function (fi) {
        var _data = $('#' + $(fi).attr('id')).data();
        if (!_data) {
            alert('_data not found for fi: ' + $(fi).attr('id'));
        }
        var data = _data[$(fi).attr('id')];
        return (data);
    }

    _self.GetItem = function (id) {
        var _d;
        var dd = _self.GetData();
        $.each(dd, function (i, d) {
            if (d.Entity.Id == id) {
                _d = d;
                return (false);
            }
        });
        return (_d);
    }
    _self.AddData = function (id, n, v) {
        var e = $('.questFilterItem[data-id="' + id + '"]', _self._e);
        if (!e) { return; }
        var d = _self._getdata(e);
        d[n] = v;
        _self.Change(true);
        return (e);
    }

    _self.Clear = function () {
        $(_self._e).empty();
    }

    _self.AddJoin = function (id, j) {
        var itm = _self.GetItem(id);
        if (!itm) { return; }
        var e = $('.questFilterItem[data-id="' + id + '"]', _self._e);
        if (!e) { return; }
        $(e).find('.questFilterJoinFrame').append('<div class="questFilterItemJoin">' + j.JoinType + '&nbsp;' + j.Identifier + '</div>');
        var _d = _self._getdata(e);
        if (!_d.Joins) { _d.Joins = []; }
        _d.Joins.push(j);
        _self.AddData(id, 'Joins', _d.Joins);
        _self.Change(true);
        return (_d.Joins);
    }
    _self.RemoveJoin = function (id, j) {
        var itm = _self.GetItem(id);
        if (!itm) { return; }
        var e = $('.questFilterItem[data-id="' + id + '"]', _self._e);
        if (!e) { return; }
        var _d = _self._getdata(e);
        // TODO: 
        var _jj = [];
        $.each(_d.Joins, function (_i, _j) {
            if (j.ColumnId !== undefined && j.ColumnId !== "undefined") {
                if (j.ColumnId == _j.ColumnId) { return; }
            }
            else {
                if (j.JoinText == $(e).find('.questFilterItemJoin').text()) { return; }
            }
            _jj.push(_j);
        });
        _d.Joins = _jj;
        _self.AddData(id, 'Joins', _d.Joins);
        if (j.ColumnId !== undefined && j.ColumnId !== "undefined") {
            $('.questFilterItemJoin[data-id="' + j.ColumnId + '"]', _self._e).remove();
        }
        else {
            $(e).find('.questFilterItemJoin:contains("' + j.JoinText + '")').remove();
        }
        _self.Change(true);
        return (_d.Joins);
    }

    _self.ShowJoinItemsOnly = function (bJoinItemsOnly, bVisibleItems) {
        var ii = bVisibleItems ? $(_self._e + ' div.questFilterItem:visible') : $(_self._e + ' div.questFilterItem');
        $.each(ii, function (i, itm) {
            if (!bJoinItemsOnly || $(itm).find('.questFilterItemJoin').text().trim().length > 0) {
                $(itm).show();
            }
            else {
                $(itm).hide();
            }
        });
    }

    _self._docallback = function (ud, data) {
        if (_self._model.callback) {
            return (_self._model.callback(ud, data));
        }
    }

    _self.Change = function (v) {
        _self._bChanges = v;
        var _evt = _self._getevt("OnChange");
        if (_evt != null) {
            if (_evt.callback({ OnChange: true }, _self._bChanges)) {
                return;
            }
        }
    }
    _self.bChanges = function () {
        return (_self._bChanges);
    }

    _self.Validate = function () {
        $(_self._e).find('.validationError').removeClass('validationError filtverr');
        var ss = $('.filtsel', _self._e);
        $.each(ss, function (i, s) {
            var _opf = $(s).closest('.questFilterItemOpFrame');
            var _vi = $(_opf).find('input.filtinput');
            if ($(_vi).val().length > 0) {
                $(_vi).addClass('validationError');
            }
            if ($(s).val() == -1) {
                if ($(_opf).find('.filttags').children().length > 0) {
                    $(_opf).find('.filttags').find('.filtvtag').addClass('filtverr');
                }
            }
            else {
                if ($(_opf).find('.filttags').children().length == 0) {
                    $(s).addClass('validationError');
                }
            }
        });
        return (($(_self._e).find('.validationError').length + $(_self._e).find('.filtverr').length) == 0);
    }


    _self._init();
}