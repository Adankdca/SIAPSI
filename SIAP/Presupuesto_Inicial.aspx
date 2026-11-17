<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Presupuesto_Inicial.aspx.cs" Inherits="SIAP.Presupuesto_Inicial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Presupuesto Inicial | SIAP</title>
    
    <!-- CSS de Argon y FontAwesome -->
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap5.min.css" />
    
    <!-- Scripts base -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
    
    <style>
        .glass-card { background: white; border-radius: 1rem; box-shadow: 0 10px 30px rgba(0,0,0,0.05); padding: 2rem; border: 1px solid rgba(0,0,0,0.05); }
        .nav-tabs .nav-link { border-radius: 0.5rem 0.5rem 0 0; font-weight: 600; }
        table.dataTable { border-collapse: collapse !important; }
        .form-label { font-weight: 600; color: var(--color-verde-oscuro); font-size: .78rem; margin-bottom: .2rem; }
        .totales .dato { font-size: 1.15rem; font-weight: 700; color: var(--color-verde-oscuro); font-variant-numeric: tabular-nums; }
        .totales .etq { font-size: .7rem; text-transform: uppercase; color: var(--color-gris-oscuro); }
        #tablaPresupuesto td { font-size: .78rem; vertical-align: top; }
        #tablaPresupuesto th { font-size: .68rem; text-transform: uppercase; color: var(--color-gris-oscuro); }
        #tablaPresupuesto tbody tr { cursor: pointer; }
        .clave-txt { font-family: Consolas, monospace; font-size: .72rem; word-break: break-all; white-space: normal; display: inline-block; width: 250px; }
        #tablaPresupuesto td { white-space: normal; }
        #tablaPresupuesto td.num { white-space: nowrap; }
        .clave-mini { font-family: Consolas, monospace; font-size: .72rem; word-break: break-all; }
        .seg-v { font-weight: 700; color: var(--color-verde-oscuro); }
        .seg-d { display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical; overflow: hidden; color: #555; max-width: 210px; white-space: normal; line-height: 1.25; }
        .sin-cat { color: #b7791f; font-weight: 600; }
        .num { text-align: right; white-space: nowrap; font-variant-numeric: tabular-nums; }
        .clave-input { font-family: Consolas, monospace; letter-spacing: .5px; }
        .seg-box { text-align: center; }
        .seg-box label { display: block; font-size: .62rem; font-weight: 700; color: var(--color-guinda); margin: 0; }
        .seg-box input { font-family: Consolas, monospace; text-align: center; padding: .25rem .2rem; font-size: .8rem; }
        .dataTables_wrapper .row { margin-left: 0; margin-right: 0; }
        .dataTables_wrapper .row > [class*="col-"] { padding-left: 0; padding-right: 0; }
        .table-responsive { overflow-x: auto; overflow-y: hidden; }
        .dataTables_paginate .page-item.previous .page-link,
        .dataTables_paginate .page-item.next .page-link { width: auto; min-width: 5.5rem; border-radius: 1rem; padding: 0 .75rem; }
        #tblSinCatalogo td { font-size: .8rem; vertical-align: middle; }
        .dataTables_length select { min-width: 75px; height: auto; padding: .3rem 1.8rem .3rem .6rem; font-size: .82rem; color: #333; display: inline-block; }
    </style>

    <script type="text/javascript">
        var data = sessionStorage.getItem('name');
        if (data == null) { window.location.href = "Login.aspx"; }
    </script>
</head>
<body class="g-sidenav-show">
    <form id="form1" runat="server" onsubmit="return false;">
        <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000000" runat="server"></asp:ScriptManager>
        
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 bg-gradient-primary">
          <div class="container-fluid py-1 px-3">
            <nav aria-label="breadcrumb">
              <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0">
                <li class="breadcrumb-item text-sm"><a class="opacity-8 text-white" href="Menu.aspx">Men&uacute;</a></li>
                <li class="breadcrumb-item text-sm text-white active">Presupuesto Inicial</li>
              </ol>
              <h6 class="font-weight-bolder text-white mb-0">Centro de Mando: Presupuesto</h6>
            </nav>
            <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
              <h3 style="color:white; margin:0 auto;">Secretaría de Infraestructura</h3>
              <div class="ms-md-auto pe-md-3 d-flex align-items-center">
                <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                    <i class="fa-solid fa-arrow-left me-2"></i> Regresar
                </a>
              </div>
            </div>
          </div>
        </nav>

        <main class="main-content mt-4">
            <div class="container-fluid py-4">

                <!-- Pestañas -->
                <ul class="nav nav-tabs px-3" id="myTab" role="tablist">
                  <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="consulta-tab" data-bs-toggle="tab" data-bs-target="#matriz" type="button" role="tab" aria-controls="matriz" aria-selected="true">
                      <i class="fa-solid fa-table-list me-2"></i> Consulta General
                    </button>
                  </li>
                  <li class="nav-item" role="presentation">
                    <button class="nav-link" id="importar-tab" data-bs-toggle="tab" data-bs-target="#importar" type="button" role="tab" aria-controls="importar" aria-selected="false">
                      <i class="fa-solid fa-plus-circle me-2"></i> Alta de Claves
                    </button>
                  </li>
                </ul>

                <div class="tab-content tab-content-card" id="presupuestoTabsContent">

                  <!-- ============================================== -->
                  <!-- PESTAÑA 1: CONSULTA GENERAL                    -->
                  <!-- ============================================== -->
                  <div class="tab-pane fade show active p-0" id="matriz" role="tabpanel" aria-labelledby="consulta-tab">
                      <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                          <h4 class="mb-0" style="color: var(--color-verde-oscuro);">Matriz del presupuesto</h4>
                          <div class="d-flex align-items-center gap-2">
                              <button type="button" id="btnSinCatalogo" class="btn btn-sm btn-outline-warning mb-0" style="display:none;">
                                  <i class="fa-solid fa-triangle-exclamation me-1"></i><span id="lblSinCatalogo">Valores sin catálogo</span>
                              </button>
                              <label class="form-label mb-0" for="cboEjercicio">Ejercicio</label>
                              <select id="cboEjercicio" class="form-control form-control-sm" style="width: 110px;"></select>
                          </div>
                      </div>

                      <!-- Filtros -->
                      <div class="row g-2 mb-3 filtros">
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fEstado">Estado</label>
                              <select id="fEstado" class="form-control form-control-sm">
                                  <option value="">Todas</option>
                                  <option value="calendarizadas">Con calendario</option>
                                  <option value="porliberar">Sin calendario (por liberar)</option>
                              </select>
                          </div>
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fUR">Unidad responsable</label>
                              <select id="fUR" class="form-control form-control-sm filtro-seg" data-abrev="UR"><option value="">Todas</option></select>
                          </div>
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fPP">Programa presupuestario</label>
                              <select id="fPP" class="form-control form-control-sm filtro-seg" data-abrev="PP"><option value="">Todos</option></select>
                          </div>
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fFF">Fuente de financiamiento</label>
                              <select id="fFF" class="form-control form-control-sm filtro-seg" data-abrev="FF"><option value="">Todas</option></select>
                          </div>
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fRegion">Región</label>
                              <select id="fRegion" class="form-control form-control-sm"><option value="">Todas</option></select>
                          </div>
                          <div class="col-lg-2 col-md-4">
                              <label class="form-label" for="fMPIO">Municipio</label>
                              <select id="fMPIO" class="form-control form-control-sm filtro-seg" data-abrev="MPIO"><option value="">Todos</option></select>
                          </div>
                      </div>

                      <!-- Totales de lo filtrado -->
                      <div class="row text-center totales mb-3">
                          <div class="col"><div class="etq">Claves</div><div class="dato" id="tClaves">0</div></div>
                          <div class="col"><div class="etq">Aprobado</div><div class="dato" id="tAprobado">$0.00</div></div>
                          <div class="col"><div class="etq">Modificado</div><div class="dato" id="tModificado">$0.00</div></div>
                          <div class="col"><div class="etq">Liberado (calendario)</div><div class="dato" id="tLiberado">$0.00</div></div>
                          <div class="col"><div class="etq">Por liberar</div><div class="dato" id="tPorLiberar">$0.00</div></div>
                          <div class="col-auto d-flex align-items-end"><button type="button" id="btnLimpiarFiltros" class="btn btn-sm btn-outline-secondary mb-0"><i class="fa-solid fa-eraser me-1"></i>Limpiar filtros</button></div>
                      </div>

                      <div class="table-responsive">
                          <table id="tablaPresupuesto" class="table align-items-center mb-0 w-100 table-hover">
                              <thead>
                                  <tr>
                                      <th>Clave</th>
                                      <th>UR</th>
                                      <th>Programa</th>
                                      <th>Partida</th>
                                      <th>Municipio</th>
                                      <th class="text-end">Aprobado</th>
                                      <th class="text-end">Modificado</th>
                                      <th class="text-end">Liberado</th>
                                      <th class="text-end">Por liberar</th>
                                  </tr>
                              </thead>
                              <tbody></tbody>
                          </table>
                      </div>
                      <p class="text-xs text-muted mt-2 mb-0">Clic en un renglón para ver el detalle de la clave. Los valores en <span class="sin-cat">ámbar</span> no están en su catálogo.</p>
                  </div>

                  <!-- ============================================== -->
                  <!-- PESTAÑA 2: ALTA DE CLAVES                      -->
                  <!-- ============================================== -->
                  <div class="tab-pane fade p-4" id="importar" role="tabpanel" aria-labelledby="importar-tab">
                      <div class="d-flex justify-content-between align-items-start flex-wrap gap-3 mb-4">
                          <div>
                              <h5 class="mb-1" style="color: var(--color-vino);">Alta manual de una clave</h5>
                              <p class="text-sm text-muted mb-0">La clave se agrega al ejercicio <b id="lblEjercicioAlta"></b> con todo su aprobado <b>por liberar</b>; se calendariza con una adecuación de liberación.</p>
                          </div>
                          <a href="PresupuestoInicial.aspx" class="btn btn-outline-primary btn-sm mb-0"><i class="fa-solid fa-file-excel me-2"></i>Carga del presupuesto inicial (Excel)</a>
                      </div>

                      <div class="row">
                          <div class="col-lg-7 mb-3">
                              <label class="form-label" for="txtClaveManual">Clave presupuestal <span class="text-muted" id="lblLongitud"></span></label>
                              <div class="input-group">
                                  <input type="text" id="txtClaveManual" class="form-control clave-input" placeholder="Escriba o pegue la clave completa" autocomplete="off" />
                                  <button class="btn btn-primary mb-0" type="button" id="btnDecodificar"><i class="fa fa-search me-1"></i> Decodificar</button>
                              </div>
                              <div class="text-xs mt-1" id="lblConteo"></div>
                          </div>
                          <div class="col-lg-2 col-md-6 mb-3">
                              <label class="form-label" for="txtAprobadoManual">Monto aprobado</label>
                              <input type="number" id="txtAprobadoManual" class="form-control" placeholder="0.00" step="0.01" min="0" />
                          </div>
                          <div class="col-lg-3 col-md-6 mb-3">
                              <label class="form-label" for="txtOficioManual">Oficio que lo autoriza</label>
                              <input type="text" id="txtOficioManual" class="form-control" maxlength="100" placeholder="Opcional" />
                          </div>
                      </div>

                      <h6 class="mt-1 text-muted text-xs text-uppercase">Captura por segmento</h6>
                      <div class="d-flex flex-wrap gap-1 mb-4" id="contenedorSegmentos"></div>

                      <div id="resultadoDecodificacion" class="mb-4" style="display:none;">
                          <h6 class="text-uppercase text-xs font-weight-bolder mb-2" style="color: var(--color-vino);">Desglose de la clave</h6>
                          <div class="card shadow-none border" style="border-radius: 0.5rem;">
                              <table class="table table-sm table-hover align-items-center mb-0" id="tablaDecodificacion">
                                  <thead>
                                      <tr>
                                          <th class="ps-3">Segmento</th>
                                          <th class="text-center">Valor</th>
                                          <th>Descripción</th>
                                      </tr>
                                  </thead>
                                  <tbody class="text-xs text-dark"></tbody>
                              </table>
                          </div>
                      </div>

                      <div class="text-end">
                          <button type="button" class="btn btn-success mb-0" id="btnGuardarClave"><i class="fa fa-save me-1"></i> Guardar clave</button>
                      </div>
                  </div>
                </div>
            </div>
        </main>

        <!-- Valores sin catálogo -->
        <div class="modal fade" id="modalSinCatalogo" tabindex="-1" aria-hidden="true">
          <div class="modal-dialog modal-xl modal-dialog-scrollable">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" style="color: var(--color-guinda);"><i class="fa-solid fa-triangle-exclamation me-2"></i>Valores que no están en su catálogo · <span id="lblAnioSinCat"></span></h5>
                <button type="button" class="btn-close text-dark" data-bs-dismiss="modal" aria-label="Cerrar"><i class="fa-solid fa-xmark"></i></button>
              </div>
              <div class="modal-body">
                <p class="text-sm mb-2">Estas claves se cargaron con su valor, pero sin la descripción del catálogo. Dé de alta los valores en el catálogo indicado
                   (en los dependientes, bajo el valor padre que se muestra) y después pulse <b>Reprocesar catálogos</b>: el sistema vuelve a buscarlos en todas las claves del ejercicio. No cambia importes.</p>
                <div class="table-responsive">
                  <table class="table table-sm" id="tblSinCatalogo">
                    <thead><tr><th>Segmento</th><th>Valor</th><th>Debe existir bajo</th><th>Catálogo</th><th class="text-end">Claves</th><th>Ejemplo</th></tr></thead>
                    <tbody></tbody>
                  </table>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-outline-secondary btn-sm mb-0" data-bs-dismiss="modal">Cerrar</button>
                <button type="button" class="btn btn-primary btn-sm mb-0" id="btnReprocesar"><i class="fa-solid fa-rotate me-1"></i>Reprocesar catálogos</button>
              </div>
            </div>
          </div>
        </div>
    </form>

    <!-- Scripts Bootstrap / DataTables -->
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js"></script>
    <script src="assets/js/PresupuestoConsulta.js?v=1"></script>
</body>
</html>
