<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="SIAP.Usuarios" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link rel="icon" type="image/png" href="assets/img/favicon.png">
    <title>SIAP - Gestión de Usuarios</title>
    <!-- Fonts and icons -->
    <script src="assets/js/jquery-3.5.1.js"></script>
    <link href="assets/css/googleapis.css" rel="stylesheet" />
    <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" rel="stylesheet" />
    <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
    <style>
        .sinborde{ border: 0; }
        .conborde{ border: 2px; }
        .linea { display: inline-block; }
        .header-guinda th {
            background-color: var(--color-guinda) !important;
            color: white !important;
            border-bottom: none !important;
            padding: 1rem !important;
        }
        .table-rounded-header thead tr th:first-child {
            border-top-left-radius: 0.75rem !important;
        }
        .table-rounded-header thead tr th:last-child {
            border-top-right-radius: 0.75rem !important;
        }
    </style>
    <script src="assets/js/SesionExpirada.js"></script>
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
                        <li class="breadcrumb-item text-sm text-white active" aria-current="page">Usuarios</li>
                    </ol>
                    <h6 class="font-weight-bolder text-white mb-0">SIAP</h6>
                </nav>
                <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
                    <h3 style="color:white; margin:0 auto;">Gestión de Usuarios</h3>
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
                                    <i class="fa-solid fa-users-gear me-2"></i>Lista de Usuarios
                                </h6>
                                <button type="button" class="btn bg-gradient-primary btn-sm ms-auto mt-3 shadow-primary" style="transition: all 0.2s ease-in-out;" onmouseover="this.style.transform='translateY(-2px)'" onmouseout="this.style.transform='translateY(0)'" onclick="abrirModalUsuario()">
                                    <i class="fas fa-user-plus me-2"></i>Nuevo Usuario
                                </button>
                            </div>
                            <div class="card-body px-4 pt-0 pb-2 mt-3">
                                <div class="table-responsive p-0">
                                    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" CssClass="table table-hover align-items-center mb-0 table-rounded-header" 
                                        OnRowCommand="gvUsuarios_RowCommand" GridLines="None" HeaderStyle-CssClass="text-uppercase text-xxs font-weight-bolder header-guinda">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Usuario">
                                                <ItemTemplate>
                                                    <div class="d-flex px-3 py-1">
                                                        <div>
                                                            <div class="avatar avatar-sm me-3 bg-gradient-dark shadow-sm d-flex align-items-center justify-content-center">
                                                                <i class="fa-solid fa-user text-white"></i>
                                                            </div>
                                                        </div>
                                                        <div class="d-flex flex-column justify-content-center">
                                                            <h6 class="mb-0 text-sm font-weight-bold"><%# Eval("Username") %></h6>
                                                            <p class="text-xs text-secondary mb-0"><%# Eval("NombreCompleto") %></p>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="ps-4" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rol">
                                                <ItemTemplate>
                                                    <span class='<%# Eval("Rol").ToString() == "Administrador" ? "badge badge-sm bg-gradient-primary shadow-primary" : "badge badge-sm bg-gradient-secondary shadow-secondary" %>'>
                                                        <i class='<%# Eval("Rol").ToString() == "Administrador" ? "fa-solid fa-shield-halved me-1" : "fa-solid fa-user-check me-1" %>'></i>
                                                        <%# Eval("Rol") %>
                                                    </span>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="align-middle text-center text-sm" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge badge-sm bg-gradient-success" : "badge badge-sm bg-gradient-danger" %>'>
                                                        <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                                                    </span>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="align-middle text-center text-sm" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-link text-primary font-weight-bolder px-3 mb-0" style="text-decoration: none; transition: all 0.2s;" onmouseover="this.style.transform='scale(1.1)'" onmouseout="this.style.transform='scale(1)'">
                                                        <i class="fas fa-user-edit text-primary me-2"></i>Editar
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-link text-danger font-weight-bolder px-3 mb-0" style="text-decoration: none; transition: all 0.2s;" onmouseover="this.style.transform='scale(1.1)'" onmouseout="this.style.transform='scale(1)'" OnClientClick="return confirm('¿Estás seguro de eliminar este usuario? Esta acción no se puede deshacer.');">
                                                        <i class="fas fa-trash text-danger me-2"></i>Eliminar
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="align-middle text-center text-sm" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal de Usuario -->
                <div class="modal fade" id="modalUsuario" tabindex="-1" role="dialog" aria-labelledby="modalUsuarioLabel" aria-hidden="true" style="zoom:100%;">
                    <div class="modal-dialog modal-dialog-centered" role="document">
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: var(--color-guinda);">
                                <h5 class="modal-title" id="modalUsuarioLabel" style="color:white;">Datos del Usuario</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body p-4">
                                <asp:HiddenField ID="hfUsuarioId" runat="server" Value="0" />
                                <div class="form-group">
                                    <label>Usuario</label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Contraseña (Dejar en blanco para no cambiar)</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Nombre Completo</label>
                                    <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Rol</label>
                                    <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control" onchange="toggleAccesos()">
                                        <asp:ListItem Value="Estandar" Text="Estándar"></asp:ListItem>
                                        <asp:ListItem Value="Administrador" Text="Administrador"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group form-check">
                                    <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label">Usuario Activo</label>
                                </div>

                                <div id="divAccesos" class="mt-4">
                                    <h6>Módulos Permitidos (Solo para Estándar)</h6>
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkVerDashboard" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label">Dashboard</label>
                                    </div>
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkVerPresupuesto" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label">Presupuesto</label>
                                    </div>
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkVerAdecuaciones" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label">Adecuaciones</label>
                                    </div>
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkVerMinistraciones" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label">Ministraciones</label>
                                    </div>
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkVerCatalogos" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label">Catálogos</label>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
    </form>
    
    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="assets/js/argon-dashboard.min.js?v=2.0.0"></script>
    <script>
        function abrirModalUsuario() {
            document.getElementById('<%= hfUsuarioId.ClientID %>').value = "0";
            document.getElementById('<%= txtUsername.ClientID %>').value = "";
            document.getElementById('<%= txtPassword.ClientID %>').value = "";
            document.getElementById('<%= txtNombreCompleto.ClientID %>').value = "";
            document.getElementById('<%= ddlRol.ClientID %>').value = "Estandar";
            document.getElementById('<%= chkActivo.ClientID %>').checked = true;
            
            document.getElementById('<%= chkVerDashboard.ClientID %>').checked = false;
            document.getElementById('<%= chkVerPresupuesto.ClientID %>').checked = false;
            document.getElementById('<%= chkVerAdecuaciones.ClientID %>').checked = false;
            document.getElementById('<%= chkVerMinistraciones.ClientID %>').checked = false;
            document.getElementById('<%= chkVerCatalogos.ClientID %>').checked = false;

            toggleAccesos();
            var myModal = new bootstrap.Modal(document.getElementById('modalUsuario'));
            myModal.show();
        }

        function toggleAccesos() {
            var rol = document.getElementById('<%= ddlRol.ClientID %>').value;
            var divAccesos = document.getElementById('divAccesos');
            if (rol === "Administrador") {
                divAccesos.style.display = 'none';
            } else {
                divAccesos.style.display = 'block';
            }
        }
        
        function mostrarModal() {
            toggleAccesos();
            var myModal = new bootstrap.Modal(document.getElementById('modalUsuario'));
            myModal.show();
        }

        // Si hay mensaje de éxito/error desde servidor
        function mostrarMensaje(tipo, msj) {
            Swal.fire({
                position: 'top-end',
                icon: tipo,
                title: msj,
                showConfirmButton: false,
                timer: 2000
            });
        }
    </script>
</body>
</html>
