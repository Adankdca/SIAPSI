using System;
using System.Collections.Generic;

namespace SIAP.Models
{
    // =============================================
    // CATÁLOGOS (NUEVOS Y EXISTENTES)
    // =============================================

    public class Modeltipoministracion
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    // NUEVO: Tipo Ministración Folio
    public class ModelTipoMinistracionFolio
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    // NUEVO: Sub Fuente Financiamiento
    public class ModelSubFuenteFinanciamiento
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    // NUEVO: Proyecto Estratégico
    public class ModelProyectoEstrategico
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    // NUEVO: Ramo
    public class ModelRamo
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    // NUEVO: Tipo Recurso
    public class ModelTipoRecurso
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    public class ModelDigitoMinistracion
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string DM { get; set; }
    }

    public class ModelFuenteFinanciamiento
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }

    public class ModelProgramaFondo
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }

        // IDs de las relaciones
        public int? TipoRecursoId { get; set; }
        public int? FuenteFinanciamientoId { get; set; }
        public int? RamoId { get; set; }

        // Claves
        public string TR { get; set; }
        public string FF { get; set; }
        public string Ramo { get; set; }

        // Descripciones completas
        public string TipoRecurso { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public string DescripcionRamo { get; set; }
    }

    public class Modelnummes
    {
        public int numeromes { get; set; }
        public string nombremes { get; set; }
    }

    // =============================================
    // MINISTRACIONES (Catálogo para combos)
    // =============================================

    public class Modelministracion
    {
        public int idministracion { get; set; }
        public string numministracion { get; set; }
    }

    // =============================================
    // MINISTRACIONES (Modelo completo)
    // =============================================

    public class ModelMinistracion
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public int? PresupuestoCalendarizadoId { get; set; }
        public int? ProgramaFondoId { get; set; }
        public int? DigitoMinistracionId { get; set; }
        public int? TipoMinistracionId { get; set; }
        public int? FuenteFinanciamientoId { get; set; }
        public string ClavePresupuestal { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Concepto { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // NUEVOS CAMPOS
        public int? id_tipoministracion_folio { get; set; }
        public int? id_tipo_recurso { get; set; }
        public int? Id_sub_fuente_financiamiento { get; set; }
        public int? Id_proyecto_estrategico { get; set; }
        public int? Id_ramo { get; set; }
    }

    // =============================================
    // MINISTRACIONES (Para mostrar en la tabla)
    // =============================================

    public class ModelMinistracionCompleta
    {
        // =============================================
        // PROPIEDADES MAPEADAS DESDE LA BASE DE DATOS
        // =============================================

        public int Id { get; set; }
        public string Folio { get; set; }
        public int? PresupuestoCalendarizadoId { get; set; }
        public string ClavePresupuestal { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public decimal ImporteTotal { get; set; }
        public string Concepto { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public decimal? PresupuestoCalendarizado { get; set; }

        // =============================================
        // CAMPOS DE CATÁLOGOS (mapeados desde las tablas)
        // =============================================

        // De digito_ministracion
        public string DigitoClave { get; set; }
        public string DigitoDescripcion { get; set; }

        // De tipo_ministracion
        public string TipoClave { get; set; }
        public string TipoDescripcion { get; set; }

        // De fuente_financiamiento
        public string FuenteClave { get; set; }
        public string FuenteDescripcion { get; set; }

        // De programa_fondo
        public string ProgramaFondoClave { get; set; }
        public string ProgramaFondoDescripcion { get; set; }
        public string ProgramaFondoTR { get; set; }
        public string ProgramaFondoFF { get; set; }
        public string ProgramaFondoRamo { get; set; }

        // NUEVOS CAMPOS DE CATÁLOGOS
        public string TipoMinistracionFolioClave { get; set; }
        public string TipoMinistracionFolioDescripcion { get; set; }
        public string TipoRecursoClave { get; set; }
        public string TipoRecursoDescripcion { get; set; }
        public string SubFuenteClave { get; set; }
        public string SubFuenteDescripcion { get; set; }
        public string ProyectoEstrategicoClave { get; set; }
        public string ProyectoEstrategicoDescripcion { get; set; }
        public string RamoClave { get; set; }
        public string RamoDescripcion { get; set; }

        // Campos antiguos (mantener por compatibilidad)
        public string TipoRecurso { get; set; }
        public string Ramo { get; set; }

        // =============================================
        // IDs DE CATÁLOGOS (EXISTENTES Y NUEVOS)
        // =============================================

        // IDs existentes
        public int? DigitoMinistracionId { get; set; }
        public int? TipoMinistracionId { get; set; }
        public int? FuenteFinanciamientoId { get; set; }
        public int? ProgramaFondoId { get; set; }

        // NUEVOS IDs
        public int? id_tipoministracion_folio { get; set; }
        public int? id_tipo_recurso { get; set; }
        public int? Id_sub_fuente_financiamiento { get; set; }
        public int? Id_proyecto_estrategico { get; set; }
        public int? Id_ramo { get; set; }

        // =============================================
        // PROPIEDADES CALCULADAS PARA EL FRONTEND
        // =============================================

        // Identificador
        public int idministracion => Id;

        // Datos básicos
        public string numministracion { get; set; }
        public string tipoministracion { get; set; }
        public decimal? importe { get; set; }
        public string enlace { get; set; }
        public int? numeromes { get; set; }
        public string fecharegistro { get; set; }
        public string estatus { get; set; }
        public string nombremes { get; set; }
        public int anio { get; set; }
        public string concepto { get; set; }
        public bool tienePDF { get; set; }

        // Fuente Financiamiento
        public string fuenteFinanciamiento { get; set; }
        public int? fuenteFinanciamientoId { get; set; }

        // Programa Fondo
        public string programaFondoDescripcion { get; set; }
        public string programaFondoClave { get; set; }
        public string programaFondoTR { get; set; }
        public string programaFondoFF { get; set; }
        public string programaFondoRamo { get; set; }

        // Dígito
        public string digitoClave { get; set; }
        public string digitoDescripcion { get; set; }
        public string digitoMinistracion { get; set; }
        public string digitoDM { get; set; }

        // Tipo
        public string tipoClave { get; set; }

        // Otros
        public decimal? montoSolicitado => 0;

        // Para compatibilidad con propiedades existentes
        public string DigitoMinistracion { get; set; }
        public string DigitoDM { get; set; }

        // NUEVAS PROPIEDADES PARA ACCESO RÁPIDO DESDE FRONTEND
        public string tipoMinistracionFolio => TipoMinistracionFolioDescripcion;
        public string tipoRecurso => TipoRecursoDescripcion;
        public string subFuenteFinanciamiento => SubFuenteDescripcion;
        public string proyectoEstrategico => ProyectoEstrategicoDescripcion;
        public string ramo => RamoDescripcion;
    }

    // =============================================
    // MINISTRACIONES (Detalle completo)
    // =============================================

    public class ModelMinistracionDetalleCompleto : ModelMinistracionCompleta
    {
        public string FuenteFinanciamiento { get; set; }
        public string TipoRecursoDescripcion { get; set; }
        public decimal? SumatoriaClaves { get; set; }
        public string FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public decimal? PresupuestoAsociado { get; set; }
        public string DescripcionPresupuesto { get; set; }
        public List<ModelClaveRelacionada> ClavesRelacionadas { get; set; }
        public List<ModelClaveRelacionada> DesgloseClaves { get; set; }
        public List<ModelCancelacionMinistracion> Cancelaciones { get; set; }
        public List<ModelMinistracionConjunta> MinistracionesConjuntas { get; set; }
        public List<ModelMovimientoAdecuacion> Movimientos { get; set; }

        public string fechaModificacion => FechaModificacion;
        public string usuarioModificacion => UsuarioModificacion;
        public string tipoRecursoDescripcion => TipoRecursoDescripcion;
        public decimal? sumatoriaClaves => SumatoriaClaves;
        public string descripcionPresupuesto => DescripcionPresupuesto;
        public List<ModelClaveRelacionada> clavesRelacionadas => ClavesRelacionadas;
        public List<ModelClaveRelacionada> desgloseClaves => DesgloseClaves;
        public List<ModelCancelacionMinistracion> cancelaciones => Cancelaciones;
        public List<ModelMinistracionConjunta> ministracionesConjuntas => MinistracionesConjuntas;
        public List<ModelMovimientoAdecuacion> movimientos => Movimientos;
    }

    // =============================================
    // CLAVE RELACIONADA
    // =============================================

    public class ModelClaveRelacionada
    {
        public string ClavePresupuestal { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Nombremes { get; set; }
        public int Mes { get; set; }

        public string clavePresupuestal => ClavePresupuestal;
        public string descripcion => Descripcion;
        public decimal monto => Monto;
        public string nombremes => Nombremes;
    }

    // =============================================
    // MINISTRACIÓN CONJUNTA
    // =============================================

    public class ModelMinistracionConjunta
    {
        public int Id { get; set; }
        public string Numministracion { get; set; }
        public string Concepto { get; set; }
        public decimal Importe { get; set; }
        public string Estatus { get; set; }
    }

    // =============================================
    // CANCELACIONES
    // =============================================

    public class ModelCancelacionMinistracion
    {
        public int Id { get; set; }
        public string FolioCancelacion { get; set; }
        public decimal MontoCancelado { get; set; }
        public string MotivoCancelacion { get; set; }
        public string FechaCancelacion { get; set; }
        public string UsuarioCancela { get; set; }
    }

    // =============================================
    // MODELO PARA CANCELAR
    // =============================================

    public class ModelCancelarMinistracion
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        public decimal Monto { get; set; }
    }

    // =============================================
    // MOVIMIENTOS DE ADECUACIÓN
    // =============================================

    public class ModelMovimientoAdecuacion
    {
        public string ClavePresupuestal { get; set; }
        public string Asunto { get; set; }
        public decimal? Monto { get; set; }
        public string TipoOperacion { get; set; }
        public string Beneficiario { get; set; }
        public string Factura { get; set; }
        public string Fecha { get; set; }

        public string clavePresupuestal => ClavePresupuestal;
        public string asunto => Asunto;
        public decimal? monto => Monto;
        public string tipoOperacion => TipoOperacion;
    }

    // =============================================
    // GUARDAR / EDITAR (ACTUALIZADO)
    // =============================================

    public class ModelMinistracionGuardar
    {
        public string accion { get; set; }
        public int id { get; set; }
        public string folio { get; set; }
        public int? presupuestoCalendarizadoId { get; set; }
        public int? programaFondoId { get; set; }
        public int? digitoMinistracionId { get; set; }
        public int? tipoMinistracionId { get; set; }
        public int? fuenteFinanciamientoId { get; set; }
        public string clavePresupuestal { get; set; }
        public int mesAplicacion { get; set; }
        public int anio { get; set; }
        public string importeTotal { get; set; }
        public string concepto { get; set; }
        public string estatus { get; set; }

        // NUEVOS CAMPOS PARA GUARDAR
        public int? id_tipoministracion_folio { get; set; }
        public int? id_tipo_recurso { get; set; }
        public int? Id_sub_fuente_financiamiento { get; set; }
        public int? Id_proyecto_estrategico { get; set; }
        public int? Id_ramo { get; set; }
    }

    // =============================================
    // RECURSOS POR MES Y FONDO
    // =============================================

    public class ModelRecursosPorMesFondo
    {
        public int Mes { get; set; }
        public string Nombremes { get; set; }
        public int ProgramaFondoId { get; set; }
        public string FondoDescripcion { get; set; }
        public decimal Total { get; set; }
        public List<ModelMinistracionResumen> Ministraciones { get; set; }
    }

    public class ModelMinistracionResumen
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string ClavePresupuestal { get; set; }
        public decimal Monto { get; set; }
        public string Estatus { get; set; }
    }

    // =============================================
    // CLAVES PRESUPUESTALES
    // =============================================

    public class ModelClavePresupuestal
    {
        public string ClavePresupuestal { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal? Monto { get; set; }
        public string Nombremes { get; set; }
        public int? Numeromes { get; set; }

        public string clavePresupuestal => ClavePresupuestal;
        public string descripcion => Descripcion;
        public string tipo => Tipo;
        public decimal? monto => Monto;
    }

    // =============================================
    // CLAVE PRESUPUESTAL CON MONTO POR MES
    // =============================================

    public class ModelClavePresupuestalPorMes
    {
        public string ClavePresupuestal { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public decimal? Monto { get; set; }
        public string Nombremes { get; set; }
        public int? Numeromes { get; set; }
        public int? ProgramaFondoId { get; set; }
        public string ProgramaFondoDescripcion { get; set; }
        public string FuenteFinanciamiento { get; set; }
        public int? FuenteFinanciamientoId { get; set; }

        public string clavePresupuestal => ClavePresupuestal;
        public string descripcion => Descripcion;
        public string tipo => Tipo;
        public decimal? monto => Monto;
        public int? numeromes => Numeromes;
        public string nombremes { get; set; }
        public string fuenteFinanciamiento => FuenteFinanciamiento;
        public int? fuenteFinanciamientoId => FuenteFinanciamientoId;
    }

    // =============================================
    // ESTADÍSTICAS
    // =============================================

    public class ModelEstadisticasMinistraciones
    {
        public int Total { get; set; }
        public int Registradas { get; set; }
        public int Iniciados { get; set; }
        public int Canceladas { get; set; }
        public decimal TotalAutorizado { get; set; }
        public decimal TotalSolicitado { get; set; }
        public decimal TotalEtiquetado { get; set; }
        public decimal TotalNoEtiquetado { get; set; }
        public int MesActual { get; set; }
        public int TotalMesActual { get; set; }
        public int TotalMesAnterior { get; set; }
        public decimal TotalPresupuesto { get; set; }
    }

    // =============================================
    // FILTROS PARA CONSULTAS
    // =============================================

    public class ModelMinistracionFiltros
    {
        public string Filtro { get; set; }
        public int? Mes { get; set; }
        public int? TipoMinistracionId { get; set; }
        public int? ProgramaFondoId { get; set; }
        public string Estatus { get; set; }

        // NUEVOS FILTROS
        public int? id_tipoministracion_folio { get; set; }
        public int? id_tipo_recurso { get; set; }
        public int? Id_sub_fuente_financiamiento { get; set; }
        public int? Id_proyecto_estrategico { get; set; }
        public int? Id_ramo { get; set; }
    }
}