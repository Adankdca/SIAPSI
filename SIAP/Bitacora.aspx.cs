using System;
using System.Web.UI;
using SIAP.Repositories;

namespace SIAP
{
    public partial class Bitacora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Sin sesión: al Login con aviso de sesión expirada. Sin permiso: al Menú.
            Seguridad.Requerir(this, soloAdministrador: true);

            if (!IsPostBack)
            {
                CargarBitacora();
            }
        }

        protected string FormatDetalleAdecuacion(object detalleObj)
        {
            string detalle = detalleObj?.ToString() ?? "";
            if (detalle.StartsWith("Registró adecuación oficio/solicitud: "))
            {
                string oficio = detalle.Replace("Registró adecuación oficio/solicitud: ", "").Trim();
                return $"<a href='DetalleOficio.aspx?oficio={oficio}' class='btn bg-gradient-primary text-white px-3 py-2 mb-0 shadow-sm' target='_blank' style='border-radius: 0.5rem;'><i class='fas fa-file-pdf text-white me-2' aria-hidden='true'></i>Ver Oficio</a>";
            }
            return "";
        }


        private void CargarBitacora()
        {
            try
            {
                var repo = new UsuarioRepository();
                var logs = repo.ObtenerLogs();

                // Filtrar por Módulo usando LINQ
                var sysLogs = System.Linq.Enumerable.Where(logs, l => l.Modulo == "Sistema" || l.Modulo == "Autenticación");
                var adecLogs = System.Linq.Enumerable.Where(logs, l => l.Modulo == "Adecuaciones");
                var minLogs = System.Linq.Enumerable.Where(logs, l => l.Modulo == "Ministraciones");

                gvSistema.DataSource = sysLogs;
                gvSistema.DataBind();

                gvAdecuaciones.DataSource = adecLogs;
                gvAdecuaciones.DataBind();

                gvMinistraciones.DataSource = minLogs;
                gvMinistraciones.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err", string.Format("alert('Error al cargar bitácora: {0}');", ex.Message.Replace("'", "")), true);
            }
        }
    }
}
