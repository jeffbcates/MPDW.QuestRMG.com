// qrmgTable.js
function qrmgTable(model) {
    var _self = this;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._model = model;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._cb = model.callback ? model.callback : null;
    _self._name = model.name || model.element;
    _self._defopers = [
        { name: 'New', label: '&nbsp;&nbsp;New&nbsp;&nbsp;', classes: 'btn-new', editor: true, New: true },
        { name: 'Save', label: '&nbsp;Save&nbsp;', classes: 'btn-default', hidden: true, Save: true },
        { name: 'Refresh', classes: 'btn-default', Refresh: true }
    ];
    _self._kc;
    _self._oe;
    _self._dpszoo = [10, 50, 100];
    _self._pgr = {
        SortColumns: {
            Columns: [
                { Name: 'Id', Direction: 1 },
            ]
        },
        Paging: {
            PageNumber: 0,
            PageSize: 0
        },
        SearchOptions: {
            SearchFieldList: [
               //// { Name: '', SearchOperation: '', Type: '', Value: ''},
            ],
            SearchString: ''
        }
    }
    _self._qryrs = {
        TotalRecords: 0,
        TotalPages: 0,
        PageNumber: 0,
        PageSize: 0
    }
    _self._sorter;
    _self._cmdpfx = '_tblcmd_';

    _self._init = function () {
        _self._mask = _self._model.mask || _self._e;
        qrmgmvc.Global.Mask(_self._mask);
        _self._initctx()
        _self._initpgszoo();
        _self._initcc();
        _self._initpgr();
        _self._initops();
        _self._initcmds();
        _self._render();
        _self._bind();
        if (_self._model.autoLoad == true) {
            _self.Load();
        }
        else {
            qrmgmvc.Global.Unmask(_self._mask);
        }
    }
    _self._initctx = function() {
        _self._ctx = new qrmgctx(_self._model.ctx);
    }
    _self._initcc = function () {
        $.each(_self._model.columns, function (i, c) {
            c.sortable = true;
            if (c.key) {
                _self._kc = c;
            }
        });
    }
    _self._initpgr = function () {
        if (!_self._model.paging) {
            _self._pgr.Paging.PageSize = 10;
            _self._pgr.Paging.PageNumber = 1;
            return;
        }
        if (_self._model.paging.PageSize) {
            _self._pgr.Paging.PageSize = _self._model.paging.PageSize;
        }
        else {
            _self._pgr.Paging.PageSize = 10;
        }
        if (_self._model.paging.PageNumber) {
            _self._pgr.Paging.PageNumber = _self._model.paging.PageNumber;
        }
        else {
            _self._pgr.Paging.PageNumber = 1;
        }
    }
    _self._initops = function () {
        _self._initdefoo();
        if (!_self._model.operations) {
            _self._model.operations = [];
        }
        $.each(_self._model.operations, function (i, o) {
            o._id = _self._pfx + o.name;
            o._lbl = (o._lbl || o.label) ? (o._lbl || o.label) : o.name;
            o.classes = o.classes ? o.classes : "tblbtnop";
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
                if (!_do) {
                    _oo.push(o);
                }
            });
            _self._model.operations = _oo;
            _self._model.operations = _self._defopers.concat(_self._model.operations);
        }
    }
    _self._initdefoo = function () {
        $.each(_self._defopers, function (i, o) {
            o._id = _self._pfx + o.name;
            o._lbl = o._lbl || o.name;
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
    _self._initpgszoo = function () {
        if (_self._model.paging) {
            if (_self._model.paging.PageSizeOptions !== undefined) {
                if ($.isArray(_self._model.paging.PageSizeOptions)) {
                    _self._dpszoo = _self._model.paging.PageSizeOptions;
                }
            }
        }
    }
    _self._initcmds = function () {
        if (!_self._model.commands) {
            _self._model.commands = [];
        }
        $.each(_self._model.commands, function (i, c) {
            c._id = _self._pfx + _self._cmdpfx + c.name;
            if (c.single || c.edit) {
                c.min = 1; c.max = 1;
            }
        });
    }

    _self._render = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<div id="' + _self._name + 'Frame" class="questTableFrame">';
        _h[_i++] = '<div id="' + _self._name + 'Operations"  class="questTableOperations">';
        _h[_i++] = _self._rndrops();
        _h[_i++] = '</div>';
        _h[_i++] = '<div id="' + _self._name + 'Subframe" class="">';
        _h[_i++] = _self._rndrhdr();
        _h[_i++] = _self._rndrtbl();
        _h[_i++] = _self._rndrftr();
        _h[_i++] = '</div>';
        _h[_i++] = '</div>';
        $(_self._e).replaceWith(_h.join(''));
    }
    _self._rndrops = function () {
        var _h = [], _i = 0;
        $.each(_self._model.operations, function (i, o) {
            if (o.editor && _self._model.editor == false) {
            }
            else {
                if (o.hidden) {
                    _h[_i++] = '<button id="' + o._id + '" class="btn btn-quest ' + (o.classes ? o.classes : "") + '" style="display:none;">' + o._lbl + '</button>';
                }
                else {
                    _h[_i++] = '<button id="' + o._id + '" class="btn btn-quest ' + (o.classes ? o.classes : "") + '">' + o._lbl + '</button>';
                }
            }
        });
        return (_h.join(''));
    }
    _self._rndrhdr = function () {
        if (_self._model.noheader) { return; }
        var _h = [], _i = 0;
        _h[_i++] = '<div class="questTableHeaderFrame">';
        _h[_i++] = '<div class="questTableHeaderInfo">';
        _h[_i++] = _self._rndrtr(true);
        _h[_i++] = _self._rndrpgsz();
        _h[_i++] = _self._rndrfltr();
        _h[_i++] = '</div>';
        _h[_i++] = _self._rndrpgr('top');
        _h[_i++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrfltr = function () {
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTableFilter">';
        _h[_j++] = 'Filter: <input type="text">';
        _h[_j++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrtbl = function () {
        var _h = [], _i = 0;
        if (_self._model.maxbodyheight) {
            _h[_i++] = '<div class="questTableBodyFrame" style="height: ' + _self._model.maxbodyheight  + '">';
        }
        _h[_i++] = '<table id="' + _self._model.name + '" class="qrmgTable table table-bordered table-hover ' + (_self._model.classes ? _self._model.classes : "") + '">';
        _h[_i++] = _self._rndrth();
        _h[_i++] = _self._rndrtb();
        _h[_i++] = _self._rndrtf();
        _h[_i++] = '</table>';
        if (_self._model.maxbodyheight) {
            _h[_i++] = '</div>';
        }
        return (_h.join(''));
    }
    _self._rndrth = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<thead class="thead-default"><tr>';
        $.each(_self._model.columns, function (i, c) {
            c._id = _self._pfx + c.name;
            c._lbl = c.label ? c.label : c.name;
            if (i == 0 && !_self._model.noops) {
                _h[_i++] = '<th class="_tblcmds dropdown text-center">';
                _h[_i++] = '<a href="#" data-toggle="dropdown" class="dropdown-toggle"><span class="no-sort fa fa-bars"></span></a>';
				if (_self._model.commands.length) {
					_h[_i++] = '<ul class="_tblcmds dropdown-menu">';
					$.each(_self._model.commands, function (i, c) {
						_h[_i++] = '<li id="' + c._id + '">' + c.label || c.name;
						_h[_i++] = '</li>';
					});
					_h[_i++] = '</ul>';
				}
                _h[_i++] = '</th>';
            }
            if (c.hidden) {
                _h[_i++] = '<th id="' + c._id + '" style="display: none;">' + c._lbl + '</th>';
            }
            else {
                _h[_i++] = '<th  id="' + c._id + '" ';
                if (c.width) {
                    _h[_i++] = ' style="width:' + c.width + '" ';
                }
                _h[_i++] = '>';
                _h[_i++] = c._lbl;
                if (c.sortable) {
                    _h[_i++] = '<span class="fa fa-sort-asc questSort questSortUp"></span>';
                    _h[_i++] = '<span class="fa fa-sort-desc questSort questSortDown"></span>';
                }
                _h[_i++] = '</th>';
            }
        });
        _h[_i++] = '</tr></thead>';
        return (_h.join(''));
    }
    _self._rndrtb = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<tbody>';
        _h[_i++] = '</tbody>';
        return (_h.join(''));
    }
    _self._rndrtf = function () {
        var _h = [], _j = 0;
        _h[_j++] = '<tfoot>';
        _h[_j++] = '</tfoot>';
        return (_h.join(''));
    }
    _self._rndrftr = function () {
        if (_self._model.nofooter) { return; }
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTableFooterFrame">';
        _h[_j++] = '<div class="questTableFooterInfo">';
        _h[_j++] = _self._rndrtr();
        _h[_j++] = _self._rndrpgsz();
        _h[_j++] = '</div>';
        _h[_j++] = _self._rndrpgr('bottom');
        _h[_j++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrtr = function (top) {
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTableTotalRecords">';
        _h[_j++] = 'Total Records: <span class="questTableTotalRecordCount"></span>';
        _h[_j++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrpgsz = function () {
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTablePageSize">Page Size: ';
        _h[_j++] = '<select class="questTablePageSizeSelect">';
        $.each(_self._dpszoo, function (i, sz) {
            _h[_j++] = '<option value="' + sz + '">' + sz + '</option>';
        });
        _h[_j++] = '</select>';
        _h[_j++] = '</div>';
        return (_h.join(''));
    }
    _self._rndrpgr = function (tb) {
        if (_self._model.nopager) { return; }
        var _b = true;
        if (tb) {
            if (_self._model.paging) {
                if (tb == 'top') {
                    _b = _self._model.paging.Top !== undefined ? _self._model.paging.Top : true;
                }
                else if (tb == 'bottom') {
                    _b = _self._model.paging.Bottom !== undefined ? _self._model.paging.Bottom : true;
                }
            }
        }
        if (!_b) { return (''); }
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTablePager">';
        _h[_j++] = '<div class="questTablePagerButton"><div class="questPagerFirst"></div></div>';
        _h[_j++] = '<div class="questTablePagerButton"><div class="questPagerPrev"></div></div>';
        _h[_j++] = '<div class="questTablePagerNofM"></div>';
        _h[_j++] = '<div class="questTablePagerButton"><div class="questPagerNext"></div></div>';
        _h[_j++] = '<div class="questTablePagerButton"><div class="questPagerLast"></div></div>';
        _h[_j++] = '</div>';
        _h[_j++] = '<div class="clear"></div>';
        return (_h.join(''));
    }

    _self._bind = function () {
        _self._bndth();
        _self._bndcmds();
        _self._bndevts();
        _self._bndops();
        _self._bndfltr();
        _self._bndpgr();
        _self._bndsrt();
    }
    _self._bndth = function () {
        $(_self._e + ' thead tr th').unbind("click").on('click', function (e) {
            if ($(this).hasClass('_tblcmds')) { return; }
            var _n = $(e.currentTarget).attr('id').substr(_self._pfx.length);
            var _c = _self._getc(_n);
            var _s = _self._setsort(_c, e);
        });
    }
    _self._bndcmds = function () {
        $('ul._tblcmds li', _self._e).unbind('click').on('click', function (e) {
            var n = this.id.substr(_self._pfx.length + _self._cmdpfx.length);
            _self._docmd(n);
        });
    }

    _self._setsort = function (c, e) {
        var _ce = $('#' + c._id, _self._e);
        if (_ce.length == 0) { return; }
        $('#' + c._id, _self._e).find('span').removeClass('questSorted');
        if (c.sort !== undefined) {
            c.sort = c.sort == 1 ? 2 : 1;
            if (c.sort == 1) {
            }
            else if (c.sort == 2) {
            }
        }
        else {
            c.sort = 2;
        }
        $(_ce).find('span.' + (c.sort == 1 ? 'questSortUp' : 'questSortDown') + '').addClass('questSorted');

        if (!e.ctrlKey) {
            $.each(_self._model.columns, function (i, c2) {
                if (c2._id != c._id) {
                    c2.sort = undefined;
                    $('#' + c2._id, _self._e).find('span').removeClass('questSorted');
                }
            });
            _self._pgr.SortColumns.Columns = [];
        }
        var _idx = -1;
        $.each(_self._pgr.SortColumns.Columns, function (i, s) {
            if (s.Name == c.name) { _idx = i; return (false); }
        });
        if (_idx > -1) {
            _self._pgr.SortColumns.Columns.splice(_idx, 1);
        }
        _self._pgr.SortColumns.Columns.push({ Name: c.name, Direction: c.sort });

        return (c.sort);
    }
    _self._bndevts = function () {
        $(_self._e + ' tbody tr').unbind("click").on('click', _self._cbRowClick);
        $(_self._e + ' tbody tr').unbind("dblclick").on('dblclick', _self._cbRowDblClick);
        $(_self._e + ' tbody tr').unbind("doubletap").on('doubletap', _self._cbRowDblClick);
    }
    _self._bndops = function () {
        _self._oe = $(_self._e).closest('.questTableFrame').find('.questTableOperations');
        $.each(_self._model.operations, function (i, o) {
            $(('#' + o._id), _self._oe).on('click', null, o, function (e) {
                _self._dooper(o);
                e.stopPropagation();
                e.preventDefault();
            });
        });
    }


    _self._bndrops = function () {
        var _rrops = $(_self._e).find('._tblrops').unbind('click').on('click', function (e) {
            var _pp = this.id.split('_');
            var _id = _pp[_pp.length - 1];
            alert('Row Operations clicked! ' + _id);
        });
        return (_rrops);
    }
    _self._bndpgr = function () {
        $(_self._e).parent().find('.questTablePagerButton').on('click', function (e) {
            var _po = $(e.currentTarget).find('div[class^="questPager"]')[0].classList[0].substr("questPager".length);
            _self[_po]();
        });
        $(_self._e).parent().find('.questTablePageSizeSelect').on('change', function (e) {
            _self._pgr.Paging.PageSize = $(e.currentTarget).val();
            _self._pgr.Paging.PageNumber = 1;
            _self.Refresh();
        });
    }
    _self._bndsrt = function () {
        var _t = $(_self._e);
        if (_t.length == 1) {
            _self._sorter = tsorter.create(_self._e.substr(1));
        }
    }
    _self._bndfltr = function () {
        $(_self._e).parent().find('.questTableFilter input').on('keyup', function (e) {
            _self.Filter($(this).val());
        });
    }

    _self.Filter = function (f) {
        var rows = $('tr', _self._e).not('thead tr');
        rows.hide();
        rows.filter(":contains('" + f + "')").show();
    }

    _self._cbRowClick = function (e) {
        $(_self._e).find('.trSelected').removeClass('trSelected');
        $(e.currentTarget).closest('tr').find('td').addClass('trSelected');
        var evt = _self._getEvent('RowClick');
        if (!evt) { return; }
        if (evt.callback) {
            var _td = $(e.target).parent().find('td')[0];
            var _d = { Id: _td.innerHTML };
            evt.callback(evt.name, _d, e);
        }
    }
    _self._cbRowDblClick = function (e) {
        $(_self._e).find('.trSelected').removeClass('trSelected');
        $(e.currentTarget).closest('tr').find('td').addClass('trSelected');
        var evt = _self._getEvent('DoubleClick');
        if (!evt) {
            if (_self._model.editor && _self._model.editor.uri) {
                ////var _d = {};
                ////var _ctx = _self._getctx();
                ////$.extend(_d, _ctx);
                ////var _id = _self._getid(e);
                ////$.extend(_d, _id);
                ////var _io = new qrmgio(_self._roper);
                ////_io.ShowView(_self._model.editor.uri, _d, 'DoubleClick');

                var _id = _self._getid(e);
                _self.Edit(_id, 'DoubleClick');
            }
            return;
        }
        if (evt.callback) {
            var _td = $(e.target).parent().find('td')[0];
            var _d = { Id: _td.innerHTML };
            evt.callback(evt.name, _d, e);
        }
    }
    _self._getid = function (e) {
        if (!_self._kc) { return; }
        var _v = $(e.currentTarget).find('td[Id^="' + _self._name + '_' + _self._keyc.name + '"]').text();
        _o = {};
        _o[_self._keyc.name] = _v;
        return (_o);
    }
    _self._getEvent = function (evt) {
        var _evt = null;
        $.each(_self._model.events, function (i, e) {
            if (e.name == evt) {
                _evt = e;
                return (false);
            }
        });
        return (_evt);
    }
    _self.Edit = function (_id, evt) {
        var _d = _self._getctx();
        $.extend(_d, _id);
        var _io = new qrmgio(_self._roper);
        _io.ShowView(_self._model.editor.uri, _d, evt || 'Edit');
    }

    _self.Load = function (data) {
        qrmgmvc.Global.Mask(_self._e);
        if (!data) {
            _self._load();
        }
        else {
            return (_self._rload(null, data));
        }
    }
    _self._load = function (m) {
        var _io = new qrmgio(_self._rload, _self._model);
        var _d = _self.Context();
        $.extend(_d, _self._pgr);
        var _uri = _self._model.uri + (m ? ('/' + m) : '/List');
        _io.GetJSON(_uri, _d);
    }
    _self._rload = function (ud, dd) {
        if (!IsAppSuccess(dd)) {
            DisplayUserMessage(dd);
        }
        if (IsAppError(dd) || IsAppFatal(dd) || (!_self._setqryfs(dd))) {
            qrmgmvc.Global.Unmask(_self._e);
            return;
        }
        var _h = [], _i = 0;
        _h[_i++] = _self._rrows(dd.Items);
        $(_self._e).find('tbody').empty().append(_h.join(''));
        $(_self._e).find('td').addClass('nowrap');

        _self._settr(dd);
        _self._setps(dd);
        _self._bndrops();

        $(_self._e).parent().find('.questTablePagerNofM').text('Page ' + dd.QueryResponse.PageNumber + ' of ' + dd.QueryResponse.TotalPages);
        _self._bndevts();

        var evt = _self._getEvent('AfterLoad');
        if (evt) {
            if (evt.callback) {
                evt.callback('AfterLoad');
            }
        }
        qrmgmvc.Global.Unmask(_self._e);
    }
    _self._rrows = function (dd, cc) {
        var _cc = cc ? cc : '';
        var _h = [], _i = 0;
        $.each(dd, function (i, d) {
            _h[_i++] = '<tr>';
            $.each(_self._model.columns, function (j, c) {
                if (c.key) {
                    _self._keyc = c;
                }
                var _d = String(d[c.name] ? d[c.name] : '') || '';
                if (j == 0 && !_self._model.noops) {' + _d + '
                    _h[_i++] = '<td class="_tblrsel ' + _cc + ' text-center"><input id="' + _self._name + '_' + c.name + '_sel_' + _d + '" data-id="' + _d + '" type="checkbox" /></td>';
                }
                if (_d !== undefined) {
                    if (c.hidden) {
                        if (c.key) {
                            _h[_i++] = '<td id="' + _self._name + '_' + c.name + '_' + _d + '" style="display: none;">' + _d + '</td>';
                        }
                        else {
                            _h[_i++] = '<td style="display: none;">' + _d + '</td>';
                        }
                    }
                    else {
                        if (c.key) {
                            _h[_i++] = '<td id="' + _self._name + '_' + c.name + '_' + _d + '" class="' + _self._rndrccc(c) + ' ' + _cc + '">' + _d + '</td>';
                        }
                        else {
                            _h[_i++] = '<td class="' + _self._rndrccc(c) + ' ' + _cc + '">' + _d + '</td>';
                        }
                    }
                }
            });
            _h[_i++] = '</tr>';
        });
        return (_h.join(''));
    }

    _self._settr = function (dd) {
        if (parseInt(dd.QueryResponse.TotalRecords) == 0) { return; }
        $(_self._e).parent().find('span.questTableTotalRecordCount').text(dd.QueryResponse.TotalRecords);
    }
    _self._setps = function (dd) {
        if (parseInt(dd.QueryResponse.TotalRecords) == 0) { return; }
        var _pss = $(_self._e).parent().find('.questTablePageSize select');
        if ($(_pss).find('option[value="' + dd.QueryResponse.PageSize + '"]').length == 0) {
            $(_pss).append('<option value="' + dd.QueryResponse.PageSize + '">' + dd.QueryResponse.PageSize + '</option>');
        }
        $(_pss).val(dd.QueryResponse.PageSize);

        if (_pss.length == 1) {
            var _oo = _self._srtpgzoo(_pss);
            $(_pss).empty().append(_oo);
        }
        else {
            $.each(_pss, function (i, s) {
                var _oo = _self._srtpgzoo(s);
                $(s).empty().append(_oo);
            });
        }

        $(_pss).val(dd.QueryResponse.PageSize);
        return (dd.QueryResponse.PageSize);
    }
    _self._srtpgzoo = function (s) {
        var _oo = $(s).find('option');
        _oo.sort(function (a, b) {
            if (parseInt(a.text) > parseInt(b.text)) return 1;
            if (parseInt(a.text) < parseInt(b.text)) return -1;
            return 0
        });
        return (_oo);
    }
    _self._setqryfs = function (dd) {
        if (!dd.QueryResponse) {
            _self._qryrs = null;
            _self._pgr.Paging.PageNumber = -1;
            return (false);
        }
        _self._qryrs = dd.QueryResponse;
        _self._pgr.Paging.PageNumber = _self.PageNumber();
        _self._pgr.Paging.PageSize = _self.PageSize();
        return (true);
    }
    _self._rndrccc = function (c) {
        var _cc = "";
        if (c.align) {
            if (c.align == 'center') {
                _cc = 'alignCenter';
            }
            else if (c.align == 'right') {
                _cc = 'alignRight';
            }
            else if (c.align == 'left') {
                _cc = 'alignLeft';
            }
        }
        if (c.classes) {
            _cc += ' ' + c.classes;
        }
        return (_cc);
    }

    _self.First = function () {
        _self._pgnum(1);
        _self._load('First');
    }
    _self.Prev = function () {
        var p = _self._pgnum();
        if (p <= 1) { return; }
        _self._pgnum(p - 1);
        _self._load('Prev');
    }
    _self.PageNum = function (num) {
        if (num <= 0) { return; }
        if (num >= _self.TotalPages()) { return; }
        _self._pgnum(num);
        _self._load('PageNum');
    }
    _self.Next = function () {
        var p = _self._pgnum();
        if (p >= _self.TotalPages()) { return; }
        _self._pgnum(p + 1);
        _self._load('Next');
    }
    _self.Last = function () {
        _self._pgnum(-1);
        _self._load('Last');
    }
    _self._pgnum = function (num) {
        if (num) {
            _self._pgr.Paging.PageNumber = (num == -1 ? _self.TotalPages : num);
        }
        return (_self._pgr.Paging.PageNumber);
    }
    _self.TotalRecords = function () {
        return (_self._qryrs.TotalPages);
    }
    _self.TotalPages = function () {
        return (_self._qryrs ? _self._qryrs.TotalPages : -1);
    }
    _self.PageNumber = function () {
        return (_self._qryrs ? _self._qryrs.PageNumber : -1);
    }
    _self.PageSize = function () {
        return (_self._qryrs ? _self._qryrs.PageSize : -1);
    }

    _self.Refresh = function () {
        return (_self.Load());
    }
    _self.Select = function (Id, bScrollTo) {
        var _kn = _self._keyc.name;
        if (!_kn) { return (false); }
        var _tr = $(_self._e).find('td[Id="' + _self._name + '_' + _kn + '_' + Id + '"]').closest('tr');
        if (!_tr || _tr.length == 0) { return (false); }
        $(_tr).focus();
        $(_self._e).find('.trSelected').removeClass('trSelected');
        $(_tr).find('td').addClass('trSelected');
        if (bScrollTo) {
            $('html, body').animate({ scrollTop: $(_tr).offset().top - 300 }, 200);
        }
        return (true);
    }
    _self.GetRow = function (Id) {
        var _kn = _self._keyc.name;
        if (!_kn) { return (false); }
        var _tdk = $(_self._e).find('td[Id="' + _self._name + '_' + _kn + '_' + Id + '"]');
        var _tr = $(_tdk).closest('tr');
        var _d = {};
        $.each($(_tr).find('td'), function (i, td) {
            if ($(td).hasClass('_tblrops')) { return; }
            if (!_self._model.noops) {
                _d[_self._model.columns[i - 1].name] = $(td).text();
            }
            else {
                _d[_self._model.columns[i].name] = $(td).text();
            }
        });
        var _ctx = _self._getctx();
        $.extend(_d, _ctx);
        return (_d);
    }

    _self.AddRow = function (dd, bP, cc) {
        var _dd;
        if ($.isArray(dd)) {
            _dd = dd;
        }
        else {
            _dd = [];
            _dd.push(dd);
        }
        var _h = [], _i = 0;
        _h[_i++] = _self._rrows(_dd, cc);
        if (bP) {
            $(_self._e).find('tbody').prepend(_h.join(''));
        }
        else {
            $(_self._e).find('tbody').append(_h.join(''));
        }
        $(_self._e).find('td').addClass('nowrap');
    }
    _self.AddClass = function (Id, cc) {
        var _r = _self.GetRow(Id);
        if (_r) {
        }
    }

    _self._dooper = function (o) {
        // TODO: mask

        if (o.Refresh) {
            _self.Load();
            return;
        }
        if (o.New) {
            _self.New(o);
            return;
        }
        if (o.Save) {
            _self.Save(o);
            return;
        }
        // TODO: custom operations.
        try {
            var _url = (o.uri ? o.uri : null);
            if (!_url) {
                if (o.editor && $.isPlainObject(_self._model.editor)) {
                    _url = _self._model.editor.uri ? _self._model.editor.uri : null;
                    _url = _url ? (_url + "/" + o.name) : _url;
                }
            }
            if (!_url) {
                _url = _self._model.uri + "/" + o.name;
            }
            var _d = _self._getctx();
            if (o.args) {
                var _a = _self._getargs(o.args);
                $.extend(_d, _a);
            }
            var _io = new qrmgio(_self._roper);
            if (o.action) {
                _io.GetJSON(_url, _d, o);
            }
            else if (o.operation) {
                _io.PostJSON(_url, _d, o);
            }
            else {
                _io.ShowView(_url, _d, o);
            }
        }
        catch (e) {
            alert('F|Error doing operation ' + o.name);
        }
    }
    _self._roper = function (ud, d) {
        if (IsUserMessage(d)) {
            DisplayUserMessage(d);
        }
        if (_self._docallback(ud, d)) {
            return;
        }
        // TODO: unmask
    }
    _self._geto = function (n) {
        var _o;
        $.each(_self._model.operations, function (i, o) {
            if (n == o.name) {
                _o = o;
                return (false);
            }
        });
        return (_o);
    }

    _self._docmd = function (n) {
        ClearUserMessage();
        var _cmd = _self._getcmd(n);
        if (!_cmd) {
            DisplayUserMessage('E|' + n  +' command definition not found.')
            return;
        }
        DisplayUserMessage('I|' + (_cmd.label || _cmd.name) + '...');
        qrmgmvc.Global.Mask(_self._mask);
        try {
            var ids = _self._getseltr();
            if (!ids.length) {
                DisplayUserMessage('E|' + n + ': no rows are selected');
                qrmgmvc.Global.Unmask(_self._mask);
                return;
            }
            if (_cmd.min && ids.length < _cmd.min) {
                DisplayUserMessage('E|' + n + ': minimum selected allowed is ' + _cmd.min);
                qrmgmvc.Global.Unmask(_self._mask);
                return;
            }
            if (_cmd.max && ids.length > _cmd.max) {
                DisplayUserMessage('E|' + n + ': maximum selected allowed is ' + _cmd.max);
                qrmgmvc.Global.Unmask(_self._mask);
                return;
            }
            var _url = (_cmd.uri ? _cmd.uri : null);
            if (!_url) {
                _url = _self._model.uri + "/" + _cmd.name;
            }
            var _d = _self._getctx();
            if (_cmd.single) {
                _d[_self._keyc.name] = ids[0];
            }
            else {
                var Items = [];
                $.each(ids, function (i, s) {
                    Items.push({ Id: s });
                });
                _d.Items = Items;
            }
            var _io = new qrmgio(_self._rcmd);
            if (_cmd.edit) {
                _self.Edit({ Id: ids[0] }, _cmd);
                return;
            }
            if (_cmd.view == true ||(_cmd.type && _cmd.type == 'view')) {
                _io.ShowView(_url, _d, _cmd);
                return;
            }
            var method = _cmd.method || 'GET';
            if (method == 'GET') {
                _io.GetJSON(_url, _d, _cmd);
            }
            else if (method == 'POST') {
                _io._postJSON(_url, _d, _cmd);
            }
            else {
                DisplayUserMessage('E|Invalid command definition');
            }
        }
        catch (e) {
            DisplayUserMessage('F|EXCEPTION performing command: ' + e.message);
        }
    }
    _self._getcmd = function (n) {
        var _cmd;
        $.each(_self._model.commands, function (i, c) {
            if (n == c.name) {
                _cmd = c;
                return (false);
            }
        });
        return (_cmd);
    }
    _self._getseltr = function () {
        var _sel = [];
        var _rr = $('tbody tr td._tblrsel input:checked', _self._e);
        $.each(_rr, function (i, r) {
            _sel.push($(r).attr('data-id'));
        });
        return (_sel);
    }
    _self._rcmd = function (ud, d) {
        if (IsUserMessage(d)) {
            DisplayUserMessage(d);
        }
        if (_self._docallback(ud, d)) {
            return;
        }
        // TODO: unmask
        qrmgmvc.Global.Unmask(_self._mask);
    }

    _self.New = function (o) {
        if (o.disabled) { return; }
        if (o.inline) {
            _self._enadiso(o, false);
            _self.Insert();
            _self._shwhido('Save', true);
            _self._shwhido('New', false);
            return;
        }
        try {
            var _url = (o.uri ? o.uri : null);
            if (!_url) {
                if (_self._model.editor) {
                    _url = _self._model.editor.uri ? _self._model.editor.uri : null;
                }
            }
            if (!_url) {
                _url = _self._model.uri + "/" + o.name;
            }
            var _ctx = _self._getctx();
            var _io = new qrmgio(_self._roper);
            _io.ShowView(_url, _ctx, o);
        }
        catch (e) {
            alert('F|Error doing operation ' + o.name);
        }
    }
    _self.Save = function (o) {
        try {
            var _url = (o.uri ? o.uri : (_self._model.editor.uri + '/Save'));
            if (!_url) {
                if (_self._model.editor) {
                    _url = _self._model.editor.uri ? _self._model.editor.uri : null;
                }
            }
            if (!_url) {
                _url = _self._model.uri + "/" + o.name;
            }
            var _d = _self.GetNewRow();
            var _ctx = _self._getctx();
            $.extend(_d, _ctx);
            var _io = new qrmgio(_self._roper);
            _io.PostJSON(_url, _d, o);
        }
        catch (e) {
            alert('F|Error doing operation ' + o.name);
        }
    }
    _self.GetNewRow = function () {
        var _d = {};
        var _ii = $(_self._e).find('tbody tr:first input');
        $.each(_ii, function (i, _i) {
            var pp = $(_i).attr('id').split('_');
            var n = pp[1];
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
        return (_d);
    }
    _self.Insert = function () {
        var _h = [], _i = 0;
        _h[_i++] = '<tr>';
        $.each(_self._model.columns, function (j, c) {
            if (j == 0 && !_self._model.noops) {
                _h[_i++] = '<td class="_tblrselNEW text-center"><input id="' + _self._name + '_' + c.name + '_sel_NEW" type="checkbox" /></td>';
                _h[_i++] = '<td id="' + _self._name + '_NEW_ops_" class="_tblrops"></td>';
            }
            if (c.hidden) {
                if (c.key) {
                    _h[_i++] = '<td id="' + _self._name + '_NEW_" style="display: none;">-1</td>';
                }
                else {
                    _h[_i++] = '<td style="display: none;">-1</td>';
                }
                return;
            }
            var t = c.type || 'text';
            switch (t) {
                case 'text':
                    if (c.key) {
                        _h[_i++] = '<td class="' + _self._rndrccc(c) + '"><input id="' + _self._name + '_' + c.name + '_NEW" type="text"  class="form-control"/></td>';
                    }
                    else {
                        _h[_i++] = '<td class="' + _self._rndrccc(c) + '"><input id="' + _self._name + '_' + c.name + '_NEW" type="text" class="form-control" /></td>';
                    }
                    break;
            }
        });
        _h[_i++] = '</tr>';
        $(_self._e).find('tbody').prepend(_h.join(''));
    }
    _self._enadiso = function (o, bE) {
        var _o;
        if (typeof o === 'string') {
            _o = _self._geto(o);
        }
        else {
            _o = o;
        }
        if (bE) {
            delete _o.disabled;
            $('#' + _o._id).removeProp('disabled');
        } else {
            _o.disabled = true;
            $('#' + _o._id).prop('disabled', true);
        }
    }
    _self._shwhido = function (o, bS) {
        var _o;
        if (typeof o === 'string') {
            _o = _self._geto(o);
        }
        else {
            _o = o;
        }
        if (bS) {
            $('#' + _o._id).show();
        }
        else {
            $('#' + _o._id).hide();
        }
    }

    _self.Context = function () {
        var _ctx = _self._getctx();
        if (_self._model.ctx.master) {
            if (_self._model.ctx.master.element) {
                _ctx[_self._model.ctx.master.name] = $(_self._model.ctx.master.value).val();
            }
            else if (_self._model.ctx.master.viewstate) {
                var _vs = _self._getViewState();
                _ctx[_self._model.ctx.master.field] = _vs[_self._model.ctx.master.field];
            }
        }
        return (_ctx);
    }
    _self._getctx = function () {
        var _ctx = _self._ctx.Context();
        return (_ctx);
    }
    _self._getViewState = function () {
        var _vs;
        if (_self._model.viewstate) {
            // TODO: name a view state element
        }
        else {
            _vs = JSON.parse($('input[Id="' + '__' + _self._e.substr(1) + 'VIEW_STATE' + '"]').val());
        }
        return (_vs);
    }

    _self._getargs = function (aa) {
        var _d = {};
        $.each(aa, function (i, a) {
            if (a.viewstate) {
                var vs = _self._getViewState();
                _d[(a.name ? a.name : a.field)] = vs[a.field];
            }
            else if ($.isPlainObject(a)) {
                $.extend(_d, a);
            }
            else {
                _d[a.name] = $('#' + a.element).val();
            }
        });
        return (_d);
    }

    _self._getc = function (n) {
        var _c;
        $.each(_self._model.columns, function (i, c) {
            if (n == c.name) {
                _c = c;
                return (false);
            }
        });
        return (_c);
    }
    _self._docallback = function (ud, d) {
        if (_self._cb) {
            return (_self._cb(ud, d));
        }
    }

    _self._init();
}

