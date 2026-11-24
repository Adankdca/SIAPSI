<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="SIAP.Configuracion" %>

<!DOCTYPE html>
<html lang="es">

<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link rel="apple-touch-icon" sizes="76x76" href="assets/img/apple-icon.png">
  <link rel="icon" type="image/png" href="assets/img/favicon.png">
  <title>
    SIAP - CONFIGURACION
  </title>
  <!--     Fonts and icons     -->
    <script src="assets/js/jquery-3.5.1.js"></script>
  <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
  <!-- Nucleo Icons -->
  <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- Font Awesome Icons -->
  <script src="https://kit.fontawesome.com/42d5adcbca.js" crossorigin="anonymous"></script>
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- CSS Files -->
  <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.4" rel="stylesheet" />
  <link href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" rel="stylesheet" />
  <script src="assets/js/SweetAlert2/popper/popper.min.js"></script>
    <style>
        .sinborde{
              border: 0;
        }
        .conborde{
            border: 2px;
        }
        .linea
        {
            display: inline-block;
        }
    </style>
    <script src="assets/js/SesionExpirada.js"></script>
</head>

<body class="g-sidenav-show bg-gray-100">
  <div class="position-absolute w-100 min-height-300 top-0 bg-gradient-primary">
    <!-- <span class="mask bg-primary opacity-6"></span> -->
  </div>
  <div class="main-content position-relative max-height-vh-100 h-100">
    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg blur border-radius-lg top-0 z-index-3 shadow position-absolute mt-4 py-2 start-0 end-0 mx-4">
      <div class="container-fluid">
                   <a class="navbar-brand font-weight-bolder ms-lg-0 ms-3 ">
                     <strong>Ruta:Menú > Configuración</strong>
                   </a>
                   <button class="navbar-toggler shadow-none ms-2" type="button" data-bs-toggle="collapse" data-bs-target="#navigation" aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
                     <span class="navbar-toggler-icon mt-2">
                       <span class="navbar-toggler-bar bar1"></span>
                       <span class="navbar-toggler-bar bar2"></span>
                       <span class="navbar-toggler-bar bar3"></span>
                     </span>
                   </button>
                   <div class="ms-md-auto pe-md-3 d-flex align-items-center">
                       <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                           <i class="fa-solid fa-arrow-left me-2"></i> Regresar
                       </a>
                   </div>
                     <h4 style="color:var(--color-guinda);">Secretaría de Infraestructura</h4>
                 </div>
   </nav>
    <!-- End Navbar -->

    <div class="container-fluid py-4 mt-9">
      <div class="row">
        <div class="col-md-2">

        </div>
        <div class="col-md-8">
          <div class="card">
            <div class="card-header pb-0">
              <div class="d-flex align-items-center">
                <p class="mb-0"><b>DATOS GENERALES</b></p>
                <button class="btn btn-primary btn-sm ms-auto mt-3" onclick="actualizarConf()">Modificar</button>
              </div>
            </div>
            <div class="card-body">
              <div class="row" style="background-color:var(--color-guinda); color:white; align-items:center">
                <div class="col-md-5 mt-3">
                  <p class="text-uppercase text-sm" id="dependencia"></p>
                </div>
                <div class="col-md-3 mt-3">
                </div>
                <div class="col-md-4 mt-3" style="text-align:right;">
                  <p class="text-uppercase text-sm" id="anio"></p>
                </div>
              </div>
              <div class="row mt-4">
                  <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <div class="row">
                            <div class="col-3">
                                <p class="text-uppercase text-sm"><b>Clave:</b></p>
                            </div>
                            <div class="col-8">
                                <p class="text-uppercase text-sm" id="clave"></p>
                            </div> 
                        </div>
                    </div>
                    <div class="col-md-2">
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                         <div class="row">
                            <div class="col-3">
                                <p class="text-uppercase text-sm"><b>Titular:</b></p>
                            </div>
                            <div class="col-8">
                                <p class="text-uppercase text-sm" id="titular"></p>
                            </div> 
                        </div>
                    </div>
                    <div class="col-md-2">
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-8">
                        <div class="row" id="firmantes">
                            
                        </div>
                    </div>
                    <div class="col-md-2">
                    </div>
                  </div>    
              </div>
            </div>
          </div>
        </div>

      </div>
        
        <%--Modal Para Modificar la Configuración--%>

         <div class="modal fade" id="modal-form" tabindex="-1" role="dialog" aria-labelledby="modal-form" aria-hidden="true" style="zoom:100%;">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: var(--color-guinda);">
                        <h5 class="modal-title" id="exampleModalLabel" style="color:white;">Modificar Configuración</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                        </button>
                    </div>
                    <div class="modal-body p-1">

                            <div class="table-responsive p-0 mt-4">
                                <table class="table table-striped align-items-center mb-0">
                                    <thead>
                                    <tr>
                                        <th class="text-uppercase text-xxs font-weight-bolder " style="background-color: var(--color-vino); color: white; text-align:center">DATOS DE LA DEPENDENCIA</th>
                                    </tr>
                                    </thead>
                                </table>
                            </div>

                            <div class="table-responsive p-0">
                                <table class="table table-striped align-items-center mb-0">
                                    <thead>
                                    <tr>
                                        <th class="text-center text-uppercase text-xxs font-weight-bolder " style="background-color: var(--color-guinda); color: white;">No.</th>
                                        <th class="text-uppercase text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Nombre de la Dependencia</th>
                                        <th class="text-uppercase  text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Clave</th>
                                        <th class="text-uppercase  text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Titular</th>
                                        <th class="text-center text-uppercase  text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Año</th>
                                        <th class="text-center text-uppercase  text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Opciones</th>
                                    </tr>
                                    </thead>
                                    <tbody id="contenidoconf">
                                         
                                    </tbody>
                                </table>
                            </div>
                             <div class="row mt-2">
                                    <div class="modal-footer" id="gconfig">                                                                        
                                    </div>
                                </div>

                         <div class="table-responsive p-0 mt-4">
                                <table class="table table-striped align-items-center mb-0">
                                    <thead>
                                    <tr>
                                        <th class="text-uppercase text-xxs font-weight-bolder " style="background-color: var(--color-vino); color: white; text-align:center">FIRMANTES</th>
                                    </tr>
                                    </thead>
                                </table>
                            </div>

                            <div class="table-responsive p-0">
                                <table class="table table-striped align-items-center mb-0">
                                    <thead>
                                    <tr>
                                        <th class="text-center text-uppercase text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">No.</th>
                                        <th class="text-uppercase text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Nombre</th>
                                        <th class="text-uppercase text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Cargo</th>
                                        <th class="text-center text-uppercase text-xxs font-weight-bolder" style="background-color: var(--color-guinda); color: white;">Opciones</th>
                                    </tr>
                                    </thead>
                                    <tbody id="firmantesconf">
                                         
                                    </tbody>
                                </table>                            
                            </div>
                            <div class="row mt-2">
                                <div class="modal-footer" id="gfirm">                                                                        
                                </div>
                            </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
  </div>

  <!--   Core JS Files   -->
  <script src="assets/js/core/popper.min.js"></script>
  <script src="assets/js/core/bootstrap.min.js"></script>
  <script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
  <script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
  <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
  <script src="assets/js/ConfiguracionGeneral.js"></script>
  <script src="assets/js/EliminarSesion.js"></script>
  <script>
      addConfiguracion();

    var win = navigator.platform.indexOf('Win') > -1;
    if (win && document.querySelector('#sidenav-scrollbar')) {
      var options = {
        damping: '0.5'
      }
      Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
    }
  </script>
  <!-- Github buttons -->
  <script async defer src="https://buttons.github.io/buttons.js"></script>
  <!-- Control Center for Soft Dashboard: parallax effects, scripts for the example pages etc -->
  <script src="assets/js/argon-dashboard.min.js?v=2.0.4"></script>
</body>

</html>
