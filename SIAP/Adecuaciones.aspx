<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adecuaciones.aspx.cs" Inherits="SIAP.Adecuaciones" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
 <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
  <link rel="apple-touch-icon" sizes="76x76" href="assets/img/apple-icon.png" />
  <link rel="icon" type="image/png" href="assets/img/favicon.png" />
  <title>
    ADECUACIONES | SIAP
  </title>
  <script src="assets/js/jquery-3.5.1.js"></script>
  <script src="assets/js/jquery-migrate-3.0.0.js"></script>
  <!--     Fonts and icons     -->
  <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
  <!-- Nucleo Icons -->
  <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- Font Awesome Icons -->
  <link href="assets/css/style.css?v=2.2" rel="stylesheet" />
  <script src="https://kit.fontawesome.com/42d5adcbca.js" crossorigin="anonymous"></script>
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- CSS Files principal -->
  <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
  <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
  <link href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" rel="stylesheet" />
  <script src="assets/js/SweetAlert2/popper/popper.min.js"></script>
  <%--++++++++++++++++++++++++++elementos de Autocomplete++++++++++++++++++++++++++++++++++++++--%>
  <script src="assets/js/jquery-ui-1.8.custom.min.js"></script>
  <link href="assets/css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
  <%--++++++++++++++++++++++++++elementos de Autocomplete++++++++++++++++++++++++++++++++++++++--%>
  <script src="assets/js/moment.min.js"></script>
  <!-- DataTables CSS -->
  <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap5.min.css" />
<style>
.modal-lg { 
    max-width: 90% !important; 
} 
.modal-sm { 
    max-width: 50% !important; 
}
.colorencabezado {
    color: #182879;
}
</style>
<script>
    var data = sessionStorage.getItem('name');
    if (data == null) {
        window.location.href = "Login.aspx";
    }
</script>
<style type="text/css">

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto;
            overflow-x: hidden;
        }
        * html .ui-autocomplete {
            height: 300px;
        }
     .style1 {
            font-size: 11px;
            text-align: left;
            font-weight: Bold;
            width: 218px;
            padding-left: 3px;
            margin-top:-10px;
        }
</style>
<script type="text/javascript">
    $(document).ready(function () {//¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡¡AGREGAR LOS NUEVOS ELEMENTOS DE LA CLAVE PRESUPUESTAL!!!!!!!!!!!!!!
        // El autocompletado fue eliminado a petición del usuario.
        // Ahora se desglosará la clave dinámicamente en las cajas segmentadas.
        $("#txtclavebusqueda").on("input keyup", function() {
            var cadena = $(this).val().trim();
            // Llenar segmentados si la cadena tiene longitud suficiente
            $("#seg_ur").val(cadena.substring(0, 2));
            $("#seg_fi").val(cadena.substring(2, 3));
            $("#seg_fu").val(cadena.substring(3, 4));
            $("#seg_sf").val(cadena.substring(4, 5));
            $("#seg_ssf").val(cadena.substring(5, 6));
            $("#seg_ps").val(cadena.substring(6, 7));
            $("#seg_pp").val(cadena.substring(7, 10));
            $("#seg_oa").val(cadena.substring(10, 12));
            $("#seg_ai").val(cadena.substring(12, 15));
            $("#seg_pt").val(cadena.substring(15, 19));
            $("#seg_mpio").val(cadena.substring(19, 22));
            $("#seg_ga").val(cadena.substring(22, 23));
            $("#seg_cogc").val(cadena.substring(23, 24));
            $("#seg_gasto").val(cadena.substring(24, 29));
            $("#seg_gto").val(cadena.substring(29, 31));
            $("#seg_ag").val(cadena.substring(31, 33));
            $("#seg_tg").val(cadena.substring(33, 34));
            $("#seg_tr").val(cadena.substring(34, 35));
            $("#seg_ff").val(cadena.substring(35, 36));
            $("#seg_sfte").val(cadena.substring(36, 37));
            $("#seg_rm").val(cadena.substring(37, 39));
            $("#seg_pf").val(cadena.substring(39, 44));
            $("#seg_cp").val(cadena.substring(44, 48));
            $("#seg_dm").val(cadena.substring(48));
        });
        });
</script>
<script type="text/javascript">
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        placement: 'top'
    });
});
</script>
<script type="text/javascript">
    $("#txtclavebusqueda").focus(function ()
    {
        $(this).css("background-color", "#FFFFFF");
        $("#txtclavebusqueda").select();
    });
</script>
    <script src="assets/js/SesionExpirada.js"></script>
