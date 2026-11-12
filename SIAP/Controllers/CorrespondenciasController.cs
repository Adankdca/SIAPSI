using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using Dapper;
using SIAP.Data;

namespace SIAP.Controllers 
{
    public class CorrespondenciasController : ApiController
    {
        public IEnumerable<Cors> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var id = nvc["idcors"];

            IEnumerable<Cors> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tblp_correspondencia where idcorrespondencia = " + id;
                list = con.Query<Cors>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class TurnadosController : ApiController
    {
        public IEnumerable<Turnado> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var id = nvc["idcors"];
            var tipo = nvc["tipou"];
            var idsec = nvc["idsec"];
            var idsubsec = nvc["idsubsec"];
            var iddir = nvc["iddir"];
            string sec = "";
            string subsec = "";
            string direc = "";

            if (idsec != "o") { 
                sec = " and idsecretaria=" + idsec;
            }
            if(idsubsec != "o")
            {
                subsec = " and idsubsecretaria=" + idsubsec;
            }
            if(iddir != "o")
            {
                direc = " and iddireccion=" + iddir;
            }

            IEnumerable<Turnado> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tblc_turnar where idcorrespondencia = " + id + sec + subsec + direc + " and tipoturnar = '" + tipo + "'";
                list = con.Query<Turnado>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class SeguimientosController : ApiController
    {
        public IEnumerable<Seguimiento> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var id = nvc["idcors"];

            IEnumerable<Seguimiento> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tblp_correspondencia where idcorrespondencia = " + id;
                list = con.Query<Seguimiento>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class SecretariasController : ApiController
    {
        public IEnumerable<Correspondencia> GetSecretarias()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var id = nvc["idsecretaria"];
            var anio = nvc["anio"];

            var sqlgen = "select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, *, (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                                   + ", (select nombretitular from tblc_subsecretaria where idsubsecretaria = ad.idsubsecretaria  ) as Turnado"
                                   + ", case when  ad.idsubsecretaria = 8 then  'Sin Turnar' "
                                   + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                                   + "when  ad. idestatus <> 6 then  'Tramite' "
                                   + "when  ad. idestatus =6 then  'Concluido' "
                                   + " else 'Otro estatus' end as EstatusActual "
                                   + " , 'false' as Atencion, 'false' as Apoyo, 'false' as Conocimiento , 'false' as Informar, 'false' as Archivo, 'X' as TurnadoT"
                                   + ", prioridadsec as prioridadGen"
                                   + " from tblp_correspondencia ad"
                                   + " where ad.idcorrespondencia > 0 ";
            var sqlvalida = " AND  ad.idsecretaria = " + id + " and ad.Tipo ='SE'";
            IEnumerable<Correspondencia> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = sqlgen + sqlvalida + " and YEAR(ad.fechaoficio)='" + anio + "' order by ad.idcorrespondencia desc";
                list = con.Query<Correspondencia>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class CoordinacionesController : ApiController
    {
        public IEnumerable<Correspondencia> GetCoordinaciones()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idsub = nvc["idsubsecretaria"];
            var anio = nvc["anio"];

            var sqlgen = "select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, *, (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                                 + ", (select director from tblc_direcciones where iddireccion = ad.iddireccion  )  as Turnado"
                                 + ", case when  ad.idsubsecretaria = 8 then  'Sin Turnar' "
                                 + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                                 + "when  ad. idestatus <> 6 then  'Tramite' "
                                 + "when  ad. idestatus =6 then  'Concluido' "
                                 + " else 'Otro estatus' end as EstatusActual "
                                 + ", 'false' as Atencion, 'false' as Apoyo, 'false' as Conocimiento , 'false' as Informar, 'false' as Archivo, 'X' as TurnadoT"
                                  + ", prioridadcoor as prioridadGen"
                                 + " from tblp_correspondencia ad"
                                 + " where ad.idcorrespondencia > 0  and  ad.Tipo = 'CG' ";
            var sqlvalida = " AND  ad.Idcoordinacion = " + idsub + " and YEAR(ad.fechaoficio)='" + anio + "'";
            var sqlvalida2 = "UNION select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, *, (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                   + ", (select director from tblc_direcciones where iddireccion = ad.iddireccion  ) as Turnado"
                   + ", case when  ad.iddireccion = 34 then  'Sin Turnar' "
                   + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                   + "when  ad. idestatus <> 4 then  'Tramite' "
                   + "when  ad. idestatus =4 then  'Concluido' "
                   + " else 'Otro estatus' end as EstatusActual "
                   + ", (SELECT Atencion FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'CS' or tipoturnar= 'CD') ) as  Atencion "
                   + ", (SELECT Apoyo FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'CS' or tipoturnar= 'CD') ) as  Apoyo "
                   + ", (SELECT Conocimiento FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'CS' or tipoturnar= 'CD') ) as  Conocimiento "
                   + ", (SELECT Informar FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'CS' or tipoturnar= 'CD') ) as  Informar "
                   + " , (SELECT Archivo FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "   and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'CS' or tipoturnar= 'CD') ) as  Archivo "
                   + " , 'T' as TurnadoT"
                   + ", prioridadcoor as prioridadGen"
                   + " from tblp_correspondencia ad"
                   + " where ad.idcorrespondencia in(SELECT idcorrespondencia  FROM tblc_turnar  where [idsubsecretaria] = " + idsub + " and (tipoturnar= 'CS' or tipoturnar= 'CD') )";
            IEnumerable<Correspondencia> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = sqlgen + sqlvalida + sqlvalida2 + " and YEAR(ad.fechaoficio)='" + anio + "' order by ad.idcorrespondencia desc";
                list = con.Query<Correspondencia>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class SubsecretariasController : ApiController
    {
        public IEnumerable<Correspondencia> GetSubsecretarias()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idsub = nvc["idsubsecretaria"];
            var anio = nvc["anio"];

            var sqlgen = "select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, "
                    + " idcorrespondencia ,numoficio ,fechaoficio ,origen ,nombre ,cargo ,asunto ,fechaacuerdo ,fechatermino ,anexo "
     + " ,iddependencia ,idsecretaria ,Idcoordinacion ,idsubsecretaria ,iddireccion,idarea ,idestatus ,instruccion "
     + " ,prioridad ,tiempo ,fechacierre ,fechaalta ,idusuario ,analista ,instruccionanalista ,descripcionanalista "
     + " ,archivoanalista ,descripcionconcluido ,archivoconcluido ,descripcionrespuesta ,archivorespuesta ,instrucciondir "
    + " ,prioridaddir  ,tiempodir ,fechaacuerdodir ,fechaterminodir ,fechaatendido ,Nofolio ,Tipo,dias ,CveMunicipio ,IDLocalidad "
     + " ,Agenda ,prioridadsec ,fechaterminosec ,prioridadcoor ,fechaterminocoor ,instruccionsec ,instruccioncoor ,fechaterminosub "
     + ", case when Tipo = 'SE' then archivocrea when Tipo = 'SB' then archivocreadir  else archivocrea end as archivocrea "
                       + ", (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                      + ", (select director from tblc_direcciones where iddireccion = ad.iddireccion  ) as Turnado"
                      + ", case " + " when ad. idestatus = 6 then  'Concluido' " + " when  ad.iddireccion = 34 then  'Sin Turnar' "
                      + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                      + "when  ad. idestatus <> 6 then  'Tramite' "
                     
                      + " else 'Otro estatus' end as EstatusActual "
                      + ", 'false' as Atencion, 'false' as Apoyo, 'false' as Conocimiento , 'false' as Informar, 'false' as Archivo, 'X' as TurnadoT"
                       + ", case when Tipo = 'SE' then prioridadsec else prioridad end as prioridadGen"
                      + " from tblp_correspondencia ad"
                      + " where ad.idcorrespondencia > 0 and  (ad.Tipo = 'SB' or ad.Tipo = 'SE' )";
            var sqlvalida = " AND  ad.idsubsecretaria = " + idsub + " and YEAR(ad.fechaoficio) = '" + anio + "'";
            var sqlvalida2 = " UNION select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, "
                         + " idcorrespondencia ,numoficio ,fechaoficio ,origen ,nombre ,cargo ,asunto ,fechaacuerdo ,fechatermino ,anexo "
     + " ,iddependencia ,idsecretaria ,Idcoordinacion ,idsubsecretaria ,iddireccion,idarea ,idestatus ,instruccion "
     + " ,prioridad ,tiempo ,fechacierre ,fechaalta ,idusuario ,analista ,instruccionanalista ,descripcionanalista "
     + " ,archivoanalista ,descripcionconcluido ,archivoconcluido ,descripcionrespuesta ,archivorespuesta ,instrucciondir "
    + " ,prioridaddir  ,tiempodir ,fechaacuerdodir ,fechaterminodir ,fechaatendido ,Nofolio ,Tipo,dias ,CveMunicipio ,IDLocalidad "
     + " ,Agenda ,prioridadsec ,fechaterminosec ,prioridadcoor ,fechaterminocoor ,instruccionsec ,instruccioncoor ,fechaterminosub "
     + ", case when Tipo = 'SE' then archivocrea when Tipo = 'SB' then archivocreadir  else archivocrea end as archivocrea"
                        + ", (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                       + ", (select director from tblc_direcciones where iddireccion = ad.iddireccion  ) as Turnado"
                       + ", case " + "when  ad. idestatus =6 then  'Concluido' " + " when  ad.iddireccion = 34 then  'Sin Turnar' "
                        + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                         + "when  ad. idestatus <> 6 then  'Tramite' "
                       + "when  ad. idestatus =6 then  'Concluido' "
                       + " else 'Otro estatus' end as EstatusActual "
                        + ", (SELECT Atencion FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'SB') ) as  Atencion "
                      + ", (SELECT Apoyo FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'SB')) as  Apoyo "
                      + ", (SELECT Conocimiento FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'SB')) as  Conocimiento "
                      + ", (SELECT Informar FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'SB')) as  Informar "
                      + " , (SELECT Archivo FROM tblc_turnar  where [idsubsecretaria] = " + idsub + "   and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'SB')) as  Archivo "
                      + " , 'T' as TurnadoT"
                        + ", case when Tipo = 'SE' then prioridadsec else prioridad end as prioridadGen"
                       + " from tblp_correspondencia ad"
                       + " where ad.idcorrespondencia in(SELECT idcorrespondencia  FROM tblc_turnar  where [idsubsecretaria] = " + idsub + " and (tipoturnar= 'SB'))";
            IEnumerable<Correspondencia> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = sqlgen + sqlvalida + sqlvalida2 + " and YEAR(ad.fechaoficio) ='" + anio  + "' order by ad.idcorrespondencia desc";
                list = con.Query<Correspondencia>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class DirectoresController : ApiController
    {
        public IEnumerable<Correspondencia> GetDirectores()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var iddir = nvc["iddireccion"];
            var anio = nvc["anio"];

            var sqlgen = "select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, "
                          + " idcorrespondencia ,numoficio ,fechaoficio ,origen ,nombre ,cargo ,asunto ,fechaacuerdo ,fechatermino ,anexo "
     + " ,iddependencia ,idsecretaria ,Idcoordinacion ,idsubsecretaria ,iddireccion,idarea ,idestatus ,instruccion "
     + " ,prioridad ,tiempo ,fechacierre ,fechaalta ,idusuario ,analista ,instruccionanalista ,descripcionanalista "
     + " ,archivoanalista ,descripcionconcluido ,archivoconcluido ,descripcionrespuesta ,archivorespuesta ,instrucciondir "
    + " ,prioridaddir  ,tiempodir ,fechaacuerdodir ,fechaterminodir ,fechaatendido ,Nofolio ,Tipo,dias ,CveMunicipio ,IDLocalidad "
     + " ,Agenda ,prioridadsec ,fechaterminosec ,prioridadcoor ,fechaterminocoor ,instruccionsec ,instruccioncoor ,fechaterminosub "
     + ", case when Tipo = 'SE' then archivocrea when Tipo = 'SB' then archivocreadir  when Tipo = 'DR' then archivocreajd   else archivocrea end as archivocrea "

                     + ", (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                     + ", (select nombrejefearea from tblc_area where idarea = ad.idarea  ) as Turnado"
                     + ", case when  ad.idarea = 2 then  'Sin Turnar' "
                     + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                     + "when  ad. idestatus <> 6 then  'Tramite' "
                     + "when  ad. idestatus =6 then  'Concluido' "
                     + " else 'Otro estatus' end as EstatusActual "
                     + ", 'false' as Atencion, 'false' as Apoyo, 'false' as Conocimiento , 'false' as Informar, 'false' as Archivo, 'X' as TurnadoT"
                     + ", prioridad as prioridadGen"
                     + " from tblp_correspondencia ad"
                     + " where ad.idcorrespondencia > 0  and  (ad.Tipo = 'DR' or ad.Tipo = 'SB'  )  ";
            var sqlvalida = " AND  ad.iddireccion = " + iddir + " and YEAR(ad.fechaoficio) ='" + anio + "'";
            var sqlvalida2 = " UNION select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, "
                          + " idcorrespondencia ,numoficio ,fechaoficio ,origen ,nombre ,cargo ,asunto ,fechaacuerdo ,fechatermino ,anexo "
     + " ,iddependencia ,idsecretaria ,Idcoordinacion ,idsubsecretaria ,iddireccion,idarea ,idestatus ,instruccion "
     + " ,prioridad ,tiempo ,fechacierre ,fechaalta ,idusuario ,analista ,instruccionanalista ,descripcionanalista "
     + " ,archivoanalista ,descripcionconcluido ,archivoconcluido ,descripcionrespuesta ,archivorespuesta ,instrucciondir "
    + " ,prioridaddir  ,tiempodir ,fechaacuerdodir ,fechaterminodir ,fechaatendido ,Nofolio ,Tipo,dias ,CveMunicipio ,IDLocalidad "
     + " ,Agenda ,prioridadsec ,fechaterminosec ,prioridadcoor ,fechaterminocoor ,instruccionsec ,instruccioncoor ,fechaterminosub "
     + ", case when Tipo = 'SE' then archivocrea when Tipo = 'SB' then archivocreadir  when Tipo = 'DR' then archivocreajd   else archivocrea end as archivocrea "

                        + ", (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                      + ", (select nombrejefearea from tblc_area where idarea = ad.idarea  ) as Turnado"
                      + ", case when  ad.idarea = 2 then  'Sin Turnar' "
                      + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                      + "when  ad. idestatus <> 6 then  'Tramite' "
                      + "when  ad. idestatus =6 then  'Concluido' "
                      + " else 'Otro estatus' end as EstatusActual "
                      + ", (SELECT Atencion FROM tblc_turnar  where [iddireccion] = " + iddir + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'DR')) as  Atencion "
                      + ", (SELECT Apoyo FROM tblc_turnar  where [iddireccion] = " + iddir + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'DR')) as  Apoyo "
                      + ", (SELECT Conocimiento FROM tblc_turnar  where [iddireccion] = " + iddir + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'DR')) as  Conocimiento "
                      + ", (SELECT Informar FROM tblc_turnar  where [iddireccion] = " + iddir + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'DR')) as  Informar "
                      + " , (SELECT Archivo FROM tblc_turnar  where [iddireccion] = " + iddir + "   and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'DR')) as  Archivo "
                      + " , 'T' as TurnadoT"
                         + ", prioridad as prioridadGen"
                      + " from tblp_correspondencia ad"
                      + " where ad.idcorrespondencia in(SELECT idcorrespondencia  FROM tblc_turnar  where [iddireccion] = " + iddir + " and (tipoturnar= 'DR' ))";
            IEnumerable<Correspondencia> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = sqlgen + sqlvalida + sqlvalida2 + " and YEAR(ad.fechaoficio) ='" + anio + "' order by ad.idcorrespondencia desc";
                list = con.Query<Correspondencia>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class JefesController : ApiController
    {
        public IEnumerable<Correspondencia> GetJefes()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idarea = nvc["idarea"];
            var anio = nvc["anio"];

            var sqlgen = "select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, *, (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                       + ", (select nombrejefearea from tblc_area where idarea = ad.idarea  ) as Turnado"
                     + ", case when  ad.idarea = 2 then  'Sin Turnar' "
                      + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                      + "when  ad. idestatus <> 6 then  'Tramite' "
                      + "when  ad. idestatus =6 then  'Concluido' "
                      + " else 'Otro estatus' end as EstatusActual "
                      + ", 'false' as Atencion, 'false' as Apoyo, 'false' as Conocimiento , 'false' as Informar, 'false' as Archivo, 'X' as TurnadoT"
                      + ", prioridaddir as prioridadGen"
                      + " from tblp_correspondencia ad"
                      + " where ad.idcorrespondencia > 0  and  (ad.Tipo = 'JD' or ad.Tipo = 'DR'  or ad.Tipo = 'SB' )  ";
            var sqlvalida = " AND  ad.idarea = " + idarea + "and YEAR(ad.fechaoficio) ='" + anio + "'";
            var sqlvalida2 = " UNION select ROW_NUMBER() OVER(ORDER BY ad.idcorrespondencia ASC) AS row, *, (select estatus from tblc_estatus e where  e.idestatus = ad. idestatus) as estatus   "
                                         + ", (select nombrejefearea from tblc_area where idarea = ad.idarea  ) as Turnado"
                                         + ", case when  ad.idarea = 2 then  'Sin Turnar' "
                                         + "when  ad.fechacierre < GETDATE() then  'Vencido' "
                                         + "when  ad. idestatus <> 6 then  'Tramite' "
                                         + "when  ad. idestatus =6 then  'Concluido' "
                                         + " else 'Otro estatus' end as EstatusActual "
                                         + ", (SELECT Atencion FROM tblc_turnar  where [idarea] = " + idarea + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'JD')) as  Atencion "
                                         + ", (SELECT Apoyo FROM tblc_turnar  where [idarea] = " + idarea + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'JD')) as  Apoyo "
                                         + ", (SELECT Conocimiento FROM tblc_turnar  where [idarea] = " + idarea + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'JD')) as  Conocimiento "
                                         + ", (SELECT Informar FROM tblc_turnar  where [idarea] = " + idarea + "  and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'JD')) as  Informar "
                                         + " , (SELECT Archivo FROM tblc_turnar  where [idarea] = " + idarea + "   and idcorrespondencia =  ad.idcorrespondencia and (tipoturnar= 'JD')) as  Archivo "
                                         + " , 'T' as TurnadoT"
                                            + ", prioridad as prioridadGen"
                                         + " from tblp_correspondencia ad"
                                         + " where ad.idcorrespondencia in(SELECT idcorrespondencia  FROM tblc_turnar  where [idarea] = " + idarea + " and (tipoturnar= 'JD' ))";
            IEnumerable<Correspondencia> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = sqlgen + sqlvalida + sqlvalida2 + " and YEAR(ad.fechaoficio) ='" + anio + "' order by ad.idcorrespondencia desc";
                list = con.Query<Correspondencia>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class MunicipiosController : ApiController
    {
        public IEnumerable<Municipio> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idmun = nvc["idmun"];

            IEnumerable<Municipio> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tbl_municipios where CveMunicipio = " + idmun;
                list = con.Query<Municipio>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class EstadosController : ApiController
    {
        public IEnumerable<Estadoo> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idestado = nvc["idestado"];

            IEnumerable<Estadoo> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tbl_estado where idestado = " + idestado;
                list = con.Query<Estadoo>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class LocalidadesController : ApiController
    {
        public IEnumerable<Localidad> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idlocalidad = nvc["idlocalidad"];

            IEnumerable<Localidad> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tbl_localidades where IDLocalidad = " + idlocalidad;
                list = con.Query<Localidad>(sql);
            }
            return list.ToArray().ToList();
        }
    }
    public class EstatusController : ApiController
    {
        public IEnumerable<Estatu> GetCorrespondencia()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var idestatus = nvc["idestatus"];

            IEnumerable<Estatu> list = null;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDCorrespondencia"].ToString()))
            {
                var sql = "select * FROM tblc_estatus where idestatus = " + idestatus;
                list = con.Query<Estatu>(sql);
            }
            return list.ToArray().ToList();
        }
    }
}