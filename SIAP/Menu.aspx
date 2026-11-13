<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="SIAP.Menu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
<link rel="icon" type="image/png" href="assets/img/favicon.png"/>
    <title>SOP | SIAP</title>
<!--    Fonts and icons     -->
<link href="assets/css/googleapis.css" rel="stylesheet" />
<!-- Nucleo Icons -->
<link href="assets/css/nucleo-icons.css" rel="stylesheet" />
<link href="assets/css/nucleo-svg.css" rel="stylesheet" />
<!-- Font Awesome Icons -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" integrity="sha512-iecdLmaskl7CVkqkXNQ/ZH/XLlvWZOJyj7Yy7tcenmpD1ypASozpmT/E0iPtmFIB46ZmdtAc9eNBvH0H/ZpiBw==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<link href="assets/css/nucleo-svg.css" rel="stylesheet" />
<link href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" rel="stylesheet" />
<script src="assets/js/SweetAlert2/popper/popper.min.js"></script>
<!-- CSS Files -->
<link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
<link href="assets/css/gobierno-theme.css" rel="stylesheet" />

    <script>
        var data = sessionStorage.getItem('name');
        if (data == null) {
            window.location.href = "Login.aspx";
        }
    </script>
    <style>
      .module-card {
        transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
        border-radius: 1rem;
        overflow: hidden;
        position: relative;
        border: none;
        box-shadow: 0 4px 6px rgba(0,0,0,0.05);
        background: white;
        cursor: pointer;
        min-height: 180px;
      }
      .module-card:hover {
        transform: translateY(-10px);
        box-shadow: 0 15px 30px rgba(0,0,0,0.1);
      }
      .module-icon-container {
        width: 60px;
        height: 60px;
        border-radius: 12px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24px;
        margin-bottom: 15px;
      }
      .module-card .card-body {
        padding: 2rem;
        display: flex;
        flex-direction: column;
        justify-content: center;
      }
      .gradient-primary { background: linear-gradient(135deg, #5e72e4 0%, #825ee4 100%); color: white; }
      .gradient-success { background: linear-gradient(135deg, #2dce89 0%, #2dcecc 100%); color: white; }
      .gradient-info { background: linear-gradient(135deg, #11cdef 0%, #1171ef 100%); color: white; }
      .gradient-warning { background: linear-gradient(135deg, #fb6340 0%, #fbb140 100%); color: white; }
      .gradient-danger { background: linear-gradient(135deg, #f5365c 0%, #f56036 100%); color: white; }
      .gradient-dark { background: linear-gradient(135deg, #212529 0%, #41464b 100%); color: white; }
      
      a.module-link { text-decoration: none; color: inherit; }
      a.module-link:hover { color: inherit; }
    </style>
</head>
<body class="g-sidenav-show virtual-reality">
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:UpdatePanel ID="updatepanelobras" runat="server">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000000" runat="server"></asp:ScriptManager>
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
                function endReq(sender, args) {
                    $("input,select").bind("keydown", function (e) {
                        var keyCode = e.keyCode || e.which;
                        if (keyCode === 13) {
                            e.preventDefault();
                            $('input, select, textarea')
                            [$('input,select,textarea').index(this) + 1].focus();
                        }
                    });
                }
            </script>
            
    <!-- Navbar -->
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="javascript:;">Men&uacute;</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Principal</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0">SIAP</h6>
        </nav>
        <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
          <h3 style="color:white;">Secretar&iacute;a de Infraestructura</h3>
          <div class="ms-md-auto pe-md-3 d-flex align-items-center">
            </div>
          </div>
          <ul class="navbar-nav  justify-content-end">
          <%--  <li class="nav-item px-3 d-flex align-items-center">
              <a href="javascript:;" class="nav-link text-white p-0">
                <i class="ni ni-button-power fixed-plugin-button-nav cursor-pointer"></i>
              </a>
            </li>--%>
            <li class="nav-item dropdown pe-2 d-flex align-items-center">
              <a href="javascript:;" class="nav-link text-white p-0" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                <i class="ni ni-settings-gear-65 cursor-pointer"></i> Opciones
              </a>
              <ul class="dropdown-menu  dropdown-menu-end  px-2 py-3 me-sm-n4" aria-labelledby="dropdownMenuButton">
                <li class="mb-2">
                  <a class="dropdown-item border-radius-md" href="javascript:;" onclick="eliminarsesion()">
                    <div class="d-flex py-1">
                     <%-- <div class="my-auto">
                        <img src="assets/img/team-2.jpg" class="avatar avatar-sm  me-3 ">
                      </div>--%>
                      <div class="d-flex flex-column justify-content-center">
                        <h6 class="text-sm font-weight-normal mb-1">
                          <span class="font-weight-bold">Cerrar Sesión</span> 
                        </h6>
                       <%-- <p class="text-xs text-secondary mb-0">
                          <i class="fa fa-clock me-1"></i>
                          13 minutes ago
                        </p>--%>
                      </div>
                    </div>
                  </a>
                </li>
                <li class="mb-2">
                  <a class="dropdown-item border-radius-md" href="Configuracion.aspx">
                    <div class="d-flex py-1">
                   <%--   <div class="my-auto">
                        <img src="assets/img/small-logos/logo-spotify.svg" class="avatar avatar-sm bg-gradient-dark  me-3 ">
                      </div>--%>
                      <div class="d-flex flex-column justify-content-center">
                        <h6 class="text-sm font-weight-normal mb-1">
                          <span class="font-weight-bold">Configuración</span>
                        </h6>
                 <%--       <p class="text-xs text-secondary mb-0">
                          <i class="fa fa-clock me-1"></i>
                          1 day
                        </p>--%>
                      </div>
                    </div>
                  </a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      <!-- </div> -->
    </nav>
    <!-- End Navbar -->
  
  <main class="main-content mt-1 border-radius-lg">
    <div class="container-fluid py-4">
      <div class="row">
        
        <!-- 1. Dashboard -->
        <% if (Session["VerDashboard"] != null && (bool)Session["VerDashboard"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Dashboard.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-info shadow-info">
                  <i class="fa-solid fa-chart-pie"></i>
                </div>
                <h5 class="font-weight-bolder">Dashboard</h5>
                <p class="text-sm text-muted mb-0">Indicadores globales y resumen financiero del ejercicio.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>
        
        <!-- 1b. Presupuesto Inicial (carga del Excel) -->
        <% if (Session["VerPresupuesto"] != null && (bool)Session["VerPresupuesto"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="PresupuestoInicial.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-success shadow-success">
                  <i class="fa-solid fa-file-import"></i>
                </div>
                <h5 class="font-weight-bolder">Presupuesto Inicial</h5>
                <p class="text-sm text-muted mb-0">Carga del Excel del ejercicio, validaci&oacute;n por fila e historial de cargas.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 2. Presupuesto -->
        <% if (Session["VerPresupuesto"] != null && (bool)Session["VerPresupuesto"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Presupuesto_Inicial.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-primary shadow-primary">
                  <i class="fa-solid fa-sack-dollar"></i>
                </div>
                <h5 class="font-weight-bolder">Presupuesto</h5>
                <p class="text-sm text-muted mb-0">Matriz principal de presupuesto inicial y calendarizado.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 3. Adecuaciones -->
        <% if (Session["VerAdecuaciones"] != null && (bool)Session["VerAdecuaciones"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Adecuaciones.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-warning shadow-warning">
                  <i class="fa-solid fa-money-bill-transfer"></i>
                </div>
                <h5 class="font-weight-bolder">Adecuaciones</h5>
                <p class="text-sm text-muted mb-0">Gestión de traspasos, ampliaciones y reducciones.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 4. Ministraciones -->
        <% if (Session["VerMinistraciones"] != null && (bool)Session["VerMinistraciones"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Ministraciones.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-success shadow-success">
                  <i class="fa-solid fa-building-columns"></i>
                </div>
                <h5 class="font-weight-bolder">Ministraciones</h5>
                <p class="text-sm text-muted mb-0">Liberaci&oacute;n de fondos y flujo de liquidez bancaria.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 5. Gastos -->
        <% if (Session["UsuarioRol"] != null && Session["UsuarioRol"].ToString() == "Administrador") { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="#" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-danger shadow-danger">
                  <i class="fa-solid fa-file-invoice-dollar"></i>
                </div>
                <h5 class="font-weight-bolder">Gastos y Pagos</h5>
                <p class="text-sm text-muted mb-0">Control de ventanilla, comprobaciones y facturaci&oacute;n.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 6. Catálogos / Config -->
        <% if (Session["VerCatalogos"] != null && (bool)Session["VerCatalogos"]) { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Catalogos.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-dark shadow-dark">
                  <i class="fa-solid fa-server"></i>
                </div>
                <h5 class="font-weight-bolder">Cat&aacute;logos</h5>
                <p class="text-sm text-muted mb-0">Configuraci&oacute;n de matrices, URs, Regiones y fuentes.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 7. Usuarios -->
        <% if (Session["UsuarioRol"] != null && Session["UsuarioRol"].ToString() == "Administrador") { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Usuarios.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container bg-gradient-secondary shadow-secondary" style="color: white;">
                  <i class="fa-solid fa-users"></i>
                </div>
                <h5 class="font-weight-bolder">Usuarios</h5>
                <p class="text-sm text-muted mb-0">Gestión de cuentas de acceso, roles y permisos.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

        <!-- 8. Bitacora -->
        <% if (Session["UsuarioRol"] != null && Session["UsuarioRol"].ToString() == "Administrador") { %>
        <div class="col-xl-4 col-md-6 mb-4">
          <a href="Bitacora.aspx" class="module-link">
            <div class="card module-card">
              <div class="card-body">
                <div class="module-icon-container gradient-dark shadow-dark">
                  <i class="fa-solid fa-clock-rotate-left"></i>
                </div>
                <h5 class="font-weight-bolder">Bit&aacute;cora</h5>
                <p class="text-sm text-muted mb-0">Historial de movimientos y auditor&iacute;a de usuarios.</p>
              </div>
            </div>
          </a>
        </div>
        <% } %>

      </div>
    </div>
  </main>
        </ContentTemplate>
      </asp:UpdatePanel>  
    </form>
<!--   Core JS Files   -->
  <script src="assets/js/core/popper.min.js"></script>
  <script src="assets/js/core/bootstrap.min.js"></script>
  <script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
  <script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
  <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
  <script src="assets/js/EliminarSesion.js"></script>
  <script>
    var win = navigator.platform.indexOf('Win') > -1;
    if (win && document.querySelector('#sidenav-scrollbar')) {
      var options = {
        damping: '0.5'
      }
      Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
    }
  </script>
  <!-- Github buttons -->
  <script async defer src="assets/js/buttons.js"></script>
</body>
</html>
