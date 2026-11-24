using SIAP.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace SIAP
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        /// <summary>
        /// Las llamadas al API (api/...) requieren sesión iniciada. Sin sesión se responde 401 en JSON;
        /// las páginas lo detectan (assets/js/SesionExpirada.js) y mandan al Login.
        /// </summary>
        protected void Application_PostAcquireRequestState(object sender, EventArgs e)
        {
            var ctx = HttpContext.Current;
            if (ctx == null) return;
            string ruta = ctx.Request.AppRelativeCurrentExecutionFilePath ?? "";
            if (!ruta.StartsWith("~/api/", StringComparison.OrdinalIgnoreCase)) return;
            if (ctx.Session != null && ctx.Session["UsuarioId"] != null) return;

            ctx.Response.Clear();
            ctx.Response.StatusCode = 401;
            ctx.Response.TrySkipIisCustomErrors = true;
            ctx.Response.ContentType = "application/json; charset=utf-8";
            ctx.Response.Write("{\"Message\":\"La sesión expiró. Vuelva a iniciar sesión.\",\"sesionExpirada\":true}");
            ctx.ApplicationInstance.CompleteRequest();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}