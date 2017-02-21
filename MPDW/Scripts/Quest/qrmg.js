/********************************************************************************
* File: qrmg.js 
*
* Description: General-purpose javascript routines.
********************************************************************************/

// NOTE: HOW TO USE.
// 
// 1. Uncomment the stage you are targeting.
// 2. Update the Version for the stage.
// 3. Deploy

var __STAGES = {

    // 1. Uncomment ONE of these.
    CurrentStage: 'DEV',
    ////CurrentStage: 'STAGE',
    ////CurrentStage: 'PROD',

    // 2. Update the Version to the stage accordingly...
    stages: [
        { name: 'DEV', Title: 'Andoo Dev', Path: '', Version: 'v.1.0.20160705001', EventServer: 'http://localhost:63143' },
        { name: 'STAGE', Title: 'Andoo STAGE', Path: '', Version: 'v.1.0.20160705001', EventServer: 'http://localhost:63243' },
        { name: 'PROD', Title: 'Andoo PROD', Path: '', Version: 'v.1.0.20160705001', EventServer: 'http://localhost:63343' },
    ],

    // 3. Edit web.config, make sure EventServerBaseURI is pointing to correct server.

    // 4. Deploy.



    get: function (name) {
        var _stage;
        $.each(this.stages, function (i, s) {
            if (name == s.name) {
                _stage = s;
                return (false);
            }
        });
        return (_stage);
    }
}


function GetAppTitle() {
    return (__STAGES.get(__STAGES.CurrentStage).Title);
}
function GetCurrentVersion() {
    return (__STAGES.get(__STAGES.CurrentStage).Version);
};
function GetCurrentPath() {
    return (__STAGES.get(__STAGES.CurrentStage).Path);
}
function GetEventServerPath() {
    return (__STAGES.get(__STAGES.CurrentStage).EventServer);
}

function confirmPrompt(prompt) {
    var agree = confirm(prompt);
    if (agree) { return true; }
    else { return false; }
};
String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}
function DisplayUserMessage(d, frm) {
    var m, mm, msg;
    $("#questumsgF").addClass("questumsgFrameHeight");
    if (d.InvalidFields && d.InvalidFields.length > 0) {
        var _h = [], _i = 0;
        _h[_i++] = '<span id="userErrorTitle">Errors!</span>';
        _h[_i++] = '<ul id="userErrorList">';
        $.each(d.UserMessages, function (index, item) {
            var mp = item.split('|');
            var _msg = mp[1];
            var _fid = mp[2] ? mp[2] : d.InvalidFields[index]
            var _label = _fid;
            if (frm) {
                _label = frm.GetLabel(_fid) || _fid;
            }
            _h[_i++] = '<li id="err_' + _fid + '"><b>' + _label + '</b> required</li>';
        });
        _h[_i++] = '</ul>';
        $('#__umsg').empty().append(_h.join('')).addClass('umsgContent').focus();
        $('#userErrorList li').click(function (e) {
            var errorId = "#" + $(this).attr('id').substring(4);
            $(errorId).focus();
        });
        classMessage('#__umsg', d.questStatus.Severity);
        return;
    }
    else if (d.questStatus) {
        var sev;
        switch (d.questStatus.Severity) {
            case 1:
                sev = "I";
                break;
            case 2:
                sev = "W";
                break;
            case 3:
                sev = "E";
                break;
            case 4:
                sev = "F";
                break;
            default:
                sev = "?";
                break;
        }
        msg = sev + '|' + d.questStatus.Message;
    }
    else {
        msg = d;
    }
    try {
        m = msg.split('|');
        if (m.length == 1) {
            mm = m;
        }
        else {
            mm = m[0] + '|' + m[1];
        }
    }
    catch (err) {
        mm = msg;
    }
    DisplayUserMessageEx($('#__umsg'), mm);
}
function DisplayUserMessageEx(divid, msg) {
    ClearMessage(divid);
    var mp;
    if (!msg) {
        return;
    }
    else if (msg instanceof Array) {
        if (msg.length < 1) {
            return;
        }
        else if (msg.length == 1) {
            classMessage(divid, 'E');
            $(divid).text(mapMessage(msg[0]));
        }
        else if (msg.length >= 2) {
            classMessage(divid, msg[0]);
            $(divid).text(mapMessage(msg[1]));
        }
    }
    else if (msg.Content) {
        mp = msg.Content.split('|');
        if (mp.length == 1) {
            classMessage(divid, 'E');
            $(divid).text(mapMessage(mp[0]));
        }
        else if (mp.length >= 2) {
            classMessage(divid, mp[0]);
            $(divid).text(mapMessage(mp[1]));
        }
    }
    else if (msg.UserMessage) {
        mp = msg.UserMessage.split('|');
        if (mp.length == 1) {
            classMessage(divid, 'E');
            $(divid).text(mapMessage(mp[0]));
        }
        else if (mp.length >= 2) {
            classMessage(divid, mp[0]);
            $(divid).text(mapMessage(mp[1]));
        }
    }
    else {
        try {
            mp = msg.split('|');
        }
        catch (err) {
            return;
        }
        if (mp.length == 1) {
            classMessage(divid, 'E');
            $(divid).text(mapMessage(mp[0]));
        }
        else if (mp.length >= 2) {
            classMessage(divid, mp[0]);
            $(divid).text(mapMessage(mp[1]));
        }
    }
}
function getMsgText(m) {
    if (m.Content) {
        return (m.Content.substring(2));
    }
    else {
        mp = m.split('|');
        if (mp.length == 1) {
            return (mp[1]);
        }
    }
}
function mapMessage(msg) {
    if (msg == 'timeout') {
        return ('The server is unavailable.');
    }
    return (msg);
}
function ClearUserMessage() {
    ClearMessage('#__umsg');
}
function ClearMessage(divid) {
    var _d = $(divid);
    $(_d).removeClass('errorDefSeverityI');
    $(_d).removeClass('errorDefSeverityW');
    $(_d).removeClass('errorDefSeverityE');
    $(_d).removeClass('errorDefSeverityF');
    $(_d).removeClass('umsgContent');
    $(_d).html('&nbsp;');
}

