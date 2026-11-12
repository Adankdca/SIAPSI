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
    </style>

    <script type="text/javascript">
        var data = sessionStorage.getItem('name');
        if (data == null) { window.location.href = "Login.aspx"; }
    </script>
</head>
<body class="g-sidenav-show">
    <form id="form1" runat="server" enctype="multipart/form-data">
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
                
                <!-- Pestañas de Navegación -->
                <ul class="nav nav-tabs px-3" id="myTab" role="tablist">
                  <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="consulta-tab" data-bs-toggle="tab" data-bs-target="#matriz" type="button" role="tab" aria-controls="matriz" aria-selected="true">
                      <i class="fa-solid fa-table-list me-2"></i> Consulta General
                    </button>
                  </li>
                  <li class="nav-item" role="presentation">
                    <button class="nav-link" id="importar-tab" data-bs-toggle="tab" data-bs-target="#importar" type="button" role="tab" aria-controls="importar" aria-selected="false">
                      <i class="fa-solid fa-plus-circle me-2"></i> Presupuesto Inicial
                    </button>
                  </li>
                </ul>

                <!-- Contenedor Principal (Blanco) -->
                <div class="tab-content tab-content-card" id="presupuestoTabsContent">
                  
                  <!-- ============================================== -->
                  <!-- PESTAÑA 1: CONSULTA MATRIZ (DataTables)        -->
                  <!-- ============================================== -->
                  <div class="tab-pane fade show active p-0" id="matriz" role="tabpanel" aria-labelledby="matriz-tab">
                      <div class="d-flex justify-content-between align-items-center mb-4">
                          <h4 class="mb-0" style="color: var(--color-verde-oscuro);">Matriz Principal</h4>
                      </div>
                      <div class="row mb-3">
                          <!-- Filtro Estado -->
                          <div class="col-md-4">
                              <div class="dropdown w-100">
                                  <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                      <span id="text-filtroEstado">Todos los Estados (Calendarizada / Por Liberar)</span>
                                      <i class="fas fa-chevron-down text-xs"></i>
                                  </button>
                                  <ul class="dropdown-menu w-100 custom-select-menu" id="menu-filtroEstado">
                                      <li><a class="dropdown-item custom-select-item active" data-value="all">Todos los Estados (Calendarizada / Por Liberar)</a></li>
                                      <li><a class="dropdown-item custom-select-item" data-value="calendarizadas">Solo Calendarizadas</a></li>
                                      <li><a class="dropdown-item custom-select-item" data-value="porliberar">Solo Por Liberar</a></li>
                                  </ul>
                              </div>
                              <input type="hidden" id="filtroEstado" value="all" />
                          </div>
                          <!-- Filtro Municipio -->
                          <div class="col-md-4">
                              <div class="dropdown w-100">
                                  <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                      <span id="text-filtroMunicipio">Todos los Municipios</span>
                                      <i class="fas fa-chevron-down text-xs"></i>
                                  </button>
                                  <ul class="dropdown-menu w-100 custom-select-menu" id="menu-filtroMunicipio">
                                      <li><a class="dropdown-item custom-select-item active" data-value="">Todos los Municipios</a></li>
                                  </ul>
                              </div>
                              <input type="hidden" id="filtroMunicipio" value="" />
                          </div>
                          <!-- Filtro Región -->
                          <div class="col-md-4">
                              <div class="dropdown w-100">
                                  <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                      <span id="text-filtroRegion">Todas las Regiones</span>
                                      <i class="fas fa-chevron-down text-xs"></i>
                                  </button>
                                  <ul class="dropdown-menu w-100 custom-select-menu" id="menu-filtroRegion">
                                      <li><a class="dropdown-item custom-select-item active" data-value="">Todas las Regiones</a></li>
                                  </ul>
                              </div>
                              <input type="hidden" id="filtroRegion" value="" />
                          </div>
                      </div>
                      <style>
                          .dataTables_filter input {
                              width: 420px !important;
                              max-width: 100%;
                          }
                          input[type="file"].form-control::file-selector-button {
                              background-color: var(--color-guinda) !important;
                              color: white !important;
                              font-weight: 600;
                              transition: all 0.2s ease-in-out;
                              box-shadow: 0 4px 6px rgba(105, 28, 50, 0.2) !important;
                          }
                          input[type="file"].form-control:hover::file-selector-button {
                              background-color: var(--color-guinda) !important;
                              color: white !important;
                              opacity: 0.9;
                          }
                          .custom-select-btn {
                              text-transform: none;
                              font-weight: 400;
                              box-shadow: none;
                          }
                          .custom-select-btn:hover, .custom-select-btn:focus, .dropdown.show .custom-select-btn {
                              border-color: var(--color-vino);
                              box-shadow: 0 0 0 0.2rem rgba(136, 14, 79, 0.25);
                          }
                          .custom-select-menu {
                              border: 1px solid #d2d6da;
                              border-radius: 0.5rem;
                              box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                              padding: 0.5rem;
                              max-height: 250px;
                              overflow-y: auto;
                          }
                          .custom-select-item {
                              color: #495057;
                              padding: 0.5rem 1rem;
                              border-radius: 0.375rem;
                              transition: background-color 0.2s, color 0.2s;
                              cursor: pointer;
                          }
                          .custom-select-item:hover {
                              background-color: transparent !important;
                              color: #af2b61 !important; /* Un vino ligeramente más claro */
                          }
                          .custom-select-item.active {
                              background-color: transparent !important;
                              color: var(--color-vino) !important;
                              font-weight: 600;
                          }
                      </style>
                      <div class="table-responsive">
                          <table id="tablaPresupuesto" class="table align-items-center mb-0 w-100 table-hover">
                              <thead>
                                  <tr>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Clave</th>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">UR</th>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Programa</th>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Objeto Gasto</th>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-end">Aprobado</th>
                                      <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-end">Modificado</th>
                                  </tr>
                              </thead>
                              <tbody></tbody>
                          </table>
                      </div>
                  </div>

                  <!-- ============================================== -->
                  <!-- PESTAÑA 2: IMPORTAR EXCEL (Legacy ASP.NET)     -->
                  <!-- ============================================== -->
                  <div class="tab-pane fade p-4" id="importar" role="tabpanel" aria-labelledby="importar-tab">
                      <ul class="nav nav-pills p-1 bg-gray-100 mb-4" id="pills-tab" role="tablist" style="border-radius: 0.75rem; display: inline-flex;">
                        <li class="nav-item" role="presentation">
                          <button class="nav-link mb-0 px-5 py-2 active font-weight-bold" id="pills-manual-tab" data-bs-toggle="pill" data-bs-target="#pills-manual" type="button" role="tab" style="border-radius: 0.5rem; transition: all 0.3s ease;">
                              <i class="fa-solid fa-keyboard me-2"></i>Captura Manual
                          </button>
                        </li>
                        <li class="nav-item" role="presentation">
                          <button class="nav-link mb-0 px-5 py-2 font-weight-bold" id="pills-excel-tab" data-bs-toggle="pill" data-bs-target="#pills-excel" type="button" role="tab" style="border-radius: 0.5rem; transition: all 0.3s ease;">
                              <i class="fa-solid fa-file-excel me-2"></i>Carga Masiva (Excel)
                          </button>
                        </li>
                      </ul>
                      
                      <style>
                          /* Estilos para asegurar que la pastilla activa se pinte con el color principal del tema */
                          .nav-pills .nav-link.active {
                              background-color: #9F2241 !important; /* Color Vino */
                              color: #fff !important;
                              box-shadow: 0 4px 6px rgba(0,0,0,0.1);
                          }
                          .nav-pills .nav-link {
                              color: #6c757d;
                          }
                      </style>

                      <div class="tab-content" id="pills-tabContent">
                          <!-- SECCION MANUAL -->
                          <div class="tab-pane fade show active" id="pills-manual" role="tabpanel">
                              <h5 class="mb-3" style="color: var(--color-vino);">Ingreso Manual de Clave</h5>
                              <div class="row">
                                  <div class="col-md-8 mb-3">
                                      <label class="form-control-label">Clave Presupuestal (Cadena Completa)</label>
                                      <div class="input-group">
                                          <input type="text" id="txtClaveManual" class="form-control" placeholder="Ingrese los 56 dígitos de la clave" maxlength="56" onkeyup="sincronizarSegmentos()" />
                                          <button class="btn btn-primary mb-0 bg-gradient-primary" type="button" onclick="decodificarClaveManual()"><i class="fa fa-search"></i> Decodificar</button>
                                      </div>
                                  </div>
                                  <div class="col-md-4 mb-3">
                                      <label class="form-control-label">Monto Aprobado</label>
                                      <input type="number" id="txtAprobadoManual" class="form-control" placeholder="0.00" step="0.01" />
                                  </div>
                              </div>
                              
                              <h6 class="mt-3 text-muted text-xs text-uppercase">Ingreso Segmentado</h6>
                              <div class="row g-2 mb-4" id="contenedorSegmentos">
                                  <!-- Los segmentos se generarán por JS -->
                              </div>
                              <div class="row justify-content-center">
                                  <div class="col-lg-9 col-md-11 mx-auto">
                                      <div id="resultadoDecodificacion" class="mb-4" style="display:none;">
                                          <h6 class="text-uppercase text-xs font-weight-bolder mb-2" style="color: var(--color-vino);">Desglose de la Clave</h6>
                                          <div class="card shadow-none border" style="border-radius: 0.5rem;">
                                              <table class="table table-hover align-items-center mb-0" id="tablaDecodificacion">
                                                  <thead>
                                                      <tr>
                                                          <th class="text-uppercase text-xs font-weight-bolder ps-3" style="color: var(--color-vino);">Segmento</th>
                                                          <th class="text-uppercase text-xs font-weight-bolder text-center" style="color: var(--color-vino);">Valor</th>
                                                          <th class="text-uppercase text-xs font-weight-bolder ps-2" style="color: var(--color-vino);">Descripción</th>
                                                      </tr>
                                                  </thead>
                                                  <tbody class="text-xs text-dark"></tbody>
                                              </table>
                                          </div>
                                      </div>
                                  </div>
                              </div>
                              
                              <div class="text-end">
                                  <button type="button" class="btn btn-success" onclick="guardarClaveManual()"><i class="fa fa-save"></i> Guardar Presupuesto</button>
                              </div>
                          </div>
                          
                          <!-- SECCION EXCEL AJAX -->
                          <div class="tab-pane fade" id="pills-excel" role="tabpanel">
                                      <h5 class="mb-4 text-primary">Cargar Archivo de Presupuesto Inicial</h5>
                                      <p class="text-sm text-muted">Sube tu archivo de Excel (.xlsx) con la pestaña MAESTRO. Podrás revisar y editar los montos principales antes de guardarlos en la base de datos.</p>
                                      
                                      <div class="row align-items-end mb-4" id="seccionUploadExcel">
                                          <div class="col-md-2 mb-3">
                                              <label class="form-control-label">A&ntilde;o</label>
                                              <select id="cboanioExcel" class="form-select"></select>    
                                          </div>
                                          <div class="col-md-3 mb-3">
                                              <label class="form-control-label">N&uacute;mero de Oficio</label>
                                              <input type="text" id="txtoficioExcel" placeholder="Ej. OF-2026-001" class="form-control" />
                                          </div>
                                          <div class="col-md-4 mb-3">
                                              <label class="form-control-label">Seleccione Archivo (Excel)</label>
                                              <input type="file" id="fileUploadExcelAjax" class="form-control" accept=".xls,.xlsx" />
                                          </div>
                                          <div class="col-md-3 mb-3">
                                              <button type="button" class="btn btn-primary w-100 mb-0" onclick="uploadPreviewExcel()">
                                                  <i class="fa-solid fa-eye me-2"></i> Procesar y Revisar
                                              </button>
                                          </div>
                                      </div>

                                      <!-- Vista Previa de Excel -->
                                      <div id="seccionPreviewExcel" style="display: none;">
                                          <hr class="horizontal dark my-4">
                                          <div class="d-flex justify-content-between align-items-center mb-3">
                                              <h6 class="text-uppercase text-muted text-xs font-weight-bolder mb-0">Vista Previa de Datos a Guardar (<span id="countFilasPreview">0</span> filas)</h6>
                                              <div>
                                                  <button type="button" class="btn btn-outline-secondary btn-sm mb-0 me-2" onclick="cancelarExcel()">Cancelar</button>
                                                  <button type="button" class="btn btn-success btn-sm mb-0" onclick="guardarMasivoExcel()">
                                                      <i class="fa fa-save me-1"></i> Confirmar y Guardar Todo
                                                  </button>
                                              </div>
                                          </div>
                                          
                                          <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
                                              <table id="tblPreviewExcel" class="table align-items-center table-bordered mb-0 w-100" style="font-size: 0.8rem;">
                                                  <thead class="bg-light sticky-top">
                                                      <tr>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">#</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7" style="min-width: 400px;">Clave Presupuestal</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Aprobado</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Enero</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Febrero</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Marzo</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Abril</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Mayo</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Junio</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Julio</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Agosto</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Septiembre</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Octubre</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Noviembre</th>
                                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Diciembre</th>
                                                      </tr>
                                                  </thead>
                                                  <tbody id="tbodyPreviewExcel"></tbody>
                                              </table>
                                          </div>
                                      </div>
                          </div>
                      </div>
                  </div>

                </div>
            </div>
        </main>

        <!-- Overlay de Carga -->
        <div id="overlayCarga" style="display:none; position:fixed; top:0; left:0; width:100%; height:100%; background:rgba(0,0,0,0.6); z-index:9999; text-align:center;">
            <div style="position:absolute; top:50%; left:50%; transform:translate(-50%,-50%); color:white; font-size:20px;">
                <i class="fa-solid fa-circle-notch fa-spin fa-2x mb-3"></i><br/>
                Procesando archivo... espere por favor
            </div>
        </div>
        <script>
            function mostrarCarga(btn) {
                document.getElementById("overlayCarga").style.display = "block";
                btn.value = "Procesando...";
            }
        </script>
    </form>

    <!-- Scripts Bootstrap / DataTables -->
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js"></script>

    <script>
        const configSegmentos = [
            { nombre: "UR", abrev: "UR", length: 2 },
            { nombre: "Unidad Responsable Op.", abrev: "URO", length: 3 },
            { nombre: "Finalidad", abrev: "FI", length: 1 },
            { nombre: "Función", abrev: "F", length: 1 },
            { nombre: "Subfunción", abrev: "SF", length: 1 },
            { nombre: "Subsubfunción", abrev: "SSF", length: 1 },
            { nombre: "Programa Sectorial", abrev: "PS", length: 1 },
            { nombre: "Programa Presupuestario", abrev: "PP", length: 3 },
            { nombre: "Origen de Asignación", abrev: "OA", length: 2 },
            { nombre: "Actividad Institucional", abrev: "AI", length: 3 },
            { nombre: "Proyecto Estratégico", abrev: "PT", length: 4 },
            { nombre: "Municipio", abrev: "MPIO", length: 3 },
            { nombre: "Grupo Asignación", abrev: "GA", length: 1 },
            { nombre: "Clasif. Objeto Gasto", abrev: "COGC", length: 2 },
            { nombre: "Gasto", abrev: "GASTO", length: 5 },
            { nombre: "Gasto (Objeto)", abrev: "GTO", length: 2 },
            { nombre: "Año de Gasto", abrev: "AG", length: 2 },
            { nombre: "Tipo de Gasto", abrev: "TG", length: 1 },
            { nombre: "Tipo de Recurso", abrev: "TR", length: 1 },
            { nombre: "Fuente Financiamiento", abrev: "FF", length: 1 },
            { nombre: "Subfuente Financiamiento", abrev: "SFTE", length: 1 },
            { nombre: "Ramo", abrev: "RM", length: 2 },
            { nombre: "Programa Fondo", abrev: "PF", length: 5 },
            { nombre: "Rubro de Ingresos", abrev: "RI", length: 3 },
            { nombre: "Ciclo Presupuestal", abrev: "CP", length: 4 },
            { nombre: "Dígito Ministración", abrev: "DM", length: 1 }
        ];

        $(document).ready(function () {
            // Llenar cboanioExcel
            var anioActual = new Date().getFullYear();
            var cbo = $('#cboanioExcel');
            cbo.append(new Option(anioActual - 1, anioActual - 1));
            cbo.append(new Option(anioActual, anioActual));
            cbo.append(new Option(anioActual + 1, anioActual + 1));
            cbo.val(anioActual);

            // Generar los inputs segmentados
            let htmlSegmentos = '';
            configSegmentos.forEach(seg => {
                htmlSegmentos += `
                    <div class="col-auto">
                        <label class="form-control-label d-block text-center text-xxs mb-0" title="${seg.nombre}">${seg.abrev}</label>
                        <input type="text" class="form-control form-control-sm text-center px-1 mx-1 input-segmento" 
                               data-length="${seg.length}" maxlength="${seg.length}" style="width: ${seg.length * 10 + 20}px;" 
                               onkeyup="sincronizarDesdeSegmentos()" />
                    </div>
                `;
            });
            $('#contenedorSegmentos').html(htmlSegmentos);
            
            // Logica DataTable existente
            $('#tablaPresupuesto').DataTable({
                serverSide: true,
                processing: true,
                ajax: { 
                    url: '/api/presupuesto/matriz/paginado',
                    type: 'GET',
                    data: function(d) {
                        var m = $('#filtroMunicipio').val();
                        var r = $('#filtroRegion').val();
                        var e = $('#filtroEstado').val();
                        return $.extend({}, d, {
                            search: d.search.value,
                            mpio: (m && m !== "Todos los Municipios") ? m : "",
                            region: (r && r !== "Todas las Regiones") ? r : "",
                            estado: e
                        });
                    }
                },
                dom: '<"d-flex justify-content-end mb-3"f>rt<"d-flex justify-content-between align-items-center mt-3"lip>',
                columns: [
                    { data: 'ClavePresupuestal' },
                    { data: 'UrDescripcion', defaultContent: 'N/A' },
                    { data: 'ProgramaDescripcion', defaultContent: 'N/A' },
                    { data: 'ObjetoGastoDescripcion', defaultContent: 'N/A', className: 'text-wrap', width: '40%' },
                    { data: 'Aprobado', className: 'text-end', render: $.fn.dataTable.render.number(',', '.', 2, '$') },
                    { data: 'Modificado', className: 'text-end', render: $.fn.dataTable.render.number(',', '.', 2, '$') }
                ],
                language: { 
                    url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/es-MX.json',
                    search: "",
                    searchPlaceholder: "Buscar...",
                    paginate: {
                        previous: '<i class="fa-solid fa-chevron-left"></i>',
                        next: '<i class="fa-solid fa-chevron-right"></i>'
                    }
                },
                pageLength: 10,
                ordering: true,
                responsive: true
            });

            $('#tablaPresupuesto tbody').on('click', 'tr', function () {
                var data = $('#tablaPresupuesto').DataTable().row(this).data();
                if (data) {
                    window.location.href = 'DetalleClave.aspx?clave=' + encodeURIComponent(data.ClavePresupuestal);
                }
            });
            $('#tablaPresupuesto tbody').css('cursor', 'pointer');

            // Evento manual para abrir/cerrar el dropdown (por si falla Bootstrap)
            $('.custom-select-btn').on('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                let $menu = $(this).siblings('.custom-select-menu');
                // Cerrar otros menús abiertos
                $('.custom-select-menu').not($menu).removeClass('show');
                // Alternar el actual
                $menu.toggleClass('show');
            });

            // Cerrar al dar clic afuera
            $(document).on('click', function(e) {
                if (!$(e.target).closest('.dropdown').length) {
                    $('.custom-select-menu').removeClass('show');
                }
            });

            // Evento para Custom Selects
            $(document).on('click', '.custom-select-item', function(e) {
                e.preventDefault();
                let $menu = $(this).closest('.custom-select-menu');
                let $container = $menu.closest('.col-md-4');
                let $btnText = $container.find('button span');
                let $hiddenInput = $container.find('input[type="hidden"]');

                // Quitar active a todos los items de este menu
                $menu.find('.custom-select-item').removeClass('active');
                // Poner active al seleccionado
                $(this).addClass('active');

                // Actualizar texto del botón y valor del input oculto
                $btnText.text($(this).text());
                $hiddenInput.val($(this).data('value'));

                // Cerrar el menú
                $menu.removeClass('show');

                // Disparar evento change en el input oculto
                $hiddenInput.trigger('change');
            });

            // Custom DataTables Filter
            $.fn.dataTable.ext.search.push(function(settings, data, dataIndex, rowData) {
                let filterEstado = $('#filtroEstado').val();
                let filterMpio = $('#filtroMunicipio').val();
                let filterRegion = $('#filtroRegion').val();
                
                let estadoMatch = true;
                if (filterEstado === 'calendarizadas') {
                    estadoMatch = rowData.EsCalendarizada === true || rowData.EsCalendarizada === 1;
                } else if (filterEstado === 'porliberar') {
                    estadoMatch = rowData.EsCalendarizada === false || rowData.EsCalendarizada === 0;
                }

                let mpioMatch = true;
                if (filterMpio) {
                    mpioMatch = rowData.NombreMunicipio === filterMpio;
                }

                let regionMatch = true;
                if (filterRegion) {
                    regionMatch = rowData.NombreRegion === filterRegion;
                }

                return estadoMatch && mpioMatch && regionMatch;
            });

            function cargarMunicipios(region = "") {
                let url = '/api/presupuesto/filtros/municipios';
                if (region && region !== "Todas las Regiones") {
                    url += '?region=' + encodeURIComponent(region);
                }
                $.get(url, function(data) {
                    if(data && data.length > 0) {
                        let options = '<li><a class="dropdown-item custom-select-item active" data-value="">Todos los Municipios</a></li>';
                        data.forEach(m => {
                            options += `<li><a class="dropdown-item custom-select-item" data-value="${m}">${m}</a></li>`;
                        });
                        $('#menu-filtroMunicipio').html(options);
                        
                        // Resetear visualmente el dropdown de Municipio cuando cargan nuevos
                        $('#text-filtroMunicipio').text("Todos los Municipios");
                        $('#filtroMunicipio').val("");
                    }
                });
            }

            // Cargar inicial
            cargarMunicipios();

            $.get('/api/presupuesto/filtros/regiones', function(data) {
                if(data && data.length > 0) {
                    let optReg = '<li><a class="dropdown-item custom-select-item active" data-value="">Todas las Regiones</a></li>';
                    data.forEach(r => {
                        optReg += `<li><a class="dropdown-item custom-select-item" data-value="${r}">${r}</a></li>`;
                    });
                    $('#menu-filtroRegion').html(optReg);
                }
            });

            // Cuando cambia región, recargar municipios y la tabla
            $('#filtroRegion').on('change', function() {
                cargarMunicipios($(this).val());
                $('#tablaPresupuesto').DataTable().draw();
            });

            $('#filtroEstado, #filtroMunicipio').on('change', function() {
                $('#tablaPresupuesto').DataTable().draw();
            });
        });

        /**
         * Sincroniza el valor ingresado en el campo de texto completo de la Clave Presupuestal
         * hacia los inputs individuales de cada segmento.
         * Toma el valor completo, lo convierte a mayúsculas y lo recorta según la longitud 
         * configurada de cada segmento, asignándolo a los inputs correspondientes de izquierda a derecha.
         */
        function sincronizarSegmentos() {
            let clave = $('#txtClaveManual').val().toUpperCase();
            $('#txtClaveManual').val(clave);
            
            let pos = 0;
            $('.input-segmento').each(function(idx) {
                let len = parseInt($(this).data('length'));
                if (pos < clave.length) {
                    $(this).val(clave.substring(pos, pos + len));
                } else {
                    $(this).val('');
                }
                pos += len;
            });
        }

        /**
         * Realiza la operación inversa: sincroniza los valores ingresados en los inputs individuales segmentados
         * para concatenarlos y formar la cadena completa de la Clave Presupuestal en el input principal.
         * Además, fuerza que todo el texto ingresado en los segmentos se convierta a mayúsculas.
         */
        function sincronizarDesdeSegmentos() {
            let clave = '';
            $('.input-segmento').each(function() {
                clave += $(this).val().toUpperCase();
                $(this).val($(this).val().toUpperCase()); // Forzar mayúsculas
            });
            $('#txtClaveManual').val(clave);
        }

        /**
         * Invoca a la API para decodificar la Clave Presupuestal ingresada manualmente.
         * Valida que la longitud de la clave sea de al menos 56 caracteres. Si es válida, realiza una petición GET
         * a '/api/presupuesto/decodificar', y utiliza la respuesta (desglose de catálogos) para poblar y mostrar
         * la tabla de decodificación en la interfaz de usuario, brindando visibilidad de la estructura de la clave.
         */
        function decodificarClaveManual() {
            let clave = $('#txtClaveManual').val();
            // Permitiendo longitud variable por ahora para adaptarse a los 58 caracteres
            if (clave.length < 56) {
                swal.fire('Atención', 'La clave parece estar incompleta.', 'warning');
                return;
            }
            
            $.ajax({
                url: '/api/presupuesto/decodificar?clave=' + encodeURIComponent(clave),
                method: 'GET',
                success: function(res) {
                    $('#resultadoDecodificacion').show();
                    let tbody = '';
                    res.Segmentos.forEach(s => {
                        tbody += `
                            <tr>
                                <td class="ps-3"><div class="d-flex px-2 py-1"><div class="d-flex flex-column justify-content-center"><h6 class="mb-0 text-xs">${s.Abreviatura}</h6><p class="text-xs text-secondary mb-0">${s.Nombre}</p></div></div></td>
                                <td class="align-middle text-center"><span class="text-dark font-weight-bolder" style="font-size: 0.9rem;">${s.Valor}</span></td>
                                <td class="align-middle"><span class="text-secondary text-xs font-weight-bold">${s.Descripcion}</span></td>
                            </tr>`;
                    });
                    $('#tablaDecodificacion tbody').html(tbody);
                },
                error: function() {
                    swal.fire('Error', 'No se pudo decodificar la clave. Verifique la conexión.', 'error');
                }
            });
        }

        /**
         * Captura la Clave Presupuestal manual y su Monto Aprobado, validando que los datos sean correctos.
         * Valida la longitud de la clave y que el monto sea mayor a 0. Luego, realiza una petición POST
         * a '/api/presupuesto/nueva' para guardar el nuevo registro presupuestal. Si tiene éxito, muestra 
         * una alerta, limpia el formulario y recarga el DataTable de presupuesto automáticamente.
         */
        function guardarClaveManual() {
            let clave = $('#txtClaveManual').val();
            let aprobado = parseFloat($('#txtAprobadoManual').val()) || 0;
            
            if (clave.length < 56) {
                swal.fire('Error', 'La clave está incompleta.', 'error');
                return;
            }
            if (aprobado <= 0) {
                swal.fire('Error', 'El monto aprobado debe ser mayor a 0.', 'error');
                return;
            }
            
            let payload = {
                ClavePresupuestal: clave,
                Aprobado: aprobado
            };
            
            $.ajax({
                url: '/api/presupuesto/nueva',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(payload),
                success: function() {
                    swal.fire({
                        title: '¡Guardado!',
                        text: 'La clave se guardó en "Por Liberar". Estará disponible para calendarizar cuando se aplique una adecuación de liberación.',
                        type: 'success',
                        confirmButtonText: 'Entendido'
                    }).then(() => {
                        $('#txtClaveManual').val('');
                        $('#txtAprobadoManual').val('');
                        $('.input-segmento').val('');
                        $('#resultadoDecodificacion').hide();
                        $('#tablaPresupuesto').DataTable().ajax.reload();
                    });
                },
                error: function(err) {
                    swal.fire('Error', 'No se pudo guardar la clave. Es posible que ya exista.', 'error');
                }
            });
        }
        var datosExcelPreview = []; // Variable global para almacenar el JSON extraído

        function uploadPreviewExcel() {
            var fileUpload = document.getElementById("fileUploadExcelAjax");
            if (fileUpload.files.length === 0) {
                swal.fire('Atención', 'Seleccione un archivo Excel.', 'warning');
                return;
            }
            var anio = $('#cboanioExcel').val();
            var oficio = $('#txtoficioExcel').val();
            if (!oficio) {
                swal.fire('Atención', 'Ingrese el número de oficio.', 'warning');
                return;
            }

            var formData = new FormData();
            formData.append("file", fileUpload.files[0]);
            formData.append("anio", anio);
            formData.append("oficio", oficio);

            swal.fire({ title: 'Procesando...', text: 'Leyendo archivo Excel, esto puede tomar unos segundos.', allowOutsideClick: false, onBeforeOpen: () => { swal.showLoading(); } });

            $.ajax({
                url: '/api/presupuesto/previewExcel',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {
                    swal.close();
                    if (res && res.length > 0) {
                        datosExcelPreview = res;
                        $('#countFilasPreview').text(res.length);
                        renderTablaPreview(res);
                        $('#seccionUploadExcel').hide();
                        $('#seccionPreviewExcel').fadeIn();
                    } else {
                        swal.fire('Aviso', 'El archivo no contiene datos válidos.', 'info');
                    }
                },
                error: function (err) {
                    var msg = err.responseJSON && err.responseJSON.Message ? err.responseJSON.Message : 'Error al procesar el archivo.';
                    swal.fire('Error', msg, 'error');
                }
            });
        }

        function renderTablaPreview(data) {
            const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
            var html = '';
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += '<tr>' +
                            '<td class="text-sm">' + (i + 1) + '</td>' +
                            '<td class="text-sm font-weight-bold">' + item.ClavePresupuestal + '</td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Aprobado" value="' + item.Aprobado + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Enero" value="' + item.Enero + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Febrero" value="' + item.Febrero + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Marzo" value="' + item.Marzo + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Abril" value="' + item.Abril + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Mayo" value="' + item.Mayo + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Junio" value="' + item.Junio + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Julio" value="' + item.Julio + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Agosto" value="' + item.Agosto + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Septiembre" value="' + item.Septiembre + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Octubre" value="' + item.Octubre + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Noviembre" value="' + item.Noviembre + '" style="min-width: 100px;"></td>' +
                            '<td class="text-sm"><input type="number" class="form-control form-control-sm text-end input-monto" data-index="' + i + '" data-field="Diciembre" value="' + item.Diciembre + '" style="min-width: 100px;"></td>' +
                        '</tr>';
            }
            $('#tbodyPreviewExcel').html(html);

            // Escuchar cambios en los inputs para actualizar el JSON en memoria
            $('.input-monto').on('change', function() {
                var index = $(this).data('index');
                var field = $(this).data('field');
                var val = parseFloat($(this).val()) || 0;
                datosExcelPreview[index][field] = val;
            });
        }

        function cancelarExcel() {
            $('#seccionPreviewExcel').hide();
            $('#seccionUploadExcel').fadeIn();
            datosExcelPreview = [];
            $('#tbodyPreviewExcel').empty();
            $('#fileUploadExcelAjax').val('');
        }

        function guardarMasivoExcel() {
            if (!datosExcelPreview || datosExcelPreview.length === 0) {
                swal.fire('Atención', 'No hay datos para guardar.', 'warning');
                return;
            }

            swal.fire({
                title: '¿Confirmar Guardado?',
                text: "Se insertarán " + datosExcelPreview.length + " claves al presupuesto.",
                type: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, guardar'
            }).then((result) => {
                if (result.value) {
                    swal.fire({ title: 'Guardando...', text: 'Por favor espere.', allowOutsideClick: false, onBeforeOpen: () => { swal.showLoading(); } });
                    
                    var payload = {
                        anio: $('#cboanioExcel').val(),
                        oficio: $('#txtoficioExcel').val(),
                        claves: datosExcelPreview
                    };

                    $.ajax({
                        url: '/api/presupuesto/saveMasivo',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(payload),
                        success: function () {
                            swal.fire('¡Éxito!', 'Presupuesto guardado correctamente.', 'success').then(() => {
                                cancelarExcel();
                                $('#tablaPresupuesto').DataTable().ajax.reload();
                                $('#pills-manual-tab').click(); // Volver a la pestaña principal
                            });
                        },
                        error: function (err) {
                            var msg = err.responseJSON && err.responseJSON.Message ? err.responseJSON.Message : 'Ocurrió un error al guardar.';
                            swal.fire('Error', msg, 'error');
                        }
                    });
                }
            });
        }
    </script>
</body>
</html>