</head>
<body class="g-sidenav-show">
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Menu.aspx">Menú</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Adecuaciones</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0">Gestión de Adecuaciones</h6>
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
<input type = "hidden" id="hddaccion" name="hddaccion" value=""/>
<input type = "hidden" id="hddposicion" name="hddposicion" value=""/>
  <div class="main-content position-relative max-height-vh-100 h-100">
    <!-- Navbar -->
    <!-- End Navbar -->
    <div class="container-fluid mt-4">
      <div class="row">
        <div class="col-md-12">
          <div class="card">
            <div class="card-body pt-4">
              <div class="row mb-3 align-items-center">
                <div class="col-md-6">
                  <h4 class="mb-0" style="color: var(--color-verde-oscuro);">Adecuaciones</h4>
                </div>
                <div class="col-md-6 text-end">
                  <button class="btn text-white btn-sm mb-0 me-2" style="background-color: #621132; border-color: #621132;" id="btncargaroficio" data-bs-toggle="modal" data-bs-target="#modalCargarOficio"><i class="fas fa-file-upload me-2"></i>Cargar Oficio (PDF)</button>
                </div>
              </div>
              <div class="row">
                <%--=============================================================================================== --%>
                <%--=========================================FILTRO DE BUSQUEDA==================================== --%>
                <%--=============================================================================================== --%>
                <style>
                    .custom-select-btn { text-transform: none; font-weight: 400; box-shadow: none; }
                    .custom-select-btn:hover, .custom-select-btn:focus, .dropdown.show .custom-select-btn { border-color: var(--color-vino); box-shadow: 0 0 0 0.2rem rgba(136, 14, 79, 0.25); }
                    .custom-select-menu { border: 1px solid #d2d6da; border-radius: 0.5rem; box-shadow: 0 4px 6px rgba(0,0,0,0.1); padding: 0.5rem; max-height: 250px; overflow-y: auto; }
                    .custom-select-item { color: #495057; padding: 0.5rem 1rem; border-radius: 0.375rem; transition: background-color 0.2s, color 0.2s; cursor: pointer; }
                    .custom-select-item:hover { background-color: transparent !important; color: #af2b61 !important; }
                    .custom-select-item.active { background-color: transparent !important; color: var(--color-vino) !important; font-weight: 600; }
                    .dataTables_filter input {
                        width: 420px !important;
                        max-width: 100%;
                    }
                </style>
                <div class="col-md-3">
                  <div class="dropdown w-100 mb-3">
                      <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                          <span id="text-numsolicitud">(Seleccione un Número de Solicitud)</span>
                          <i class="fas fa-chevron-down text-xs"></i>
                      </button>
                      <ul class="dropdown-menu w-100 custom-select-menu" id="menu-numsolicitud">
                          <li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Número de Solicitud)</a></li>
                      </ul>
                  </div>
                  <input type="hidden" id="numsolicitud_filtro" value="0" onchange="filtrar()" />
                </div>
                <div class="col-md-3">
                  <div class="dropdown w-100 mb-3">
                      <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                          <span id="text-tipoadecuacion">(Seleccione un Tipo de Adecuación)</span>
                          <i class="fas fa-chevron-down text-xs"></i>
                      </button>
                      <ul class="dropdown-menu w-100 custom-select-menu" id="menu-tipo">
                          <li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Tipo de Adecuación)</a></li>
                      </ul>
                  </div>
                  <input type="hidden" id="tipoadecuacion" value="0" onchange="filtrar()" />
                </div>
                <div class="col-md-3">
                  <div class="dropdown w-100 mb-3">
                      <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                          <span id="text-mes">(Seleccione un Mes)</span>
                          <i class="fas fa-chevron-down text-xs"></i>
                      </button>
                      <ul class="dropdown-menu w-100 custom-select-menu" id="menu-mes">
                          <li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Mes)</a></li>
                      </ul>
                  </div>
                  <input type="hidden" id="mes" value="0" onchange="filtrar()" />
                </div>
                <div class="col-md-3">
                  <div class="dropdown w-100 mb-3">
                      <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                          <span id="text-anio">(Seleccione un Año)</span>
                          <i class="fas fa-chevron-down text-xs"></i>
                      </button>
                      <ul class="dropdown-menu w-100 custom-select-menu" id="menu-anio">
                          <li><a class="dropdown-item custom-select-item active" data-value="0">(Seleccione un Año)</a></li>
                      </ul>
                  </div>
                  <input type="hidden" id="anio" value="0" onchange="filtrar()" />
                </div>
                <%--=============================================================================================== --%>
                <%--=========================================FILTRO DE BUSQUEDA==================================== --%>
                <%--=============================================================================================== --%>

                <%--=============================================================================================== --%>
                <%--=========================GRID PRINCIPAL DE LAS ADECUACIONES==================================== --%>
                <%--=============================================================================================== --%>
                <div class="table-responsive p-0">
                    <table id="tablaclavepresupuestaladecuacion" class="table align-items-center mb-0 w-100 table-hover">
                      <thead>
                        <tr>
                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Num. Solicitud</th>
                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Tipo de Adecuación</th>
                          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Fecha Solicitud</th>
                          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Monto Total Adecuación</th>
                          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Total Emisor</th>
                          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Monto Receptor</th>
                          <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Acciones</th>
                        </tr>
                      </thead>
                      <tbody id="display">
                      </tbody>
                    </table>
                </div>

                <!-- Modal Cargar Oficio -->
                <div class="modal fade" id="modalCargarOficio" tabindex="-1" role="dialog" aria-labelledby="modalCargarOficioLabel" aria-hidden="true">
                  <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                      <div class="modal-header bg-gradient-primary">
                        <h5 class="modal-title text-white" id="modalCargarOficioLabel"><i class="fas fa-file-upload me-2"></i>Cargar Oficio Inteligente</h5>
                        <button type="button" class="btn-close text-white" data-bs-dismiss="modal" aria-label="Close"></button>
                      </div>
                      <div class="modal-body p-4">
                        <p class="text-sm text-center mb-4 text-muted">Sube un oficio en formato PDF. El sistema extraerá automáticamente las adecuaciones y montos para procesarlos.</p>
                        <form id="formCargarOficio">
                          
                          <div class="modern-file-upload mb-4">
                              <input type="file" id="fileOficio" name="fileOficio" accept=".pdf" class="d-none" required onchange="updateFileName(this)">
                              <label for="fileOficio" class="modern-file-label d-flex flex-column align-items-center justify-content-center p-4 border-2 border-dashed rounded-3 cursor-pointer mb-0">
                                  <i class="fas fa-cloud-upload-alt text-primary mb-2" style="font-size: 2rem;"></i>
                                  <span class="font-weight-bold text-dark">Arrastra tu archivo PDF aquí</span>
                                  <span class="text-xs text-muted" id="fileOficioName">o haz clic para explorar (.pdf)</span>
                              </label>
                          </div>
                          <script>
                              function updateFileName(input) {
                                  if(input.files && input.files[0]) {
                                      document.getElementById('fileOficioName').innerText = input.files[0].name;
                                      document.getElementById('fileOficioName').classList.add('text-primary', 'font-weight-bold');
                                      document.getElementById('fileOficioName').closest('.modern-file-label').style.backgroundColor = '#e9ecef';
                                  } else {
                                      document.getElementById('fileOficioName').innerText = 'o haz clic para explorar (.pdf)';
                                      document.getElementById('fileOficioName').classList.remove('text-primary', 'font-weight-bold');
                                      document.getElementById('fileOficioName').closest('.modern-file-label').style.backgroundColor = '#f8f9fa';
                                  }
                              }
                          </script>

                          <div class="pretty-progress-container mb-3 mt-3" id="uploadProgressContainer" style="display: none;">
                              <div class="d-flex justify-content-between align-items-center mb-2">
                                  <span class="text-sm font-weight-bold" style="color: #cb0c9f;" id="uploadProgressLabel">Analizando Documento...</span>
                                  <span class="text-sm font-weight-bold" style="color: #cb0c9f;" id="uploadProgressText">0%</span>
                              </div>
                              <div class="progress" style="height: 12px; border-radius: 10px; background-color: #f8f9fa; box-shadow: inset 0 1px 2px rgba(0,0,0,.1);">
                                  <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" id="uploadProgressBar" style="width: 0%; background: linear-gradient(90deg, #cb0c9f, #7928ca); border-radius: 10px;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                              </div>
                          </div>

                          <div id="uploadResult" class="mt-3"></div>
                        </form>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cerrar</button>
                        <button type="button" class="btn btn-success" id="btnProcesarOficio" onclick="procesarOficio()">Procesar Archivo</button>
                      </div>
                    </div>
                  </div>
                </div>
                <!-- End Modal Cargar Oficio -->

                <!-- Modal Revision Oficio (Split Screen) -->
                <div class="modal fade" id="modalRevisionOficio" tabindex="-1" role="dialog" aria-labelledby="modalRevisionOficioLabel" aria-hidden="true" data-bs-backdrop="static">
                  <div class="modal-dialog modal-xl" role="document" style="max-width: 95%;">
                    <div class="modal-content" style="height: 90vh;">
                      <div class="modal-header bg-gradient-primary">
                        <h5 class="modal-title text-white" id="modalRevisionOficioLabel"><i class="fas fa-search me-2"></i>Revisión de Oficio Capturado</h5>
                        <button type="button" class="btn-close text-white" data-bs-dismiss="modal" aria-label="Close"></button>
                      </div>
                      <div class="modal-body p-0" style="overflow: hidden;">
                        <div class="row m-0 h-100">
                          <!-- Columna Izquierda: Datos Extraidos -->
                          <div class="col-md-6 h-100 p-3" style="overflow-y: auto; background-color: #f8f9fa;">
                            <h6 class="mb-3 font-weight-bold text-dark">Detalles y Movimientos</h6>
                            <div class="alert alert-warning text-white py-2 px-3 text-sm mb-3" style="border-radius: 10px; background: linear-gradient(135deg, #fb6340 0%, #fbb140 100%); border: none; box-shadow: 0 4px 6px rgba(251, 99, 64, 0.2);">
                              <i class="fas fa-exclamation-triangle me-2"></i>Por favor, verifica los detalles del oficio y corrige si es necesario antes de aplicar.
                            </div>

                            <!-- Tarjeta de Detalles del Oficio -->
                            <div class="card mb-3 shadow-none border" style="border-radius: 12px; background: #fff;">
                                <div class="card-body p-3">
                                    <div class="row">
                                        <div class="col-6 mb-2">
                                            <label class="text-xs font-weight-bold text-secondary mb-1">No. Solicitud</label>
                                            <input type="text" id="revNumSolicitud" class="form-control form-control-sm" style="border-radius: 8px;" />
                                        </div>
                                        <div class="col-6 mb-2">
                                            <label class="text-xs font-weight-bold text-secondary mb-1">Fecha Oficio</label>
                                            <input type="text" id="revFechaOficio" class="form-control form-control-sm" style="border-radius: 8px;" placeholder="DD/MM/YYYY" />
                                        </div>
                                        <div class="col-6 mb-2">
                                            <label class="text-xs font-weight-bold text-secondary mb-1">Solicita</label>
                                            <input type="text" id="revSolicita" class="form-control form-control-sm" style="border-radius: 8px;" />
                                        </div>
                                        <div class="col-6">
                                            <label class="text-xs font-weight-bold text-secondary mb-1">Autoriza</label>
                                            <input type="text" id="revAutoriza" class="form-control form-control-sm" style="border-radius: 8px;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="modern-scroll" style="max-height: 60vh;">
                              <div id="listaRevisionMovimientos">
                                  <!-- Llenado por JS con cards -->
                              </div>
                              <div class="text-center mt-3 mb-2">
                                  <button type="button" class="btn btn-sm bg-gradient-info text-white shadow-sm" onclick="agregarMovimientoRevisionManual()">
                                      <i class="fas fa-plus me-2"></i> Agregar Movimiento Manual
                                  </button>
                              </div>
                            </div>
                            
                            <div class="totals-panel">
                                <div class="totals-item">
                                    <span class="totals-label">Total Transacciones</span>
                                    <span class="totals-value text-dark" id="revTotalTrans">0</span>
                                </div>
                                <div class="totals-item">
                                    <span class="totals-label">Total Ampliación</span>
                                    <span class="totals-value text-success" id="revTotalAmp">$0.00</span>
                                </div>
                                <div class="totals-item">
                                    <span class="totals-label">Total Reducción</span>
                                    <span class="totals-value text-danger" id="revTotalRed">$0.00</span>
                                </div>
                            </div>
                          </div>
                          
                          <!-- Columna Derecha: Previsualización PDF -->
                          <div class="col-md-6 h-100 p-0 border-start">
                            <iframe id="previewFrame" style="width: 100%; height: 100%; border: none;"></iframe>
                          </div>
                        </div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-success" id="btnConfirmarRevision" onclick="confirmarRevisionOficio()"><i class="fas fa-check me-2"></i>Confirmar y Aplicar</button>
                      </div>
                    </div>
                  </div>
                </div>
                <!-- End Modal Revision Oficio -->
                <%--=============================================================================================== --%>
                <%--=========================GRID PRINCIPAL DE LAS ADECUACIONES==================================== --%>
                <%--=============================================================================================== --%>

                <%--=============================================================================================== --%>
                <%--===================================MODAL CAPTURA ADECUACIÓN==================================== --%>
                <%--=============================================================================================== --%>
                <%-- DESACTIVADO PORQUE LA CARGA DE OFICIO PDF YA INCLUYE MOVIMIENTOS MANUALES. SE DEJA COMENTADO POR SI SE REQUIERE REACTIVAR 
                <div class="modal fade" id="modal-form"  role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true"  style="zoom:90%;">
                  <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-gradient-primary">
                          <h5 class="modal-title" id="exampleModalLabel" style="color:white;">Adecuación Presupuestaria</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                        </button>
                      </div>
                      <div class="modal-body p-1">
                        <div class="card card-plain">
                          <div class="card-body">
                          <div class="card-body">
                              <div class="row">                              
                              <!-- Removed txtoficionumero --> 
                              <div class="col-md-6">
                                <div class="form-group">
                                  <label>Asunto</label>
                                <input id="txtasunto" type="text" class="form-control"  aria-describedby="password-addon" value="" onkeypress="nextFocus('txtasunto', 'txtnumsolicitud')"  />
                              </div>
                              </div>
                              <div class="col-md-3">
                                <div class="form-group">
                                  <label>Número Solicitud</label>
                                <input id="txtnumsolicitud" type="text" class="form-control"  aria-describedby="password-addon" value=""  onkeypress="nextFocus('txtnumsolicitud', 'cbotipoadecuacion')" />
                              </div>
                              </div>
                              </div>
                              <div class="row">                              
                                <div class="col-md-3" style="display:none;">
                                  <div class="form-group">
                                    <label>Tipo de Adecuación</label>
                                      <div class="dropdown w-100">
                                          <button class="form-control form-control-sm w-100 d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                              <span id="text-cbotipoadecuacion">Seleccione el Tipo de Adecuación</span>
                                              <i class="fas fa-chevron-down text-xs"></i>
                                          </button>
                                          <ul class="dropdown-menu w-100 custom-select-menu" id="menu-cbotipoadecuacion">
                                              <li><a class="dropdown-item custom-select-item active" data-value="">Seleccione el Tipo de Adecuación</a></li>
                                          </ul>
                                      </div>
                                      <input type="hidden" id="cbotipoadecuacion" name="cbotipoadecuacion" value="" onchange="llenadotipooperacion();" /> 
                                  </div>
                                </div> 
                                <div class="col-md-3">
                                  <div class="form-group">
                                    <label>Fecha Solicitud</label>
                                  <input id="txtfechasolicitud" type="date"  class="form-control date-picker"  aria-describedby="password-addon" value="" onkeypress="nextFocus('txtfechasolicitud', 'txtconcepto')" />
                                  </div>
                                </div>
                                <div class="col-md-6">
                                  <div class="form-group">
                                    <label>Concepto</label>
                                    <input id="txtconcepto" type="text" class="form-control"  aria-describedby="password-addon" value="" onkeypress="nextFocus('txtconcepto', 'txtmontoadecuacion')" />
                                  </div>
                                </div>
                                <div class="col-md-3">
                                    <style>
                                      #chkApartado:checked {
                                        background-color: #621132 !important;
                                        border-color: #621132 !important;
                                      }
                                    </style>
                                    <div class="form-check form-switch mt-1 mb-1">
                                      <input class="form-check-input" type="checkbox" id="chkApartado" onchange="toggleApartado()">
                                      <label class="form-check-label" for="chkApartado" style="font-weight: bold; font-size: 0.85rem;">¿Viene de Apartado?</label>
                                    </div>
                                    <div id="divFolioApartado" style="display:none;">
                                      <div class="input-group input-group-sm">
                                        <input type="text" id="txtFolioApartado" class="form-control" placeholder="Folio (Ej. 12)">
                                        <button class="btn text-white mb-0" style="background-color: #621132;" type="button" onclick="buscarApartado()">Buscar</button>
                                      </div>
                                    </div>
                                </div>
                                </div>
                              <div class="row">                              
                                <div class="col-md-12 text-end">
                                  <div class="form-group" style="font-weight: bold; font-size: 1.2rem; margin-top: 10px;">
                                    <label id="lblsumaadecuacion" title=""></label>    
                                  </div>
                                </div>
                              </div>
                              <div class="row" id="general" style="display:block; margin-top: 20px;">
                               <div class="col-md-12">
                                <div class="card shadow-sm border mb-4">
                                 <div class="card-body p-3">
                                  <div class="row align-items-end mb-3">
                                    <div class="col-md-5">
                                      <div class="form-group mb-0">
                                        <label class="font-weight-bold text-primary">Clave Presupuestal</label>
                                        <input id="txtclavebusqueda" style="font-weight:bold;" type="text" placeholder="Escriba Clave Presupuestal" class="form-control" onchange="limpiarclave()" onkeypress="nextFocus('txtclavebusqueda', 'cbotipoclave')"  />
                                      </div>
                                    </div>
                                    <div class="col-md-3">
                                      <div class="form-group mb-0">
                                        <label class="font-weight-bold text-primary">Tipo de Clave</label>
                                        <div class="dropdown w-100">
                                            <button class="form-control d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                                <span id="text-cbotipoclave">EXISTENTE</span>
                                                <i class="fas fa-chevron-down text-xs"></i>
                                            </button>
                                            <ul class="dropdown-menu w-100 custom-select-menu" id="menu-cbotipoclave">
                                                <li><a class="dropdown-item custom-select-item active" data-value="EX">EXISTENTE</a></li>
                                                <li><a class="dropdown-item custom-select-item" data-value="NU">NUEVO</a></li>
                                            </ul>
                                        </div>
                                        <input type="hidden" id="cbotipoclave" name="cbotipoclave" value="EX" onchange="ventananuevaclave();" />
                                      </div>
                                    </div>
                                    <div class="col-md-2" style="display:none;">
                                      <div class="form-group mb-0">
                                        <label>Estatus</label>
                                        <input type="hidden" id="cbostatusadecuacion" name="cbostatusadecuacion" value="" /> 
                                      </div>
                                    </div>
                                    <div class="col-md-4">
                                      <div class="form-group mb-0">
                                        <label class="font-weight-bold text-primary">Tipo de Operación</label>
                                        <div class="dropdown w-100">
                                            <button class="form-control d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                                <span id="text-t_o">(Seleccione)</span>
                                                <i class="fas fa-chevron-down text-xs"></i>
                                            </button>
                                            <ul class="dropdown-menu w-100 custom-select-menu" id="menu-t_o">
                                                <li><a class="dropdown-item custom-select-item active" data-value="">(Seleccione)</a></li>
                                            </ul>
                                        </div>
                                        <input type="hidden" id="t_o" name="t_o" value="" /> 
                                      </div>
                                    </div>
                                  </div>

                                  <div class="alert alert-info" role="alert" id="nuevaclave" style="display:none;">
                                        <strong>crear nueva clave</strong> verificar los catálogos
                                  </div>

                                  <!-- Fragmented Keys (Hidden to save space, but maintained for backend/autocomplete compatibility) -->
                                  <div style="display:none;">
                                      <input type="text" id="sec" /><input type="text" id="ca" /><input type="text" id="ur" /><input type="text" id="fi" />
                                      <input type="text" id="fu" /><input type="text" id="sf" /><input type="text" id="ssf" /><input type="text" id="ps" />
                                      <input type="text" id="pp" /><input type="text" id="oa" /><input type="text" id="ai" /><input type="text" id="pt" />
                                      <input type="text" id="mpio" /><input type="text" id="ga" /><input type="text" id="cogc" /><input type="text" id="gasto" />
                                      <input type="text" id="gto" /><input type="text" id="ag" /><input type="text" id="tg" /><input type="text" id="tr" />
                                      <input type="text" id="ff" /><input type="text" id="sfte" /><input type="text" id="rm" /><input type="text" id="pf" />
                                      <input type="text" id="cp" /><input type="text" id="dm" />
                                      <input type="text" id="nl" /><input type="text" id="li" />
                                  </div>

                                  <!-- Ingreso Segmentado Visual -->
                                  <div class="row mt-3 mb-2" id="segmented_key_container">
                                      <div class="col-md-12">
                                          <label class="text-xs font-weight-bold text-primary mb-2">CLAVE SEGMENTADA</label>
                                          <div class="d-flex flex-nowrap" style="width: 100%; overflow-x: auto; padding-bottom: 5px;">
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">UR</label><br/><input type="text" id="seg_ur" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">FI</label><br/><input type="text" id="seg_fi" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">F</label><br/><input type="text" id="seg_fu" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">SF</label><br/><input type="text" id="seg_sf" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">SSF</label><br/><input type="text" id="seg_ssf" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">PS</label><br/><input type="text" id="seg_ps" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">PP</label><br/><input type="text" id="seg_pp" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">OA</label><br/><input type="text" id="seg_oa" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">AI</label><br/><input type="text" id="seg_ai" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">PT</label><br/><input type="text" id="seg_pt" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">MPIO</label><br/><input type="text" id="seg_mpio" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">GA</label><br/><input type="text" id="seg_ga" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">COGC</label><br/><input type="text" id="seg_cogc" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">GASTO</label><br/><input type="text" id="seg_gasto" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">GTO</label><br/><input type="text" id="seg_gto" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">AG</label><br/><input type="text" id="seg_ag" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">TG</label><br/><input type="text" id="seg_tg" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">TR</label><br/><input type="text" id="seg_tr" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">FF</label><br/><input type="text" id="seg_ff" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">SFTE</label><br/><input type="text" id="seg_sfte" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">RM</label><br/><input type="text" id="seg_rm" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">PF</label><br/><input type="text" id="seg_pf" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">CP</label><br/><input type="text" id="seg_cp" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                              <div class="text-center" style="flex: 1; padding: 0 2px;"><label class="text-xxs mb-0" style="color:#1d2939;">DM</label><br/><input type="text" id="seg_dm" class="form-control form-control-sm px-0 text-center" style="width: 100%; height: 30px; font-size: 0.75rem; background-color:#f8f9fa;" readonly /></div>
                                          </div>
                                      </div>
                                  </div>

                                  <div class="row mb-3 mt-3">
                                      <div class="col-md-3">
                                        <div class="form-group mb-0">
                                          <label class="text-xs">IMPORTE TOTAL</label>
                                          <input type="text" class="form-control form-control-sm" id="montoliberar" size="21" maxlength="22" onkeypress="mascara(this,cpf)" onpaste="return false" style="border-color: #f5365c;" />
                                        </div>
                                      </div>
                                      <script>
                                          function nextFocus(inputF, inputS) {
                                            let el = document.getElementById(inputF);
                                            if(el) {
                                                el.addEventListener('keydown', function (event) {
                                                    if (event.keyCode == 13) { document.getElementById(inputS).focus(); }
                                                });
                                            }
                                          }
                                      </script>
                                  </div>

                                  <div id="divMesesGrid" style="display:none;">
                                    <hr class="horizontal dark mt-2 mb-3">
                                    
                                    <div class="row mb-1">
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">ENERO</label><input type="text" class="form-control form-control-sm" id="txtenero" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">FEBRERO</label><input type="text" class="form-control form-control-sm" id="txtfebrero" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">MARZO</label><input type="text" class="form-control form-control-sm" id="txtmarzo" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">ABRIL</label><input type="text" class="form-control form-control-sm" id="txtabril" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">MAYO</label><input type="text" class="form-control form-control-sm" id="txtmayo" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-2"><label class="text-xs">JUNIO</label><input type="text" class="form-control form-control-sm" id="txtjunio" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                    </div>
                                    <div class="row mb-3">
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">JULIO</label><input type="text" class="form-control form-control-sm" id="txtjulio" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">AGOSTO</label><input type="text" class="form-control form-control-sm" id="txtagosto" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">SEPTIEMBRE</label><input type="text" class="form-control form-control-sm" id="txtseptiembre" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">OCTUBRE</label><input type="text" class="form-control form-control-sm" id="txtoctubre" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">NOVIEMBRE</label><input type="text" class="form-control form-control-sm" id="txtnoviembre" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                      <div class="col-md-2"><div class="form-group mb-0"><label class="text-xs">DICIEMBRE</label><input type="text" class="form-control form-control-sm" id="txtdiciembre" onkeypress="mascara(this,cpf)" onpaste="return false" /></div></div>
                                    </div>
                                  </div>

                                  <div class="row align-items-center mt-3">
                                      <div class="col-sm-12 text-end">
                                            <button class="btn btn-outline-danger mb-0 me-2" id="btncancelarcambios" onclick="btncancelarcambios();">Cancelar</button>
                                            <button class="btn btn-primary mb-0" style="background-color: #621132;" id="btnagregarclavepresupuestal" onclick="btnagregarclavepresupuestal();"><i class="fas fa-plus"></i> Agregar Movimiento</button>
                                      </div>  
                                  </div>  
                                 </div>
                                </div>

                                <div class="card shadow-sm border bg-white mb-4">
                                  <div class="table-responsive p-0">
                                    <table class="table align-items-center mb-0 w-100 table-hover" id="tablaclavepres">
                                      <thead>
                                        <tr>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Num. Sec.</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Clave Presupuestal</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">T_O</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Importe Operación</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">DE</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">AL</th>
                                          <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Importe por mes</th>
                                        </tr>
                                      </thead>
                                      <tbody id="contenidoadecuacion">
                                      </tbody>
                                    </table>
                                  </div>
                                </div>
                               </div>   
                              </div>   
                              
                          </div>
                        </div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-outline-primary" data-bs-dismiss="modal">Cerrar</button>
                        <button type="button" class="btn btn-success" onclick="guardarAdecuacionManual()">Guardar Adecuación</button>
                      </div>
                    </div>
                  </div>
                </div>
                --%>
                <%--===================================MODAL CAPTURA ADECUACIÓN==================================== --%>
                <%--=============================================================================================== --%>
                  </div>
                </div>

                <%--=============================================================================================== --%>
                <%--===================================MODAL UPLOAD EXCEL ADECUACIONES============================= --%>
                <%--=============================================================================================== --%>
                <div class="modal fade" id="modal-upload-adec" tabindex="-1" role="dialog" aria-hidden="true">
                  <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                      <div class="modal-header bg-gradient-primary">
                        <h5 class="modal-title" style="color:white;">Carga Masiva de Adecuaciones</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                      </div>
                      <div class="modal-body">
                        <p class="text-sm">Seleccione el archivo <b>CONCENTRADO DE ADECUACIONES</b> en formato Excel (.xlsx).</p>
                        <input type="file" id="fileAdecExcel" class="form-control" accept=".xlsx, .xls" />
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn-primary" onclick="uploadExcelAdecuaciones()">Cargar Archivo</button>
                      </div>
                    </div>
                  </div>
                </div>

              </div>
            </div>
          </div>
        </div>
      </div>
      <%--=============================================================================================== --%>
      <%--======================MODAL PARA DAR DE ALTA LA NUEVA CLAVE==================================== --%>
      <%--=============================================================================================== --%>
      <div class="modal fade" id="altanuevaclave" role="dialog" aria-labelledby="modal-form" aria-hidden="true"  style="zoom:90%;">
        <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #6c757d;">
            <h5 class="modal-title" id="titulo" style="color:white;">Adecuación Presupuestaria</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" <%--style="color:red;opacity:1;"--%>>
            </button>
            </div>
            <div class="modal-body p-1">
                <h2>Nueva clave presupuestal</h2>
                <h2>Catálogo1</h2>
                <h2>Catálogo2</h2>
                <h2>Catálogo3</h2>

            </div>
        </div>
        </div>
       </div>
      <%--=============================================================================================== --%>
      <%--======================MODAL PARA DAR DE ALTA LA NUEVA CLAVE==================================== --%>
      <%--=============================================================================================== --%>
      <!-- MODAL DECIFRADOR DE CLAVES -->
      <div class="modal fade" id="modal-decifrador" tabindex="-1" role="dialog" aria-hidden="true" style="zoom:90%;">
        <div class="modal-dialog modal-xl" role="document">
          <div class="modal-content">
            <div class="modal-header bg-gradient-primary">
                <h5 class="modal-title text-white">Desglose de Clave Presupuestaria</h5>
                <button type="button" class="btn-close text-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-4">
                <p class="text-sm mb-4">Clave analizada: <strong id="titulo-clave" class="text-primary fs-5 ms-2" style="letter-spacing: 2px;"></strong></p>
                <div class="table-responsive">
                    <table class="table table-striped table-bordered table-sm align-items-center mb-0">
                        <thead class="bg-light">
                            <tr>
                                <th class="text-uppercase text-xxs font-weight-bolder text-center">Acrónimo</th>
                                <th class="text-uppercase text-xxs font-weight-bolder">Concepto</th>
                                <th class="text-uppercase text-xxs font-weight-bolder text-center">Valor Extraído</th>
                                <th class="text-uppercase text-xxs font-weight-bolder">Descripción en Catálogo</th>
                            </tr>
                        </thead>
                        <tbody id="cuerpo-decifrador">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar Desglose</button>
            </div>
          </div>
        </div>
      </div>

      <footer class="footer pt-3  ">
        <div class="container-fluid">
          <div class="row align-items-center justify-content-lg-between">
            <div class="col-lg-6 mb-lg-0 mb-4">
              <div class="copyright text-center text-sm text-muted text-lg-start">
                © <script>
                  document.write(new Date().getFullYear())
                </script>,
                Secretaría de Infraestructura.
              </div>
            </div>
          </div>
        </div>
      </footer>
  </div>
  <!--   Core JS Files   -->
  <script src="assets/js/core/popper.min.js"></script>
  <script src="assets/js/core/bootstrap.min.js"></script>
  <script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
  <script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
  <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
  <!-- DataTables JS -->
  <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
  <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js"></script>
  <script src="assets/js/Adecuaciones.js?v=6.7"></script>
  <script src="assets/js/EliminarSesion.js"></script>
  <script>
    var win = navigator.platform.indexOf('Win') > -1;
      if (win && document.querySelector('#sidenav-scrollbar'))
      {
      var options = {
        damping: '0.5'
      }
      Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
      }
      adec();
      solicitudes();
      tipos();
      mes();
      anios();

      function decifrarClave(clave) {
          const segmentos = [
              { acronimo: "UR", nombre: "UNIDAD RESPONSABLE", longitud: 2 },
              { acronimo: "UR0", nombre: "UNIDAD RESPONSABLE OPERATIVA", longitud: 3 },
              { acronimo: "FI", nombre: "FINALIDAD", longitud: 1 },
              { acronimo: "F", nombre: "FUNCION", longitud: 1 },
              { acronimo: "SF", nombre: "SUBFUNCION", longitud: 1 },
              { acronimo: "SSF", nombre: "SUBSUBFUNCION", longitud: 1 },
              { acronimo: "PS", nombre: "PROGRAMA SECTORIAL", longitud: 1 },
              { acronimo: "PP", nombre: "PROGRAMA PRESUPUESTARIO", longitud: 3 },
              { acronimo: "OA", nombre: "ORIGEN DE LA ASIGNACION", longitud: 2 },
              { acronimo: "AI", nombre: "ACTIVIDAD INSTITUCIONAL", longitud: 3 },
              { acronimo: "PT", nombre: "PROYECTO ESTRATEGICO", longitud: 4 },
              { acronimo: "MPIO", nombre: "MUNICIPIO", longitud: 3 },
              { acronimo: "GA", nombre: "ASIGNACION DEL GASTO", longitud: 1 },
              { acronimo: "COGC", nombre: "OBJETO DEL GASTO POR CATEGORIA", longitud: 2 },
              { acronimo: "GASTO", nombre: "OBJETO DEL GASTO", longitud: 5 },
              { acronimo: "GTO", nombre: "ORIENTACION DEL GASTO", longitud: 2 },
              { acronimo: "AG", nombre: "AÑO DEL GASTO", longitud: 2 },
              { acronimo: "TG", nombre: "TIPO DE GASTO", longitud: 1 },
              { acronimo: "TR", nombre: "TIPO DE RECURSO", longitud: 1 },
              { acronimo: "FF", nombre: "FUENTE DE FINANCIAMIENTO", longitud: 1 },
              { acronimo: "SFTE", nombre: "SUBFUENTE DE FINANCIAMIENTO", longitud: 1 },
              { acronimo: "RM", nombre: "RAMO", longitud: 2 },
              { acronimo: "PF", nombre: "PROGRAMA FONDO", longitud: 7 },
              { acronimo: "RI", nombre: "RUBRO DE INGRESO", longitud: 3 },
              { acronimo: "CP", nombre: "CICLO PRESUPUESTARIO", longitud: 4 },
              { acronimo: "DM", nombre: "DIGITO DE MINISTRACION", longitud: 1 }
          ];

          const catalogoGasto = {
              "11305": "Sueldo al Personal Docente con Carrera Magisterial.",
              "11311": "Sueldo al Personal Sindicalizado.",
              "11312": "Se deroga.",
              "11313": "Se deroga.",
              "11314": "Se deroga.",
              "11315": "Se deroga.",
              "11316": "Se deroga.",
              "00114": "Remuneraciones por Adscripción Laboral en el Extranjero.",
              "01200": "REMUNERACIONES AL PERSONAL DE CARÁCTER TRANSITORIO."
          };

          let index = 0;
          let html = '';
          
          if (clave.length !== 58) {
              html = '<tr><td colspan="4" class="text-danger text-center">Error: La clave debe tener 58 caracteres. Actualmente tiene ' + clave.length + '.</td></tr>';
          } else {
              segmentos.forEach(seg => {
                  let valor = clave.substring(index, index + seg.longitud);
                  index += seg.longitud;
                  
                  let descripcion = "";
                  if (seg.acronimo === "GASTO" && catalogoGasto[valor]) {
                      descripcion = '<span class="badge bg-success" style="font-size: 11px;">' + catalogoGasto[valor] + '</span>';
                  } else if (seg.acronimo === "GASTO") {
                      descripcion = '<span class="text-muted" style="font-size: 11px;">No encontrado en catálogo (' + valor + ')</span>';
                  } else {
                      descripcion = '<span class="text-muted" style="font-size: 11px;">N/A</span>';
                  }

                  html += '<tr>' +
                      '<td class="text-xs font-weight-bold text-center">' + seg.acronimo + '</td>' +
                      '<td class="text-xs">' + seg.nombre + '</td>' +
                      '<td class="text-xs font-weight-bold text-primary text-center">' + valor + '</td>' +
                      '<td class="text-xs">' + descripcion + '</td>' +
                  '</tr>';
              });
          }
          
          document.getElementById('cuerpo-decifrador').innerHTML = html;
          document.getElementById('titulo-clave').innerText = clave;
      }
  </script>
</body>
</html>
