<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Prueba.aspx.cs" Inherits="SIAP.Prueba" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
<link rel="apple-touch-icon" sizes="76x76" href="assets/img/apple-icon.png" />
<link rel="icon" type="image/png" href="assets/img/favicon.png" />
    <title>PRUEBAS | SIAP</title>
<%--Elemento necesario * migrate corrige compatibilidad con vers. anteriores --%>

<link href="assets/css/googleapis.css" rel="stylesheet" />
<!-- Nucleo Icons *iconos-->
<link href="assets/css/nucleo-icons.css" rel="stylesheet" />
<link href="assets/css/nucleo-svg.css" rel="stylesheet" />
<link href="assets/css/style.css" rel="stylesheet" />
<script src="assets/js/42d5adcbca.js" crossorigin="anonymous"></script>
<!-- CSS Files principal -->
<link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
<!-- Mensajes de Alerta -->
<link href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" rel="stylesheet" />
<script src="assets/js/SweetAlert2/popper/popper.min.js"></script>
<!-- Elementos de Autocomplete -->
<script src="assets/js/jquery-ui-1.8.custom.min.js"></script>
<link href="assets/css/jquery-ui-1.10.1.custom.min.css" rel="stylesheet" />
<!-- Manejo de fechas y horas sencillas -->
<script src="assets/js/moment.min.js"></script>
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
</head>
<body class="g-sidenav-show bg-gray-100" style="background-color: #e9ecef !important;" >
 <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl  mt-3 mx-3 bg-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="javascript:;">Menú</a></li>
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
            <li class="nav-item dropdown pe-2 d-flex align-items-center">
              <a href="javascript:;" class="nav-link text-white p-0" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                <i class="ni ni-settings-gear-65 cursor-pointer"></i> Opciones
              </a>
              <ul class="dropdown-menu  dropdown-menu-end  px-2 py-3 me-sm-n4" aria-labelledby="dropdownMenuButton">
                <li class="mb-2">
                  <a class="dropdown-item border-radius-md" href="javascript:;" onclick="eliminarsesion()">
                    <div class="d-flex py-1">
                      <div class="d-flex flex-column justify-content-center">
                        <h6 class="text-sm font-weight-normal mb-1">
                          <span class="font-weight-bold">Cerrar Sesión</span> 
                        </h6>
                      </div>
                    </div>
                  </a>
                </li>
                <li class="mb-2">
                  <a class="dropdown-item border-radius-md" href="Configuracion.aspx">
                    <div class="d-flex py-1">
                      <div class="d-flex flex-column justify-content-center">
                        <h6 class="text-sm font-weight-normal mb-1">
                          <span class="font-weight-bold">Configuración</span>
                        </h6>
                      </div>
                    </div>
                  </a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
 </nav>
