<%@ Page Language="C#" AutoEventWireup="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <title>DASHBOARD | SIAP</title>
    
    <!-- ============================================= -->
    <!-- ESTILOS Y RECURSOS EXTERNOS                    -->
    <!-- ============================================= -->
    <link href="assets/css/googleapis.css" rel="stylesheet" />
    <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.1/font/bootstrap-icons.min.css" />
    <script src="assets/js/kitfontaweson.js" crossorigin="anonymous"></script>
    <script src="assets/js/jquery-3.5.1.js"></script>
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    
    <style>
        /* =============================================
           ESTILOS PRINCIPALES
           ============================================= */
        
        /* ---- Contenedor principal ---- */
        .dashboard-container {
            max-width: 1400px;
            margin: 0 auto;
        }

        /* ---- Separadores de sección ---- */
        .section-divider {
            border: none;
            border-top: 2px solid #f0f0f0;
            margin: 20px 0;
        }
        .section-title {
            font-size: 0.75rem;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #6c757d;
            font-weight: 700;
            margin-bottom: 12px;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .section-title i {
            font-size: 1rem;
            color: #9F2241;
        }

        /* ---- Hero / Bienvenida ---- */
        .welcome-hero {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 16px;
            padding: 20px 30px;
            border: 1px solid #f0f0f0;
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 15px;
        }
        .welcome-hero .hero-left {
            display: flex;
            align-items: center;
            gap: 15px;
        }
        .welcome-hero .hero-icon {
            font-size: 2rem;
            color: #9F2241;
            background: rgba(159,34,65,0.08);
            width: 50px;
            height: 50px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 12px;
        }
        .welcome-hero .hero-text h5 {
            margin: 0;
            font-weight: 700;
            color: #1a1a1a;
        }
        .welcome-hero .hero-text p {
            margin: 0;
            font-size: 0.85rem;
            color: #6c757d;
        }
        .hero-badge {
            background: #9F2241;
            color: white;
            padding: 5px 16px;
            border-radius: 50px;
            font-size: 0.7rem;
            font-weight: bold;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        /* ---- Estadísticas generales ---- */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
            gap: 12px;
        }
        .stat-card {
            background: white;
            border-radius: 12px;
            padding: 16px 18px;
            border: 1px solid #f0f0f0;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            gap: 14px;
        }
        .stat-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(0,0,0,0.06);
            border-color: #9F2241;
        }
        .stat-card .stat-icon {
            font-size: 1.5rem;
            color: #9F2241;
            opacity: 0.7;
            width: 42px;
            height: 42px;
            background: rgba(159,34,65,0.06);
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }
        .stat-card .stat-info {
            flex: 1;
            min-width: 0;
        }
        .stat-card .stat-number {
            font-size: 1.4rem;
            font-weight: 700;
            color: #1a1a1a;
            line-height: 1.2;
        }
        .stat-card .stat-label {
            font-size: 0.7rem;
            color: #6c757d;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            font-weight: 600;
        }
        .stat-card .stat-trend {
            font-size: 0.65rem;
            color: #28a745;
            background: #e8f5e9;
            padding: 2px 10px;
            border-radius: 50px;
            display: inline-block;
            margin-top: 2px;
        }

        /* ---- Buscador ---- */
        .search-wrapper {
            position: relative;
            margin-top: 8px;
        }
        .search-wrapper .search-input {
            padding-left: 48px;
            padding-right: 60px;
            height: 52px;
            font-size: 0.95rem;
            border-radius: 12px;
            border: 2px solid #e8e8e8;
            transition: all 0.3s ease;
            background: white;
            width: 100%;
        }
        .search-wrapper .search-input:focus {
            border-color: #9F2241;
            box-shadow: 0 0 0 3px rgba(159,34,65,0.08);
            outline: none;
        }
        .search-wrapper .search-input::placeholder {
            color: #adb5bd;
            font-weight: 300;
        }
        .search-wrapper .search-icon-left {
            position: absolute;
            left: 16px;
            top: 50%;
            transform: translateY(-50%);
            color: #adb5bd;
            font-size: 1.1rem;
            transition: color 0.3s ease;
            pointer-events: none;
        }
        .search-wrapper .search-input:focus + .search-icon-left,
        .search-wrapper .search-input:not(:placeholder-shown) + .search-icon-left {
            color: #9F2241;
        }
        .search-wrapper .search-shortcut {
            position: absolute;
            right: 14px;
            top: 50%;
            transform: translateY(-50%);
            background: #f0f0f0;
            padding: 2px 10px;
            border-radius: 4px;
            font-size: 0.6rem;
            color: #999;
            font-weight: 600;
            letter-spacing: 0.5px;
        }
        .search-hint {
            font-size: 0.7rem;
            color: #adb5bd;
            margin-top: 8px;
            display: flex;
            align-items: center;
            gap: 12px;
            flex-wrap: wrap;
        }
        .search-hint .hint-item {
            display: flex;
            align-items: center;
            gap: 4px;
        }
        .search-hint .hint-item i {
            font-size: 0.8rem;
            color: #9F2241;
        }
        .search-hint kbd {
            background: #f5f5f5;
            padding: 1px 8px;
            border-radius: 3px;
            font-size: 0.6rem;
            color: #666;
            border: 1px solid #e8e8e8;
        }

        /* ---- Autocomplete ---- */
        .autocomplete-items {
            position: absolute;
            top: calc(100% + 6px);
            left: 0;
            right: 0;
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.12);
            max-height: 320px;
            overflow-y: auto;
            z-index: 1000;
            border: 1px solid #f0f0f0;
        }
        .autocomplete-item {
            padding: 12px 16px;
            cursor: pointer;
            transition: background 0.15s;
            border-bottom: 1px solid #f5f5f5;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .autocomplete-item:last-child {
            border-bottom: none;
        }
        .autocomplete-item:hover {
            background: #f8f9fa;
        }
        .autocomplete-item .item-main {
            flex: 1;
            min-width: 0;
        }
        .autocomplete-item .item-main strong {
            color: #9F2241;
        }
        .autocomplete-item .item-meta {
            font-size: 0.7rem;
            color: #999;
            font-family: monospace;
            text-align: right;
            flex-shrink: 0;
            margin-left: 12px;
        }
        .autocomplete-item .item-badge {
            background: #FFD700;
            color: #5a3e1b;
            font-size: 0.55rem;
            padding: 2px 8px;
            border-radius: 50px;
            font-weight: 600;
            margin-left: 8px;
        }

        /* ---- Obras frecuentes ---- */
        .frecuentes-wrapper {
            display: flex;
            align-items: center;
            gap: 12px;
            flex-wrap: wrap;
            margin-top: 12px;
        }
        .frecuentes-wrapper .frecuentes-label {
            font-size: 0.7rem;
            color: #6c757d;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            display: flex;
            align-items: center;
            gap: 6px;
        }
        .frecuentes-wrapper .frecuentes-label i {
            color: #9F2241;
        }
        .badge-frecuente {
            background: #f0f0f0;
            color: #333;
            padding: 4px 14px;
            border-radius: 50px;
            font-size: 0.75rem;
            cursor: pointer;
            transition: all 0.2s;
            display: inline-block;
            border: 1px solid transparent;
        }
        .badge-frecuente:hover {
            background: #9F2241;
            color: white;
            border-color: #9F2241;
            transform: scale(1.03);
        }

        /* =============================================
           VISTA DE DETALLE (se muestra al seleccionar)
           ============================================= */
        .detail-view {
            display: none;
            animation: fadeInUp 0.4s ease;
            margin-top: 20px;
        }
        .detail-view.active {
            display: block;
        }
        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(16px); }
            to { opacity: 1; transform: translateY(0); }
        }

        /* ---- Header de vuelta (más visible) ---- */
        .detail-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px 24px;
            background: linear-gradient(to right, #ffffff, #fdfdfd);
            border-radius: 16px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.04);
            border: 1px solid #c8ccd0;
            margin-bottom: 24px;
            flex-wrap: wrap;
            gap: 16px;
        }
        .btn-close-detail {
            background: #e74a3b;
            border: none;
            color: white;
            width: 36px;
            height: 36px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.25s ease;
            cursor: pointer;
            box-shadow: 0 2px 4px rgba(231,74,59,0.3);
        }
        .btn-close-detail:hover {
            background: #9F2241;
            color: white;
            box-shadow: 0 4px 8px rgba(159,34,65,0.4);
            transform: scale(1.05);
        }
        .btn-close-detail i {
            font-size: 1.1rem;
        }

        .detail-header .clave-info {
            display: flex;
            flex-direction: column;
            gap: 4px;
        }
        .detail-header .clave-info h5 {
            margin: 0;
            font-weight: 800;
            font-size: 1.25rem;
            color: #2c3e50;
            letter-spacing: -0.3px;
        }
        .detail-header .clave-info code {
            background: #eef2f5;
            padding: 4px 14px;
            border-radius: 50px;
            font-size: 0.75rem;
            color: #4a5568;
            border: 1px solid #e2e8f0;
            font-weight: 600;
            display: inline-block;
            width: fit-content;
        }
        .detail-header .clave-actions {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        /* ---- Botones ---- */
        .btn-solicitar {
            background: linear-gradient(135deg, #FFB703 0%, #FB8500 100%);
            color: white;
            border: none;
            font-weight: 700;
            padding: 10px 22px;
            border-radius: 50px;
            transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            font-size: 0.85rem;
            display: flex;
            align-items: center;
            gap: 8px;
            box-shadow: 0 4px 12px rgba(251, 133, 0, 0.3);
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .btn-solicitar:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 16px rgba(251, 133, 0, 0.4);
            color: white;
        }

        .btn-simulacion {
            background: #6c757d;
            color: white;
            border: none;
            font-weight: 600;
            padding: 5px 14px;
            border-radius: 50px;
            font-size: 0.7rem;
            transition: all 0.2s;
            display: flex;
            align-items: center;
            gap: 4px;
        }
        .btn-simulacion:hover {
            background: #5a6268;
            color: white;
        }

        .badge-simulacion {
            background: #FFD700;
            color: #5a3e1b;
            padding: 3px 12px;
            border-radius: 50px;
            font-size: 0.65rem;
            font-weight: 600;
            display: none;
            align-items: center;
            gap: 4px;
        }
        .badge-simulacion.active {
            display: inline-flex;
            animation: pulse 1.5s ease-in-out infinite;
        }
        @keyframes pulse {
            0%, 100% { opacity: 1; }
            50% { opacity: 0.6; }
        }

        /* ---- Métricas en grid ---- */
        .metrics-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 12px;
            margin-bottom: 20px;
        }
        .metric-card {
            background: white;
            border-radius: 12px;
            padding: 16px 18px;
            border: 1px solid #c8ccd0;
            box-shadow: 0 2px 10px rgba(0,0,0,0.04);
            transition: all 0.3s ease;
        }
        .metric-card:hover {
            box-shadow: 0 4px 16px rgba(0,0,0,0.06);
            transform: translateY(-2px);
        }
        .metric-card .metric-label {
            font-size: 0.65rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #6c757d;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        .metric-card .metric-label i {
            font-size: 0.8rem;
        }
        .metric-card .metric-value {
            font-size: 1.3rem;
            font-weight: 700;
            margin: 4px 0 2px 0;
        }
        .metric-card .metric-sub {
            font-size: 0.65rem;
            color: #999;
        }
        .metric-card .metric-bar {
            margin-top: 8px;
            height: 4px;
            background: #e9ecef;
            border-radius: 4px;
            overflow: hidden;
        }
        .metric-card .metric-bar-fill {
            height: 100%;
            border-radius: 4px;
            transition: width 0.8s ease;
        }

        /* ---- Contenedor gráfica + resumen ---- */
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
            min-height: 280px;
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
        .summary-divider {
            border: none;
            border-top: 1px solid #f0f0f0;
            margin: 6px 0;
        }
        .summary-footer {
            font-size: 0.75rem;
            color: #999;
            display: flex;
            flex-direction: column;
            gap: 4px;
        }
        .summary-footer span {
            display: flex;
            align-items: center;
            gap: 6px;
        }
        .summary-footer i {
            color: #9F2241;
            font-size: 0.8rem;
        }

        /* ---- Estado de bienvenida en detalle ---- */
        .welcome-state {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            height: 280px;
            text-align: center;
            padding: 20px;
        }
        .welcome-state .ws-icon {
            font-size: 3.5rem;
            color: #d0d0d0;
            margin-bottom: 12px;
            animation: float 3s ease-in-out infinite;
        }
        @keyframes float {
            0%, 100% { transform: translateY(0); }
            50% { transform: translateY(-8px); }
        }
        .welcome-state h5 {
            color: #555;
            font-weight: 600;
            margin-bottom: 6px;
        }
        .welcome-state p {
            color: #999;
            font-size: 0.85rem;
            max-width: 380px;
            margin: 0 auto;
        }
        .welcome-state .ws-tips {
            display: flex;
            gap: 14px;
            margin-top: 16px;
            flex-wrap: wrap;
            justify-content: center;
        }
        .welcome-state .ws-tip {
            background: #f8f9fa;
            padding: 5px 14px;
            border-radius: 50px;
            font-size: 0.7rem;
            color: #666;
            display: flex;
            align-items: center;
            gap: 5px;
        }
        .welcome-state .ws-tip i {
            color: #9F2241;
            font-size: 0.8rem;
        }

        /* ---- Loading ---- */
        .loading-spinner {
            display: none;
            text-align: center;
            padding: 30px;
        }
        .loading-spinner .spinner-border {
            width: 2.5rem;
            height: 2.5rem;
            color: #9F2241;
        }

        /* ---- Modal ---- */
        .modal-content-blanc {
            background-color: #ffffff;
            border: none;
            border-radius: 16px;
            box-shadow: 0 20px 35px -8px rgba(0,0,0,0.15);
        }
        .modal-content-blanc .modal-header {
            border-bottom: 2px solid #f0f0f0;
            padding: 1.25rem 1.5rem;
        }
        .modal-content-blanc .modal-header .modal-title {
            color: #9F2241;
            font-weight: 700;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .modal-content-blanc .modal-header .btn-close {
            background-color: #f0f0f0;
            border-radius: 50%;
            opacity: 1;
            padding: 6px;
        }
        .modal-content-blanc .modal-body {
            padding: 1.5rem;
        }
        .modal-content-blanc .modal-footer {
            border-top: 1px solid #f0f0f0;
            padding: 1rem 1.5rem 1.5rem 1.5rem;
        }
        .modal-content-blanc .form-label {
            color: #333;
            font-weight: 600;
            font-size: 0.8rem;
        }
        .modal-content-blanc .form-control {
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 0.65rem 1rem;
        }
        .modal-content-blanc .form-control:focus {
            border-color: #9F2241;
            box-shadow: 0 0 0 3px rgba(159,34,65,0.08);
        }
        .btn-enviar-solicitud {
            background: linear-gradient(135deg, #28a745 0%, #1e7e34 100%);
            color: white;
            border: none;
            padding: 8px 22px;
            border-radius: 50px;
            font-weight: 600;
            transition: all 0.2s;
            display: flex;
            align-items: center;
            gap: 6px;
        }
        .btn-enviar-solicitud:hover {
            transform: translateY(-2px);
            color: white;
            box-shadow: 0 4px 12px rgba(40,167,69,0.3);
        }

        /* ---- VISTA INICIO (oculta cuando hay detalle) ---- */
        .view-inicio {
            display: block;
        }
        .view-inicio.hidden {
            display: none;
        }

        /* ---- Ocultar Resumen General en Vista Detalle ---- */
        .resumen-general-wrapper {
            display: block;
        }
        .resumen-general-wrapper.hidden {
            display: none;
        }

        /* ---- Responsive ---- */
        @media (max-width: 992px) {
            .stats-grid { grid-template-columns: repeat(2, 1fr); }
            .metrics-grid { grid-template-columns: repeat(2, 1fr); }
            .chart-summary-wrapper { grid-template-columns: 1fr; }
            .summary-side {
                border-left: none;
                border-top: 2px solid #f0f0f0;
                padding: 16px 0 0 0;
            }
            .welcome-hero { flex-direction: column; align-items: flex-start; }
            .detail-header { flex-direction: column; align-items: stretch; }
        }
        @media (max-width: 576px) {
            .stats-grid { grid-template-columns: 1fr; }
            .metrics-grid { grid-template-columns: 1fr; }
            .detail-header .clave-actions { width: 100%; }
            .detail-header .clave-actions .btn-solicitar { flex: 1; justify-content: center; }
            .search-wrapper .search-shortcut { display: none; }
        }
    </style>
    
    <!-- ============================================= -->
    <!-- VALIDACIÓN DE SESIÓN                          -->
    <!-- ============================================= -->
    <script type="text/javascript">
        var data = sessionStorage.getItem('name');
        if (data == null) {
            window.location.href = "Login.aspx";
        }
        function eliminarsesion() {
            sessionStorage.removeItem('name');
            window.location.href = "Login.aspx";
        }
    </script>
</head>
<body>
    <!-- ============================================= -->
    <!-- BARRA DE NAVEGACIÓN                           -->
    <!-- ============================================= -->
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
      <div class="container-fluid py-1 px-3">
        <nav aria-label="breadcrumb">
          <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
            <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Menu.aspx">Menú</a></li>
            <li class="breadcrumb-item text-sm text-white active" aria-current="page">Dashboard</li>
          </ol>
          <h6 class="font-weight-bolder text-white mb-0">Resumen Ejecutivo</h6>
        </nav>
        <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
          <h3 style="color:white; margin:0 auto;">Secretaría de Infraestructura</h3>
          <div class="ms-md-auto pe-md-3 d-flex align-items-center"></div>
          <ul class="navbar-nav justify-content-end">
            <li class="nav-item dropdown pe-2 d-flex align-items-center">
              <a href="javascript:;" class="nav-link text-white p-0" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                <i class="ni ni-settings-gear-65 cursor-pointer"></i> Opciones
              </a>
              <ul class="dropdown-menu dropdown-menu-end px-2 py-3 me-sm-n4" aria-labelledby="dropdownMenuButton">
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
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- ============================================= -->
    <!-- CONTENIDO PRINCIPAL                          -->
    <!-- ============================================= -->
    <main class="main-content mt-4">
        <div class="container-fluid py-4">
            <div class="row">
                <!-- COLUMNA IZQUIERDA: BÚSQUEDAS RECIENTES -->
                <div class="col-lg-3">
                    <div class="card glass-card h-100" id="cardRecientes" style="min-height: calc(100vh - 150px);">
                        <div class="card-body">
                            <div class="section-title mb-3">
                                <i class="bi bi-clock-history"></i> Búsquedas Recientes
                            </div>
                            <div id="obrasFrecuentesContainer" style="display: none;">
                                <div id="listaObrasFrecuentes" class="d-flex flex-column gap-2"></div>
                            </div>
                            <div id="recientesVacio" class="text-center text-muted py-5" style="display: block;">
                                <i class="bi bi-search" style="font-size: 2rem; opacity: 0.5;"></i>
                                <p class="text-sm mt-2 mb-0">Aún no hay búsquedas recientes.</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- COLUMNA DERECHA: BUSCADOR Y DETALLE -->
                <div class="col-lg-9">
                    
                    <!-- TARJETA 1: BUSCADOR Y RESUMEN GENERAL -->
                    <div class="card glass-card mb-4" id="cardBuscador">
                        <div class="card-header pb-0 pt-3 bg-transparent border-0">
                            <div class="d-flex justify-content-between align-items-center flex-wrap">
                                <div>
                                    <h6 class="text-capitalize mb-0">Panel de Control</h6>
                                    <p class="text-sm mb-0 text-muted">
                                        <i class="bi bi-info-circle text-info"></i>
                                        <span class="font-weight-bold">Consulta de presupuesto</span> por nombre de obra o clave presupuestal
                                    </p>
                                </div>
                                <div class="d-flex align-items-center gap-2">
                                    <span id="badgeSimulacion" class="badge-simulacion text-xs py-1 px-2 rounded-pill font-weight-bold" style="display: none; background: #fff3cd; color: #856404; border: 1px solid #ffeeba;">
                                        <i class="bi bi-exclamation-triangle-fill me-1"></i> CON APARTADO
                                    </span>
                                    <div class="hero-badge">
                                        <i class="bi bi-calendar3"></i>
                                        <span id="fechaActual"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <!-- Buscador -->
                            <div class="section-title">
                                <i class="bi bi-search"></i> Buscador de Obras
                            </div>
                            <div class="search-wrapper">
                                <input type="text" id="buscadorClave" class="search-input"
                                    placeholder="Ej. Programa de modernización, 2025-001, o nombre de municipio..."
                                    autocomplete="off">
                                <i class="bi bi-search search-icon-left"></i>
                               
                                <div id="autocompleteResultados" class="autocomplete-items" style="display: none; position: absolute; z-index: 1000; width: 100%;"></div>
                            </div>
                            <div class="search-hint mb-4">
                                <span class="hint-item"><i class="bi bi-lightbulb"></i> Sugerencias:</span>
                                <span class="hint-item"><kbd>Clave</kbd> presupuestal</span>
                                <span class="hint-item"><kbd>Municipio</kbd> o localidad</span>
                                <span class="hint-item"><kbd>Programa</kbd> o proyecto</span>
                            </div>

                            <!-- Loading -->
                            <div id="loadingIndicator" class="loading-spinner" style="display: none;">
                                <div class="spinner-border" role="status">
                                    <span class="visually-hidden">Cargando...</span>
                                </div>
                                <p class="mt-2 text-muted">Buscando información...</p>
                            </div>

                            <!-- Resumen General -->
                            <div id="resumenGeneralWrapper" class="resumen-general-wrapper">
                                <hr class="section-divider">
                                <div class="section-title">
                                    <i class="bi bi-bar-chart-fill"></i> Resumen General
                                </div>
                                <div class="stats-grid" id="statsGrid">
                                    <div class="stat-card">
                                        <div class="stat-icon"><i class="bi bi-building"></i></div>
                                        <div class="stat-info">
                                            <div class="stat-number" id="totalObras">0</div>
                                            <div class="stat-label">Obras Registradas</div>
                                        </div>
                                    </div>
                                    <div class="stat-card">
                                        <div class="stat-icon"><i class="bi bi-cash-stack"></i></div>
                                        <div class="stat-info">
                                            <div class="stat-number" id="totalPresupuesto">$0</div>
                                            <div class="stat-label">Presupuesto Total</div>
                                        </div>
                                    </div>
                                    <div class="stat-card">
                                        <div class="stat-icon"><i class="bi bi-check-circle"></i></div>
                                        <div class="stat-info">
                                            <div class="stat-number" id="totalEjecutado">$0</div>
                                            <div class="stat-label">Total Ejecutado</div>
                                            <div class="stat-trend" id="trendEjecucion">0%</div>
                                        </div>
                                    </div>
                                    <div class="stat-card">
                                        <div class="stat-icon" style="background: #FFF3CD; color: #FF8C00;"><i class="bi bi-lock"></i></div>
                                        <div class="stat-info">
                                            <div class="stat-number" id="totalApartado" style="color: #FF8C00;">$0</div>
                                            <div class="stat-label">Total Apartado</div>
                                        </div>
                                    </div>
                                    <div class="stat-card">
                                        <div class="stat-icon"><i class="bi bi-clock-history"></i></div>
                                        <div class="stat-info">
                                            <div class="stat-number" id="totalDisponible">$0</div>
                                            <div class="stat-label">Saldo Disponible</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- TARJETA 2: DETALLES DE LA OBRA -->
                    <div class="card glass-card" id="cardDetalleObra" style="display: none;">
                        <div class="card-body">
                            <!-- Header con botón de vuelta (X a la derecha) -->
                            <div class="detail-header">
                                <div style="display:flex; align-items:center; gap:16px; flex-wrap:wrap; flex: 1;">
                                    <div class="clave-info">
                                        <h5 id="claveNombre"></h5>
                                        <code id="claveCompleta"></code>
                                    </div>
                                </div>
                                <div class="clave-actions mt-3 mt-md-0 d-flex gap-2 align-items-center">
                                    <button class="btn btn-sm btn-outline-danger mb-0 d-none font-weight-bold" style="border-radius: 50px; padding: 10px 20px;" id="btnResetSimulacion" onclick="resetearSimulacion()">
                                        <i class="bi bi-x-circle-fill me-1"></i> Cancelar Todos
                                    </button>
                                    <button class="btn btn-solicitar mb-0" onclick="abrirModalAdecuacion()">
                                        <i class="bi bi-shield-lock-fill"></i> Solicitar Apartado
                                    </button>
                                    <button class="btn-close-detail mb-0" onclick="cerrarDetalle()" title="Cerrar detalles">
                                        <i class="bi bi-x-lg"></i>
                                    </button>
                                </div>
                            </div>

                            <!-- Métricas -->
                            <div class="metrics-grid">
                                <div class="metric-card">
                                    <div class="metric-label"><i class="bi bi-pencil-square"></i> Presupuesto Modificado</div>
                                    <div class="metric-value" id="montoModificado">$0.00</div>
                                    <div class="metric-sub">Total con adecuaciones</div>
                                </div>
                                <div class="metric-card" style="border-left: 3px solid #28a745;">
                                    <div class="metric-label"><i class="bi bi-wallet2" style="color:#28a745;"></i> Saldo Disponible</div>
                                    <div class="metric-value" style="color: #28a745;" id="saldoDisponible">$0.00</div>
                                    <div class="metric-sub">Para ejercer</div>
                                </div>
                                <div class="metric-card" style="border-left: 3px solid #FF8C00;">
                                    <div class="metric-label"><i class="bi bi-lock" style="color:#FF8C00;"></i> Saldo Apartado</div>
                                    <div class="metric-value" style="color: #FF8C00;" id="saldoApartado">$0.00</div>
                                    <div class="metric-sub" id="simulacionInfo"></div>
                                </div>
                                <div class="metric-card" style="border-left: 3px solid #9F2241;">
                                    <div class="metric-label"><i class="bi bi-graph-up" style="color:#9F2241;"></i> Ejecución</div>
                                    <div class="metric-value" id="porcentajeEjecucion">0%</div>
                                    <div class="metric-bar">
                                        <div id="barraProgreso" class="metric-bar-fill" style="width: 0%; background: #9F2241;"></div>
                                    </div>
                                </div>
                            </div>

                            <!-- Gráfica + Resumen -->
                            <div class="chart-summary-wrapper">
                                <div class="chart-container">
                                    <div id="piechart_3d" style="width: 100%; height: 280px;"></div>
                                </div>
                                <div class="summary-side">
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #9F2241;"></span>
                                        <span class="summary-label">Monto Ministrado</span>
                                        <span class="summary-value" id="sumMinistrado" style="color: #9F2241;">$0.00</span>
                                    </div>
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #FF8C00;"></span>
                                        <span class="summary-label">Saldo Apartado</span>
                                        <span class="summary-value" id="sumApartado" style="color: #FF8C00;">$0.00</span>
                                    </div>
                                    <div class="summary-item">
                                        <span class="summary-dot" style="background: #235B4E;"></span>
                                        <span class="summary-label">Saldo Disponible</span>
                                        <span class="summary-value" id="sumDisponible" style="color: #235B4E;">$0.00</span>
                                    </div>
                                    <hr class="summary-divider">
                                    <div class="summary-footer">
                                        <span><i class="bi bi-building"></i> <span id="unidadResponsable">Unidad: -</span></span>
                                        <span><i class="bi bi-tag"></i> <span id="programaPresupuestario">Programa: -</span></span>
                                    </div>
                                </div>
                            </div>

                            <!-- Nuevo Card de Listado de Apartados -->
                            <div id="listaApartadosContainer" class="card mt-4 shadow-sm d-none" style="border: 1px solid rgba(255, 140, 0, 0.3); border-radius: 12px;">
                                <div class="card-body p-3">
                                    <h6 class="text-uppercase font-weight-bolder mb-3" style="color: #FF8C00; font-size: 0.85rem;"><i class="bi bi-card-list"></i> Apartados Activos en esta Clave</h6>
                                    <div id="listaApartadosBody" class="d-flex flex-column gap-2">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <footer class="footer pt-3 mt-4">
                <div class="container-fluid">
                    <div class="row align-items-center justify-content-lg-between">
                        <div class="col-lg-6 mb-lg-0 mb-4">
                            <div class="copyright text-center text-sm text-muted text-lg-start" style="color:var(--color-dorado-oscuro) !important">
                                © <script> document.write(new Date().getFullYear()) </script>,
                                Secretaría de Infraestructura.
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </main>

    <!-- ============================================= -->
    <!-- MODAL PARA SOLICITAR APARTADO                 -->
    <!-- ============================================= -->
    <div class="modal fade" id="modalAdecuacion" tabindex="-1" aria-labelledby="modalAdecuacionLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content glass-card" style="border:none; box-shadow: 0 10px 30px rgba(0,0,0,0.2);">
                <div class="modal-header" style="background: linear-gradient(135deg, var(--color-vino), #7a1a32); color: white; border-radius: 15px 15px 0 0;">
                    <h5 class="modal-title font-weight-bolder" style="color: white;">
                        <i class="bi bi-shield-lock-fill me-2"></i> Reservar Presupuesto
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body p-4">
                    <p class="text-sm text-muted mb-4">El presupuesto será apartado por un periodo de <strong>7 días</strong> a la espera de la adecuación. Durante este tiempo el saldo no podrá ser utilizado por otros proyectos.</p>
                    
                    <input type="hidden" id="modalClaveActual" />
                    
                    <div class="form-group mb-3">
                        <label class="form-control-label text-xs font-weight-bolder text-uppercase"><i class="bi bi-key-fill text-primary"></i> Clave Presupuestal</label>
                        <input type="text" id="modalClaveTexto" class="form-control" readonly style="background-color: #e9ecef; font-family: monospace; font-size: 0.85rem; border-radius: 8px;">
                    </div>
                    
                    <div class="form-group mb-3">
                        <label class="form-control-label text-xs font-weight-bolder text-uppercase"><i class="bi bi-cash-stack text-success"></i> Monto a Reservar <span class="text-danger">*</span></label>
                        <div class="input-group">
                            <span class="input-group-text" style="border-radius: 8px 0 0 8px; background-color: #f8f9fa;">$</span>
                            <input type="number" id="montoApartar" class="form-control" placeholder="0.00" step="0.01" min="0.01" style="border-radius: 0 8px 8px 0;">
                        </div>
                        <small id="saldoDisponibleModal" class="text-xs font-weight-bold text-success mt-1 d-block"></small>
                    </div>
                    
                    <div class="form-group mb-3">
                        <label class="form-control-label text-xs font-weight-bolder text-uppercase"><i class="bi bi-chat-text-fill text-info"></i> Motivo / Justificación <span class="text-danger">*</span></label>
                        <textarea id="motivoApartado" class="form-control" rows="3" placeholder="Describa el motivo de la reserva..." style="border-radius: 8px;"></textarea>
                    </div>
                    
                    <div class="form-group mb-3">
                        <label class="form-control-label text-xs font-weight-bolder text-uppercase"><i class="bi bi-file-earmark-text-fill text-secondary"></i> Número de Oficio</label>
                        <input type="text" id="numOficio" class="form-control" placeholder="Ej. OF-2026-001" style="border-radius: 8px;">
                    </div>
                </div>
                <div class="modal-footer border-0 pt-0">
                    <button type="button" class="btn text-white mb-0" style="background-color: #9F2241; border-radius: 8px; border: none;" data-bs-dismiss="modal">Cancelar</button>
                    <button type="button" class="btn text-white mb-0" style="background-color: #235B4E; border-radius: 8px; border: none;" onclick="simularApartado()">
                        Confirmar Reserva
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- ============================================= -->
    <!-- SCRIPTS                                       -->
    <!-- ============================================= -->
    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/highcharts-3d.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script>
        // =============================================
        // VARIABLES GLOBALES
        // =============================================
        let graficoActual = null;
        let claveSeleccionadaActual = null;
        let datosOriginales = null;
        let datosSimulados = null;
        let estaSimulado = false;
        let todasLasClaves = [];
        const API_URL = '/api/presupuesto/matriz';

        // =============================================
        // INICIALIZACIÓN
        // =============================================
        document.addEventListener('DOMContentLoaded', async function () {
            console.log('🚀 Dashboard iniciado (MODO SIMULACIÓN)');

            // Fecha actual
            const hoy = new Date();
            document.getElementById('fechaActual').textContent = hoy.toLocaleDateString('es-MX', {
                weekday: 'long',
                year: 'numeric',
                month: 'long',
                day: 'numeric'
            });

            // Nombre de usuario
            const nombre = sessionStorage.getItem('name') || 'Usuario';
            const elNombre = document.getElementById('nombreUsuario');
            if (elNombre) elNombre.textContent = nombre;

            // Estado inicial: Vista Inicio
            mostrarVistaInicio();

            // Cargar datos
            await cargarTodasLasClaves();
            await actualizarEstadisticasGenerales();
            mostrarObrasFrecuentes();

            console.log('✅ Sistema listo, claves disponibles:', todasLasClaves.length);

            // Shortcut: Ctrl+K o Cmd+K para enfocar buscador
            document.addEventListener('keydown', function (e) {
                if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
                    e.preventDefault();
                    document.getElementById('buscadorClave').focus();
                }
            });
        });

        // =============================================
        // TRANSICIONES DE UI
        // =============================================
        function mostrarInicioUI() {
            document.getElementById('cardDetalleObra').style.display = 'none';
            document.getElementById('resumenGeneralWrapper').style.display = 'block';
            document.getElementById('buscadorClave').value = '';
            document.getElementById('autocompleteResultados').style.display = 'none';
        }

        function mostrarDetalleUI() {
            document.getElementById('resumenGeneralWrapper').style.display = 'none';
            document.getElementById('cardDetalleObra').style.display = 'block';
            document.getElementById('autocompleteResultados').style.display = 'none';
            
            // Scroll suave hacia el detalle
            setTimeout(() => {
                document.getElementById('cardDetalleObra').scrollIntoView({ behavior: 'smooth', block: 'start' });
            }, 100);
        }

        function cerrarDetalle() {
            claveSeleccionadaActual = null;
            mostrarInicioUI();
        }

        function mostrarVistaInicio() {
            mostrarInicioUI();
        }

        function mostrarVistaDetalle() {
            mostrarDetalleUI();
        }

        function volverAlInicio() {
            cerrarDetalle();
        }

        // =============================================
        // ESTADÍSTICAS GENERALES
        // =============================================
        async function actualizarEstadisticasGenerales() {
            if (todasLasClaves.length === 0) await cargarTodasLasClaves();

            const totalObras = todasLasClaves.length;
            const totalPresupuesto = todasLasClaves.reduce((sum, item) => sum + (item.Modificado || 0), 0);
            const totalEjecutado = todasLasClaves.reduce((sum, item) => sum + (item.Ministerio || 0), 0);
            const totalApartado = todasLasClaves.reduce((sum, item) => sum + (item.SaldoApartado || 0), 0);
            const totalDisponible = totalPresupuesto - totalEjecutado - totalApartado;
            const porcentajeEjecucion = totalPresupuesto > 0 ? (totalEjecutado / totalPresupuesto * 100) : 0;

            document.getElementById('totalObras').textContent = totalObras.toLocaleString();
            document.getElementById('totalPresupuesto').textContent = '$' + totalPresupuesto.toLocaleString('es-MX', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
            document.getElementById('totalEjecutado').textContent = '$' + totalEjecutado.toLocaleString('es-MX', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
            document.getElementById('totalApartado').textContent = '$' + totalApartado.toLocaleString('es-MX', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
            document.getElementById('totalDisponible').textContent = '$' + totalDisponible.toLocaleString('es-MX', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
            document.getElementById('trendEjecucion').textContent = porcentajeEjecucion.toFixed(1) + '%';
        }

        // =============================================
        // CARGAR TODAS LAS CLAVES
        // =============================================
        async function cargarTodasLasClaves() {
            try {
                console.log('📡 Cargando todas las claves...');
                const response = await fetch(API_URL);
                const data = await response.json();
                console.log('✅ Claves cargadas:', data.length);
                todasLasClaves = data;
                return data;
            } catch (error) {
                console.error('❌ Error al cargar claves:', error);
                return [];
            }
        }

        // =============================================
        // BÚSQUEDA
        // =============================================
        async function buscarObras(texto) {
            if (todasLasClaves.length === 0) {
                await cargarTodasLasClaves();
            }

            texto = texto.toLowerCase();
            const resultados = todasLasClaves.filter(item => {
                if (item.ClavePresupuestal && item.ClavePresupuestal.toLowerCase().includes(texto)) return true;
                if (item.UrDescripcion && item.UrDescripcion.toLowerCase().includes(texto)) return true;
                if (item.ProgramaDescripcion && item.ProgramaDescripcion.toLowerCase().includes(texto)) return true;
                if (item.ObjetoGastoDescripcion && item.ObjetoGastoDescripcion.toLowerCase().includes(texto)) return true;
                if (item.NombreMunicipio && item.NombreMunicipio.toLowerCase().includes(texto)) return true;
                return false;
            }).slice(0, 15);

            return resultados;
        }

        async function obtenerDetalleClave(clave) {
            const item = todasLasClaves.find(c => c.ClavePresupuestal === clave);
            if (item) {
                let apartadosExtra = [];
                try {
                    const res = await fetch('/api/presupuesto/detalle?clave=' + encodeURIComponent(clave));
                    if (res.ok) {
                        const detallesBackend = await res.json();
                        apartadosExtra = detallesBackend.Apartados || detallesBackend.apartados || [];
                    }
                } catch(e) { console.warn('No se pudieron obtener apartados del servidor', e); }

                const saldoApartado = item.SaldoApartado || 0;
                const saldoDisponible = (item.Modificado || 0) - (item.Ministerio || 0) - saldoApartado;

                // Mapear los apartados para asegurar que las propiedades estén en minúscula como lo espera la vista
                const apartadosMapeados = apartadosExtra.map(ap => ({
                    id: ap.Id || ap.id,
                    monto: ap.Monto || ap.monto || 0,
                    motivo: ap.Motivo || ap.motivo || '',
                    oficio: ap.Oficio || ap.oficio || ''
                }));

                const detalle = {
                    clave: item.ClavePresupuestal,
                    nombre: item.ProgramaDescripcion || item.UrDescripcion || item.ObjetoGastoDescripcion || 'Sin descripción',
                    aprobado: item.Aprobado || 0,
                    modificado: item.Modificado || 0,
                    ministrado: item.Ministerio || 0,
                    devengado: 0,
                    pagado: 0,
                    saldoDisponible: saldoDisponible,
                    saldoApartado: saldoApartado,
                    porcentajeEjecucion: item.Modificado > 0 ? ((item.Ministerio || 0) / item.Modificado * 100) : 0,
                    unidadResponsable: item.UrDescripcion || 'N/A',
                    objetoGasto: item.ObjetoGastoDescripcion || 'N/A',
                    tipoGasto: 'N/A',
                    fuenteFinanciamiento: 'N/A',
                    programaPresupuestario: item.ProgramaDescripcion || 'N/A',
                    municipio: item.NombreMunicipio || 'N/A',
                    apartados: apartadosMapeados,
                    _original: {
                        saldoDisponible: (item.Modificado || 0) - (item.Ministerio || 0),
                        saldoApartado: 0
                    }
                };
                return detalle;
            }
            throw new Error('Clave no encontrada');
        }

        // =============================================
        // BUSCADOR (UI)
        // =============================================
        const buscador = document.getElementById('buscadorClave');
        const autocompleteDiv = document.getElementById('autocompleteResultados');
        const loadingIndicator = document.getElementById('loadingIndicator');
        let timeoutBusqueda;

        if (buscador) {
            buscador.addEventListener('input', function () {
                clearTimeout(timeoutBusqueda);
                const texto = this.value.trim();
                if (texto.length < 2) {
                    autocompleteDiv.style.display = 'none';
                    return;
                }
                timeoutBusqueda = setTimeout(() => buscar(texto), 300);
            });

            buscador.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    const texto = this.value.trim();
                    if (texto.length >= 2) buscar(texto);
                }
                if (e.key === 'Escape') {
                    autocompleteDiv.style.display = 'none';
                }
            });

            document.addEventListener('keydown', function (e) {
                if (e.key === 'Escape') {
                    autocompleteDiv.style.display = 'none';
                }
            });
        }

        async function buscar(texto) {
            try {
                loadingIndicator.style.display = 'block';
                const resultados = await buscarObras(texto);
                loadingIndicator.style.display = 'none';

                if (resultados && resultados.length > 0) {
                    mostrarAutocompletado(resultados, texto);
                } else {
                    autocompleteDiv.innerHTML = '<div class="autocomplete-item"><span class="text-muted">No se encontraron resultados</span></div>';
                    autocompleteDiv.style.display = 'block';
                }
            } catch (error) {
                console.error('Error en búsqueda:', error);
                loadingIndicator.style.display = 'none';
                autocompleteDiv.style.display = 'none';
            }
        }

        function mostrarAutocompletado(resultados, textoBuscado) {
            let html = '';
            resultados.forEach(item => {
                let nombre = item.ProgramaDescripcion || item.ObjetoGastoDescripcion || item.UrDescripcion || 'Sin descripción';
                if (item.NombreMunicipio && item.NombreMunicipio !== 'Sin municipio') {
                    nombre = item.NombreMunicipio + ' - ' + nombre;
                }

                const nombreResaltado = nombre.replace(new RegExp('(' + escapeRegex(textoBuscado) + ')', 'gi'), '<strong>$1</strong>');
                const montoFormateado = item.Modificado ? '$' + item.Modificado.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : '$0.00';
                const saldoApartado = item.SaldoApartado || 0;
                html += `
                    <div class="autocomplete-item" onclick="seleccionarClave('${item.ClavePresupuestal.replace(/'/g, "\\'")}', '${nombre.replace(/'/g, "\\'")}')">
                        <div class="item-main">
                            ${nombreResaltado}
                            ${saldoApartado > 0 ? '<span class="item-badge" style="background:#f6c23e; color:white;"><i class="bi bi-info-circle"></i> APARTADO</span>' : ''}
                            <br><span style="font-size:0.7rem; color:#999; font-family:monospace;">${item.ClavePresupuestal}</span>
                        </div>
                        <div class="item-meta">
                            ${montoFormateado}
                            <br><span style="font-size:0.6rem; color:#bbb;">${item.UrDescripcion || 'N/A'}</span>
                        </div>
                    </div>
                `;
            });
            autocompleteDiv.innerHTML = html;
            autocompleteDiv.style.display = 'block';
        }

        function escapeRegex(string) { return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); }

        document.addEventListener('click', function (e) {
            if (buscador && !buscador.contains(e.target) && !autocompleteDiv.contains(e.target)) {
                autocompleteDiv.style.display = 'none';
            }
        });

        // =============================================
        // SELECCIÓN DE CLAVE - Cambia a Vista Detalle
        // =============================================
        async function seleccionarClave(clave, nombreObra) {
            if (buscador) buscador.value = nombreObra;
            autocompleteDiv.style.display = 'none';

            // Cambiar a Vista Detalle
            mostrarVistaDetalle();

            Swal.fire({ title: 'Cargando...', allowOutsideClick: false, didOpen: () => { Swal.showLoading(); } });

            try {
                const detalle = await obtenerDetalleClave(clave);
                console.log(' Datos cargados:', detalle);

                claveSeleccionadaActual = clave;
                datosOriginales = JSON.parse(JSON.stringify(detalle));
                datosSimulados = JSON.parse(JSON.stringify(detalle));

                if (detalle.saldoApartado > 0) {
                    datosSimulados.saldoApartado = detalle.saldoApartado;
                    datosSimulados.saldoDisponible = (detalle.modificado || 0) - (detalle.ministrado || 0) - detalle.saldoApartado;
                    estaSimulado = true;
                } else {
                    estaSimulado = false;
                }

                guardarObraFrecuente(clave, detalle.nombre);
                actualizarUIDetalle(datosSimulados);
                Swal.close();
            } catch (error) {
                console.error('❌ Error al cargar:', error);
                Swal.close();
                Swal.fire('Error', 'No se pudo cargar la información: ' + error.message, 'error');
                // Volver a la vista inicio si hay error
                volverAlInicio();
            }
        }

        // =============================================
        // ACTUALIZAR UI DETALLE
        // =============================================
        function actualizarUIDetalle(detalle) {
            document.getElementById('claveNombre').textContent = detalle.nombre || 'Obra sin nombre';
            document.getElementById('claveCompleta').textContent = detalle.clave;
            document.getElementById('montoModificado').textContent = '$' + formatearMonto(detalle.modificado);
            document.getElementById('saldoDisponible').textContent = '$' + formatearMonto(detalle.saldoDisponible);
            document.getElementById('saldoApartado').textContent = '$' + formatearMonto(detalle.saldoApartado);
            document.getElementById('porcentajeEjecucion').textContent = detalle.porcentajeEjecucion + '%';
            document.getElementById('barraProgreso').style.width = detalle.porcentajeEjecucion + '%';
            document.getElementById('sumMinistrado').textContent = '$' + formatearMonto(detalle.ministrado);
            document.getElementById('sumApartado').textContent = '$' + formatearMonto(detalle.saldoApartado);
            document.getElementById('sumDisponible').textContent = '$' + formatearMonto(detalle.saldoDisponible);
            document.getElementById('unidadResponsable').textContent = 'Unidad: ' + detalle.unidadResponsable;
            document.getElementById('programaPresupuestario').textContent = 'Programa: ' + detalle.programaPresupuestario;

            const badge = document.getElementById('badgeSimulacion');
            const btnReset = document.getElementById('btnResetSimulacion');
            if (detalle.saldoApartado > 0) {
                if (btnReset) {
                    btnReset.classList.remove('d-none');
                    btnReset.style.display = 'inline-flex';
                }
                document.getElementById('simulacionInfo').textContent = '⚠️ Apartados activos';
                
                // Mostrar lista
                const container = document.getElementById('listaApartadosContainer');
                const body = document.getElementById('listaApartadosBody');
                if(container && body) {
                    container.classList.remove('d-none');
                    let html = '';
                    if(detalle.apartados && detalle.apartados.length > 0) {
                        detalle.apartados.forEach(ap => {
                            html += `
                            <div class="d-flex justify-content-between align-items-center p-3 border rounded" style="background: rgba(255, 140, 0, 0.05); border-color: rgba(255,140,0,0.2) !important;">
                                <div>
                                    <h6 class="mb-1 text-warning">$${formatearMonto(ap.monto)}</h6>
                                    <p class="text-sm mb-0 text-muted"><strong>Oficio:</strong> ${ap.oficio || 'S/N'} | <strong>Motivo:</strong> ${ap.motivo || 'N/A'}</p>
                                </div>
                                <div>
                                    <button class="btn btn-sm btn-danger mb-0" onclick="cancelarApartadoIndividual(${ap.id})"><i class="bi bi-trash"></i> Cancelar</button>
                                </div>
                            </div>`;
                        });
                    }
                    body.innerHTML = html;
                }

            } else {
                if (badge) badge.style.display = 'none';
                if (btnReset) {
                    btnReset.classList.add('d-none');
                    btnReset.style.display = 'none';
                }
                document.getElementById('simulacionInfo').textContent = '';
                
                const container = document.getElementById('listaApartadosContainer');
                if(container) container.classList.add('d-none');
            }

            actualizarGrafico(detalle);
        }

        function formatearMonto(monto) { return monto.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }

        // =============================================
        // GRÁFICO 3D
        // =============================================
        function actualizarGrafico(detalle) {
            if (graficoActual) graficoActual.destroy();

            const chartContainer = document.getElementById('piechart_3d');
            if (!chartContainer) return;

            const total = detalle.ministrado + detalle.saldoApartado + detalle.saldoDisponible;
            const welcomeState = document.getElementById('welcomeState');
            
            if (total === 0) {
                document.getElementById('piechart_3d').style.display = 'none';
                if (welcomeState) welcomeState.style.display = 'flex';
                return;
            }

            document.getElementById('piechart_3d').style.display = 'block';
            if (welcomeState) welcomeState.style.display = 'none';

            graficoActual = Highcharts.chart('piechart_3d', {
                chart: {
                    type: 'pie',
                    backgroundColor: 'transparent',
                    options3d: { enabled: true, alpha: 45, beta: 0 }
                },
                title: { text: null },
                tooltip: { pointFormat: '<b>$ {point.y:,.2f} MXN</b><br/>{point.percentage:.1f}%' },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 45,
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b><br/>{point.percentage:.1f}%',
                            style: { fontSize: '10px', color: '#444', fontWeight: 'normal' }
                        },
                        showInLegend: false
                    }
                },
                legend: { enabled: false },
                series: [{
                    type: 'pie',
                    name: 'Presupuesto',
                    data: (function() {
                        let d = [];
                        if (detalle.ministrado > 0) d.push({ name: 'Monto Ministrado', y: detalle.ministrado, color: '#9F2241' });
                        if (detalle.saldoApartado > 0) {
                            if (detalle.apartados && detalle.apartados.length > 0) {
                                detalle.apartados.forEach((ap, idx) => {
                                    d.push({ name: 'Apartado: ' + (ap.oficio || ('#' + (idx + 1))), y: ap.monto, color: '#FF8C00' });
                                });
                            } else {
                                d.push({ name: 'Saldo Apartado', y: detalle.saldoApartado, color: '#FF8C00' });
                            }
                        }
                        if (detalle.saldoDisponible > 0) d.push({ name: 'Saldo Disponible', y: detalle.saldoDisponible, color: '#235B4E' });
                        return d;
                    })()
                }],
                credits: { enabled: false }
            });
        }

        // =============================================
        // OBRAS FRECUENTES
        // =============================================
        function guardarObraFrecuente(clave, nombre) {
            let frecuentes = JSON.parse(localStorage.getItem('obrasFrecuentes')) || [];
            frecuentes = frecuentes.filter(item => item.clave !== clave);
            frecuentes.unshift({ clave: clave, nombre: nombre, fecha: new Date().toISOString() });
            frecuentes = frecuentes.slice(0, 5);
            localStorage.setItem('obrasFrecuentes', JSON.stringify(frecuentes));
            mostrarObrasFrecuentes();
        }

        function mostrarObrasFrecuentes() {
            const frecuentes = JSON.parse(localStorage.getItem('obrasFrecuentes')) || [];
            const container = document.getElementById('obrasFrecuentesContainer');
            const lista = document.getElementById('listaObrasFrecuentes');
            const vacio = document.getElementById('recientesVacio');
            if (!container || !lista) return;

            if (frecuentes.length > 0) {
                container.style.display = 'block';
                if (vacio) vacio.style.display = 'none';
                lista.innerHTML = frecuentes.map(item => `
                    <span class="badge-frecuente" onclick="seleccionarClave('${item.clave.replace(/'/g, "\\'")}', '${item.nombre.replace(/'/g, "\\'")}')">
                        ${item.nombre.length > 30 ? item.nombre.substring(0, 30) + '...' : item.nombre}
                        ${item.saldoApartado > 0 ? ' <i class="bi bi-info-circle text-warning" style="font-size:0.8rem;" title="Apartado Activo"></i>' : ''}
                    </span>
                `).join('');
            } else { 
                container.style.display = 'none'; 
                if (vacio) vacio.style.display = 'block';
            }
        }

        // =============================================
        // SIMULAR APARTADO
        // =============================================
        let modalAdecuacionInstance;

        function abrirModalAdecuacion() {
            if (!claveSeleccionadaActual) {
                Swal.fire({
                    title: 'Selecciona una obra',
                    text: 'Primero busca y selecciona una clave presupuestal para simular un apartado.',
                    icon: 'info',
                    confirmButtonColor: '#9F2241',
                    confirmButtonText: 'Entendido'
                });
                return;
            }

            const saldo = document.getElementById('saldoDisponible').textContent;
            document.getElementById('modalClaveActual').value = claveSeleccionadaActual;
            document.getElementById('modalClaveTexto').value = claveSeleccionadaActual;
            document.getElementById('saldoDisponibleModal').textContent = 'Saldo disponible: ' + saldo;
            document.getElementById('montoApartar').value = '';
            document.getElementById('motivoApartado').value = '';
            document.getElementById('numOficio').value = '';

            if (!modalAdecuacionInstance) {
                modalAdecuacionInstance = new bootstrap.Modal(document.getElementById('modalAdecuacion'));
            }
            modalAdecuacionInstance.show();
        }

        function simularApartado() {
            const monto = parseFloat(document.getElementById('montoApartar').value);
            const motivo = document.getElementById('motivoApartado').value.trim();
            const oficio = document.getElementById('numOficio').value.trim() || "SIM-" + Date.now();
            const clave = document.getElementById('modalClaveActual').value;

            if (isNaN(monto) || monto <= 0) {
                Swal.fire('Error', 'Ingresa un monto válido mayor a 0', 'error');
                return;
            }
            if (!motivo) {
                Swal.fire('Error', 'Debes proporcionar un motivo para la solicitud', 'error');
                return;
            }

            const saldoDisponibleActual = datosSimulados ? datosSimulados.saldoDisponible : 0;
            if (monto > saldoDisponibleActual) {
                Swal.fire('Error', 'El monto excede el saldo disponible', 'error');
                return;
            }

            const payload = { Clave: clave, Monto: parseFloat(monto), Motivo: motivo, Oficio: oficio };

            fetch('/api/presupuesto/apartar', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            })
            .then(async res => {
                if (!res.ok) {
                    const text = await res.text();
                    throw new Error(text || 'Error desconocido');
                }
                return res.json();
            })
            .then(data => {
                modalAdecuacionInstance.hide();
                Swal.fire({
                    title: 'Apartado Registrado',
                    text: 'El monto ha sido apartado correctamente por 7 días.',
                    icon: 'success',
                    confirmButtonColor: '#9F2241'
                }).then(async () => {
                    await cargarTodasLasClaves();
                    actualizarEstadisticasGenerales();
                    const nombreObra = document.getElementById('claveNombre').textContent;
                    seleccionarClave(clave, nombreObra);
                });
            })
            .catch(err => {
                console.error(err);
                Swal.fire('Error', err.message.replace(/"/g, ''), 'error');
            });
        }

        // =============================================
        // CANCELAR APARTADO
        // =============================================
        function resetearSimulacion() {
            Swal.fire({
                title: '¿Cancelar TODOS los apartados?',
                text: 'Esto liberará todo el saldo reservado para esta obra en la base de datos.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, cancelar todos',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {
                    fetch('/api/presupuesto/cancelar-apartado', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ Clave: claveSeleccionadaActual })
                    })
                    .then(res => res.json())
                    .then(() => {
                        Swal.fire('Cancelados', 'Todos los apartados han sido liberados.', 'success').then(async () => {
                            await cargarTodasLasClaves(); // Recarga la info real del backend
                            actualizarEstadisticasGenerales(); // Actualiza los números del resumen general
                            const nombreObra = document.getElementById('claveNombre').textContent;
                            seleccionarClave(claveSeleccionadaActual, nombreObra); // Recarga la tarjeta y la gráfica actual
                        });
                    })
                    .catch(err => {
                        Swal.fire('Error', 'No se pudieron cancelar los apartados.', 'error');
                    });
                }
            });
        }

        function cancelarApartadoIndividual(id) {
            Swal.fire({
                title: '¿Cancelar este apartado?',
                text: 'El monto reservado volverá al saldo disponible.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, cancelar',
                cancelButtonText: 'No'
            }).then((result) => {
                if (result.isConfirmed) {
                    fetch('/api/presupuesto/cancelar-apartado-id', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({ Id: id })
                    })
                    .then(res => res.json())
                    .then(() => {
                        Swal.fire('Cancelado', 'El apartado ha sido liberado.', 'success').then(async () => {
                            await cargarTodasLasClaves();
                            actualizarEstadisticasGenerales();
                            const nombreObra = document.getElementById('claveNombre').textContent;
                            seleccionarClave(claveSeleccionadaActual, nombreObra);
                        });
                    })
                    .catch(err => {
                        Swal.fire('Error', 'No se pudo cancelar el apartado.', 'error');
                    });
                }
            });
        }
    </script>
</body>
</html>