/*================================================================================
* Routine: classUserMessage
*
* Description: Apply class to user message accordingly.
*===============================================================================*/
function classUserMessage(mp) {
    classUserMessage($('#__umsg'), mp);
}
function classMessage(divid, sev) {
    $(divid).removeClass('errorDefSeverityI errorDefSeverityW errorDefSeverityE errorDefSeverityF');
    if (sev == 'I' || sev == 1) {
        $(divid).addClass('errorDefSeverityI');
    }
    else if (sev == 'W' || sev == 2) {
        $(divid).addClass('errorDefSeverityW');
    }
    else if (sev == 'E' || sev == 3) {
        $(divid).addClass('errorDefSeverityE');
    }
    else if (sev == 'F' || sev == 4) {
        $(divid).addClass('errorDefSeverityF');
    }
}
function isHTTPError(r) {
    if (r == undefined) {
        return (false);
    }
    if ((r >= 400) && (r <= 600)) {
        return (true);
    }
    return (false);
}
function IsUserMessage(m) {
    if ((m === undefined) || (m === null)) { return (false); }
    if (m.questStatus !== undefined) {
        return (m.questStatus.Message != null);
    }
    if (m.length < 2) { return (false); }
    if ((m.Content && m.Content.length >= 2) && (m.Content[1] == '|')) { return (true); }
    if (m[1] == '|') { return (true); }
    return (false);
}
function isSeverity(m, s) {
    var _m = m ? m : m.UserMessage;
    if (_m.substring) {
        return (_m.substring(0, 2) === s);
    }
}
function IsAppSuccess(m) {
    if (m == undefined) { return (false) }
    if (m.questStatus !== undefined) {
        return (m.questStatus.Severity == 1);
    }
    if (m.length < 2) { return (false) }
    return (isSeverity(m, 'I|'));
}
function IsAppWarning(m) {
    if (m == undefined) { return (false) }
    if (m.questStatus !== undefined) {
        return (m.questStatus.Severity == 2);
    }
    if (m.length < 2) { return (false) }
    return (isSeverity(m, 'W|'));
}
function IsAppError(m) {
    if (m == undefined) { return (false) }
    if (m.questStatus !== undefined) {
        return (m.questStatus.Severity == 3);
    }
    if (m.length < 2) { return (false) }
    return (isSeverity(m, 'E|'));
}
function IsAppFatal(m) {
    if (m == undefined) { return (false) }
    if (m.questStatus !== undefined) {
        return (m.questStatus.Severity == 4);
    }
    if (m.length < 2) { return (false) }
    return (isSeverity(m, 'F|'));
}

