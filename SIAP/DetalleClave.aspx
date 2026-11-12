<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
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
        .chart-summary-wrapper {
            display: grid;
            grid-template-columns: 2fr 1fr;
            gap: 20px;
            background: white;
            border-radius: 12px;
            padding: 20px;
            border: 1px solid #c8ccd0;
            box-shadow: 0 2px 10px rgba(0,0,0,0.04);
        }
        .chart-container {
            position: relative;
            min-height: 250px;
        }
        .summary-side {
            display: flex;
            flex-direction: column;
            justify-content: center;
            gap: 12px;
            padding-left: 20px;
            border-left: 2px solid #f0f0f0;
        }
        .summary-item {
            display: flex;
            align-items: center;
            gap: 12px;
        }
        .summary-dot {
            width: 12px;
            height: 12px;
            border-radius: 50%;
            flex-shrink: 0;
        }
        .summary-item .summary-label {
            font-size: 0.8rem;
            color: #666;
            flex: 1;
        }
        .summary-item .summary-value {
            font-weight: 700;
            font-size: 0.9rem;
        }
        @media (max-width: 768px) {
            .chart-summary-wrapper {
                grid-template-columns: 1fr;
            }
            .summary-side {
                padding-left: 0;
                border-left: none;
                border-top: 2px solid #f0f0f0;
                padding-top: 20px;
            }
        }
    </style>
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
        <div class="container-fluid py-4">
            
            <div id="alertApartado" class="alert alert-warning text-white d-none" role="alert" style="background-color: #f6c23e; border-color: #f6c23e;">
                <i class="fas fa-exclamation-triangle me-2"></i> 
                <strong>Aviso:</strong> Existen apartados en curso. Si no se registra una adecuación en 7 días, el saldo volverá a estar disponible.
            </div>

            <!-- KPIs Row -->
            <div class="row mb-4">
                <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                    <div class="card glass-card">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold">Aprobado</p>
                                        <h5 class="font-weight-bolder" style="color: var(--color-vino);" id="kpiAprobado">
                                            $0.00
                                        </h5>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-primary shadow-primary text-center rounded-circle">
                                        <i class="fa-solid fa-coins text-lg opacity-10 text-white" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                    <div class="card glass-card">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold">Modificado</p>
                                        <h5 class="font-weight-bolder" style="color: var(--color-vino);" id="kpiModificado">
                                            $0.00
                                        </h5>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-primary shadow-primary text-center rounded-circle">
                                        <i class="fa-solid fa-chart-line text-lg opacity-10 text-white" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                    <div class="card glass-card">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold">Devengado</p>
                                        <h5 class="font-weight-bolder" style="color: var(--color-vino);" id="kpiDevengado">
                                            $0.00
                                        </h5>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-primary shadow-primary text-center rounded-circle">
                                        <i class="fa-solid fa-file-invoice-dollar text-lg opacity-10 text-white" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xl-3 col-sm-6">
                    <div class="card glass-card">
                        <div class="card-body p-3">
                            <div class="row">
                                <div class="col-8">
                                    <div class="numbers">
                                        <p class="text-sm mb-0 text-uppercase font-weight-bold">Pagado</p>
                                        <h5 class="font-weight-bolder" style="color: var(--color-vino);" id="kpiPagado">
                                            $0.00
                                        </h5>
                                    </div>
                                </div>
                                <div class="col-4 text-end">
                                    <div class="icon icon-shape bg-gradient-primary shadow-primary text-center rounded-circle">
                                        <i class="fa-solid fa-money-check-dollar text-lg opacity-10 text-white" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Desglose y Calendarizado -->
            <div class="row mb-4">
                <div class="col-lg-6">
                    <div class="card glass-card h-100">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize">Desglose de Clave Presupuestaria</h6>
                        </div>
                        <div class="card-body p-3">
                            <div class="table-responsive">
                                <table class="table align-items-center mb-0 table-sm" id="tablaDesglose">
                                    <thead>
                                        <tr>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Dato</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Abrv</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Clave</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Descripción</th>
                                        </tr>
                                    </thead>
                                    <tbody id="contenidoDesglose">
                                        <!-- Se llenará con JS -->
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6">
                    <!-- Pastel -->
                    <div class="card glass-card mb-4">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize">Estado General de la Obra</h6>
                        </div>
                        <div class="card-body p-3">
                            <div class="chart-summary-wrapper">
                                <div class="chart-container">
                                    <div id="piechart_3d" style="width: 100%; height: 250px;"></div>
                                </div>
                                <div class="summary-side">
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #9F2241;"></span>
                                        <span class="summary-label">Gastado</span>
                                        <span class="summary-value" id="legGastado" style="color: #9F2241;">$0.00</span>
                                    </div>
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #235B4E;"></span>
                                        <span class="summary-label">Saldo por Gastar</span>
                                        <span class="summary-value" id="legSaldo" style="color: #235B4E;">$0.00</span>
                                    </div>
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #FF8C00;"></span>
                                        <span class="summary-label">Apartado</span>
                                        <span class="summary-value" id="legApartado" style="color: #FF8C00;">$0.00</span>
                                    </div>
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #6c757d;"></span>
                                        <span class="summary-label">Por Liberar</span>
                                        <span class="summary-value" id="legLiberar" style="color: #6c757d;">$0.00</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Progress Calendarizado -->
                    <div class="card glass-card">
                        <div class="card-header pb-0 bg-transparent d-flex justify-content-between align-items-center">
                            <h6 class="text-capitalize">Progreso de Calendarizaci&oacute;n</h6>
                            <button id="btnCalendarizar" class="btn btn-primary btn-sm mb-0 d-none" onclick="abrirModalCalendarizar()">Calendarizar Ahora</button>
                        </div>
                        <div class="card-body p-3">
                            <div id="tablaCalendarizado">
                                <!-- Generado por JS -->
                            </div>
                        </div>
                    </div>

                    <!-- Detalles del Apartado (Dinamico) -->
                    <div class="card glass-card mt-4 d-none" id="cardDetalleApartado">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize mb-0"><i class="bi bi-info-circle text-warning me-2"></i>Detalles del Apartado</h6>
                        </div>
                        <div class="card-body p-3" id="detApartadoContainer">
                            <!-- Los apartados se insertarán aquí dinámicamente -->
                        </div>
                    </div>
                </div>
            </div>

            <!-- Obras Relacionadas -->
            <div class="row mb-4">
                <div class="col-lg-12">
                    <div class="card glass-card h-100">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize">Otras Fuentes de Financiamiento para esta Obra</h6>
                        </div>
                        <div class="card-body p-3">
                            <div class="table-responsive">
                                <table class="table align-items-center mb-0 table-sm" id="tablaObrasRelacionadas">
                                    <thead>
                                        <tr>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Clave Presupuestal</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Aprobado</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Modificado</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Devengado</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Acci&oacute;n</th>
                                        </tr>
                                    </thead>
                                    <tbody id="contenidoObrasRelacionadas">
                                        <tr><td colspan="5" class="text-center text-muted">Cargando...</td></tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-lg-6">
                    <div class="card glass-card h-100">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize">Historial de Adecuaciones</h6>
                        </div>
                        <div class="card-body p-3">
                            <div class="table-responsive">
                                <table class="table align-items-center mb-0">
                                    <thead>
                                        <tr>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Oficio / Fecha</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Tipo de Operación</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-end">Monto Total Afectado</th>
                                            <th class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 text-center">Desglose</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbodyHistorialAdec">
                                        <tr><td colspan="4" class="text-center text-muted">Aún no hay movimientos registrados para esta clave.</td></tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6">
                    <div class="card glass-card h-100">
                        <div class="card-header pb-0 bg-transparent">
                            <h6 class="text-capitalize">Historial de Ministraciones</h6>
                        </div>
                        <div class="card-body p-3">
                            <p class="text-sm text-secondary">Próximamente...</p>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </main>

    <!-- Modal Calendarizar -->
    <div class="modal fade" id="modalCalendarizar" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-gradient-primary">
                    <h5 class="modal-title text-white">Calendarizar Presupuesto</h5>
                </div>
                <div class="modal-body">
                    <p class="text-sm">Ingrese el monto a calendarizar por mes. La suma debe coincidir con el monto Aprobado (<b id="lblAprobadoModal"></b>).</p>
                    <div class="row">
                        <script>
                            const mesesArr = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
                            mesesArr.forEach((m, i) => {
                                document.write(`<div class="col-md-3 mb-3">
                                    <label class="form-control-label text-xs">${m}</label>
                                    <input type="number" class="form-control form-control-sm mes-input" id="mes_${i+1}" value="0">
                                </div>`);
                            });
                        </script>
                    </div>
                    <div class="mt-3">
                        <h6 class="text-end">Suma Total: <span id="lblSumaModal" class="text-danger">$0.00</span></h6>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-success" onclick="guardarCalendarizacion()">Guardar Calendarizaci&oacute;n</button>
                </div>
            </div>
        </div>
    </div>

    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-3d.js"></script>
    <script>
        let montoAprobadoGlobal = 0;
        let claveGlobal = "";

        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const claveStr = urlParams.get('clave');
            
            if(claveStr) {
                claveGlobal = claveStr;
                $("#lblClavePrincipal").text("Clave: " + claveStr);
                decodificarClaveAPI(claveStr);
                cargarDatosFinancieros(claveStr);
                cargarObrasRelacionadas(claveStr);
                cargarHistorialAdecuaciones(claveStr);
            } else {
                $("#lblClavePrincipal").text("Error: Clave no proporcionada");
            }

            $('.mes-input').on('input', calcularSumaModal);
        });

        function decodificarClaveAPI(clave) {
            $.ajax({
                url: '/api/presupuesto/decodificar?clave=' + clave,
                method: 'GET',
                success: function(res) {
                    if(res && res.Segmentos) {
                        let html = "";
                        res.Segmentos.forEach(seg => {
                            html += `<tr>
                                <td class="text-sm font-weight-bold">${seg.Nombre}</td>
                                <td class="text-sm text-secondary">${seg.Abreviatura}</td>
                                <td class="text-sm font-weight-bolder text-primary">${seg.Valor}</td>
                                <td class="text-sm" style="white-space: normal; min-width: 200px;">${seg.Descripcion}</td>
                            </tr>`;
                        });
                        $("#contenidoDesglose").html(html);
                    }
                }
            });
        }

        function cargarHistorialAdecuaciones(clave) {
            $.ajax({
                url: 'api/adecuaciones/GetPorClave/' + clave,
                method: 'GET',
                success: function(res) {
                    let html = '';
                    let totalNeto = 0;
                    if (res && res.length > 0) {
                        res.forEach(item => {
                            let f = item.FechaSolicitud;
                            let fecha = "";
                            if (f && f != "0001-01-01T00:00:00") {
                                let fech = f.split('T')[0].split('-');
                                fecha = fech[2] + "-" + fech[1] + "-" + fech[0];
                            }
                            
                            let formattedTotal = "$" + parseFloat(item.Total).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                            let tOp = item.TipoOperacion || 'MOVIMIENTO';
                            
                            let colorClass = (tOp.toUpperCase() === 'AMPLIACION' || tOp.toUpperCase() === 'AMPLIACIÓN') ? 'text-success' : 
                                            (tOp.toUpperCase() === 'REDUCCION' || tOp.toUpperCase() === 'REDUCCIÓN') ? 'text-danger' : 'text-primary';

                            if (tOp.toUpperCase() === 'REDUCCION' || tOp.toUpperCase() === 'REDUCCIÓN') {
                                totalNeto -= parseFloat(item.Total || 0);
                            } else {
                                totalNeto += parseFloat(item.Total || 0);
                            }

                            html += `<tr>
                                <td>
                                    <div class="d-flex flex-column justify-content-center">
                                        <h6 class="mb-0 text-sm">${item.NumOficio || 'Sin Oficio'}</h6>
                                        <p class="text-xs text-secondary mb-0">${fecha}</p>
                                    </div>
                                </td>
                                <td><span class="text-xs font-weight-bold ${colorClass}">${tOp}</span></td>
                                <td class="align-middle text-end text-sm">
                                    <span class="font-weight-bold ${colorClass}">${formattedTotal}</span>
                                </td>
                                <td class="align-middle text-center">
                                    <a href="${item.NumOficio ? 'DetalleOficio.aspx?oficio=' + encodeURIComponent(item.NumOficio) : 'Adecuaciones.aspx'}" class="btn btn-primary btn-sm mb-0" data-toggle="tooltip" data-original-title="Ver Adecuación">
                                        Ir al Oficio
                                    </a>
                                </td>
                            </tr>`;
                        });
                        
                        let formattedNeto = "$" + parseFloat(totalNeto).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                        let colorNeto = totalNeto < 0 ? 'text-danger' : 'text-success';
                        
                        html += `<tr style="background-color: #f8f9fa;">
                            <td colspan="2" class="text-end font-weight-bolder text-dark text-sm py-3" style="border-top: 2px solid #e9ecef;">Monto Neto de Adecuaciones:</td>
                            <td class="align-middle text-end text-sm py-3" style="border-top: 2px solid #e9ecef;">
                                <span class="font-weight-bolder ${colorNeto}" style="font-size: 1.1rem;">${formattedNeto}</span>
                            </td>
                            <td style="border-top: 2px solid #e9ecef;"></td>
                        </tr>`;
                    } else {
                        html = '<tr><td colspan="4" class="text-center text-muted py-4">Aún no hay movimientos registrados para esta clave.</td></tr>';
                    }
                    $("#tbodyHistorialAdec").html(html);
                },
                error: function(err) {
                    $("#tbodyHistorialAdec").html('<tr><td colspan="4" class="text-center text-danger py-4">Error al cargar el historial.</td></tr>');
                }
            });
        }

        function cargarDatosFinancieros(clave) {
            $.ajax({
                url: '/api/presupuesto/detalle?clave=' + clave,
                method: 'GET',
                success: function(res) {
                    if(res) {
                        const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
                        montoAprobadoGlobal = res.Modificado || 0;

                        $("#kpiAprobado").text(formater.format(res.Aprobado || 0));
                        $("#kpiModificado").text(formater.format(res.Modificado || 0));
                        $("#kpiDevengado").text(formater.format(res.Devengado || 0));
                        $("#kpiPagado").text(formater.format(res.Pagado || 0));

                        if (res.SaldoApartado && res.SaldoApartado > 0) {
                            $("#alertApartado").removeClass("d-none");
                            $("#cardDetalleApartado").removeClass("d-none");
                            let apHtml = '';
                            if (res.Apartados && res.Apartados.length > 0) {
                                res.Apartados.forEach(ap => {
                                    let f = new Date(ap.FechaApartado);
                                    f.setDate(f.getDate() + 7);
                                    let vencimiento = f.toLocaleDateString();
                                    
                                    apHtml += `
                                    <div class="row mb-3 pb-3 border-bottom border-light">
                                        <div class="col-md-4 mb-3 mb-md-0">
                                            <p class="text-sm mb-1 text-muted font-weight-bold">Monto Apartado</p>
                                            <h5 class="mb-0 text-warning">${formater.format(ap.Monto)}</h5>
                                        </div>
                                        <div class="col-md-4 mb-3 mb-md-0">
                                            <p class="text-sm mb-1 text-muted font-weight-bold">Oficio / Referencia</p>
                                            <p class="mb-0 font-weight-bold text-dark">${ap.Oficio || 'Sin oficio'}</p>
                                        </div>
                                        <div class="col-md-4">
                                            <p class="text-sm mb-1 text-muted font-weight-bold">Vencimiento</p>
                                            <p class="mb-0 font-weight-bold text-danger">${vencimiento}</p>
                                        </div>
                                        <div class="col-12 mt-3 d-flex justify-content-between align-items-center">
                                            <div>
                                                <p class="text-sm mb-1 text-muted font-weight-bold">Motivo / Justificación</p>
                                                <p class="mb-0 text-sm">${ap.Motivo || 'Sin motivo'}</p>
                                            </div>
                                        </div>
                                    </div>`;
                                });
                            }
                            $("#detApartadoContainer").html(apHtml);
                        } else {
                            $("#alertApartado").addClass("d-none");
                            $("#cardDetalleApartado").addClass("d-none");
                        }

                        renderPieChart(res);
                        renderProgressBar(res);
                    }
                }
            });
        }

        function cargarObrasRelacionadas(clave) {
            if (clave.length < 22) return;
            const prefijo = clave.substring(0, 22);
            $.ajax({
                url: '/api/presupuesto/obra?prefijo=' + prefijo,
                method: 'GET',
                success: function(res) {
                    if(res && res.length > 0) {
                        let html = "";
                        const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
                        res.forEach(item => {
                            let badge = item.ClavePresupuestal === claveGlobal ? '<span class="badge bg-gradient-success ms-2">Actual</span>' : '';
                            html += `<tr>
                                <td class="text-sm font-weight-bold">${item.ClavePresupuestal} ${badge}</td>
                                <td class="text-sm">${formater.format(item.Aprobado || 0)}</td>
                                <td class="text-sm">${formater.format(item.Modificado || 0)}</td>
                                <td class="text-sm text-primary">${formater.format(item.Devengado || 0)}</td>
                                <td class="text-sm">
                                    <a href="DetalleClave.aspx?clave=${item.ClavePresupuestal}" class="btn btn-primary btn-sm mb-0">Ver Detalle</a>
                                </td>
                            </tr>`;
                        });
                        $("#contenidoObrasRelacionadas").html(html);
                    } else {
                        $("#contenidoObrasRelacionadas").html('<tr><td colspan="5" class="text-center text-muted">No hay otras fuentes para esta obra.</td></tr>');
                    }
                },
                error: function() {
                }
            });
        }

        function renderPieChart(data) {
            let totalGastado = data.GastadoPorAdecuacion || 0;
            
            const meses = [data.Enero, data.Febrero, data.Marzo, data.Abril, data.Mayo, data.Junio, data.Julio, data.Agosto, data.Septiembre, data.Octubre, data.Noviembre, data.Diciembre];
            let sumaCal = 0;
            for(let i=0; i<12; i++) {
                sumaCal += (parseFloat(meses[i]) || 0);
            }

            let apartado = data.SaldoApartado || 0;
            // El Saldo Real Disponible es el Modificado menos lo apartado.
            // No le restamos totalGastado (Adecuaciones) porque el Modificado ya viene con la reducción aplicada.
            // Las adecuaciones (reducciones) reducen el pastel total, no se restan de nuevo del saldo.
            let saldo = sumaCal - apartado;
            if (saldo < 0) saldo = 0;

            // Por liberar solo compara el Modificado contra lo calendarizado. Las reducciones bajan el modificado,
            // por lo tanto no deben generar un falso "por liberar".
            let porLiberar = (data.Modificado || 0) - sumaCal;
            if (porLiberar < 0) porLiberar = 0;

            const formaterLegend = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
            document.getElementById('legGastado').innerText = formaterLegend.format(totalGastado);
            document.getElementById('legSaldo').innerText = formaterLegend.format(saldo);
            document.getElementById('legApartado').innerText = formaterLegend.format(apartado);
            document.getElementById('legLiberar').innerText = formaterLegend.format(porLiberar);

            let chartData = [];
            let chartColors = [];

            if (totalGastado === 0 && saldo === 0 && porLiberar === 0 && apartado === 0) {
                chartData = [{ name: 'Sin Asignación Inicial', y: 1 }];
                chartColors = ['#e9ecef'];
            } else {
                if (totalGastado > 0) {
                    chartData.push({ name: 'Gastado (Candado/Adecuaciones)', y: totalGastado });
                    chartColors.push('#9F2241');
                }
                if (saldo > 0) {
                    chartData.push({ name: 'Saldo por Gastar', y: saldo });
                    chartColors.push('#235B4E');
                }
                if (apartado > 0) {
                    if (data.Apartados && data.Apartados.length > 0) {
                        data.Apartados.forEach((ap, idx) => {
                            chartData.push({ name: 'Apartado: ' + (ap.Oficio || ('#' + (idx + 1))), y: ap.Monto });
                            chartColors.push('#FF8C00');
                        });
                    } else {
                        chartData.push({ name: 'Saldo Apartado', y: apartado });
                        chartColors.push('#FF8C00');
                    }
                }
                if (porLiberar > 0) {
                    chartData.push({ name: 'Por Liberar', y: porLiberar });
                    chartColors.push('#6c757d');
                }
            }

            Highcharts.chart('piechart_3d', {
                chart: { type: 'pie', backgroundColor: 'transparent', options3d: { enabled: true, alpha: 45, beta: 0 } },
                title: { text: null },
                tooltip: { 
                    pointFormatter: function() {
                        if (this.name === 'Sin Asignación Inicial') return '<b>$ 0.00</b>';
                        return '<b>$ ' + Highcharts.numberFormat(this.y, 2, '.', ',') + '</b>';
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true, cursor: 'pointer', depth: 45,
                        dataLabels: { enabled: true, format: '<b>{point.name}</b>' }
                    }
                },
                series: [{
                    type: 'pie',
                    name: 'Presupuesto',
                    colors: chartColors,
                    data: chartData
                }],
                credits: { enabled: false }
            });
        }

        function renderProgressBar(data) {
            const meses = [data.Enero, data.Febrero, data.Marzo, data.Abril, data.Mayo, data.Junio, data.Julio, data.Agosto, data.Septiembre, data.Octubre, data.Noviembre, data.Diciembre];
            const nombres = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
            
            let htmlBarras = "";
            let htmlEtiquetas = "";
            let htmlMontos = "";
            let sumaCal = 0;
            let acumulado = 0;
            const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });

            for(let i=0; i<12; i++) {
                let val = parseFloat(meses[i]) || 0;
                sumaCal += val;
                
                let color = '#e9ecef';
                if (val > 0) {
                    color = '#235B4E'; // Verde para saldo por gastar
                }
                
                let borderRight = i < 11 ? '1px solid white' : 'none';
                
                htmlBarras += `<div style="flex: 1; background-color: ${color}; border-right: ${borderRight};" title="${nombres[i]}: ${formater.format(val)}"></div>`;
                htmlEtiquetas += `<div style="flex: 1; text-align: center;">${nombres[i]}</div>`;
                
                let textColor = val > 0 ? "text-dark font-weight-bold" : "text-muted";
                htmlMontos += `<div style="flex: 1; text-align: center;" class="${textColor} text-xxs">${formater.format(val)}</div>`;
            }

            let porLiberar = montoAprobadoGlobal - sumaCal;
            if (porLiberar < 0) porLiberar = 0;

            let htmlCompleto = `
                <div class="d-flex w-100 justify-content-between mb-1 mt-2" style="border-radius: 10px; overflow: hidden; height: 30px; background: #e9ecef;">
                    ${htmlBarras}
                </div>
                <div class="d-flex w-100 justify-content-between text-xs text-muted mb-1 mt-2 px-1">
                    ${htmlEtiquetas}
                </div>
                <div class="d-flex w-100 justify-content-between text-xs mb-3 px-1">
                    ${htmlMontos}
                </div>
                <div class="d-flex justify-content-between mt-3 px-2 border-top pt-3 bg-light rounded" style="padding-bottom: 0.5rem;">
                    <div><span class="text-uppercase text-secondary font-weight-bolder text-xxs">Aprobado:</span> <span class="text-sm font-weight-bold text-dark ms-1">${formater.format(montoAprobadoGlobal)}</span></div>
                    <div><span class="text-uppercase text-secondary font-weight-bolder text-xxs">Suma Total:</span> <span class="text-sm font-weight-bold text-primary ms-1">${formater.format(sumaCal)}</span></div>
                    <div><span class="text-uppercase text-secondary font-weight-bolder text-xxs">Por Liberar:</span> <span class="text-sm font-weight-bold ${porLiberar > 0 ? 'text-danger' : 'text-success'} ms-1">${formater.format(porLiberar)}</span></div>
                </div>
            `;

            $("#tablaCalendarizado").html(htmlCompleto);
            
            // Se elimina el botón de calendarizar manualmente para claves nuevas / Por Liberar.
            // La calendarización ahora solo se permite estrictamente mediante adecuaciones de liberación.
            $("#btnCalendarizar").addClass('d-none');
        }

        function abrirModalCalendarizar() {
            const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
            $("#lblAprobadoModal").text(formater.format(montoAprobadoGlobal));
            $('#modalCalendarizar').modal('show');
            calcularSumaModal();
        }

        function calcularSumaModal() {
            let suma = 0;
            $('.mes-input').each(function() {
                suma += parseFloat($(this).val()) || 0;
            });
            const formater = new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' });
            $("#lblSumaModal").text(formater.format(suma));
            
            if (Math.abs(suma - montoAprobadoGlobal) < 1) {
                $("#lblSumaModal").removeClass('text-danger').addClass('text-success');
            } else {
                $("#lblSumaModal").removeClass('text-success').addClass('text-danger');
            }
        }

        function guardarCalendarizacion() {
            let suma = 0;
            let payload = { ClavePresupuestal: claveGlobal };
            const nombresCompletos = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
            
            $('.mes-input').each(function(i) {
                let val = parseFloat($(this).val()) || 0;
                suma += val;
                payload[nombresCompletos[i]] = val;
            });

            if (Math.abs(suma - montoAprobadoGlobal) > 1 && montoAprobadoGlobal > 0) {
                swal.fire('Error', 'La suma de los meses no coincide con el monto aprobado.', 'error');
                return;
            }

            // Llamada e a la base de datos real
            $.ajax({
                url: '/api/presupuesto/calendarizar',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(payload),
                success: function() {
                    $('#modalCalendarizar').modal('hide');
                    swal.fire('Éxito', 'Presupuesto calendarizado correctamente.', 'success');
                    cargarDatosFinancieros(claveGlobal);
                },
                error: function(err) {
                    console.error("Error al guardar:", err);
                    swal.fire('Error', 'Hubo un problema de conexión al guardar los datos en la base de datos.', 'error');
                }
            });
        }
    </script>
</body>
</html>
