<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Catalogos.aspx.cs" Inherits="SIAP.Catalogos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <title>Cat&aacute;logos | SIAP</title>
    <!-- CSS de Argon y FontAwesome -->
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.4" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <style>
        .glass-card { background: white; border-radius: 1rem; box-shadow: 0 10px 30px rgba(0,0,0,0.05); padding: 2rem; border: 1px solid rgba(0,0,0,0.05); }
        /* Fix para DataTable Length Menu (Mostrar X entradas) */
        .dataTables_length label {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin-bottom: 0;
            white-space: nowrap;
        }
        .dataTables_length select {
            width: auto !important;
            display: inline-block;
            padding: 0.25rem 1.5rem 0.25rem 0.5rem !important;
            height: auto !important;
            border-radius: 0.25rem !important;
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Menu.aspx">Menú</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Catálogos</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0">Gestión de Catálogos</h6>
        </nav>
        <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
          <h3 style="color:white; margin:0 auto;">Secretaría de Infraestructura</h3>
          <div class="ms-md-auto pe-md-3 d-flex align-items-center gap-2">
            <button id="btnVolverDashboard" class="btn btn-outline-white btn-sm mb-0 shadow-sm d-none" onclick="volverDashboard()">
                <i class="fa-solid fa-table-cells-large me-2"></i> Dashboard
            </button>
            <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                <i class="fa-solid fa-arrow-left me-2"></i> Regresar
            </a>
          </div>
        </div>
      </div>
    </nav>

    <main class="main-content mt-4">
        <div class="container-fluid py-4">
            <!-- Select Area in a Card -->
            <div class="card shadow-sm border-0 mb-4">
                <div class="card-body">
                    <div class="row align-items-end">
                        <div class="col-md-8">
                            <label class="form-control-label mb-2">Seleccionar Catálogo:</label>
                            <div class="dropdown w-100">
                                <button class="form-control d-flex justify-content-between align-items-center custom-select-btn" type="button" data-bs-toggle="dropdown" aria-expanded="false" style="background-color: #fff; border: 1px solid #dee2e6; color: #32325d; height: auto; padding: 0.5rem 0.75rem;">
                                    <span id="selectCatalogoText" class="selected-text text-start text-truncate" style="flex: 1;">Cargando catálogos...</span>
                                    <i class="fa-solid fa-chevron-down ms-2 text-muted" style="font-size: 0.8rem;"></i>
                                </button>
                                <ul id="selectCatalogoMenu" class="dropdown-menu w-100 custom-select-menu shadow" style="max-height: 300px; overflow-y: auto; z-index: 1050;">
                                    <!-- Opciones inyectadas por JS -->
                                </ul>
                                <input type="hidden" id="selectCatalogo" value="">
                            </div>
                        </div>
                        <div class="col-md-4 text-end mt-3 mt-md-0">
                            <button id="btnAgregar" class="btn btn-primary d-none mb-0 w-100 w-md-auto" onclick="abrirModal(false)">
                                <i class="fa-solid fa-plus me-2"></i> Nuevo Registro
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div id="tableContainer" class="card d-none shadow-sm border-0">
                <div class="card-body">
                    <div class="table-responsive">
                        <table id="tablaCatalogos" class="table table-hover w-100 align-items-center mb-0">
                            <thead class="text-secondary opacity-7">
                                <tr id="tablaHeaders"></tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Dashboard de Catálogos (Tarjetas) -->
            <div id="dashboardCatalogos" class="row mt-4">
                <!-- Se inyectarán las tarjetas vía JS -->
            </div>
        </div>
    </main>

    <!-- Modal Form -->
    <div class="modal fade" id="modalForm" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-gradient-primary">
                    <h5 class="modal-title text-white" id="modalTitle">Registro</h5>
                    <button type="button" class="btn-close text-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="formCatalogo">
                        <div id="formFields" class="row"></div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn btn-primary" onclick="guardarRegistro()">Guardar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Scripts -->
    <script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
    <script src="assets/js/core/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.13.6/js/dataTables.bootstrap5.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="/js/catalogos.js"></script>
</body>
</html>