function GetUMessageSev(msg) {
    return (msg.split('|')[0]);
}
function GetUMessageText(msg) {
    return (msg.split('|')[1]);
}
function GetUMessageId(msg) {
    return (msg.split('|')[2]);
}

function bIsTrue(value) {
    var _trueValues = [true, 'true', '1'];
    return (_trueValues.indexOf(valuestr.toLowerCase()) > -1);
}
function GetURLArgs() {
    var aa = {};
    var args = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < args.length; i++) {
        var a = args[i].split('=');
        aa[a[0]] = a[1];
    }
    return (aa);
}
function RemoveQSParam(p) {
    var url = document.location.href;
    var urlparts = url.split('?');
    if (urlparts.length >= 2) {
        var urlBase = urlparts.shift();
        var queryString = urlparts.join("?");

        var prefix = encodeURIComponent(p) + '=';
        var pars = queryString.split(/[&;]/g);
        for (var i = pars.length - 1; i >= 0; i--) {
            if (pars[i].lastIndexOf(prefix, 0) !== -1) {
                pars.splice(i, 1);
            }
        }
        url = urlBase + '?' + pars.join('&');
        window.history.pushState('', document.title, url);
    }
}

function bValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^[_a-zA-Z0-9-]+(.[_a-zA-Z0-9-]+)*@[_a-zA-Z0-9-]+[.]([a-zA-Z0-9-]+)*([a-zA-Z])$/i);
    return pattern.test(emailAddress);
};
function bValidNumber(number) {
    var pattern = new RegExp(/^\d{1,3}(?:,\d{3})*$/i);
    return pattern.test(number);
};
function formatNumber(number) {
    return number.toString().replace(/\./g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function GetViewState(e) {
    return($.parseJSON($('#' + e).val()));
}
function SetViewState(e, f, v) {
    var _e = $('#' + e);
    if (!_e.length) { return;}
    $('#' + f, _e).val(v);
}

/*================================================================================
* Masking
*===============================================================================*/
function Maskit(e, t) {
    var _e = e ? $(e) : $('#content');
    if (!_e) { return (false); }
    $(_e).append('<div class="questmask"></div>');
}
function Unmaskit(e) {
    $('.questmask').remove();
}
function Mask(e, c, umsg) {
    var _c = c || 'questmask';
    var redraw = $(e).offsetHeight;
    $(e).append('<div class="questmask"></div>').show();
    if (umsg) {
        DisplayUserMessage(umsg);
    }
}
function Unmask(e, c, bCM) {
    var _c = c || 'questmask';
    $(e).find('.questmask').remove();
    if (bCM) {
        ClearUserMessage();
    }
}

/*================================================================================
* Clipboard
*===============================================================================*/
var ClipboardHelper = {
    copyElement: function ($element) {
        this.copyText($element.text())
    },
    copyText: function (text) // Linebreaks with \n
    {
        var $tempInput = $("<textarea>");
        $("body").append($tempInput);
        $tempInput.val(text).select();
        document.execCommand("copy");
        $tempInput.remove();
    }
};


/*================================================================================
* TEMPORARY: Max lengths for given datatypes.
*===============================================================================*/
function maxclen(c) {
    var _mxl = '';
    switch (c.Type) {
        case 'int':
            _mxl = '10';
            break;
        case 'varchar':
            _mxl = c.ColumnSize;
            break;
        case 'nvarchar':
            _mxl = c.ColumnSize;
            break;
        case 'datetime':
            _mxl = '20';
            break;
    }
    return (_mxl);
}
function maxlength(datatype) {
    var _datatype = datatype.toString().toLowerCase();
    if (_datatype == 'int') {
        return (10);
    }
    else if (_datatype == 'short') {
        return (5);
    }
    else if (_datatype == 'char') {
        return (3);
    }
    else if (_datatype == 'nvarchar') {  // temp
        return (255);
    }
    else if (_datatype == 'bit') {  // temp
        return (255);
    }
    return ('');
}
function size(datatype) {
    return (maxlength(datatype) + 2);
}
