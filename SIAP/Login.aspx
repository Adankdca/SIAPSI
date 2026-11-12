<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SIAP.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
  <link rel="apple-touch-icon" sizes="76x76" href="assets/img/apple-icon.png" />
  <link rel="icon" type="image/png" href="assets/img/favicon.png" />
  <title>
    Iniciar Sesión | SIAP
  </title>
  <!--     Fonts and icons     -->
  <script src="assets/js/SweetAlert2/jquery/jquery-3.3.1.min.js"></script>
  <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
  <!-- Nucleo Icons -->
  <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- Font Awesome Icons -->
  <script src="https://kit.fontawesome.com/42d5adcbca.js" crossorigin="anonymous"></script>
  <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
  <!-- CSS Files -->
  <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
  <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
  <%--<link rel="stylesheet" href="assets/js/SweetAlert2/bootstrap4/css/bootstrap.min.css" />--%>
  <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" />
  <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/animate.css/animate.css" />
  <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
     <script>
         var data = sessionStorage.getItem('name');
         if (data != null) {
             window.location.href = "Menu.aspx";
         }
     </script>
      <style>
        .form-control-lg { 
            border-radius: 0.75rem; 
            transition: all 0.2s; 
            background-color: #F8F7F4 !important; /* Input no blanco total */
            border: 1px solid #EBE9E1;
        }
        .form-control-lg:focus { 
            box-shadow: 0 0 0 3px rgba(159, 34, 65, 0.2); 
            border-color: var(--color-vino); 
            background-color: #FFFFFF !important;
        }
        .card-plain { 
            background-color: #FFFFFF !important; /* Card blanca elevada */
            border-radius: 1rem; 
            box-shadow: 0 20px 40px rgba(0,0,0,0.08), 0 5px 15px rgba(0,0,0,0.05) !important; 
            padding: 2.5rem 2rem; 
            border: none !important; 
        }
        .card-header { background: transparent; }
      </style>
</head>
<body class="">
  
  <div class="container position-sticky z-index-sticky top-0">
    <div class="row">
      <div class="col-12">
        <!-- Navbar -->
        <!-- End Navbar -->
      </div>
    </div>
  </div>
  <main class="main-content  mt-0">
    <section>
      <div class="page-header min-vh-100">
        <div class="container">
          <div class="row">
            <div class="col-xl-4 col-lg-5 col-md-7 d-flex flex-column mx-lg-0 mx-auto">
              <div class="card card-plain">
                <div class="card-header pb-0 text-start">
                  <h4 class="font-weight-bolder">Bienvenido</h4>
                  <p class="mb-0">Ingrese su usuario y contrase&ntilde;a para continuar</p>
                </div>
                <div class="card-body">
                  <form role="form" runat="server">
                        <asp:ScriptManager ID="login" AsyncPostBackTimeout="36000000" runat="server"></asp:ScriptManager>
                    <div class="mb-3">
                      <%--<input type="email" class="form-control form-control-lg" placeholder="Usuario" aria-label="Email"/>--%>
                        <asp:TextBox runat="server" ID="TxtUsuario" placeholder="Usuario" CssClass="form-control form-control-lg"></asp:TextBox> 
                    </div>
                    <div class="mb-3">
                     <%-- <input type="password" class="form-control form-control-lg" placeholder="Contraseña" aria-label="Password" />--%>
                        <asp:TextBox runat="server" ID="TextContrasenia" placeholder="Contrase&ntilde;a" CssClass="form-control form-control-lg" TextMode="Password"></asp:TextBox>
                    </div>
                    <%--<div class="form-check form-switch">
                      <input class="form-check-input" type="checkbox" id="rememberMe" />
                        <asp:CheckBox runat="server" ID="remember" CssClass="" />
                      <label class="form-check-label" for="rememberMe">Recuérdame</label>
                    </div>--%>
                    <div class="text-center">
                      <%--<button type="button" class="btn btn-lg btn-primary btn-lg w-100 mt-4 mb-0">Continuar</button>--%>
                      <asp:Button runat="server" ID="iniciar" Text="Continuar"  CssClass="btn btn-lg btn-primary btn-lg w-100 mt-4 mb-0" OnClick="BtnLoguear_Click"/>
                    </div>
                  </form>
                </div>
                <div class="card-footer text-center pt-0 px-lg-2 px-1">
                </div>
              </div>
            </div>
            <div class="col-6 d-lg-flex d-none h-100 my-auto pe-0 position-absolute top-0 end-0 text-center justify-content-center flex-column">
              <div class="position-relative bg-gradient-primary h-100 m-3 px-7 border-radius-lg d-flex flex-column justify-content-center overflow-hidden" style="background-image: url('assets/img/Imagen.png'); background-size: cover; background-position: center;">
                <span class="mask bg-gradient-primary opacity-8"></span>
                
                <!-- Bootstrap Carousel -->
                <div id="loginCarousel" class="carousel slide z-index-2" data-bs-ride="carousel" data-bs-interval="3000">
                  <div class="carousel-inner">
                    <div class="carousel-item active">
                      <img src="assets/img/LogoSIAP.png" class="mb-4" style="max-width: 250px;" alt="Logo SIAP" />
                      <h4 class="text-white font-weight-bolder">Sistema Integral de Administraci&oacute;n Presupuestal</h4>
                      <p class="text-white">Transparencia, Control y Eficiencia en la Gesti&oacute;n de Recursos.</p>
                    </div>
                    <div class="carousel-item">
                      <div class="d-flex align-items-center justify-content-center mb-4" style="height: 120px;">
                          <i class="fas fa-chart-pie text-white" style="font-size: 5rem;"></i>
                      </div>
                      <h4 class="text-white font-weight-bolder">Toma de Decisiones Inteligente</h4>
                      <p class="text-white">An&aacute;lisis de datos en tiempo real para una planeaci&oacute;n estrat&eacute;gica.</p>
                    </div>
                    <div class="carousel-item">
                      <div class="d-flex align-items-center justify-content-center mb-4" style="height: 120px;">
                          <i class="fas fa-shield-alt text-white" style="font-size: 5rem;"></i>
                      </div>
                      <h4 class="text-white font-weight-bolder">Seguridad Institucional</h4>
                      <p class="text-white">Tus datos protegidos bajo los más altos estándares de seguridad.</p>
                    </div>
                  </div>
                  
                  <!-- Carousel Indicators -->
                  <div class="carousel-indicators position-relative mt-4">
                    <button type="button" data-bs-target="#loginCarousel" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                    <button type="button" data-bs-target="#loginCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
                    <button type="button" data-bs-target="#loginCarousel" data-bs-slide-to="2" aria-label="Slide 3"></button>
                  </div>
                </div>

              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
  <!--   Core JS Files   -->

      <script src="assets/js/core/popper.min.js"></script>
      <script src="assets/js/SweetAlert2/bootstrap4/js/bootstrap.min.js"></script>
<%--  <script src="assets/js/core/bootstrap.min.js"></script>--%>
  <%--<script src="assets/js/SweetAlert2/popper/popper.min.js"></script>--%>
  <script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
  <script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
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
  <script async defer src="https://buttons.github.io/buttons.js"></script>
  <!-- Control Center for Soft Dashboard: parallax effects, scripts for the example pages etc -->
  <%--<script src="assets/js/argon-dashboard.min.js?v=2.0.0"></script>--%>
  <%--<script src="assets/js/SweetAlert2/jquery/alertas.js"></script>--%>
</body>
</html>
