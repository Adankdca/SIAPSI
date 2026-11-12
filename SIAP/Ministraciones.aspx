<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ministraciones.aspx.cs" Inherits="SIAP.Ministraciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="apple-touch-icon" sizes="76x76" href="assets/img/apple-icon.png" />
    <link rel="icon" type="image/png" href="assets/img/favicon.png" />
    <title>MINISTRACIONES | SIAP</title>

    <!-- Fonts and icons -->
    <link href="assets/css/googleapis.css" rel="stylesheet" />
    <link href="assets/css/nucleo-icons.css" rel="stylesheet" />
    <link href="assets/css/nucleo-svg.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.1/font/bootstrap-icons.min.css" />
    <script src="assets/js/kitfontaweson.js" crossorigin="anonymous"></script>

    <!-- Inter Font - Google Fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Inter:opsz,wght@14..32,300;14..32,400;14..32,500;14..32,600;14..32,700&display=swap" rel="stylesheet" />

    <!-- CSS Files -->
    <script src="assets/js/jquery-3.5.1.js"></script>
    <link id="pagestyle" href="assets/css/argon-dashboard.css?v=2.0.0" rel="stylesheet" />
    <link href="assets/css/gobierno-theme.css" rel="stylesheet" />
    <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/animate.css/animate.css" />
    <link rel="stylesheet" href="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.min.css" />
    <script src="assets/js/SweetAlert2/popper/popper.min.js"></script>

    <style>
        /* ===== ESTILOS EXISTENTES (MANTENIDOS) ===== */
        .jm-loadingpage {
            display: none;
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('assetsDU/wait.gif') 50% 50% no-repeat rgb(249,249,249);
            opacity: .40;
        }

        .btn2 {
            display: inline-block;
            font-weight: 700;
            line-height: 1.5;
            color: #67748e;
            text-align: center;
            vertical-align: middle;
            cursor: pointer;
            user-select: none;
            background-color: transparent;
            border: 1px solid transparent;
            padding: 0.425rem 0.65rem;
            font-size: 0.875rem;
            border-radius: 0.5rem;
            transition: all 0.15s ease-in;
        }

        .modal-lg {
            max-width: 85% !important;
        }

        .modal-sm {
            max-width: 50% !important;
        }

        .modal-xl {
            max-width: 95% !important;
        }

        .celda-spinner {
            position: relative;
        }

            .celda-spinner::after {
                content: "";
                position: absolute;
                right: 5px;
                top: 50%;
                width: 14px;
                height: 14px;
                margin-top: -7px;
                border: 2px solid #fff;
                border-top-color: #007bff;
                border-radius: 50%;
                animation: spin 0.7s linear infinite;
            }

        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }

        /* ===== STATS GRID ===== */
        .stats-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 12px;
            margin-bottom: 20px;
        }

        @media (max-width: 992px) {
            .stats-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 576px) {
            .stats-grid {
                grid-template-columns: 1fr;
            }
        }

        .stat-card {
            background: white;
            border-radius: 12px;
            padding: 14px 16px;
            border: 1px solid #e8e8e8;
            transition: all 0.3s ease;
            display: flex;
            align-items: flex-start;
            gap: 12px;
        }

            .stat-card:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 20px rgba(0, 0, 0, 0.06);
                border-color: #9F2241;
            }

            .stat-card .stat-icon {
                font-size: 1.3rem;
                color: #9F2241;
                opacity: 0.8;
                width: 38px;
                height: 38px;
                background: rgba(159, 34, 65, 0.06);
                border-radius: 10px;
                display: flex;
                align-items: center;
                justify-content: center;
                flex-shrink: 0;
                margin-top: 2px;
            }

            .stat-card .stat-info {
                flex: 1;
                min-width: 0;
            }

            .stat-card .stat-number {
                font-size: 1.3rem;
                font-weight: 700;
                color: #1a1a1a;
                line-height: 1.2;
            }

            .stat-card .stat-label {
                font-size: 0.65rem;
                color: #6c757d;
                text-transform: uppercase;
                letter-spacing: 0.3px;
                font-weight: 600;
            }

            .stat-card .stat-details {
                margin-top: 4px;
                display: flex;
                flex-wrap: wrap;
                gap: 3px;
            }

                .stat-card .stat-details .badge-status {
                    font-size: 0.55rem;
                    padding: 1px 8px;
                    border-radius: 12px;
                    font-weight: 500;
                }

                    .stat-card .stat-details .badge-status.registrada {
                        background: #e9ecef;
                        color: #495057;
                    }

                    .stat-card .stat-details .badge-status.iniciado {
                        background: #fff3cd;
                        color: #856404;
                    }

                    .stat-card .stat-details .badge-status.cancelada {
                        background: #f8d7da;
                        color: #721c24;
                    }

                .stat-card .stat-details .badge-tipo {
                    font-size: 0.55rem;
                    padding: 1px 8px;
                    border-radius: 12px;
                    font-weight: 500;
                }

                    .stat-card .stat-details .badge-tipo.etiquetado {
                        background: #d4edda;
                        color: #155724;
                    }

                    .stat-card .stat-details .badge-tipo.no-etiquetado {
                        background: #e9ecef;
                        color: #495057;
                    }

            .stat-card .stat-trend {
                font-size: 0.6rem;
                color: #28a745;
                background: #e8f5e9;
                padding: 1px 10px;
                border-radius: 50px;
                display: inline-block;
                font-weight: 500;
            }

                .stat-card .stat-trend.negativo {
                    color: #dc3545;
                    background: #fbe9eb;
                }

                .stat-card .stat-trend.neutral {
                    color: #6c757d;
                    background: #e9ecef;
                }

        #detalleDiferencia {
            font-size: 0.6rem;
            font-weight: 500;
        }

            #detalleDiferencia .diferencia-ok {
                color: #28a745;
            }

            #detalleDiferencia .diferencia-error {
                color: #dc3545;
            }

            #detalleDiferencia .diferencia-cero {
                color: #6c757d;
            }

        /* ===== TABLA ===== */
        .table-mini {
            font-size: 0.8rem;
        }

            .table-mini th {
                background: #f8f9fa;
                font-weight: 600;
                text-transform: uppercase;
                font-size: 0.65rem;
                letter-spacing: 0.5px;
                padding: 8px 6px;
                white-space: nowrap;
            }

            .table-mini td {
                padding: 6px 6px;
                vertical-align: middle;
            }

        .badge-status {
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 0.7rem;
            font-weight: 600;
        }

            .badge-status.registrada {
                background: #e9ecef;
                color: #495057;
            }

            .badge-status.iniciado {
                background: #fff3cd;
                color: #856404;
            }

            .badge-status.cancelada {
                background: #f8d7da;
                color: #721c24;
            }

        .clave-presupuestal {
            font-family: 'Courier New', monospace;
            font-size: 0.7rem;
            background: #f1f3f5;
            padding: 2px 8px;
            border-radius: 4px;
            word-break: break-all;
        }

        .btn-accion {
            padding: 6px 10px;
            font-size: 0.75rem;
            border-radius: 8px;
            margin: 2px;
        }

        .modal-header-custom {
            background: #9F2241;
            color: white;
            border-radius: 12px 12px 0 0;
        }

        .pagination-custom .page-item .page-link {
            color: #1a1a2e;
            border-radius: 8px;
            margin: 0 3px;
        }

        .pagination-custom .page-item.active .page-link {
            background: #1a1a2e;
            border-color: #1a1a2e;
            color: white;
        }

        /* ===== SEARCH ===== */
        .search-wrapper {
            position: relative;
        }

            .search-wrapper .search-input {
                padding-left: 42px;
                padding-right: 16px;
                height: 38px;
                font-size: 0.85rem;
                border-radius: 10px;
                border: 2px solid #e8e8e8;
                transition: all 0.3s ease;
                background: #f8f9fa;
                width: 100%;
            }

                .search-wrapper .search-input:focus {
                    border-color: #9F2241;
                    background: white;
                    box-shadow: 0 0 0 3px rgba(159, 34, 65, 0.08);
                    outline: none;
                }

                .search-wrapper .search-input::placeholder {
                    color: #adb5bd;
                    font-weight: 300;
                }

            .search-wrapper .search-icon-left {
                position: absolute;
                left: 14px;
                top: 50%;
                transform: translateY(-50%);
                color: #adb5bd;
                font-size: 1rem;
                transition: color 0.3s ease;
                pointer-events: none;
            }

            .search-wrapper .search-input:focus + .search-icon-left,
            .search-wrapper .search-input:not(:placeholder-shown) + .search-icon-left {
                color: #9F2241;
            }

            .search-wrapper .search-shortcut {
                position: absolute;
                right: 12px;
                top: 50%;
                transform: translateY(-50%);
                background: #e8e8e8;
                padding: 2px 10px;
                border-radius: 4px;
                font-size: 0.6rem;
                color: #999;
                font-weight: 600;
                letter-spacing: 0.5px;
            }

        /* ===== AUTOCOMPLETE ===== */
        .autocomplete-items {
            position: absolute;
            top: calc(100% + 6px);
            left: 0;
            right: 0;
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.12);
            max-height: 320px;
            overflow-y: auto;
            z-index: 1000;
            border: 1px solid #f0f0f0;
        }

        .autocomplete-item {
            padding: 10px 16px;
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

                .autocomplete-item .item-main .item-folio {
                    font-size: 0.75rem;
                    color: #666;
                }

                .autocomplete-item .item-main .item-clave {
                    font-family: monospace;
                    font-size: 0.65rem;
                    color: #999;
                    background: #f5f5f5;
                    padding: 1px 6px;
                    border-radius: 3px;
                    display: inline-block;
                    margin-top: 2px;
                }

            .autocomplete-item .item-meta {
                font-size: 0.7rem;
                color: #999;
                text-align: right;
                flex-shrink: 0;
                margin-left: 12px;
            }

        .search-hint {
            font-size: 0.65rem;
            color: #adb5bd;
            margin-top: 4px;
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
        }

            .search-hint .hint-item {
                display: flex;
                align-items: center;
                gap: 4px;
            }

                .search-hint .hint-item i {
                    font-size: 0.7rem;
                    color: #9F2241;
                }

            .search-hint kbd {
                background: #f5f5f5;
                padding: 1px 8px;
                border-radius: 3px;
                font-size: 0.55rem;
                color: #666;
                border: 1px solid #e8e8e8;
            }

        /* ===== CARD HEADER ===== */
        .card-header-custom {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 10px;
            padding-bottom: 0;
        }

            .card-header-custom .header-left {
                display: flex;
                align-items: center;
                gap: 12px;
            }

                .card-header-custom .header-left .header-icon {
                    font-size: 1.5rem;
                    color: #9F2241;
                    background: rgba(159, 34, 65, 0.08);
                    width: 44px;
                    height: 44px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border-radius: 10px;
                }

                .card-header-custom .header-left .header-text h6 {
                    margin: 0;
                    font-weight: 700;
                    color: #1a1a1a;
                }

                .card-header-custom .header-left .header-text p {
                    margin: 0;
                    font-size: 0.8rem;
                    color: #6c757d;
                }

            .card-header-custom .header-actions {
                display: flex;
                align-items: center;
                gap: 12px;
                flex-wrap: wrap;
            }

        .header-badge {
            background: #9F2241;
            color: white;
            padding: 4px 14px;
            border-radius: 50px;
            font-size: 0.7rem;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 6px;
            white-space: nowrap;
        }

        .clave-badge {
            display: inline-block;
            font-family: monospace;
            font-size: 0.6rem;
            background: #f1f3f5;
            padding: 1px 6px;
            border-radius: 3px;
            margin: 1px 2px;
            color: #333;
            border: 1px solid #e8e8e8;
            max-width: 200px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

            .clave-badge:hover {
                background: #e9ecef;
                border-color: #9F2241;
            }

        .claves-container {
            display: flex;
            flex-wrap: wrap;
            gap: 2px;
            max-width: 250px;
        }

        .btn-info:hover {
            background-color: #115eefbd;
        }

        .btn-info {
            background-color: #115eefbd
        }

        .btn-cancelar {
            background: #dc3545;
            color: white;
            border: none;
        }

            .btn-cancelar:hover {
                background: #c82333;
                color: white;
            }

            .btn-cancelar:disabled {
                opacity: 0.5;
                cursor: not-allowed;
            }

        /* ===== MODAL CLAVES ===== */
        .modal-claves .modal-body {
            padding: 0.5rem 1rem;
        }

        .modal-claves .table-claves {
            font-size: 0.75rem;
        }

            .modal-claves .table-claves th {
                font-size: 0.65rem;
                padding: 4px 6px;
            }

            .modal-claves .table-claves td {
                padding: 4px 6px;
                vertical-align: middle;
            }

        .modal-claves .clave-item {
            font-family: monospace;
            font-size: 0.7rem;
            background: #f8f9fa;
            padding: 2px 8px;
            border-radius: 4px;
            cursor: pointer;
            transition: all 0.2s;
        }

            .modal-claves .clave-item:hover {
                background: #9F2241;
                color: white;
            }

            .modal-claves .clave-item.seleccionada {
                background: #28a745;
                color: white;
            }

        /* ===== DETALLE ===== */
        .detalle-mini .card {
            border: none;
            background: #f8f9fa;
            border-radius: 8px;
            margin-bottom: 6px;
        }

            .detalle-mini .card .card-body {
                padding: 8px 12px;
            }

                .detalle-mini .card .card-body h6 {
                    font-size: 0.6rem;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    color: #6c757d;
                    margin-bottom: 2px;
                }

                .detalle-mini .card .card-body p {
                    font-size: 0.8rem;
                    margin: 0;
                    font-weight: 500;
                }

                .detalle-mini .card .card-body .h5 {
                    font-size: 0.95rem;
                }

        .detalle-mini .badge-status {
            font-size: 0.6rem;
            padding: 3px 10px;
        }

        .detalle-mini .clave-presupuestal {
            font-size: 0.65rem;
        }

        .detalle-mini .table {
            font-size: 0.7rem;
        }

            .detalle-mini .table th {
                font-size: 0.6rem;
                padding: 4px 6px;
            }

            .detalle-mini .table td {
                padding: 4px 6px;
            }

        .btn-buscar-claves {
            background: #17a2b8;
            color: white;
            border-radius: 8px;
            padding: 4px 12px;
            font-size: 0.7rem;
            border: none;
            transition: all 0.3s;
        }

            .btn-buscar-claves:hover {
                background: #138496;
                color: white;
            }

        .campo-clave {
            font-family: monospace;
            font-size: 0.75rem;
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            padding: 4px 8px;
            width: 100%;
            cursor: default;
        }

        .comparacion-card {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 12px 16px;
            margin-top: 10px;
        }

            .comparacion-card .comparacion-item {
                display: flex;
                justify-content: space-between;
                padding: 4px 0;
                font-size: 0.8rem;
            }

                .comparacion-card .comparacion-item .label {
                    color: #6c757d;
                }

                .comparacion-card .comparacion-item .value {
                    font-weight: 600;
                }

                .comparacion-card .comparacion-item.diferencia .value {
                    font-weight: 700;
                }

                .comparacion-card .comparacion-item.diferencia.positiva .value {
                    color: #28a745;
                }

                .comparacion-card .comparacion-item.diferencia.negativa .value {
                    color: #dc3545;
                }

                .comparacion-card .comparacion-item.diferencia.cero .value {
                    color: #6c757d;
                }

        .comparacion-divider {
            border-top: 1px dashed #dee2e6;
            margin: 6px 0;
        }

        .badge-validacion {
            padding: 3px 12px;
            border-radius: 50px;
            font-size: 0.65rem;
            font-weight: 600;
        }

            .badge-validacion.ok {
                background: #d4edda;
                color: #155724;
            }

            .badge-validacion.error {
                background: #f8d7da;
                color: #721c24;
            }

        .btn-pdf {
            background: #dc3545;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 4px 12px;
            font-size: 0.7rem;
        }

            .btn-pdf:hover {
                background: #c82333;
                color: white;
            }

            .btn-pdf:disabled {
                opacity: 0.4;
                cursor: not-allowed;
            }

            .btn-pdf.sin-pdf {
                background: #6c757d;
            }

                .btn-pdf.sin-pdf:hover {
                    background: #5a6268;
                }

        .clave-agrupada {
            font-family: monospace;
            font-size: 0.65rem;
            background: #e9ecef;
            padding: 2px 6px;
            border-radius: 3px;
            display: inline-block;
            margin: 1px;
        }

        .campo-lectura {
            background: #e9ecef !important;
            cursor: not-allowed;
        }

        .form-label-sm {
            font-size: 0.7rem;
            font-weight: 600;
            color: #495057;
            margin-bottom: 2px;
        }

        .form-control-sm,
        .form-select-sm {
            font-size: 0.8rem;
        }

        .badge-tipo {
            font-size: 0.6rem;
            padding: 2px 8px;
            border-radius: 4px;
        }

            .badge-tipo.etiquetado {
                background: #d4edda;
                color: #155724;
            }

            .badge-tipo.no-etiquetado {
                background: #e9ecef;
                color: #495057;
            }

        .detalle-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 8px;
            margin-bottom: 10px;
        }

            .detalle-grid .detalle-item {
                background: #f8f9fa;
                border-radius: 6px;
                padding: 6px 10px;
            }

                .detalle-grid .detalle-item .label {
                    font-size: 0.55rem;
                    text-transform: uppercase;
                    letter-spacing: 0.5px;
                    color: #6c757d;
                }

                .detalle-grid .detalle-item .value {
                    font-size: 0.85rem;
                    font-weight: 600;
                    word-break: break-all;
                }

                    .detalle-grid .detalle-item .value.clave {
                        font-family: monospace;
                        font-size: 0.7rem;
                        background: white;
                        padding: 2px 6px;
                        border-radius: 4px;
                    }

        /* ===== NUEVOS ESTILOS ===== */
        .monto-ministrado {
            font-weight: 700;
            color: #28a745;
        }

        .monto-presupuesto {
            font-weight: 600;
            color: #6c757d;
        }

        .campo-info-programa {
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            padding: 4px 8px;
            font-size: 0.75rem;
            font-family: monospace;
            min-height: 31px;
        }

        /* ===== ESTILOS PARA EL MODAL DE COMPARACIÓN ===== */
        .comparacion-totales {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 12px 16px;
            margin-top: 10px;
            border: 2px solid #dee2e6;
        }

            .comparacion-totales .total-item {
                display: flex;
                justify-content: space-between;
                padding: 4px 0;
                font-size: 0.9rem;
            }

                .comparacion-totales .total-item .label {
                    color: #6c757d;
                    font-weight: 500;
                }

                .comparacion-totales .total-item .value {
                    font-weight: 700;
                }

                .comparacion-totales .total-item.diferencia .value {
                    font-size: 1.1rem;
                }

                .comparacion-totales .total-item.diferencia.positiva .value {
                    color: #28a745;
                }

                .comparacion-totales .total-item.diferencia.negativa .value {
                    color: #dc3545;
                }

                .comparacion-totales .total-item.diferencia.cero .value {
                    color: #6c757d;
                }

            .comparacion-totales .badge-estado {
                font-size: 0.7rem;
                padding: 4px 14px;
                border-radius: 50px;
            }

                .comparacion-totales .badge-estado.ok {
                    background: #d4edda;
                    color: #155724;
                }

                .comparacion-totales .badge-estado.error {
                    background: #f8d7da;
                    color: #721c24;
                }

                .comparacion-totales .badge-estado.info {
                    background: #d1ecf1;
                    color: #0c5460;
                }

        .comparacion-divider {
            border-top: 1px dashed #dee2e6;
            margin: 6px 0;
        }

        /* ============================================================
    FILTROS CON DROPDOWNS PERSONALIZADOS
    ============================================================ */
        .filter-section {
            background: linear-gradient(145deg, #ffffff 0%, #fafafa 100%);
            padding: 20px 24px 16px 24px;
            margin-bottom: 20px;
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.04), 0 1px 2px rgba(0, 0, 0, 0.03);
            transition: all 0.3s ease;
        }

        .filter-container {
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .filter-row {
            display: grid;
            grid-template-columns: 1fr 1fr 1fr 1fr 1fr auto;
            gap: 12px;
            align-items: end;
        }

            .filter-row.search-row {
                grid-template-columns: 1fr;
                margin-top: 4px;
            }

        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 4px;
            min-width: 0;
        }

        .filter-label {
            display: flex;
            align-items: center;
            gap: 6px;
            font-size: 0.65rem;
            font-weight: 600;
            color: #495057;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 0 4px;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }

            .filter-label i {
                font-size: 0.7rem;
                color: #9F2241;
                opacity: 0.8;
            }

        /* ============================================================
    BOTÓN AGREGAR
    ============================================================ */
        .filter-action {
            display: flex;
            flex-direction: column;
            justify-content: flex-end;
        }

        .btn-add {
            background: linear-gradient(135deg, #9F2241 0%, #7a1a32 100%);
            border: none;
            border-radius: 10px;
            color: #ffffff;
            font-size: 0.75rem;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            font-weight: 600;
            height: 40px;
            padding: 0 18px;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            transition: all 0.3s cubic-bezier(0.25, 0.46, 0.45, 0.94);
            cursor: pointer;
            width: 100%;
            white-space: nowrap;
            box-shadow: 0 2px 8px rgba(159, 34, 65, 0.15);
            letter-spacing: 0.3px;
        }

            .btn-add:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 16px rgba(159, 34, 65, 0.25);
                background: linear-gradient(135deg, #b0284a 0%, #8a1d3a 100%);
            }

            .btn-add:active {
                transform: translateY(0px);
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.15);
            }

            .btn-add i {
                font-size: 1rem;
            }

        /* ============================================================
    BUSCADOR DE TEXTO (GLOBAL)
    ============================================================ */
        .search-wrapper-modern {
            position: relative;
            width: 100%;
        }

        .search-input-group {
            position: relative;
            display: flex;
            align-items: center;
            background: #f8f9fa;
            border: 1.5px solid #e9ecef;
            border-radius: 10px;
            transition: all 0.3s cubic-bezier(0.25, 0.46, 0.45, 0.94);
        }

            .search-input-group:focus-within {
                background: #ffffff;
                border-color: #9F2241;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.06), 0 2px 12px rgba(159, 34, 65, 0.06);
            }

        .search-icon {
            position: absolute;
            left: 14px;
            color: #adb5bd;
            font-size: 0.95rem;
            transition: all 0.3s ease;
            pointer-events: none;
            z-index: 1;
        }

        .search-input-group:focus-within .search-icon {
            color: #9F2241;
        }

        .search-input-modern {
            width: 100%;
            border: none;
            background: transparent;
            padding: 0.5rem 1rem 0.5rem 42px;
            height: 40px;
            font-size: 0.85rem;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            font-weight: 400;
            color: #212529;
            border-radius: 10px;
            outline: none;
            transition: all 0.3s ease;
        }

            .search-input-modern::placeholder {
                color: #adb5bd;
                font-weight: 300;
                font-size: 0.8rem;
                font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

        .search-shortcut-modern {
            position: absolute;
            right: 14px;
            background: rgba(159, 34, 65, 0.06);
            color: #9F2241;
            font-size: 0.55rem;
            font-weight: 600;
            padding: 2px 10px;
            border-radius: 6px;
            letter-spacing: 0.5px;
            border: 1px solid rgba(159, 34, 65, 0.10);
            pointer-events: none;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }

        /* ============================================================
    HINTS DE BÚSQUEDA
    ============================================================ */
        .search-hints {
            display: flex;
            gap: 6px;
            margin-top: 6px;
            flex-wrap: wrap;
            padding: 0 4px;
        }

        .hint-badge {
            display: inline-flex;
            align-items: center;
        }

            .hint-badge kbd {
                background: #f1f3f5;
                padding: 1px 10px;
                border-radius: 4px;
                font-size: 0.55rem;
                font-weight: 600;
                color: #6c757d;
                border: 1px solid #e9ecef;
                transition: all 0.2s ease;
                font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            }

                .hint-badge kbd:hover {
                    background: #e9ecef;
                    border-color: #9F2241;
                    color: #9F2241;
                }

        /* ============================================================
    AUTOCOMPLETE MODERNO
    ============================================================ */
        .autocomplete-modern {
            position: absolute;
            top: calc(100% + 8px);
            left: 0;
            right: 0;
            background: #ffffff;
            border-radius: 12px;
            box-shadow: 0 12px 48px rgba(0, 0, 0, 0.10), 0 2px 8px rgba(0, 0, 0, 0.04);
            max-height: 340px;
            overflow-y: auto;
            z-index: 1050;
            border: 1px solid rgba(0, 0, 0, 0.04);
            padding: 4px;
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
        }

            .autocomplete-modern .autocomplete-item {
                padding: 10px 14px;
                cursor: pointer;
                border-radius: 8px;
                transition: all 0.15s ease;
                display: flex;
                justify-content: space-between;
                align-items: center;
                border-bottom: 1px solid #f5f5f5;
            }

                .autocomplete-modern .autocomplete-item:last-child {
                    border-bottom: none;
                }

                .autocomplete-modern .autocomplete-item:hover {
                    background: linear-gradient(135deg, #faf0f3 0%, #f5e8ec 100%);
                }

                .autocomplete-modern .autocomplete-item .item-main {
                    flex: 1;
                    min-width: 0;
                }

                    .autocomplete-modern .autocomplete-item .item-main strong {
                        color: #9F2241;
                        font-weight: 700;
                        font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                    }

                    .autocomplete-modern .autocomplete-item .item-main .item-folio {
                        font-size: 0.75rem;
                        color: #666;
                    }

                    .autocomplete-modern .autocomplete-item .item-main .clave-badge {
                        font-family: 'Courier New', monospace;
                        font-size: 0.65rem;
                        background: #f1f3f5;
                        padding: 1px 8px;
                        border-radius: 4px;
                        color: #495057;
                        border: 1px solid #e9ecef;
                        display: inline-block;
                        max-width: 200px;
                        overflow: hidden;
                        text-overflow: ellipsis;
                        white-space: nowrap;
                    }

                .autocomplete-modern .autocomplete-item .item-meta {
                    font-size: 0.7rem;
                    color: #6c757d;
                    text-align: right;
                    flex-shrink: 0;
                    margin-left: 12px;
                    font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                }

        /* ============================================================
    FILTROS CON BUSCADOR INTEGRADO 
    ============================================================ */

        /* Contenedor del filtro con buscador */
        .filter-group {
            display: flex;
            flex-direction: column;
            gap: 4px;
            min-width: 0;
            position: relative;
        }

        .filter-label {
            display: flex;
            align-items: center;
            gap: 6px;
            font-size: 0.65rem;
            font-weight: 600;
            color: #495057;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 0 4px;
            font-family: 'Inter', sans-serif;
        }

            .filter-label i {
                font-size: 0.7rem;
                color: #9F2241;
                opacity: 0.8;
            }

        /* ============================================================
    SELECT NATIVO CON BUSCADOR INTEGRADO
    ============================================================ */

        /* El contenedor que simula un select con buscador */
        .select-search-wrapper {
            position: relative;
            width: 100%;
        }

        /* Input que actúa como select + buscador */
        .select-search-input {
            width: 100%;
            height: 40px;
            min-height: 40px;
            max-height: 40px;
            background: #f8f9fa;
            border: 1.5px solid #e9ecef;
            border-radius: 10px;
            padding: 0.45rem 36px 0.45rem 0.85rem;
            font-size: 0.8rem;
            font-family: 'Inter', sans-serif;
            color: #212529;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.25s ease;
            appearance: none;
            -webkit-appearance: none;
            outline: none;
            box-sizing: border-box;
        }

            .select-search-input:hover {
                background: #ffffff;
                border-color: #9F2241;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08);
            }

            .select-search-input:focus {
                background: #ffffff;
                border-color: #9F2241;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10);
                outline: none;
            }

            .select-search-input::placeholder {
                color: #adb5bd;
                font-weight: 400;
                font-size: 0.8rem;
            }

        /* Flecha personalizada */
        .select-search-wrapper::after {
            content: '';
            position: absolute;
            right: 14px;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 0;
            border-left: 5px solid transparent;
            border-right: 5px solid transparent;
            border-top: 6px solid #9F2241;
            pointer-events: none;
            transition: transform 0.3s ease;
        }

        .select-search-wrapper.open::after {
            transform: translateY(-50%) rotate(180deg);
        }

        /* ============================================================
    DROPDOWN DE OPCIONES
    ============================================================ */
        .select-search-dropdown {
            display: none;
            position: absolute;
            top: calc(100% + 4px);
            left: 0;
            right: 0;
            background: #ffffff;
            border: 1.5px solid #e9ecef;
            border-radius: 10px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.10);
            z-index: 1050;
            max-height: 280px;
            overflow: hidden;
            min-width: 100%;
        }

            .select-search-dropdown.show {
                display: block;
            }

        /* Lista de opciones */
        .select-search-options {
            list-style: none;
            margin: 0;
            padding: 4px 0;
            max-height: 260px;
            overflow-y: auto;
        }

            .select-search-options li {
                padding: 8px 14px;
                font-size: 0.8rem;
                font-family: 'Inter', sans-serif;
                color: #212529;
                cursor: pointer;
                transition: all 0.15s ease;
                border-bottom: 1px solid #f8f9fa;
            }

                .select-search-options li:last-child {
                    border-bottom: none;
                }

                .select-search-options li:hover {
                    background: #9F2241;
                    color: #ffffff;
                }

                .select-search-options li.selected {
                    background: rgba(159, 34, 65, 0.08);
                    color: #9F2241;
                    font-weight: 600;
                }

                .select-search-options li .highlight {
                    font-weight: 700;
                    color: #9F2241;
                }

                .select-search-options li:hover .highlight {
                    color: #ffffff;
                }

            /* Scroll personalizado */
            .select-search-options::-webkit-scrollbar {
                width: 4px;
            }

            .select-search-options::-webkit-scrollbar-track {
                background: #f1f1f1;
                border-radius: 4px;
            }

            .select-search-options::-webkit-scrollbar-thumb {
                background: #9F2241;
                border-radius: 4px;
            }

        /* ============================================================
    ESTILOS PARA SELECTS EN MODALES (MISMA ESTÉTICA)
    ============================================================ */

        /* Contenedor de buscador en modales */
        .modal .select-search-wrapper {
            position: relative;
            width: 100%;
        }

        .modal .select-search-input {
            width: 100%;
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            background: #f8f9fa;
            border: 1.5px solid #e9ecef;
            border-radius: 10px;
            padding: 0.45rem 36px 0.45rem 0.85rem;
            font-size: 0.8rem;
            font-family: 'Inter', sans-serif;
            color: #212529;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.25s ease;
            appearance: none;
            -webkit-appearance: none;
            outline: none;
            box-sizing: border-box;
        }

            .modal .select-search-input:hover {
                background: #ffffff;
                border-color: #9F2241;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08);
            }

            .modal .select-search-input:focus {
                background: #ffffff;
                border-color: #9F2241;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10);
                outline: none;
            }

        .modal .select-search-wrapper::after {
            content: '';
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 0;
            border-left: 5px solid transparent;
            border-right: 5px solid transparent;
            border-top: 6px solid #9F2241;
            pointer-events: none;
            transition: transform 0.3s ease;
        }

        .modal .select-search-wrapper.open::after {
            transform: translateY(-50%) rotate(180deg);
        }

        .modal .select-search-dropdown {
            display: none;
            position: absolute;
            top: calc(100% + 4px);
            left: 0;
            right: 0;
            background: #ffffff;
            border: 1.5px solid #e9ecef;
            border-radius: 10px;
            box-shadow: 0 8px 30px rgba(0, 0, 0, 0.10);
            z-index: 1060;
            max-height: 200px;
            overflow: hidden;
            min-width: 100%;
        }

            .modal .select-search-dropdown.show {
                display: block;
            }

        .modal .select-search-options {
            list-style: none;
            margin: 0;
            padding: 4px 0;
            max-height: 180px;
            overflow-y: auto;
        }

            .modal .select-search-options li {
                padding: 6px 12px;
                font-size: 0.75rem;
                font-family: 'Inter', sans-serif;
                color: #212529;
                cursor: pointer;
                transition: all 0.15s ease;
                border-bottom: 1px solid #f8f9fa;
            }

                .modal .select-search-options li:last-child {
                    border-bottom: none;
                }

                .modal .select-search-options li:hover {
                    background: #9F2241;
                    color: #ffffff;
                }

                .modal .select-search-options li.selected {
                    background: rgba(159, 34, 65, 0.08);
                    color: #9F2241;
                    font-weight: 600;
                }

        /* ============================================================
    ESTILOS PARA SELECTS NATIVOS EN MODALES
    ============================================================ */

        /* Unificar todos los selects del modal */
        .modal .form-select-sm,
        .modal select.form-select-sm {
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            border-radius: 10px !important;
            border: 1.5px solid #e9ecef !important;
            background: #f8f9fa !important;
            font-size: 0.75rem !important;
            font-family: 'Inter', sans-serif !important;
            color: #212529 !important;
            padding: 0.45rem 36px 0.45rem 0.85rem !important;
            width: 100% !important;
            appearance: none !important;
            -webkit-appearance: none !important;
            cursor: pointer !important;
            font-weight: 500 !important;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8'%3E%3Cpath d='M1 1l5 5 5-5' stroke='%239F2241' stroke-width='1.5' fill='none' stroke-linecap='round'/%3E%3C/svg%3E") !important;
            background-repeat: no-repeat !important;
            background-position: right 12px center !important;
            background-size: 12px !important;
            transition: all 0.25s ease !important;
        }

            .modal .form-select-sm:hover,
            .modal select.form-select-sm:hover {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

            .modal .form-select-sm:focus,
            .modal select.form-select-sm:focus {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10) !important;
                outline: none !important;
            }

            /* Hover en opciones de selects nativos del modal */
            .modal .form-select-sm option:hover,
            .modal .form-select-sm option:focus,
            .modal .form-select-sm option:checked,
            .modal select.form-select-sm option:hover,
            .modal select.form-select-sm option:focus,
            .modal select.form-select-sm option:checked {
                background: #9F2241 !important;
                color: #ffffff !important;
            }

        /* Ajuste para selects que no son .form-select-sm */
        .modal select:not(.select-search-input) {
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            border-radius: 10px !important;
            border: 1.5px solid #e9ecef !important;
            background: #f8f9fa !important;
            font-size: 0.75rem !important;
            font-family: 'Inter', sans-serif !important;
            color: #212529 !important;
            padding: 0.45rem 36px 0.45rem 0.85rem !important;
            width: 100% !important;
            appearance: none !important;
            -webkit-appearance: none !important;
            cursor: pointer !important;
            font-weight: 500 !important;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='8' viewBox='0 0 12 8'%3E%3Cpath d='M1 1l5 5 5-5' stroke='%239F2241' stroke-width='1.5' fill='none' stroke-linecap='round'/%3E%3C/svg%3E") !important;
            background-repeat: no-repeat !important;
            background-position: right 12px center !important;
            background-size: 12px !important;
            transition: all 0.25s ease !important;
        }

            .modal select:not(.select-search-input):hover {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

            .modal select:not(.select-search-input):focus {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10) !important;
                outline: none !important;
            }

            .modal select:not(.select-search-input) option:hover,
            .modal select:not(.select-search-input) option:focus,
            .modal select:not(.select-search-input) option:checked {
                background: #9F2241 !important;
                color: #ffffff !important;
            }

        /* ============================================================
    ESTILOS PARA INPUTS EN MODALES (MISMO FONDO)
    ============================================================ */

        /* Todos los inputs de tipo text, number y los campos de clave */
        .modal .form-control-sm,
        .modal input.form-control-sm,
        .modal .campo-clave,
        .modal input[type="text"],
        .modal input[type="number"] {
            background-color: #f8f9fa !important;
            border: 1.5px solid #e9ecef !important;
            border-radius: 10px !important;
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            font-size: 0.75rem !important;
            font-family: 'Inter', sans-serif !important;
            color: #212529 !important;
            padding: 0.45rem 0.85rem !important;
            transition: all 0.25s ease !important;
            width: 100% !important;
        }

            .modal .form-control-sm:hover,
            .modal input.form-control-sm:hover,
            .modal .campo-clave:hover,
            .modal input[type="text"]:hover,
            .modal input[type="number"]:hover {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

            .modal .form-control-sm:focus,
            .modal input.form-control-sm:focus,
            .modal .campo-clave:focus,
            .modal input[type="text"]:focus,
            .modal input[type="number"]:focus {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10) !important;
                outline: none !important;
            }

            /* Placeholder con color más suave */
            .modal .form-control-sm::placeholder,
            .modal input.form-control-sm::placeholder,
            .modal .campo-clave::placeholder,
            .modal input[type="text"]::placeholder,
            .modal input[type="number"]::placeholder {
                color: #adb5bd !important;
                font-weight: 400 !important;
                font-size: 0.75rem !important;
            }

        /* Campo de clave presupuestal (readonly) */
        .modal .campo-clave {
            cursor: default !important;
            background-color: #f8f9fa !important;
        }

            .modal .campo-clave:hover {
                background-color: #ffffff !important;
            }

        /* Input de filtro clave en el modal de claves */
        #txtFiltroClave {
            background-color: #f8f9fa !important;
            border: 1.5px solid #e9ecef !important;
            border-radius: 10px !important;
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            font-size: 0.75rem !important;
            font-family: 'Inter', sans-serif !important;
            color: #212529 !important;
            padding: 0.45rem 0.85rem !important;
            transition: all 0.25s ease !important;
            width: 100% !important;
        }

            #txtFiltroClave:hover {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

            #txtFiltroClave:focus {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                box-shadow: 0 0 0 4px rgba(159, 34, 65, 0.10) !important;
                outline: none !important;
            }

            #txtFiltroClave::placeholder {
                color: #adb5bd !important;
                font-weight: 400 !important;
                font-size: 0.75rem !important;
            }

        /* ============================================================
    INPUT GROUP EN MODAL (Clave Presupuestal)
    ============================================================ */

        .modal .input-group {
            display: flex !important;
            align-items: stretch !important;
            width: 100% !important;
        }

            .modal .input-group .form-control-sm,
            .modal .input-group .campo-clave,
            .modal .input-group input[type="text"] {
                flex: 1 1 auto !important;
                width: 1% !important;
                border-radius: 10px 0 0 10px !important;
                height: 38px !important;
                min-height: 38px !important;
                max-height: 38px !important;
                border-right: none !important;
            }

            .modal .input-group .btn-sm {
                flex: 0 0 auto !important;
                height: 38px !important;
                min-height: 38px !important;
                max-height: 38px !important;
                border-radius: 0 10px 10px 0 !important;
                background: #f8f9fa !important;
                border: 1.5px solid #e9ecef !important;
                border-left: none !important;
                color: #9F2241 !important;
                display: flex !important;
                align-items: center !important;
                justify-content: center !important;
                padding: 0 12px !important;
                transition: all 0.25s ease !important;
                cursor: pointer !important;
            }

                .modal .input-group .btn-sm:hover {
                    background: #ffffff !important;
                    border-color: #9F2241 !important;
                    color: #9F2241 !important;
                    box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
                }

                .modal .input-group .btn-sm i {
                    font-size: 0.9rem !important;
                }

            /* Cuando el input está enfocado, el botón mantiene consistencia */
            .modal .input-group .form-control-sm:focus + .btn-sm,
            .modal .input-group .campo-clave:focus + .btn-sm {
                border-color: #9F2241 !important;
                background: #ffffff !important;
            }

        /* ============================================================
    ESTILOS PARA BOTONES EN MODALES
    ============================================================ */

        /* Botones en modales - mismo estilo que los inputs */
        .modal .btn-sm {
            height: 38px !important;
            min-height: 38px !important;
            max-height: 38px !important;
            border-radius: 10px !important;
            font-size: 0.75rem !important;
            font-family: 'Inter', sans-serif !important;
            padding: 0 16px !important;
            display: inline-flex !important;
            align-items: center !important;
            justify-content: center !important;
            gap: 6px !important;
            transition: all 0.25s ease !important;
        }

        /* Botón secundario (Cerrar) */
        .modal .btn-secondary {
            background-color: #f8f9fa !important;
            border: 1.5px solid #e9ecef !important;
            color: #212529 !important;
        }

            .modal .btn-secondary:hover {
                background-color: #ffffff !important;
                border-color: #9F2241 !important;
                color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

        /* Botón primario (Guardar) */
        .modal .btn-primary {
            background: linear-gradient(135deg, #9F2241 0%, #7a1a32 100%) !important;
            border: none !important;
            color: #ffffff !important;
        }

            .modal .btn-primary:hover {
                background: linear-gradient(135deg, #b0284a 0%, #8a1d3a 100%) !important;
                box-shadow: 0 4px 16px rgba(159, 34, 65, 0.25) !important;
                transform: translateY(-1px) !important;
            }

        /* Botón éxito (Seleccionar Clave) */
        .modal .btn-success {
            background: #28a745 !important;
            border: none !important;
            color: #ffffff !important;
        }

            .modal .btn-success:hover {
                background: #218838 !important;
                box-shadow: 0 4px 16px rgba(40, 167, 69, 0.25) !important;
                transform: translateY(-1px) !important;
            }

        /* Botón info (Buscar clave) */
        .modal .btn-info {
            background: #f8f9fa !important;
            border: 1.5px solid #e9ecef !important;
            color: #9F2241 !important;
        }

            .modal .btn-info:hover {
                background: #ffffff !important;
                border-color: #9F2241 !important;
                color: #9F2241 !important;
                box-shadow: 0 2px 8px rgba(159, 34, 65, 0.08) !important;
            }

        /* ============================================================
    RESPONSIVE
    ============================================================ */
        @media (max-width: 768px) {
            .select-search-input {
                height: 36px !important;
                min-height: 36px !important;
                max-height: 36px !important;
                font-size: 0.75rem !important;
            }

            .select-search-options li {
                padding: 6px 12px;
                font-size: 0.75rem;
            }
            /* Estilos para modales en responsive */
            .modal .select-search-input {
                height: 34px !important;
                min-height: 34px !important;
                max-height: 34px !important;
                font-size: 0.7rem !important;
            }

            .modal .select-search-options li {
                padding: 4px 10px;
                font-size: 0.7rem;
            }

            .modal .btn-sm {
                height: 34px !important;
                min-height: 34px !important;
                max-height: 34px !important;
                font-size: 0.7rem !important;
                padding: 0 12px !important;
            }
        }
    </style>


    <script type="text/javascript">
        var data = sessionStorage.getItem('name');
        if (data == null) {
            window.location.href = "Login.aspx";
        }
    </script>

    <script>
        $('document').ready(function () {
            $("input,select").bind("keydown", function (e) {
                var keyCode = e.keyCode || e.which;
                if (keyCode === 13) {
                    e.preventDefault();
                    $('input, select, textarea')[$('input,select,textarea').index(this) + 1].focus();
                }
            });
        });
    </script>
</head>

<body class="g-sidenav-show">
    <nav class="navbar navbar-main navbar-expand-lg px-0 mx-4 shadow-none border-radius-xl mt-3 mx-3 bg-gradient-primary" id="navbarBlur" data-scroll="false">
        <div class="container-fluid py-1 px-3">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb bg-transparent mb-0 pb-0 pt-1 px-0 me-sm-6 me-5">
                    <li class="breadcrumb-item text-sm"><a class="opacity-5 text-white" href="Menu.aspx"><i class="bi bi-house-door"></i>Menú</a></li>
                    <li class="breadcrumb-item text-sm text-white active" aria-current="page">Ministraciones</li>
                </ol>
                <h6 class="font-weight-bolder text-white mb-0">
                    <i class="bi bi-cash-stack me-2"></i>Gestión de Ministraciones
                </h6>
            </nav>
            <div class="collapse navbar-collapse mt-sm-0 mt-2 me-md-0 me-sm-4" id="navbar">
                <h3 style="color: white; margin: 0 auto;">Secretaría de Infraestructura</h3>
                <div class="d-flex align-items-center me-3">
                    <a href="javascript:history.back();" class="btn btn-outline-white btn-sm mb-0 shadow-sm">
                        <i class="bi bi-arrow-left me-2"></i> Regresar
                    </a>
                </div>
            </div>
        </div>
    </nav>

    <div class="main-content position-relative max-height-vh-100 h-100">
        <div class="container-fluid mt-4">

            <!-- ===== STATS ===== -->
            <div class="stats-grid" id="statsGrid">
                <div class="stat-card">
                    <div class="stat-icon"><i class="bi bi-grid-3x3-gap-fill"></i></div>
                    <div class="stat-info">
                        <div class="stat-number" id="totalMinistraciones">0</div>
                        <div class="stat-label">Total Ministraciones</div>
                        <div class="stat-details" id="detalleEstatus">
                            <span class="badge-status registrada">Reg: 0</span>
                            <span class="badge-status iniciado">Inc: 0</span>
                            <span class="badge-status cancelada">Can: 0</span>
                        </div>
                    </div>
                </div>

                <div class="stat-card">
                    <div class="stat-icon"><i class="bi bi-coin"></i></div>
                    <div class="stat-info">
                        <div class="stat-number" id="totalImporte">$0.00</div>
                        <div class="stat-label">Importe Total</div>
                        <div class="stat-details" id="detalleFuente">
                            <span class="badge-tipo etiquetado"><i class="bi bi-tag-fill me-1"></i>Etiquetado: $0</span>
                            <span class="badge-tipo no-etiquetado"><i class="bi bi-tag me-1"></i>No Etiquetado: $0</span>
                        </div>
                    </div>
                </div>

                <div class="stat-card">
                    <div class="stat-icon"><i class="bi bi-calendar-check"></i></div>
                    <div class="stat-info">
                        <div class="stat-number" id="totalMesActual">0</div>
                        <div class="stat-label">Ministraciones <span id="nombreMesActual">este mes</span></div>
                        <div class="stat-details">
                            <span class="stat-trend" id="trendMensual"><i class="bi bi-arrow-right-short"></i>Sin datos previos</span>
                        </div>
                    </div>
                </div>

                <div class="stat-card">
                    <div class="stat-icon" style="background: #e8f0fe; color: #1a73e8;"><i class="bi bi-calculator"></i></div>
                    <div class="stat-info">
                        <div class="stat-number" id="totalPresupuesto">$0.00</div>
                        <div class="stat-label">Presupuesto Calendarizado</div>
                        <div class="stat-details" id="detalleDiferencia">
                            <span class="diferencia-cero"><i class="bi bi-dash-circle"></i>Diferencia: $0.00</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- ===== TABLA PRINCIPAL ===== -->
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header pb-0">
                            <div class="card-header-custom">
                                <div class="header-left">
                                    <div class="header-icon"><i class="bi bi-cash-stack"></i></div>
                                    <div class="header-text">
                                        <h6>Ministraciones</h6>
                                        <p><i class="bi bi-info-circle text-info me-1"></i>Gestión, seguimiento y control de ministraciones presupuestales</p>
                                    </div>
                                </div>
                                <div class="header-actions">
                                    <div class="header-badge">
                                        <i class="bi bi-calendar3"></i>
                                        <span id="fechaActual"></span>
                                    </div>
                                    <button class="btn btn-primary btn-sm" onclick="abrirComparacionMesGlobal()" style="border-radius: 50px; padding: 5px 18px; margin: 0;">
                                        <i class="bi bi-calculator me-1"></i>Comparar Mes
                                    </button>
                                    <button class="btn btn-primary btn-sm" onclick="exportarExcel()" style="border-radius: 50px; padding: 5px 18px; margin: 0;">
                                        <i class="bi bi-file-earmark-excel me-1"></i>Exportar Excel
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <!-- ===== FILTROS MODERNOS ===== -->
                            <div class="filter-section">
                                <div class="filter-container">
                                    <!-- Fila superior: Filtros principales -->
                                    <div class="filter-row">
                                        <!-- Ministración - CON buscador integrado -->
                                        <div class="filter-group">
                                            <div class="filter-label">
                                                <i class="bi bi-tag-fill"></i>
                                                <span>Ministración</span>
                                            </div>
                                            <div class="select-search-wrapper" data-filter="ministracion">
                                                <input type="text" class="select-search-input" placeholder="Todas"
                                                    autocomplete="off"
                                                    onfocus="openSearchDropdown(this)"
                                                    onkeyup="filterSearchOptions(this)"
                                                    onblur="closeSearchDropdownDelayed(this)"/>
                                                <div class="select-search-dropdown">
                                                    <ul class="select-search-options"></ul>
                                                </div>
                                            </div>
                                            <input type="hidden" id="cboministracion" value=""/>
                                        </div>

                                        <!-- Mes - AHORA CON buscador integrado -->
                                        <div class="filter-group">
                                            <div class="filter-label">
                                                <i class="bi bi-calendar3"></i>
                                                <span>Mes</span>
                                            </div>
                                            <div class="select-search-wrapper" data-filter="mes">
                                                <input type="text" class="select-search-input" placeholder="Todos"
                                                    autocomplete="off"
                                                    onfocus="openSearchDropdown(this)"
                                                    onkeyup="filterSearchOptions(this)"
                                                    onblur="closeSearchDropdownDelayed(this)"/>
                                                <div class="select-search-dropdown">
                                                    <ul class="select-search-options"></ul>
                                                </div>
                                            </div>
                                            <input type="hidden" id="cbomes" value=""/>
                                        </div>
                                        <!-- Tipo - CON buscador integrado -->
                                        <div class="filter-group">
                                            <div class="filter-label">
                                                <i class="bi bi-layers"></i>
                                                <span>Tipo</span>
                                            </div>
                                            <div class="select-search-wrapper" data-filter="tipo">
                                                <input type="text" class="select-search-input" placeholder="Todos"
                                                    autocomplete="off"
                                                    onfocus="openSearchDropdown(this)"
                                                    onkeyup="filterSearchOptions(this)"
                                                    onblur="closeSearchDropdownDelayed(this)"/>
                                                <div class="select-search-dropdown">
                                                    <ul class="select-search-options"></ul>
                                                </div>
                                            </div>
                                            <input type="hidden" id="cbTipoMinistracion" value=""/>
                                        </div>

                                        <!-- Programa Fondo - CON buscador integrado -->
                                        <div class="filter-group">
                                            <div class="filter-label">
                                                <i class="bi bi-folder2-open"></i>
                                                <span>Programa Fondo</span>
                                            </div>
                                            <div class="select-search-wrapper" data-filter="programaFondo">
                                                <input type="text" class="select-search-input" placeholder="Todos"
                                                    autocomplete="off"
                                                    onfocus="openSearchDropdown(this)"
                                                    onkeyup="filterSearchOptions(this)"
                                                    onblur="closeSearchDropdownDelayed(this)"/>
                                                <div class="select-search-dropdown">
                                                    <ul class="select-search-options"></ul>
                                                </div>
                                            </div>
                                            <input type="hidden" id="cbProgramaFondo" value="" />
                                        </div>

                                        <!-- Estatus - AHORA CON buscador integrado -->
                                        <div class="filter-group">
                                            <div class="filter-label">
                                                <i class="bi bi-circle-fill"></i>
                                                <span>Estatus</span>
                                            </div>
                                            <div class="select-search-wrapper" data-filter="estatus">
                                                <input type="text" class="select-search-input" placeholder="Todos"
                                                    autocomplete="off"
                                                    onfocus="openSearchDropdown(this)"
                                                    onkeyup="filterSearchOptions(this)"
                                                    onblur="closeSearchDropdownDelayed(this)" />
                                                <div class="select-search-dropdown">
                                                    <ul class="select-search-options"></ul>
                                                </div>
                                            </div>
                                            <input type="hidden" id="cbEstatus" value=""/>
                                        </div>

                                        <!-- Botón Agregar -->
                                        <div class="filter-group filter-action">
                                            <div class="filter-label">&nbsp;</div>
                                            <button class="btn-add" onclick="Modalministracion();">
                                                <i class="bi bi-plus-circle"></i>
                                                Agregar
                                            </button>
                                        </div>
                                    </div>

                                    <!-- Fila inferior: Buscador mejorado -->
                                    <div class="filter-row search-row">
                                        <div class="search-wrapper-modern">
                                            <div class="search-input-group">
                                                <i class="bi bi-search search-icon"></i>
                                                <input type="text" id="txtClavePresupuestal" class="search-input-modern"
                                                    placeholder="Buscar por folio, clave presupuestal, enlace o concepto..."
                                                    onkeyup="buscarClaves()" />
                                                <span class="search-shortcut-modern">Ctrl + K</span>
                                                <div id="autocompleteResultados" class="autocomplete-modern" style="display: none;"></div>
                                            </div>
                                            <div class="search-hints">
                                                <span class="hint-badge"><kbd>Folio</kbd></span>
                                                <span class="hint-badge"><kbd>Clave</kbd></span>
                                                <span class="hint-badge"><kbd>Enlace</kbd></span>
                                                <span class="hint-badge"><kbd>Concepto</kbd></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ===== TABLA ===== -->
                            <div class="table-responsive p-0">
                                <table class="table table-striped align-items-center mb-0 table-mini" id="tablaMinistraciones">
                                    <thead>
                                        <tr>
                                            <th style="width: 50px;">Año</th>
                                            <th style="width: 70px;">Mes</th>
                                            <th style="min-width: 100px;">Folio</th>
                                            <th style="min-width: 80px;">Tipo</th>
                                            <th style="min-width: 70px;">Dígito</th>
                                            <th style="min-width: 180px;">Clave Presupuestal</th>
                                            <th style="min-width: 100px;" class="text-end">Monto</th>
                                            <th style="min-width: 120px;" class="text-end">Presupuesto</th>
                                            <th style="min-width: 80px;">Estatus</th>
                                            <th class="text-center" style="min-width: 120px;">Opciones</th>
                                        </tr>
                                    </thead>
                                    <tbody id="display">
                                    </tbody>
                                </table>
                            </div>

                            <div class="row mt-3">
                                <div class="col-12">
                                    <nav>
                                        <ul class="pagination pagination-custom justify-content-end" id="pagination">
                                        </ul>
                                    </nav>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <footer class="footer pt-3">
                <div class="container-fluid">
                    <div class="row align-items-center justify-content-lg-between">
                        <div class="col-lg-6 mb-lg-0 mb-4">
                            <div class="copyright text-center text-sm text-muted text-lg-start" style="color: white">
                                ©
                                <script>document.write(new Date().getFullYear())</script>
                                ,
                                Secretaría de Infraestructura.
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    </div>

    <!-- ===== HIDDENS ===== -->
    <input type="hidden" id="hddaccion" value="" />
    <input type="hidden" id="hddIdMinistracion" value="" />
    <input type="hidden" id="hddMesActual" value="" />

    <!-- ===== MODAL: AGREGAR/EDITAR MINISTRACIÓN ===== -->
    <div class="modal fade" id="modal-form" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-custom">
                    <h5 class="modal-title" style="color: white !important;">
                        <i class="bi bi-plus-circle me-2"></i><span id="modalTitle">Nueva Ministración</span>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-3">
                    <!-- FILA 1: Tipo Folio, Folio, Mes, Año -->
                    <div class="row g-2">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-calendar me-1"></i>Año *</label>
                                <input id="txtAnio" type="number" class="form-control form-control-sm" placeholder="2025" min="2000" max="2100" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-calendar-month me-1"></i>Mes *</label>
                                <div class="select-search-wrapper" data-filter="mesModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbomescaptura" value="">
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-hash me-1"></i>Tipo Folio</label>
                                <div class="select-search-wrapper" data-filter="tipoFolioModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbTipoFolio" value="">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-file-text me-1"></i>Folio *</label>
                                <input id="txtnumministracion" type="text" class="form-control form-control-sm" placeholder="Ej: 202501-MM001" />
                            </div>
                        </div>
                    </div>

                    <!-- FILA 2: Tipo Ministración, Digito, Sub Fuente -->
                    <div class="row g-2">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-tag me-1"></i>Tipo Ministración</label>
                                <div class="select-search-wrapper" data-filter="tipoModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbotipoministracion" value="">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-sort-numeric-down me-1"></i>Dígito Ministración</label>
                                <div class="select-search-wrapper" data-filter="digitoModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbDigitoMinistracion" value="">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-currency-dollar me-1"></i>Sub Fuente Financiamiento</label>
                                <div class="select-search-wrapper" data-filter="subFuenteModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbSubFuente" value="">
                            </div>
                        </div>
                    </div>

                    <!-- FILA 3: Programa Fondo, Clave Presupuestal, Proyecto Estratégico -->
                    <div class="row g-2">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-folder me-1"></i>Programa Fondo</label>
                                <div class="select-search-wrapper" data-filter="programaFondoModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbProgramaFondoModal" value="">
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-key me-1"></i>Clave Presupuestal *</label>
                                <div class="input-group input-group-sm">
                                    <input id="txtClavePresupuestalModal" type="text" class="form-control form-control-sm campo-clave" placeholder="Seleccione una clave..." readonly />
                                    <button class="btn btn-info btn-sm" type="button" onclick="abrirModalClaves()" title="Buscar clave presupuestal">
                                        <i class="bi bi-search"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-bullseye me-1"></i>Proyecto Estratégico</label>
                                <div class="select-search-wrapper" data-filter="proyectoEstrategicoModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbProyectoEstrategico" value="">
                            </div>
                        </div>
                    </div>

                    <!-- FILA 3.5: Detalles del Programa Fondo (TR, FF, Ramo, Clave PF) -->
                    <div class="row g-2 mt-1">
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-tag me-1"></i>TR (Tipo Recurso)</label>
                                <input id="txtTR" type="text" class="form-control form-control-sm campo-lectura" readonly placeholder="-" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-currency-dollar me-1"></i>FF (Fuente Financiamiento)</label>
                                <input id="txtFF" type="text" class="form-control form-control-sm campo-lectura" readonly placeholder="-" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-folder me-1"></i>Ramo</label>
                                <input id="txtRamo" type="text" class="form-control form-control-sm campo-lectura" readonly placeholder="-" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-hash me-1"></i>Clave PF (Programa Fondo)</label>
                                <input id="txtClavePF" type="text" class="form-control form-control-sm campo-lectura" readonly placeholder="-" />
                            </div>
                        </div>
                    </div>

                    <!-- FILA 4: Importe Total, Estatus -->
                    <div class="row g-2 mt-1">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-check-circle me-1"></i>Importe Total *</label>
                                <input id="txtmontoautorizado" type="text" class="form-control form-control-sm" placeholder="$0.00" onkeypress="mascara(this,cpf)" onpaste="return false" style="font-weight: 700;" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-tag me-1"></i>Estatus</label>
                                <div class="select-search-wrapper" data-filter="estatusModal" style="width: 100%;">
                                    <input type="text" class="select-search-input" placeholder="Seleccione..."
                                        autocomplete="off"
                                        onfocus="openSearchDropdown(this)"
                                        onkeyup="filterSearchOptions(this)"
                                        onblur="closeSearchDropdownDelayed(this)" style="height: 38px; min-height: 38px; max-height: 38px; font-size: 0.75rem;">
                                    <div class="select-search-dropdown">
                                        <ul class="select-search-options"></ul>
                                    </div>
                                </div>
                                <input type="hidden" id="cbEstatusModal" value="REGISTRADA">
                            </div>
                        </div>
                    </div>

                    <!-- FILA 5: Concepto -->
                    <div class="row g-2">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-file-text me-1"></i>Concepto</label>
                                <input id="txtconcepto" type="text" class="form-control form-control-sm" placeholder="Descripción del concepto..." />
                            </div>
                        </div>
                    </div>

                    <!-- FILA 6: Presupuesto Calendarizado Asociado -->
                    <div class="row g-2">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="form-label-sm"><i class="bi bi-calendar2-week me-1"></i>Presupuesto Calendarizado Asociado</label>
                                <input id="txtPresupuestoAsociado" type="text" class="form-control form-control-sm campo-lectura" placeholder="Seleccione una clave para ver el presupuesto" readonly />
                            </div>
                        </div>
                    </div>

                    <!-- FILA 7: Resumen de Campos -->
                    <div class="row g-2 mt-1">
                        <div class="col-12">
                            <div class="alert alert-info alert-sm py-1 px-2 mb-0" style="font-size:0.7rem; background:#e8f4fd; border-color:#b8d4e8; color:#0c5460;">
                                <i class="bi bi-info-circle me-1"></i>
                                <span id="lblResumenCampos">Seleccione los campos para completar la ministración</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">
                        <i class="bi bi-x-circle me-1"></i>Cancelar
                    </button>
                    <button type="button" class="btn btn-primary btn-sm" onclick="guardarMinistracion()">
                        <i class="bi bi-save me-1"></i>Guardar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <!-- ===== MODAL: BÚSQUEDA DE CLAVES PRESUPUESTALES ===== -->
    <div class="modal fade modal-claves" id="modal-claves" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header modal-header-custom">
                    <h5 class="modal-title" style="color: white !important;">
                        <i class="bi bi-key me-2"></i>Seleccionar Clave Presupuestal <span id="tituloClavesModal"></span>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-2">
                    <div class="row g-2 mb-2">
                        <div class="col-md-12">
                            <div class="form-group">
                                <input type="text" class="form-control form-control-sm" id="txtFiltroClave" placeholder="Filtrar clave..." onkeyup="filtrarClavesModal()" />
                            </div>
                        </div>
                    </div>
                    <div class="table-responsive" style="max-height: 350px; overflow-y: auto;">
                        <table class="table table-sm table-striped table-claves">
                            <thead>
                                <tr>
                                    <th style="width: 40px;">
                                        <input type="checkbox" id="chkSeleccionarTodos" onchange="toggleAllClavesModal()" />
                                    </th>
                                    <th style="width: 35%;">Clave Presupuestal</th>
                                    <th style="width: 20%;">Descripción</th>
                                    <th style="width: 15%;">Fuente</th>
                                    <th style="width: 10%;">Mes</th>
                                    <th style="width: 10%;" class="text-end">Monto</th>
                                </tr>
                            </thead>
                            <tbody id="clavesBodyModal">
                            </tbody>
                        </table>
                    </div>
                    <div class="row mt-2">
                        <div class="col-12 text-end">
                            <button class="btn btn-success btn-sm" onclick="seleccionarClaveModal()" style="height: 38px; border-radius: 10px;">
                                <i class="bi bi-check2 me-1"></i>Seleccionar Clave
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- ===== MODAL: DETALLE DE MINISTRACIÓN ===== -->
    <div class="modal fade" id="modal-detalle" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-custom">
                    <h5 class="modal-title" style="color: white !important; font-size: 0.95rem;">
                        <i class="bi bi-eye me-2"></i>Detalle de Ministración
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-2 detalle-mini" id="detalleContent">
                    <div class="text-center py-4">
                        <div class="spinner-border text-primary" role="status" style="width: 2rem; height: 2rem;">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                        <p class="mt-2" style="font-size: 0.8rem;">Cargando detalles...</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- ===== MODAL: COMPARACIÓN DEL MES ===== -->
    <div class="modal fade" id="modal-comparacion-mes" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header modal-header-custom">
                    <h5 class="modal-title" style="color: white !important; font-size: 0.95rem;">
                        <i class="bi bi-calculator me-2"></i>Comparación del Mes <span id="tituloComparacionMes"></span>
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-3" id="comparacionContent">
                    <div class="text-center py-4">
                        <div class="spinner-border text-primary" role="status" style="width: 2rem; height: 2rem;">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                        <p class="mt-2" style="font-size: 0.8rem;">Cargando comparación...</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- ===== LOADING OVERLAY ===== -->
    <div class="jm-loadingpage" id="loadingOverlay"></div>

    <!-- ===== SCRIPTS ===== -->
    <script src="assets/js/core/popper.min.js"></script>
    <script src="assets/js/core/bootstrap.min.js"></script>
    <script src="assets/js/plugins/perfect-scrollbar.min.js"></script>
    <script src="assets/js/SweetAlert2/plugins/sweetAlert2/sweetalert2.all.min.js"></script>
    <script src="assets/js/plugins/smooth-scrollbar.min.js"></script>
    <script src="assets/js/EliminarSesion.js"></script>
    <script src="assets/js/argon-dashboard.min.js?v=2.0.0"></script>

    <!-- Argon Dashboard - Carga con manejo de errores -->
    <script>
        // Prevenir errores antes de cargar Argon
        window.navbarColorOnResize = function () { return; };

        // Cargar Argon
        var argonScript = document.createElement('script');
        argonScript.src = 'assets/js/argon-dashboard.min.js?v=2.0.0';
        argonScript.onload = function () {
            // Después de cargar, silenciar cualquier error
            setTimeout(function () {
                try {
                    if (typeof navbarColorOnResize !== 'undefined') {
                        navbarColorOnResize = function () { };
                    }
                } catch (e) { }
            }, 100);
        };
        document.body.appendChild(argonScript);
    </script>

    <script>
        var win = navigator.platform.indexOf('Win') > -1;
        if (win && document.querySelector('#sidenav-scrollbar')) {
            var options = { damping: '0.5' };
            Scrollbar.init(document.querySelector('#sidenav-scrollbar'), options);
        }

        // =============================================
        // INICIALIZACIÓN
        // =============================================
        $(document).ready(function () {
            const hoy = new Date();
            document.getElementById('fechaActual').textContent = hoy.toLocaleDateString('es-MX', {
                weekday: 'long',
                year: 'numeric',
                month: 'long',
                day: 'numeric'
            });
            document.getElementById('hddMesActual').value = hoy.getMonth() + 1;

            catalogos();
            filtrar();

            document.addEventListener('keydown', function (e) {
                if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
                    e.preventDefault();
                    document.getElementById('txtClavePresupuestal').focus();
                }
                if (e.key === 'Escape') {
                    document.getElementById('autocompleteResultados').style.display = 'none';
                }
            });

            document.addEventListener('click', function (e) {
                const searchWrapper = document.querySelector('.search-wrapper-modern');
                if (searchWrapper && !searchWrapper.contains(e.target)) {
                    document.getElementById('autocompleteResultados').style.display = 'none';
                }
            });
        });

        // =============================================
        // FUNCIONES DE UTILERÍA
        // =============================================
        function formatMoney(amount) {
            if (!amount || isNaN(amount)) return '0.00';
            return parseFloat(amount).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }

        function mascara(o, f) {
            v_obj = o;
            v_fun = f;
            setTimeout("execmascara()", 1);
        }
        function execmascara() {
            v_obj.value = v_fun(v_obj.value);
        }
        function cpf(v) {
            v = v.replace(/[^\d.]/g, ""); // Permitir solo números y punto
            v = v.replace(/(\..*?)\..*/g, '$1'); // Evitar más de un punto
            // Nota: No se le agrega comas mientras escribe porque interfiere con los decimales
            // El formato de moneda final se aplica cuando se pierde el foco (blur)
            return v;
        }

        function showNotification(message, type) {
            Swal.fire({
                type: type,
                title: message,
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000
            });
        }

        function eliminarsesion() {
            sessionStorage.removeItem('name');
            window.location.href = "Login.aspx";
        }

        // =============================================
        // DATOS DE LOS FILTROS CON BUSCADOR
        // =============================================

        var searchData = {
            ministracion: { options: [], selectedValue: '', selectedLabel: '' },
            mes: { options: [], selectedValue: '', selectedLabel: '' },
            tipo: { options: [], selectedValue: '', selectedLabel: '' },
            programaFondo: { options: [], selectedValue: '', selectedLabel: '' },
            estatus: { options: [], selectedValue: '', selectedLabel: '' },
            mesModal: { options: [], selectedValue: '', selectedLabel: '' },
            digitoModal: { options: [], selectedValue: '', selectedLabel: '' },
            fuenteModal: { options: [], selectedValue: '', selectedLabel: '' },
            estatusModal: { options: [], selectedValue: '', selectedLabel: '' },
            fuenteClaveModal: { options: [], selectedValue: '', selectedLabel: '' },
            mesFiltroClave: { options: [], selectedValue: '', selectedLabel: '' },
            selectorMes: { options: [], selectedValue: '', selectedLabel: '' },
            tipoFolioModal: { options: [], selectedValue: '', selectedLabel: '' },
            subFuenteModal: { options: [], selectedValue: '', selectedLabel: '' },
            proyectoEstrategicoModal: { options: [], selectedValue: '', selectedLabel: '' },
            tipoRecursoModal: { options: [], selectedValue: '', selectedLabel: '' },
            ramoModal: { options: [], selectedValue: '', selectedLabel: '' }
        };

        var modalSearchData = {
            tipoModal: { options: [], selectedValue: '', selectedLabel: '' },
            programaFondoModal: { options: [], selectedValue: '', selectedLabel: '' },
            tipoFolioModal: { options: [], selectedValue: '', selectedLabel: '' },
            subFuenteModal: { options: [], selectedValue: '', selectedLabel: '' },
            proyectoEstrategicoModal: { options: [], selectedValue: '', selectedLabel: '' }
        };

        // =============================================
        // FUNCIONES DEL BUSCADOR PERSONALIZADO
        // =============================================

        function openSearchDropdown(input) {
            var wrapper = input.closest('.select-search-wrapper');
            var dropdown = wrapper.querySelector('.select-search-dropdown');
            var filterKey = wrapper.dataset.filter;

            document.querySelectorAll('.select-search-dropdown.show').forEach(function (el) {
                if (el !== dropdown) {
                    el.classList.remove('show');
                    el.closest('.select-search-wrapper').classList.remove('open');
                }
            });

            if (dropdown) {
                if (!dropdown.classList.contains('show')) {
                    dropdown.classList.add('show');
                    wrapper.classList.add('open');
                }
                renderSearchOptions(wrapper, filterKey, '');
                setTimeout(function () {
                    input.focus();
                    input.select();
                }, 50);
            }
        }

        function closeSearchDropdownDelayed(input) {
            setTimeout(function () {
                var wrapper = input.closest('.select-search-wrapper');
                if (wrapper) {
                    var dropdown = wrapper.querySelector('.select-search-dropdown');
                    if (dropdown) {
                        dropdown.classList.remove('show');
                        wrapper.classList.remove('open');
                    }
                }
            }, 200);
        }

        function filterSearchOptions(input) {
            var wrapper = input.closest('.select-search-wrapper');
            var filterKey = wrapper.dataset.filter;
            renderSearchOptions(wrapper, filterKey, input.value);
        }

        function renderSearchOptions(wrapper, filterKey, searchTerm) {
            var dropdown = wrapper.querySelector('.select-search-dropdown');
            var list = dropdown.querySelector('.select-search-options');

            var dataSource;
            if (['tipoModal', 'programaFondoModal', 'tipoFolioModal', 'subFuenteModal', 'proyectoEstrategicoModal'].includes(filterKey)) {
                dataSource = modalSearchData[filterKey];
            } else {
                dataSource = searchData[filterKey];
            }

            var options = dataSource ? dataSource.options || [] : [];
            var selectedValue = dataSource ? dataSource.selectedValue || '' : '';
            var term = searchTerm.toLowerCase().trim();

            list.innerHTML = '';

            var filtered = options;
            if (term) {
                filtered = options.filter(function (opt) {
                    return opt.label.toLowerCase().includes(term);
                });
            }

            if (filtered.length === 0) {
                var li = document.createElement('li');
                li.textContent = 'No hay resultados';
                li.style.color = '#adb5bd';
                li.style.cursor = 'default';
                li.style.textAlign = 'center';
                li.style.padding = '12px';
                list.appendChild(li);
                return;
            }

            filtered.forEach(function (opt) {
                var li = document.createElement('li');
                li.textContent = opt.label;
                li.dataset.value = opt.value;

                if (opt.value === selectedValue) {
                    li.classList.add('selected');
                }

                if (term && opt.label.toLowerCase().includes(term)) {
                    var index = opt.label.toLowerCase().indexOf(term);
                    var before = opt.label.substring(0, index);
                    var match = opt.label.substring(index, index + term.length);
                    var after = opt.label.substring(index + term.length);
                    li.innerHTML = before + '<span class="highlight">' + match + '</span>' + after;
                }

                li.addEventListener('mousedown', function (e) {
                    e.preventDefault();
                    selectSearchOption(wrapper, filterKey, opt.value, opt.label);
                });

                list.appendChild(li);
            });
        }

        function selectSearchOption(wrapper, filterKey, value, label) {
            var input = wrapper.querySelector('.select-search-input');
            var hiddenInput = wrapper.parentElement.querySelector('input[type="hidden"]');
            var dropdown = wrapper.querySelector('.select-search-dropdown');

            if (['tipoModal', 'programaFondoModal', 'tipoFolioModal', 'subFuenteModal', 'proyectoEstrategicoModal'].includes(filterKey)) {
                modalSearchData[filterKey].selectedValue = value;
                modalSearchData[filterKey].selectedLabel = label;
            } else {
                searchData[filterKey].selectedValue = value;
                searchData[filterKey].selectedLabel = label;
            }

            input.value = label;

            if (hiddenInput) {
                hiddenInput.value = value;
                // Disparar el evento change para que los event listeners lo detecten
                var event = new Event('change', { bubbles: true });
                hiddenInput.dispatchEvent(event);
                // O con jQuery si está garantizado: $(hiddenInput).trigger('change');
            }

            dropdown.classList.remove('show');
            wrapper.classList.remove('open');

            if (!['tipoModal', 'programaFondoModal', 'tipoFolioModal', 'subFuenteModal', 'proyectoEstrategicoModal'].includes(filterKey)) {
                filtrar();
            }
        }

        function loadSearchOptions(filterKey, options, selectedValue, selectedLabel) {
            if (['tipoModal', 'programaFondoModal', 'tipoFolioModal', 'subFuenteModal', 'proyectoEstrategicoModal'].includes(filterKey)) {
                modalSearchData[filterKey].options = options;
                modalSearchData[filterKey].selectedValue = selectedValue || '';
                modalSearchData[filterKey].selectedLabel = selectedLabel || '';
            } else {
                searchData[filterKey].options = options;
                searchData[filterKey].selectedValue = selectedValue || '';
                searchData[filterKey].selectedLabel = selectedLabel || '';
            }

            var wrapper = document.querySelector('.select-search-wrapper[data-filter="' + filterKey + '"]');
            if (wrapper) {
                var input = wrapper.querySelector('.select-search-input');
                var hiddenInput = wrapper.parentElement.querySelector('input[type="hidden"]');
                if (selectedLabel) {
                    input.value = selectedLabel;
                } else {
                    input.value = options.length > 0 ? options[0].label : '';
                }
                if (hiddenInput) {
                    hiddenInput.value = selectedValue || '';
                }
            }
        }

        document.addEventListener('click', function (e) {
            if (!e.target.closest('.select-search-wrapper')) {
                document.querySelectorAll('.select-search-dropdown.show').forEach(function (el) {
                    el.classList.remove('show');
                    el.closest('.select-search-wrapper').classList.remove('open');
                });
            }
        });

        // =============================================
        // CATÁLOGOS
        // =============================================

        function catalogos() {
            // 1. MINISTRACION
            $.ajax({
                url: 'api/Ministraciones/GetMinistracionesCatalogo',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Todas' }];
                    data.forEach(function (item) {
                        options.push({ value: item.idministracion, label: item.numministracion });
                    });
                    loadSearchOptions('ministracion', options, '', 'Todas');
                }
            });

            // 2. MES
            $.ajax({
                url: 'api/Ministraciones/getmes',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Todos' }];
                    data.forEach(function (item) {
                        options.push({ value: item.numeromes, label: item.nombremes });
                    });
                    loadSearchOptions('mes', options, '', 'Todos');

                    var modalOptions = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        modalOptions.push({ value: item.numeromes, label: item.nombremes });
                    });
                    loadSearchOptions('mesModal', modalOptions, '', 'Seleccione...');

                    var filtroOptions = [{ value: '', label: 'Todos los meses' }];
                    data.forEach(function (item) {
                        filtroOptions.push({ value: item.numeromes, label: item.nombremes });
                    });
                    loadSearchOptions('mesFiltroClave', filtroOptions, '', 'Todos los meses');
                }
            });

            // 3. TIPO MINISTRACION
            $.ajax({
                url: 'api/Ministraciones/getTipoministracion',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Todos' }];
                    data.forEach(function (item) {
                        var labelText = item.Clave ? (item.Clave + ' - ' + item.Descripcion) : item.Descripcion;
                        options.push({ value: item.Id, label: labelText });
                    });
                    loadSearchOptions('tipo', options, '', 'Todos');

                    var modalOptions = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        var labelText = item.Clave ? (item.Clave + ' - ' + item.Descripcion) : item.Descripcion;
                        modalOptions.push({ value: item.Id, label: labelText });
                    });
                    loadSearchOptions('tipoModal', modalOptions, '', 'Seleccione...');
                }
            });

            // 4. DIGITO MINISTRACION
            $.ajax({
                url: 'api/Ministraciones/getDigitoMinistracion',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        options.push({ value: item.Id, label: item.Clave + ' - ' + item.Descripcion });
                    });
                    loadSearchOptions('digitoModal', options, '', 'Seleccione...');
                }
            });

            // 5. FUENTE FINANCIAMIENTO
            $.ajax({
                url: 'api/Ministraciones/getFuenteFinanciamiento',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        options.push({ value: item.Id, label: item.Clave + ' - ' + item.Descripcion });
                    });
                    loadSearchOptions('fuenteModal', options, '', 'Seleccione...');
                }
            });

            // 6. PROGRAMA FONDO
            $.ajax({
                url: 'api/Ministraciones/getProgramasFondo',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    var options = [{ value: '', label: 'Todos' }];
                    data.forEach(function (item) {
                        var label = item.Descripcion;
                        if (item.Clave) label = item.Clave + ' - ' + label;
                        options.push({ value: item.Id, label: label });
                    });
                    loadSearchOptions('programaFondo', options, '', 'Todos');

                    var modalOptions = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        var label = item.Descripcion;
                        if (item.Clave) label = item.Clave + ' - ' + label;
                        modalOptions.push({ value: item.Id, label: label });
                    });
                    loadSearchOptions('programaFondoModal', modalOptions, '', 'Seleccione...');
                }
            });

            // 7. ESTATUS
            var estatusOptions = [
                { value: '', label: 'Todos' },
                { value: 'REGISTRADA', label: 'Registrada' },
                { value: 'INICIADO', label: 'Iniciado' },
                { value: 'CANCELADA', label: 'Cancelada' }
            ];
            loadSearchOptions('estatus', estatusOptions, '', 'Todos');

            var estatusModalOptions = [
                { value: '', label: 'Seleccione...' },
                { value: 'REGISTRADA', label: 'Registrada' },
                { value: 'INICIADO', label: 'Iniciado' },
                { value: 'CANCELADA', label: 'Cancelada' }
            ];
            loadSearchOptions('estatusModal', estatusModalOptions, '', 'Seleccione...');

            // 8. FUENTE CLAVE
            var fuenteClaveOptions = [
                { value: '', label: 'Todas las fuentes' },
                { value: 'Etiquetado', label: 'Etiquetado' },
                { value: 'No Etiquetado', label: 'No Etiquetado' }
            ];
            loadSearchOptions('fuenteClaveModal', fuenteClaveOptions, '', 'Todas las fuentes');

            // ===== NUEVOS CATALOGOS =====

            // 9. TIPO MINISTRACION FOLIO
            $.ajax({
                url: 'api/Ministraciones/GetTiposMinistracionFolio',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    console.log('Tipos Folio cargados:', data);
                    var options = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        options.push({ value: item.Id, label: item.Clave + ' - ' + item.Descripcion });
                    });
                    loadSearchOptions('tipoFolioModal', options, '', 'Seleccione...');
                },
                error: function (xhr, status, error) {
                    console.error('Error al cargar tipos de folio:', error);
                    var options = [
                        { value: '', label: 'Seleccione...' },
                        { value: '1', label: 'MM - Mensual' },
                        { value: '2', label: 'MT - Trimestral' },
                        { value: '3', label: 'MS - Semestral' },
                        { value: '4', label: 'MA - Anual' },
                        { value: '5', label: 'ME - Extraordinaria / Especial' },
                        { value: '6', label: 'MV - Virtual' },
                        { value: '7', label: 'MU - Unica vez' }
                    ];
                    loadSearchOptions('tipoFolioModal', options, '', 'Seleccione...');
                }
            });

            // 10. SUB FUENTE FINANCIAMIENTO
            $.ajax({
                url: 'api/Ministraciones/GetSubFuentesFinanciamiento',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    console.log('Sub Fuentes cargadas:', data);
                    var options = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        options.push({ value: item.Id, label: item.Clave + ' - ' + item.Descripcion });
                    });
                    loadSearchOptions('subFuenteModal', options, '', 'Seleccione...');
                },
                error: function (xhr, status, error) {
                    console.error('Error al cargar sub fuentes:', error);
                    var options = [
                        { value: '', label: 'Seleccione...' },
                        { value: '1', label: 'A - Gastos Comprometidos y/o Devengados por Registrar' },
                        { value: '2', label: 'G - Economias Derivadas de la Subfuente f' },
                        { value: '3', label: 'F - Recursos por Ingresos Excedentes' },
                        { value: '4', label: 'H - Recursos por Reducciones en Otras Previsiones' },
                        { value: '5', label: 'E - Productos Financieros del Año en Curso' },
                        { value: '6', label: 'B - Economias de Ejercicios Anteriores' },
                        { value: '7', label: 'I - Recursos por Reducciones en Otras Previsiones (Ingresos Excedentes F)' },
                        { value: '8', label: 'D - Productos Financieros de Ejercicios Anteriores' },
                        { value: '9', label: 'S - Recursos del Ejercicio' },
                        { value: '10', label: 'C - Recursos en Proceso de Ejecucion' }
                    ];
                    loadSearchOptions('subFuenteModal', options, '', 'Seleccione...');
                }
            });

            // 11. PROYECTO ESTRATEGICO
            $.ajax({
                url: 'api/Ministraciones/GetProyectosEstrategicos',
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    console.log('Proyectos Estrategicos cargados:', data);
                    var options = [{ value: '', label: 'Seleccione...' }];
                    data.forEach(function (item) {
                        options.push({ value: item.Id, label: item.Clave + ' - ' + item.Descripcion });
                    });
                    loadSearchOptions('proyectoEstrategicoModal', options, '', 'Seleccione...');
                },
                error: function (xhr, status, error) {
                    console.error('Error al cargar proyectos estrategicos:', error);
                    var options = [
                        { value: '', label: 'Seleccione...' },
                        { value: '1', label: 'C - Desarrollo Social' },
                        { value: '2', label: 'D - Concertado Diverso' },
                        { value: '3', label: 'B - Programa Normal' },
                        { value: '4', label: 'A - Institucional' },
                        { value: '5', label: 'E - Eventual' },
                        { value: '6', label: 'G - Genero' }
                    ];
                    loadSearchOptions('proyectoEstrategicoModal', options, '', 'Seleccione...');
                }
            });
        }

        // =============================================
        // FILTRAR
        // =============================================

        function filtrar() {
            var params = {
                busqueda: $('#txtClavePresupuestal').val() || '',
                ministracion: searchData.ministracion.selectedValue || '',
                mes: searchData.mes.selectedValue || '',
                tipo: searchData.tipo.selectedValue || '',
                programaFondo: searchData.programaFondo.selectedValue || '',
                estatus: searchData.estatus.selectedValue || ''
            };

            $.ajax({
                url: 'api/Ministraciones/getMinistraciones',
                type: 'GET',
                data: params,
                dataType: 'json',
                beforeSend: function () {
                    $('#display').html(`
                        <tr><td colspan="10" class="text-center py-4">
                            <div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div>
                            <p class="mt-2" style="font-size:0.8rem;">Cargando ministraciones...</p>
                        </td></tr>
                    `);
                },
                success: function (data) {
                    var html = '';
                    var mesActual = parseInt(document.getElementById('hddMesActual').value) || 1;

                    if (data.length === 0) {
                        html = `<tr><td colspan="10" class="text-center py-4 text-muted">
                                <i class="bi bi-inbox fs-2 d-block mb-2"></i>No hay ministraciones registradas
                            </td></tr>`;
                    } else {
                        data.forEach(function (item) {
                            var estatusClass = (item.estatus || 'REGISTRADA').toLowerCase();
                            var puedeEditarDirecto = (item.estatus === 'REGISTRADA');
                            var tienePDF = item.tienePDF || false;
                            var digitoDisplay = item.digitoDescripcion || item.DigitoMinistracion || 'N/A';

                            html += `
                                <tr>
                                    <td>${item.anio || 'N/A'}</td>
                                    <td>${item.nombremes || 'N/A'}</td>
                                    <td><span class="fw-bold">${item.numministracion || 'N/A'}</span></td>
                                    <td>${item.tipoministracion || 'N/A'}</td>
                                    <td style="font-size:0.7rem;">${digitoDisplay}</td>
                                    <td><span class="clave-badge" style="max-width:250px;">${item.enlace || 'N/A'}</span></td>
                                    <td class="text-end monto-ministrado">$${formatMoney(item.importe)}</td>
                                    <td class="text-end monto-presupuesto">$${formatMoney(item.PresupuestoCalendarizado || item.presupuestoCalendarizado || 0)}</td>
                                    <td><span class="badge-status ${estatusClass}">${item.estatus || 'REGISTRADA'}</span></td>
                                    <td class="text-center">
                                        <div class="btn-group btn-group-sm" role="group">
                                            <button class="btn btn-info btn-accion" onclick="verDetalle(${item.idministracion})" title="Detalle">
                                                <i class="bi bi-eye"></i>
                                            </button>
                                            ${puedeEditarDirecto ? `
                                            <button class="btn btn-primary btn-accion" onclick="editarMinistracion(${item.idministracion})" title="Editar">
                                                <i class="bi bi-pencil-square"></i>
                                            </button>` : ''}
                                            <button class="btn btn-accion ${tienePDF ? 'btn-pdf' : 'btn-pdf sin-pdf'}" onclick="${tienePDF ? 'verPDF(' + item.idministracion + ')' : ''}" title="${tienePDF ? 'Ver PDF' : 'Sin PDF'}" ${!tienePDF ? 'disabled' : ''}>
                                                <i class="bi bi-file-pdf"></i>
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            `;
                        });
                    }
                    $('#display').html(html);
                    actualizarEstadisticas(data);
                },
                error: function () {
                    $('#display').html(`<tr><td colspan="10" class="text-center py-4 text-danger">
                        <i class="bi bi-exclamation-triangle fs-2 d-block mb-2"></i>Error al cargar los datos
                    </td></tr>`);
                }
            });
        }

        // =============================================
        // ESTADÍSTICAS
        // =============================================

        function actualizarEstadisticas(data) {
            var total = data.length;
            var totalImporte = 0;
            var totalEtiquetado = 0;
            var totalNoEtiquetado = 0;
            var registradas = 0, iniciados = 0, canceladas = 0;
            var mesActual = parseInt(document.getElementById('hddMesActual').value) || new Date().getMonth() + 1;
            var anioActual = new Date().getFullYear();
            var totalMesActual = 0;
            var totalMesAnterior = 0;
            var totalPresupuesto = 0;

            var nombresMeses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
            document.getElementById('nombreMesActual').textContent = nombresMeses[mesActual - 1] || 'este mes';

            data.forEach(function (item) {
                var importe = parseFloat(item.importe || 0);
                totalImporte += importe;

                var estatus = (item.estatus || 'REGISTRADA').toUpperCase();
                if (estatus === 'REGISTRADA') registradas++;
                else if (estatus === 'INICIADO') iniciados++;
                else if (estatus === 'CANCELADA') canceladas++;

                var fuente = parseInt(item.fuenteFinanciamientoId || 0);
                if (fuente === 2) {
                    totalEtiquetado += importe;
                } else if (fuente === 1) {
                    totalNoEtiquetado += importe;
                }

                var mesItem = parseInt(item.numeromes || 0);
                var anioItem = parseInt(item.anio || 0);
                if (mesItem === mesActual && anioItem === anioActual) {
                    totalMesActual++;
                }
                if (mesItem === mesActual - 1 && anioItem === anioActual) {
                    totalMesAnterior++;
                }
                if (mesItem === 12 && mesActual === 1 && anioItem === anioActual - 1) {
                    totalMesAnterior++;
                }

                totalPresupuesto += parseFloat(item.PresupuestoCalendarizado || item.presupuestoCalendarizado || 0);
            });

            var trendMensual = 0;
            var trendClase = 'neutral';
            var trendTexto = 'Sin datos previos';
            if (totalMesAnterior > 0) {
                trendMensual = ((totalMesActual - totalMesAnterior) / totalMesAnterior) * 100;
                if (trendMensual > 0) {
                    trendClase = '';
                    trendTexto = '+' + trendMensual.toFixed(0) + '% vs mes anterior';
                } else if (trendMensual < 0) {
                    trendClase = 'negativo';
                    trendTexto = trendMensual.toFixed(0) + '% vs mes anterior';
                } else {
                    trendClase = 'neutral';
                    trendTexto = 'Sin cambio vs mes anterior';
                }
            } else if (totalMesActual > 0) {
                trendClase = '';
                trendTexto = 'Nuevo registro este mes';
            }

            $('#totalMinistraciones').text(total);
            $('#detalleEstatus').html(`
                <span class="badge-status registrada">Reg: ${registradas}</span>
                <span class="badge-status iniciado">Inc: ${iniciados}</span>
                <span class="badge-status cancelada">Can: ${canceladas}</span>
            `);

            $('#totalImporte').text('$' + formatMoney(totalImporte));
            $('#detalleFuente').html(`
                <span class="badge-tipo etiquetado"><i class="bi bi-tag-fill me-1"></i>Etiquetado: $${formatMoney(totalEtiquetado)}</span>
                <span class="badge-tipo no-etiquetado"><i class="bi bi-tag me-1"></i>No Etiquetado: $${formatMoney(totalNoEtiquetado)}</span>
            `);

            $('#totalMesActual').text(totalMesActual);
            $('#trendMensual').html(`<i class="bi bi-arrow-${trendMensual > 0 ? 'up' : trendMensual < 0 ? 'down' : 'right'}-short"></i> ${trendTexto}`)
                .removeClass('negativo neutral')
                .addClass(trendClase);

            $('#totalPresupuesto').text('$' + formatMoney(totalPresupuesto));
            var diferencia = totalPresupuesto - totalImporte;
            var diferenciaClase = 'diferencia-cero';
            var diferenciaIcono = 'bi-dash-circle';
            var diferenciaTexto = 'Diferencia: $0.00 (Cuadra perfectamente)';
            if (diferencia > 0.01) {
                diferenciaClase = 'diferencia-ok';
                diferenciaIcono = 'bi-arrow-up-circle';
                diferenciaTexto = 'Falta por ministrar: $' + formatMoney(diferencia);
            } else if (diferencia < -0.01) {
                diferenciaClase = 'diferencia-error';
                diferenciaIcono = 'bi-arrow-down-circle';
                diferenciaTexto = 'Sobra ministrado: $' + formatMoney(Math.abs(diferencia));
            }
            $('#detalleDiferencia').html(`
                <span class="${diferenciaClase}">
                    <i class="bi ${diferenciaIcono}"></i> ${diferenciaTexto}
                </span>
            `);
        }

        function buscarClaves() {
            var texto = document.getElementById('txtClavePresupuestal').value.trim();
            var autocomplete = document.getElementById('autocompleteResultados');

            if (texto.length < 2) {
                autocomplete.style.display = 'none';
                return;
            }

            $.ajax({
                url: 'api/Ministraciones/getMinistraciones',
                type: 'GET',
                data: { busqueda: texto },
                dataType: 'json',
                success: function (data) {
                    var resultados = data.slice(0, 10);
                    if (resultados.length > 0) {
                        var html = '';
                        resultados.forEach(function (item) {
                            var estatusClass = (item.estatus || 'REGISTRADA').toLowerCase();
                            html += `
                                <div class="autocomplete-item" onclick="seleccionarFila(${item.idministracion})">
                                    <div class="item-main">
                                        <strong>${item.numministracion || 'N/A'}</strong>
                                        <div class="item-folio"><span class="clave-badge">${item.enlace || 'Sin clave'}</span></div>
                                        <div style="font-size:0.65rem; color:#999;">${item.concepto || 'Sin concepto'}</div>
                                    </div>
                                    <div class="item-meta">
                                        $${formatMoney(item.importe)}
                                        <br><span class="badge-status ${estatusClass}">${item.estatus || 'REGISTRADA'}</span>
                                        <br><small>${item.nombremes || ''}</small>
                                    </div>
                                </div>
                            `;
                        });
                        autocomplete.innerHTML = html;
                        autocomplete.style.display = 'block';
                    } else {
                        autocomplete.innerHTML = '<div class="autocomplete-item"><span class="text-muted">No se encontraron resultados</span></div>';
                        autocomplete.style.display = 'block';
                    }
                },
                error: function () {
                    autocomplete.innerHTML = '<div class="autocomplete-item"><span class="text-danger">Error al buscar</span></div>';
                    autocomplete.style.display = 'block';
                }
            });
        }

        function seleccionarFila(id) {
            document.getElementById('autocompleteResultados').style.display = 'none';
            document.getElementById('txtClavePresupuestal').value = '';
            verDetalle(id);
        }

        // =============================================
        // EXPORTAR EXCEL Y PDF
        // =============================================

        function exportarExcel() {
            Swal.fire({ type: 'info', title: 'Exportando...', text: 'Generando archivo Excel', showConfirmButton: false, timer: 1500 });
            window.location.href = 'api/Ministraciones/exportarExcel';
        }

        function verPDF(id) {
            Swal.fire({
                type: 'info',
                title: 'PDF',
                text: 'Visualizando PDF de la ministración ID: ' + id,
                timer: 2000,
                showConfirmButton: false
            });
        }

        // =============================================
        // COMPARACIÓN DEL MES
        // =============================================

        function abrirComparacionMesGlobal() {
            var fechaActual = new Date();
            var mesActual = fechaActual.getMonth() + 1;
            var anioActual = fechaActual.getFullYear();

            var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

            var options = [{ value: '', label: 'Seleccione un mes...' }];
            meses.forEach(function (mes, index) {
                options.push({ value: index + 1, label: mes });
            });

            searchData['selectorMes'].options = options;
            searchData['selectorMes'].selectedValue = mesActual;
            searchData['selectorMes'].selectedLabel = meses[mesActual - 1];

            var modalHTML = `
                <div class="modal fade" id="modalComparacionSelector" tabindex="-1" role="dialog" aria-hidden="true">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                            <div class="modal-header modal-header-custom">
                                <h5 class="modal-title" style="color: white !important; font-size: 0.9rem;">
                                    <i class="bi bi-calculator me-2"></i>Seleccionar Mes y Año
                                </h5>
                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                            </div>
                            <div class="modal-body p-3">
                                <div class="form-group mb-3">
                                    <label class="form-label-sm"><i class="bi bi-calendar-month me-1"></i>Mes</label>
                                    <div class="select-search-wrapper" data-filter="selectorMes" style="width:100%;">
                                        <input type="text" class="select-search-input" placeholder="Seleccione un mes..." 
                                               autocomplete="off" 
                                               onfocus="openSearchDropdown(this)" 
                                               onkeyup="filterSearchOptions(this)" 
                                               onblur="closeSearchDropdownDelayed(this)" 
                                               id="selectorMesInput" 
                                               style="height:38px; min-height:38px; max-height:38px; font-size:0.75rem;">
                                        <div class="select-search-dropdown">
                                            <ul class="select-search-options" id="selectorMesList"></ul>
                                        </div>
                                    </div>
                                    <input type="hidden" id="selectorMesHidden" value="${mesActual}">
                                </div>
                                <div class="form-group">
                                    <label class="form-label-sm"><i class="bi bi-calendar me-1"></i>Año</label>
                                    <input type="number" id="selectorAnio" class="form-control form-control-sm" 
                                           value="${anioActual}" min="2000" max="2100" 
                                           style="height:38px; border-radius:10px; border:1.5px solid #e9ecef; background:#f8f9fa; padding:0.45rem 0.85rem; font-size:0.75rem; font-family:'Inter',sans-serif;">
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">
                                    <i class="bi bi-x-circle me-1"></i>Cancelar
                                </button>
                                <button type="button" class="btn btn-primary btn-sm" onclick="confirmarComparacionMes()" style="background:linear-gradient(135deg,#9F2241 0%,#7a1a32 100%); border:none; color:white;">
                                    <i class="bi bi-check-circle me-1"></i>Comparar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            if ($('#modalComparacionSelector').length) {
                $('#modalComparacionSelector').modal('hide');
                $('#modalComparacionSelector').remove();
            }

            $('body').append(modalHTML);

            document.getElementById('selectorMesInput').value = meses[mesActual - 1];
            document.getElementById('selectorMesHidden').value = mesActual;

            var list = document.getElementById('selectorMesList');
            list.innerHTML = '';

            meses.forEach(function (mes, index) {
                var li = document.createElement('li');
                li.textContent = mes;
                li.dataset.value = index + 1;

                if (index + 1 === mesActual) {
                    li.classList.add('selected');
                }

                li.addEventListener('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    var mesSeleccionado = this.textContent;
                    var valorMes = parseInt(this.dataset.value);

                    document.getElementById('selectorMesInput').value = mesSeleccionado;
                    document.getElementById('selectorMesHidden').value = valorMes;

                    document.querySelectorAll('#selectorMesList li').forEach(function (el) {
                        el.classList.remove('selected');
                    });
                    this.classList.add('selected');

                    var dropdown = document.querySelector('#modalComparacionSelector .select-search-dropdown');
                    if (dropdown) dropdown.classList.remove('show');
                    var wrapper = document.querySelector('#modalComparacionSelector .select-search-wrapper');
                    if (wrapper) wrapper.classList.remove('open');

                    console.log('Mes seleccionado:', mesSeleccionado, 'Valor:', valorMes);
                });

                list.appendChild(li);
            });

            var selectorInput = document.getElementById('selectorMesInput');
            selectorInput.addEventListener('keyup', function () {
                var filter = this.value.toLowerCase();
                var items = document.querySelectorAll('#selectorMesList li');
                items.forEach(function (item) {
                    var text = item.textContent.toLowerCase();
                    item.style.display = text.includes(filter) ? '' : 'none';
                });
            });

            $('#modalComparacionSelector').modal('show');
        }

        function confirmarComparacionMes() {
            var mes = parseInt(document.getElementById('selectorMesHidden').value);
            var anio = parseInt(document.getElementById('selectorAnio').value);

            if (!mes || isNaN(mes) || mes < 1 || mes > 12) {
                Swal.fire('Atención', 'Seleccione un mes válido', 'warning');
                return;
            }

            if (!anio || isNaN(anio) || anio < 2000) {
                Swal.fire('Atención', 'Ingrese un año válido', 'warning');
                return;
            }

            $('#modalComparacionSelector').modal('hide');
            $('#modalComparacionSelector').remove();

            cargarComparacionMes(mes, anio);
        }

        function cargarComparacionMes(mes, anio) {
            var nombreMes = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'][mes - 1];
            $('#tituloComparacionMes').text('(' + nombreMes + ' ' + anio + ')');
            $('#modal-comparacion-mes').modal('show');
            $('#comparacionContent').html(`
                <div class="text-center py-4">
                    <div class="spinner-border text-primary" role="status" style="width:2rem; height:2rem;">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                    <p class="mt-2" style="font-size:0.8rem;">Cargando comparación...</p>
                </div>
            `);

            $.ajax({
                url: 'api/Ministraciones/GetComparacionMes',
                type: 'GET',
                data: { id: 0, mes: mes, anio: anio },
                dataType: 'json',
                success: function (data) {
                    if (data.error) {
                        console.error('Error del servidor:', data.mensaje);
                        $('#comparacionContent').html(`
                            <div class="text-center py-4 text-danger">
                                <i class="bi bi-exclamation-triangle fs-1"></i>
                                <p class="mt-2">Error al cargar la comparación</p>
                                <p class="text-muted" style="font-size:0.7rem;">${data.mensaje || 'Error desconocido'}</p>
                            </div>
                        `);
                        return;
                    }

                    if (!data || !data.claves) {
                        $('#comparacionContent').html(`
                            <div class="text-center py-4 text-warning">
                                <i class="bi bi-exclamation-triangle fs-1"></i>
                                <p class="mt-2">La API no devolvió datos</p>
                            </div>
                        `);
                        return;
                    }

                    var html = '';

                    if (data.claves && data.claves.length > 0) {
                        html += `
                            <h6 style="font-size:0.8rem; font-weight:600; margin-bottom:8px;">
                                <i class="bi bi-list-ul me-1"></i>Claves Presupuestales del Mes
                            </h6>
                            <div class="table-responsive" style="max-height: 55vh; overflow-y: auto; border: 1px solid #e9ecef; border-radius: 4px;">
                                <table class="table table-sm table-striped mb-0">
                                    <thead style="position: sticky; top: 0; z-index: 10; box-shadow: 0 1px 3px rgba(0,0,0,0.1);">
                                        <tr>
                                            <th style="background-color: #f8f9fa; width: 400px;">Clave</th>
                                            <th style="background-color: #f8f9fa;">Descripción</th>
                                            <th class="text-end" style="background-color: #f8f9fa; white-space: nowrap; width: 15%;">Ministrado</th>
                                            <th class="text-end" style="background-color: #f8f9fa; white-space: nowrap; width: 15%;">Monto</th>
                                            <th style="background-color: #f8f9fa; white-space: nowrap; width: 10%;">Mes</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                        `;

                        data.claves.forEach(function (item) {
                            html += `
                                <tr>
                                    <td style="max-width: 400px; word-break: break-all; white-space: normal;"><span class="clave-agrupada">${item.Clave || 'N/A'}</span></td>
                                    <td>${item.Descripcion || 'N/A'}</td>
                                    <td class="text-end" style="color:#28a745; font-weight:600;">
                                        $${formatMoney(item.Ministrado || 0)}
                                    </td>
                                    <td class="text-end" style="color:#6c757d;">
                                        $${formatMoney(item.Monto || 0)}
                                    </td>
                                    <td>${['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'][item.Mes - 1] || item.Mes || 'N/A'}</td>
                                </tr>
                            `;
                        });

                        html += `
                                    </tbody>
                                    <tfoot>
                                        <tr class="fw-bold">
                                            <td colspan="2">TOTALES</td>
                                            <td class="text-end" style="color:#28a745;">
                                                $${formatMoney(data.totalMinistrado || 0)}
                                            </td>
                                            <td class="text-end" style="color:#6c757d;">
                                                $${formatMoney(data.totalPresupuesto || 0)}
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                            <div class="comparacion-divider"></div>
                        `;
                    } else {
                        html += `
                            <div class="text-center py-4 text-muted">
                                <i class="bi bi-inbox fs-1"></i>
                                <p class="mt-2">No hay claves presupuestales para este mes</p>
                            </div>
                        `;
                    }

                    var totalPresupuesto = parseFloat(data.totalPresupuesto) || 0;
                    var totalMinistrado = parseFloat(data.totalMinistrado) || 0;
                    var diffTotal = totalPresupuesto - totalMinistrado;
                    var diffTotalClass = Math.abs(diffTotal) < 0.01 ? 'ok' : 'error';

                    var diffTotalText = '';
                    var diffIcon = '';
                    if (Math.abs(diffTotal) < 0.01) {
                        diffTotalText = 'Los montos coinciden perfectamente';
                        diffIcon = '<i class="bi bi-check-circle-fill me-1"></i>';
                    } else if (diffTotal > 0) {
                        diffTotalText = 'El presupuesto es mayor que lo ministrado por: $' + formatMoney(diffTotal);
                        diffIcon = '<i class="bi bi-exclamation-triangle-fill me-1"></i>';
                    } else {
                        diffTotalText = 'Lo ministrado es mayor que el presupuesto por: $' + formatMoney(Math.abs(diffTotal));
                        diffIcon = '<i class="bi bi-exclamation-triangle-fill me-1"></i>';
                    }

                    html += `
                        <div class="comparacion-totales">
                            <h6 style="font-size:0.8rem; font-weight:600; margin-bottom:10px;">
                                <i class="bi bi-calculator me-1"></i>Resumen de Montos
                            </h6>
                            <div class="total-item">
                                <span class="label">Total Presupuesto</span>
                                <span class="value" style="color:#6c757d;">$${formatMoney(totalPresupuesto)}</span>
                            </div>
                            <div class="total-item">
                                <span class="label">Total Ministrado</span>
                                <span class="value" style="color:#28a745;">$${formatMoney(totalMinistrado)}</span>
                            </div>
                            <div class="comparacion-divider"></div>
                            <div class="total-item diferencia ${diffTotalClass}">
                                <span class="label">Diferencia</span>
                                <span class="value">$${formatMoney(Math.abs(diffTotal))}</span>
                            </div>
                            <div class="text-end mt-2">
                                <span class="badge-estado ${diffTotalClass}">${diffIcon} ${diffTotalText}</span>
                            </div>
                        </div>
                    `;

                    $('#comparacionContent').html(html);
                },
                error: function (xhr, status, error) {
                    console.error('Error en la petición:', error);
                    console.error('Respuesta del servidor:', xhr.responseText);

                    var errorMsg = error || 'Error desconocido';
                    try {
                        var response = JSON.parse(xhr.responseText);
                        if (response && response.Message) {
                            errorMsg = response.Message;
                        }
                    } catch (e) { }

                    $('#comparacionContent').html(`
                        <div class="text-center py-4 text-danger">
                            <i class="bi bi-exclamation-triangle fs-1"></i>
                            <p class="mt-2">Error al cargar la comparación</p>
                            <p class="text-muted" style="font-size:0.7rem;">${errorMsg}</p>
                        </div>
                    `);
                }
            });
        }

        // =============================================
        // DETALLE DE MINISTRACIÓN
        // =============================================

        function verDetalle(id) {
            $('#modal-detalle').modal('show');

            $('#detalleContent').html(`
                <div class="text-center py-4">
                    <div class="spinner-border text-primary" role="status" style="width:2rem; height:2rem;">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                    <p class="mt-2" style="font-size:0.8rem;">Cargando detalles...</p>
                </div>
            `);

            $.ajax({
                url: 'api/Ministraciones/GetMinistracionDetalleCompleto?id=' + id,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    mostrarDetalleMinistracion(data);
                },
                error: function () {
                    $('#detalleContent').html(`
                        <div class="text-center py-4 text-danger">
                            <i class="bi bi-exclamation-triangle fs-1"></i>
                            <p style="font-size:0.8rem;">Error al cargar los detalles</p>
                        </div>
                    `);
                }
            });
        }

        function mostrarDetalleMinistracion(data) {
            var mesActual = parseInt(document.getElementById('hddMesActual').value) || 1;
            
            // Basic data
            var folio = data.numministracion || data.Folio || 'N/A';
            var anio = data.anio || data.Anio || 'N/A';
            var mes = data.nombremes || 'N/A';
            var estatus = data.estatus || data.Estatus || 'REGISTRADA';
            var fechaRegistro = data.fecharegistro || 'N/A';
            var concepto = data.concepto || data.Concepto || 'Sin concepto';
            var importe = data.importe || data.ImporteTotal || 0;
            var presupuesto = data.PresupuestoCalendarizado || data.presupuestoCalendarizado || 0;
            var clavePresupuestal = data.enlace || data.ClavePresupuestal || 'N/A';

            // Helpers for clave - descripcion format
            // Helpers for clave - descripcion format
            function createClaveDescHTML(baseLabel, clave, desc, descSpan) {
                descSpan = descSpan || 1;
                var c = (!clave || clave === 'N/A' || clave.trim() === '') ? 'N/A' : clave;
                var d = (!desc || desc === 'N/A' || desc.trim() === '') ? 'N/A' : desc;
                var ds = descSpan > 1 ? ` style="grid-column: span ${descSpan};"` : '';
                return `
                    <div class="detalle-item">
                        <div class="label">Clave ${baseLabel}</div>
                        <div class="value" style="font-size:0.85rem;">${c}</div>
                    </div>
                    <div class="detalle-item"${ds}>
                        <div class="label">${baseLabel}</div>
                        <div class="value">${d}</div>
                    </div>
                `;
            }

            var tipoFolio = createClaveDescHTML('Tipo Folio', data.tipoMinistracionFolioClave || data.TipoMinistracionFolioClave, data.tipoMinistracionFolioDescripcion || data.TipoMinistracionFolioDescripcion, 2);
            var tipoMinistracion = createClaveDescHTML('Tipo Ministración', data.tipoClave, data.tipoDescripcion || data.tipoministracion, 3);
            var digito = createClaveDescHTML('Dígito Ministración', data.digitoClave || data.digitoDM, data.digitoDescripcion || data.digitoMinistracion, 1);
            var subFuente = createClaveDescHTML('Sub Fuente Financiamiento', data.subFuenteClave || data.SubFuenteClave, data.subFuenteDescripcion || data.subFuenteFinanciamiento || data.SubFuenteDescripcion, 1);
            var programaFondo = createClaveDescHTML('Programa Fondo', data.programaFondoClave || data.ProgramaFondoClave, data.programaFondoDescripcion || data.ProgramaFondoDescripcion, 1);
            var proyecto = createClaveDescHTML('Proyecto Estratégico', data.proyectoEstrategicoClave || data.ProyectoEstrategicoClave, data.proyectoEstrategicoDescripcion || data.proyectoEstrategico || data.ProyectoEstrategicoDescripcion, 1);
            
            // For TR, FF, Ramo we use the specific ones if they are available
            var trGeneral = createClaveDescHTML('TR (Tipo de Recurso)', data.tipoRecursoClave || data.TipoRecursoClave, data.tipoRecursoDescripcion || data.tipoRecurso || data.TipoRecursoDescripcion, 1);
            var ffGeneral = createClaveDescHTML('FF (Fuente Financiamiento)', data.fuenteClave || data.FuenteClave, data.fuenteDescripcion || data.fuenteFinanciamiento || data.FuenteDescripcion, 1);
            var ramoGeneral = createClaveDescHTML('Ramo', data.ramoClave || data.RamoClave, data.ramoDescripcion || data.ramo || data.RamoDescripcion, 1);

            var html = `
                <div class="detalle-grid">
                    <!-- SECCIÓN: DATOS GENERALES -->
                    <div style="grid-column: 1 / -1; margin-top: 5px; margin-bottom: 5px; border-bottom: 1px solid #dee2e6; padding-bottom: 3px;">
                        <h6 style="font-size: 0.75rem; color: #495057; font-weight: bold; margin: 0; text-transform: uppercase; letter-spacing: 0.5px;">
                            <i class="bi bi-info-circle me-1"></i> Datos Generales
                        </h6>
                    </div>
                    
                    <div class="detalle-item">
                        <div class="label">Folio</div>
                        <div class="value" style="color: #5e72e4; font-weight: bold;">${folio}</div>
                    </div>
                    <div class="detalle-item">
                        <div class="label">Estatus</div>
                        <div class="value"><span class="badge-status ${estatus.toLowerCase()}">${estatus}</span></div>
                    </div>
                    <div class="detalle-item">
                        <div class="label">Año</div>
                        <div class="value">${anio}</div>
                    </div>
                    <div class="detalle-item">
                        <div class="label">Mes</div>
                        <div class="value">${mes}</div>
                    </div>
                    ${tipoFolio}
                </div>

                <!-- SECCIÓN: CLASIFICACIÓN PRESUPUESTAL -->
                <div style="margin-top: 10px; margin-bottom: 5px; border-bottom: 1px solid #dee2e6; padding-bottom: 3px;">
                    <h6 style="font-size: 0.75rem; color: #495057; font-weight: bold; margin: 0; text-transform: uppercase; letter-spacing: 0.5px;">
                        <i class="bi bi-tags me-1"></i> Clasificación Presupuestal
                    </h6>
                </div>
                
                <div class="detalle-grid" style="grid-template-columns: repeat(4, 1fr); margin-bottom: 10px;">
                    <!-- FILA 1 -->
                    <div class="detalle-item" style="grid-column: span 2;">
                        <div class="label">Clave Presupuestal</div>
                        <div class="value" style="font-size:0.85rem; word-break: break-all; max-height: 60px; overflow-y: auto;">${clavePresupuestal}</div>
                    </div>
                    ${programaFondo}
                    
                    <!-- FILA 2 -->
                    ${trGeneral}
                    ${ffGeneral}
                    
                    <!-- FILA 3 -->
                    ${ramoGeneral}
                    ${subFuente}
                    
                    <!-- FILA 4 -->
                    ${tipoMinistracion}
                    
                    <!-- FILA 5 -->
                    ${digito}
                    ${proyecto}
                </div>

                <div class="detalle-grid">
                    <!-- SECCIÓN: MONTOS Y CONCEPTOS -->
                    <div style="grid-column: 1 / -1; margin-top: 10px; margin-bottom: 5px; border-bottom: 1px solid #dee2e6; padding-bottom: 3px;">
                        <h6 style="font-size: 0.75rem; color: #495057; font-weight: bold; margin: 0; text-transform: uppercase; letter-spacing: 0.5px;">
                            <i class="bi bi-cash-stack me-1"></i> Concepto y Montos
                        </h6>
                    </div>

                    <div class="detalle-item" style="grid-column: span 2;">
                        <div class="label">Concepto</div>
                        <div class="value" style="font-size:0.8rem; max-height: 100px; overflow-y: auto; word-break: normal;">${concepto}</div>
                    </div>
                    <div class="detalle-item">
                        <div class="label">Importe Total</div>
                        <div class="value" style="color:#28a745; font-size:1rem;">$${formatMoney(data.importe)}</div>
                    </div>
                    <div class="detalle-item">
                        <div class="label">Presupuesto Calendarizado</div>
                        <div class="value" style="color:#6c757d; font-size:0.9rem;">$${formatMoney(data.PresupuestoCalendarizado || data.presupuestoCalendarizado || 0)}</div>
                    </div>

                    <!-- SECCIÓN: METADATOS -->
                    <div style="grid-column: 1 / -1; margin-top: 10px; margin-bottom: 5px; border-bottom: 1px solid #dee2e6; padding-bottom: 3px;">
                        <h6 style="font-size: 0.75rem; color: #495057; font-weight: bold; margin: 0; text-transform: uppercase; letter-spacing: 0.5px;">
                            <i class="bi bi-clock-history me-1"></i> Registro y Documentos
                        </h6>
                    </div>

                    <div class="detalle-item">
                        <div class="label">Fecha Registro</div>
                        <div class="value" style="font-size:0.75rem;">${data.fecharegistro || 'N/A'}</div>
                    </div>
                    ${data.fechaModificacion ? `
                    <div class="detalle-item">
                        <div class="label">Última Modificación</div>
                        <div class="value" style="font-size:0.75rem;">${data.fechaModificacion}<br><small>${data.usuarioModificacion || ''}</small></div>
                    </div>` : ''}
                    <div class="detalle-item">
                        <div class="label">Documento PDF</div>
                        <div class="value" style="margin-top: 3px;">
                            <button class="btn ${data.tienePDF ? 'btn-pdf' : 'btn-pdf sin-pdf'}" onclick="${data.tienePDF ? 'verPDF(' + data.idministracion + ')' : ''}" ${!data.tienePDF ? 'disabled' : ''} style="padding:4px 12px; font-size:0.7rem; margin:0;">
                                <i class="bi bi-file-pdf me-1"></i>${data.tienePDF ? 'Ver PDF' : 'Sin PDF'}
                            </button>
                        </div>
                    </div>
                </div>
            `;

            var presupuestoTotal = parseFloat(data.PresupuestoCalendarizado || data.presupuestoCalendarizado || 0);
            var ministradoTotal = parseFloat(data.importe || 0);
            var diferencia = presupuestoTotal - ministradoTotal;
            var diferenciaClass = 'cero';
            var diferenciaText = 'Cuadra perfectamente';

            if (diferencia > 0.01) {
                diferenciaClass = 'positiva';
                diferenciaText = 'Faltan por ministrar: $' + formatMoney(diferencia);
            } else if (diferencia < -0.01) {
                diferenciaClass = 'negativa';
                diferenciaText = 'Sobra ministrado: $' + formatMoney(Math.abs(diferencia));
            }

            var desgloseHtml = '';
            var desgloseArr = data.desgloseClaves || data.DesgloseClaves || [];
            if (desgloseArr.length > 0) {
                desgloseHtml += `
                <div class="comparacion-card" style="margin-bottom: 10px;">
                    <h6 style="font-size:0.7rem; font-weight:600; color:#495057;">
                        <i class="bi bi-list-check me-1"></i>Desglose Presupuesto Calendarizado
                    </h6>
                    <div style="max-height: 120px; overflow-y: auto;">
                        <table class="table table-sm table-striped mt-2" style="font-size: 0.75rem; margin-bottom: 0;">
                            <thead>
                                <tr>
                                    <th>Clave Presupuestal</th>
                                    <th class="text-end">Presupuesto</th>
                                </tr>
                            </thead>
                            <tbody>
                `;
                desgloseArr.forEach(function(item) {
                    desgloseHtml += `
                                <tr>
                                    <td style="word-break: break-all;">${item.clavePresupuestal || item.ClavePresupuestal}</td>
                                    <td class="text-end">$${formatMoney(item.monto || item.Monto || 0)}</td>
                                </tr>
                    `;
                });
                desgloseHtml += `
                            </tbody>
                        </table>
                    </div>
                </div>
                `;
            }

            html += desgloseHtml + `
                <div class="comparacion-card">
                    <h6 style="font-size:0.7rem; font-weight:600; color:#495057;">
                        <i class="bi bi-calculator me-1"></i>Comparación Presupuesto vs Ministrado
                        <span class="badge-validacion ${diferenciaClass === 'cero' ? 'ok' : 'error'} ms-2">
                            ${diferenciaClass === 'cero' ? '✓ Validado' : '⚠ Revisar'}
                        </span>
                    </h6>
                    <div class="comparacion-item">
                        <span class="label">Presupuesto Calendarizado Total</span>
                        <span class="value">$${formatMoney(presupuestoTotal)}</span>
                    </div>
                    <div class="comparacion-item">
                        <span class="label">Importe Ministrado</span>
                        <span class="value">$${formatMoney(ministradoTotal)}</span>
                    </div>
                    <div class="comparacion-divider"></div>
                    <div class="comparacion-item diferencia ${diferenciaClass}">
                        <span class="label">Diferencia</span>
                        <span class="value">${diferenciaText}</span>
                    </div>
                </div>
            `;

            $('#detalleContent').html(html);
        }

        // =============================================
        // MODAL DE CLAVES PRESUPUESTALES
        // =============================================

        var modalClavesModo = 'normal';

        function abrirModalClaves() {
            modalClavesModo = 'normal';
            var mes = $('#cbomescaptura').val() || '';
            var nombreMes = mes ? $('#cbomescaptura option:selected').text() : '';
            $('#tituloClavesModal').text('(Claves del mes ' + (nombreMes || 'seleccionado') + ')');
            $('#modal-claves').modal('show');
            cargarClavesModal();
        }

        function cargarClavesModal() {
            var mes = $('#cbomescaptura').val() || '';
            var anio = $('#txtAnio').val() || new Date().getFullYear();

            if (!mes) {
                var hoy = new Date();
                mes = hoy.getMonth() + 1;
                $('#cbomescaptura').val(mes);
            }

            $('#clavesBodyModal').html('<tr><td colspan="6" class="text-center"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div></td></tr>');

            $.ajax({
                url: 'api/Ministraciones/GetClavesPresupuestalesPorMes',
                type: 'GET',
                data: { mes: mes, anio: anio },
                dataType: 'json',
                success: function (data) {
                    var html = '';
                    if (data && data.length > 0) {
                        data.forEach(function (item) {
                            var fuenteDisplay = item.fuenteFinanciamiento || 'No Etiquetado';
                            var badgeClass = 'bg-secondary';
                            if (fuenteDisplay === 'Etiquetado' || item.fuenteFinanciamientoId === 2) {
                                badgeClass = 'bg-success';
                                fuenteDisplay = 'Etiquetado';
                            } else {
                                badgeClass = 'bg-secondary';
                                fuenteDisplay = 'No Etiquetado';
                            }
                            var nombreMes = item.nombremes || obtenerNombreMes(parseInt(item.numeromes)) || 'N/A';
                            html += `
                                <tr data-clave="${item.clavePresupuestal || ''}" data-mes="${item.numeromes || ''}" data-fuente="${fuenteDisplay}">
                                    <td><input type="checkbox" name="chkClaveModal" value="${item.clavePresupuestal || ''}" data-monto="${item.monto || 0}" data-desc="${item.descripcion || ''}" /></td>
                                    <td><span class="clave-item">${item.clavePresupuestal || 'N/A'}</span></td>
                                    <td>${item.descripcion || 'Sin descripción'}</td>
                                    <td><span class="badge ${badgeClass}">${fuenteDisplay}</span></td>
                                    <td>${nombreMes}</td>
                                    <td class="text-end">$${formatMoney(item.monto)}</td>
                                </tr>
                            `;
                        });
                    } else {
                        html = `<tr><td colspan="6" class="text-center text-muted">No hay claves disponibles para el mes seleccionado</td></tr>`;
                    }
                    $('#clavesBodyModal').html(html);
                    $('#cbMesFiltroClave').val('');

                    // Pre-select existing claves
                    var currentClaves = ($('#txtClavePresupuestalModal').val() || '').split(',').map(function(c) { return c.trim(); });
                    if (currentClaves.length > 0 && currentClaves[0] !== '') {
                        $('#clavesBodyModal input[name="chkClaveModal"]').each(function() {
                            if (currentClaves.includes($(this).val())) {
                                $(this).prop('checked', true);
                            }
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error al cargar claves:', error);
                    $('#clavesBodyModal').html(`<tr><td colspan="6" class="text-center text-danger">Error al cargar claves: ${error}</td></tr>`);
                }
            });
        }

        function obtenerNombreMes(numero) {
            var meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
                'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
            ];
            return meses[numero - 1] || 'N/A';
        }

        function filtrarClavesModal() {
            var filtro = $('#txtFiltroClave').val().toLowerCase();

            $('#clavesBodyModal tr').each(function () {
                var clave = $(this).data('clave') || '';
                var mostrar = true;

                if (filtro && !clave.toLowerCase().includes(filtro)) mostrar = false;

                $(this).toggle(mostrar);
            });
        }

        function toggleAllClavesModal() {
            var checked = $('#chkSeleccionarTodos').is(':checked');
            $('input[name="chkClaveModal"]:visible').prop('checked', checked);
        }

        function seleccionarClaveModal() {
            var selected = $('input[name="chkClaveModal"]:checked');
            if (selected.length === 0) {
                Swal.fire('Atención', 'Seleccione al menos una clave presupuestal', 'warning');
                return;
            }

            var claves = [];
            var montoTotal = 0;
            var desc = "";

            selected.each(function() {
                var chk = $(this);
                claves.push(chk.val());
                montoTotal += parseFloat(chk.data('monto') || 0);
                if (desc === "") {
                    desc = chk.data('desc') || chk.closest('tr').find('td:eq(2)').text();
                }
            });

            var claveFinal = claves.join(', ');
            var descrToUse = selected.length > 1 ? (selected.length + " Claves Seleccionadas") : desc;

            $('#txtClavePresupuestalModal').val(claveFinal);
            $('#txtPresupuestoAsociado').val('Presupuesto: $' + formatMoney(montoTotal) + ' - ' + descrToUse);

            if (parseFloat($('#txtmontoautorizado').val().replace(/,/g, '')) <= 0) {
                $('#txtmontoautorizado').val(formatMoney(montoTotal));
            }

            $('#modal-claves').modal('hide');

            Swal.fire({
                icon: 'success',
                title: selected.length > 1 ? 'Claves seleccionadas' : 'Clave seleccionada',
                text: descrToUse || 'Claves presupuestales seleccionadas',
                timer: 1500,
                showConfirmButton: false
            });
        }

        // =============================================
        // GENERACIÓN AUTOMÁTICA DE FOLIO
        // =============================================

        $(document).on('change', '#cbTipoFolio, #txtAnio, #cbomescaptura', function () {
            generarFolioAutomatico();
        });

        $(document).on('change', '#cbomescaptura, #txtAnio', function () {
            var tipoFolioId = $('#cbTipoFolio').val();
            if (tipoFolioId) {
                generarFolioAutomatico();
            }
        });

        function generarFolioAutomatico() {
            var tipoFolioId = $('#cbTipoFolio').val();
            if (!tipoFolioId) {
                $('#txtnumministracion').val('');
                return;
            }

            var option = modalSearchData.tipoFolioModal.options.find(function (o) { return o.value == tipoFolioId; });
            if (!option) return;

            var clave = option.label.split(' - ')[0];
            var anio = $('#txtAnio').val() || new Date().getFullYear();
            var mes = $('#cbomescaptura').val() || (new Date().getMonth() + 1);
            var mesStr = mes.toString().padStart(2, '0');

            $('#txtnumministracion').val('Generando...');

            $.ajax({
                url: 'api/Ministraciones/GetSiguienteFolio?anio=' + anio + '&mes=' + mes + '&idTipoFolio=' + tipoFolioId,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    if (data && data.folio) {
                        $('#txtnumministracion').val(data.folio);
                    } else {
                        fallbackFolio();
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error al generar folio:', error);
                    fallbackFolio();
                }
            });

            function fallbackFolio() {
                var anioStr = anio.toString();
                var timestamp = Date.now().toString().substr(-3);
                var folio = anioStr + mesStr + '-' + clave + timestamp;
                $('#txtnumministracion').val(folio);
            }
        }

        // =============================================
        // ACTUALIZAR RESUMEN DE CAMPOS
        // =============================================

        function actualizarResumenCampos() {
            var campos = [];

            var tipoFolio = $('#cbTipoFolio').val();
            if (tipoFolio) {
                var option = modalSearchData.tipoFolioModal.options.find(function (o) { return o.value == tipoFolio; });
                if (option) campos.push('Tipo Folio: ' + option.label);
            }

            var subFuente = $('#cbSubFuente').val();
            if (subFuente) {
                var option = modalSearchData.subFuenteModal.options.find(function (o) { return o.value == subFuente; });
                if (option) campos.push('Sub Fuente: ' + option.label);
            }

            var proyecto = $('#cbProyectoEstrategico').val();
            if (proyecto) {
                var option = modalSearchData.proyectoEstrategicoModal.options.find(function (o) { return o.value == proyecto; });
                if (option) campos.push('Proyecto: ' + option.label);
            }

            if (campos.length > 0) {
                $('#lblResumenCampos').text('Campos seleccionados: ' + campos.join(' | '));
            } else {
                $('#lblResumenCampos').text('Seleccione los campos para completar la ministración');
            }
        }

        $(document).on('change', '#cbTipoFolio, #cbSubFuente, #cbProyectoEstrategico', function () {
            actualizarResumenCampos();
        });

        // =============================================
        // CRUD
        // =============================================

        function Modalministracion() {
            $('#hddaccion').val('NUEVO');
            $('#hddIdMinistracion').val('');
            $('#modalTitle').text('Nueva Ministración');

            // Limpiar campos existentes
            $('#txtnumministracion').val('');
            $('#txtAnio').val(new Date().getFullYear());
            $('#txtconcepto').val('');
            $('#txtClavePresupuestalModal').val('');
            $('#txtPresupuestoAsociado').val('');
            $('#txtmontoautorizado').val('');
            $('#cbomescaptura').val('');
            $('#cbotipoministracion').val('');
            $('#cbDigitoMinistracion').val('');
            $('#cbProgramaFondoModal').val('');
            $('#txtTR').val('-');
            $('#txtFF').val('-');
            $('#txtRamo').val('-');
            $('#txtClavePF').val('-');
            $('#cbEstatusModal').val('REGISTRADA');

            // Limpiar nuevos campos
            $('#cbTipoFolio').val('');
            $('#cbSubFuente').val('');
            $('#cbProyectoEstrategico').val('');

            // Resetear inputs visuales
            document.querySelectorAll('[data-filter="tipoFolioModal"] .select-search-input').forEach(function (el) { el.value = ''; });
            document.querySelectorAll('[data-filter="subFuenteModal"] .select-search-input').forEach(function (el) { el.value = ''; });
            document.querySelectorAll('[data-filter="proyectoEstrategicoModal"] .select-search-input').forEach(function (el) { el.value = ''; });

            // Resetear resumen
            $('#lblResumenCampos').text('Seleccione los campos para completar la ministración');

            var tipoFolioId = $('#cbTipoFolio').val();
            if (tipoFolioId) {
                generarFolioAutomatico();
            }

            $('#modal-form').modal('show');
        }

        function editarMinistracion(id) {
            $('#hddaccion').val('EDITAR');
            $('#hddIdMinistracion').val(id);
            $('#modalTitle').text('Editar Ministración');

            $.ajax({
                url: 'api/Ministraciones/GetMinistracionById?id=' + id,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    // Campos existentes
                    $('#txtnumministracion').val(data.numministracion || data.Folio || '');
                    $('#txtAnio').val(data.anio || new Date().getFullYear());
                    
                    var mesVal = data.numeromes || data.Mes || '';
                    if (mesVal) {
                        $('#cbomescaptura').val(mesVal);
                        var wrapper = document.querySelector('[data-filter="mesModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = searchData.mesModal.options.find(function (o) { return o.value == mesVal; });
                            if (option) input.value = option.label;
                        }
                    } else {
                        $('#cbomescaptura').val('');
                    }

                    var tmId = data.TipoMinistracionId || data.tipoMinistracionId || data.id_tipo_ministracion;
                    if (tmId) {
                        $('#cbotipoministracion').val(tmId);
                        var wrapper = document.querySelector('[data-filter="tipoModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = modalSearchData.tipoModal.options.find(function (o) { return o.value == tmId; });
                            if (option) input.value = option.label;
                        }
                    } else {
                        $('#cbotipoministracion').val('');
                    }

                    var dmId = data.DigitoMinistracionId || data.digitoMinistracionId || data.id_digito_ministracion;
                    if (dmId) {
                        $('#cbDigitoMinistracion').val(dmId);
                        var wrapper = document.querySelector('[data-filter="digitoModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = searchData.digitoModal.options.find(function (o) { return o.value == dmId; });
                            if (option) input.value = option.label;
                        }
                    } else {
                        $('#cbDigitoMinistracion').val('');
                    }

                    $('#txtClavePresupuestalModal').val(data.enlace || data.ClavePresupuestal || '');
                    $('#txtconcepto').val(data.concepto || '');
                    $('#txtmontoautorizado').val(formatMoney(data.importe) || '');
                    
                    var estatusVal = data.estatus || 'REGISTRADA';
                    $('#cbEstatusModal').val(estatusVal);
                    var wrapperEstatus = document.querySelector('[data-filter="estatusModal"]');
                    if (wrapperEstatus) {
                        var inputEstatus = wrapperEstatus.querySelector('.select-search-input');
                        var optionEstatus = searchData.estatusModal.options.find(function (o) { 
                            return o.value && o.value.toString().toLowerCase() === estatusVal.toString().toLowerCase(); 
                        });
                        if (optionEstatus) inputEstatus.value = optionEstatus.label;
                    }

                    // Nuevos campos editables
                    if (data.id_tipoministracion_folio) {
                        $('#cbTipoFolio').val(data.id_tipoministracion_folio);
                        var wrapper = document.querySelector('[data-filter="tipoFolioModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = modalSearchData.tipoFolioModal.options.find(function (o) { return o.value == data.id_tipoministracion_folio; });
                            if (option) input.value = option.label;
                        }
                    }

                    if (data.Id_sub_fuente_financiamiento) {
                        $('#cbSubFuente').val(data.Id_sub_fuente_financiamiento);
                        var wrapper = document.querySelector('[data-filter="subFuenteModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = modalSearchData.subFuenteModal.options.find(function (o) { return o.value == data.Id_sub_fuente_financiamiento; });
                            if (option) input.value = option.label;
                        }
                    }

                    if (data.Id_proyecto_estrategico) {
                        $('#cbProyectoEstrategico').val(data.Id_proyecto_estrategico);
                        var wrapper = document.querySelector('[data-filter="proyectoEstrategicoModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = modalSearchData.proyectoEstrategicoModal.options.find(function (o) { return o.value == data.Id_proyecto_estrategico; });
                            if (option) input.value = option.label;
                        }
                    }

                    var pfId = data.ProgramaFondoId || data.programaFondoId || data.id_programa_fondo;
                    if (pfId) {
                        $('#cbProgramaFondoModal').val(pfId);
                        var wrapper = document.querySelector('[data-filter="programaFondoModal"]');
                        if (wrapper) {
                            var input = wrapper.querySelector('.select-search-input');
                            var option = modalSearchData.programaFondoModal.options.find(function (o) { return o.value == pfId; });
                            if (option) input.value = option.label;
                        }
                    }

                    actualizarResumenCampos();

                    // Cargar detalles del programa fondo
                    if (pfId) {
                        $.ajax({
                            url: 'api/Ministraciones/GetProgramaFondoById?id=' + pfId,
                            type: 'GET',
                            dataType: 'json',
                            success: function (pfData) {
                                var trDisplay = (pfData.TR || pfData.tr || '');
                                if (pfData.TipoRecurso) {
                                    trDisplay += ' - ' + pfData.TipoRecurso;
                                }
                                $('#txtTR').val(trDisplay || '-');

                                var ffDisplay = (pfData.FF || pfData.ff || '');
                                if (pfData.FuenteFinanciamiento) {
                                    ffDisplay += ' - ' + pfData.FuenteFinanciamiento;
                                }
                                $('#txtFF').val(ffDisplay || '-');

                                var ramoDisplay = (pfData.Ramo || pfData.ramo || '');
                                if (pfData.DescripcionRamo) {
                                    ramoDisplay += ' - ' + pfData.DescripcionRamo;
                                }
                                $('#txtRamo').val(ramoDisplay || '-');

                                $('#txtClavePF').val(pfData.Clave || pfData.clave || '-');
                            },
                            error: function () {
                                $('#txtTR').val('-');
                                $('#txtFF').val('-');
                                $('#txtRamo').val('-');
                                $('#txtClavePF').val('-');
                            }
                        });
                    } else {
                        $('#txtTR').val('-');
                        $('#txtFF').val('-');
                        $('#txtRamo').val('-');
                        $('#txtClavePF').val('-');
                    }

                    if (data.PresupuestoCalendarizado) {
                        $('#txtPresupuestoAsociado').val('Presupuesto: $' + formatMoney(data.PresupuestoCalendarizado));
                    } else {
                        $('#txtPresupuestoAsociado').val('');
                    }

                    if ($('#cbTipoFolio').val()) {
                        generarFolioAutomatico();
                    }

                    $('#modal-form').modal('show');
                },
                error: function (xhr, status, error) {
                    console.error('Error al cargar datos:', error);
                    showNotification('Error al cargar los datos', 'error');
                }
            });
        }

        function guardarMinistracion() {
            var data = {
                accion: $('#hddaccion').val(),
                id: parseInt($('#hddIdMinistracion').val()) || 0,
                folio: $('#txtnumministracion').val().trim(),
                mesAplicacion: parseInt($('#cbomescaptura').val()) || 0,
                anio: parseInt($('#txtAnio').val()) || new Date().getFullYear(),
                tipoMinistracionId: parseInt($('#cbotipoministracion').val()) || null,
                digitoMinistracionId: parseInt($('#cbDigitoMinistracion').val()) || null,
                programaFondoId: parseInt($('#cbProgramaFondoModal').val()) || null,
                clavePresupuestal: $('#txtClavePresupuestalModal').val().trim(),
                concepto: $('#txtconcepto').val().trim(),
                importeTotal: $('#txtmontoautorizado').val().replace(/,/g, ''),
                estatus: $('#cbEstatusModal').val(),
                id_tipoministracion_folio: parseInt($('#cbTipoFolio').val()) || null,
                Id_sub_fuente_financiamiento: parseInt($('#cbSubFuente').val()) || null,
                Id_proyecto_estrategico: parseInt($('#cbProyectoEstrategico').val()) || null
            };

            if (!data.folio) {
                showNotification('El folio es requerido', 'error');
                return;
            }
            if (!data.mesAplicacion) {
                showNotification('Seleccione un mes', 'error');
                return;
            }
            if (!data.anio || data.anio < 2000) {
                showNotification('Ingrese un año válido', 'error');
                return;
            }
            if (!data.clavePresupuestal) {
                showNotification('Seleccione una clave presupuestal', 'error');
                return;
            }
            if (!data.importeTotal || parseFloat(data.importeTotal) <= 0) {
                showNotification('Ingrese un importe total válido', 'error');
                return;
            }

            if (data.programaFondoId) {
                var tr = $('#txtTR').val();
                var ff = $('#txtFF').val();
                var ramo = $('#txtRamo').val();
                if (tr === '-' || ff === '-' || ramo === '-') {
                    showNotification('El Programa Fondo seleccionado no tiene datos completos', 'warning');
                }
            }

            console.log('Datos a guardar:', data);

            $.ajax({
                url: 'api/Ministraciones/GuardarMinistracion',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                dataType: 'json',
                beforeSend: function () {
                    $('#loadingOverlay').show();
                },
                success: function (response) {
                    $('#loadingOverlay').hide();
                    if (response.success) {
                        showNotification(response.message || 'Guardado exitosamente', 'success');
                        $('#modal-form').modal('hide');
                        filtrar();
                    } else {
                        showNotification(response.message || 'Error al guardar', 'error');
                    }
                },
                error: function (xhr, status, error) {
                    $('#loadingOverlay').hide();
                    console.error('Error al guardar:', error);
                    console.error('Respuesta:', xhr.responseText);

                    var errorMsg = 'Error al guardar la ministración';
                    try {
                        var response = JSON.parse(xhr.responseText);
                        if (response && response.message) {
                            errorMsg = response.message;
                        }
                    } catch (e) { }

                    showNotification(errorMsg, 'error');
                }
            });
        }

        // =============================================
        // EVENTO: PROGRAMA FONDO AUTO-COMPLETAR
        // =============================================

        // Escuchar el cambio del hidden input (donde se guarda el valor real)
        $('#modal-form').on('change', '#cbProgramaFondoModal', function () {
            var id = $(this).val();
            console.log('🔍 Programa Fondo seleccionado - ID:', id);

            if (id) {
                console.log('📡 Haciendo peticion a: api/Ministraciones/GetProgramaFondoById?id=' + id);

                $.ajax({
                    url: 'api/Ministraciones/GetProgramaFondoById?id=' + id,
                    type: 'GET',
                    dataType: 'json',
                    success: function (data) {
                        console.log('✅ Datos recibidos del PF:', data);

                        // TR (Tipo Recurso) - Clave + Descripcion
                        var trDisplay = (data.TR || data.tr || '');
                        if (data.TipoRecurso) {
                            trDisplay += ' - ' + data.TipoRecurso;
                        }
                        $('#txtTR').val(trDisplay || '-');
                        console.log('📝 TR asignado:', $('#txtTR').val());

                        // FF (Fuente Financiamiento) - Clave + Descripcion
                        var ffDisplay = (data.FF || data.ff || '');
                        if (data.FuenteFinanciamiento) {
                            ffDisplay += ' - ' + data.FuenteFinanciamiento;
                        }
                        $('#txtFF').val(ffDisplay || '-');
                        console.log('📝 FF asignado:', $('#txtFF').val());

                        // Ramo - Clave + Descripcion
                        var ramoDisplay = (data.Ramo || data.ramo || '');
                        if (data.DescripcionRamo) {
                            ramoDisplay += ' - ' + data.DescripcionRamo;
                        }
                        $('#txtRamo').val(ramoDisplay || '-');
                        console.log('📝 Ramo asignado:', $('#txtRamo').val());

                        // Clave PF - Solo la clave
                        $('#txtClavePF').val(data.Clave || data.clave || '-');
                        console.log('📝 Clave PF asignado:', $('#txtClavePF').val());
                    },
                    error: function (xhr, status, error) {
                        console.error('❌ Error al cargar detalles del PF:', error);
                        console.error('📄 Respuesta:', xhr.responseText);
                        $('#txtTR').val('-');
                        $('#txtFF').val('-');
                        $('#txtRamo').val('-');
                        $('#txtClavePF').val('-');
                    }
                });
            } else {
                console.log('🔄 Limpiando campos de Programa Fondo');
                $('#txtTR').val('-');
                $('#txtFF').val('-');
                $('#txtRamo').val('-');
                $('#txtClavePF').val('-');
            }
        });

        // TAMBIÉN agregar un evento para cuando se selecciona en el dropdown visual
        // Esto captura el momento exacto en que el usuario hace clic en una opción
        $(document).on('click', '.select-search-wrapper[data-filter="programaFondoModal"] .select-search-options li', function () {
            var value = $(this).data('value');
            var hidden = $('#cbProgramaFondoModal');

            // Si el valor es diferente al actual, forzar el cambio
            if (value !== hidden.val()) {
                console.log('🖱️ Opción seleccionada en dropdown:', value);
                hidden.val(value);
                hidden.trigger('change');
            }
        });

        // También capturar cuando se escribe y se selecciona con Enter
        $(document).on('keydown', '.select-search-wrapper[data-filter="programaFondoModal"] .select-search-input', function (e) {
            if (e.key === 'Enter') {
                var wrapper = $(this).closest('.select-search-wrapper');
                var selected = wrapper.find('.select-search-options li.selected');
                if (selected.length > 0) {
                    var value = selected.data('value');
                    var hidden = $('#cbProgramaFondoModal');
                    if (value !== hidden.val()) {
                        console.log('⌨️ Selección con Enter:', value);
                        hidden.val(value);
                        hidden.trigger('change');
                    }
                }
            }
        });

        // =============================================
        // APROBAR MINISTRACIÓN
        // =============================================

        function aprobarMinistracion(id) {
            Swal.fire({
                title: '¿Iniciar ministración?',
                text: 'La ministración pasará a estatus INICIADO',
                type: 'question',
                showCancelButton: true,
                confirmButtonColor: '#28a745',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Sí, iniciar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: 'api/Ministraciones/AprobarMinistracion?id=' + id,
                        type: 'POST',
                        success: function (response) {
                            if (response.success) {
                                showNotification('Ministración iniciada', 'success');
                                filtrar();
                            } else {
                                showNotification(response.message || 'Error al iniciar', 'error');
                            }
                        },
                        error: function () {
                            showNotification('Error al iniciar', 'error');
                        }
                    });
                }
            });
        }
    </script>
</body>
</html>