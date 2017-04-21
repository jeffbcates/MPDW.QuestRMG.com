if (!window.qrmgmvc) {
    qrmgmvc = {};
}
qrmgmvc.AppRoot = function (approot) {
    if (approot) {
        qrmgmvc._approot = approot ? approot : '/';
        qrmgmvc._approot = qrmgmvc._approot.charAt(qrmgmvc._approot.length - 1) == '/' ? qrmgmvc._approot.substr(0, qrmgmvc._approot.length - 1) : qrmgmvc._approot;
    }
    else {
        return (qrmgmvc._approot);
    }
}

if (!qrmgmvc.Global) {
    qrmgmvc.Global = {};
}
qrmgmvc.Global.Maskit = function (e, t) {
    Maskit(e, t);
}
qrmgmvc.Global.Mask = function (e, msg) {
    Maskit(e, true);
}
qrmgmvc.Global.Unmask = function (e) {
    Unmaskit(e);
}

