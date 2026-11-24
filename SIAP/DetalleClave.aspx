<%@ Page Language="C#" AutoEventWireup="true" %>
<script runat="server">
    // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
    protected void Page_Load(object sender, EventArgs e) { SIAP.Seguridad.Requerir(this); }
</script>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Detalle de Clave | SIAP</title>
    <!-- CSS and Fonts -->
    <link href="assets/css/googleapis.css" rel="stylesheet" />
    <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <script src="assets/js/jquery-3.5.1.js"></script>
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    
    <script type="text/javascript">
        // sesion autenticada
        var data = sessionStorage.getItem('name');
        if (data == null) {
            window.location.href = "Login.aspx";
        }
        function eliminarsesion() {
            sessionStorage.removeItem('name');
            window.location.href = "Login.aspx";
        }
    </script>
    <style>
        .glass-card { background: #fff; border-radius: 1rem; box-shadow: 0 10px 30px rgba(0,0,0,0.05); border: 1px solid rgba(0,0,0,0.05); }
        .sec-titulo { color: var(--color-guinda); font-weight: 700; font-size: .95rem; margin: 0; }
        .clave-grande { font-family: Consolas, monospace; font-size: 1.05rem; letter-spacing: .5px; color: var(--color-verde-oscuro); word-break: break-all; }
        .chip { display: inline-flex; flex-direction: column; border: 1px solid var(--color-arena, #e5d9c3); border-radius: .6rem; padding: .3rem .6rem; margin: .2rem .3rem .2rem 0; max-width: 320px; }
        .chip b { font-size: .64rem; text-transform: uppercase; color: var(--color-guinda); letter-spacing: .3px; }
        .chip span { font-size: .8rem; color: #333; line-height: 1.2; }
        .chip .sin-cat { color: #b7791f; }
        .kpi { padding: 1rem 1.2rem; height: 100%; }
        .kpi .etq { font-size: .7rem; text-transform: uppercase; font-weight: 700; color: var(--color-gris-oscuro, #6c757d); }
        .kpi .valor { font-size: 1.45rem; font-weight: 800; color: var(--color-verde-oscuro); font-variant-numeric: tabular-nums; margin: .15rem 0; }
        .kpi .sub { font-size: .74rem; color: #666; }
        .kpi.principal { background: linear-gradient(135deg, #235B4E, #10312B); }
        .kpi.principal .etq, .kpi.principal .sub { color: rgba(255,255,255,.8); }
        .kpi.principal .valor { color: #fff; font-size: 1.75rem; }
        .num { text-align: right; white-space: nowrap; font-variant-numeric: tabular-nums; }
        .barra { display: flex; height: 34px; border-radius: 8px; overflow: hidden; background: #eef0f2; }
        .barra > div { height: 100%; }
        .leyenda { display: grid; grid-template-columns: repeat(4, minmax(0, 1fr)); gap: .75rem; margin-top: .9rem; }
        .leyenda .item { border-left: 4px solid; padding-left: .6rem; }
        .leyenda .item b { display: block; font-size: .72rem; text-transform: uppercase; color: #555; }
        .leyenda .item span { font-size: 1rem; font-weight: 700; font-variant-numeric: tabular-nums; }
        .leyenda .item small { display: block; font-size: .72rem; color: #777; }
        .puente { display: flex; flex-wrap: wrap; align-items: center; gap: .4rem; font-size: .85rem; color: #444; }
        .puente .op { color: #999; font-weight: 700; }
        .puente .v { font-weight: 700; font-variant-numeric: tabular-nums; }
        .c-min { background: #9F2241; border-color: #9F2241 !important; }
        .c-apa { background: #E39B23; border-color: #E39B23 !important; }
        .c-dis { background: #235B4E; border-color: #235B4E !important; }
        .c-lib { background: #A7AAAD; border-color: #A7AAAD !important; }
        #tblMeses td, #tblMeses th { font-size: .8rem; vertical-align: middle; }
        #tblMeses tr.actual td { background: #eef6f3; font-weight: 700; }
        .mini { height: 8px; border-radius: 4px; background: #eef0f2; overflow: hidden; min-width: 90px; }
        .mini > div { height: 100%; background: #9F2241; }
        .tabla-chica td, .tabla-chica th { font-size: .78rem; vertical-align: middle; }
        .clave-txt { font-family: Consolas, monospace; font-size: .72rem; word-break: break-all; white-space: normal; }
        details.plegable > summary { cursor: pointer; list-style: none; display: flex; justify-content: space-between; align-items: center; }
        details.plegable > summary::-webkit-details-marker { display: none; }
        details.plegable > summary .flecha { transition: transform .2s; color: var(--color-guinda); }
        details.plegable[open] > summary .flecha { transform: rotate(90deg); }
        .badge-est { font-size: .66rem; padding: .25rem .5rem; border-radius: .4rem; }
        .obra { border-left: 4px solid #BC955C; background: #faf7f1; border-radius: .5rem; padding: .6rem .9rem; }
        .obra .proy { font-weight: 700; color: var(--color-verde-oscuro); font-size: .95rem; }
        .obra .etq-obra { font-size: .64rem; text-transform: uppercase; font-weight: 700; color: #8a6d3b; letter-spacing: .3px; }
        .obra ul { margin: .25rem 0 0; padding-left: 1.1rem; }
        .obra li { font-size: .8rem; color: #444; }
        .obra .nota { font-size: .76rem; color: #777; }
        @media (max-width: 768px) { .leyenda { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
    </style>
    <script src="assets/js/SesionExpirada.js"></script>
</head>
<body>
    <!-- Navbar (Píldora Flotante) -->
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Presupuesto_Inicial.aspx">Presupuesto Inicial</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Detalle de Clave</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0" id="lblClavePrincipal">Cargando Clave...</h6>
        </nav>
        <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
          <div class="ms-md-auto pe-md-3 d-flex align-items-center">
            <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                <i class="fa-solid fa-arrow-left me-2"></i> Regresar
            </a>
          </div>
        </div>
      </div>
    </nav>

    <!-- Main Content -->
    <main class="main-content mt-4">
        <div class="container-fluid py-3">

            <!-- 1. La clave -->
            <div class="glass-card p-3 mb-4">
                <div class="d-flex justify-content-between flex-wrap gap-2 align-items-start">
                    <div>
                        <div class="text-xs text-uppercase text-muted font-weight-bold">Clave presupuestal · Ejercicio <span id="lblEjercicio">—</span></div>
                        <div class="clave-grande" id="lblClave">Cargando…</div>
                    </div>
                </div>
                <div id="bloqueObra" class="obra mt-2" style="display:none;"></div>
                <div class="mt-2" id="chips"></div>
            </div>

            <div id="alertas"></div>

            <!-- 2. Lo que el usuario busca primero: el disponible -->
            <div class="row g-3 mb-4">
                <div class="col-xl-3 col-md-6">
                    <div class="glass-card kpi principal">
                        <div class="etq">Disponible en <span id="lblMesActual">—</span></div>
                        <div class="valor" id="kpiDisponibleMes">$0.00</div>
                        <div class="sub" id="subDisponibleMes">Calendarizado del mes menos lo ministrado</div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6">
                    <div class="glass-card kpi">
                        <div class="etq">Disponible liberado (todo el año)</div>
                        <div class="valor" id="kpiDisponible">$0.00</div>
                        <div class="sub">Liberado − ministrado − apartado</div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6">
                    <div class="glass-card kpi">
                        <div class="etq">Modificado</div>
                        <div class="valor" id="kpiModificado">$0.00</div>
                        <div class="sub" id="subModificado">Aprobado + aumentos − disminuciones</div>
                    </div>
                </div>
                <div class="col-xl-3 col-md-6">
                    <div class="glass-card kpi">
                        <div class="etq">Por liberar (mes cero)</div>
                        <div class="valor" id="kpiPorLiberar">$0.00</div>
                        <div class="sub">Se calendariza con una adecuación de liberación</div>
                    </div>
                </div>
            </div>

            <!-- 3. Cómo está el modificado -->
            <div class="glass-card p-3 mb-4">
                <div class="d-flex justify-content-between flex-wrap gap-2 mb-2">
                    <h6 class="sec-titulo">¿Cómo está el modificado?</h6>
                    <div class="puente" id="puente"></div>
                </div>
                <div class="barra" id="barraModificado"></div>
                <div class="leyenda">
                    <div class="item c-min" style="background:none;"><b>Ministrado</b><span id="lgMin">$0.00</span><small id="pcMin">0 %</small></div>
                    <div class="item c-apa" style="background:none;"><b>Apartado (adecuación en trámite)</b><span id="lgApa">$0.00</span><small id="pcApa">0 %</small></div>
                    <div class="item c-dis" style="background:none;"><b>Disponible</b><span id="lgDis">$0.00</span><small id="pcDis">0 %</small></div>
                    <div class="item c-lib" style="background:none;"><b>Por liberar</b><span id="lgLib">$0.00</span><small id="pcLib">0 %</small></div>
                </div>
                <div class="text-xs text-muted mt-3">
                    Ejercido: devengado <b id="lblDevengado">$0.00</b> · pagado <b id="lblPagado">$0.00</b>
                    <span class="ms-1">(se registran con el módulo de pagos).</span>
                </div>
            </div>

            <!-- 4. Calendario por mes -->
            <div class="row g-3 mb-4">
                <div class="col-lg-7">
                    <div class="glass-card p-3 h-100">
                        <h6 class="sec-titulo mb-2">Calendario por mes</h6>
                        <div class="table-responsive">
                            <table class="table table-sm mb-0" id="tblMeses">
                                <thead><tr><th>Mes</th><th class="num">Calendarizado</th><th class="num">Ministrado</th><th class="num">Disponible</th><th>Avance</th></tr></thead>
                                <tbody></tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>
                        <div class="text-xs text-muted mt-2">Los apartados todavía no se registran por mes; se descuentan del disponible del año.</div>
                    </div>
                </div>
                <div class="col-lg-5">
                    <div class="glass-card p-3 mb-3" id="cardApartados" style="display:none;">
                        <h6 class="sec-titulo mb-2"><i class="fa-solid fa-hourglass-half me-2" style="color:#E39B23;"></i>Apartados activos</h6>
                        <div id="listaApartados"></div>
                    </div>
                    <div class="glass-card p-3">
                        <h6 class="sec-titulo mb-2">Historial de ministraciones</h6>
                        <div class="table-responsive">
                            <table class="table table-sm tabla-chica mb-0" id="tblMinistraciones">
                                <thead><tr><th>Folio</th><th>Mes</th><th class="num">Importe</th><th>Estatus</th></tr></thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 5. Historial de adecuaciones -->
            <div class="glass-card p-3 mb-4">
                <h6 class="sec-titulo mb-2">Historial de adecuaciones</h6>
                <div class="table-responsive">
                    <table class="table table-sm tabla-chica mb-0">
                        <thead><tr><th>Oficio</th><th>Fecha</th><th>Tipo de operación</th><th class="num">Monto</th><th></th></tr></thead>
                        <tbody id="tbodyHistorialAdec"><tr><td colspan="5" class="text-center text-muted">Cargando…</td></tr></tbody>
                    </table>
                </div>
            </div>

            <!-- 6. Información secundaria, plegada -->
            <details class="glass-card p-3 mb-3 plegable" id="detProyecto">
                <summary>
                    <h6 class="sec-titulo">Otras claves del mismo proyecto <span class="text-muted font-weight-normal" id="lblNumProyecto"></span></h6>
                    <i class="fa-solid fa-chevron-right flecha"></i>
                </summary>
                <div class="text-xs text-muted mt-2" id="lblPrefijo"></div>
                <div class="table-responsive mt-2">
                    <table class="table table-sm tabla-chica mb-0">
                        <thead><tr><th>Partida</th><th>Municipio</th><th>Fondo</th><th class="num">Modificado</th><th class="num">Por liberar</th><th></th></tr></thead>
                        <tbody id="tbodyProyecto"></tbody>
                    </table>
                </div>
            </details>

            <details class="glass-card p-3 mb-4 plegable">
                <summary>
                    <h6 class="sec-titulo">Desglose de la clave <span class="text-muted font-weight-normal" id="lblNumSegmentos"></span></h6>
                    <i class="fa-solid fa-chevron-right flecha"></i>
                </summary>
                <div class="table-responsive mt-2">
                    <table class="table table-sm tabla-chica mb-0">
                        <thead><tr><th>Segmento</th><th>Abrev.</th><th>Valor</th><th>Descripción</th></tr></thead>
                        <tbody id="contenidoDesglose"></tbody>
                    </table>
                </div>
            </details>
        </div>
    </main>

    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="assets/js/DetalleClave.js?v=2"></script>
</body>
</html>
