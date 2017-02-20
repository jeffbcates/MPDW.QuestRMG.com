// qrmgTreeview.js
function qrmgTreeview(model) {
    var _self = this;
    _self._model = model;
    _self._e = model.element.substring(0, 1) == '#' ? model.element : '#' + model.element;
    _self._pfx = model.pfx;
    _self._uri = model.uri.charAt(model.uri.length - 1) == '/' ? model.uri : model.uri + '/';
    _self._callback = model.callback;
    _self._bMasking = _self._model.mask || !_self._model.bNoMasking;
    _self._tvw;
    _self._kn;
    _self._bInit = false;
    _self.draggableOptions = {
        start: function (event, ui) {
            console.log("draggableOptions start ");
            _self.draggingObjs = $('li.tvwSelected', _self._e);
        },
        ////helper: 'clone',
        ////revert: 'invalid',
        helper: function (e) {
            console.log("draggableOptions helper ");
            var selected = $('li.tvwSelected', _self._e);
            if (selected.length === 0) {
                selected = $(this);
            }
            var container = $('#draggingContainer').length > 0 ? $('#draggingContainer') : $('<div/>').attr('id', 'draggingContainer');
            container.empty().append(selected.clone());
            return container;
        },
        drag: function (event, ui) {
            console.log("draggableOptions drag ");
            var currentLoc = $(this).position();
            var prevLoc = $(this).data('prevLoc');
            if (!prevLoc) {
                prevLoc = ui.originalPosition;
            }

            var offsetLeft = currentLoc.left - prevLoc.left;
            var offsetTop = currentLoc.top - prevLoc.top;

            _self.moveSelected(offsetLeft, offsetTop);
            $(this).data('prevLoc', currentLoc);
        },
        containment: $('#pageContentFrame'),
        appendTo: 'body'
    };

    _self._init = function () {
        _self._kn = _self._model.keyname || 'Id'; 
        _self._initctx()
        _self._mask = _self._model.mask || _self._e;
        _self.Mask();
        if (!_self._model.contextMenus) { _self._model.contextMenus = [] }
        if (!_self._model.events) { _self._model.events = [] }
        _self._render();
        _self.Unmask();
    }
    _self._initctx = function () {
        _self._ctx = new qrmgctx(_self._model.ctx);
    }

    _self._render = function (Data, bFill) {
        $(_self._e).treeview({
            data: Data || {},
            multiSelect: _self._model.multiSelect,
            onTreeRender: _self._onrndr
        });
        _self._tvw = _self._gettvw();
        if (!bFill) {
            _self._rndrhdr();
        }
        _self.UpdateHeader();
        _self.Draggable();
        _self.Droppable();
        _self.Sortable();
        _self.Selectable();
    }

    _self.Draggable = function (b) {
        if (_self._model.draggable) {
            $(_self._e).find('li').addClass('tvwDraggable').draggable(
                _self.draggableOptions
            );
        }
    }
    _self.moveSelected = function (ol, ot) {
        ////console.log("moving to: " + ol + ":" + ot);
        _self.draggingObjs.each(function () {
            $this = $(this);
            var p = $this.position();
            var l = p.left;
            var t = p.top;
            ////console.log({ id: $this.attr('id'), l: l, t: t });

            $this.css('left', l + ol);
            $this.css('top', t + ot);
        })
    }
    _self.Droppable = function (b) {
        if (_self._model.droppable) {
            $(_self._e).droppable({
                drop: function (e, ui) {
                    console.log("droppable drop: ");
                    if ($(ui.draggable).closest('div.treeview').attr('id') == _self._e.substr(1)) {
                        _self.Refresh();
                        return;
                    }
                    var _evt = _self._getevt("OnDrop");
                    if (_evt != null) {
                        if (_evt.callback("OnDrop", ui)) {
                            return;
                        }
                    }
                    var ee = $(ui.helper).find('li');
                    ////$(ee).detach().appendTo($(this).find('ul.list-group'));
                    if (_self._model.dropsource) {
                        var _nn = [];
                        $.each(ee, function (i, e) {
                            var id = parseInt(e.dataset.id);
                            var n = _self._model.dropsource.GetNode(id);
                            _nn.push(n);
                        });
                        var _d = { questStatus: { Severity: 1, Message: "" }, Items: _nn };
                        _self.Insert(_d);
                        _self.Refresh();

                        $.each(_nn, function (i, n) {
                            _self._model.dropsource.Remove(n.Id, 'Id');
                        });
                        _self._model.dropsource.Refresh();
                    }
                    else {
                        alert('TODO: implement drag-n-drop NOT using a dropsource.');
                    }
                    $(ee).detach().remove();
                }
            });
        }
    }
    _self.DropSource = function (s) {
        if (s) {
            _self._model.dropsource = s;
        }
        else {
            return (_self._model.dropsource);
        }
    }
    _self.Sortable = function (b) {
        if (_self._model.sortable) {
            $(_self._e).find('ul.list-group').sortable();
        }
    }
    _self.Sort = function (nodes) {
        console.log('qrmgTreeview.Sort' + _self._e.substr(1));
        var nn = nodes || _self._tvw.Nodes;
        try {
            nn.sort(function (a, b) {
                var ta = a.text;
                var tb = b.text;
                if (ta > tb) {
                    return 1;
                }
                if (ta < tb) {
                    return -1;
                }
                return 0;
            });
        }
        catch (e) {
            alert('EXCEPTION: sorting ' + _self._e.substr(1) + ': ' + e);
        }
        return (_self._tvw.Nodes);
    }

    _self.Selectable = function (b) {
        if (_self._model.selectable) {
            $(_self._e).find('ul.list-group').selectable();
        }
    }

    _self._setcnt = function() {
        _self.Count = $(_self._e).find('ul.list-group li').length;
    }
    _self._rndrhdr = function () {
        if (!_self._model.header) { return; }
        var _h = [], _i = 0;
        _h[_i++] = '<div class="' + _self._e.substr(1) + ' ' + (_self._model.header.bInlineHeader ? 'questTableInlineHeaderInfo' : ' questTableHeaderInfo ') + (_self._model.header.classes ? (' ' + _self._model.header.classes + ' ') : '') + '">';
        if (_self._model.header.count) {
            _h[_i++] = '    <div class="questTableTotalRecords">';
            _h[_i++] = '        <label class="control-label">' + (_self._model.header.label || '') + '</label>';
            _h[_i++] = '        <span class="questTableTotalRecordCount"><label class="control-label tvwCount"></label></span>';
            _h[_i++] = '    </div>';
        }
        if (_self._model.header.filter) {
            _h[_i++] = '    <div class="questTableFilter">';
            _h[_i++] = '        Filter:';
            _h[_i++] = '        <input type="text" class="tvwFilter">';
            _h[_i++] = '    </div>';
        }
        _h[_i++] = '</div>';
        _h[_i++] = '    <div class="questTreeviewCommands">';
        _h[_i++] = _self._rndrcmds();
        _h[_i++] = '    </div>';
        $(_self._e).before(_h.join(''));
    }
    _self._rndrcmds = function () {
        var _h = [], _i = 0;
        _self._model.commands = _self._model.commands || [];
        _self._model.commands.push({ name: 'Refresh', classes: 'fa fa-refresh', title: 'Fresh Treevie', callback: _self.Refresh });
        $.each(_self._model.commands, function (i, c) {
            _h[_i++] = '<span data-name="' + c.name + '" class="tvwcmd ';
            if (c.classes) {
                _h[_i++] = ' ' + c.classes + ' ';
            }
            _h[_i++] = '" ';
            if (c.title) {
                _h[_i++] = ' title="' + c.title + '" ';
            }
            _h[_i++] = '></span>';
        });
        return(_h.join(''));
    }
    _self._onrndr = function () {
        _evt = _self._getevt("OnTreeRender");
        if (_evt != null) {
            if (_evt.callback("OnTreeRender"));
        }
        if (_self._model.draggable) {
            _self.Draggable();
        }
        if (_self._model.droppable) {
            _self.Droppable();
        }
    }

    _self._bind = function () {
        var _evt;
        var n;
        $(_self._e).on('nodeSelected', function (e, d) {
            _evt = _self._getevt("NodeSelected");
            if (_evt != null) {
                _evt.callback({ NodeSelected: true }, d, e);
            }
            n = d;
        });
        _evt = _self._getevt("Click");
        if (_evt != null) {
            $(_self._e).find('ul.list-group li').on('click', null, n, function (e) {
                if (_evt.callback({ Click: true, node: n, event: e })) {
                    e.stopPropagation();
                    e.preventDefault();
                }
            });
        }
        if (_self._model.header.filter) {
            _self._bndfltr();
        }
        _self._bndcmds();
    }
    _self._bndfltr = function () {
        $(_self._e).prev().find('.tvwFilter').on('keyup', function (e) {
            _self.Filter($(_self._e).prev().find('.tvwFilter').val());
        });
    }
    _self._bndcmds = function () {
        var cc = $(_self._e).prev().find('.tvwcmd');
        $.each(cc, function (i, c) {
            $(c).on('click', null, c, function (e) {
                var _c = _self._getcmd($(c).attr('data-name'));
                if (_c.callback) {
                    var ss = $('.tvwSelected', _self._e);
                    if (_c.callback(_c, ss)) { return; }
                }
            });
        });
    }

    _self._docallback = function (ud, d) {
        if (_self._cb) {
            return (_self._cb(ud, d));
        }
    }

    _self.Load = function (Data) {
        _self.Mask();
        if (!Data) {
            var _d = _self._getctx();
            var _url = _self._bldurl(_self._uri + '/Load');
            var _io = new qrmgio(_self._rload);
            _io.GetJSON(_url, _d, 'Load');
        }
        else {
            _self._load(Data);
            _self.Unmask();
        }
    }
    _self._load = function (Data) {
    }
    _self._rload = function (ud, d) {
        if (!IsAppSuccess(d)) {
            DisplayUserMessage(d);
        }
        if (_self._docallback(ud, d)) {
            return;
        }
        _self._render(d.Items, true);
        _self._bind();
        if (!_self._bInit) {
            _self._setiv();
            _self._bInit = true;
        }
        _self.Unmask();
    }

    _self.Insert = function (Data) {
        var _nn = _self._tvw.Tree;
        var _ii = Data.Items;
        var _aa = [];
        _aa = _aa.concat(_nn);
        _aa = _aa.concat(_ii);
        Data.Items = _aa;
        if (_self._model.sorted) {
            _self.Sort(Data.Items);
        }
        _self._rload('Load', Data);
    }
    _self.Fill = function (Data) {
        if (_self._model.sorted) {
            _self.Sort(Data.Items);
        }
        _self._rload('Load', Data);
    }
    _self.Remove = function (Id) {
        var n = _self.GetNode(Id);
        if (!n) { return (false); }
        $(_self._e).find('ul.list-group li[data-id="' + n.Id + '"]').remove();
        _self._removen(n.Id);
        _self.UpdateHeader();
    }
    _self._removen = function (Id) {
        var idx = -1;
        $.each(_self._tvw.Nodes, function (i, n) {
            if (n.Id == Id) {
                idx = i; return (false);
            }
        });
        if (idx > -1) {
            _self._tvw.Nodes.splice(idx, 1);
        }
        idx = -1
        $.each(_self._tvw.Tree, function (i, n) {
            if (n.Id == Id) {
                idx = i; return (false);
            }
        });
        if (idx > -1) {
            _self._tvw.Tree.splice(idx, 1);
        }
    }

    _self.UpdateHeader = function () {
        _self._setcnt();
        $(_self._e).parent().find('.' + _self._e.substr(1)).find('.tvwCount').text(_self.Count);
    }
    _self._setiv = function () {
        var _vs = _self.ViewState();
        if (!_vs || !_vs[_self._model.viewstate.field]) { return; }
        _self.Select(_vs[_self._model.viewstate.field]);
    }
    _self.ViewState = function () {
        if (!_self._model.viewstate) { return;}
        var _vs = $.parseJSON($('#' + _self._model.viewstate.element).val());
        return (_vs);
    }

    _self.Select = function (id, bExpand) {
        var n = _self.GetNode(id);
        if (!n) { return; }
        _tvwCategories._tvw.selectNode(n.nodeId);
        _tvwCategories._tvw.revealNode(n);
        if (n.nodes.length) {
            _tvwCategories._tvw.expandNode(n);
        }
        return (n);
    }
    _self.ClearSelected = function () {
        $('.tvwSelected', _self._e).removeClass('tvwSelected');
    }

    _self._gettvw = function () {
        return($.data($(_self._e)[0], "treeview"));
    }
    _self.GetNode = function (id, p) {
        var _n;
        var _p = 'Id' || p;
        $.each(_self._tvw.Nodes, function (i, n) {
            if (id == n[_p]) {
                _n = n; 
                return (false);
            }
        });
        return(_n);
    }
    _self.GetNodeKlugie = function (itm) {
        var _n;
        $.each(_self._tvw.Nodes, function (i, n) {
            if (itm.Entity.type == n.type && itm.Entity.Name == n.Name) {
                if (itm.ParentEntity.type == n.parentType) {
                    _n = n;
                    return (false);
                }
            }
        });
        return (_n);
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
    _self._getcmd = function (n) {
        var _c = null;
        $.each(_self._model.commands, function (i, c) {
            if (c.name == n) {
                _c = c;
                return (false);
            }
        });
        return (_c);
    }

    _self.Refresh = function () {
        console.log('qrmgTreeview.Refresh' + _self._e.substr(1));
        var _d = { questStatus: _viewstate.questStatus, Items: _self._tvw.Nodes };
        _self.Fill(_d);
        if (_self._model.sorted) {
            _self.Sort();
        }
        var _evt = _self._getevt("Refresh");
        if (_evt != null) {
            if (_evt.callback("Refresh")) {
                return;
            }
        }
    }

    _self._getctx = function () {
        var _ctx = _self._ctx.Context();
        return (_ctx);
    }
    _self.Context = function () {
        var ctx = {};
        ctx[_self._kn] = _self._getselv();
        return (ctx);
    }
    _self._getselv = function () {
        var _s = $(_self._e).treeview('getSelected');
        if (_s.length) {
            return(_s[0].Id);
        }
        return(_s.length ? _s[0].Id : undefined);
    }

    _self._bldurl = function (uri) {
        var _ctx = _self._ctx.Context();
        return (uri += '?' + $.param(_ctx));
    }

    _self.Valid = function (bV) {
        bV ? $('#tvwCategories').removeClass('validationError') : $('#tvwCategories').addClass('validationError');
    }
    _self.Validate = function () {
        var kv = _self._getselv();
        if (kv !== undefined) {
            $(_self._e).removeClass('validationError');
            return (true);
        }
        $(_self._e).addClass('validationError');
        return (false);
    }
    _self.ClearSelected = function () {
        $(_self._e).find('ul.list-group li.tvwSelected').removeClass('tvwSelected');
    }
    _self.Clear = function () {
        var _d = { questStatus: _viewstate.questStatus, Items: [] };
        _self.Fill(_d);
    }
    _self.Filter = function (v) {
        $(_self._e).find('ul.list-group li').each(function (i, e) {
            if ($(this).text().indexOf(v) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }

    _self.GetData = function () {
        var nodes = [];
        $.each(_self._tvw.Nodes, function (i, n) {
            nodes.push({ Id: n.Id, ParentId: n.ParentId, state: n.state, tags: n.tags, text: n.text, type: n.type, parentType: n.parentType, Schema: n.Schema, Name: n.Name});
        });
        return (nodes);
    }

    _self.Mask = function (e, msg) {
        if (!_self._bMasking) { return; }
        Mask(e || _self._mask, null, msg);
    }
    _self.Unmask = function (e, bCM) {
        if (!_self._bMasking) { return; }
        Unmask(e || _self._mask, null, bCM);
    }

    _self._init();
}
