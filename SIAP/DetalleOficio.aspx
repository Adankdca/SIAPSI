<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Detalle de Oficio | SIAP</title>
    <!-- Fonts and icons -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet" />
    <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
    <link href="assets/css/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/dataTables.bootstrap5.min.css" />
    
    <style>
        .glass-card { background: white; border-radius: 1rem; box-shadow: 0 10px 30px rgba(0,0,0,0.05); padding: 1rem; border: 1px solid rgba(0,0,0,0.05); }
        table.dataTable { border-collapse: collapse !important; }
    </style>

    <script src="assets/js/jquery-3.5.1.js"></script>
</head>
<body class="g-sidenav-show">
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Adecuaciones.aspx">Adecuaciones</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Detalle de Oficio</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0" id="lblOficioPrincipal">Resumen de Oficio</h6>
        </nav>
        <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
          <h3 style="color:white; margin:0 auto;">Secretaría de Infraestructura</h3>
          <div class="ms-md-auto pe-md-3 d-flex align-items-center">
            <button class="btn btn-outline-white btn-sm mb-0 shadow-sm me-2" id="btnViewPDF" style="display:none;">
                <i class="fas fa-file-pdf me-2"></i> Visualizar PDF
            </button>
            <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                <i class="fas fa-arrow-left me-2"></i> Regresar
            </a>
          </div>
        </div>
      </div>
    </nav>

    <main class="main-content mt-4">
        <div class="container-fluid py-4">

            <!-- KPI Cards Row -->
            <div class="row mb-4">
                <!-- Card 1: No. Solicitud -->
                <div class="col-xl-4 col-sm-6 mb-xl-0 mb-4">
                    <div class="card h-100 shadow-sm border-0" style="border-radius: 1rem;">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold" style="color: var(--color-dorado);">Oficio Seleccionado</p>
                                        <h4 class="font-weight-bolder text-dark mb-0" id="docOficio">
                                            ---
                                        </h4>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-warning shadow text-center rounded-circle">
                                        <i class="fa-solid fa-file-invoice text-lg opacity-10" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Card 2: Fecha y Estatus -->
                <div class="col-xl-4 col-sm-6 mb-xl-0 mb-4">
                    <div class="card h-100 shadow-sm border-0" style="border-radius: 1rem;">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold text-secondary">Fecha y Estatus</p>
                                        <h5 class="font-weight-bolder text-dark mb-1" id="docFecha">
                                            ---
                                        </h5>
                                        <span class="badge bg-gradient-secondary" id="docEstatus">---</span>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-primary shadow text-center rounded-circle">
                                        <i class="fa-solid fa-calendar-day text-lg opacity-10" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Card 3: Total -->
                <div class="col-xl-4 col-sm-6 mb-xl-0 mb-4">
                    <div class="card h-100 shadow-sm border-0" style="border-radius: 1rem;">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold" style="color: var(--color-verde);">Total Adecuación</p>
                                        <h4 class="font-weight-bolder mb-0 text-success" id="docTotal">
                                            ---
                                        </h4>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-success shadow text-center rounded-circle">
                                        <i class="fa-solid fa-money-bill-wave text-lg opacity-10" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Card Row for Elaboró / Autorizó -->
            <div class="row mb-4">
                <div class="col-12">
                    <div class="card shadow-sm border-0" style="border-radius: 1rem;">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-md-6 text-center">
                                    <p class="text-xs mb-1 text-uppercase font-weight-bold text-secondary">Solicita</p>
                                    <h6 class="font-weight-bolder text-dark mb-0" id="docSolicita">---</h6>
                                </div>
                                <div class="col-md-6 text-center border-start">
                                    <p class="text-xs mb-1 text-uppercase font-weight-bold text-secondary">Autoriza</p>
                                    <h6 class="font-weight-bolder text-dark mb-0" id="docAutoriza">---</h6>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Table Section -->
            <div class="row">
                <div class="col-12">
                    <div class="card shadow-sm border-0" style="border-radius: 1rem;">
                        <div class="card-header pb-0 bg-transparent border-0 px-4 pt-4">
                            <h6 class="text-dark font-weight-bolder mb-0">Desglose de Movimientos Presupuestales</h6>
                        </div>
                        <div class="card-body px-4 pt-2 pb-4">
                            <div class="table-responsive">
                                <table class="table align-items-center mb-0 w-100 table-hover" id="tablaMovimientosOficio">
                                    <thead>
                                        <tr>
                                            <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7" style="width: 5%">No.</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2" style="width: 45%">Clave Presupuestaria</th>
                                            <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7" style="width: 15%">Tipo</th>
                                            <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7" style="width: 15%">Monto Afectado</th>
                                            <th class="text-center text-uppercase text-secondary text-xxs font-weight-bolder opacity-7" style="width: 15%">Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody id="displayDetalle">
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td colspan="3" class="text-end text-sm text-secondary font-weight-bolder">TOTAL GENERAL:</td>
                                            <td class="text-center text-sm text-dark font-weight-bold" id="sumMonto">$0.00</td>
                                            <td></td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.4/js/dataTables.bootstrap5.min.js"></script>
    <script src="assets/js/DetalleOficio.js?v=2.2"></script>
</body>
</html>
