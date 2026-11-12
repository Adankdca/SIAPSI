<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="SIAP.Bitacora" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="icon" type="image/png" href="assets/img/favicon.png">
    <title>SIAP - Bitácora de Movimientos</title>
    <!-- Fonts and icons -->
    <link href="assets/css/googleapis.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <style>
        .header-guinda th {
            background-color: var(--color-guinda) !important;
            color: white !important;
            border-bottom: none !important;
            padding: 1rem !important;
        }
        .table-rounded-header {
            border-collapse: separate !important;
            border-spacing: 0 !important;
        }
        .table-rounded-header tr th:first-child {
            border-top-left-radius: 0.75rem !important;
            border-bottom-left-radius: 0.75rem !important;
        }
        .table-rounded-header tr th:last-child {
            border-top-right-radius: 0.75rem !important;
            border-bottom-right-radius: 0.75rem !important;
        }
    </style>
</head>
<body class="g-sidenav-show virtual-reality">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- Navbar -->
        <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
            <div class="container-fluid py-1 px-3">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
                        <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Menu.aspx">Menú</a></li>
                        <li class="breadcrumb-item text-sm text-white active" aria-current="page">Bitácora</li>
                    </ol>
                    <h6 class="font-weight-bolder text-white mb-0">SIAP</h6>
                </nav>
                <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
                    <h3 style="color:white; margin:0 auto;">Bitácora de Movimientos</h3>
                    <div class="ms-md-auto pe-md-3 d-flex align-items-center">
                        <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                            <i class="fa-solid fa-arrow-left me-2"></i> Regresar
                        </a>
                    </div>
                </div>
            </div>
        </nav>
        <!-- End Navbar -->

        <main class="main-content mt-1 border-radius-lg">
            <div class="container-fluid py-4">
                <div class="row">
                    <div class="col-12">
                        <div class="card mb-4" style="border-radius: 1rem; box-shadow: 0 4px 6px rgba(0,0,0,0.05); border: none;">
                            <div class="card-header pb-0 p-3 d-flex justify-content-between align-items-center">
                                <h6 class="mb-0 text-uppercase text-secondary font-weight-bolder opacity-8">
                                    <i class="fa-solid fa-clock-rotate-left me-2"></i>Historial de Movimientos
                                </h6>
                            </div>
                            <div class="card-body px-4 pt-0 pb-2 mt-3">
                                <!-- Nav tabs -->
                                <ul class="nav nav-pills nav-fill p-2 mb-4 bg-gray-100" id="bitacoraTabs" role="tablist" style="border-radius: 1rem; box-shadow: inset 0 2px 4px rgba(0,0,0,0.06);">
                                    <li class="nav-item" role="presentation">
                                        <a class="nav-link mb-0 px-0 py-2 active d-flex align-items-center justify-content-center fw-bold" data-bs-toggle="tab" href="#sistema" role="tab" aria-controls="sistema" aria-selected="true" style="border-radius: 0.75rem; transition: all 0.3s ease;">
                                            <i class="fa-solid fa-desktop me-2 text-lg"></i> Inicios de Sesión
                                        </a>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <a class="nav-link mb-0 px-0 py-2 d-flex align-items-center justify-content-center fw-bold" data-bs-toggle="tab" href="#adecuaciones" role="tab" aria-controls="adecuaciones" aria-selected="false" style="border-radius: 0.75rem; transition: all 0.3s ease;">
                                            <i class="fa-solid fa-file-invoice-dollar me-2 text-lg"></i> Adecuaciones
                                        </a>
                                    </li>
                                    <li class="nav-item" role="presentation">
                                        <a class="nav-link mb-0 px-0 py-2 d-flex align-items-center justify-content-center fw-bold" data-bs-toggle="tab" href="#ministraciones" role="tab" aria-controls="ministraciones" aria-selected="false" style="border-radius: 0.75rem; transition: all 0.3s ease;">
                                            <i class="fa-solid fa-money-bill-transfer me-2 text-lg"></i> Ministraciones
                                        </a>
                                    </li>
                                </ul>

                                <!-- Tab panes -->
                                <div class="tab-content" id="bitacoraTabsContent">
                                    <!-- Pestaña Sistema -->
                                    <div class="tab-pane fade show active" id="sistema" role="tabpanel" aria-labelledby="sistema-tab">
                                        <div class="table-responsive p-0" style="max-height: 500px; overflow-y: auto;">
                                            <asp:GridView ID="gvSistema" runat="server" UseAccessibleHeader="true" AutoGenerateColumns="False" CssClass="table table-hover table-rounded-header align-items-center mb-0" GridLines="None" HeaderStyle-CssClass="text-uppercase text-xxs font-weight-bolder header-guinda" ShowHeaderWhenEmpty="true" EmptyDataText="<div class='text-center p-4'><i class='fa-solid fa-folder-open text-muted mb-3' style='font-size: 2rem;'></i><p class='text-muted mb-0'>No hay registros de inicio de sesión.</p></div>">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate><span class="text-secondary text-xs font-weight-bold"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Usuario">
                                                        <ItemTemplate>
                                                            <div class="d-flex px-3 py-1">
                                                                <div class="avatar avatar-sm me-3 bg-gradient-dark shadow-sm d-flex align-items-center justify-content-center">
                                                                    <i class="fa-solid fa-user-astronaut text-white"></i>
                                                                </div>
                                                                <div class="d-flex flex-column justify-content-center">
                                                                    <h6 class="mb-0 text-sm font-weight-bold"><%# Eval("NombreUsuario") %></h6>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="ps-4" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acción">
                                                        <ItemTemplate><span class="text-xs font-weight-bold"><%# Eval("Accion") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-center text-sm" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalle">
                                                        <ItemTemplate><span class="text-secondary text-xs"><%# Eval("Detalle") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-sm" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- Pestaña Adecuaciones -->
                                    <div class="tab-pane fade" id="adecuaciones" role="tabpanel" aria-labelledby="adecuaciones-tab">
                                        <div class="table-responsive p-0" style="max-height: 500px; overflow-y: auto;">
                                            <asp:GridView ID="gvAdecuaciones" runat="server" UseAccessibleHeader="true" AutoGenerateColumns="False" CssClass="table table-hover table-rounded-header align-items-center mb-0" GridLines="None" HeaderStyle-CssClass="text-uppercase text-xxs font-weight-bolder header-guinda" ShowHeaderWhenEmpty="true" EmptyDataText="<div class='text-center p-4'><i class='fa-solid fa-folder-open text-muted mb-3' style='font-size: 2rem;'></i><p class='text-muted mb-0'>No hay registros de adecuaciones.</p></div>">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate><span class="text-secondary text-xs font-weight-bold"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Usuario">
                                                        <ItemTemplate>
                                                            <div class="d-flex px-3 py-1">
                                                                <div class="avatar avatar-sm me-3 bg-gradient-dark shadow-sm d-flex align-items-center justify-content-center">
                                                                    <i class="fa-solid fa-user-astronaut text-white"></i>
                                                                </div>
                                                                <div class="d-flex flex-column justify-content-center">
                                                                    <h6 class="mb-0 text-sm font-weight-bold"><%# Eval("NombreUsuario") %></h6>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="ps-4" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalle">
                                                        <ItemTemplate><span class="text-secondary text-xs"><%# Eval("Detalle") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-sm" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate><%# FormatDetalleAdecuacion(Eval("Detalle")) %></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-end pe-4" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <!-- Pestaña Ministraciones -->
                                    <div class="tab-pane fade" id="ministraciones" role="tabpanel" aria-labelledby="ministraciones-tab">
                                        <div class="table-responsive p-0" style="max-height: 500px; overflow-y: auto;">
                                            <asp:GridView ID="gvMinistraciones" runat="server" UseAccessibleHeader="true" AutoGenerateColumns="False" CssClass="table table-hover table-rounded-header align-items-center mb-0" GridLines="None" HeaderStyle-CssClass="text-uppercase text-xxs font-weight-bolder header-guinda" ShowHeaderWhenEmpty="true" EmptyDataText="<div class='text-center p-4'><i class='fa-solid fa-folder-open text-muted mb-3' style='font-size: 2rem;'></i><p class='text-muted mb-0'>No hay registros de ministraciones.</p></div>">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Fecha">
                                                        <ItemTemplate><span class="text-secondary text-xs font-weight-bold"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Usuario">
                                                        <ItemTemplate>
                                                            <div class="d-flex px-3 py-1">
                                                                <div class="avatar avatar-sm me-3 bg-gradient-dark shadow-sm d-flex align-items-center justify-content-center">
                                                                    <i class="fa-solid fa-user-astronaut text-white"></i>
                                                                </div>
                                                                <div class="d-flex flex-column justify-content-center">
                                                                    <h6 class="mb-0 text-sm font-weight-bold"><%# Eval("NombreUsuario") %></h6>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="ps-4" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acción">
                                                        <ItemTemplate><span class="badge badge-sm bg-gradient-info shadow-info"><%# Eval("Accion") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-center text-sm" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Detalle">
                                                        <ItemTemplate><span class="text-secondary text-xs"><%# Eval("Detalle") %></span></ItemTemplate>
                                                        <ItemStyle CssClass="align-middle text-sm" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </form>
    
    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="assets/js/argon-dashboard.min.js?v=2.0.0"></script>
</body>
</html>