<div class="container">
    <div class="row">
        <div class="col-md-12">
            <div class="card">
            <div class="card-header pb-0">
                <div class="d-flex align-items-center">
                <p class="mb-0">Edit Profile</p>
                <button class="btn btn-primary btn-sm ms-auto">Settings</button>
                </div>
            </div>
            <div class="card-body">
                <p class="text-uppercase text-sm">User Information</p>
                <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Username</label>
                    <input class="form-control" type="text" value="lucky.jesse">
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Email address</label>
                    <input class="form-control" type="email" value="jesse@example.com">
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">First name</label>
                    <input class="form-control" type="text" value="Jesse">
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Last name</label>
                    <input class="form-control" type="text" value="Lucky">
                    </div>
                </div>
                </div>
                <hr class="horizontal dark">
                <p class="text-uppercase text-sm">Contact Information</p>
                <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Address</label>
                    <input class="form-control" type="text" value="Bld Mihail Kogalniceanu, nr. 8 Bl 1, Sc 1, Ap 09">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">City</label>
                    <input class="form-control" type="text" value="New York">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Country</label>
                    <input class="form-control" type="text" value="United States">
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">Postal code</label>
                    <input class="form-control" type="text" value="437300">
                    </div>
                </div>
                </div>
                <hr class="horizontal dark">
                <p class="text-uppercase text-sm">About me</p>
                <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                    <label for="example-text-input" class="form-control-label">About me</label>
                    <input class="form-control" type="text" value="A beautiful Dashboard for Bootstrap 5. It is Free and Open Source.">
                    </div>
                </div>
                </div>
                <form>
                    <div class="row">
                      <div class="col-md-6">
                        <div class="form-group">
                          <input type="email" class="form-control" id="exampleFormControlInput1" placeholder="name@example.com">
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="form-group">
                          <input type="text" placeholder="Regular" class="form-control" disabled />
                        </div>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-md-6">
                        <div class="form-group">
                          <div class="input-group mb-4">
                            <span class="input-group-text"><i class="ni ni-zoom-split-in"></i></span>
                            <input class="form-control" placeholder="Search" type="text">
                          </div>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="form-group">
                          <div class="input-group mb-4">
                            <input class="form-control" placeholder="Birthday" type="text">
                            <span class="input-group-text"><i class="ni ni-zoom-split-in"></i></span>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div class="row">
                      <div class="col-md-6">
                        <div class="form-group has-success">
                          <input type="text" placeholder="Success" class="form-control is-valid" />
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="form-group has-danger">
                          <input type="email" placeholder="Error Input" class="form-control is-invalid" />
                        </div>
                      </div>
                    </div>
                </form>
            </div>
            </div>
        </div>
    </div>
    <div class="row mt-4">
        <h3>Modal Ejemplos</h3>
    </div>
    <div class="row">
        <div class="col-sm-3">
            <button type="button" class="btn bg-gradient-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
              Launch demo modal
            </button>
            <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
              <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                  </div>
                  <div class="modal-body">
                    ...
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn bg-gradient-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn bg-gradient-primary">Save changes</button>
                  </div>
                </div>
              </div>
            </div>
        </div>
        <div class="col-sm-3">
            <button type="button" class="btn btn-block bg-gradient-primary mb-3" data-bs-toggle="modal" data-bs-target="#modal-default">Default</button>
            <div class="modal fade" id="modal-default" tabindex="-1" role="dialog" aria-labelledby="modal-default" aria-hidden="true">
                <div class="modal-dialog modal- modal-dialog-centered modal-" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                    <h6 class="modal-title" id="modal-title-default">Type your modal title</h6>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                    </div>
                    <div class="modal-body">
                    <p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean.</p>
                    <p>A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
                    </div>
                    <div class="modal-footer">
                    <button type="button" class="btn bg-gradient-primary">Save changes</button>
                    <button type="button" class="btn btn-link  ml-auto" data-bs-dismiss="modal">Close</button>
                    </div>
                </div>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <button type="button" class="btn btn-block bg-gradient-warning mb-3" data-bs-toggle="modal" data-bs-target="#modal-notification">Notification</button>
            <div class="modal fade" id="modal-notification" tabindex="-1" role="dialog" aria-labelledby="modal-notification" aria-hidden="true">
              <div class="modal-dialog modal-danger modal-dialog-centered modal-" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h6 class="modal-title" id="modal-title-notification">Your attention is required</h6>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">×</span>
                    </button>
                  </div>
                  <div class="modal-body">
                    <div class="py-3 text-center">
                      <i class="ni ni-bell-55 ni-3x"></i>
                      <h4 class="text-gradient text-danger mt-4">You should read this!</h4>
                      <p>A small river named Duden flows by their place and supplies it with the necessary regelialia.</p>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-white">Ok, Got it</button>
                    <button type="button" class="btn btn-link text-white ml-auto" data-bs-dismiss="modal">Close</button>
                  </div>
                </div>
              </div>
            </div>
        </div>
        <div class="col-sm-3">
            <button type="button" class="btn btn-block btn-default mb-3" data-bs-toggle="modal" data-bs-target="#modal-form">Form</button>
            <div class="modal fade" id="modal-form" tabindex="-1" role="dialog" aria-labelledby="modal-form" aria-hidden="true">
              <div class="modal-dialog modal-dialog-centered modal-md" role="document">
                <div class="modal-content">
                  <div class="modal-body p-0">
                    <div class="card card-plain">
                      <div class="card-header pb-0 text-left">
                        <h3 class="font-weight-bolder text-info text-gradient">Welcome back</h3>
                        <p class="mb-0">Enter your email and password to sign in</p>
                      </div>
                      <div class="card-body">
                        <form role="form text-left">
                          <label>Email</label>
                          <div class="input-group mb-3">
                            <input type="email" class="form-control" placeholder="Email" aria-label="Email" aria-describedby="email-addon">
                          </div>
                          <label>Password</label>
                          <div class="input-group mb-3">
                            <input type="email" class="form-control" placeholder="Password" aria-label="Password" aria-describedby="password-addon">
                          </div>
                          <div class="form-check form-switch">
                            <input class="form-check-input" type="checkbox" id="rememberMe" checked="">
                            <label class="form-check-label" for="rememberMe">Remember me</label>
                          </div>
                          <div class="text-center">
                            <button type="button" class="btn btn-round bg-gradient-info btn-lg w-100 mt-4 mb-0">Sign in</button>
                          </div>
                        </form>
                      </div>
                      <div class="card-footer text-center pt-0 px-lg-2 px-1">
                        <p class="mb-4 text-sm mx-auto">
                          Don't have an account?
                          <a href="javascript:;" class="text-info text-gradient font-weight-bold">Sign up</a>
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        </div>
     </div>
    <div class="row">
          <div class="col-md-4">
            <!-- Button trigger modal -->
            <button type="button" class="btn bg-gradient-danger btn-block mb-3" data-bs-toggle="modal" data-bs-target="#exampleModalLong">
              Long Modal
            </button>
            <!-- Modal -->
            <div class="modal fade" id="exampleModalLong" tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
              <div class="modal-dialog" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Modal title</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">&times;</span>
                    </button>
                  </div>
                  <div class="modal-body">
                    I always felt like I could do anything. That’s the main thing people are controlled by! Thoughts- their perception of themselves! They're slowed down by their perception of themselves. If you're taught you can’t do anything, you won’t do anything. I was taught I could do everything.
                    <br/><br/>
                    As we live, our hearts turn colder. Cause pain is what we go through as we become older. We get insulted by others, lose trust for those others. We get back stabbed by friends. It becomes harder for us to give others a hand. We get our heart broken by people we love, even that we give them all we have. Then we lose family over time. What else could rust the heart more over time? Blackgold.
                    <br/><br/>


                    We’re not always in the position that we want to be at. We’re constantly growing. We’re constantly making mistakes. We’re constantly trying to express ourselves and actualize our dreams. If you have the opportunity to play this game of life you need to appreciate every moment. A lot of people don’t appreciate the moment until it’s passed.
                    <br/><br/>


                    There’s nothing I really wanted to do in life that I wasn’t able to get good at. That’s my skill. I’m not really specifically talented at anything except for the ability to learn. That’s what I do. That’s what I’m here for. Don’t be afraid to be wrong because you can’t learn anything from a compliment.
                    <br/><br/>

                    It really matters and then like it really doesn’t matter. What matters is the people who are sparked by it. And the people who are like offended by it, it doesn’t matter. Because it's about motivating the doers. Because I’m here to follow my dreams and inspire other people to follow their dreams, too.
                    <br/><br/>

                    The time is now for it to be okay to be great. People in this world shun people for being great. For being a bright color. For standing out. But the time is now to be okay to be the greatest you. Would you believe in what you believe in, if you were the only one who believed it?
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn bg-gradient-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn bg-gradient-primary">Save changes</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <!-- Button trigger modal -->
            <button type="button" class="btn bg-gradient-success btn-block mb-3" data-bs-toggle="modal" data-bs-target="#exampleModalMessage">
              Message Modal
            </button>
            <!-- Modal -->
            <div class="modal fade" id="exampleModalMessage" tabindex="-1" role="dialog" aria-labelledby="exampleModalMessageTitle" aria-hidden="true">
              <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                  <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">New message to @CT</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
                      <span aria-hidden="true">×</span>
                    </button>
                  </div>
                  <div class="modal-body">
                    <form>
                      <div class="form-group">
                        <label for="recipient-name" class="col-form-label">Recipient:</label>
                        <input type="text" class="form-control" value="Creative Tim" id="recipient-name">
                      </div>
                      <div class="form-group">
                        <label for="message-text" class="col-form-label">Message:</label>
                        <textarea class="form-control" id="message-text"></textarea>
                      </div>
                    </form>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn bg-gradient-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn bg-gradient-primary">Send message</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <!-- Button trigger modal -->
            <button type="button" class="btn bg-gradient-info btn-block" data-bs-toggle="modal" data-bs-target="#exampleModalSignUp">
              SignUp Modal
            </button>
            <!-- Modal -->
            <div class="modal fade" id="exampleModalSignUp" tabindex="-1" role="dialog" aria-labelledby="exampleModalSignTitle" aria-hidden="true">
              <div class="modal-dialog modal-dialog-centered modal-md" role="document">
                <div class="modal-content">
                  <div class="modal-body p-0">
                    <div class="card card-plain">
                      <div class="card-header pb-0 text-left">
                          <h3 class="font-weight-bolder text-primary text-gradient">Join us today</h3>
                          <p class="mb-0">Enter your email and password to register</p>
                      </div>
                      <div class="card-body pb-3">
                        <form role="form text-left">
                          <label>Name</label>
                          <div class="input-group mb-3">
                            <input type="text" class="form-control" placeholder="Name" aria-label="Name" aria-describedby="name-addon">
                          </div>
                          <label>Email</label>
                          <div class="input-group mb-3">
                            <input type="email" class="form-control" placeholder="Email" aria-label="Email" aria-describedby="email-addon">
                          </div>
                          <label>Password</label>
                          <div class="input-group mb-3">
                            <input type="password" class="form-control" placeholder="Password" aria-label="Password" aria-describedby="password-addon">
                          </div>
                          <div class="form-check form-check-info text-left">
                            <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault" checked="">
                            <label class="form-check-label" for="flexCheckDefault">
                              I agree the <a href="javascrpt:;" class="text-dark font-weight-bolder">Terms and Conditions</a>
                            </label>
                          </div>
                          <div class="text-center">
                            <button type="button" class="btn bg-gradient-primary btn-lg btn-rounded w-100 mt-4 mb-0">Sign up</button>
                          </div>
                        </form>
                      </div>
                      <div class="card-footer text-center pt-0 px-sm-4 px-1">
                        <p class="mb-4 mx-auto">
                          Already have an account?
                          <a href="javascrpt:;" class="text-primary text-gradient font-weight-bold">Sign in</a>
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
     </div>
     <div class="Row">
       <div class="col-sm-2">
          <h3>select2 autocomplete</h3>
          <div class="form-group">
            <select class="js-example-basic-single form-control" name="state">
              <option value="AL">Alabama</option>
              <option value="WY">Wyoming</option>
            </select>
          </div>
       </div> 
          
     </div>
