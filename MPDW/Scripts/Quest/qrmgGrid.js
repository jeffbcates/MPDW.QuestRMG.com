// qrmgGrid.js
function qrmgGrid(model) {
    var _self = this;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._model = model;
    _self._pfx = model.prefix ? model.prefix : '';
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._cb = model.callback ? model.callback : null;
    _self._name = model.name || model.element;
    _self._defopers = [];
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
        _self._render();
        _self._autogen();
		qrmgmvc.Global.Unmask(_self._mask);
    }
    _self._initctx = function () {
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
            o._lbl = o._lbl ? o._lbl : o.name;
            o.classes = o.classes ? o.classes : "btn-default";
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
    _self._rndrfltr = function () {
        var _h = [], _j = 0;
        _h[_j++] = '<div class="questTableFilter">';
        _h[_j++] = 'Filter: <input type="text">';
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
    _self._rndrtbl = function () {
        var _h = [], _i = 0;
        if (_self._model.maxbodyheight) {
            _h[_i++] = '<div class="questTableBodyFrame" style="height: ' + _self._model.maxbodyheight + '">';
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
    _self._rndrth = function (bOverwrite) {
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
        if (bOverwrite) {
            $('thead', self._e).replaceWith(_h.join(''));
        }
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

    _self._bind = function () {
        _self._bndth();
        _self._bndevts();
        _self._bndops();
        _self._bndfltr();
        _self._bndpgr();
        _self._bndsrt();
    }
    _self._bndth = function () {
        $(_self._e + ' thead tr th').unbind("click").on('click', function (e) {
            if ($(this).hasClass('_tblsel')) {
                // TODO: SORT selected/not-selected
                e.stopPropagation();
                e.preventDefault();
                return;
            }
            if ($(this).hasClass('_tblops')) {
                e.stopPropagation();
                e.preventDefault();
                return;
            }
            var _n = $(e.currentTarget).attr('id').substr(_self._pfx.length);
            var _c = _self._getc(_n);
            var _s = _self._setsort(_c, e);
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

    _self._autogen = function () {
        if (!_self._model.autogen) { return; }
        if (_self._model.autogen.rows) {
            _self._agrr();
        }
    }
    _self._agrr = function () {
        var _rr = [];
        for (i=0; i < _self._model.autogen.rows; i++) {
            var _r = {};
            $.each(_self._model.columns, function (j, c) {
                _r[c.name] = '&nbsp;';
            });
            _rr.push(_r);
        }
        var _h = [], _i = 0;
        _h[_i++] = _self._rrows(_rr);
        $(_self._e).find('tbody').empty().append(_h.join(''));
        $(_self._e).find('td').addClass('nowrap');
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
                var _d = String(d[c.label || c.name] ? d[c.label || c.name] : '') || '';
                if (j == 0 && !_self._model.noops) {
                    _h[_i++] = '<td class="_tblrsel ' + _cc + ' text-center"><input id="' + _self._name + '_' + c.name + '_sel_' + (c.key ? _d : j) + '" type="checkbox" /></td>';
                    ////_h[_i++] = '<td id="' + _self._name + '_' + c.name + '_ops_' + _d + '" class="_tblrops ' + _cc + '"></td>';
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
        var _d = _self._getctx();
        $.extend(_d, _self._pgr);
        var _uri = _self._model.uri + (m ? ('/' + m) : '/List');
        _io.GetJSON(_uri, _d);
    }
    _self._rload = function (ud, dd) {
        if (!IsAppSuccess(dd)) {
            DisplayUserMessage(dd);
        }
        ////if (IsAppError(dd) || IsAppFatal(dd) || (!_self._setqryfs(dd))) {
        if (IsAppError(dd) || IsAppFatal(dd)) {
            qrmgmvc.Global.Unmask(_self._e);
            return;
        }
        if (_self._model.dynamic) {
            _self._ldcc(dd[_self._model.dynamic.field].Columns);
        }
        var _data = _self._fmtdynd(dd[_self._model.dynamic.field]);
        var _h = [], _i = 0;
        _h[_i++] = _self._rrows(_data);
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
    _self._ldcc = function (cc) {
        var _cc = [];
        $.each(cc, function (i, c) {
            var _c = { name: c.Name };
            _c.label = c.Label ? c.Label : c.Name;
            _cc.push(_c);
        });
        _self._model.columns = _cc;
        _self._rndrth(true);

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
    _self._fmtdynd = function (dd) {
        var _rr = [];
        $.each(dd.Items, function (i, d) {
            var _r = {}
            $.each(d.ColumnValues, function (j, cv) {
                _r[cv.Label ? cv.Label : cv.Name] = cv.Value;
            });
            _rr.push(_r);
        });
        return (_rr);
    }

    _self.NumRows = function () {
        return ($(_self._e + ' tbody tr').length);
    }
    _self._init();
}

