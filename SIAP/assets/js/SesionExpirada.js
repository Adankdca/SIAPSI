/* Si el servidor responde 401 (sesión expirada) a cualquier llamada Ajax o fetch,
   se limpia la sesión del navegador y se manda al Login. Se carga en el <head> de cada página. */
(function () {
    'use strict';
    var redirigiendo = false;
    function irAlLogin() {
        if (redirigiendo) return;
        redirigiendo = true;
        try { sessionStorage.clear(); } catch (e) { }
        window.location.href = 'Login.aspx?expirada=1';
    }

    // fetch
    if (window.fetch && !window.fetch.__siap) {
        var original = window.fetch;
        var envuelto = function () {
            return original.apply(this, arguments).then(function (r) {
                if (r && r.status === 401) irAlLogin();
                return r;
            });
        };
        envuelto.__siap = true;
        window.fetch = envuelto;
    }

    // jQuery (en algunas páginas se carga después de este script)
    var enlazado = false;
    function enlazarJQuery() {
        if (enlazado || !window.jQuery) return;
        enlazado = true;
        window.jQuery(document).ajaxError(function (e, xhr) {
            if (xhr && xhr.status === 401) irAlLogin();
        });
    }
    enlazarJQuery();
    document.addEventListener('DOMContentLoaded', enlazarJQuery);
    window.addEventListener('load', enlazarJQuery);
})();