</div>
<script src="assets/js/jquery-3.5.1.js"></script>
<script src="assets/js/jquery-migrate-3.0.0.js"></script>
<script src="assets/js/core/popper.min.js"></script>
<script src="assets/js/core/bootstrap.min.js"></script>
<%--<script src="assets/js/sweetalert.min.js"></script>--%>
<script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
<script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
<script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
<script src='https://cdn.jsdelivr.net/npm/vanillajs-datepicker@1.1.4/dist/js/datepicker-full.min.js'></script>
<script async defer src="https://buttons.github.io/buttons.js"></script>
<script src="assets/js/argon-dashboard.min.js?v=2.0.0"></script>
<%--Elementos para el Datatable--%>
<script src="assets/js/jquery.dataTables.min.js"></script>
<%--Elementos para el Calendario--%>
<script src="assets/js/bootstrap-datepicker.min.js"></script>
<script src="assets/js/flatpickr.min.js"></script>
<script src="assets/js/dataTables.bootstrap5.min.js"></script> 
<%--Elementos para el Select--%>
<script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>

<script>
    var win = navigator.platform.indexOf('Win') > -1;
    if (win && document.querySelector('#sidenav-scrollbar')) {
        var options = {
            damping: '0.5'
        }
        Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
    }
</script>
<%--<script>
    /* Bootstrap 5 JS included */
    /* vanillajs-datepicker 1.1.4 JS included */
    const getDatePickerTitle = elem => {
        // From the label or the aria-label
        const label = elem.nextElementSibling;
        let titleText = '';
        if (label && label.tagName === 'LABEL') {
            titleText = label.textContent;
        } else {
            titleText = elem.getAttribute('aria-label') || '';
        }
        return titleText;
    }
    const elems = document.querySelectorAll('.datepicker_input');
    for (const elem of elems)
    {
        const datepicker = new Datepicker(elem, {
            'format': 'dd/mm/yyyy', // UK format
            title: getDatePickerTitle(elem)
        });
    }
</script>
<script type="text/javascript" class="init">

      $(document).ready(function () {
          $('#example').DataTable();
      });

</script>
<script type="text/javascript">
    $(function () {
        $("#datepicker").datepicker();
    });
</script>--%>
<script type="text/javascript">
    $(document).ready(function () {
        $('.js-example-basic-single').select2();
    });
</script>    
</body>
</html>
