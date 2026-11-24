<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PresupuestoInicial.aspx.cs" Inherits="SIAP.PresupuestoInicial" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Carga de Presupuesto Inicial | SIAP</title>
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap5.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
    <style>
        .glass-card { background: white; border-radius: 1rem; box-shadow: 0 10px 30px rgba(0,0,0,0.05); padding: 1.75rem; border: 1px solid rgba(0,0,0,0.05); }
        .titulo-seccion { color: var(--color-guinda); font-weight: 700; }
        .form-label { font-weight: 600; color: var(--color-verde-oscuro); font-size: .85rem; }
        .num { text-align: right; white-space: nowrap; font-variant-numeric: tabular-nums; }
        .resumen .dato { font-size: 1.35rem; font-weight: 700; color: var(--color-verde-oscuro); }
        .resumen .etq { font-size: .75rem; text-transform: uppercase; color: var(--color-gris-oscuro); }
        .clave { font-family: Consolas, monospace; font-size: .78rem; word-break: break-all; }
        .icono-excel { color: #1D6F42; font-size: 1.3rem; }
        #tblErrores td, #tblAdvertencias td, #tblHistorial td { font-size: .82rem; vertical-align: middle; }
        .estructura span { display: inline-block; border: 1px solid var(--color-arena); border-radius: .35rem; padding: .1rem .4rem; margin: .1rem; font-size: .72rem; }
        .estructura b { color: var(--color-guinda); }
        /* DataTables: sin barra horizontal y botones Anterior/Siguiente completos */
        .dataTables_wrapper .row { margin-left: 0; margin-right: 0; }
        .dataTables_wrapper .row > [class*="col-"] { padding-left: 0; padding-right: 0; }
        .table-responsive { overflow-x: auto; overflow-y: hidden; }
        .dataTables_paginate .page-item.previous .page-link,
        .dataTables_paginate .page-item.next .page-link { width: auto; min-width: 5.5rem; border-radius: 1rem; padding: 0 .75rem; }
        .dataTables_length select { min-width: 75px; height: auto; padding: .3rem 1.8rem .3rem .6rem; font-size: .82rem; color: #333; display: inline-block; }
    </style>
    <script type="text/javascript">
        if (sessionStorage.getItem('name') == null) { window.location.href = "Login.aspx"; }
    </script>
    <script src="assets/js/SesionExpirada.js"></script>
</head>
<body class="g-sidenav-show">
    <form id="form1" runat="server" onsubmit="return false;">
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 bg-gradient-primary">
          <div class="container-fluid py-1 px-3">
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0">
                <li class="breadcrumb-item text-sm"><a class="opacity-8 text-white" href="Menu.aspx">Men&uacute;</a></li>
                <li class="breadcrumb-item text-sm text-white active">Presupuesto Inicial</li>
              </ol>
              <h6 class="font-weight-bolder text-white mb-0">Carga de Presupuesto Inicial</h6>
            </nav>
            <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
              <h3 style="color:white; margin:0 auto;">Secretar&iacute;a de Infraestructura</h3>
              <div class="ms-md-auto pe-md-3 d-flex align-items-center">
                <a href="Menu.aspx" class="btn btn-outline-white btn-sm mb-0 shadow-sm"><i class="fa-solid fa-arrow-left me-2"></i> Regresar</a>
              </div>
            </div>
          </div>
        </nav>

        <main class="main-content mt-4">
          <div class="container-fluid py-3">

            <!-- 1. Formulario de carga -->
            <div class="glass-card mb-4">
              <h5 class="titulo-seccion mb-3"><i class="fa-solid fa-file-arrow-up me-2"></i>Cargar presupuesto inicial</h5>
              <div class="row g-3 align-items-end">
                <div class="col-md-2">
                  <label class="form-label" for="cboAnio">A&ntilde;o</label>
                  <select id="cboAnio" class="form-control"></select>
                </div>
                <div class="col-md-3">
                  <label class="form-label" for="txtOficio">N&uacute;mero de oficio</label>
                  <input id="txtOficio" type="text" class="form-control" maxlength="100" placeholder="Ej. SH/SUBE/0001/2026" />
                </div>
                <div class="col-md-4">
                  <label class="form-label" for="fileExcel">Seleccione archivo (.xlsx, hoja MAESTRO)</label>
                  <input id="fileExcel" type="file" class="form-control" accept=".xlsx" />
                </div>
                <div class="col-md-3">
                  <button id="btnCargar" type="button" class="btn btn-primary w-100 mb-0"><i class="fa-solid fa-magnifying-glass-chart me-2"></i>Cargar archivo</button>
                </div>
              </div>
              <div id="estructura" class="estructura mt-3 text-muted small"></div>
            </div>

            <!-- 2. Resultado de la validación -->
            <div id="panelResultado" class="glass-card mb-4" style="display:none;">
              <div class="d-flex justify-content-between align-items-center mb-3">
                <h5 id="tituloResultado" class="mb-0"></h5>
                <div>
                  <button id="btnCancelar" type="button" class="btn btn-outline-secondary btn-sm mb-0 me-2">Cancelar</button>
                  <button id="btnImportar" type="button" class="btn btn-success btn-sm mb-0" style="display:none;"><i class="fa-solid fa-check me-2"></i>Confirmar importaci&oacute;n</button>
                </div>
              </div>

              <div class="row resumen text-center mb-3">
                <div class="col"><div class="etq">Claves</div><div class="dato" id="rClaves">0</div></div>
                <div class="col"><div class="etq">Aprobado</div><div class="dato" id="rAprobado">$0.00</div></div>
                <div class="col"><div class="etq">Liberado (meses)</div><div class="dato" id="rLiberado">$0.00</div></div>
                <div class="col"><div class="etq">No liberado</div><div class="dato" id="rNoLiberado">$0.00</div></div>
                <div class="col"><div class="etq">Filas con error</div><div class="dato" id="rErrores">0</div></div>
              </div>

              <div id="erroresGenerales"></div>

              <div id="bloqueErrores" style="display:none;">
                <h6 class="text-danger"><i class="fa-solid fa-circle-xmark me-1"></i>Corrija estas filas en el Excel y vuelva a cargarlo</h6>
                <div id="avisoTope" class="small text-muted mb-2"></div>
                <div class="table-responsive">
                  <table id="tblErrores" class="table table-sm table-striped w-100">
                    <thead><tr><th>Fila</th><th>Clave</th><th>Motivo</th></tr></thead>
                    <tbody></tbody>
                  </table>
                </div>
              </div>

              <div id="bloqueAdvertencias" style="display:none;" class="mt-3">
                <h6 style="color: var(--color-dorado);"><i class="fa-solid fa-triangle-exclamation me-1"></i>Valores que no est&aacute;n en su cat&aacute;logo (no impiden la carga)</h6>
                <div id="advertenciasGenerales"></div>
                <div class="table-responsive">
                  <table id="tblAdvertencias" class="table table-sm w-100">
                    <thead><tr><th>Segmento</th><th>Valor</th><th class="num">Claves</th></tr></thead>
                    <tbody></tbody>
                  </table>
                </div>
              </div>
            </div>

            <!-- 3. Historial -->
            <div class="glass-card">
              <h5 class="titulo-seccion mb-3"><i class="fa-solid fa-clock-rotate-left me-2"></i>Cargas realizadas</h5>
              <div class="table-responsive">
                <table id="tblHistorial" class="table table-hover w-100">
                  <thead>
                    <tr><th>A&ntilde;o</th><th>Oficio n&uacute;mero</th><th>Nombre del archivo</th><th class="num">Monto total aprobado</th>
                        <th class="num">Claves</th><th>Fecha de carga</th><th>Usuario</th></tr>
                  </thead>
                  <tbody></tbody>
                </table>
              </div>
              <div class="text-end small text-muted mt-2">Total de registros: <b id="totalRegistros">0</b></div>
            </div>
          </div>
        </main>
    </form>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js"></script>
    <script src="assets/js/PresupuestoInicial.js?v=2"></script>
</body>
</html>
