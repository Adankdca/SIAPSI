using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Timers;/*new 21102019*/

using System.ComponentModel;


/// <summary>
/// Descripción breve de ClassConexion
/// </summary>
public class SelectCommand1
{
    public DataTable Table;
    public bool Ejecutado;
    public string MsjError;
    public SelectCommand1()
    {
        Table = null;
        Ejecutado = false;
        MsjError = "";
    }
}
public class SelectCommand2
{
    public string Dato;
    public bool Ejecutado;
    public string MsjError;
    public SelectCommand2()
    {
        Ejecutado = false;
        Dato = "";
        MsjError = "";
    }
}
public class ExecCommand
{
    public int RowAffected;
    public bool Ejecutado;
    public string MsjError;
    public string IDNew;
   

    public ExecCommand()
    {
        RowAffected = 0;
        Ejecutado = false;
        MsjError = "";
    }
}
public class StrCadena
{
    public string _StrCadena;
    public StrCadena()
    {
        _StrCadena = "";
    }
}
public class Cnn
{
    public string idgeneralNew;
    public int IDNewx;
    public Int64 IDNewtwitter;
    public string idobraccion;
    string StrCad;
    SqlConnection cnn;
    SqlCommand cmd;
    SqlDataAdapter Adapter;
    /// <summary>
    /// Objeto conexion
    /// </summary>
    public Cnn()
    {
        StrCad = @"data source=SIDIA; initial catalog=BDSICOFIN; persist security info=False;user id=accesosop; password=accesosop; workstation id=ADMIN;packet size=24096;";
        //StrCad = @"data source=187.157.115.235,1433; initial catalog=BDSICOFIN; persist security info=False;user id=accesosop; password=accesosop; workstation id=ADMIN;packet size=24096;";
        //StrCad = @"data source=SOPYC\SOPYC; initial catalog=BDSICOFIN; persist security info=False;user id=accesobd; password=accesobd; workstation id=ADMIN;packet size=24096;";
        //  StrCad = @"data source=SERVIDOR; initial catalog=BDSICOFIN;Integrated Security=SSPI; persist security info=True;";
        //StrCad = @"Data Source=localhost; Initial Catalog=BDSICOFIN;Trusted_Connection=True; MultipleActiveResultSets=true;Integrated Security=True";


        cnn = new SqlConnection(StrCad); cnn = new SqlConnection(StrCad);
        cmd = new SqlCommand("", cnn);
        Adapter = new SqlDataAdapter("", cnn);
    }
    public string conec()
    {
        //  string CadenaConexion = @"data source=SERVIDOR; initial catalog=BDSICOFIN;Integrated Security=SSPI; persist security info=True;";
        //string CadenaConexion = @"data source=SOPYC\SOPYC; initial catalog=BDSICOFIN; persist security info=False;user id=accesobd; password=accesobd; workstation id=ADMIN;packet size=24096;";
        string CadenaConexion = @"data source=SIDIA; initial catalog=BDSICOFIN; persist security info=False;user id=accesosop; password=accesosop; workstation id=ADMIN;packet size=24096;";
        //string CadenaConexion = @"data source=187.157.115.235,1433; initial catalog=BDSICOFIN; persist security info=False;user id=accesosop; password=accesosop; workstation id=ADMIN;packet size=24096;";
        return CadenaConexion; 
    }


    /// <summary>
    /// Ejecuta una consulta y devuelve el resultado como una clase
    /// </summary>
    /// <param name="SQL">SQL a ajecutar</param>
    /// <returns>Tbl: DataTable, MsjError: mensaje de error</returns>
    public SelectCommand1 SelectCmd1(string SQL)
    {
        SelectCommand1 rs = new SelectCommand1();
        DataSet ds = new DataSet();
        try
        {
            cnn.Open();
            Adapter.SelectCommand.CommandText = SQL;
            Adapter.Fill(ds);
            rs.Table = ds.Tables[0];
            rs.Ejecutado = true;
            return rs;
        }
        catch (Exception e)
        {
            rs.Table = null;
            rs.Ejecutado = false;
            rs.MsjError = e.Message;
            return rs;
        }
        finally
        {
            cnn.Close();
        }
    }
    public StrCadena _StrCad_()
    {
        StrCadena _StrCad = new StrCadena();//cadena de conexión      
        _StrCad._StrCadena = StrCad.ToString();
        return _StrCad;
    }
    /// <summary>
    /// Ejecuta una consulta y devuelve el resultado como una clase
    /// </summary>
    /// <param name="SQL">SQL a ajecutar</param>
    /// <returns>string: resultado</returns>
    public SelectCommand2 SelectCmd2(string SQL)
    {
        SelectCommand2 rs = new SelectCommand2();
        DataSet ds = new DataSet();
        try
        {
            cnn.Open();
            Adapter.SelectCommand.CommandText = SQL;
            Adapter.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                rs.Dato = ds.Tables[0].Rows[0][0].ToString();
            }
            rs.Ejecutado = true;
            return rs;
        }
        catch (Exception e)
        {
            rs.Ejecutado = false;
            rs.Dato = "";
            rs.MsjError = e.Message;
            return rs;
        }
        finally
        {
            cnn.Close();
        }
    }
    /// <summary>
    /// Ejecuta la insercion, update o eliminacion de registros de una tabla.
    /// </summary>
    /// <param name="SQL">Sql a ejecutar</param>
    /// <returns>i: número de registros afectados, msgError: mensaje de error</returns>
    public ExecCommand ExecuteCmd(string SQL)
    {
        ExecCommand ins = new ExecCommand();
        try
        {
            cnn.Open();
            cmd.CommandText = SQL;
            cmd.CommandType = CommandType.Text;
            ins.RowAffected = cmd.ExecuteNonQuery();
            ins.Ejecutado = true;
            return ins;
        }
        catch (Exception e)
        {
            ins.RowAffected = -1;
            ins.Ejecutado = false;
            ins.MsjError = e.Message;
            return ins;
        }
        finally
        {
            cnn.Close();
        }
    }

    public ExecCommand ExecuteCmdScalar(string SQL)
    {
        ExecCommand ins = new ExecCommand();
        try
        {
            cnn.Open();
            cmd.CommandText = SQL;
            cmd.CommandType = CommandType.Text;
            ins.IDNew = Convert.ToString(cmd.ExecuteScalar());
            ins.Ejecutado = true;
            return ins;
        }
        catch (Exception e)
        {
            ins.IDNew = "0";
            ins.Ejecutado = false;
            ins.MsjError = e.Message;
            return ins;
        }
        finally
        {
            cnn.Close();
        }
    }
    /// <summary>
    /// Clase para llenar un ComboBox
    /// </summary>
    /// <param name="cmb">ComboBox que se va a rellenar</param>
    /// <param name="SQL">Sql a ejecutar</param>
    public void LlenaCombo(DropDownList cmb, string SQL)
    {
        DataSet ds = new DataSet();
        try
        {
            cnn.Open();
            Adapter.SelectCommand.CommandText = SQL;
            Adapter.Fill(ds);
            cmb.DataValueField = ds.Tables[0].Columns[0].ColumnName;
            cmb.DataTextField = ds.Tables[0].Columns[1].ColumnName;
            cmb.DataSource = ds.Tables[0];
            cmb.DataBind();
        }
        catch (Exception e)
        {

            //MessageBox.Show(e.Message, "Control Paciente", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        finally
        {
            cnn.Close();
        }
    }
    
    public SelectCommand1 sp_documentos(String filtro)
    {
        SelectCommand1 rs = new SelectCommand1();
        DataSet ds = new DataSet();
        try
        {
            cnn.Open();
            //Crear un objeto DataAdapter y proporcionar el nombre del procedimiento almacenado. 
            Adapter = new SqlDataAdapter("sp_deudores", cnn);
            //Establecer el tipo de comando como StoredProcedure. 
            Adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

            //Crear y agregar un parámetro a la colección Parameters del procedimiento almacenado. 
            Adapter.SelectCommand.Parameters.Add(new SqlParameter("@filtro", SqlDbType.NVarChar, 250));

            //Asignar el valor  a los parámetros. 
            Adapter.SelectCommand.Parameters["@filtro"].Value = filtro.ToString();

            Adapter.Fill(ds);
            rs.Table = ds.Tables[0];
            rs.Ejecutado = true;
            return rs;
        }
        catch (Exception e)
        {
            rs.Table = null;
            rs.Ejecutado = false;
            rs.MsjError = e.Message;
            return rs;
        }
        finally
        {
            cnn.Close();
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------SICOFIN-----------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------
    //#endregion
    #region permisos_ADG
    //Permisos
    public Boolean funInsertPermisos(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Insert_RolesArbol", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatePermisos(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Update_RolesArbol", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion

    #region permisosPanel_KGS
    //Permisos
    public Boolean funInsertPermisosPanel(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Insert_PanelRolesArbol", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatePermisosPanel(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Update_PanelRolesArbol", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion


    #region permisosEjecutivo_KGS
    //Permisos
    public Boolean funInsertPermisosejecutivo(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Insert_RolesArbolejecutivo", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatePermisosejecutivo(string pIDusuario, string pIDArbol, string pPermiso)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDusuario", "8", pIDusuario }, { "@IDArbol", "8", pIDArbol }, { "@Permiso", "2", pPermiso } };
            funUpdateDelete(tx, "Update_RolesArbolejecutivo", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion

    #region Procedimientos Almacenados Get Insert Update Delete


    public virtual void funInsert(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)



        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(0);
                int lower = pArreglo.GetLowerBound(0);


                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = lower; ctr <= upper; ctr++)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }


                //ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
                IDNewx = (int)SqlHelper.ExecuteScalar(tx, CommandType.StoredProcedure, pNombreStored, sqlParams);
            }


        }
        catch (SqlException A)
        {
            throw new System.InvalidOperationException("Error de datos");
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");

        }

    }
    public virtual void funUpdateDelete(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)

        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {



                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }



                SqlHelper.ExecuteNonQuery(tx, CommandType.StoredProcedure, pNombreStored, sqlParams);
            }



        }
        catch (Exception e)
        {
          
            throw new System.InvalidOperationException("Error de datos");
        }


    }
    public virtual DataSet funGetbyStoresPropiosgrid(string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
        cnn.Open();

        DataSet ds = new DataSet("Set");
        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(1);
                int lower = pArreglo.GetLowerBound(0);



                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }

                //'Cuando se pasan parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
            }
            else
            {
                //'Para storeds que no necesitan pase de parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
            }

            if (ds.Tables.Count > 1)
            {
                if (ds.Tables.Count == 2)
                {
                    ds.Tables[0].TableName = "ParentTabled";
                    ds.Tables[1].TableName = "ChildTabled";
                    ////// Set the table's primary key field
                    ds.Tables["ParentTabled"].PrimaryKey = new DataColumn[] {
                    ds.Tables["ParentTabled"].Columns[0] };
                   ds.AcceptChanges();
                   ds.Relations.Add(new DataRelation("relacion", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0]));
                    
                }

                if (ds.Tables.Count == 3)
                {
                    ds.Tables[0].TableName = "ParentTabled";
                    ds.Tables[1].TableName = "ChildTabled";
                    ds.Tables[2].TableName = "ChildTabled3";
                    //// Set the table's primary key field
                    ds.Tables["ParentTabled"].PrimaryKey = new DataColumn[] { ds.Tables["ParentTabled"].Columns[0] };
                    ds.AcceptChanges();
                    ds.Relations.Add(new DataRelation("relacion", ds.Tables[0].Columns[0], ds.Tables[1].Columns[0]));


                    ds.Tables["ChildTabled"].PrimaryKey = new DataColumn[] { ds.Tables["ChildTabled"].Columns[1] };
                    ds.AcceptChanges();
                    ds.Relations.Add(new DataRelation("relacion2", ds.Tables[1].Columns[1], ds.Tables[2].Columns[1]));

                }
            }

        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos:" + e.Message);

        }
        finally
        {
            cnn.Close();
            //cnn.Dispose();
        }
        return ds;
    }
    public virtual DataSet funGetbyStoresPropios(string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
        cnn.Open();

        DataSet ds = new DataSet();
        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(1);
                int lower = pArreglo.GetLowerBound(0);

                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }

                //'Cuando se pasan parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
            }
            else
            {
                //'Para storeds que no necesitan pase de parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
            }

            ds.AcceptChanges();

        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos:" + e.Message);

        }
        finally
        {
            cnn.Close();
            //cnn.Dispose();
        }
        return ds;
    }

    public virtual DataSet funGetbyStoresPropiosTMs(string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
      

        DataSet ds = new DataSet();
        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(1);
                int lower = pArreglo.GetLowerBound(0);

                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }

                //'Cuando se pasan parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
            }
            else
            {
                //'Para storeds que no necesitan pase de parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
            }

            ds.AcceptChanges();

        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos:" + e.Message);

        }
        finally
        {
            //cnn.Close();
            //cnn.Dispose();
        }
        return ds;
    }
    public virtual DataSet funGetbyStoresPropiossc(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
        

        DataSet ds = new DataSet();
        try
        {

            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(1);
                int lower = pArreglo.GetLowerBound(0);

                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }

                //'Cuando se pasan parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored, sqlParams));
            }
            else
            {
                //'Para storeds que no necesitan pase de parametros
                ds.Merge(SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure, pNombreStored));
            }

            ds.AcceptChanges();

        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos:" + e.Message);

        }
        finally
        {
           // cnn.Close();
            //cnn.Dispose();
        }
        return ds;
    }
    public virtual SqlDbType TipoDato(string pNumTipo)
    {
        switch (pNumTipo)
        {
            case "0":
                return SqlDbType.BigInt;
                break;

            case "1":
                return SqlDbType.Binary;
                break;
            case "2":
                return SqlDbType.Bit;
                break;
            case "3":
                return SqlDbType.Char;
                break;
            case "4":
                return SqlDbType.DateTime;
                break;
            case "5":
                return SqlDbType.Decimal;
                break;
            case "6":
                return SqlDbType.Float;
                break;
            case "7":
                return SqlDbType.Image;
                break;
            case "8":
                return SqlDbType.Int;
                break;
            case "9":
                return SqlDbType.Money;
                break;
            case "10":
                return SqlDbType.NChar;
                break;
            case "11":
                return SqlDbType.NText;
                break;
            case "12":
                return SqlDbType.NVarChar;
                break;
            case "13":
                return SqlDbType.Real;
                break;
            case "14":
                return SqlDbType.SmallDateTime;
                break;
            case "15":
                return SqlDbType.SmallInt;
                break;
            case "16":
                return SqlDbType.SmallMoney;
                break;
            case "17":
                return SqlDbType.Text;
                break;
            case "18":
                return SqlDbType.Timestamp;
                break;
            case "19":
                return SqlDbType.TinyInt;
                break;
            case "20":
                return SqlDbType.UniqueIdentifier;
                break;
            case "21":
                return SqlDbType.VarBinary;
                break;
            case "22":
                return SqlDbType.VarChar;
                break;
            case "23":
                return SqlDbType.Variant;
                break;

            default:
                return System.Data.SqlDbType.NVarChar;
                break;
        }

    }
    #endregion
    #region Peticiones

    #endregion
    #region Obras_ADG
    public Boolean fundeleteidobra(string idobra)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idobra", "8", idobra } };
            funUpdateDelete(tx, "Delete_obra", array);
            funUpdateDelete(tx, "Delete_antecedenteobra", array);


           

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {

            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    #endregion
    #region Catalogo Tipo de Solicitud_KGS
    public virtual void funeliminarSolicitud(string pidsolicitud)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idsolicitud", "8", pidsolicitud.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_tiposolicitud", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertSolicitud(string pidsolicitud, string pnombresolicitud)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idsolicitud", "8", pidsolicitud }, { "@nombresolicitud", "12", pnombresolicitud } };
            funUpdateDelete(tx, "Insert_tblcp_tiposolicitud", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateSolicitud(string pidsolicitud, string pnombresolicitud)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idsolicitud", "8", pidsolicitud }, { "@nombresolicitud", "12", pnombresolicitud } };
            funUpdateDelete(tx, "Update_tblcp_tiposolicitud", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo Peticionario_ADG 
    public virtual void funeliminarPeticionario(string pidpeticionario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idpeticionario", "8", pidpeticionario.ToString() } };
            funUpdateDelete(tx, "Delete_tblc_peticionario", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertPeticionario(string pidpeticionario, string pnombrepeticionario, string pcargopeticionario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionario", "8", pidpeticionario }, { "@nombrepeticionario", "12", pnombrepeticionario }, { "@cargopeticionario", "12", pcargopeticionario } };
            funUpdateDelete(tx, "Insert_tblcp_peticionario", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertorganizacion(string pidorganizacion, string organizacion, string idpromovente)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idtipoorganizacion", "8", pidorganizacion }, { "@organizacion", "12", organizacion }, { "@idpromovente", "8", idpromovente } };
            funUpdateDelete(tx, "Insert_tblcp_PET_organizacion", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatePeticionario(string pidpeticionario, string pnombrepeticionario, string pcargopeticionario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionario", "8", pidpeticionario }, { "@nombrepeticionario", "12", pnombrepeticionario }, { "@cargopeticionario", "12", pcargopeticionario } };
            funUpdateDelete(tx, "Update_tblc_peticionario", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo Peticionario Organizacion_KGS
    public virtual void funeliminarOrganizacion(string pidpeticionarioorganizacion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idpeticionarioorganizacion", "8", pidpeticionarioorganizacion.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_peticionarioorganizacion", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertOrganizacion(string pidpeticionarioorganizacion, string pnombrepeticionarioorganizacion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionarioorganizacion", "8", pidpeticionarioorganizacion }, { "@nombrepeticionarioorganizacion", "12", pnombrepeticionarioorganizacion } };
            funUpdateDelete(tx, "Insert_tblcp_peticionarioorganizacion", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateOrganizacion(string pidpeticionarioorganizacion, string pnombrepeticionarioorganizacion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionarioorganizacion", "8", pidpeticionarioorganizacion }, { "@nombrepeticionarioorganizacion", "12", pnombrepeticionarioorganizacion } };
            funUpdateDelete(tx, "Update_tblcp_peticionarioorganizacion", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo Funcionarios_KGS
    public virtual void funeliminarFuncionarios(string pidfuncionario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idfuncionario", "8", pidfuncionario.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_funcionario", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertFuncionarios(string pidfuncionario, string pnombrefuncionario, string pcargofuncionario, string pdependencia)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idfuncionario", "8", pidfuncionario }, { "@nombrefuncionario", "12", pnombrefuncionario }, { "@cargofuncionario", "12", pcargofuncionario }, { "@dependencia", "12", pdependencia } };
            funUpdateDelete(tx, "Insert_tblcp_funcionario", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }
    }

    public Boolean funupdateFuncionarios(string pidfuncionario, string pnombrefuncionario, string pcargofuncionario, string pdependencia)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idfuncionario", "8", pidfuncionario }, { "@nombrefuncionario", "12", pnombrefuncionario }, { "@cargofuncionario", "12", pcargofuncionario }, { "@dependencia", "12", pdependencia } };
            funUpdateDelete(tx, "Update_tblcp_funcionario", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo Dependencia_KGS
    public virtual void funeliminarDependencia(string piddependencia)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@iddependencia", "8", piddependencia.ToString() } };
            funUpdateDelete(tx, "Delete_tblc_dependencia", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertDependencia(string piddependencia, string pnombredependencia)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@iddependencia", "8", piddependencia }, { "@nombredependencia", "12", pnombredependencia } };
            funUpdateDelete(tx, "Insert_tblc_dependencia", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateDependencia(string piddependencia, string pnombredependencia)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@iddependencia", "8", piddependencia }, { "@nombredependencia", "12", pnombredependencia } };
            funUpdateDelete(tx, "Update_tblc_dependencia", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo area_KGS
    public virtual void funeliminarArea(string pidarearesponsable)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idarearesponsable", "8", pidarearesponsable.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_arearesponsable", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertArea(string pidarearesponsable, string pnombrearearesponsable, string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idarearesponsable", "8", pidarearesponsable }, { "@nombrearearesponsable", "12", pnombrearearesponsable }, { "@IDRubro", "8", pIDRubro } };
            funUpdateDelete(tx, "Insert_tblcp_arearesponsable", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateArea(string pidarearesponsable, string pnombrearearesponsable, string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idarearesponsable", "8", pidarearesponsable }, { "@nombrearearesponsable", "12", pnombrearearesponsable }, { "@IDRubro", "8", pIDRubro } };
            funUpdateDelete(tx, "Update_tblcp_arearesponsable", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo Rubro_KGS
    public virtual void funeliminarRubro(string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@IDRubro", "8", pIDRubro.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_rubros", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertRubro(string pIDRubro, string pRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@IDRubro", "8", pIDRubro }, { "@Rubro", "12", pRubro } };
            funUpdateDelete(tx, "Insert_tblcp_rubros", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateRubro(string pIDRubro, string pRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDRubro", "8", pIDRubro }, { "@Rubro", "12", pRubro } };
            funUpdateDelete(tx, "Update_tblcp_rubros", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Catalogo SubRubro_KGS
   public virtual void funeliminarSubrubro(string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@IDSubrubro", "8", pIDRubro.ToString() } };
            funUpdateDelete(tx, "Delete_tblcp_subrubro", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
   public Boolean funInsertSubrubro(string pIDSubrubro, string pSubrubro, string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@IDSubrubro", "8", pIDSubrubro }, { "@Subrubro", "12", pSubrubro }, { "@IDRubro", "8", pIDRubro } };
            funUpdateDelete(tx, "Insert_tblcp_subrubro", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateSubrubro(string pIDSubrubro, string pSubrubro, string pIDRubro)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDSubrubro", "8", pIDSubrubro }, { "@Subrubro", "12", pSubrubro }, { "@IDRubro", "8", pIDRubro } };
            funUpdateDelete(tx, "Update_tblcp_subrubro", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion

    #region Catalogo areasresponsable_KGS
    public virtual void funeliminararea(string pidarearesponsable)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idarearesponsable", "8", pidarearesponsable.ToString() } };
            funUpdateDelete(tx, "Delete_tblc_PET_arearesponsable", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertareas(string pidarearesponsable, string pnombrearearesponsable)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idarearesponsable", "8", pidarearesponsable }, { "@nombrearearesponsable", "12", pnombrearearesponsable }};
            funUpdateDelete(tx, "Insert_tblc_PET_arearesponsable", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }
    }

    public Boolean funupdateareas(string pidarearesponsable, string pnombrearearesponsable)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idarearesponsable", "8", pidarearesponsable }, { "@nombrearearesponsable", "12", pnombrearearesponsable } };
            funUpdateDelete(tx, "Update_ttblc_PET_arearesponsable", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion

    #region Estimaciones_KGS

    public virtual void funeliminarestimacioness(string pidestimacion, string pidobra, string pfechaemision, string pidseguimiento, string pobservacionesaccion, string pidusuario, string pAccion, string pidestatusestimacion, string pestatusestimacion, string idarearevision,  string arearevision,  string analista, string idanalista)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] arrayse = new string[,] { { "@idseguimiento", "8", pidestimacion }, { "@fechaemision", "4", pfechaemision }, { "@idusuario", "8", pidusuario }, { "@idestimacion", "8", pidestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@Observaciones", "12", pobservacionesaccion }, { "@Accion", "12", pAccion }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision } , { "@idanalista", "8", idanalista },  { "@analista", "12", analista }};
            funInsert(tx, "Insert_tble_seguimientoestimacion", arrayse);

            string[,] pArreglo = new string[,] { { "@idestimacion", "8", pidestimacion.ToString() } };
                       funUpdateDelete(tx, "Delete_tble_estimacion", pArreglo);


           

            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertestimaciones(string pidestimacion, string pidobra, string pfechaemision, string pfechaingreso, string pnoestimacion, string pidestatusestimacion, string pestatusestimacion, string pidtipoestimacion, string ptipoestimacion, string pimportesiva, string pimporteciva, string pimporteliquido, string ppersonalrecibe, string pprocedencia, string pfechainicial, string pfechafinal, string pobservaciones, string paportacionicic, string pestado, string pinspeccion, string pimpuestonomina, string pamortizacion, string ptotaldeduccion, string pretencion, string pliquidopagar, string pnumfactura, string pidseguimiento, string pobservacionesaccion, string pidusuario, string pAccion, string pFactor, string pidagremiado, string pagremiado , string pContratista , string pCorreo, string pmanodeobra, string idarearevision, string arearevision, string pfecha, string pdidgeneral, string analista, string idanalista, string aportacionsig, string adicionalnomina)
    {
        

        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idestimacion", "8", pidestimacion }, { "@Contrato", "12", pidobra }, { "@fechaemision", "4", pfechaemision }, { "@fechaingreso", "4", pfechaingreso }, { "@noestimacion", "8", pnoestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@idtipoestimacion", "8", pidtipoestimacion }, { "@tipoestimacion", "12", ptipoestimacion }, { "@importesiva", "5", pimportesiva }, { "@importeciva", "5", pimporteciva }, { "@importeliquido", "5", pimporteliquido }, { "@personalrecibe", "12", ppersonalrecibe }, { "@procedencia", "12", pprocedencia }, { "@fechainicial", "4", pfechainicial }, { "@fechafinal", "4", pfechafinal }, { "@observaciones", "12", pobservaciones }, { "@aportacionicic", "5", paportacionicic }, { "@estado", "5", pestado }, { "@inspeccion", "5", pinspeccion }, { "@impuestonomina", "5", pimpuestonomina }, { "@amortizacion", "5", pamortizacion }, { "@totaldeduccion", "5", ptotaldeduccion }, { "@retencion", "5", pretencion }, { "@liquidopagar", "5", pliquidopagar }, { "@numfactura", "12", pnumfactura }, { "@Factor", "5",pFactor }, { "@idagremiado", "8", pidagremiado }, { "@agremiado", "12", pagremiado }, { "@Contratista", "12", pContratista }, { "@Email", "12", pCorreo }, { "@manodeobra", "5", pmanodeobra }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista },{ "@idusuario", "8", pidusuario }, { "@fechamovimiento", "4", pfecha },  { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina } };
            funInsert(tx, "Insert_tble_estimacion", array);
          
            String IDEstimacionnew = IDNewx.ToString();  


            string[,] arrayse = new string[,] { { "@idseguimiento", "8", pidseguimiento }, { "@fechaemision", "4", pfechaemision }, { "@idusuario", "8", pidusuario }, { "@idestimacion", "8", pidestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@Observaciones", "12", pobservacionesaccion }, { "@Accion", "12", pAccion }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista } };
            funInsert(tx, "Insert_tble_seguimientoestimacion", arrayse);


            string[,] arrayh = new string[,] { { "@idestimacion", "8", IDEstimacionnew }, { "@Contrato", "12", pidobra }, { "@fechaemision", "4", pfechaemision }, { "@fechaingreso", "4", pfechaingreso }, { "@noestimacion", "8", pnoestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@idtipoestimacion", "8", pidtipoestimacion }, { "@tipoestimacion", "12", ptipoestimacion }, { "@importesiva", "5", pimportesiva }, { "@importeciva", "5", pimporteciva }, { "@importeliquido", "5", pimporteliquido }, { "@personalrecibe", "12", ppersonalrecibe }, { "@procedencia", "12", pprocedencia }, { "@fechainicial", "4", pfechainicial }, { "@fechafinal", "4", pfechafinal }, { "@observaciones", "12", pobservaciones }, { "@aportacionicic", "5", paportacionicic }, { "@estado", "5", pestado }, { "@inspeccion", "5", pinspeccion }, { "@impuestonomina", "5", pimpuestonomina }, { "@amortizacion", "5", pamortizacion }, { "@totaldeduccion", "5", ptotaldeduccion }, { "@retencion", "5", pretencion }, { "@liquidopagar", "5", pliquidopagar }, { "@numfactura", "12", pnumfactura }, { "@Factor", "5", pFactor }, { "@idagremiado", "8", pidagremiado }, { "@agremiado", "12", pagremiado }, { "@Contratista", "12", pContratista }, { "@Email", "12", pCorreo },{ "@fecha", "4", pfecha },{ "@idusuario", "8", pidusuario },   { "@Accion", "12", pAccion }  ,   { "@manodeobra", "5", pmanodeobra }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina } };
            funInsert(tx, "Insert_tble_estimacionhistorial", arrayh);


            //para generar el time line estimaciones
            string[,] arraytm = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", "" }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", IDEstimacionnew  }, { "@fechaingreso", "4", pfechaemision }, { "@noestimacion", "12", pnoestimacion  }, { "@idestatus", "8", pidestatusestimacion }, { "@estatus", "12", pestatusestimacion }, { "@idtipoestimacion", "8", pidtipoestimacion  }, { "@tipoestimacion", "12", ptipoestimacion }, { "@importeliquido", "5", pimporteliquido }, { "@totaldeducciones", "5", ptotaldeduccion  }, { "@liquidopagar", "5", pliquidopagar  }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", pdidgeneral}, { "@fecha_alta", "4", pfechaemision }, { "@tipo_proceso", "12", "EST" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
            funInsert(tx, "Insert_Timelineobrapublica", arraytm);
            //para generar el time line estimaciones


            tx.Commit();

            // se quito a peticiòn del jefe 07/10/2019

            //if (pidestatusestimacion == "8")  //envia telegram cuando manda a envio a pago  
            //{
            //    //======================================================================================================================================================================
            //    //======================================================================mandar dato por telegram  ======================================================================
            //    //======================================================================================================================================================================

            //    SelectCommand1 rs = new SelectCommand1();
            //    SelectCommand1 rs1 = new SelectCommand1();
            //    DataSet ds = new DataSet();
            //    DataSet dsS = new DataSet();
            //    string consultaavances = "";
            //    //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
            //    consultaavances = "select top 1 V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where G.contrato = " + pidobra + " order by idvisitaobra desc ";

            //    //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
            //    //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
            //    Adapter.SelectCommand.CommandText = consultaavances;
            //    Adapter.Fill(ds);
            //    rs.Table = ds.Tables[0];

            //    if (rs.Table.Rows.Count > 0)
            //    {


            //        for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
            //        {
            //            //============================consultamos la  última foto ==================================================   


            //                string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
            //                string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
            //                string chatId = "@sopchiapas";
            //                string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
            //                              + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"

            //                              + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
            //                              + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

            //                              + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

            //                //Estructuramos los campos que se van a mandar
            //                urlString = String.Format(urlString, apiToken, chatId, text);

            //                WebRequest request = WebRequest.Create(urlString);
            //                Stream rst = request.GetResponse().GetResponseStream();
            //                StreamReader reader = new StreamReader(rst);


            //        }
            //    }
            //    //======================================================================================================================================================================
            //    //======================================================================mandar dato por telegram  ======================================================================
            //    //======================================================================================================================================================================
            //}

            // se quito a peticiòn del jefe 07/10/2019


            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateestimaciones(string pidestimacion, string pidobra, string pfechaemision, string pfechaingreso, string pnoestimacion, string pidestatusestimacion, string pestatusestimacion, string pidtipoestimacion, string ptipoestimacion, string pimportesiva, string pimporteciva, string pimporteliquido, string ppersonalrecibe, string pprocedencia, string pfechainicial, string pfechafinal, string pobservaciones, string paportacionicic, string pestado, string pinspeccion, string pimpuestonomina, string pamortizacion, string ptotaldeduccion, string pretencion, string pliquidopagar, string pnumfactura, string pidseguimiento, string pobservacionesaccion, string pidusuario, string pAccion, string pFactor, string pidagremiado, string pagremiado, string pContratista, string pCorreo, string pmanodeobra, string idarearevision, string arearevision, string pfecha,string analista,string idanalista, string aportacionsig, string adicionalnomina)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idestimacion", "8", pidestimacion }, { "@Contrato", "12", pidobra }, { "@fechaemision", "4", pfechaemision }, { "@fechaingreso", "4", pfechaingreso }, { "@noestimacion", "8", pnoestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@idtipoestimacion", "8", pidtipoestimacion }, { "@tipoestimacion", "12", ptipoestimacion }, { "@importesiva", "5", pimportesiva }, { "@importeciva", "5", pimporteciva }, { "@importeliquido", "5", pimporteliquido }, { "@personalrecibe", "12", ppersonalrecibe }, { "@procedencia", "12", pprocedencia }, { "@fechainicial", "4", pfechainicial }, { "@fechafinal", "4", pfechafinal }, { "@observaciones", "12", pobservaciones }, { "@aportacionicic", "5", paportacionicic }, { "@estado", "5", pestado }, { "@inspeccion", "5", pinspeccion }, { "@impuestonomina", "5", pimpuestonomina }, { "@amortizacion", "5", pamortizacion }, { "@totaldeduccion", "5", ptotaldeduccion }, { "@retencion", "5", pretencion }, { "@liquidopagar", "5", pliquidopagar }, { "@numfactura", "12", pnumfactura }, { "@Factor", "5", pFactor }, { "@idagremiado", "8", pidagremiado }, { "@agremiado", "12", pagremiado }, { "@Contratista", "12", pContratista }, { "@Email", "12", pCorreo }, { "@manodeobra", "5", pmanodeobra }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista }, { "@idusuario", "8", pidusuario }, { "@fechamovimiento", "4", pfecha }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina } };
            funUpdateDelete(tx, "Update_tble_estimacion", array);

            string[,] arrayse = new string[,] { { "@idseguimiento", "8", pidseguimiento }, { "@fechaemision", "4", pfechaemision }, { "@idusuario", "8", pidusuario }, { "@idestimacion", "8", pidestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@Observaciones", "12", pobservacionesaccion }, { "@Accion", "12", pAccion }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista } };
            funInsert(tx, "Insert_tble_seguimientoestimacion", arrayse);

            string[,] arrayh = new string[,] { { "@idestimacion", "8", pidestimacion }, { "@Contrato", "12", pidobra }, { "@fechaemision", "4", pfechaemision }, { "@fechaingreso", "4", pfechaingreso }, { "@noestimacion", "8", pnoestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@idtipoestimacion", "8", pidtipoestimacion }, { "@tipoestimacion", "12", ptipoestimacion }, { "@importesiva", "5", pimportesiva }, { "@importeciva", "5", pimporteciva }, { "@importeliquido", "5", pimporteliquido }, { "@personalrecibe", "12", ppersonalrecibe }, { "@procedencia", "12", pprocedencia }, { "@fechainicial", "4", pfechainicial }, { "@fechafinal", "4", pfechafinal }, { "@observaciones", "12", pobservaciones }, { "@aportacionicic", "5", paportacionicic }, { "@estado", "5", pestado }, { "@inspeccion", "5", pinspeccion }, { "@impuestonomina", "5", pimpuestonomina }, { "@amortizacion", "5", pamortizacion }, { "@totaldeduccion", "5", ptotaldeduccion }, { "@retencion", "5", pretencion }, { "@liquidopagar", "5", pliquidopagar }, { "@numfactura", "12", pnumfactura }, { "@Factor", "5", pFactor }, { "@idagremiado", "8", pidagremiado }, { "@agremiado", "12", pagremiado }, { "@Contratista", "12", pContratista }, { "@Email", "12", pCorreo }, { "@fecha", "4", pfecha }, { "@idusuario", "12", pidusuario }, { "@Accion", "12", pAccion }, { "@manodeobra", "5", pmanodeobra }, { "@idarearevision", "8", idarearevision }, { "@arearevision", "12", arearevision }, { "@idanalista", "8", idanalista }, { "@analista", "12", analista }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina } };
            funInsert(tx, "Insert_tble_estimacionhistorial", arrayh);

            tx.Commit();

            //// se quito a peticiòn del jefe 07/10/2019
            //if (pidestatusestimacion == "8")  //envia telegram cuando manda a envio a pago  
            //{
            //    //======================================================================================================================================================================
            //    //======================================================================mandar dato por telegram  ======================================================================
            //    //======================================================================================================================================================================

            //    SelectCommand1 rs = new SelectCommand1();
            //    SelectCommand1 rs1 = new SelectCommand1();
            //    DataSet ds = new DataSet();
            //    DataSet dsS = new DataSet();
            //    string consultaavances = "";
            //    //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
            //    consultaavances = "select top 1 V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where G.contrato = '" + pidobra + "' order by idvisitaobra desc ";

            //    //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
            //    //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
            //    Adapter.SelectCommand.CommandText = consultaavances;
            //    Adapter.Fill(ds);
            //    rs.Table = ds.Tables[0];

            //    if (rs.Table.Rows.Count > 0)
            //    {


            //        for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
            //        {
            //            //============================consultamos la  última foto ==================================================   


            //            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
            //            string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
            //            string chatId = "@sopchiapas";
            //            string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
            //                          +  "<strong>Módulo : Estimaciones</strong>" + "\n"
            //                          + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
            //                          + "<strong>Estimación No. :</strong>" + pnoestimacion +  " " + ptipoestimacion + "\n"
            //                          + "<strong>Estatus:</strong>" + pestatusestimacion + "\n"
            //                          + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
            //                          + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

            //                          + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

            //            //Estructuramos los campos que se van a mandar
            //            urlString = String.Format(urlString, apiToken, chatId, text);

            //            WebRequest request = WebRequest.Create(urlString);
            //            Stream rst = request.GetResponse().GetResponseStream();
            //            StreamReader reader = new StreamReader(rst);


            //        }
            //    }
            //    //======================================================================================================================================================================
            //    //======================================================================mandar dato por telegram  ======================================================================
            //    //======================================================================================================================================================================
            //}
            //// se quito a peticiòn del jefe 07/10/2019

            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public virtual void funGuardarConvenio(GridView grvconvenio)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            Int16 i = 0;
            foreach (GridViewRow row in grvconvenio.Rows)
            {
                //checamos si no existe el registro en la base insertamos               
                if (Convert.ToInt64(grvconvenio.DataKeys[i].Values[0]) <= 0)
                {

                    funInsertconvenio(tx, "0", grvconvenio.DataKeys[i].Values[1].ToString(), grvconvenio.DataKeys[i].Values[2].ToString(), grvconvenio.DataKeys[i].Values[3].ToString());

                }

                i += 1;
            }


            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertconvenio(SqlTransaction tx, string pIDObraconvenio, string pIDObra, string pConvenio, string pImporte)
    {
        try
        {


            string[,] array = new string[,] { { "@IDObraconvenio", "8", pIDObraconvenio }, { "@IDObra", "8", pIDObra }, { "@Convenio", "12", pConvenio }, { "@Importe", "5", pImporte } };
            funInsert(tx, "Insert_tbl_ObrasConvenio", array);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }


    }
    #endregion
    #region EstimacionesDocumentos_ADG
    public Boolean funeliminarestimacionesdocumentos(SqlTransaction tx, string piddocumentoestimacion)
    {
        try
        {
            string[,] pArreglo = new string[,] { { "@iddocumentoestimacion", "8", piddocumentoestimacion.ToString() } };
            funUpdateDelete(tx, "Delete_tble_estimaciondocumentos", pArreglo);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }



    }
    public virtual void funGuardardocumentos(GridView grvdocts, string pidestimacion, string parchivozip, string pobservaciones)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idestimaciondocto = 0;
        Int64 idocto = 0;
        string docto = "";
        string archivo = "";

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtRow = (row.FindControl("txthojas") as TextBox);
                    idestimaciondocto = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);
                    idocto = Convert.ToInt64(grvdocts.DataKeys[i].Values[2]);
                    docto = Convert.ToString(grvdocts.DataKeys[i].Values[3]);

                    CheckBox chkRow = (row.FindControl("chkdocumento") as CheckBox);
                    if (chkRow.Checked == true)
                    {

                        if (txtRow.Text != "")
                        {

                            if (Convert.ToInt64(txtRow.Text) > 0)
                            {
                                funeliminarestimacionesdocumentos(tx, idestimaciondocto.ToString());



                                if (funInsertestimacionesdocumentos(tx, "0", pidestimacion, idocto.ToString(), docto, txtRow.Text, parchivozip, pobservaciones) == true)
                                {

                                }
                                else
                                {
                                    throw new System.InvalidOperationException("Error de datos");

                                }
                            }
                            else
                            {
                                throw new System.InvalidOperationException("Valor invalido" + docto);
                            }
                        }
                        else
                        {
                            throw new System.InvalidOperationException("Valor invalido" + docto);

                        }
                    }
                    else
                    {
                        funeliminarestimacionesdocumentos(tx, idestimaciondocto.ToString());

                    }
                }
                i += 1;

            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertestimacionesdocumentos(SqlTransaction tx, string piddocumentoestimacion, string pidestimacion, string piddocumento, string pdocumento, string phojas, string parchivo, string pobservaciones)
    {

        try
        {
            string[,] array = new string[,] { { "@iddocumentoestimacion", "8", piddocumentoestimacion }, { "@idestimacion", "8", pidestimacion }, { "@iddocumento", "8", piddocumento }, { "@documento", "12", pdocumento }, { "@hojas", "8", phojas }, { "@archivo", "12", parchivo }, { "@observaciones", "12", pobservaciones } };

            funInsert(tx, "Insert_tble_estimaciondocumentos", array);


            if (parchivo != "")
            {
                string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\adjudicaciones\ArchivosZip\";

                //=======================================================================================================================================================================================================================
                //====================================================================== para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================

                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones" }, { "@rutaarchivo", "12", Rutaraiz + parchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                //=======================================================================================================================================================================================================================
                //======================================================================   para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================
            }

            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }



    }

    #endregion
    #region archivos_ADG
    public Boolean funinsertarchivos(string iddocumento, string Contrato, string ruta, string fechapublicacion, string tipodocumento)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@iddocumento", "8", iddocumento }, { "@Contrato", "12", Contrato }, { "@ruta", "12", ruta }, { "@fechapublicacion", "4", fechapublicacion }, { "@tipodocumento", "12", tipodocumento } };
            funInsert(tx, "Insert_Documentoscontrato", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {

            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }

    #endregion

    #region usuarios_ADG
    public Boolean funInsertusuario(string Nombre, string Rfc, string idarearesponsableusuario, string descripcionarea, string idtipousuario, string descripciontipousuario, string idcontratista, string nombrecontratista, string FechaAlta, string Activo, string Usuario, string Password)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Nombre", "12", Nombre }, { "@Rfc", "12", Rfc }, { "@idarearesponsableusuario", "8", idarearesponsableusuario }, { "@descripcionarea", "12", descripcionarea },  { "@idtipousuario", "8", idtipousuario }, { "@descripciontipousuario", "12", descripciontipousuario }, { "@idcontratista", "8", idcontratista }, { "@nombrecontratista", "12", nombrecontratista }, { "@FechaAlta", "4", FechaAlta }, { "@Activo", "2", Activo }, { "@Usuario", "12", Usuario }, { "@Password", "12", Password } };
            funUpdateDelete(tx, "Insert_tblcu_Usuarios", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateusuario(string Idusuario,string Nombre, string Rfc, string idarearesponsableusuario, string descripcionarea, string idtipousuario, string descripciontipousuario, string idcontratista, string nombrecontratista, string FechaAlta, string Activo, string Usuario, string Password)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Idusuario", "8", Idusuario }, { "@Nombre", "12", Nombre }, { "@Rfc", "12", Rfc }, { "@idarearesponsableusuario", "8", idarearesponsableusuario }, { "@descripcionarea", "12", descripcionarea }, { "@idtipousuario", "8", idtipousuario }, { "@descripciontipousuario", "12", descripciontipousuario }, { "@idcontratista", "8", idcontratista }, { "@nombrecontratista", "12", nombrecontratista }, { "@FechaAlta", "4", FechaAlta }, { "@Activo", "2", Activo }, { "@Usuario", "12", Usuario }, { "@Password", "12", Password } };
            funUpdateDelete(tx, "Update_tblcu_UsuariosbyID", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }

    }
    public virtual void funeliminarUsuario(string Idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@Idusuario", "8", Idusuario.ToString() } };
            funUpdateDelete(tx, "Delete_tblcu_usuariosbyID", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region manoobra_KGS

    public Boolean funupdategeneral(string pcontrato,  string pmano, string pporcentaje, string pMontosiva, string pAnticiposiva, string pporcentajeagre)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Contrato", "12", pcontrato }, { "@manoobra", "5", pmano }, { "@PorceAnticipo", "5", pporcentaje }, { "@Montosiva", "5", pMontosiva }, { "@Anticiposiva", "5", pAnticiposiva }, { "@PorceAgremiado", "5", pporcentajeagre } };
            funUpdateDelete(tx, "Update_tbl_general", array);

          
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    #endregion
    #region supervisor_KGS

    public Boolean funupdategeneralSupervisor(string pcontrato, string pIDSupervisor, string pSupervisor)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Contrato", "12", pcontrato }, { "@IDSupervisor", "5", pIDSupervisor }, { "@Supervisor", "12", pSupervisor } };
            funUpdateDelete(tx, "Update_tbl_generalsupervisor", array);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    #endregion
    #region Bitacora_KGS
    public virtual void funGuardarBitacora(GridView grvbitacora)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            Int16 i = 0;
            foreach (GridViewRow row in grvbitacora.Rows)
            {
                //checamos si no existe el registro en la base insertamos               
                if (Convert.ToInt64(grvbitacora.DataKeys[i].Values[0]) <= 0)
                {
                    funInsertBitacora(tx, "0", grvbitacora.DataKeys[i].Values[1].ToString(), grvbitacora.DataKeys[i].Values[2].ToString(), grvbitacora.DataKeys[i].Values[3].ToString(), grvbitacora.DataKeys[i].Values[4].ToString(), grvbitacora.DataKeys[i].Values[5].ToString(), grvbitacora.DataKeys[i].Values[6].ToString(), grvbitacora.DataKeys[i].Values[7].ToString(), grvbitacora.DataKeys[i].Values[8].ToString(), grvbitacora.DataKeys[i].Values[9].ToString(), grvbitacora.DataKeys[i].Values[10].ToString());

                }

                i += 1;
            }


            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    public Boolean funInsertBitacora(SqlTransaction tx, string pidbitacora, string pidestimacion, string piddocumento, string pdocumento, string pidestatus, string pestatus, string pObservaciones, string pidusuario, string pFechaalta, string pObservacionesGenerales, string pArchivo)
    {
        try
        {
            
          

            string[,] array = new string[,] { { "@idbitacora", "8", pidbitacora }, { "@idestimacion", "8", pidestimacion }, { "@iddocumento", "8", piddocumento }, { "@documento", "12", pdocumento }, { "@idestatus", "8", pidestatus }, { "@estatus", "12", pestatus }, { "@Observaciones", "12", pObservaciones }, { "@idusuario", "8", pidusuario }, { "@fechaalta", "4", pFechaalta } , { "@ObservacionesGenerales", "12", pObservacionesGenerales }, { "@Archivo", "12", pArchivo } };
            funInsert(tx, "Insert_tble_bitacoradocumentos", array);

            if (pArchivo != "")
            {
                string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\adjudicaciones\ArchivosBitacora\";

                //=======================================================================================================================================================================================================================
                //====================================================================== para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================

                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones" }, { "@rutaarchivo", "12", Rutaraiz + pArchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                //=======================================================================================================================================================================================================================
                //======================================================================   para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================
            }




            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }


    }
    #endregion
    #region PRESUPUESTO_INICIAL_ADG
    public Boolean funupdatemontopresupuestal(string oficionumeroinicial,string archivo,string montototalaprobado,string idanio, string anio, string fechapublicacion)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@oficionumeroinicial", "12", oficionumeroinicial }, { "@archivo", "12", archivo }, { "@montototalaprobado", "5", montototalaprobado }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@fechapublicacion", "4", fechapublicacion } };
            funUpdateDelete(tx, "Update_PRE_Montototalpresupuestopreini", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    #endregion
    #region ADECUACIONES_ADG
    public Boolean funInsertadecuaciontmp(string numsec, string clavepresupuestaria, string t_o, string importe_operacion, string de, string al,string importe_mes,string Idusuario)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@numsec", "12", numsec }, { "@clavepresupuestaria", "12", clavepresupuestaria }, { "@t_o", "12", t_o }, { "@importe_operacion", "12", importe_operacion }, { "@de", "12", de }, { "@al", "12", al }, { "@importe_mes", "12", importe_mes }, { "@Idusuario", "8", Idusuario } };
            funUpdateDelete(tx, "Insert_PRE_Adecuaciontmp", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funInsertaradecuacion(DataTable datosmasivo)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        DataView dv;
        try
        {
            DateTime fecha = DateTime.Now;
            int anio = fecha.Year;
            foreach (DataRow row in datosmasivo.Rows)
            {
                //insertamos en la tabla tblp_PRE_adecuacionpresupuestaria
                String IDNew = "0";//0
                string[,] array = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@numsec", "12", row["numsec"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@RM", "12", row["RM"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@importeoperacion", "5", row["importeoperacion"].ToString() }, { "@DE", "8", row["DE"].ToString() }, { "@AL", "8", row["AL"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@concepto", "12", row["concepto"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@asunto", "12", row["asunto"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@montoadecuacion", "5", row["montoadecuacion"].ToString() }, { "@idestatus", "8", row["idestatus"].ToString() }, { "@estatus", "12", row["estatus"].ToString() } };
                funInsert(tx, "Insert_PRE_nuevaclavepresupuestaria", array);
                IDNew = IDNewx.ToString();
                //desglosamos los meses y su importe por cada registro
                string[,] arraymeses = new string[,] { { "@mesdel", "8", row["DE"].ToString() }, { "@mesal", "8", row["AL"].ToString() } };
                DataSet dsS = new DataSet();
                DataSet dsI = new DataSet();
                DataSet dsC = new DataSet();
                DataSet dsCero = new DataSet();
                DataSet dsIv=new DataSet();
                dsS = funGetbyStoresPropiomesadecuacion(tx, "sp_desglocemesnumeroletraadecuacion", arraymeses);
                if (dsS.Tables.Count > 0)
                {
                    if (dsS.Tables[0].Rows.Count > 0)
                    {
                        dv = new DataView(dsS.Tables[0]);
                        foreach (DataRowView dr in dv)//recorremos los meses de un registro
                        {
                            string[,] arraydesgloce = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@mesletra", "12", dr["mesletra"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@idadecuacionpresupuestaria", "8", IDNew.ToString() } };
                            funInsert(tx, "Insert_PRE_desglocenuevaclavepresupuestaria", arraydesgloce);
                            //*********verificamos si es una nueva clave la insertamos en la tabla de tblp_presupuestoegresosinicial siempre y cuando este en estatus de procesada*********
                            if (row["idestatus"].ToString() =="1")// diferente de estatus procesada
                            {
                            }
                            if (row["idestatus"].ToString() == "2") // estatus en tramite
                            {
                            }
                            if (row["idestatus"].ToString() == "3")// estatus procesada
                            {
                                //VERIFICAR QUE TIPO DE OPERACION ES PARA VERIFICAR SI SE INSERTA O NO EN LA TABLA tblp_presupuestoegresosinicial
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS RECEPTORES QUE SON 1->TRASPASO,RECALENDARIZACION 5->LIBERACION 3->AMPLIACION(3->REDUCCION NO APLICA YA QUE ES UNA CLAVE DE HACIENDA)*********************
                                //*********************************************************************************************************************************************************************  
                                if ((row["T_O"].ToString().Trim() != "4") && (row["T_O"].ToString().Trim() != "2")) //&& (row["T_O"].ToString().Trim() != "0")
                                {                                                                                                                             
                                    string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    dsI = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclave", arrayclave);//$$ modificacion de posicion de clave
                                    String IDNewok = "0";
                                    if (dsI.Tables[0].Rows.Count <= 0)//se debe insertar no importa si ya esta ya que la diferencia de uno a otro es el mes...
                                    {
                                        //pregunta se necesita verificar si existe la clave presupuestal y si existe el mes nada mas se actualiza los montos?
                                        //en le caso de la ampliacion el emisor debe ser omitido para ser insertado ya que es una clave de hacienda
                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) != 2) //en el caso de la reduccion todos los emisores se pasan por alto ya que es una clave de hacienda
                                        {
                                            string[,] arraydesglocepreini = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@mesletra", "12", dr["mesletra"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@anio", "8", anio.ToString() }, { "@fechapublicacion", "4", fecha.ToString() }, { "@oficionumeroadecuacion", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@tipo", "12", "A" } };
                                            funInsert(tx, "sp_PRE_insertarclavepresupuestalegresoinicial", arraydesglocepreini);//$$ modificacion de posicion de clave
                                            IDNewok = IDNewx.ToString();
                                            //una vez insertado se realiza la operacion de sumas y restas
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) ==1 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2) //en el caso de ampliación, reduccion cuando existe la clave y el mes
                                        {
                                            string[,] arraycomplementoampliacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumeroadecuacion", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() } };
                                            funUpdateDelete(tx, "Update_PRE_Datosclavetraspasoreceptorampliacion", arraycomplementoampliacion);
                                        }
                                    }
                                    string[,] arraydesglocepreinidatosdeoperacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    switch (Convert.ToInt32(row["idtipoadecuacion"].ToString()))//para realizar los mov. de sumas y restas se verificara la clave, mes de mov. y tipo de operación
                                    {// verificar lo que se manda a hacienda o recibe a hacienda no se realiza operación
                                        case 1://AMPLIACION
                                        case 2://REDUCCION
                                        case 3: //TRASPASO
                                        case 4: //LIBERACIÓN PREGUNTAR COMO FUNCIONA
                                        case 6: //RECALENDARIZACION
                                            int mesnumero = 0;
                                            decimal montoaprobado = 0;
                                            decimal aumento = 0;
                                            decimal disminucion = 0;
                                            decimal modificado = 0;
                                            decimal liberado = 0;
                                            decimal mesnumeronl = 0;
                                            //en el caso del tipo de adecuacion liberacion
                                            decimal aprobado_liberado = 0;
                                            decimal aumento_liberado = 0;
                                            decimal disminucion_liberado = 0;
                                            decimal modificado_liberado = 0;
                                            //para el mes cero
                                            int mesnumerocero = 0;
                                            decimal montoaprobadocero = 0;
                                            decimal aumentocero = 0;
                                            decimal disminucioncero = 0;
                                            decimal modificadocero = 0;
                                            decimal liberadocero = 0;
                                            dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacion);//$$ modificacion de posicion de clave
                                            if (dsC.Tables[0].Rows.Count > 0)
                                            {
                                                mesnumero = Convert.ToInt32(dsC.Tables[0].Rows[0]["mesnumero"].ToString());
                                                montoaprobado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                aumento = Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                disminucion = Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                modificado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["liberado"].ToString());
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 4)//LIBERACION
                                                {
                                                    if (dr["mesnumero"].ToString() == "0")//cuando se le libera el mes 0 sobre el mismo mes 0 
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------verificacion de saldo de clave cuando clave tiene calendario 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        mesnumeronl = Convert.ToDecimal(row["importexmes"].ToString());
                                                        if (mesnumeronl > modificado)
                                                        {
                                                            return false;
                                                        }
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------verificacion de saldo de clave cuando clave tiene calendario 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        disminucion = disminucion + Convert.ToDecimal(row["importexmes"].ToString());//disminuye el monto en el calendario 0
                                                        modificado = (montoaprobado + aumento) - disminucion;
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());
                                                        string[,] arrayactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacion", arrayactualizacionmontoliberacion);//$$ modificacion de posicion de clave
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                    if (dr["mesnumero"].ToString() != "0")//cuando se le libera el mes diferente de (0)
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------verificacion de saldo de clave cuando clave tiene calendario es <> 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        mesnumeronl = Convert.ToDecimal(row["importexmes"].ToString());
                                                        string[,] arraydesglocepreinidatosdeoperacioncero = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                        dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacioncero);
                                                        aprobado_liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                        aumento_liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                        disminucion_liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                        modificado_liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                        modificado_liberado = (aprobado_liberado + aumento_liberado) - disminucion_liberado;
                                                        if (dsC.Tables[0].Rows.Count > 0)
                                                        {

                                                            if (mesnumeronl > modificado_liberado)
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------verificacion de saldo de clave cuando clave tiene calendario es <> 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        modificado = (montoaprobado + aumento) - (disminucion);
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());//pregunta cuando hay liberacion se mueve el modificado?
                                                        string[,] arrayactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacioncalendarizado", arrayactualizacionmontoliberacion);
                                                        //********************verificar la disminucion de la misma clave pero en el calendario 0
                                                        string[,] arraydatoscalendariocero = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                        dsCero = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydatoscalendariocero);
                                                        if (dsCero.Tables[0].Rows.Count > 0)
                                                        {
                                                            mesnumerocero = Convert.ToInt32(dsCero.Tables[0].Rows[0]["mesnumero"].ToString());
                                                            montoaprobadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                            aumentocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["aumento"].ToString());
                                                            disminucioncero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["disminucion"].ToString());
                                                            modificadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["modificado"].ToString());
                                                            liberadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["liberado"].ToString());
                                                            disminucioncero = disminucioncero + Convert.ToDecimal(row["importexmes"].ToString());
                                                            modificadocero = (montoaprobadocero + aumentocero) - (disminucioncero);
                                                            string[,] arrayactualizacionmontoliberacion1 = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" }, { "@disminucion", "5", disminucioncero.ToString().Trim() }, { "@modificado", "5", modificadocero.ToString().Trim() } };
                                                            funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacioncalendarizadocero", arrayactualizacionmontoliberacion1);//$$ modificacion de posicion de clave
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }

                                                        //********************verificar la disminucion de la misma clave pero en el calendario 0
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) != 2) //cuando es reducción no aplica ya que el tipo de operación 3 es de hacienda
                                                    {
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        modificado = (montoaprobado + aumento) - (disminucion);
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());//preguntar si el liberado es igual al modificado
                                                                                                                               //actualizamos la operacion
                                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 3) //cuando es AMPLIACION cuando es mes 0 ó mayor que 0
                                                        {
                                                            if (dr["mesnumero"].ToString() == "0")//cuando se le amplia el mes 0 se realiza cambios en los campos aumento y modificado
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorampliacioncero", arrayactualizacionmontos);//$$ modificacion de posicion de clave
                                                            }
                                                            if (dr["mesnumero"].ToString() != "0")// cuando se le amplia un mes del calendario se realiza cambios en los campos AUMENTO-MODIFICADO-LIBERADO
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberado", "5", liberado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptor", arrayactualizacionmontos);
                                                            }
                                                        }
                                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6) //cuando es RECALENDATIZACION cuando es mes 0 ó mayor que 0
                                                        {
                                                            if (dr["mesnumero"].ToString() != "0")// cuando se recalendariza se realiza cambios en los campos AUMENTO-MODIFICACION-LIBERADO
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberado", "5", liberado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorcalendarizacion", arrayactualizacionmontos);
                                                            }
                                                        }

                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", aumento.ToString().Trim() }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2)//cuando el que recibe es una reduccion no se aplica ningun aumento ya que es una clave de hacienda pero si se guarda en el historial
                                                {
                                                    string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", row["importeoperacion"].ToString() }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                    funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                                break;
                                        //case 4: //LIBERACIÓN PREGUNTAR COMO FUNCIONA

                                        //    break;
                                        case 5: //RETENSION

                                            break;
                                        //case 6: //RECALENDARIZACION

                                        //    break;
                                        default:
                                            break;
                                    }
                                }
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS RECEPTORES QUE SON 1->TRASPASO,RECALENDARIZACION 5->LIBERACION 3->AMPLIACION(3->REDUCCION NO APLICA YA QUE ES UNA CLAVE DE HACIENDA)*********************
                                //*********************************************************************************************************************************************************************  

                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS EMISORES QUE SON 2->TRASPASO,RECALENDARIZACION 4->AMPLIACIÓN (4->REDUCCION )*****************************************************************************
                                //*********************************************************************************************************************************************************************  
                                else //***PARA LOS EMISORES****
                                {
                                    
                                    string[,] arraydesglocepreinidatosdeoperacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    switch (Convert.ToInt32(row["idtipoadecuacion"].ToString()))//para realizar los mov. de sumas y restas se verificara la clave, mes de mov. y tipo de operación
                                    {// verificar lo que se manda a hacienda o recibe a hacienda no se realiza operación
                                        case 1://AMPLIACION
                                        case 2://REDUCCION
                                        case 3 ://TRASPASO
                                        case 6: //RECALENDARIZACION
                                            int mesnumero = 0;
                                            decimal montoaprobado = 0;
                                            decimal aumento = 0;
                                            decimal disminucion = 0;
                                            decimal modificado = 0;
                                            decimal liberado = 0;
                                            dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacion);
                                            if (dsC.Tables[0].Rows.Count > 0)
                                            {
                                                mesnumero = Convert.ToInt32(dsC.Tables[0].Rows[0]["mesnumero"].ToString());
                                                montoaprobado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                aumento = Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                disminucion = Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                modificado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["liberado"].ToString());
                                                disminucion = disminucion + Convert.ToDecimal(row["importexmes"].ToString());
                                                modificado = (montoaprobado + aumento) - (disminucion);
                                               
                                                //actualizamos la operacion
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString())==3 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2 )//traspaso ->3, reduccion->2, recalendarizacion ->6
                                                {
                                                    if (dr["mesnumero"].ToString() == "0")//cuando el traspaso y reduccion toman el calendario 0->no liberado realiza cambios en los campos DISMINUCION y MODIFICADO
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario 0--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                            if ((row["T_O"].ToString().Trim() == "2") || (row["T_O"].ToString().Trim() == "4") )
                                                            {
                                                                //verificacion de la clave si tiene recurso
                                                                
                                                                decimal liberadov = 0;
                                                                decimal liberadobasev = 0;
                                                                decimal aprobadov = 0;
                                                                decimal modificadov = 0;
                                                                decimal aumentov = 0;
                                                                decimal disminucionv = 0;
                                                                string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                                dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                                if (dsIv.Tables[0].Rows.Count > 0)
                                                                {
                                                                    //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                                    liberadov = Convert.ToDecimal(row["importexmes"].ToString().Trim());
                                                                    liberadobasev = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["liberado"].ToString());
                                                                    aprobadov = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                                    aumentov = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                                    disminucionv = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                                    modificadov = (aprobadov + aumentov) - disminucionv;
                                                                    if (liberadov > modificadov) //if (liberado > liberadobase)
                                                                    {
                                                                        return false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    return false;
                                                                }
                                                            }
                                                        
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario 0--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisor", arrayactualizacionmontos);//$$ modificacion de posicion de clave
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                    if (dr["mesnumero"].ToString() != "0")//cuando el traspaso y reduccion toman el calendario  <> 0->no liberado realiza cambios en los campos DISMINUCION , MODIFICADO Y LIBERADO 
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario <>0------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

                                                        if ((Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2) || (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 3)  || (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6))//REDUCCION-TRASPASO-LIBERACION(se quito)-RECALENDARIZACION
                                                        {
                                                            //*********************VERIFICAR EN EL CASO DE ESTOS ESTATUS SI COMO EMISOR SE PUEDE DEFINIR MAS DE UN CALENDARIO*****************
                                                            decimal aprobadovc = 0;
                                                            decimal modificadovc = 0;
                                                            decimal aumentovc = 0;
                                                            decimal disminucionvc = 0;
                                                            decimal montosolicitadovc = 0;
                                                            string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                                            dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                            if (dsIv.Tables[0].Rows.Count > 0)
                                                            {
                                                                //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                                aprobadovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                                aumentovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                                disminucionvc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                                modificadovc = (aprobadovc + aumentovc) - disminucionvc;
                                                                montosolicitadovc = Convert.ToDecimal(row["importexmes"].ToString().Trim());
                                                                if (montosolicitadovc > modificadovc) //if (liberado > liberadobase)
                                                                {
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                            //*********************VERIFICAR EN EL CASO DE ESTOS ESTATUS SI COMO EMISOR SE PUEDE DEFINIR MAS DE UN CALENDARIO*****************
                                                        }
                                                        //******************verificamos si la clave tiene recurso para realizar un traspaso o una recalendarización
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario <>0------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        liberado = liberado- Convert.ToDecimal(row["importexmes"].ToString());//si es diferente de calendario 0 se agrega la modificacion del liberado
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisorliberacion", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                if ( Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6)// recalendarizacion ->6
                                                {
                                                    if (dr["mesnumero"].ToString() != "0")//cuando el traspaso y reduccion toman el calendario  <> 0->no liberado realiza cambios en los campos DISMINUCION , MODIFICADO Y LIBERADO 
                                                    {
                                                        decimal aprobadovc = 0;
                                                        decimal modificadovc = 0;
                                                        decimal aumentovc = 0;
                                                        decimal disminucionvc = 0;
                                                        decimal montosolicitadovc = 0;
                                                        string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                                        dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                        if (dsIv.Tables[0].Rows.Count > 0)
                                                        {
                                                            //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                            aprobadovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                            aumentovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                            disminucionvc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                            modificadovc = (aprobadovc + aumentovc) - disminucionvc;
                                                            montosolicitadovc = Convert.ToDecimal(row["importexmes"].ToString().Trim());
                                                            if (montosolicitadovc > modificadovc) //if (liberado > liberadobase)
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                        liberado = liberado - Convert.ToDecimal(row["importexmes"].ToString());//si es diferente de calendario 0 se agrega la modificacion del liberado
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisorcalendarizacion", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1)//si es ampliacion no aplica ningun movimiento ya que es una clave de hacienda 
                                                {
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1)//si es ampliacion no aplica ningun movimiento ya que es una clave de hacienda solo se guarda en historial
                                                {
                                                    string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", row["importeoperacion"].ToString() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                    funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                                // Console.WriteLine("Por alguna razon la clave presupuestal no fue encontrada verifica con tu administrador...");
                                            }
                                            break;
                                        case 4: //LIBERACIÓN

                                            break;
                                        case 5: //RETENSION

                                            break;
                                        //case 6: //RECALENDARIZACION

                                        //    break;
                                        default:

                                            break;
                                    }
                                }
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS EMISORES QUE SON 2->TRASPASO,RECALENDARIZACION 4->AMPLIACIÓN (4->REDUCCION )*****************************************************************************
                                //*********************************************************************************************************************************************************************  
                            }
                            //********verificamos si es una nueva clave la insertamos en la tabla de tblp_presupuestoegresosinicial siempre y cuando este en estatus de procesada************
                        }
                    }
                }
            }
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funActualizaradecuacion(DataTable datosmasivo, string lbleliminarclavepresupuestal, string lblmodificarclavepresupuestal)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        DataView dv;
        try
        {
            DateTime fecha = DateTime.Now;
            int anio = fecha.Year;
            //*************************************************************************************************************************************************************
            //*********************eliminamos los cambios en la tabla de la adecuacion presupuestaria en los estatus en formulacion y en tramite***************************
            //*************************************************************************************************************************************************************
            if (lbleliminarclavepresupuestal != "")
            {
                string[,] arrayidadecuacioneliminar = new string[,] { { "@eliminarclavesids", "12", lbleliminarclavepresupuestal } };
                funUpdateDelete(tx, "Delete_PRE_Datosidadecuacion", arrayidadecuacioneliminar);
            }
            if (lblmodificarclavepresupuestal != "")
            {
                string[,] arrayidadecuacionmodificar = new string[,] { { "@eliminarclavesidmodificar", "12", lblmodificarclavepresupuestal } };
                funUpdateDelete(tx, "Delete_PRE_Datosidadecuacionmodificar", arrayidadecuacionmodificar);
            }
            //*************************************************************************************************************************************************************
            //*********************eliminamos los cambios en la tabla de la adecuacion presupuestaria en los estatus en formulacion y en tramite***************************
            //*************************************************************************************************************************************************************
            foreach (DataRow row in datosmasivo.Rows)
            {
                String IDNew = "0";//0
                String IDNewA = "0";
                //insertamos en la tabla tblp_PRE_adecuacionpresupuestaria si es nueva clave o clave modificada
                if (Convert.ToInt64(row["idadecuacionpresupuestaria"]) <= 0)
                {
                    string[,] array = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@numsec", "12", row["numsec"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@RM", "12", row["RM"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@importeoperacion", "5", row["importeoperacion"].ToString() }, { "@DE", "8", row["DE"].ToString() }, { "@AL", "8", row["AL"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@concepto", "12", row["concepto"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@asunto", "12", row["asunto"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@montoadecuacion", "5", row["montoadecuacion"].ToString() }, { "@idestatus", "8", row["idestatus"].ToString() }, { "@estatus", "12", row["estatus"].ToString() } };
                    funInsert(tx, "Insert_PRE_nuevaclavepresupuestaria", array);
                    IDNew = IDNewx.ToString();
                    //desglosamos los meses y su importe por cada registro
                }
                else
                {
                    //actualizar el estatus de la clave presupuestal
                    string[,] arrayclaveupdatestatus = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@idadecuacionpresupuestaria", "8", row["idadecuacionpresupuestaria"].ToString() }, { "@idestatus", "8", row["idestatus"].ToString() }, { "@estatus", "12", row["estatus"].ToString() } };
                    funUpdateDelete(tx, "Update_PRE_claveactualizacionestatus", arrayclaveupdatestatus);
                    //actualizar el estatus de la clave presupuestal
                }
                string[,] arraymeses = new string[,] { { "@mesdel", "8", row["DE"].ToString() }, { "@mesal", "8", row["AL"].ToString() } };
                DataSet dsS = new DataSet();
                DataSet dsI = new DataSet();
                DataSet dsC = new DataSet();
                DataSet dsCero = new DataSet();
                DataSet dsIv = new DataSet();
                dsS = funGetbyStoresPropiomesadecuacion(tx, "sp_desglocemesnumeroletraadecuacion", arraymeses);
                if (dsS.Tables.Count > 0)
                {
                    if (dsS.Tables[0].Rows.Count > 0)
                    {
                        dv = new DataView(dsS.Tables[0]);
                        foreach (DataRowView dr in dv)//recorremos los meses de un registro
                        {
                            if (Convert.ToInt64(row["idadecuacionpresupuestaria"]) <= 0)
                            {
                                string[,] arraydesgloce = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@mesletra", "12", dr["mesletra"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@RM", "12", row["RM"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@idadecuacionpresupuestaria", "8", IDNew.ToString() } };
                                funInsert(tx, "Insert_PRE_desglocenuevaclavepresupuestaria", arraydesgloce);
                            }
                            //*********verificamos si es una nueva clave la insertamos en la tabla de tblp_presupuestoegresosinicial siempre y cuando este en estatus de procesada*********
                            if (row["idestatus"].ToString() == "1")// diferente de estatus procesada
                            {
                            }
                            if (row["idestatus"].ToString() == "2") // estatus en tramite
                            {
                            }
                            if (row["idestatus"].ToString() == "3")// estatus procesada
                            {
                                //VERIFICAR QUE TIPO DE OPERACION ES PARA VERIFICAR SI SE INSERTA O NO EN LA TABLA tblp_presupuestoegresosinicial
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS RECEPTORES QUE SON 1->TRASPASO,RECALENDARIZACION 5->LIBERACION 3->AMPLIACION(3->REDUCCION NO APLICA YA QUE ES UNA CLAVE DE HACIENDA)*********************
                                //********************************************************************************************************************************************************************* 
                                if ((row["T_O"].ToString().Trim() != "4") && (row["T_O"].ToString().Trim() != "2")) //&& (row["T_O"].ToString().Trim() != "0")***PARA LOS RECEPTORES****
                                {
                                    string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    dsI = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclave", arrayclave);
                                    
                                    if (dsI.Tables[0].Rows.Count <= 0)//se debe insertar no importa si ya esta ya que la diferencia de uno a otro es el mes...
                                    {
                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) != 2) //en el caso de la reduccion todos los emisores se pasan por alto ya que es una clave de hacienda
                                        {
                                            //pregunta se necesita verificar si existe la clave presupuestal y si existe el mes nada mas se actualiza los montos?
                                            string[,] arraydesglocepreini = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@mesletra", "12", dr["mesletra"].ToString() }, { "@importexmes", "5", row["importexmes"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@anio", "8", anio.ToString() }, { "@fechapublicacion", "4", fecha.ToString() }, { "@oficionumeroadecuacion", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@tipo", "12", "A" } };
                                            funInsert(tx, "sp_PRE_insertarclavepresupuestalegresoinicial", arraydesglocepreini);
                                            IDNewA = IDNewx.ToString();
                                            //una vez insertado se realiza la operacion de sumas y restas
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2) //en el caso de ampliación, reduccion cuando existe la clave y el mes
                                        {
                                            string[,] arraycomplementoampliacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumeroadecuacion", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() } };
                                            funUpdateDelete(tx, "Update_PRE_Datosclavetraspasoreceptorampliacion", arraycomplementoampliacion);
                                        }
                                    }
                                    string[,] arraydesglocepreinidatosdeoperacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    switch (Convert.ToInt32(row["idtipoadecuacion"].ToString()))//para realizar los mov. de sumas y restas se verificara la clave, mes de mov. y tipo de operación
                                    {// verificar lo que se manda a hacienda o recibe a hacienda no se realiza operación
                                        case 1://AMPLIACION
                                        case 2://REDUCCION
                                        case 3: //TRASPASO
                                        case 4: //LIBERACIÓN PREGUNTAR COMO FUNCIONA
                                        case 6: //RECALENDARIZACION
                                            int mesnumero = 0;
                                            decimal montoaprobado = 0;
                                            decimal aumento = 0;
                                            decimal disminucion = 0;
                                            decimal modificado = 0;
                                            decimal liberado = 0;
                                            decimal mesnumeronl = 0;
                                            //en el caso del tipo de adecuacion liberacion
                                            decimal aprobado_liberado = 0;
                                            decimal aumento_liberado = 0;
                                            decimal disminucion_liberado = 0;
                                            decimal modificado_liberado = 0;
                                            //para el mes cero
                                            int mesnumerocero = 0;
                                            decimal montoaprobadocero = 0;
                                            decimal aumentocero = 0;
                                            decimal disminucioncero = 0;
                                            decimal modificadocero = 0;
                                            decimal liberadocero = 0;
                                            dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacion);
                                            if (dsC.Tables[0].Rows.Count > 0)
                                            {
                                                mesnumero = Convert.ToInt32(dsC.Tables[0].Rows[0]["mesnumero"].ToString());
                                                montoaprobado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                aumento = Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                disminucion = Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                modificado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["liberado"].ToString());
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 4)//LIBERACION
                                                {
                                                    if (dr["mesnumero"].ToString() == "0")//cuando se le libera el mes 0 sobre el mismo mes 0 
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------verificacion de saldo de clave cuando clave tiene calendario 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        mesnumeronl = Convert.ToDecimal(row["importexmes"].ToString());
                                                        if (mesnumeronl > modificado)
                                                        {
                                                            return false;
                                                        }
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------verificacion de saldo de clave cuando clave tiene calendario 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        disminucion = disminucion + Convert.ToDecimal(row["importexmes"].ToString());//disminuye el monto en el calendario 0
                                                        modificado = (montoaprobado + aumento) - disminucion;
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());
                                                        string[,] arrayactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacion", arrayactualizacionmontoliberacion);
                                                        //string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", liberado.ToString().Trim() } };
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", row["importexmes"].ToString() } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                    if (dr["mesnumero"].ToString() != "0")//cuando se le libera el mes diferente de (0)
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------verificacion de saldo de clave cuando clave tiene calendario es <> 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        mesnumeronl = Convert.ToDecimal(row["importexmes"].ToString());
                                                        string[,] arraydesglocepreinidatosdeoperacioncero = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                        dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacioncero);
                                                        aprobado_liberado= Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                        aumento_liberado= Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                        disminucion_liberado= Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                        modificado_liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                        modificado_liberado = (aprobado_liberado + aumento_liberado) - disminucion_liberado;
                                                        if (dsC.Tables[0].Rows.Count > 0)
                                                        {

                                                            if (mesnumeronl > modificado_liberado)
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------verificacion de saldo de clave cuando clave tiene calendario es <> 0 en el tipo de adecuacion LIBERACION--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        modificado = (montoaprobado + aumento) - (disminucion);
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());//pregunta cuando hay liberacion se mueve el modificado?
                                                        string[,] arrayactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacioncalendarizado", arrayactualizacionmontoliberacion);
                                                        //********************verificar la disminucion de la misma clave pero en el calendario 0
                                                        string[,] arraydatoscalendariocero = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                        dsCero = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydatoscalendariocero);
                                                        if (dsCero.Tables[0].Rows.Count > 0)
                                                        {
                                                            mesnumerocero = Convert.ToInt32(dsCero.Tables[0].Rows[0]["mesnumero"].ToString());
                                                            montoaprobadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                            aumentocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["aumento"].ToString());
                                                            disminucioncero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["disminucion"].ToString());
                                                            modificadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["modificado"].ToString());
                                                            liberadocero = Convert.ToDecimal(dsCero.Tables[0].Rows[0]["liberado"].ToString());
                                                            disminucioncero = disminucioncero + Convert.ToDecimal(row["importexmes"].ToString());
                                                            modificadocero = (montoaprobadocero + aumentocero) - (disminucioncero);
                                                            string[,] arrayactualizacionmontoliberacion1 = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" }, { "@disminucion", "5", disminucioncero.ToString().Trim() }, { "@modificado", "5", modificadocero.ToString().Trim() } };
                                                            funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorliberacioncalendarizadocero", arrayactualizacionmontoliberacion1);
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }

                                                        //********************verificar la disminucion de la misma clave pero en el calendario 0
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) != 2) //cuando es reducción no aplica ya que el tipo de operación 3 es de hacienda
                                                    {
                                                        aumento = aumento + Convert.ToDecimal(row["importexmes"].ToString());
                                                        modificado = (montoaprobado + aumento) - (disminucion);
                                                        liberado = liberado + Convert.ToDecimal(row["importexmes"].ToString());//preguntar si el liberado es igual al modificado
                                                                                                                               //actualizamos la operacion
                                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 3) //cuando es AMPLIACION cuando es mes 0 ó mayor que 0
                                                        {
                                                            if (dr["mesnumero"].ToString() == "0")//cuando se le amplia el mes 0 se realiza cambios en los campos aumento y modificado
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorampliacioncero", arrayactualizacionmontos);
                                                            }
                                                            if (dr["mesnumero"].ToString() != "0")// cuando se le amplia un mes del calendario se realiza cambios en los campos AUMENTO-MODIFICADO-LIBERADO
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberado", "5", liberado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptor", arrayactualizacionmontos);
                                                            }
                                                        }
                                                        if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6) //cuando es RECALENDATIZACION cuando es mes 0 ó mayor que 0
                                                        {
                                                            if (dr["mesnumero"].ToString() != "0")// cuando se recalendariza se realiza cambios en los campos AUMENTO-MODIFICACION-LIBERADO
                                                            {
                                                                string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberado", "5", liberado.ToString().Trim() } };
                                                                funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptorcalendarizacion", arrayactualizacionmontos);
                                                            }
                                                        }
                                                        //string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@aumento", "5", aumento.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberado", "5", liberado.ToString().Trim() } };
                                                        //funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoreceptor", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", aumento.ToString().Trim() }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2)//cuando el que recibe es una reduccion no se aplica ningun aumento ya que es una clave de hacienda pero si se guarda en el historial
                                                {
                                                    string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", row["importeoperacion"].ToString() }, { "@mov_disminucion", "5", "0" }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                    funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);

                                                }
                                                else
                                                {
                                                    return false;
                                                }

                                            }
                                            break;
                                        //case 4: //LIBERACIÓN PREGUNTAR COMO FUNCIONA

                                        //    break;
                                        case 5: //RETENSION

                                            break;
                                        //case 6: //RECALENDARIZACION

                                        //    break;
                                        default:

                                            break;
                                    }
                                }
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS RECEPTORES QUE SON 1->TRASPASO,RECALENDARIZACION 5->LIBERACION 3->AMPLIACION(3->REDUCCION NO APLICA YA QUE ES UNA CLAVE DE HACIENDA)*********************
                                //*********************************************************************************************************************************************************************  

                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS EMISORES QUE SON 2->TRASPASO,RECALENDARIZACION 4->AMPLIACIÓN (4->REDUCCION )*****************************************************************************
                                //*********************************************************************************************************************************************************************  
                                else //***PARA LOS EMISORES****
                                {
                                    string[,] arraydesglocepreinidatosdeoperacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    switch (Convert.ToInt32(row["idtipoadecuacion"].ToString()))//para realizar los mov. de sumas y restas se verificara la clave, mes de mov. y tipo de operación
                                    {// verificar lo que se manda a hacienda o recibe a hacienda no se realiza operación
                                        case 1://AMPLIACION
                                        case 2://REDUCCION
                                        case 3://TRASPASO
                                        case 6: //RECALENDARIZACION
                                            int mesnumero = 0;
                                            decimal montoaprobado = 0;
                                            decimal aumento = 0;
                                            decimal disminucion = 0;
                                            decimal modificado = 0;
                                            decimal liberado = 0;
                                            dsC = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arraydesglocepreinidatosdeoperacion);
                                            if (dsC.Tables[0].Rows.Count > 0)//verificamos si es una clave diferente a la de hacienda
                                            {
                                                mesnumero = Convert.ToInt32(dsC.Tables[0].Rows[0]["mesnumero"].ToString());
                                                montoaprobado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                aumento = Convert.ToDecimal(dsC.Tables[0].Rows[0]["aumento"].ToString());
                                                disminucion = Convert.ToDecimal(dsC.Tables[0].Rows[0]["disminucion"].ToString());
                                                modificado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["modificado"].ToString());
                                                liberado = Convert.ToDecimal(dsC.Tables[0].Rows[0]["liberado"].ToString());
                                                disminucion = disminucion + Convert.ToDecimal(row["importexmes"].ToString());
                                                modificado = (montoaprobado + aumento) - (disminucion);
                                                //actualizamos la operacion 
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 3 || Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2 )//traspaso ->3, reduccion->2, recalendarizacion ->6
                                                {
                                                    if (dr["mesnumero"].ToString() == "0")//cuando el traspaso y reduccion toman el calendario 0->no liberado realiza cambios en los campos DISMINUCION y MODIFICADO
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario 0--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

                                                        if ((row["T_O"].ToString().Trim() == "2") || (row["T_O"].ToString().Trim() == "4"))
                                                        {
                                                            //verificacion de la clave si tiene recurso

                                                            decimal liberadov = 0;
                                                            decimal liberadobasev = 0;
                                                            decimal aprobadov = 0;
                                                            decimal modificadov = 0;
                                                            decimal aumentov = 0;
                                                            decimal disminucionv = 0;
                                                            string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", "0" } };
                                                            dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                            if (dsIv.Tables[0].Rows.Count > 0)
                                                            {
                                                                //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                                liberadov = Convert.ToDecimal(row["importexmes"].ToString().Trim());
                                                                liberadobasev = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["liberado"].ToString());
                                                                aprobadov = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                                aumentov = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                                disminucionv = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                                modificadov = (aprobadov + aumentov) - disminucionv;
                                                                if (liberadov > modificadov) //if (liberado > liberadobase)
                                                                {
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                        }

                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario 0--------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisor", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                    if (dr["mesnumero"].ToString() != "0")//cuando el traspaso y reduccion toman el calendario  <> 0->no liberado realiza cambios en los campos DISMINUCION , MODIFICADO Y LIBERADO 
                                                    {
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario <>0------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

                                                        if ((Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 2) || (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 3) || (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6))//REDUCCION-TRASPASO-LIBERACION(se quito)-RECALENDARIZACION
                                                        {
                                                            //*********************VERIFICAR EN EL CASO DE ESTOS ESTATUS SI COMO EMISOR SE PUEDE DEFINIR MAS DE UN CALENDARIO*****************
                                                            decimal aprobadovc = 0;
                                                            decimal modificadovc = 0;
                                                            decimal aumentovc = 0;
                                                            decimal disminucionvc = 0;
                                                            decimal montosolicitadovc = 0;
                                                            string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                                            dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                            if (dsIv.Tables[0].Rows.Count > 0)
                                                            {
                                                                //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                                aprobadovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                                aumentovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                                disminucionvc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                                modificadovc = (aprobadovc + aumentovc) - disminucionvc;
                                                                montosolicitadovc = Convert.ToDecimal(row["importexmes"].ToString().Trim());

                                                                //sumamontosolicitado = sumamontosolicitado + montosolicitado;
                                                                if (montosolicitadovc > modificadovc) //if (liberado > liberadobase)
                                                                {
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                return false;
                                                            }
                                                            //*********************VERIFICAR EN EL CASO DE ESTOS ESTATUS SI COMO EMISOR SE PUEDE DEFINIR MAS DE UN CALENDARIO*****************
                                                        }
                                                        //******************verificamos si la clave tiene recurso para realizar un traspaso o una recalendarización
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        //-------------------------------------------------------------verificacion de saldo de clave cuando clave tiene calendario <>0------------------------------
                                                        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
                                                        liberado = liberado- Convert.ToDecimal(row["importexmes"].ToString());//si es diferente de calendario 0 se agrega la modificacion del liberado
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisorliberacion", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 6)// recalendarizacion ->6
                                                {
                                                    if (dr["mesnumero"].ToString() != "0")//cuando el traspaso y reduccion toman el calendario  <> 0->no liberado realiza cambios en los campos DISMINUCION , MODIFICADO Y LIBERADO 
                                                    {
                                                        decimal aprobadovc = 0;
                                                        decimal modificadovc = 0;
                                                        decimal aumentovc = 0;
                                                        decimal disminucionvc = 0;
                                                        decimal montosolicitadovc = 0;
                                                        string[,] arrayclave = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                                        dsIv = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavecondatos", arrayclave);
                                                        if (dsIv.Tables[0].Rows.Count > 0)
                                                        {
                                                            //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                                            aprobadovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["montoaprobado"].ToString());
                                                            aumentovc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["aumento"].ToString());
                                                            disminucionvc = Convert.ToDecimal(dsIv.Tables[0].Rows[0]["disminucion"].ToString());
                                                            modificadovc = (aprobadovc + aumentovc) - disminucionvc;
                                                            montosolicitadovc = Convert.ToDecimal(row["importexmes"].ToString().Trim());

                                                            //sumamontosolicitado = sumamontosolicitado + montosolicitado;
                                                            if (montosolicitadovc > modificadovc) //if (liberado > liberadobase)
                                                            {
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                        liberado = liberado - Convert.ToDecimal(row["importexmes"].ToString());//si es diferente de calendario 0 se agrega la modificacion del liberado
                                                        string[,] arrayactualizacionmontos = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@disminucion", "5", disminucion.ToString().Trim() }, { "@modificado", "5", modificado.ToString().Trim() }, { "@liberacion", "5", liberado.ToString().Trim() } };
                                                        funUpdateDelete(tx, "Update_PRE_Montoclavetraspasoemisorcalendarizacion", arrayactualizacionmontos);
                                                        string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", disminucion.ToString().Trim() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                        funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                    }
                                                }
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1)//si es ampliacion no aplica ningun movimiento ya que es una clave de hacienda 
                                                {

                                                }
                                            }
                                            else
                                            {
                                                // Console.WriteLine("Por alguna razon la clave presupuestal no fue encontrada verifica con tu administrador...");
                                                if (Convert.ToInt32(row["idtipoadecuacion"].ToString()) == 1)//si es ampliacion no aplica ningun movimiento ya que es una clave de hacienda solo se guarda en historial
                                                {
                                                    string[,] arrayhistorialactualizacionmontoliberacion = new string[,] { { "@cve", "12", row["cve"].ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() }, { "@fechasolicitud", "4", row["fechasolicitud"].ToString() }, { "@fechamovimiento", "4", DateTime.Now.ToString() }, { "@tipo", "12", "PROCESADA" }, { "@T_O", "12", row["T_O"].ToString().Trim() }, { "@mov_aumento", "5", "0" }, { "@mov_disminucion", "5", row["importeoperacion"].ToString() }, { "@mov_modificado", "5", "0" }, { "@mov_liberado", "5", "0" } };
                                                    funUpdateDelete(tx, "Insert_PRE_HistorialMontoclavetraspasoreceptorliberacion", arrayhistorialactualizacionmontoliberacion);
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            break;
                                        case 4: //LIBERACIÓN

                                            break;
                                        case 5: //RETENSION

                                            break;
                                        //case 6: //RECALENDARIZACION

                                        //    break;
                                        default:

                                            break;
                                    }
                                }
                                //*********************************************************************************************************************************************************************  
                                //***PARA LOS EMISORES QUE SON 2->TRASPASO,RECALENDARIZACION 4->AMPLIACIÓN (4->REDUCCION )*****************************************************************************
                                //*********************************************************************************************************************************************************************  
                            }
                            //********verificamos si es una nueva clave la insertamos en la tabla de tblp_presupuestoegresosinicial siempre y cuando este en estatus de procesada************
                        }
                    }
                }
            }
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funinsertarclavenueva(SqlTransaction tx,string cve,string numsec,string CA,string UR,string FI, string FU,string SF,string SSF,string PS,string PP,string OA,string AI, string PT,string MPIO, string COGC, string OG,string TG, string TR, string FF, string SFF, string RM, string PF,string CP,string DM,string T_O,string importeoperacion,string DE,string AL,string importexmes,string idtipoadecuacion,string tipoadecuacion,string numsolicitud,string fechasolicitud,string concepto,string oficionumero,string asunto,string fechaingreso)
    {
        try
        {
            string[,] array = new string[,] { { "@cve", "12", cve }, { "@numsec", "12", numsec }, { "@CA", "12", CA }, { "@UR", "12", UR }, { "@FI", "12", FI }, { "@FU", "12", FU }, { "@SF", "12", SF }, { "@SSF", "12", SSF }, { "@PS", "12", PS }, { "@PP", "12", PP }, { "@OA", "12", OA }, { "@AI", "12", AI }, { "@PT", "12", PT }, { "@MPIO", "12", MPIO }, { "@COGC", "12", COGC }, { "@OG", "12", OG }, { "@TG", "12", TG }, { "@TR", "12", TR }, { "@FF", "12", FF }, { "@SFF", "12", SFF }, { "@RM", "12", RM }, { "@PF", "12", PF }, { "@CP", "12", CP }, { "@DM", "12", DM }, { "@T_O", "12", T_O }, { "@importeoperacion", "5", importeoperacion }, { "@DE", "8", DE }, { "@AL", "8", AL }, { "@importexmes", "5", importexmes }, { "@idtipoadecuacion", "8", idtipoadecuacion }, { "@tipoadecuacion", "12", tipoadecuacion }, { "@numsolicitud", "12", numsolicitud }, { "@fechasolicitud", "4", fechasolicitud }, { "@concepto", "12", concepto }, { "@oficionumero", "12", oficionumero }, { "@asunto", "12", asunto }, { "@fechaingreso", "4", fechaingreso } };
            funInsert(tx, "Insert_PRE_nuevaclavepresupuestaria", array);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;
        }
    }
    public virtual DataSet funGetbyStoresPropiomesadecuacion(SqlTransaction tx, string pNombreStored, string[,] pArreglo)
    {
        //'Creación de la función pública utilizada para llamar a todos los storeds
        //       ' pasando como parámetros el nombre del stored correspondiente y un 
        //       ' arreglo(nombre de la variable, tipo de dato, valor de la varible)
        DataSet ds = new DataSet();
        try
        {
            if (pArreglo.GetUpperBound(1) > 0)
            {
                int upper = pArreglo.GetUpperBound(1);
                int lower = pArreglo.GetLowerBound(0);
                SqlParameter[] sqlParams = new SqlParameter[pArreglo.GetUpperBound(0) + 1];
                for (int ctr = pArreglo.GetLowerBound(0); ctr <= pArreglo.GetUpperBound(0); ++ctr)
                {
                    sqlParams[ctr] = new SqlParameter(pArreglo[ctr, 0].ToString(), TipoDato(pArreglo[ctr, 1]));
                    sqlParams[ctr].Value = pArreglo[ctr, 2];
                }
                //'Cuando se pasan parametros
                ds.Merge(SqlHelper.ExecuteDataset(tx, CommandType.StoredProcedure, pNombreStored, sqlParams));
            }
            else
            {
                //'Para storeds que no necesitan pase de parametros
                ds.Merge(SqlHelper.ExecuteDataset(tx, CommandType.StoredProcedure, pNombreStored));
            }
            ds.AcceptChanges();
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos:" + e.Message);
        }
        return ds;
    }
    public Boolean funupdateclavepresupuestal()
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            //string[,] array = new string[,] { { "@Clave", "3", pIDClave }, { "@Nombre", "12", pnombre }, { "@TipoGestor", "3", pTipoGestor }, { "@Activo", "2", pActivo }, { "@Usuario", "12", pUsuario }, { "@Password", "12", pPassword }, { "@IDcorporativo", "8", pIDcorporativo } };
            //funUpdateDelete(tx, "solac.Update_Gestoress", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funEliminaradecuacion(string numerosolicitud)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@numerosolicitud", "12", numerosolicitud.ToString().Trim() } };
            funUpdateDelete(tx, "Delete_PRE_Adecuacion", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funActualizaradecuacionobra(DataTable datosmasivo, string lblclavepresupuestalobras, string txtoficionumero, string txtnumsolicitud)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        DataTable datostablaobras = new DataTable();
        try
        {
            DateTime fecha = DateTime.Now;
            int anio = fecha.Year;
            //****************************************************************************************************************************************************************************************
            //*********************Consultamos en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra el desgloce de acciones guardadas con el num. de clave a modificar***************************
            //****************************************************************************************************************************************************************************************
            DataSet obras;
            int banderainsertar = 0;
            string[,] pArreglor = new string[,] { { "@cve", "12", lblclavepresupuestalobras }, { "@oficionumero", "12", txtoficionumero }, { "@numsolicitud", "12", txtnumsolicitud } };
            obras = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_obrasbycvepresupuestaldesgloceobra", pArreglor);
            //if (obras.Tables.Count > 0)
            //{
            //    datostablaobras = obras.Tables[0];

            //}
            datostablaobras = obras.Tables[0];
            if (datostablaobras.Rows.Count > 0)
            {
                banderainsertar = 1;
            }
            else
            {
                banderainsertar = 0;
            }

            //****************************************************************************************************************************************************************************************
            //*********************Consultamos en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra el desgloce de acciones guardadas con el num. de clave a modificar***************************
            //****************************************************************************************************************************************************************************************
            foreach (DataRow row in datosmasivo.Rows)
            {
                String IDNew = "0";//0
                int insertamosregistro = 0;
                if (banderainsertar == 0)
                {
                    //insertamos nuevo registro en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra
                    string[,] array = new string[,] { { "@idc", "12", row["idc"].ToString() }, { "@idaccion", "12", row["idaccion"].ToString() }, { "@accion", "12", row["accion"].ToString() }, { "@idproyecto", "12", row["idproyecto"].ToString() }, { "@nombreproyecto", "12", row["nombreproyecto"].ToString() }, { "@proyecto", "12", row["proyecto"].ToString() }, { "@anio", "12", row["anio"].ToString() }, { "@municipio", "12", row["municipio"].ToString() }, { "@contrato", "12", row["contrato"].ToString() }, { "@cve", "12", row["cve"].ToString() }, { "@numeromes", "8", row["numeromes"].ToString() }, { "@nombremes", "12", row["nombremes"].ToString() }, { "@desgloceimportexmesobra", "5", row["desgloceimportexmesobra"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@RM", "12", row["RM"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() } };
                    funInsert(tx, "Insert_PRE_desglocenuevaclavepresupuestariaobra", array);
                    IDNew = IDNewx.ToString();
                }
                else
                {
                    for (int i = 0; i <= datostablaobras.Rows.Count - 1; i++)
                    {
                        if ((row["idc"].ToString().Trim()==datostablaobras.Rows[i]["idc"].ToString().Trim()) && ((row["cve"].ToString().Trim() == datostablaobras.Rows[i]["cve"].ToString().Trim())) && ((row["numeromes"].ToString().Trim() == datostablaobras.Rows[i]["numeromes"].ToString().Trim())) && ((row["oficionumero"].ToString().Trim() == datostablaobras.Rows[i]["oficionumero"].ToString().Trim())))
                        {
                            insertamosregistro = 1;
                        }
                    }
                    if (insertamosregistro == 0)
                    {
                        //insertamos nuevo registro en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra
                        string[,] array = new string[,] { { "@idc", "12", row["idc"].ToString() }, { "@idaccion", "12", row["idaccion"].ToString() }, { "@accion", "12", row["accion"].ToString() }, { "@idproyecto", "12", row["idproyecto"].ToString() }, { "@nombreproyecto", "12", row["nombreproyecto"].ToString() }, { "@proyecto", "12", row["proyecto"].ToString() }, { "@anio", "12", row["anio"].ToString() }, { "@municipio", "12", row["municipio"].ToString() }, { "@contrato", "12", row["contrato"].ToString() }, { "@cve", "12", row["cve"].ToString() }, { "@numeromes", "8", row["numeromes"].ToString() }, { "@nombremes", "12", row["nombremes"].ToString() }, { "@desgloceimportexmesobra", "5", row["desgloceimportexmesobra"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() }, { "@CA", "12", row["CA"].ToString() }, { "@UR", "12", row["UR"].ToString() }, { "@FI", "12", row["FI"].ToString() }, { "@FU", "12", row["FU"].ToString() }, { "@SF", "12", row["SF"].ToString() }, { "@SSF", "12", row["SSF"].ToString() }, { "@PS", "12", row["PS"].ToString() }, { "@PP", "12", row["PP"].ToString() }, { "@OA", "12", row["OA"].ToString() }, { "@AI", "12", row["AI"].ToString() }, { "@PT", "12", row["PT"].ToString() }, { "@MPIO", "12", row["MPIO"].ToString() }, { "@COGC", "12", row["COGC"].ToString() }, { "@OG", "12", row["OG"].ToString() }, { "@TG", "12", row["TG"].ToString() }, { "@TR", "12", row["TR"].ToString() }, { "@FF", "12", row["FF"].ToString() }, { "@SFF", "12", row["SFF"].ToString() }, { "@RM", "12", row["RM"].ToString() }, { "@PF", "12", row["PF"].ToString() }, { "@CP", "12", row["CP"].ToString() }, { "@DM", "12", row["DM"].ToString() }, { "@T_O", "12", row["T_O"].ToString() }, { "@fechaingreso", "4", row["fechaingreso"].ToString() } };
                        funInsert(tx, "Insert_PRE_desglocenuevaclavepresupuestariaobra", array);
                        IDNew = IDNewx.ToString();
                    }
                    else
                    {
                        //actualizar registro en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra
                        string[,] array = new string[,] { { "@idc", "12", row["idc"].ToString() }, { "@idaccion", "12", row["idaccion"].ToString() }, { "@accion", "12", row["accion"].ToString() }, { "@idproyecto", "12", row["idproyecto"].ToString() }, { "@nombreproyecto", "12", row["nombreproyecto"].ToString() }, { "@proyecto", "12", row["proyecto"].ToString() }, { "@anio", "12", row["anio"].ToString() }, { "@municipio", "12", row["municipio"].ToString() }, { "@contrato", "12", row["contrato"].ToString() }, { "@cve", "12", row["cve"].ToString() }, { "@numeromes", "8", row["numeromes"].ToString() }, { "@nombremes", "12", row["nombremes"].ToString() }, { "@desgloceimportexmesobra", "5", row["desgloceimportexmesobra"].ToString() }, { "@idtipoadecuacion", "8", row["idtipoadecuacion"].ToString() }, { "@tipoadecuacion", "12", row["tipoadecuacion"].ToString() }, { "@numsolicitud", "12", row["numsolicitud"].ToString() }, { "@oficionumero", "12", row["oficionumero"].ToString() } };
                        funUpdateDelete(tx, "Update_PRE_desglocenuevaclavepresupuestariaobra", array);
                        //actualizar registro en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra
                    }
                }
                //insertamos en la tabla tblp_PRE_adecuacionpresupuestariadesgloceobra si es nueva clave o clave modificada
            }
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funverificarclave(string[,] pArreglo, string cve,string t_o)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            DataSet dsS = new DataSet();
            DataSet dsI = new DataSet();
            if (t_o != "5")
            {
                for (int x = 0; x < 12; x++)
                {
                    if (!String.IsNullOrEmpty(pArreglo[x, 0]))
                    {
                        string[,] arraymeses = new string[,] { { "@mesdel", "8", pArreglo[x, 1] }, { "@mesal", "8", pArreglo[x, 2] } };
                        DataView dv;
                        dsS = funGetbyStoresPropiomesadecuacion(tx, "sp_desglocemesnumeroletraadecuacion", arraymeses);
                        if (dsS.Tables.Count > 0)
                        {
                            if (dsS.Tables[0].Rows.Count > 0)
                            {
                                dv = new DataView(dsS.Tables[0]);
                               // decimal sumamontosolicitado = 0;
                                foreach (DataRowView dr in dv)//recorremos los meses de un registro y verificamos uno por uno si tiene registro
                                {

                                    decimal aprobado = 0;
                                    decimal modificado = 0;
                                    decimal aumento = 0;
                                    decimal disminucion = 0;
                                    decimal montosolicitado = 0;
                                    string[,] arrayclave = new string[,] { { "@cve", "12", cve.ToString() }, { "@mesnumero", "8", dr["mesnumero"].ToString() } };
                                    dsI = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavesitienerecurso", arrayclave);
                                    if (dsI.Tables[0].Rows.Count > 0)
                                    {
                                        //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                                        aprobado = Convert.ToDecimal(dsI.Tables[0].Rows[0]["montoaprobado"].ToString());
                                        aumento = Convert.ToDecimal(dsI.Tables[0].Rows[0]["aumento"].ToString());
                                        disminucion = Convert.ToDecimal(dsI.Tables[0].Rows[0]["disminucion"].ToString());
                                        modificado = (aprobado + aumento) - disminucion;
                                        montosolicitado = Convert.ToDecimal(pArreglo[x, 0]);
                                        
                                        //sumamontosolicitado = sumamontosolicitado + montosolicitado;
                                        if (montosolicitado > modificado) //if (liberado > liberadobase)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                            }
                        }
                        //adecuacion += Convert.ToDecimal(pArreglo[x, 3]);// monto total de la clave asignado
                    }
                }
            }
            else
            {
                decimal adecuacion = 0;
               if (t_o=="5")
                {
                    for (int x = 0; x < 12; x++)
                    {
                        if (!String.IsNullOrEmpty(pArreglo[x, 0]))
                        {
                            adecuacion += Convert.ToDecimal(pArreglo[x, 3]);// monto total de la clave asignado
                        }
                    }
                    if (adecuacion > 0)
                    {
                        decimal aprobado = 0;
                        decimal modificado = 0;
                        decimal aumento = 0;
                        decimal disminucion = 0;
                        string[,] arrayclave = new string[,] { { "@cve", "12", cve.ToString() }, { "@mesnumero", "8", "0" } };
                        dsI = funGetbyStoresPropiomesadecuacion(tx, "Get_All_PRE_verificacionclavesitienerecurso", arrayclave);
                        if (dsI.Tables[0].Rows.Count > 0)
                        {
                            //extraer el dato que indica cuanto recurso tiene la clave y compararla con lo que se esta reducciendo
                            aprobado = Convert.ToDecimal(dsI.Tables[0].Rows[0]["montoaprobado"].ToString());
                            aumento = Convert.ToDecimal(dsI.Tables[0].Rows[0]["aumento"].ToString());
                            disminucion = Convert.ToDecimal(dsI.Tables[0].Rows[0]["disminucion"].ToString());
                            modificado = (aprobado + aumento) - disminucion;
                            
                            if (adecuacion > modificado) //if (liberado > liberadobase)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //funUpdateDelete(tx, "Insert_PRE_Adecuaciontmp", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    #endregion
    #region MARCAR_DEPOSITO_MINISTRACIONES
    public Boolean funActualizaracveministracion( DataTable cves)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            foreach (DataRow row in cves.Rows)
            {
                string[,] array = new string[,] { { "@idministracion", "8", row["idministracion"].ToString() }, { "@numministracion", "12", row["numministracion"].ToString() }, { "@cve", "12", row["cve"].ToString() }, { "@asignarsaldocve", "5", row["asignarsaldocve"].ToString() }, { "@saldopendientecve", "5", row["saldopendientecve"].ToString() } };
                funUpdateDelete(tx, "Update_PRE_Ministracioncvesaldo", array);
            }
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    #endregion
    #region PAGOS_KGS

    public virtual void funeliminarPagos(string pidestimacion, string pfechaemision, string pidseguimiento, string pobservacionesaccion, string pidusuario, string pAccion, string pidestatusestimacion, string pestatusestimacion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] arrayse = new string[,] { { "@idseguimiento", "8", pidestimacion }, { "@fechaemision", "4", pfechaemision }, { "@idusuario", "8", pidusuario }, { "@idestimacion", "8", pidestimacion }, { "@idestatusestimacion", "8", pidestatusestimacion }, { "@estatusestimacion", "12", pestatusestimacion }, { "@Observaciones", "12", pobservacionesaccion }, { "@Accion", "12", pAccion } };
            funInsert(tx, "Insert_tble_seguimientoestimacion", arrayse);

            string[,] pArreglo = new string[,] { { "@idestimacion", "8", pidestimacion.ToString() } };
            funUpdateDelete(tx, "Delete_tble_estimacion", pArreglo);

            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertPagos(string pIdvalidacionpresupuestal, string pFolio, string pFecha, string pFechaAlta, string pidbenefeciario, string pbeneficiario, string pidconceptopago, string pconceptopago, string pContrato, string pNombreObra, string pPeriodo, string pclavepresupuestal, string pfuentefinanciamiento, string pfondo, string pministracion, string pImporteTotal, string pRetenciones, string pImporteLiquido, string pNota, string pMunicipio, string pLocalidad, string pFederal, string pPorFederal, string pEstatal, string pPorEstatal, string pMunicipal, string pPorMunicipal, string pReciboOficial, string pObraBeneficioSocial1, string  pApaortacionICIC, string pImpuestoSobreNomina2, string pInspeccionVigilancia5, string pSancion, string pRetencion, string pComprometido, string pDevengado, string pEjercido, string pPagado, string pidestatus, string pEstatus, string pPasivos, string pCtaBanco, string pTranfCheque, string pPorRegularizar, string pObservaciones, string pJDptoControlPresupuestal, string pJDptoControlPagos, string pTipoPago)
    {


        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Idvalidacionpresupuestal", "8", pIdvalidacionpresupuestal }, { "@Folio", "12", pFolio }, { "@Fecha", "4", pFecha }, { "@FechaAlta", "4", pFechaAlta }, { "@idbenefeciario", "8", pidbenefeciario }, { "@beneficiario", "12",  pbeneficiario }, { "@idconceptopago", "8", pidconceptopago }, { "@conceptopago", "12", pconceptopago }, { "@Contrato", "12", pContrato }, { "@NombreObra", "12", pNombreObra }, { "@Periodo", "12", pPeriodo }, { "@clavepresupuestal", "12", pclavepresupuestal }, { "@fuentefinanciamiento", "12", pfuentefinanciamiento }, { "@fondo", "12", pfondo }, { "@ministracion", "12", pministracion }, { "@ImporteTotal", "5", pImporteTotal}, { "@Retenciones", "5", pRetenciones }, { "@ImporteLiquido", "5", pImporteLiquido }, { "@Nota", "12", pNota }, { "@Municipio", "12", pMunicipio }, { "@Localidad", "12", pLocalidad }, { "@Federal", "5", pFederal }, { "@PorFederal", "5", pPorFederal }, { "@Estatal", "5", pEstatal }, { "@PorEstatal", "5", pPorEstatal }, { "@Municipal", "5", pMunicipal } , { "@PorMunicipal", "5", pPorMunicipal }, { "@ReciboOficial", "12", pReciboOficial }, { "@ObraBeneficioSocial1", "5", pObraBeneficioSocial1 }, { "@ImpuestoSobreNomina2", "5", pImpuestoSobreNomina2 }, { "@ApaortacionICIC", "5", pApaortacionICIC }, { "@InspeccionVigilancia5", "5", pInspeccionVigilancia5 }, { "@Sancion", "5", pSancion }, { "@Retencion", "5", pRetencion }, { "@Comprometido", "12", pComprometido }, { "@Devengado", "12", pDevengado }, { "@Ejercido", "12", pEjercido }, { "@Pagado", "12", pPagado }, { "@idestatus", "8", pidestatus }, { "@Estatus", "12", pEstatus } , { "@Pasivos", "12", pPasivos }, { "@CtaBanco", "12", pCtaBanco }, { "@TranfCheque", "12", pTranfCheque }, { "@PorRegularizar", "12", pPorRegularizar }, { "@Observaciones", "12", pObservaciones } , { "@JDptoControlPresupuestal", "12", pJDptoControlPresupuestal }, { "@JDptoControlPagos", "12", pJDptoControlPagos }, { "@TipoPago", "12", pTipoPago } };
            funInsert(tx, "Insert_tblp_PRE_Comprometido", array);
            

           tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatePagos(string pIdvalidacionpresupuestal, string pFolio, string pFecha, string pFechaAlta, string pidbenefeciario, string pbeneficiario, string pidconceptopago, string pconceptopago, string pContrato, string pNombreObra, string pPeriodo, string pclavepresupuestal, string pfuentefinanciamiento, string pfondo, string pministracion, string pImporteTotal, string pRetenciones, string pImporteLiquido, string pNota, string pMunicipio, string pLocalidad, string pFederal, string pPorFederal, string pEstatal, string pPorEstatal, string pMunicipal, string pPorMunicipal, string pReciboOficial, string pObraBeneficioSocial1, string pApaortacionICIC, string pImpuestoSobreNomina2, string pInspeccionVigilancia5, string pSancion, string pRetencion, string pComprometido, string pDevengado, string pEjercido, string pPagado, string pidestatus, string pEstatus, string pPasivos, string pCtaBanco, string pTranfCheque, string pPorRegularizar, string pObservaciones, string pJDptoControlPresupuestal, string pJDptoControlPagos)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@Idvalidacionpresupuestal", "8", pIdvalidacionpresupuestal }, { "@Folio", "12", pFolio }, { "@Fecha", "4", pFecha }, { "@FechaAlta", "4", pFechaAlta }, { "@idbenefeciario", "8", pidbenefeciario }, { "@beneficiario", "12", pbeneficiario }, { "@idconceptopago", "8", pidconceptopago }, { "@conceptopago", "12", pconceptopago }, { "@Contrato", "12", pContrato }, { "@NombreObra", "12", pNombreObra }, { "@Periodo", "12", pPeriodo }, { "@clavepresupuestal", "12", pclavepresupuestal }, { "@fuentefinanciamiento", "12", pfuentefinanciamiento }, { "@fondo", "12", pfondo }, { "@ministracion", "12", pministracion }, { "@ImporteTotal", "5", pImporteTotal }, { "@Retenciones", "5", pRetenciones }, { "@ImporteLiquido", "5", pImporteLiquido }, { "@Nota", "12", pNota }, { "@Municipio", "12", pMunicipio }, { "@Localidad", "12", pLocalidad }, { "@Federal", "5", pFederal }, { "@PorFederal", "5", pPorFederal }, { "@Estatal", "5", pEstatal }, { "@PorEstatal", "5", pPorEstatal }, { "@Municipal", "5", pMunicipal }, { "@PorMunicipal", "5", pPorMunicipal }, { "@ReciboOficial", "12", pReciboOficial }, { "@ObraBeneficioSocial1", "5", pObraBeneficioSocial1 }, { "@ImpuestoSobreNomina2", "5", pImpuestoSobreNomina2 }, { "@ApaortacionICIC", "5", pApaortacionICIC }, { "@InspeccionVigilancia5", "5", pInspeccionVigilancia5 }, { "@Sancion", "5", pSancion }, { "@Retencion", "5", pRetencion }, { "@Comprometido", "12", pComprometido }, { "@Devengado", "12", pDevengado }, { "@Ejercido", "12", pEjercido }, { "@Pagado", "12", pPagado }, { "@idestatus", "8", pidestatus }, { "@Estatus", "12", pEstatus }, { "@Pasivos", "12", pPasivos }, { "@CtaBanco", "12", pCtaBanco }, { "@TranfCheque", "12", pTranfCheque }, { "@PorRegularizar", "12", pPorRegularizar }, { "@Observaciones", "12", pObservaciones }, { "@JDptoControlPresupuestal", "12", pJDptoControlPresupuestal }, { "@JDptoControlPagos", "12", pJDptoControlPagos }};
            funUpdateDelete(tx, "Update_tblp_PRE_Comprometido", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    #endregion
    #region DOCUMENTOS PAGOS_KGS

    public virtual void funGuardardocumentosPagos(GridView grvdocts,  string idvalidacionpesupuestal)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 iddocumentocomprometido = 0;
        Int64 iddocumentopago = 0;
        string documento = "";

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    CheckBox chkRowOriginal = (row.FindControl("chkoriginal") as CheckBox);
                    CheckBox chkRowCopia = (row.FindControl("chkcopia") as CheckBox);

                    if (chkRowOriginal.Checked == true || chkRowCopia.Checked == true)
                    {
                        TextBox txtRow = (row.FindControl("txthojas") as TextBox);
                        TextBox txtRowdocto = (row.FindControl("txtdato") as TextBox);
                        iddocumentocomprometido = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);
                        iddocumentopago = Convert.ToInt64(grvdocts.DataKeys[i].Values[1]);
                        documento = Convert.ToString(grvdocts.DataKeys[i].Values[2]);

                        if (txtRow.Text != "")
                        {

                            if (Convert.ToInt64(txtRow.Text) > 0)
                            {
                                funeliminardocumentosPagos(tx, iddocumentocomprometido.ToString());
                                
                                if (funInsertdocumentosPagos(tx, "0", idvalidacionpesupuestal, iddocumentopago.ToString(),documento, txtRowdocto.Text, chkRowOriginal.Checked.ToString(), chkRowCopia.Checked.ToString(), txtRow.Text) == true)
                                {

                                }
                                else
                                {
                                    throw new System.InvalidOperationException("Error de datos");

                                }
                            }
                            else
                            {
                                throw new System.InvalidOperationException("Valor invalido" + documento);
                            }
                        }
                        else
                        {
                            throw new System.InvalidOperationException("Valor invalido" + documento);

                        }
                    }
                    else
                    {
                        funeliminardocumentosPagos(tx, iddocumentocomprometido.ToString());

                    }
                }
                i += 1;

            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    public Boolean funeliminardocumentosPagos(SqlTransaction tx, string iddocumentocomprometido)
    {
        try
        {
            string[,] pArreglo = new string[,] { { "@iddocumentocomprometido", "8", iddocumentocomprometido.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_PRE_ComprometidoDocumentos", pArreglo);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }



    }


    public Boolean funInsertdocumentosPagos(SqlTransaction tx, string iddocumentoestimacion, string Idvalidacionpresupuestal, string iddocumentopago, string documento, string NumDocumento , string Original, string Copia, string Fojas)
    {

        try
        {
    
            string[,] array = new string[,] { { "@iddocumentoestimacion", "8", iddocumentoestimacion }, { "@Idvalidacionpresupuestal", "8", Idvalidacionpresupuestal }, { "@iddocumentopago", "8", @iddocumentopago }, { "@documento", "12", documento }, { "@NumDocumento", "12", NumDocumento }, { "@Original", "12", Original }, { "@Copia", "12", Copia }, { "@Fojas", "8", Fojas }  };
            funInsert(tx, "Insert_tblp_PRE_ComprometidoDocumentos", array);

            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }



    }


    #endregion
    #region ProyectoAccion_KGS
    public virtual void funeliminarproyectos(string pidgeneral, string Folio, string Idproyecto, string IdAccion, string idanio, string anio, string idpoa, string idrubro, string rubro, string idsubrubro, string subrubro, string nombreproyecto, string accion, string idtipoaccion, string tipoaccion, string idmunicipio, string cve_mpio, string municipio, string idlocalidad, string cve_localidad, string localidad, string longuitud, string latitud, string idprograma, string programa, string idestatus, string estatus, string mujer, string hombre, string beneficiarios, string idtiposolicitud, string tiposolicitud, string oficiovalidacion, string fechaoficiovalidacion, string oficioautorizacion, string fechaoficioautorizacion, string IDPED, string eje, string tema, string politicapublica, string objetivo, string estrategia, string idtipoobra, string tipo_obra, string idhijo, string idobraproyectopeticion, string idpeticion, string idanteproyecto, string avancefisico, string avancefinanciero, string iddireccionresponsable, string direccionresponsable, string iddepartamento, string departamento, string compromisogobierno, string idhgeneral, string fechamovimiento, string idusuario, string Maccion, string pmetaprogramada, string pmetaalcanzada,  string Idtipobeneficiario, string tipobeneficiario,/*beneficiarios*/string beneficiariohombresaccion, string beneficiariomujeresaccion, string totalbeneficiariosaccion, string idtiporecurso, string tiporecurso, string iddependencia, string dependencia, string obraconvenida)
    {
        cnn.Open();
        SqlTransaction tx;
        
        tx = cnn.BeginTransaction();
        try
        {


            string[,] arrayH = new string[,] {{ "@idhgeneral", "8", "0" },
                                              { "@idGeneral", "8", pidgeneral },
                                              { "@folio", "12",Folio},
                                              { "@idaccion", "12", IdAccion},
                                              { "@idproyecto", "12",Idproyecto },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idpoa", "12", idpoa },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idstatus", "8", idestatus },
                                              { "@estatus", "12", estatus },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@idped", "8", IDPED },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idhijo", "8", idhijo },
                                              { "@idobraproyectopeticion", "8", idobraproyectopeticion },
                                              { "@idpeticion", "8", idpeticion },
                                              { "@idanteproyecto", "8", idanteproyecto },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@fechamovimiento", "4", fechamovimiento },
                                              { "@idusuario", "8", idusuario },
                                              { "@Maccion", "12", Maccion },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida }
                

                   };

            funInsert(tx, "Insert_tblp_SP_Hgeneral", arrayH);




            string[,] pArreglo = new string[,] { { "@idgeneral", "8", pidgeneral.ToString() } };
            funUpdateDelete(tx, "Delete_SP_general_byidhijo", pArreglo);

            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funinsertarproyectoobra(string IDGeneral, string Folio, string Idproyecto, string IdAccion, string idanio, string anio, string idpoa, string idrubro, string rubro, string idsubrubro, string subrubro, string nombreproyecto, string accion, string idtipoaccion, string tipoaccion, string idmunicipio, string cve_mpio, string municipio, string idlocalidad, string cve_localidad, string localidad, string longuitud, string latitud, string idprograma, string programa, string idestatus, string estatus, string mujer, string hombre, string beneficiarios, string idtiposolicitud, string tiposolicitud, string oficiovalidacion, string fechaoficiovalidacion, string oficioautorizacion, string fechaoficioautorizacion, string IDPED, string eje, string tema, string politicapublica, string objetivo, string estrategia, string idtipoobra, string tipo_obra, string idhijo, string idobraproyectopeticion, string idpeticion, string idanteproyecto, string avancefisico, string avancefinanciero, string iddireccionresponsable, string direccionresponsable, string iddepartamento, string departamento, string compromisogobierno, GridView grvarch, GridView grvmetasm, string  idhgeneral, string  fechamovimiento, string idusuario, string Maccion, string  pmetaprogramada, string pmetaalcanzada, string nocontrolpeticionesTM, string fecharecepcionTM, string numoficiopeticionTM, string nocontrolobraTM, string nombreobraTM, string estatusobraTM, string idobraTM, string NombreproyectoTM, string ClavePresupuestalTM, string TotalesTM, string descripcioncontratoTM, string fechacontratoTM, string fechainiciocontratoTM, string fechaterminocontratoTM, string empresaTM, string numlicitacionTM,string idadjudicacionesTM,string idconvocatoriaTM,string descripcionTM,string nolicitacionTM, string fechapublicacionTM, string Idtipobeneficiario, string tipobeneficiario,/*beneficiarios*/string beneficiariohombresaccion,string beneficiariomujeresaccion,string totalbeneficiariosaccion, string idtiporecurso, string tiporecurso, string iddependencia, string dependencia, string obraconvenida, string letrero)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] {{ "@IDGeneral", "8", IDGeneral },
                                              { "@folio", "12",Folio},
                                              { "@idaccion", "12", IdAccion},
                                              { "@idproyecto", "12",Idproyecto },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idpoa", "12", idpoa },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idstatus", "8", idestatus },
                                              { "@estatus", "12", estatus },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@idped", "8", IDPED },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idhijo", "8", idhijo },
                                              { "@idobraproyectopeticion", "8", idobraproyectopeticion },
                                              { "@idpeticion", "8", idpeticion },
                                              { "@idanteproyecto", "8", idanteproyecto },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }
         

                   };
            funInsert(tx, "Insert_tblp_SP_general", array);
            idobraccion = IDNewx.ToString();
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayH = new string[,] {{ "@idhgeneral", "8", idhgeneral },
                                              { "@idGeneral", "8", idgeneralNew },
                                              { "@folio", "12",Folio},
                                              { "@idaccion", "12", IdAccion},
                                              { "@idproyecto", "12",Idproyecto },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idpoa", "12", idpoa },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idstatus", "8", idestatus },
                                              { "@estatus", "12", estatus },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@idped", "8", IDPED },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idhijo", "8", idhijo },
                                              { "@idobraproyectopeticion", "8", idobraproyectopeticion },
                                              { "@idpeticion", "8", idpeticion },
                                              { "@idanteproyecto", "8", idanteproyecto },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@fechamovimiento", "4", fechamovimiento },
                                              { "@idusuario", "8", idusuario },
                                              { "@Maccion", "12", Maccion },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "2", obraconvenida }

                   };

            funInsert(tx, "Insert_tblp_SP_Hgeneral", arrayH);

            //si viene de una peticion se actualiza el valor en la tabla

            if (Convert.ToInt64(idobraproyectopeticion) > 0)
            {
                string[,] arrayp = new string[,] {{ "@idobra", "8", idobraproyectopeticion },
                                              { "@idgeneral", "8", IDNewx.ToString()  }
                                             };
                funUpdateDelete(tx, "Update_tblp_PET_obraproyectoidgeneral", arrayp);
              

                //para generar el time line peticiones
                       string[,] arraytm = new string[,] {{ "@idtimeline", "8", "0" }, { "@idobra", "8", idobraproyectopeticion }, { "@idpeticion", "8", idpeticion }, { "@nocontrolpeticiones", "12", nocontrolpeticionesTM }, { "@fecharecepcion", "4", fecharecepcionTM }, { "@numerooficio", "12", numoficiopeticionTM }, { "@nocontrolobra", "12", nocontrolobraTM }, { "@idanteproyecto", "8", idanteproyecto }, { "@descripcion_obra", "12", nombreobraTM }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", estatusobraTM }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneralNew }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "PET" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                funInsert(tx, "Insert_Timelineobrapublica", arraytm);


                //para generar el time line 




            }

            //si viene de un anteproyecto se actualiza el valor en la tabla

            if (Convert.ToInt64(idanteproyecto) > 0)
            {
                //string[,] arrayp = new string[,] {{ "@IDAnteproyecto", "8", idanteproyecto },
                //                              { "@idgeneral", "8", idgeneralNew.ToString()  }
                //                             };
                //funUpdateDelete(tx, "Update_tbl_Anteproyectoidgeneral", arrayp);

                string[,] arrayp = new string[,] {{ "@IDAnteproyecto", "8", idanteproyecto },
                                              { "@idgeneral", "8", idgeneralNew.ToString()  }
                                             };
                funUpdateDelete(tx, "Update_tbl_Anteproyectoidgeneral", arrayp);


                //para generar el time line anteproyecto



                //checa si el anteproyecto viene de una peticion para generar el registro y depsues generear el registro de anteproyecto
                if ( Convert.ToInt64(idobraTM)> 0)
                {

                    

                    //generar el time line de peticiones

                    string[,] arraytm = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", idobraproyectopeticion }, { "@idpeticion", "8", idpeticion }, { "@nocontrolpeticiones", "12", nocontrolpeticionesTM }, { "@fecharecepcion", "4",  fecharecepcionTM }, { "@numerooficio", "12", numoficiopeticionTM  }, { "@nocontrolobra", "12", nocontrolobraTM  }, { "@idanteproyecto", "8", idanteproyecto }, { "@descripcion_obra", "12", nombreobraTM }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", estatusobraTM }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneralNew }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "PET" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                    funInsert(tx, "Insert_Timelineobrapublica", arraytm);
                }

               
                string[,] arraya = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12","" }, { "@idanteproyecto", "8", idanteproyecto }, { "@descripcion_obra", "12", NombreproyectoTM }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", ClavePresupuestalTM }, { "@totales", "5", TotalesTM }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12",""}, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneralNew }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "ANP" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                funInsert(tx, "Insert_Timelineobrapublica", arraya);


                //para generar el time line de anteproyecto
            }


            //para generar el time line de Obra/Accion

            string[,] arrayo = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", nombreproyecto  }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", Folio  }, { "@idaccion", "12", IdAccion  }, { "@idproyecto", "12", Idproyecto }, { "@idpoa", "12", idpoa  }, { "@rubro", "12", rubro  }, { "@subrubro", "12", subrubro  }, { "@accion", "12",  accion  }, { "@municipio", "12", municipio }, { "@localidad", "12", localidad  }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", "" }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneralNew }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "OBRA" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
            funInsert(tx, "Insert_Timelineobrapublica", arrayo);

            //para generar el time line de Obra/Accion


            string[,] pArreglo = new string[,] { { "@idgeneral", "8", idgeneralNew.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_SP_generaloficios", pArreglo);
                        //crea oficios
            Int64 idoficioobra =0;
            Int64 idgeneral = 0;
            Int64 idtipooficio = 0;
            string nombredocumento = "";
            String Observaciones = "";

            DateTime FechaOficio = Convert.ToDateTime ( "01/01/1900");
            string Archivo = "";


            //insertaroficios
            Int16 i = 0;
            foreach (GridViewRow row in grvarch.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {


                    idoficioobra = Convert.ToInt64("0");
                    idgeneral = Convert.ToInt64(idgeneralNew);
                    idtipooficio = Convert.ToInt64(grvarch.DataKeys[i].Values[2]);
                    nombredocumento = Convert.ToString(grvarch.DataKeys[i].Values[3]);
                    FechaOficio = Convert.ToDateTime(grvarch.DataKeys[i].Values[4]);
                    Archivo = Convert.ToString(grvarch.DataKeys[i].Values[5]);
                    Observaciones =  Convert.ToString(grvarch.DataKeys[i].Values[6]);


                    funInsertOficios(tx, idoficioobra.ToString(), idgeneral.ToString (),  idtipooficio.ToString(), nombredocumento.ToString(), FechaOficio.ToString(), Archivo.ToString(), "0", fechamovimiento, idusuario, "A", Observaciones);
                    String idoficioobraNEW = IDNewx.ToString();
                    //para generar el time line de oficio de autorización

                    //tipo ofocio de autorizacio //21/02/2021
                    //modificao a orifcio de validación 21/02/2021
                    if (idtipooficio ==2)
                    {
                        string[,] arrayv = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "" }, { "@idproyecto", "12", "" }, { "@idpoa", "12", "" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", idoficioobraNEW }, { "@idtipooficio", "8", idtipooficio.ToString()  }, { "@nombredocumento", "12", nombredocumento }, { "@archivogestion", "12", Archivo }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", ""}, { "@descripcion_convocatoria", "12","" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", "" }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneralNew }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "GES" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                        funInsert(tx, "Insert_Timelineobrapublica", arrayv);

                    }

                    //para generar el time line de oficio de autorización


                }
                i += 1;

            }
            //insertarmetas

            string[,] pArreglom = new string[,] { { "@idgeneral", "8", IDGeneral } };
            funUpdateDelete(tx, "Delete_tblp_SP_generalmetas", pArreglom);
            Int64 idmetaobra = 0;
            Int64 idgeneralm = 0;
            Int64 idunidad = 0;
            String unidad ="";
            String cantidad = "0";
            Decimal metaprogramada = 0;
            Decimal metaalcanzada = 0;

            String descripcion = "";
                       
            Int16 im = 0;
            foreach (GridViewRow row in grvmetasm.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {


                    idmetaobra = Convert.ToInt64("0");
                    idgeneralm = Convert.ToInt64(idgeneralNew);
                    idunidad = Convert.ToInt64(grvmetasm.DataKeys[im].Values[2]);
                    unidad = Convert.ToString(grvmetasm.DataKeys[im].Values[3]);                    
                    descripcion = Convert.ToString(grvmetasm.DataKeys[im].Values[4]);
                    cantidad = Convert.ToString(grvmetasm.DataKeys[im].Values[5]);
                    funInsertmetas(tx, idmetaobra.ToString(), idgeneralm.ToString(), idunidad.ToString(), unidad.ToString(), metaprogramada.ToString(), metaalcanzada.ToString(), descripcion.ToString(), "0", fechamovimiento, idusuario, "A", cantidad);

                }
                im += 1;

            }


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdateproyectoobra(string IDGeneral, string idrubro, string rubro, string idsubrubro, string subrubro, string nombreproyecto, string idprograma, string programa,  string mujer, string hombre, string beneficiarios, string eje, string tema, string politicapublica, string objetivo, string estrategia, string idanio, string anio, string idmunicipio, string cve_mpio, string municipio, string idlocalidad, string cve_localidad, string localidad, string longuitud, string latitud, string idhgeneral, string fechamovimiento, string idusuario, string Maccion, string pmetaprogramada, string pmetaalcanzada, string Idtipobeneficiario, string tipobeneficiario,string idtiporecurso, string tiporecurso, string iddependencia, string dependencia, string obraconvenida, string letrero)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] {{ "@IDGeneral", "8", IDGeneral },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idmunicipio", "8", idmunicipio },
                                              //{ "@idlocalidad", "8", idlocalidad },
                                              //{ "@cve_localidad", "12", cve_localidad },
                                              //{ "@localidad", "12", localidad },
                                              //{ "@longuitud", "5", longuitud },
                                              //{ "@latitud", "5", latitud },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso   },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }

        };
            funUpdateDelete(tx, "Update_tblp_SP_general", array);

            string[,] arrayH = new string[,] {{ "@idhgeneral", "8", idhgeneral },
                                              { "@IDGeneral", "8", IDGeneral },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@fechamovimiento", "4", fechamovimiento },
                                              { "@idusuario", "8", idusuario },
                                              { "@Maccion", "12", Maccion },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada }  ,
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso } ,
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }
                                             };

            funInsert(tx, "Insert_tblp_SP_Hgeneralup", arrayH);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funinsertaraccion(string IDGeneral, string Folio, string Idproyecto, string IdAccion, string idanio, string anio, string idpoa, string idrubro, string rubro, string idsubrubro, string subrubro, string nombreproyecto, string accion, string idtipoaccion, string tipoaccion, string idmunicipio, string cve_mpio, string municipio, string idlocalidad, string cve_localidad, string localidad, string longuitud, string latitud, string idprograma, string programa, string idestatus, string estatus, string mujer, string hombre, string beneficiarios, string idtiposolicitud, string tiposolicitud, string oficiovalidacion, string fechaoficiovalidacion, string oficioautorizacion, string fechaoficioautorizacion, string IDPED, string eje, string tema, string politicapublica, string objetivo, string estrategia, string idtipoobra, string tipo_obra, string idhijo, string idobraproyectopeticion, string idpeticion, string idanteproyecto,  string  avancefisico, string avancefinanciero, string iddireccionresponsable, string direccionresponsable, string iddepartamento, string departamento, string compromisogobierno, GridView grvarch, GridView grvmetasm, string idhgeneral, string fechamovimiento, string idusuario, string Maccion, string pmetaprogramada, string pmetaalcanzada, string descripcioncontratoTM, string fechacontratoTM, string fechainiciocontratoTM, string fechaterminocontratoTM, string empresaTM, string numlicitacionTM, string idadjudicacionesTM, string idconvocatoriaTM, string descripcionTM, string nolicitacionTM, string fechapublicacionTM, string Idtipobeneficiario, string tipobeneficiario,/*beneficiarios*/string beneficiariohombresaccion, string beneficiariomujeresaccion, string totalbeneficiariosaccion, string idtiporecurso, string tiporecurso, string iddependencia, string dependencia, string obraconvenida, string letrero)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] {{ "@IDGeneral", "8", IDGeneral },
                                              { "@folio", "12", Folio },
                                              { "@idaccion", "12", IdAccion },
                                              { "@idproyecto", "12", Idproyecto },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idpoa", "12", idpoa },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idstatus", "8", idestatus },
                                              { "@estatus", "12", estatus },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer },
                                              { "@hombre", "8", hombre  },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@idped", "8", IDPED },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idhijo", "8", idhijo },
                                              { "@idobraproyectopeticion", "8", idobraproyectopeticion },
                                              { "@idpeticion", "8", idpeticion },
                                              { "@idanteproyecto", "8", idanteproyecto },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }
                                    };
            funInsert(tx, "Insert_tblp_SP_generalAccion", array);
            idobraccion = IDNewx.ToString();


            string[,] arrayH = new string[,] {{ "@idhgeneral", "8", idhgeneral },

                                              { "@idGeneral", "8", idobraccion },
                                              { "@folio", "12",Folio},
                                              { "@idaccion", "12", IdAccion},
                                              { "@idproyecto", "12",Idproyecto },
                                              { "@idanio", "8", idanio },
                                              { "@anio", "8", anio },
                                              { "@idpoa", "12", idpoa },
                                              { "@idrubro", "8", idrubro },
                                              { "@rubro", "12", rubro },
                                              { "@idsubrubro", "8", idsubrubro },
                                              { "@subrubro", "12", subrubro },
                                              { "@nombreproyecto", "12", nombreproyecto },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idmunicipio", "8", idmunicipio },
                                              { "@cve_mpio", "12", cve_mpio },
                                              { "@municipio", "12", municipio },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idprograma", "8", idprograma },
                                              { "@programa", "12", programa },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idstatus", "8", idestatus },
                                              { "@estatus", "12", estatus },
                                              { "@beneficiarios", "8", beneficiarios },
                                              { "@mujer", "8", mujer  },
                                              { "@hombre", "8", hombre },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@idped", "8", IDPED },
                                              { "@eje", "12", eje },
                                              { "@tema", "12", tema },
                                              { "@politicapublica", "12", politicapublica },
                                              { "@objetivo", "12", objetivo },
                                              { "@estrategia", "12", estrategia },
                                              { "@idhijo", "8", idhijo },
                                              { "@idobraproyectopeticion", "8", idobraproyectopeticion },
                                              { "@idpeticion", "8", idpeticion },
                                              { "@idanteproyecto", "8", idanteproyecto },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@fechamovimiento", "4", fechamovimiento },
                                              { "@idusuario", "8", idusuario },
                                              { "@Maccion", "12", Maccion },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@Idtipobeneficiario", "8", Idtipobeneficiario },
                                              { "@tipobeneficiario", "12", tipobeneficiario },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }

                   };

            funInsert(tx, "Insert_tblp_SP_Hgeneral", arrayH);



            string[,] pArreglo = new string[,] { { "@idgeneral", "8", idobraccion } };
            funUpdateDelete(tx, "Delete_tblp_SP_generaloficios", pArreglo);
            //crea oficios
            Int64 idoficioobra = 0;
            Int64 idgeneral = 0;
            Int64 idtipooficio = 0;
            string nombredocumento = "";
            string Observaciones = "";
            DateTime FechaOficio = Convert.ToDateTime("01/01/1900");
            string Archivo = "";



            Int16 i = 0;
            foreach (GridViewRow row in grvarch.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    idoficioobra = Convert.ToInt64("0");
                    idgeneral = Convert.ToInt64(grvarch.DataKeys[i].Values[0] );
                    idtipooficio = Convert.ToInt64(grvarch.DataKeys[i].Values[2]);
                    nombredocumento = Convert.ToString(grvarch.DataKeys[i].Values[3]);
                    FechaOficio = Convert.ToDateTime(grvarch.DataKeys[i].Values[4]);
                    Archivo = Convert.ToString(grvarch.DataKeys[i].Values[5]);
                    Observaciones = Convert.ToString(grvarch.DataKeys[i].Values[6]);

                    funInsertOficios(tx, idoficioobra.ToString(), idobraccion, idtipooficio.ToString(), nombredocumento.ToString(), FechaOficio.ToString(), Archivo.ToString(), "0", fechamovimiento, idusuario, "A", Observaciones);


                    String idoficioobraNEW = IDNewx.ToString();
                    //para generar el time line de oficio de autorización

                    //tipo ofocio de autorizaciom
                    if (idtipooficio == 2)
                    {
                        string[,] arrayv = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "" }, { "@idproyecto", "12", "" }, { "@idpoa", "12", "" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", idoficioobraNEW }, { "@idtipooficio", "8", idtipooficio.ToString() }, { "@nombredocumento", "12", nombredocumento }, { "@archivogestion", "12", Archivo }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", "" }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idobraccion }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "GES" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                        funInsert(tx, "Insert_Timelineobrapublica", arrayv);

                    }

                    //para generar el time line de oficio de autorización

                }
                i += 1;

            }

            //insertarmetas

            string[,] pArreglom = new string[,] { { "@idgeneral", "8", IDNewx.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_SP_generalmetas", pArreglom);
            Int64 idmetaobra = 0;
            Int64 idgeneralm = 0;
            Int64 idunidad = 0;
            String unidad = "";
            Decimal metaprogramada = 0;
            Decimal metaalcanzada = 0;
            String cantidad = "0";
            String descripcion = "";
            Int16 im = 0;
            foreach (GridViewRow row in grvmetasm.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {


                    idmetaobra = Convert.ToInt64("0");
                    idgeneralm = Convert.ToInt64(grvmetasm.DataKeys[im].Values[1]);
                    idunidad = Convert.ToInt64(grvmetasm.DataKeys[im].Values[2]);
                    unidad = Convert.ToString(grvmetasm.DataKeys[im].Values[3]);
                    
                    descripcion = Convert.ToString(grvmetasm.DataKeys[im].Values[4]);
                    cantidad = Convert.ToString(grvmetasm.DataKeys[im].Values[5]);

                    funInsertmetas(tx, idmetaobra.ToString(), idobraccion, idunidad.ToString(), unidad.ToString(), metaprogramada.ToString(), metaalcanzada.ToString(), descripcion.ToString(), "0", fechamovimiento, idusuario, "A", cantidad);

                }
                im += 1;

            }




            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdateaccion(string IDGeneral, string idpoa,  string accion, string idtipoaccion, string tipoaccion,   string idtiposolicitud, string tiposolicitud, string oficiovalidacion, string fechaoficiovalidacion, string oficioautorizacion, string fechaoficioautorizacion, string idtipoobra, string tipo_obra, string idmunicipio, string cve_mpio, string municipio, string idlocalidad, string cve_localidad, string localidad, string longuitud, string latitud,  string avancefisico, string avancefinanciero, string iddireccionresponsable, string direccionresponsable, string iddepartamento, string departamento, string compromisogobierno,  GridView grvarch, GridView grvmetasm, string idhgeneral, string fechamovimiento, string idusuario, string Maccion, string pmetaprogramada, string pmetaalcanzada, string descripcioncontratoTM, string fechacontratoTM, string fechainiciocontratoTM, string fechaterminocontratoTM, string empresaTM, string numlicitacionTM, string idadjudicacionesTM, string idconvocatoriaTM, string descripcionTM, string nolicitacionTM, string fechapublicacionTM,/*beneficiarios*/string beneficiariohombresaccion, string beneficiariomujeresaccion, string totalbeneficiariosaccion, string idtiporecurso, string tiporecurso, string iddependencia, string dependencia, string obraconvenida, string letrero)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] {{ "@IDGeneral", "8", IDGeneral },                                             
                                              { "@idpoa", "12", idpoa },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },                                                
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                              { "@Descripcionletrero", "12", letrero }
                                             };

            funUpdateDelete(tx, "Update_tblp_SP_generalaccion", array);

            string[,] arrayH = new string[,] {{ "@idhgeneral", "8", idhgeneral },
                                              { "@idGeneral", "8", IDGeneral },
                                              { "@idpoa", "12", idpoa },
                                              { "@accion", "12", accion },
                                              { "@idtipoaccion", "8", idtipoaccion },
                                              { "@tipoaccion", "12", tipoaccion },
                                              { "@idtipoobra", "8", idtipoobra },
                                              { "@tipo_obra", "12", tipo_obra },
                                              { "@idtiposolicitud", "8", idtiposolicitud },
                                              { "@tiposolicitud", "12", tiposolicitud },
                                              { "@avancefisico", "5", avancefisico },
                                              { "@avancefinanciero", "5", avancefinanciero },
                                              { "@iddireccionresponsable", "8", iddireccionresponsable },
                                              { "@direccionresponsable", "12", direccionresponsable },
                                              { "@iddepartamento", "8", iddepartamento },
                                              { "@departamento", "12", departamento },
                                              { "@compromisogobierno", "2", compromisogobierno },
                                              { "@fechamovimiento", "4", fechamovimiento },
                                              { "@idusuario", "8", idusuario },
                                              { "@Maccion", "12", Maccion },
                                              { "@metaprogramada", "5", pmetaprogramada },
                                              { "@metaalcanzada", "5", pmetaalcanzada },
                                              { "@beneficiariohombresaccion", "8", beneficiariohombresaccion },
                                              { "@beneficiariomujeresaccion", "8", beneficiariomujeresaccion },
                                              { "@totalbeneficiariosaccion", "8", totalbeneficiariosaccion },
                                              { "@idlocalidad", "8", idlocalidad },
                                              { "@cve_localidad", "12", cve_localidad },
                                              { "@localidad", "12", localidad },
                                              { "@longuitud", "5", longuitud },
                                              { "@latitud", "5", latitud },
                                              { "@idtiporecurso", "8", idtiporecurso },
                                              { "@tipo_recurso", "12", tiporecurso },
                                              { "@IDDependencia", "8", iddependencia },
                                              { "@Dependencia", "12", dependencia },
                                              { "@obraconvenida", "2", obraconvenida },
                                               { "@Descripcionletrero", "12", letrero }

                   };

            funInsert(tx, "Insert_tblp_SP_HgeneralAccionup", arrayH);
            
            string[,] pArreglo = new string[,] { { "@idgeneral", "8", IDGeneral } };
            funUpdateDelete(tx, "Delete_tblp_SP_generaloficios", pArreglo);
            //crea oficios
            Int64 idoficioobra = 0;
            Int64 idgeneral = 0;
            Int64 idtipooficio = 0;
            string nombredocumento = "";
            string Observaciones = "";

            DateTime FechaOficio = Convert.ToDateTime("01/01/1900");
            string Archivo = "";
            
            Int16 i = 0;
            foreach (GridViewRow row in grvarch.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    idoficioobra = Convert.ToInt64("0");
                    idgeneral = Convert.ToInt64(grvarch.DataKeys[i].Values[1]);
                    idtipooficio = Convert.ToInt64(grvarch.DataKeys[i].Values[2]);
                    nombredocumento = Convert.ToString(grvarch.DataKeys[i].Values[3]);
                    FechaOficio = Convert.ToDateTime(grvarch.DataKeys[i].Values[4]);
                    Archivo = Convert.ToString(grvarch.DataKeys[i].Values[5]);
                    Observaciones = Convert.ToString(grvarch.DataKeys[i].Values[6]);

                    funInsertOficios(tx, idoficioobra.ToString(), IDGeneral.ToString(), idtipooficio.ToString(), nombredocumento.ToString(), FechaOficio.ToString(), Archivo.ToString(), "0", fechamovimiento, idusuario, "A", Observaciones);
                    String idoficioobraNEW = IDNewx.ToString();
                    //para generar el time line de oficio de autorización

                    //tipo ofocio de autorizaciom
                    if (idtipooficio == 2)
                    {
                        string[,] arrayv = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", "" }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "" }, { "@idproyecto", "12", "" }, { "@idpoa", "12", "" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", idoficioobraNEW }, { "@idtipooficio", "8", idtipooficio.ToString() }, { "@nombredocumento", "12", nombredocumento }, { "@archivogestion", "12", Archivo }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", "" }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", IDGeneral }, { "@fecha_alta", "4", fechamovimiento }, { "@tipo_proceso", "12", "GES" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                        funInsert(tx, "Insert_Timelineobrapublica", arrayv);

                    }

                    //para generar el time line de oficio de autorización
                }
                i += 1;

            }

            //insertarmetas

            string[,] pArreglom = new string[,] { { "@idgeneral", "8", IDGeneral } };
            funUpdateDelete(tx, "Delete_tblp_SP_generalmetas", pArreglom);


            Int64 idmetaobra = 0;
            Int64 idgeneralm = 0;
            Int64 idunidad = 0;
            String unidad = "";
            Decimal metaprogramada = 0;
            Decimal metaalcanzada = 0;
            String cantidad = "0";
            String descripcion = "";
            Int16 im = 0;
            foreach (GridViewRow row in grvmetasm.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {


                    idmetaobra = Convert.ToInt64("0");
                    idgeneralm = Convert.ToInt64(grvmetasm.DataKeys[im].Values[1]);
                    idunidad = Convert.ToInt64(grvmetasm.DataKeys[im].Values[2]);
                    unidad = Convert.ToString(grvmetasm.DataKeys[im].Values[3]);
                  
                    descripcion = Convert.ToString(grvmetasm.DataKeys[im].Values[4]);
                    cantidad = Convert.ToString(grvmetasm.DataKeys[im].Values[5]);

                    funInsertmetas(tx, idmetaobra.ToString(), IDGeneral.ToString(), idunidad.ToString(), unidad.ToString(), metaprogramada.ToString(), metaalcanzada.ToString(), descripcion.ToString(), "0", fechamovimiento, idusuario, "A", cantidad);

                }
                im += 1;

            }


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertOficios(SqlTransaction tx, string idoficioobra,string idgeneral , string idtipooficio, string  nombredocumento, string FechaOficio, string Archivo, string idohficioobra, string fechamovimiento, string idusuario, string accion, string Observaciones)
    {
        try
        {

           
            string[,] array = new string[,] { { "@idoficioobra", "8", idoficioobra }, { "@idgeneral", "8", idgeneral }, { "@idtipooficio", "8", idtipooficio }, { "@nombredocumento", "12", nombredocumento }, { "@FechaOficio", "4", FechaOficio }, { "@Archivo", "12", Archivo }, { "@Observaciones", "12", Observaciones }   };
            funInsert(tx, "Insert_tblp_SP_generaloficios", array);
            string idgeneralNew = IDNewx.ToString();

            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\Oficios\";

            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SP_generaloficios" }, { "@rutaarchivo", "12", Rutaraiz + Archivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================




            string[,] arrayH = new string[,] { { "@idhoficioobra", "8", "0" },  { "@idoficioobra", "8", idgeneralNew }, { "@idgeneral", "8", idgeneral }, { "@idtipooficio", "8", idtipooficio }, { "@nombredocumento", "12", nombredocumento }, { "@FechaOficio", "4", FechaOficio }, { "@Archivo", "12", Archivo }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@Observaciones", "12", Observaciones } };
            funInsert(tx, "Insert_tblp_SP_Hgeneraloficios", arrayH);

            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }

    }
    public Boolean funInsertmetas(SqlTransaction tx, string idmetaobra, string idgeneral, string idunidad, string unidad, string metaprogramada, string metaalcanzada, string descripcion, string idhmetaobra, string fechamovimiento, string idusuario, string accion, string cantidad)
    {
        try
        {
       

            string[,] array = new string[,] { { "@idmetaobra", "8", idmetaobra }, { "@idgeneral", "8", idgeneral }, { "@idunidad", "8", idunidad }, { "@unidad", "12", unidad }, { "@metaprogramada", "5", metaprogramada }, { "@metaalcanzada", "5", metaalcanzada }, { "@descripcion", "12", descripcion }, { "@cantidad", "12", cantidad } };
            funInsert(tx, "Insert_tblp_SP_generalmetas", array);
            string idgeneralNew = IDNewx.ToString();


            string[,] arrayH = new string[,] { { "@idhmetaobra", "8", "0" }, { "@idmetaobra", "8", idgeneralNew }, { "@idgeneral", "8", idgeneral }, { "@idunidad", "8", idunidad }, { "@unidad", "12", unidad }, { "@metaprogramada", "5", metaprogramada }, { "@metaalcanzada", "5", metaalcanzada }, { "@descripcion", "12", descripcion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@cantidad", "12", cantidad } };
            funInsert(tx, "Insert_tblp_SP_Hgeneralmetas", arrayH);

            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }

    }

    #endregion
    #region Anteporyectos_KGS 
    public virtual void funeliminaranteproyecto(string IDAnteproyecto, string pNombreproyecto, string pidanio, string pannio, string pIDMunicipio, string pCveMunicipio, string pMNP_Nombre, string pClavePresupuestal, string pTotales, string pIDPeticion, string pFechaAlta, string pFechaMovimiento, string pIDUsuario, string pAccion, string pidobra, string pCveLocalidades, string pLCL_Nombre)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] pArreglo = new string[,] { { "@IDAnteproyecto", "8", IDAnteproyecto.ToString() } };

            string[,] arrayse = new string[,] { { "@IDAnteproyecto", "8", IDAnteproyecto }, { "@Nombreproyecto", "12", pNombreproyecto }, { "@idanio", "8", pidanio }, { "@annio", "12", pannio }, { "@IDMunicipio", "8", pIDMunicipio }, { "@CveMunicipio", "12", pCveMunicipio }, { "@MNP_Nombre", "12", pMNP_Nombre }, { "@ClavePresupuestal", "12", pClavePresupuestal }, { "@Totales", "5", pTotales }, { "@IDPeticion", "8", pIDPeticion }, { "@FechaAlta", "4", pFechaAlta }, { "@FechaMovimiento", "4", pFechaMovimiento }, { "@IDUsuario", "8", pIDUsuario }, { "@Accion", "12", pAccion }, { "@idobra", "8", pidobra }, { "@CveLocalidades", "12", pCveLocalidades }, { "@LCL_Nombre", "12", pLCL_Nombre } };
            funInsert(tx, "Insert_tblH_Anteproyecto", arrayse);

            funUpdateDelete(tx, "Delete_tbl_Anteproyecto", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertanteproyecto(string pIDAnteproyecto, string pNombreproyecto, string pidanio,string pannio, string pIDMunicipio, string pCveMunicipio, string pMNP_Nombre, string pClavePresupuestal, string pTotales, string pIDPeticion, string pFechaAlta, string pFechaMovimiento, string pIDUsuario, string pAccion, string pidobra, string pCveLocalidades, string pLCL_Nombre)
    {
                     cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDAnteproyecto", "8", pIDAnteproyecto }, { "@Nombreproyecto", "12", pNombreproyecto }, { "@idanio", "8", pidanio }, { "@annio", "12", pannio }, { "@IDMunicipio", "8", pIDMunicipio }, { "@CveMunicipio", "12", pCveMunicipio }, { "@MNP_Nombre", "12", pMNP_Nombre }, { "@ClavePresupuestal", "12", pClavePresupuestal }, { "@Totales", "5", pTotales },  { "@IDPeticion", "8", pIDPeticion }, { "@FechaAlta", "4", pFechaAlta }, { "@idobra", "8", pidobra }, { "@CveLocalidades", "12", pCveLocalidades }, { "@LCL_Nombre", "12", pLCL_Nombre } };
            funInsert(tx, "Insert_tbl_Anteproyecto", array);
            string idanteproyecNew = IDNewx.ToString();
            string[,] arrayse = new string[,] { { "@IDAnteproyecto", "8", pIDAnteproyecto }, { "@Nombreproyecto", "12", pNombreproyecto }, { "@idanio", "8", pidanio }, { "@annio", "12", pannio }, { "@IDMunicipio", "8", pIDMunicipio }, { "@CveMunicipio", "12", pCveMunicipio }, { "@MNP_Nombre", "12", pMNP_Nombre }, { "@ClavePresupuestal", "12", pClavePresupuestal }, { "@Totales", "5", pTotales }, { "@IDPeticion", "8", pIDPeticion }, { "@FechaAlta", "4", pFechaAlta }, { "@FechaMovimiento", "4", pFechaMovimiento }, { "@IDUsuario", "8", pIDUsuario }, { "@Accion", "12", pAccion }, { "@idobra", "8", pidobra }, { "@CveLocalidades", "12", pCveLocalidades }, { "@LCL_Nombre", "12", pLCL_Nombre } };
            funInsert(tx, "Insert_tblH_Anteproyecto", arrayse);


            //si viene de una peticion se actualiza el valor en la tabla

            if (Convert.ToInt64(pidobra) > 0)
            {
                string[,] arrayp = new string[,] {{ "@idobra", "8", pidobra },
                                              { "@idanteproyecto", "8", idanteproyecNew.ToString()  }
                                             };
                funUpdateDelete(tx, "Update_tblp_PET_obraproyectoidanteproyecto", arrayp);
            }


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateanteproyecto(string pIDAnteproyecto, string pNombreproyecto, string pidanio, string pannio, string pIDMunicipio, string pCveMunicipio, string pMNP_Nombre, string pClavePresupuestal, string pTotales, string pFechaMovimiento, string pIDUsuario, string pAccion,  string pIDPeticion, string pFechaAlta, string pidobra, string pCveLocalidades, string pLCL_Nombre)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDAnteproyecto", "8", pIDAnteproyecto }, { "@Nombreproyecto", "12", pNombreproyecto }, { "@idanio", "8", pidanio }, { "@annio", "12", pannio }, { "@IDMunicipio", "8", pIDMunicipio }, { "@CveMunicipio", "12", pCveMunicipio }, { "@MNP_Nombre", "12", pMNP_Nombre }, { "@ClavePresupuestal", "12", pClavePresupuestal }, { "@Totales", "5", pTotales }, { "@CveLocalidades", "12", pCveLocalidades }, { "@LCL_Nombre", "12", pLCL_Nombre } };
            funUpdateDelete(tx, "Update_tbl_Anteproyecto", array);


            string[,] arrayse = new string[,] { { "@IDAnteproyecto", "8", pIDAnteproyecto }, { "@Nombreproyecto", "12", pNombreproyecto }, { "@idanio", "8", pidanio }, { "@annio", "12", pannio }, { "@IDMunicipio", "8", pIDMunicipio }, { "@CveMunicipio", "12", pCveMunicipio }, { "@MNP_Nombre", "12", pMNP_Nombre }, { "@ClavePresupuestal", "12", pClavePresupuestal }, { "@Totales", "5", pTotales }, { "@IDPeticion", "8", pIDPeticion }, { "@FechaAlta", "4", pFechaAlta }, { "@FechaMovimiento", "4", pFechaMovimiento }, { "@IDUsuario", "8", pIDUsuario }, { "@Accion", "12", pAccion }, { "@idobra", "8", pidobra }, { "@CveLocalidades", "12", pCveLocalidades }, { "@LCL_Nombre", "12", pLCL_Nombre } };
            funInsert(tx, "Insert_tblH_Anteproyecto", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }


    public virtual void funGuardaanteproyecto(GridView grvdocts, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;


        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);


                        CheckBox chkRow = (row.FindControl("chkdocumento") as CheckBox);

                        CheckBox chkRow1 = (row.FindControl("chkdocumento1") as CheckBox);

                        string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@anteproyecto", "2", chkRow.Checked.ToString() } };
                            funUpdateDelete(tx, "Update_anteproyectocoplade", array);
                            string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@anteproyecto", "2", chkRow.Checked.ToString() }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                            funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdateanteproyecto", arrayH);
                            
                        

                    }
                    i += 1;

                }
            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }


    public virtual void funGuardaficha(GridView grvdocts, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;


        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);



                        CheckBox chkRow1 = (row.FindControl("chkdocumento11") as CheckBox);

                        string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@Ficha", "2", chkRow1.Checked.ToString() } };
                        funUpdateDelete(tx, "Update_fichacoplade", array);
                        string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@Ficha", "2", chkRow1.Checked.ToString() }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                        funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdateficha", arrayH);



                    }
                    i += 1;

                }
            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    public virtual void funGuardaaprobado(GridView grvdocts, string accion, string archivoaprobacion, string idusuario, string fechamovimiento, string idusuarioa, string usuarioa)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;
        string archivo = "";
        string idusuarioarow ="0";
        string usuarioarow = "0";

        string Rutaraiz =  @"C:\inetpub\wwwroot\SIOP\Uploads\coplade\";

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);


                        CheckBox chkRow = (row.FindControl("chkdocumentoSEC") as CheckBox);

                        if (chkRow.Checked == true)
                        {
                            archivo = archivoaprobacion;
                            idusuarioarow= idusuarioa;
                            usuarioarow = usuarioa;
                        }
                        else
                        {
                            archivo = "";
                            idusuarioarow = "0";
                            usuarioarow = "0";
                        }

                        string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@aprobada", "2", chkRow.Checked.ToString() }, { "@rutaarchivoaprobacion", "12", archivo }, { "@Idusuarioautoriza", "8", idusuarioarow }, { "@usuarioautoriza", "12", usuarioarow } };
                        funUpdateDelete(tx, "Update_aprobadocoplade", array);
                        string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@aprobada", "2", chkRow.Checked.ToString() }, { "@rutaarchivoaprobacion", "12", archivo }, { "@Idusuarioautoriza", "8", idusuarioarow }, { "@usuarioautoriza", "12", usuarioarow }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                        funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdateaprobada", arrayH);

                        if (archivo != "")
                        {

                            //=======================================================================================================================================================================================================================
                            //====================================================================== para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================

                            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Anteproyecto" }, { "@rutaarchivo", "12", Rutaraiz + archivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                            //=======================================================================================================================================================================================================================
                            //======================================================================   para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================
                        }




                    }
                    i += 1;

                }
            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    public virtual void funGuardatieneproyecto(GridView grvdocts, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;
          
        tx = cnn.BeginTransaction();
        try
        {           
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);
                                              

                        CheckBox chkRow1 = (row.FindControl("chkdocumento1") as CheckBox);

                        string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@tieneProyecto", "2", chkRow1.Checked.ToString() } };
                        funUpdateDelete(tx, "Update_tieneanteproyectocoplade", array);
                        string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@tieneProyecto", "2", chkRow1.Checked.ToString() }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                        funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdatetieneanteproyecto", arrayH);
                        

                    }
                    i += 1;

                }
            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }


    #endregion
    #region SICEVALFinanciero_ADG
    //public Boolean funinsertardatosfinancieros(/*iddatosfinanciero*/string iddatosfinanciero,/*tipo de fuente*/string idtipofuente, string fuente,/*Fuente Fin*/string Idfuentefinanciamiento, string fuentefinanciamiento, /*origenfuente*/string idorigenrecurso, string origen_recurso,/*ciclo origen*/ string idcicloorigen, string aniocicloorigen,/*clave presp*/string cvepresupuestalsiahefederalobra, string cvepresupuestalsiahefederalindirecto, string cvepresupuestalsiaheestatalobra, string cvepresupuestalsiaheestatalindirecto,/*subfuente fin*/string idsubfuentefin, string subfuentefinanciamiento,/*Fecha vencimiento*/string fechavencimientoFF, string fechaprorrogavencimientoFF,/*Aprobado*/ string aprobadofederal, string aprobadofederalobra, string aprobadofederalindirecto, string aprobadoestatal, string aprobadoestatalobra, string aprobadoestatalindirecto,/*ampliacion*/string ampliacionfederal, string ampliacionfederalobra, string ampliacionfederalindirecto, string ampliacionestatal, string ampliacionestatalobra, string ampliacionestatalindirecto,/*reduccion*/string reduccionfederal, string reduccionfederalobra, string reduccionfederalindirecto, string reduccionestatal, string reduccionestatalobra, string reduccionestatalindirecto,/*modificacion*/string modificadofederal, string modificadofederalobra, string modificadofederalindirecto, string modificadoestatal, string modificadoestatalobra, string modificadoestatalindirecto,/*liberado*/string liberadofederal, string liberadofederalobra, string liberadofederalindirecto, string liberadoestatal, string liberadoestatalobra, string liberadoestatalindirecto, /*Ministrado*/string ministradofederal, string ministradofederalobra, string ministradofederalindirecto, string ministradoestatal, string ministradoestatalobra, string ministradoestatalindirecto, /*Devengado*/string devengadofederal, string devengadofederalobra, string devengadofederalindirecto, string devengadoestatal, string devengadoestatalobra, string devengadoestatalindirecto,/*ejercido*/string ejercidofederal, string ejercidofederalobra, string ejercidofederalindirecto, string ejercidoestatal, string ejercidoestatalobra, string ejercidoestatalindirecto,/*pagado*/string pagadofederal, string pagadofederalobra, string pagadofederalindirecto, string pagadoestatal, string pagadoestatalobra, string pagadoestatalindirecto,/*Refrendo*/string refrendofederal, string refrendofederalobra, string refrendofederalindirecto, string refrendoestatal, string refrendoestatalobra, string refrendoestatalindirecto,/*Economia*/string economiafederal, string economiafederalobra, string economiafederalindirecto, string economiaestatal, string economiaestatalobra, string economiaestatalindirecto,/*municipal*/string municipaltotal, string municipalobra, string municipalindirecto,/*monto contrato*/string contratadofederal, string contratadofederalobra, string contratadofederalindirecto, string contratadoestatal, string contratadoestatalobra, string contratadoestatalindirecto,/*municipal*/string contratadomunicipal, string contratadomunicipalobra, string contratadomunicipalindirecto, string idgeneral, string idaccion, string idproyecto, string idanio, string anio, string idhijo)
    public Boolean funinsertardatosfinancieros(/*iddatosfinanciero*/string iddatosfinanciero,/*tipo de fuente*/string idtipofuente,string fuente,/*Fuente Fin*/string Idfuentefinanciamiento, string fuentefinanciamiento, /*origenfuente*/string idorigenrecurso,string origen_recurso,/*ciclo origen*/ string idcicloorigen,string aniocicloorigen,/*clave presp*/string cvepresupuestalsiahefederalobra, string cvepresupuestalsiahefederalindirecto,string cvepresupuestalsiaheestatalobra,string cvepresupuestalsiaheestatalindirecto,/*subfuente fin*/string idsubfuentefin, string subfuentefinanciamiento,/*Fecha vencimiento*/string fechavencimientoFF,string fechaprorrogavencimientoFF,/*Aprobado*/string aprobadototal, string aprobadofederal,string aprobadofederalobra, string aprobadofederalindirecto, string aprobadoestatal, string aprobadoestatalobra, string aprobadoestatalindirecto,/*ampliacion*/string ampliaciontotal, string ampliacionfederal,string ampliacionfederalobra,string ampliacionfederalindirecto,string ampliacionestatal,string ampliacionestatalobra,string ampliacionestatalindirecto,/*reduccion*/string reducciontotal, string reduccionfederal,string reduccionfederalobra,string reduccionfederalindirecto,string reduccionestatal,string reduccionestatalobra,string reduccionestatalindirecto,/*modificacion*/string modificadototal, string modificadofederal,string modificadofederalobra,string modificadofederalindirecto,string modificadoestatal,string modificadoestatalobra,string modificadoestatalindirecto,/*liberado*/string liberadototal, string liberadofederal,string liberadofederalobra,string liberadofederalindirecto,string liberadoestatal,string liberadoestatalobra,string liberadoestatalindirecto, /*Ministrado*/string ministradototal, string ministradofederal,string ministradofederalobra,string ministradofederalindirecto,string ministradoestatal,string ministradoestatalobra,string ministradoestatalindirecto, /*Devengado*/string devengadototal, string devengadofederal,string devengadofederalobra,string devengadofederalindirecto,string devengadoestatal,string devengadoestatalobra,string devengadoestatalindirecto,/*ejercido*/string ejercidototal, string ejercidofederal,string ejercidofederalobra,string ejercidofederalindirecto,string ejercidoestatal,string ejercidoestatalobra,string ejercidoestatalindirecto,/*pagado*/string pagadototal, string pagadofederal,string pagadofederalobra,string pagadofederalindirecto,string pagadoestatal,string pagadoestatalobra,string pagadoestatalindirecto,/*Refrendo*/string refrendototal, string refrendofederal,string refrendofederalobra,string  refrendofederalindirecto,string refrendoestatal,string refrendoestatalobra, string refrendoestatalindirecto,/*Economia*/ string economiatotal, string economiafederal,string economiafederalobra,string economiafederalindirecto,string economiaestatal,string economiaestatalobra,string economiaestatalindirecto,/*municipal*/string municipaltotal,string municipalobra,string municipalindirecto,/*monto contrato*/string contratadototal,string contratadototalobra,string contratadototalindirecto, string contratadofederal,string contratadofederalobra,string contratadofederalindirecto,string contratadoestatal,string contratadoestatalobra,string contratadoestatalindirecto,/*municipal*/string contratadomunicipal,string contratadomunicipalobra,string contratadomunicipalindirecto,string idgeneral,string idaccion,string idproyecto,string idanio,string anio,string idhijo,/*Economia presupuestal*/ string economiapresupuestaltotal, string economiapresupuestalfederal, string economiapresupuestalfederalobra, string economiapresupuestalfederalindirecto, string economiapresupuestalestatal, string economiapresupuestalestatalobra, string economiapresupuestalestatalindirecto)
    { 
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
 
            string[,] array = new string[,] {
                                           { "@iddatosfinanciero", "8", iddatosfinanciero },
                                           { "@idtipofuente", "8",idtipofuente},
                                           { "@fuente", "12", fuente},
                                           { "@Idfuentefinanciamiento", "8",Idfuentefinanciamiento},
                                           { "@fuentefinanciamiento", "12", fuentefinanciamiento},
                                           { "@idorigenrecurso", "8",idorigenrecurso },
                                           { "@origen_recurso", "12", origen_recurso },
                                           { "@idcicloorigen", "8",idcicloorigen },
                                           { "@aniocicloorigen", "12", aniocicloorigen },
                                           { "@cvepresupuestalsiahefederalobra", "12", cvepresupuestalsiahefederalobra },
                                           { "@cvepresupuestalsiahefederalindirecto", "12", cvepresupuestalsiahefederalindirecto },
                                           { "@cvepresupuestalsiaheestatalobra", "12", cvepresupuestalsiaheestatalobra },
                                           { "@cvepresupuestalsiaheestatalindirecto", "12", cvepresupuestalsiaheestatalindirecto },
                                           { "@idsubfuentefin", "8", idsubfuentefin },
                                           { "@subfuentefinanciamiento", "12", subfuentefinanciamiento },
                                           { "@fechavencimientoFF", "4", fechavencimientoFF },
                                           { "@fechaprorrogavencimientoFF", "4", fechaprorrogavencimientoFF },
                                           { "@aprobadototal", "5", aprobadototal },//new
                                           { "@aprobadofederal", "5", aprobadofederal },
                                           { "@aprobadofederalobra", "5", aprobadofederalobra },
                                           { "@aprobadofederalindirecto", "5", aprobadofederalindirecto },
                                           { "@aprobadoestatal", "5", aprobadoestatal },
                                           { "@aprobadoestatalobra", "5", aprobadoestatalobra },
                                           { "@aprobadoestatalindirecto", "5", aprobadoestatalindirecto },
                                           { "@ampliaciontotal", "5", ampliaciontotal },//new
                                           { "@ampliacionfederal", "5", ampliacionfederal },
                                           { "@ampliacionfederalobra", "5", ampliacionfederalobra },
                                           { "@ampliacionfederalindirecto", "5", ampliacionfederalindirecto },
                                           { "@ampliacionestatal", "5", ampliacionestatal },
                                           { "@ampliacionestatalobra", "5", ampliacionestatalobra },
                                           { "@ampliacionestatalindirecto", "5", ampliacionestatalindirecto },
                                           { "@reducciontotal", "5", reducciontotal },//new
                                           { "@reduccionfederal", "5", reduccionfederal },
                                           { "@reduccionfederalobra", "5", reduccionfederalobra },
                                           { "@reduccionfederalindirecto", "5", reduccionfederalindirecto },
                                           { "@reduccionestatal", "5", reduccionestatal },
                                           { "@reduccionestatalobra", "5", reduccionestatalobra },
                                           { "@reduccionestatalindirecto", "5", reduccionestatalindirecto },
                                           { "@modificadototal", "5", modificadototal },//new
                                           { "@modificadofederal", "5", modificadofederal },
                                           { "@modificadofederalobra", "5", modificadofederalobra },
                                           { "@modificadofederalindirecto", "5", modificadofederalindirecto },
                                           { "@modificadoestatal", "5", modificadoestatal },
                                           { "@modificadoestatalobra", "5", modificadoestatalobra },
                                           { "@modificadoestatalindirecto", "5", modificadoestatalindirecto },
                                           { "@liberadototal", "5", liberadototal },//new
                                           { "@liberadofederal", "5", liberadofederal },
                                           { "@liberadofederalobra", "5", liberadofederalobra },
                                           { "@liberadofederalindirecto", "5", liberadofederalindirecto },
                                           { "@liberadoestatal", "5", liberadoestatal },
                                           { "@liberadoestatalobra", "5", liberadoestatalobra },
                                           { "@liberadoestatalindirecto", "5", liberadoestatalindirecto },
                                           { "@ministradototal", "5", ministradototal },//new
                                           { "@ministradofederal", "5", ministradofederal },
                                           { "@ministradofederalobra", "5", ministradofederalobra },
                                           { "@ministradofederalindirecto", "5", ministradofederalindirecto },
                                           { "@ministradoestatal", "5", ministradoestatal },
                                           { "@ministradoestatalobra", "5", ministradoestatalobra },
                                           { "@ministradoestatalindirecto", "5", ministradoestatalindirecto },
                                           { "@devengadototal", "5", devengadototal },//new
                                           { "@devengadofederal", "5", devengadofederal },
                                           { "@devengadofederalobra", "5", devengadofederalobra },
                                           { "@devengadofederalindirecto", "5", devengadofederalindirecto },
                                           { "@devengadoestatal", "5", devengadoestatal },
                                           { "@devengadoestatalobra", "5", devengadoestatalobra },
                                           { "@devengadoestatalindirecto", "5", devengadoestatalindirecto },
                                           { "@ejercidototal", "5", ejercidototal  },//new
                                           { "@ejercidofederal", "5", ejercidofederal  },
                                           { "@ejercidofederalobra", "5", ejercidofederalobra },
                                           { "@ejercidofederalindirecto", "5", ejercidofederalindirecto },
                                           { "@ejercidoestatal", "5", ejercidoestatal },
                                           { "@ejercidoestatalobra", "5", ejercidoestatalobra },
                                           { "@ejercidoestatalindirecto", "5", ejercidoestatalindirecto },
                                           { "@pagadototal", "5", pagadototal },//new
                                           { "@pagadofederal", "5", pagadofederal },
                                           { "@pagadofederalobra", "5", pagadofederalobra },
                                           { "@pagadofederalindirecto", "5", pagadofederalindirecto },
                                           { "@pagadoestatal", "5", pagadoestatal },
                                           { "@pagadoestatalobra", "5", pagadoestatalobra },
                                           { "@pagadoestatalindirecto", "5", pagadoestatalindirecto },
                                           { "@refrendototal", "5", refrendototal },//new
                                           { "@refrendofederal", "5", refrendofederal },
                                           { "@refrendofederalobra", "5", refrendofederalobra },
                                           { "@refrendofederalindirecto", "5", refrendofederalindirecto },
                                           { "@refrendoestatal", "5", refrendoestatal },
                                           { "@refrendoestatalobra", "5", refrendoestatalobra },
                                           { "@refrendoestatalindirecto", "5", refrendoestatalindirecto },
                                           { "@economiatotal", "5", economiatotal },//new
                                           { "@economiafederal", "5", economiafederal },
                                           { "@economiafederalobra", "5", economiafederalobra },
                                           { "@economiafederalindirecto", "5", economiafederalindirecto },
                                           { "@economiaestatal", "5", economiaestatal },
                                           { "@economiaestatalobra", "5", economiaestatalobra },
                                           { "@economiaestatalindirecto", "5", economiaestatalindirecto },
                                           { "@municipaltotal", "5", municipaltotal },
                                           { "@municipalobra", "5", municipalobra },
                                           { "@municipalindirecto", "5", municipalindirecto },
                                           { "@contratadototal", "5", contratadototal },//new
                                           { "@contratadototalobra", "5", contratadototalobra },//new
                                           { "@contratadototalindirecto", "5", contratadototalindirecto },//new
                                           { "@contratadofederal", "5", contratadofederal },
                                           { "@contratadofederalobra", "5", contratadofederalobra },
                                           { "@contratadofederalindirecto", "5", contratadofederalindirecto },
                                           { "@contratadoestatal", "5", contratadoestatal },
                                           { "@contratadoestatalobra", "5", contratadoestatalobra },
                                           { "@contratadoestatalindirecto", "5", contratadoestatalindirecto },
                                           { "@contratadomunicipal", "5", contratadomunicipal },
                                           { "@contratadomunicipalobra", "5", contratadomunicipalobra },
                                           { "@contratadomunicipalindirecto", "5", contratadomunicipalindirecto },
                                           { "@idgeneral", "8", idgeneral },
                                           { "@idaccion", "12", idaccion },
                                           { "@idproyecto", "12", idproyecto },
                                           { "@idanio", "8", idanio },
                                           { "@anio", "12", anio },
                                           { "@idhijo", "8", idhijo },
                                           { "@economiapresupuestaltotal", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederal", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobra", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirecto", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatal", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobra", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirecto", "5", economiapresupuestalestatalindirecto },
                                           { "@economiapresupuestaltotalH", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederalH", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobraH", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirectoH", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatalH", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobraH", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirectoH", "5", economiapresupuestalestatalindirecto }
                                       };
            funInsert(tx, "Insert_tblp_SP_financiero", array);
            string iddatosfinancieronew = IDNewx.ToString();
            string[,] arrayH = new string[,] {
                                           { "@iddatosfinanciero", "8", iddatosfinancieronew },
                                           { "@idtipofuente", "8",idtipofuente},
                                           { "@fuente", "12", fuente},
                                           { "@Idfuentefinanciamiento", "8",Idfuentefinanciamiento},
                                           { "@fuentefinanciamiento", "12", fuentefinanciamiento},
                                           { "@idorigenrecurso", "8",idorigenrecurso },
                                           { "@origen_recurso", "12", origen_recurso },
                                           { "@idcicloorigen", "8",idcicloorigen },
                                           { "@aniocicloorigen", "12", aniocicloorigen },
                                           { "@cvepresupuestalsiahefederalobra", "12", cvepresupuestalsiahefederalobra },
                                           { "@cvepresupuestalsiahefederalindirecto", "12", cvepresupuestalsiahefederalindirecto },
                                           { "@cvepresupuestalsiaheestatalobra", "12", cvepresupuestalsiaheestatalobra },
                                           { "@cvepresupuestalsiaheestatalindirecto", "12", cvepresupuestalsiaheestatalindirecto },
                                           { "@idsubfuentefin", "8", idsubfuentefin },
                                           { "@subfuentefinanciamiento", "12", subfuentefinanciamiento },
                                           { "@fechavencimientoFF", "4", fechavencimientoFF },
                                           { "@fechaprorrogavencimientoFF", "4", fechaprorrogavencimientoFF },
                                           { "@aprobadototal", "5", aprobadototal },//new
                                           { "@aprobadofederal", "5", aprobadofederal },
                                           { "@aprobadofederalobra", "5", aprobadofederalobra },
                                           { "@aprobadofederalindirecto", "5", aprobadofederalindirecto },
                                           { "@aprobadoestatal", "5", aprobadoestatal },
                                           { "@aprobadoestatalobra", "5", aprobadoestatalobra },
                                           { "@aprobadoestatalindirecto", "5", aprobadoestatalindirecto },
                                           { "@ampliaciontotal", "5", ampliaciontotal },//new
                                           { "@ampliacionfederal", "5", ampliacionfederal },
                                           { "@ampliacionfederalobra", "5", ampliacionfederalobra },
                                           { "@ampliacionfederalindirecto", "5", ampliacionfederalindirecto },
                                           { "@ampliacionestatal", "5", ampliacionestatal },
                                           { "@ampliacionestatalobra", "5", ampliacionestatalobra },
                                           { "@ampliacionestatalindirecto", "5", ampliacionestatalindirecto },
                                           { "@reducciontotal", "5", reducciontotal },//new
                                           { "@reduccionfederal", "5", reduccionfederal },
                                           { "@reduccionfederalobra", "5", reduccionfederalobra },
                                           { "@reduccionfederalindirecto", "5", reduccionfederalindirecto },
                                           { "@reduccionestatal", "5", reduccionestatal },
                                           { "@reduccionestatalobra", "5", reduccionestatalobra },
                                           { "@reduccionestatalindirecto", "5", reduccionestatalindirecto },
                                           { "@modificadototal", "5", modificadototal },//new
                                           { "@modificadofederal", "5", modificadofederal },
                                           { "@modificadofederalobra", "5", modificadofederalobra },
                                           { "@modificadofederalindirecto", "5", modificadofederalindirecto },
                                           { "@modificadoestatal", "5", modificadoestatal },
                                           { "@modificadoestatalobra", "5", modificadoestatalobra },
                                           { "@modificadoestatalindirecto", "5", modificadoestatalindirecto },
                                           { "@liberadototal", "5", liberadototal },//new
                                           { "@liberadofederal", "5", liberadofederal },
                                           { "@liberadofederalobra", "5", liberadofederalobra },
                                           { "@liberadofederalindirecto", "5", liberadofederalindirecto },
                                           { "@liberadoestatal", "5", liberadoestatal },
                                           { "@liberadoestatalobra", "5", liberadoestatalobra },
                                           { "@liberadoestatalindirecto", "5", liberadoestatalindirecto },
                                           { "@ministradototal", "5", ministradototal },//new
                                           { "@ministradofederal", "5", ministradofederal },
                                           { "@ministradofederalobra", "5", ministradofederalobra },
                                           { "@ministradofederalindirecto", "5", ministradofederalindirecto },
                                           { "@ministradoestatal", "5", ministradoestatal },
                                           { "@ministradoestatalobra", "5", ministradoestatalobra },
                                           { "@ministradoestatalindirecto", "5", ministradoestatalindirecto },
                                           { "@devengadototal", "5", devengadototal },//new
                                           { "@devengadofederal", "5", devengadofederal },
                                           { "@devengadofederalobra", "5", devengadofederalobra },
                                           { "@devengadofederalindirecto", "5", devengadofederalindirecto },
                                           { "@devengadoestatal", "5", devengadoestatal },
                                           { "@devengadoestatalobra", "5", devengadoestatalobra },
                                           { "@devengadoestatalindirecto", "5", devengadoestatalindirecto },
                                           { "@ejercidototal", "5", ejercidototal  },//new
                                           { "@ejercidofederal", "5", ejercidofederal  },
                                           { "@ejercidofederalobra", "5", ejercidofederalobra },
                                           { "@ejercidofederalindirecto", "5", ejercidofederalindirecto },
                                           { "@ejercidoestatal", "5", ejercidoestatal },
                                           { "@ejercidoestatalobra", "5", ejercidoestatalobra },
                                           { "@ejercidoestatalindirecto", "5", ejercidoestatalindirecto },
                                           { "@pagadototal", "5", pagadototal },//new
                                           { "@pagadofederal", "5", pagadofederal },
                                           { "@pagadofederalobra", "5", pagadofederalobra },
                                           { "@pagadofederalindirecto", "5", pagadofederalindirecto },
                                           { "@pagadoestatal", "5", pagadoestatal },
                                           { "@pagadoestatalobra", "5", pagadoestatalobra },
                                           { "@pagadoestatalindirecto", "5", pagadoestatalindirecto },
                                           { "@refrendototal", "5", refrendototal },//new
                                           { "@refrendofederal", "5", refrendofederal },
                                           { "@refrendofederalobra", "5", refrendofederalobra },
                                           { "@refrendofederalindirecto", "5", refrendofederalindirecto },
                                           { "@refrendoestatal", "5", refrendoestatal },
                                           { "@refrendoestatalobra", "5", refrendoestatalobra },
                                           { "@refrendoestatalindirecto", "5", refrendoestatalindirecto },
                                           { "@economiatotal", "5", economiatotal },//new
                                           { "@economiafederal", "5", economiafederal },
                                           { "@economiafederalobra", "5", economiafederalobra },
                                           { "@economiafederalindirecto", "5", economiafederalindirecto },
                                           { "@economiaestatal", "5", economiaestatal },
                                           { "@economiaestatalobra", "5", economiaestatalobra },
                                           { "@economiaestatalindirecto", "5", economiaestatalindirecto },
                                           { "@municipaltotal", "5", municipaltotal },
                                           { "@municipalobra", "5", municipalobra },
                                           { "@municipalindirecto", "5", municipalindirecto },
                                           { "@contratadototal", "5", contratadototal },//new
                                           { "@contratadototalobra", "5", contratadototalobra },//new
                                           { "@contratadototalindirecto", "5", contratadototalindirecto },//new
                                           { "@contratadofederal", "5", contratadofederal },
                                           { "@contratadofederalobra", "5", contratadofederalobra },
                                           { "@contratadofederalindirecto", "5", contratadofederalindirecto },
                                           { "@contratadoestatal", "5", contratadoestatal },
                                           { "@contratadoestatalobra", "5", contratadoestatalobra },
                                           { "@contratadoestatalindirecto", "5", contratadoestatalindirecto },
                                           { "@contratadomunicipal", "5", contratadomunicipal },
                                           { "@contratadomunicipalobra", "5", contratadomunicipalobra },
                                           { "@contratadomunicipalindirecto", "5", contratadomunicipalindirecto },
                                           { "@idgeneral", "8", idgeneral },
                                           { "@idaccion", "12", idaccion },
                                           { "@idproyecto", "12", idproyecto },
                                           { "@idanio", "8", idanio },
                                           { "@anio", "12", anio },
                                           { "@idhijo", "8", idhijo },
                                           { "@economiapresupuestaltotal", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederal", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobra", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirecto", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatal", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobra", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirecto", "5", economiapresupuestalestatalindirecto },
                                           { "@economiapresupuestaltotalH", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederalH", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobraH", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirectoH", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatalH", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobraH", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirectoH", "5", economiapresupuestalestatalindirecto }
                                       };
            funInsert(tx, "Insert_tblp_SP_Hfinanciero", arrayH);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
     }
    
    //public Boolean funupdatedatosfinancieros(/*iddatosfinanciero*/string iddatosfinanciero,/*fuente*/string idtipofuente, string fuente,/*origenfuente*/string idorigenrecurso, string origen_recurso,/*ciclo origen*/ string idcicloorigen, string aniocicloorigen,/*clave presp*/string cvepresupuestalsiahefederalobra, string cvepresupuestalsiahefederalindirecto, string cvepresupuestalsiaheestatalobra, string cvepresupuestalsiaheestatalindirecto,/*subfuente fin*/string idsubfuentefin, string subfuentefinanciamiento,/*Aprobado*/ string aprobadofederal, string aprobadofederalobra, string aprobadofederalindirecto, string aprobadoestatal, string aprobadoestatalobra, string aprobadoestatalindirecto,/*ampliacion*/string ampliacionfederal, string ampliacionfederalobra, string ampliacionfederalindirecto, string ampliacionestatal, string ampliacionestatalobra, string ampliacionestatalindirecto,/*reduccion*/string reduccionfederal, string reduccionfederalobra, string reduccionfederalindirecto, string reduccionestatal, string reduccionestatalobra, string reduccionestatalindirecto,/*modificacion*/string modificadofederal, string modificadofederalobra, string modificadofederalindirecto, string modificadoestatal, string modificadoestatalobra, string modificadoestatalindirecto,/*liberado*/string liberadofederal, string liberadofederalobra, string liberadofederalindirecto, string liberadoestatal, string liberadoestatalobra, string liberadoestatalindirecto, /*Ministrado*/string ministradofederal, string ministradofederalobra, string ministradofederalindirecto, string ministradoestatal, string ministradoestatalobra, string ministradoestatalindirecto, /*Devengado*/string devengadofederal, string devengadofederalobra, string devengadofederalindirecto, string devengadoestatal, string devengadoestatalobra, string devengadoestatalindirecto,/*ejercido*/string ejercidofederal, string ejercidofederalobra, string ejercidofederalindirecto, string ejercidoestatal, string ejercidoestatalobra, string ejercidoestatalindirecto,/*pagado*/string pagadofederal, string pagadofederalobra, string pagadofederalindirecto, string pagadoestatal, string pagadoestatalobra, string pagadoestatalindirecto,/*Refrendo*/string refrendofederal, string refrendofederalobra, string refrendofederalindirecto, string refrendoestatal, string refrendoestatalobra, string refrendoestatalindirecto,/*Economia*/string economiafederal, string economiafederalobra, string economiafederalindirecto, string economiaestatal, string economiaestatalobra, string economiaestatalindirecto,/*municipal*/string municipaltotal, string municipalobra, string municipalindirecto,/*monto contrato*/string contratadofederal, string contratadofederalobra, string contratadofederalindirecto, string contratadoestatal, string contratadoestatalobra, string contratadoestatalindirecto,/*municipal*/string contratadomunicipal, string contratadomunicipalobra, string contratadomunicipalindirecto, string idgeneral, string idaccion, string idproyecto, string idanio, string anio, string idhijo)
    public Boolean funupdatedatosfinancieros(/*iddatosfinanciero*/string iddatosfinanciero,/*tipo de fuente*/string idtipofuente, string fuente,/*Fuente Fin*/string Idfuentefinanciamiento, string fuentefinanciamiento, /*origenfuente*/string idorigenrecurso, string origen_recurso,/*ciclo origen*/ string idcicloorigen, string aniocicloorigen,/*clave presp*/string cvepresupuestalsiahefederalobra, string cvepresupuestalsiahefederalindirecto, string cvepresupuestalsiaheestatalobra, string cvepresupuestalsiaheestatalindirecto,/*subfuente fin*/string idsubfuentefin, string subfuentefinanciamiento,/*Fecha vencimiento*/string fechavencimientoFF, string fechaprorrogavencimientoFF,/*Aprobado*/string aprobadototal, string aprobadofederal, string aprobadofederalobra, string aprobadofederalindirecto, string aprobadoestatal, string aprobadoestatalobra, string aprobadoestatalindirecto,/*ampliacion*/string ampliaciontotal, string ampliacionfederal, string ampliacionfederalobra, string ampliacionfederalindirecto, string ampliacionestatal, string ampliacionestatalobra, string ampliacionestatalindirecto,/*reduccion*/string reducciontotal, string reduccionfederal, string reduccionfederalobra, string reduccionfederalindirecto, string reduccionestatal, string reduccionestatalobra, string reduccionestatalindirecto,/*modificacion*/string modificadototal, string modificadofederal, string modificadofederalobra, string modificadofederalindirecto, string modificadoestatal, string modificadoestatalobra, string modificadoestatalindirecto,/*liberado*/string liberadototal, string liberadofederal, string liberadofederalobra, string liberadofederalindirecto, string liberadoestatal, string liberadoestatalobra, string liberadoestatalindirecto, /*Ministrado*/string ministradototal, string ministradofederal, string ministradofederalobra, string ministradofederalindirecto, string ministradoestatal, string ministradoestatalobra, string ministradoestatalindirecto, /*Devengado*/string devengadototal, string devengadofederal, string devengadofederalobra, string devengadofederalindirecto, string devengadoestatal, string devengadoestatalobra, string devengadoestatalindirecto,/*ejercido*/string ejercidototal, string ejercidofederal, string ejercidofederalobra, string ejercidofederalindirecto, string ejercidoestatal, string ejercidoestatalobra, string ejercidoestatalindirecto,/*pagado*/string pagadototal, string pagadofederal, string pagadofederalobra, string pagadofederalindirecto, string pagadoestatal, string pagadoestatalobra, string pagadoestatalindirecto,/*Refrendo*/string refrendototal, string refrendofederal, string refrendofederalobra, string refrendofederalindirecto, string refrendoestatal, string refrendoestatalobra, string refrendoestatalindirecto,/*Economia*/ string economiatotal, string economiafederal, string economiafederalobra, string economiafederalindirecto, string economiaestatal, string economiaestatalobra, string economiaestatalindirecto,/*municipal*/string municipaltotal, string municipalobra, string municipalindirecto,/*monto contrato*/string contratadototal, string contratadototalobra, string contratadototalindirecto, string contratadofederal, string contratadofederalobra, string contratadofederalindirecto, string contratadoestatal, string contratadoestatalobra, string contratadoestatalindirecto,/*municipal*/string contratadomunicipal, string contratadomunicipalobra, string contratadomunicipalindirecto, string idgeneral, string idaccion, string idproyecto, string idanio, string anio, string idhijo,/*Economia presupuestal*/ string economiapresupuestaltotal, string economiapresupuestalfederal, string economiapresupuestalfederalobra, string economiapresupuestalfederalindirecto, string economiapresupuestalestatal, string economiapresupuestalestatalobra, string economiapresupuestalestatalindirecto)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] {
                                           { "@iddatosfinanciero", "8", iddatosfinanciero },
                                           { "@idtipofuente", "8",idtipofuente},
                                           { "@fuente", "12", fuente},
                                           { "@Idfuentefinanciamiento", "8",Idfuentefinanciamiento},
                                           { "@fuentefinanciamiento", "12", fuentefinanciamiento},
                                           { "@idorigenrecurso", "8",idorigenrecurso },
                                           { "@origen_recurso", "12", origen_recurso },
                                           { "@idcicloorigen", "8",idcicloorigen },
                                           { "@aniocicloorigen", "12", aniocicloorigen },
                                           { "@cvepresupuestalsiahefederalobra", "12", cvepresupuestalsiahefederalobra },
                                           { "@cvepresupuestalsiahefederalindirecto", "12", cvepresupuestalsiahefederalindirecto },
                                           { "@cvepresupuestalsiaheestatalobra", "12", cvepresupuestalsiaheestatalobra },
                                           { "@cvepresupuestalsiaheestatalindirecto", "12", cvepresupuestalsiaheestatalindirecto },
                                           { "@idsubfuentefin", "8", idsubfuentefin },
                                           { "@subfuentefinanciamiento", "12", subfuentefinanciamiento },
                                           { "@fechavencimientoFF", "4", fechavencimientoFF },
                                           { "@fechaprorrogavencimientoFF", "4", fechaprorrogavencimientoFF },
                                           { "@aprobadototal", "5", aprobadototal },//new
                                           { "@aprobadofederal", "5", aprobadofederal },
                                           { "@aprobadofederalobra", "5", aprobadofederalobra },
                                           { "@aprobadofederalindirecto", "5", aprobadofederalindirecto },
                                           { "@aprobadoestatal", "5", aprobadoestatal },
                                           { "@aprobadoestatalobra", "5", aprobadoestatalobra },
                                           { "@aprobadoestatalindirecto", "5", aprobadoestatalindirecto },
                                           { "@ampliaciontotal", "5", ampliaciontotal },//new
                                           { "@ampliacionfederal", "5", ampliacionfederal },
                                           { "@ampliacionfederalobra", "5", ampliacionfederalobra },
                                           { "@ampliacionfederalindirecto", "5", ampliacionfederalindirecto },
                                           { "@ampliacionestatal", "5", ampliacionestatal },
                                           { "@ampliacionestatalobra", "5", ampliacionestatalobra },
                                           { "@ampliacionestatalindirecto", "5", ampliacionestatalindirecto },
                                           { "@reducciontotal", "5", reducciontotal },//new
                                           { "@reduccionfederal", "5", reduccionfederal },
                                           { "@reduccionfederalobra", "5", reduccionfederalobra },
                                           { "@reduccionfederalindirecto", "5", reduccionfederalindirecto },
                                           { "@reduccionestatal", "5", reduccionestatal },
                                           { "@reduccionestatalobra", "5", reduccionestatalobra },
                                           { "@reduccionestatalindirecto", "5", reduccionestatalindirecto },
                                           { "@modificadototal", "5", modificadototal },//new
                                           { "@modificadofederal", "5", modificadofederal },
                                           { "@modificadofederalobra", "5", modificadofederalobra },
                                           { "@modificadofederalindirecto", "5", modificadofederalindirecto },
                                           { "@modificadoestatal", "5", modificadoestatal },
                                           { "@modificadoestatalobra", "5", modificadoestatalobra },
                                           { "@modificadoestatalindirecto", "5", modificadoestatalindirecto },
                                           { "@liberadototal", "5", liberadototal },//new
                                           { "@liberadofederal", "5", liberadofederal },
                                           { "@liberadofederalobra", "5", liberadofederalobra },
                                           { "@liberadofederalindirecto", "5", liberadofederalindirecto },
                                           { "@liberadoestatal", "5", liberadoestatal },
                                           { "@liberadoestatalobra", "5", liberadoestatalobra },
                                           { "@liberadoestatalindirecto", "5", liberadoestatalindirecto },
                                           { "@ministradototal", "5", ministradototal },//new
                                           { "@ministradofederal", "5", ministradofederal },
                                           { "@ministradofederalobra", "5", ministradofederalobra },
                                           { "@ministradofederalindirecto", "5", ministradofederalindirecto },
                                           { "@ministradoestatal", "5", ministradoestatal },
                                           { "@ministradoestatalobra", "5", ministradoestatalobra },
                                           { "@ministradoestatalindirecto", "5", ministradoestatalindirecto },
                                           { "@devengadototal", "5", devengadototal },//new
                                           { "@devengadofederal", "5", devengadofederal },
                                           { "@devengadofederalobra", "5", devengadofederalobra },
                                           { "@devengadofederalindirecto", "5", devengadofederalindirecto },
                                           { "@devengadoestatal", "5", devengadoestatal },
                                           { "@devengadoestatalobra", "5", devengadoestatalobra },
                                           { "@devengadoestatalindirecto", "5", devengadoestatalindirecto },
                                           { "@ejercidototal", "5", ejercidototal  },//new
                                           { "@ejercidofederal", "5", ejercidofederal  },
                                           { "@ejercidofederalobra", "5", ejercidofederalobra },
                                           { "@ejercidofederalindirecto", "5", ejercidofederalindirecto },
                                           { "@ejercidoestatal", "5", ejercidoestatal },
                                           { "@ejercidoestatalobra", "5", ejercidoestatalobra },
                                           { "@ejercidoestatalindirecto", "5", ejercidoestatalindirecto },
                                           { "@pagadototal", "5", pagadototal },//new
                                           { "@pagadofederal", "5", pagadofederal },
                                           { "@pagadofederalobra", "5", pagadofederalobra },
                                           { "@pagadofederalindirecto", "5", pagadofederalindirecto },
                                           { "@pagadoestatal", "5", pagadoestatal },
                                           { "@pagadoestatalobra", "5", pagadoestatalobra },
                                           { "@pagadoestatalindirecto", "5", pagadoestatalindirecto },
                                           { "@refrendototal", "5", refrendototal },//new
                                           { "@refrendofederal", "5", refrendofederal },
                                           { "@refrendofederalobra", "5", refrendofederalobra },
                                           { "@refrendofederalindirecto", "5", refrendofederalindirecto },
                                           { "@refrendoestatal", "5", refrendoestatal },
                                           { "@refrendoestatalobra", "5", refrendoestatalobra },
                                           { "@refrendoestatalindirecto", "5", refrendoestatalindirecto },
                                           { "@economiatotal", "5", economiatotal },//new
                                           { "@economiafederal", "5", economiafederal },
                                           { "@economiafederalobra", "5", economiafederalobra },
                                           { "@economiafederalindirecto", "5", economiafederalindirecto },
                                           { "@economiaestatal", "5", economiaestatal },
                                           { "@economiaestatalobra", "5", economiaestatalobra },
                                           { "@economiaestatalindirecto", "5", economiaestatalindirecto },
                                           { "@municipaltotal", "5", municipaltotal },
                                           { "@municipalobra", "5", municipalobra },
                                           { "@municipalindirecto", "5", municipalindirecto },
                                           { "@contratadototal", "5", contratadototal },//new
                                           { "@contratadototalobra", "5", contratadototalobra },//new
                                           { "@contratadototalindirecto", "5", contratadototalindirecto },//new
                                           { "@contratadofederal", "5", contratadofederal },
                                           { "@contratadofederalobra", "5", contratadofederalobra },
                                           { "@contratadofederalindirecto", "5", contratadofederalindirecto },
                                           { "@contratadoestatal", "5", contratadoestatal },
                                           { "@contratadoestatalobra", "5", contratadoestatalobra },
                                           { "@contratadoestatalindirecto", "5", contratadoestatalindirecto },
                                           { "@contratadomunicipal", "5", contratadomunicipal },
                                           { "@contratadomunicipalobra", "5", contratadomunicipalobra },
                                           { "@contratadomunicipalindirecto", "5", contratadomunicipalindirecto },
                                           { "@idgeneral", "8", idgeneral },
                                           { "@idaccion", "12", idaccion },
                                           { "@idproyecto", "12", idproyecto },
                                           { "@idanio", "8", idanio },
                                           { "@anio", "12", anio },
                                           { "@idhijo", "8", idhijo },
                                           { "@economiapresupuestaltotal", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederal", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobra", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirecto", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatal", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobra", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirecto", "5", economiapresupuestalestatalindirecto },
                                           { "@economiapresupuestaltotalH", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederalH", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobraH", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirectoH", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatalH", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobraH", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirectoH", "5", economiapresupuestalestatalindirecto }
                                       };
            funUpdateDelete(tx, "Update_tblp_SP_financiero", array);
            string[,] arrayH = new string[,] {
                                           { "@iddatosfinanciero", "8", iddatosfinanciero },
                                           { "@idtipofuente", "8",idtipofuente},
                                           { "@fuente", "12", fuente},
                                           { "@Idfuentefinanciamiento", "8",Idfuentefinanciamiento},
                                           { "@fuentefinanciamiento", "12", fuentefinanciamiento},
                                           { "@idorigenrecurso", "8",idorigenrecurso },
                                           { "@origen_recurso", "12", origen_recurso },
                                           { "@idcicloorigen", "8",idcicloorigen },
                                           { "@aniocicloorigen", "12", aniocicloorigen },
                                           { "@cvepresupuestalsiahefederalobra", "12", cvepresupuestalsiahefederalobra },
                                           { "@cvepresupuestalsiahefederalindirecto", "12", cvepresupuestalsiahefederalindirecto },
                                           { "@cvepresupuestalsiaheestatalobra", "12", cvepresupuestalsiaheestatalobra },
                                           { "@cvepresupuestalsiaheestatalindirecto", "12", cvepresupuestalsiaheestatalindirecto },
                                           { "@idsubfuentefin", "8", idsubfuentefin },
                                           { "@subfuentefinanciamiento", "12", subfuentefinanciamiento },
                                           { "@fechavencimientoFF", "4", fechavencimientoFF },
                                           { "@fechaprorrogavencimientoFF", "4", fechaprorrogavencimientoFF },
                                           { "@aprobadototal", "5", aprobadototal },//new
                                           { "@aprobadofederal", "5", aprobadofederal },
                                           { "@aprobadofederalobra", "5", aprobadofederalobra },
                                           { "@aprobadofederalindirecto", "5", aprobadofederalindirecto },
                                           { "@aprobadoestatal", "5", aprobadoestatal },
                                           { "@aprobadoestatalobra", "5", aprobadoestatalobra },
                                           { "@aprobadoestatalindirecto", "5", aprobadoestatalindirecto },
                                           { "@ampliaciontotal", "5", ampliaciontotal },//new
                                           { "@ampliacionfederal", "5", ampliacionfederal },
                                           { "@ampliacionfederalobra", "5", ampliacionfederalobra },
                                           { "@ampliacionfederalindirecto", "5", ampliacionfederalindirecto },
                                           { "@ampliacionestatal", "5", ampliacionestatal },
                                           { "@ampliacionestatalobra", "5", ampliacionestatalobra },
                                           { "@ampliacionestatalindirecto", "5", ampliacionestatalindirecto },
                                           { "@reducciontotal", "5", reducciontotal },//new
                                           { "@reduccionfederal", "5", reduccionfederal },
                                           { "@reduccionfederalobra", "5", reduccionfederalobra },
                                           { "@reduccionfederalindirecto", "5", reduccionfederalindirecto },
                                           { "@reduccionestatal", "5", reduccionestatal },
                                           { "@reduccionestatalobra", "5", reduccionestatalobra },
                                           { "@reduccionestatalindirecto", "5", reduccionestatalindirecto },
                                           { "@modificadototal", "5", modificadototal },//new
                                           { "@modificadofederal", "5", modificadofederal },
                                           { "@modificadofederalobra", "5", modificadofederalobra },
                                           { "@modificadofederalindirecto", "5", modificadofederalindirecto },
                                           { "@modificadoestatal", "5", modificadoestatal },
                                           { "@modificadoestatalobra", "5", modificadoestatalobra },
                                           { "@modificadoestatalindirecto", "5", modificadoestatalindirecto },
                                           { "@liberadototal", "5", liberadototal },//new
                                           { "@liberadofederal", "5", liberadofederal },
                                           { "@liberadofederalobra", "5", liberadofederalobra },
                                           { "@liberadofederalindirecto", "5", liberadofederalindirecto },
                                           { "@liberadoestatal", "5", liberadoestatal },
                                           { "@liberadoestatalobra", "5", liberadoestatalobra },
                                           { "@liberadoestatalindirecto", "5", liberadoestatalindirecto },
                                           { "@ministradototal", "5", ministradototal },//new
                                           { "@ministradofederal", "5", ministradofederal },
                                           { "@ministradofederalobra", "5", ministradofederalobra },
                                           { "@ministradofederalindirecto", "5", ministradofederalindirecto },
                                           { "@ministradoestatal", "5", ministradoestatal },
                                           { "@ministradoestatalobra", "5", ministradoestatalobra },
                                           { "@ministradoestatalindirecto", "5", ministradoestatalindirecto },
                                           { "@devengadototal", "5", devengadototal },//new
                                           { "@devengadofederal", "5", devengadofederal },
                                           { "@devengadofederalobra", "5", devengadofederalobra },
                                           { "@devengadofederalindirecto", "5", devengadofederalindirecto },
                                           { "@devengadoestatal", "5", devengadoestatal },
                                           { "@devengadoestatalobra", "5", devengadoestatalobra },
                                           { "@devengadoestatalindirecto", "5", devengadoestatalindirecto },
                                           { "@ejercidototal", "5", ejercidototal  },//new
                                           { "@ejercidofederal", "5", ejercidofederal  },
                                           { "@ejercidofederalobra", "5", ejercidofederalobra },
                                           { "@ejercidofederalindirecto", "5", ejercidofederalindirecto },
                                           { "@ejercidoestatal", "5", ejercidoestatal },
                                           { "@ejercidoestatalobra", "5", ejercidoestatalobra },
                                           { "@ejercidoestatalindirecto", "5", ejercidoestatalindirecto },
                                           { "@pagadototal", "5", pagadototal },//new
                                           { "@pagadofederal", "5", pagadofederal },
                                           { "@pagadofederalobra", "5", pagadofederalobra },
                                           { "@pagadofederalindirecto", "5", pagadofederalindirecto },
                                           { "@pagadoestatal", "5", pagadoestatal },
                                           { "@pagadoestatalobra", "5", pagadoestatalobra },
                                           { "@pagadoestatalindirecto", "5", pagadoestatalindirecto },
                                           { "@refrendototal", "5", refrendototal },//new
                                           { "@refrendofederal", "5", refrendofederal },
                                           { "@refrendofederalobra", "5", refrendofederalobra },
                                           { "@refrendofederalindirecto", "5", refrendofederalindirecto },
                                           { "@refrendoestatal", "5", refrendoestatal },
                                           { "@refrendoestatalobra", "5", refrendoestatalobra },
                                           { "@refrendoestatalindirecto", "5", refrendoestatalindirecto },
                                           { "@economiatotal", "5", economiatotal },//new
                                           { "@economiafederal", "5", economiafederal },
                                           { "@economiafederalobra", "5", economiafederalobra },
                                           { "@economiafederalindirecto", "5", economiafederalindirecto },
                                           { "@economiaestatal", "5", economiaestatal },
                                           { "@economiaestatalobra", "5", economiaestatalobra },
                                           { "@economiaestatalindirecto", "5", economiaestatalindirecto },
                                           { "@municipaltotal", "5", municipaltotal },
                                           { "@municipalobra", "5", municipalobra },
                                           { "@municipalindirecto", "5", municipalindirecto },
                                           { "@contratadototal", "5", contratadototal },//new
                                           { "@contratadototalobra", "5", contratadototalobra },//new
                                           { "@contratadototalindirecto", "5", contratadototalindirecto },//new
                                           { "@contratadofederal", "5", contratadofederal },
                                           { "@contratadofederalobra", "5", contratadofederalobra },
                                           { "@contratadofederalindirecto", "5", contratadofederalindirecto },
                                           { "@contratadoestatal", "5", contratadoestatal },
                                           { "@contratadoestatalobra", "5", contratadoestatalobra },
                                           { "@contratadoestatalindirecto", "5", contratadoestatalindirecto },
                                           { "@contratadomunicipal", "5", contratadomunicipal },
                                           { "@contratadomunicipalobra", "5", contratadomunicipalobra },
                                           { "@contratadomunicipalindirecto", "5", contratadomunicipalindirecto },
                                           { "@idgeneral", "8", idgeneral },
                                           { "@idaccion", "12", idaccion },
                                           { "@idproyecto", "12", idproyecto },
                                           { "@idanio", "8", idanio },
                                           { "@anio", "12", anio },
                                           { "@idhijo", "8", idhijo },
                                           { "@economiapresupuestaltotal", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederal", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobra", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirecto", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatal", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobra", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirecto", "5", economiapresupuestalestatalindirecto },
                                           { "@economiapresupuestaltotalH", "5", economiapresupuestaltotal },//new kary 09032020
                                           { "@economiapresupuestalfederalH", "5", economiapresupuestalfederal },
                                           { "@economiapresupuestalfederalobraH", "5", economiapresupuestalfederalobra },
                                           { "@economiapresupuestalfederalindirectoH", "5", economiapresupuestalfederalindirecto },
                                           { "@economiapresupuestalestatalH", "5", economiapresupuestalestatal },
                                           { "@economiapresupuestalestatalobraH", "5", economiapresupuestalestatalobra },
                                           { "@economiapresupuestalestatalindirectoH", "5", economiapresupuestalestatalindirecto }
                                       };
            funUpdateDelete(tx, "Update_tblp_SP_Hfinanciero", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funEliminardatofinanciero(string iddatosfinanciero)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@iddatosfinanciero", "8", iddatosfinanciero } };
            funUpdateDelete(tx, "Delete_SP_financiero", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {

            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    #endregion
    #region Adjudicaciones_KGS 
    public virtual void funeliminaradjudicaciones(string idadjudicaciones,string idhadjudicaciones ,string idconvocatoria,string numconvocatoria, string idtipoadjudicacion, string idgeneral, string descripcion, string nolicitacion, string ncontrol, string nasiginvlic, string fechacaptura, string fechapublicacion,string archivooficoinv,string asignado, string fechainiciopub,string fechaterminopub, string observaciones, string fechamovimiento, string idusuario, string accion,string idtipoconvocatoria , string tipoconvocatoria, string idanio, string anio)
    {
        cnn.Open();
        SqlTransaction tx;
        
        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idadjudicaciones", "8", idadjudicaciones } };
            string[,] arrayse = new string[,] { { "@idhadjudicaciones", "8", idhadjudicaciones }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idconvocatoria", "8", idconvocatoria }, { "@numconvocatoria", "12", numconvocatoria },  { "@idtipoadjudicacion", "8", idtipoadjudicacion }, { "@idgeneral", "8", idgeneral }, { "@descripcion", "12", descripcion }, { "@nolicitacion", "12", nolicitacion }, { "@ncontrol", "12", ncontrol }, { "@nasiginvlic", "12", nasiginvlic }, { "@fechacaptura", "4", fechacaptura }, { "@fechapublicacion", "4", fechapublicacion }, { "@archivooficoinv", "12", archivooficoinv }, { "@asignado", "2", asignado }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaterminopub", "4", fechaterminopub }, { "@observaciones", "12", observaciones }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@idtipoconvocatoria", "8", idtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicaciones", arrayse);

            funUpdateDelete(tx, "Delete_tblp_SP_adjudicaciones", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertadjudicaciones(string idadjudicaciones, string idhadjudicaciones, string idconvocatoria, string numconvocatoria, string idtipoadjudicacion, string idgeneral, string descripcion, string nolicitacion, string ncontrol, string nasiginvlic, string fechacaptura, string fechapublicacion,string archivooficoinv,string asignado, string fechainiciopub,string fechaterminopub, string observaciones, string fechamovimiento, string idusuario, string accion, string idtipoconvocatoria, string tipoconvocatoria, string idanio, string anio)
    {
        cnn.Open();
        SqlTransaction tx;
        string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\adjudicaciones\oficioinvitacion\";


        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idadjudicaciones", "8", idadjudicaciones }, { "@idconvocatoria", "8", idconvocatoria }, { "@numconvocatoria", "12", numconvocatoria }, { "@idtipoadjudicacion", "8", idtipoadjudicacion }, { "@idgeneral", "8", idgeneral }, { "@descripcion", "12", descripcion }, { "@nolicitacion", "12", nolicitacion }, { "@ncontrol", "12", ncontrol }, { "@nasiginvlic", "12", nasiginvlic }, { "@fechacaptura", "4", fechacaptura }, { "@fechapublicacion", "4", fechapublicacion }, { "@archivooficoinv", "12", archivooficoinv }, { "@asignado", "2", asignado }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaterminopub", "4", fechaterminopub }, { "@observaciones", "12", observaciones }, { "@idtipoconvocatoria", "8", idtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblp_SP_adjudicaciones", array);
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayse = new string[,] { { "@idhadjudicaciones", "8", idhadjudicaciones }, { "@idadjudicaciones", "8", idgeneralNew }, { "@idconvocatoria", "8", idconvocatoria }, { "@numconvocatoria", "12", numconvocatoria }, { "@idtipoadjudicacion", "8", idtipoadjudicacion }, { "@idgeneral", "8", idgeneral }, { "@descripcion", "12", descripcion }, { "@nolicitacion", "12", nolicitacion }, { "@ncontrol", "12", ncontrol }, { "@nasiginvlic", "12", nasiginvlic }, { "@fechacaptura", "4", fechacaptura }, { "@fechapublicacion", "4", fechapublicacion }, { "@archivooficoinv", "12", archivooficoinv }, { "@asignado", "2", asignado }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaterminopub", "4", fechaterminopub }, { "@observaciones", "12", observaciones }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@idtipoconvocatoria", "8", idtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicaciones", arrayse);
            


                //=======================================================================================================================================================================================================================
                //====================================================================== para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================

                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones" }, { "@rutaarchivo", "12", Rutaraiz + archivooficoinv }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                //=======================================================================================================================================================================================================================
                //======================================================================   para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================
           
            

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateaadjudicaciones(string idadjudicaciones,string idhadjudicaciones, string idconvocatoria, string numconvocatoria, string idtipoadjudicacion, string idgeneral, string descripcion, string nolicitacion, string ncontrol, string nasiginvlic, string fechacaptura, string fechapublicacion, string archivooficoinv, string asignado, string fechainiciopub, string fechaterminopub, string observaciones, string fechamovimiento, string idusuario, string accion, string idtipoconvocatoria, string tipoconvocatoria, string idanio, string anio)
    {
        cnn.Open();
        SqlTransaction tx;
        string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\adjudicaciones\oficioinvitacion\";

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idadjudicaciones", "8", idadjudicaciones }, { "@idconvocatoria", "8", idconvocatoria }, { "@numconvocatoria", "12", numconvocatoria }, { "@idtipoadjudicacion", "8", idtipoadjudicacion }, { "@idgeneral", "8", idgeneral }, { "@descripcion", "12", descripcion }, { "@nolicitacion", "12", nolicitacion },  { "@nasiginvlic", "12", nasiginvlic }, { "@fechapublicacion", "4", fechapublicacion }, { "@archivooficoinv", "12", archivooficoinv },  { "@observaciones", "12", observaciones }, { "@idtipoconvocatoria", "8", idtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funUpdateDelete(tx, "Update_tblp_SP_adjudicaciones", array);


            string[,] arrayse = new string[,] { { "@idhadjudicaciones", "8", idhadjudicaciones }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idconvocatoria", "8", idconvocatoria }, { "@numconvocatoria", "12", numconvocatoria }, { "@idtipoadjudicacion", "8", idtipoadjudicacion }, { "@idgeneral", "8", idgeneral }, { "@descripcion", "12", descripcion }, { "@nolicitacion", "12", nolicitacion }, { "@ncontrol", "12", ncontrol }, { "@nasiginvlic", "12", nasiginvlic }, { "@fechacaptura", "4", fechacaptura }, { "@fechapublicacion", "4", fechapublicacion }, { "@archivooficoinv", "12", archivooficoinv }, { "@asignado", "2", asignado }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaterminopub", "4", fechaterminopub }, { "@observaciones", "12", observaciones }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@idtipoconvocatoria", "8", idtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicaciones", arrayse);


            //if (archivooficoinv != "")
            //{

                //=======================================================================================================================================================================================================================
                //====================================================================== para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================

                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones" }, { "@rutaarchivo", "12", Rutaraiz + archivooficoinv }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                //=======================================================================================================================================================================================================================
                //======================================================================   para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================
            //}



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region Convocatoria_KGS 
    public virtual void funeliminarconvocatoria(string idconvocatoria, string idHconvocatoria, string IDtipoconvocatoria, string tipoconvocatoria, string nconvocatoria, string fechaalta, string fechainiciopub, string fechamovimiento, string idusuario, string accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] pArreglo = new string[,] { { "@idconvocatoria", "8", idconvocatoria } };

            string[,] arrayse = new string[,] { { "@idHconvocatoria", "8", idHconvocatoria }, { "@idconvocatoria", "8", idconvocatoria }, { "@IDtipoconvocatoria", "8", IDtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@nconvocatoria", "12", nconvocatoria }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaalta", "4", fechaalta }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            funInsert(tx, "Insert_tblc_SP_Hconvocatoria", arrayse);

            funUpdateDelete(tx, "Delete_tblc_SP_convocatoria", pArreglo);
            tx.Commit();

        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertconvocatoria(string idconvocatoria, string idHconvocatoria, string IDtipoconvocatoria, string tipoconvocatoria, string nconvocatoria, string fechaalta, string fechainiciopub, string fechamovimiento, string idusuario, string accion, string idanio, string anio)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idconvocatoria", "8", idconvocatoria }, { "@IDtipoconvocatoria", "8", IDtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@nconvocatoria", "12", nconvocatoria }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaalta", "4", fechaalta }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblc_SP_convocatoria", array);
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayse = new string[,] { { "@idHconvocatoria", "8", idHconvocatoria }, { "@idconvocatoria", "8", idgeneralNew }, { "@IDtipoconvocatoria", "8", IDtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@nconvocatoria", "12", nconvocatoria }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaalta", "4", fechaalta }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblc_SP_Hconvocatoria", arrayse);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdateconvocatoria(string idconvocatoria, string idHconvocatoria, string IDtipoconvocatoria, string tipoconvocatoria, string nconvocatoria, string fechaalta, string fechainiciopub, string fechamovimiento, string idusuario, string accion,string idanio, string anio)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idconvocatoria", "8", idconvocatoria }, { "@IDtipoconvocatoria", "8", IDtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@nconvocatoria", "12", nconvocatoria }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaalta", "4", fechaalta }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funUpdateDelete(tx, "Update_tblc_SP_convocatoria", array);

            string[,] arrayse = new string[,] { { "@idHconvocatoria", "8", idHconvocatoria }, { "@idconvocatoria", "8", idconvocatoria }, { "@IDtipoconvocatoria", "8", IDtipoconvocatoria }, { "@tipoconvocatoria", "12", tipoconvocatoria }, { "@nconvocatoria", "12", nconvocatoria }, { "@fechainiciopub", "4", fechainiciopub }, { "@fechaalta", "4", fechaalta }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblc_SP_Hconvocatoria", arrayse);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();
        }

    }
    #endregion


    #region Contratos_ADG
    public Boolean funinsertarcontrato(string idcontrato, string numcontrato, string idtipo, string nombretipo, string descripcioncontrato, string numlicitacion, string fechacontrato, string fechainiciocontrato, string fechaterminocontrato, string idempresa, string empresa,string montocontratosiva, string montocontratoiva, string montocontratociva, string contratadototalindirecto, string porceAnticipo, string anticiposiva, string anticipoiva, string anticipociva, string idagremiado, string agremiado, string PorceAgremiado, string fechavencimientocontrato, string cuentaconcatalogo, string rutaarchivo, string observaciones,GridView grvmetas,GridView grvconvenios, string vigilancia, string beneficiosocial,string nomina, string sinanticipo,string idusuario, string adeudum,string fechaadeudum, GridView grvobras,string lbleliminarobras,string lbleliminarmeta,string lbleliminarconvenio,string idanio, string anio, string idstatuscontrato,string statuscontrato,string renunciaanticipo, string eliminarfuentes,string idmodalidad, string modalidad, string nombrearchivoCO, string nombrearchivoCC)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            String RutaContratro = @"C:\inetpub\wwwroot\SIOP\Uploads\contratos\";
		   
            string[,] array = new string[,] { { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@montocontratosiva", "5", montocontratosiva }, { "@montocontratoiva", "5", montocontratoiva }, { "@montocontratociva", "5", montocontratociva }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@porceAnticipo", "5", porceAnticipo }, { "@anticiposiva", "5", anticiposiva }, { "@anticipoiva", "5", anticipoiva }, { "@anticipociva", "5", anticipociva }, { "@idagremiado", "8", idagremiado }, { "@agremiado", "12", agremiado }, { "@PorceAgremiado", "5", PorceAgremiado }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@vigilancia", "2", vigilancia }, { "@beneficiosocial", "2", beneficiosocial }, { "@nomina", "2", nomina }, { "@sinanticipo", "2", sinanticipo }, { "@adeudum", "12", adeudum }, { "@fechaadeudum", "4", fechaadeudum }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, {"@idstatuscontrato", "8", idstatuscontrato }, {"@statuscontrato", "12", statuscontrato }, { "@renunciaanticipo", "2", renunciaanticipo }, { "@idmodalidad", "8", idmodalidad }, { "@modalidad", "12", modalidad }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@nombrearchivoCO", "12", nombrearchivoCO }, { "@nombrearchivoCC", "12", nombrearchivoCC } };
            funInsert(tx, "Insert_tblp_CON_contrato", array);
            string idcontratoNew = IDNewx.ToString();
            //string[,] arrayH = new string[,] { { "@idhistorialcontrato", "8", "0" }, { "@idcontrato", "8", idgeneralNew }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@contratadototal", "5", contratadototal }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@anticipo", "5", anticipo }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@accion", "12", "A" } };
            string[,] arrayH = new string[,] { { "@idhistorialcontrato", "8", "0" }, { "@idcontrato", "8", idcontratoNew }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@montocontratosiva", "5", montocontratosiva }, { "@montocontratoiva", "5", montocontratoiva }, { "@montocontratociva", "5", montocontratociva }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@porceAnticipo", "5", porceAnticipo }, { "@anticiposiva", "5", anticiposiva }, { "@anticipoiva", "5", anticipoiva }, { "@anticipociva", "5", anticipociva }, { "@idagremiado", "8", idagremiado }, { "@agremiado", "12", agremiado }, { "@PorceAgremiado", "5", PorceAgremiado }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@vigilancia", "2", vigilancia }, { "@beneficiosocial", "2", beneficiosocial }, { "@nomina", "2", nomina }, { "@sinanticipo", "2", sinanticipo }, { "@adeudum", "12", adeudum }, { "@fechaadeudum", "4", fechaadeudum }, { "@accion", "12", "AC" }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) },{ "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@idstatuscontrato", "8", idstatuscontrato }, { "@statuscontrato", "12", statuscontrato }, { "@renunciaanticipo", "2", renunciaanticipo }, { "@idmodalidad", "8", idmodalidad }, { "@modalidad", "12", modalidad }, { "@nombrearchivoCO", "12", nombrearchivoCO }, { "@nombrearchivoCC", "12", nombrearchivoCC } };
            funInsert(tx, "Insert_tblp_CON_Hcontrato", arrayH);
            //======================================================================================================================================================================
            //======================================================================actualizar obras canceladas=====================================================================
            //======================================================================================================================================================================
            if (idstatuscontrato=="9")
            {
                string[,] arrayCC = new string[,] { { "@idcontrato", "8", idcontratoNew } };
                funUpdateDelete(tx, "Update_tblp_CON_idgeneralcancelada", arrayCC);

            }
            

            //======================================================================================================================================================================
            //======================================================================actualizar obras canceladas=====================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar obras========================================================================
            //======================================================================================================================================================================
            Int16 ii = 0;
            string idgeneral = "";
            string montoanticipoporaccion = "0";
            string montodecontratoobra = "";
            string Idfuentefinanciamiento = "";
            string idtipofuente = "";
            string nombreobra = "";
            string cvemunicipio = "0";
            string beneficiario = "0";
            string tipocontratoaccion = "0";
            string tipoobra = "0";
            string organismo = "0";
            string idstatus = "0";

            foreach (GridViewRow row in grvobras.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos
                    idgeneral = Convert.ToString(grvobras.DataKeys[ii].Values[0]);
                    nombreobra = Convert.ToString(grvobras.DataKeys[ii].Values[1]);
                    montodecontratoobra = Convert.ToString(grvobras.DataKeys[ii].Values[5]);
                    Idfuentefinanciamiento = Convert.ToString(grvobras.DataKeys[ii].Values[6]);
                    idtipofuente = Convert.ToString(grvobras.DataKeys[ii].Values[8]);
                    cvemunicipio = Convert.ToString(grvobras.DataKeys[ii].Values[10]);
                    beneficiario = Convert.ToString(grvobras.DataKeys[ii].Values[11]);
                    tipocontratoaccion = Convert.ToString(grvobras.DataKeys[ii].Values[12]);
                    tipoobra = Convert.ToString(grvobras.DataKeys[ii].Values[13]);
                    organismo = Convert.ToString(grvobras.DataKeys[ii].Values[14]);
                    idstatus = Convert.ToString(grvobras.DataKeys[ii].Values[15]);

                    string[,] arrayC = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontratoNew }, { "@numcontrato", "12", numcontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@montoanticipoporaccion", "5", montoanticipoporaccion }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa } };
                    funUpdateDelete(tx, "Update_tblp_CON_idgeneral", arrayC);
                    //actualizamos el monto de contrato de obra la parte financiera
                    string[,] arrayF = new string[,] { { "@idgeneral", "8", idgeneral }, { "@montodecontratoobra", "5", montodecontratoobra }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@idtipofuente", "8", idtipofuente } };
                    funUpdateDelete(tx, "tblp_financiero_idgeneralfuente", arrayF);




                    //para generar el time line contratos
                    string[,] arraytm = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", anio }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", idcontratoNew }, { "@numcontrato", "12", numcontrato }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@empresa", "12", empresa }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2019" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", "0" }, { "@estatus", "12", "" }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneral }, { "@fecha_alta", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@tipo_proceso", "12", "CON" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
                    funInsert(tx, "Insert_Timelineobrapublica", arraytm);
                    //para generar el time line contratos




                    //=======================================================================================================================================================================================================================
                    //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra y generar los respaldo=====================================================================
                    //=======================================================================================================================================================================================================================

                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontratoNew }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + rutaarchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "true" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);




                    //=======================================================================================================================================================================================================================
                    //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra y generar los respaldo =====================================================================
                    //=======================================================================================================================================================================================================================



                }
                ii += 1;

                if (nombrearchivoCO != "") //catalogo de obras
                {
                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontratoNew }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + nombrearchivoCO }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
                }

                if (nombrearchivoCC != "") //catalogo concepto
                {
                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontratoNew }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + nombrearchivoCC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
                }


            }
            if (lbleliminarobras!="")
            {
                string[,] pArreglo = new string[,] { { "@actualizarobras", "12", lbleliminarobras.ToString() } };
                funUpdateDelete(tx, "Update_tblp_CON_obras", pArreglo);
            }
            
            //======================================================================================================================================================================
            //======================================================================agregar actualizar obras========================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar metas========================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            Int64 idcontratonew = 0;
            string numcontratonew = "";
            string metasnew = "";
            string metaprogramadanew = "";
            string idunidadmedidanew = "";
            string unidadmedidanew = "";
            foreach (GridViewRow row in grvmetas.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvmetas.DataKeys[i].Values[0]) <= 0)
                    {
                        idcontratonew = Convert.ToInt64(idcontratoNew);
                        numcontratonew = Convert.ToString(grvmetas.DataKeys[i].Values[2]);
                        metaprogramadanew= Convert.ToString(grvmetas.DataKeys[i].Values[3]);
                        idunidadmedidanew= Convert.ToString(grvmetas.DataKeys[i].Values[4]);
                        unidadmedidanew = Convert.ToString(grvmetas.DataKeys[i].Values[5]);
                        metasnew = Convert.ToString(grvmetas.DataKeys[i].Values[6]);
                        funInsertmetas(tx, idcontratonew.ToString(), numcontratonew.ToString(), metaprogramadanew.ToString(), idunidadmedidanew.ToString(), unidadmedidanew.ToString(), metasnew.ToString(), "0");
                    }
                   
                }
                i += 1;
            }
            //*************************eliminar metas**********************
            //funDeletemetas(lbleliminarmeta);
            //======================================================================================================================================================================
            //======================================================================agregar actualizar metas========================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar convenios====================================================================
            //======================================================================================================================================================================
            Int16 i1 = 0;
            Int64 idcontratoC = 0;
            string numcontratoC = "";
            string numconvenioC = "";
            decimal convenioobraC = 0;
            decimal convenioindirectoC = 0;
            Int64 idgeneralconvenio = 0;
            string rutaarchivoC = "";
            foreach (GridViewRow row in grvconvenios.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvconvenios.DataKeys[i1].Values[0]) <= 0)
                    {
                        idcontratoC = Convert.ToInt64(idcontratoNew);
                        numcontratoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[2]);
                        numconvenioC = Convert.ToString(grvconvenios.DataKeys[i1].Values[3]);
                        convenioobraC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[4]);
                        convenioindirectoC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[5]);
                        idgeneralconvenio= Convert.ToInt64(grvconvenios.DataKeys[i1].Values[6]);
                        rutaarchivoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[7]);
                        funInsertconvenio(tx, idcontratoC.ToString(), numcontratoC.ToString(), numconvenioC.ToString(), convenioobraC.ToString(), convenioindirectoC.ToString(), idgeneralconvenio.ToString(), rutaarchivoC.ToString());
                    }

                }
                i1 += 1;
            }
            //**************************eliminar convenios************************
            //funDeleteconvenios(lbleliminarconvenio);
            //======================================================================================================================================================================
            //======================================================================agregar actualizar convenios====================================================================
            //======================================================================================================================================================================

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertmetas(SqlTransaction tx, string idcontratonew, string numcontratonew, string metaprogramadanew, string idunidadmedidanew, string unidadmedidanew, string metasnew, string metaalcanzadanew)
    {
        try
        {
            string[,] array = new string[,] { { "@idcontratonew", "8", idcontratonew }, { "@numcontratonew", "12", numcontratonew }, { "@metaprogramadanew", "5", metaprogramadanew }, { "@metaalcanzadanew", "5", metaalcanzadanew }, { "@idunidadmedidanew", "8", idunidadmedidanew }, { "@unidadmedidanew", "12", unidadmedidanew }, { "@metasnew", "12", metasnew } };
            funInsert(tx, "Insert_tblp_CON_metas", array);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }
      

    }
    public Boolean funInsertconvenio(SqlTransaction tx, string idcontrato, string numcontrato, string numconvenio, string convenioobra, string convenioindirecto,string idgeneral,string rutaarchivo)
    {
        try
        {
            string[,] array = new string[,] { { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@numconvenio", "12", numconvenio }, { "@convenioobra", "5", convenioobra }, { "@convenioindirecto", "5", convenioindirecto } , { "@idgeneral", "8", idgeneral }, { "@rutaarchivo", "12", rutaarchivo } };
            funInsert(tx, "Insert_tblp_CON_convenios", array);

            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\contratos\";
            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            if (rutaarchivo != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_CON_contratos_tblp_CON_convenios " }, { "@rutaarchivo", "12", Rutaraiz + rutaarchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================


            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }
        

    }
    public Boolean funupdatecontrato(string idcontrato, string numcontrato, string idtipo, string nombretipo, string descripcioncontrato, string numlicitacion, string fechacontrato, string fechainiciocontrato, string fechaterminocontrato, string idempresa, string empresa, string montocontratosiva, string montocontratoiva, string montocontratociva, string contratadototalindirecto, string porceAnticipo, string anticiposiva, string anticipoiva, string anticipociva, string idagremiado, string agremiado, string PorceAgremiado, string fechavencimientocontrato, string cuentaconcatalogo, string rutaarchivo, string observaciones, GridView grvmetas, GridView grvconvenios, string vigilancia, string beneficiosocial, string nomina, string sinanticipo,string idusuario, string adeudum, string fechaadeudum, GridView grvobras, string lbleliminarobras, string lbleliminarmeta, string lbleliminarconvenio, string idanio, string anio, string idstatuscontrato, string statuscontrato, string renunciaanticipo,string eliminarfuentes, string idmodalidad, string modalidad, string nombrearchivoCO, string nombrearchivoCC)
    {
        cnn.Open();
        SqlTransaction tx;
        String RutaContratro = @"C:\inetpub\wwwroot\SIOP\Uploads\contratos\";

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@montocontratosiva", "5", montocontratosiva }, { "@montocontratoiva", "5", montocontratoiva }, { "@montocontratociva", "5", montocontratociva }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@porceAnticipo", "5", porceAnticipo }, { "@anticiposiva", "5", anticiposiva }, { "@anticipoiva", "5", anticipoiva }, { "@anticipociva", "5", anticipociva }, { "@idagremiado", "8", idagremiado }, { "@agremiado", "12", agremiado }, { "@PorceAgremiado", "5", PorceAgremiado }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@vigilancia", "2", vigilancia }, { "@beneficiosocial", "2", beneficiosocial }, { "@nomina", "2", nomina }, { "@sinanticipo", "2", sinanticipo },{ "@adeudum", "12", adeudum }, { "@fechaadeudum", "4", fechaadeudum }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@idstatuscontrato", "8", idstatuscontrato }, { "@statuscontrato", "12", statuscontrato }, { "@renunciaanticipo", "2", renunciaanticipo }, { "@idmodalidad", "8", idmodalidad }, { "@modalidad", "12", modalidad }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@nombrearchivoCO", "12", nombrearchivoCO }, { "@nombrearchivoCC", "12", nombrearchivoCC } };
            funUpdateDelete(tx, "Update_tblp_CON_contrato", array);

            //string[,] arrayH = new string[,] { { "@idhistorialcontrato", "8", "0" }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@contratadototal", "5", contratadototal }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@anticipo", "5", anticipo }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@accion", "12", "M" } };
            string[,] arrayH = new string[,] { { "@idhistorialcontrato", "8", "0" }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@montocontratosiva", "5", montocontratosiva }, { "@montocontratoiva", "5", montocontratoiva }, { "@montocontratociva", "5", montocontratociva }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@porceAnticipo", "5", porceAnticipo }, { "@anticiposiva", "5", anticiposiva }, { "@anticipoiva", "5", anticipoiva }, { "@anticipociva", "5", anticipociva }, { "@idagremiado", "8", idagremiado }, { "@agremiado", "12", agremiado }, { "@PorceAgremiado", "5", PorceAgremiado }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@vigilancia", "2", vigilancia }, { "@beneficiosocial", "2", beneficiosocial }, { "@nomina", "2", nomina }, { "@sinanticipo", "2", sinanticipo }, { "@adeudum", "12", adeudum }, { "@fechaadeudum", "4", fechaadeudum },{ "@accion", "12", "MC" }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@idstatuscontrato", "8", idstatuscontrato }, { "@statuscontrato", "12", statuscontrato }, { "@renunciaanticipo", "2", renunciaanticipo }, { "@idmodalidad", "8", idmodalidad }, { "@modalidad", "12", modalidad }, { "@nombrearchivoCO", "12", nombrearchivoCO }, { "@nombrearchivoCC", "12", nombrearchivoCC } };
            funInsert(tx, "Insert_tblp_CON_Hcontrato", arrayH);
            //======================================================================================================================================================================
            //======================================================================actualizar obras canceladas=====================================================================
            //======================================================================================================================================================================
            if (idstatuscontrato == "9")
            {
                string[,] arrayCC = new string[,] { { "@idcontrato", "8", idcontrato } };
                funUpdateDelete(tx, "Update_tblp_CON_idgeneralcancelada", arrayCC);

            }
            //======================================================================================================================================================================
            //======================================================================actualizar obras canceladas=====================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar obras========================================================================
            //======================================================================================================================================================================
            Int16 ii = 0;
            string idgeneral = "";
            string montoanticipoporaccion = "0";
            string montodecontratoobra = "";
            string Idfuentefinanciamiento = "";
            string idtipofuente = "";
                      
            string nombreobra = "";
            string cvemunicipio = "0";
            string beneficiario = "0";
            string tipocontratoaccion = "0";
            string tipoobra = "0";
            string organismo = "0";
            string idstatus = "0";



            foreach (GridViewRow row in grvobras.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos
                    idgeneral = Convert.ToString(grvobras.DataKeys[ii].Values[0]);

                    idgeneral = Convert.ToString(grvobras.DataKeys[ii].Values[0]);
                    nombreobra = Convert.ToString(grvobras.DataKeys[ii].Values[1]);
                    montodecontratoobra = Convert.ToString(grvobras.DataKeys[ii].Values[5]);
                    Idfuentefinanciamiento = Convert.ToString(grvobras.DataKeys[ii].Values[6]);
                    idtipofuente = Convert.ToString(grvobras.DataKeys[ii].Values[8]);
                    cvemunicipio = Convert.ToString(grvobras.DataKeys[ii].Values[10]);
                    beneficiario = Convert.ToString(grvobras.DataKeys[ii].Values[11]);
                    tipocontratoaccion = Convert.ToString(grvobras.DataKeys[ii].Values[12]);
                    tipoobra = Convert.ToString(grvobras.DataKeys[ii].Values[13]);
                    organismo = Convert.ToString(grvobras.DataKeys[ii].Values[14]);
                    idstatus = Convert.ToString(grvobras.DataKeys[ii].Values[15]);



                    //// montoanticipoporaccion = Convert.ToString(grvobras.DataKeys[ii].Values[5]);
                    //montodecontratoobra = Convert.ToString(grvobras.DataKeys[ii].Values[5]);
                    //Idfuentefinanciamiento = Convert.ToString(grvobras.DataKeys[ii].Values[6]);
                    //idtipofuente = Convert.ToString(grvobras.DataKeys[ii].Values[8]);
                    string[,] arrayC = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@montoanticipoporaccion", "5", montoanticipoporaccion }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa } };
                    //string[,] arrayC = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato } };
                    funUpdateDelete(tx, "Update_tblp_CON_idgeneral", arrayC);
                    //actualizamos el monto de contrato de obra la parte financiera
                    string[,] arrayF = new string[,] { { "@idgeneral", "8", idgeneral }, { "@montodecontratoobra", "5", montodecontratoobra }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@idtipofuente", "8", idtipofuente } };
                    funUpdateDelete(tx, "tblp_financiero_idgeneralfuente", arrayF);


                    //=======================================================================================================================================================================================================================
                    //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra =====================================================================
                    //=======================================================================================================================================================================================================================

                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + rutaarchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                    //=======================================================================================================================================================================================================================
                    //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra =====================================================================
                    //=======================================================================================================================================================================================================================

                
                }
                ii += 1;

                if (nombrearchivoCO != "")
                {
                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + nombrearchivoCO }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
                }

                if (nombrearchivoCC != "")
                {
                    string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontratoaccion }, { "@municipio", "8", cvemunicipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiario }, { "@inversion", "5", montodecontratoobra }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", nombreobra }, { "@contratista", "8", idempresa }, { "@estatus", "8", idstatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", anio }, { "@idobra", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", RutaContratro + nombrearchivoCC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                    funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
                }





            }
            if (lbleliminarobras != "")
            {
                string[,] pArreglo = new string[,] { { "@actualizarobras", "12", lbleliminarobras.ToString() } };
                funUpdateDelete(tx, "Update_tblp_CON_obras", pArreglo);
            }
            if (eliminarfuentes != "")
            {
                string[] str = eliminarfuentes.Split(',');
                for (int x = 0; x < str.Length; x++)
                {
                    string xx = str[x].ToString();
                    string[] xxx = xx.Split('|');
                    string[,] arrayX = new string[,] { { "@idgeneral", "8", xxx[0] }, { "@montodecontratoobra", "5", "0" }, { "@Idfuentefinanciamiento", "8", xxx[1] }, { "@idtipofuente", "8", xxx[2] } };
                    funUpdateDelete(tx, "tblp_financiero_idgeneralfuente", arrayX);
                }
            }
            //======================================================================================================================================================================
            //======================================================================agregar actualizar obras========================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar metas========================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            ExecCommand ins;
            Int64 idmetacontrato = 0;
            Int64 idcontratonew = 0;
            string numcontratonew = "";
            decimal metaprogramadanew = 0;
            decimal metaalcanzadanew = 0;
            Int64 idunidadmedidanew = 0;
            string unidadmedidanew = "";
            string metasnew = "";
            foreach (GridViewRow row in grvmetas.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvmetas.DataKeys[i].Values[0]) <= 0)
                    {
                        idmetacontrato = Convert.ToInt64(grvmetas.DataKeys[i].Values[0]);
                        idcontratonew = Convert.ToInt64(grvmetas.DataKeys[i].Values[1]);
                        numcontratonew = Convert.ToString(grvmetas.DataKeys[i].Values[2]);
                        metaprogramadanew = Convert.ToDecimal(grvmetas.DataKeys[i].Values[3]);
                        metaalcanzadanew = 0;
                        idunidadmedidanew = Convert.ToInt64(grvmetas.DataKeys[i].Values[4]);
                        unidadmedidanew = Convert.ToString(grvmetas.DataKeys[i].Values[5]);
                        metasnew = Convert.ToString(grvmetas.DataKeys[i].Values[6]);
                        funInsertmetas(tx, idcontratonew.ToString(), numcontratonew.ToString(), metaprogramadanew.ToString(), idunidadmedidanew.ToString(), unidadmedidanew.ToString(), metasnew.ToString(), metaalcanzadanew.ToString());
                        //funUpdatemetas(tx, idmetacontrato.ToString(), idcontratonew.ToString(), numcontratonew.ToString(), metaprogramadanew.ToString(), metaalcanzadanew.ToString(), idunidadmedidanew.ToString(), unidadmedidanew.ToString(), metasnew.ToString());
                    }
                    else
                    {
                        idmetacontrato = Convert.ToInt64(grvmetas.DataKeys[i].Values[0]);
                        idcontratonew = Convert.ToInt64(grvmetas.DataKeys[i].Values[1]);
                        numcontratonew = Convert.ToString(grvmetas.DataKeys[i].Values[2]);
                        metaprogramadanew = Convert.ToDecimal(grvmetas.DataKeys[i].Values[3]);
                        metaalcanzadanew = 0;
                        idunidadmedidanew = Convert.ToInt64(grvmetas.DataKeys[i].Values[4]);
                        unidadmedidanew = Convert.ToString(grvmetas.DataKeys[i].Values[5]);
                        metasnew = Convert.ToString(grvmetas.DataKeys[i].Values[6]);
                        funUpdatemetas(tx, idmetacontrato.ToString(), idcontratonew.ToString(), numcontratonew.ToString(), metaprogramadanew.ToString(), metaalcanzadanew.ToString(), idunidadmedidanew.ToString(), unidadmedidanew.ToString(), metasnew.ToString());
                    }

                }
                i += 1;
            }

            //======================================================================================================================================================================
            //======================================================================agregar actualizar metas========================================================================
            //======================================================================================================================================================================

            //======================================================================================================================================================================
            //======================================================================agregar actualizar convenios====================================================================
            //======================================================================================================================================================================
            Int16 i1 = 0;
            Int64 idconveniocontratoC = 0;
            Int64 idcontratoC = 0;
            string numcontratoC = "";
            string numconvenioC = "";
            decimal convenioobraC = 0;
            decimal convenioindirectoC = 0;
            Int64 idgeneralC = 0;
            string rutaarchivoC = "";
            foreach (GridViewRow row in grvconvenios.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvconvenios.DataKeys[i1].Values[0]) <= 0)
                    {
                        idconveniocontratoC = Convert.ToInt64(grvconvenios.DataKeys[i1].Values[0]);
                        idcontratoC = Convert.ToInt64(grvconvenios.DataKeys[i1].Values[1]);
                        numcontratoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[2]);
                        numconvenioC = Convert.ToString(grvconvenios.DataKeys[i1].Values[3]);
                        convenioobraC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[4]);
                        convenioindirectoC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[5]);
                        idgeneralC= Convert.ToInt64(grvconvenios.DataKeys[i1].Values[6]);
                        rutaarchivoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[7]);
                        funInsertconvenio(tx, idcontratoC.ToString(), numcontratoC.ToString(), numconvenioC.ToString(), convenioobraC.ToString(), convenioindirectoC.ToString(),idgeneralC.ToString(), rutaarchivoC.ToString());
                    }
                    else
                    {
                        idconveniocontratoC = Convert.ToInt64(grvconvenios.DataKeys[i1].Values[0]);
                        idcontratoC = Convert.ToInt64(grvconvenios.DataKeys[i1].Values[1]);
                        numcontratoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[2]);
                        numconvenioC = Convert.ToString(grvconvenios.DataKeys[i1].Values[3]);
                        convenioobraC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[4]);
                        convenioindirectoC = Convert.ToDecimal(grvconvenios.DataKeys[i1].Values[5]);
                        idgeneralC = Convert.ToInt64(grvconvenios.DataKeys[i1].Values[6]);
                        rutaarchivoC = Convert.ToString(grvconvenios.DataKeys[i1].Values[7]);
                        funUpdateconvenio(tx, idconveniocontratoC.ToString(), idcontratoC.ToString(), numcontratoC.ToString(), numconvenioC.ToString(), convenioobraC.ToString(), convenioindirectoC.ToString(), idgeneralC.ToString(), rutaarchivoC.ToString());
                    }

                }
                i1 += 1;
            }
           

            //======================================================================================================================================================================
            //======================================================================agregar actualizar convenios====================================================================
            //======================================================================================================================================================================
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funUpdatemetas(SqlTransaction tx, string idmetacontrato, string idcontratonew, string numcontratonew, string metaprogramadanew, string metaalcanzadanew, string idunidadmedidanew, string unidadmedidanew, string metasnew)
    {
        try
        {
            string[,] array = new string[,] { { "@idmetacontrato", "8", idmetacontrato }, { "@idcontratonew", "8", idcontratonew }, { "@numcontratonew", "12", numcontratonew }, { "@metaprogramadanew", "5", metaprogramadanew }, { "@metaalcanzadanew", "5", metaalcanzadanew }, { "@idunidadmedidanew", "8", idunidadmedidanew }, { "@unidadmedidanew", "12", unidadmedidanew }, { "@metasnew", "12", metasnew } };
            funUpdateDelete(tx, "Update_tblp_CON_metas", array);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }
       

    }
    public Boolean funUpdateconvenio(SqlTransaction tx, string idconveniocontrato, string idcontrato, string numcontrato, string numconvenio, string convenioobra, string convenioindirecto,string idgeneral,string rutaarchivo)
    {
        try
        {
            string[,] array = new string[,] { { "@idconveniocontrato", "8", idconveniocontrato }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@numconvenio", "12", numconvenio }, { "@convenioobra", "5", convenioobra }, { "@convenioindirecto", "5", convenioindirecto }, { "@idgeneral", "8", idgeneral }, { "@rutaarchivo", "12", rutaarchivo } };
            funUpdateDelete(tx, "Update_tblp_CON_convenios", array);


            string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\contratos\";
            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            if (rutaarchivo != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_CON_contratos_tblp_CON_convenios " }, { "@rutaarchivo", "12", Rutaraiz + rutaarchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================



            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }
        
    }
    public virtual void funDeletemetas(string eliminarmetas)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@eliminarmetas", "12", eliminarmetas.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_CON_metas", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
        }
        finally
        {
            cnn.Close();

        }

    }
    public virtual void funDeleteconvenios(string eliminarconvenios)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@eliminarconvenios", "12", eliminarconvenios.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_CON_convenios", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funeliminarcontrato(string idcontrato, string numcontrato, string idtipo, string nombretipo, string descripcioncontrato, string numlicitacion, string fechacontrato, string fechainiciocontrato, string fechaterminocontrato, string idempresa, string empresa, string contratadototal, string contratadototalindirecto, string anticipo, string fechavencimientocontrato, string cuentaconcatalogo, string rutaarchivo, string observaciones,string accion,string idusuario, string organismo, string tipocontrato, string municipio, string tipoobra, string beneficiados, string objeto, string idobra, string idproyectoAPI, string ejercicio, string  estatus)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            //string[,] array = new string[,] { { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@contratadototal", "5", contratadototal }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@anticipo", "5", anticipo }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones } };
            //funInsert(tx, "Insert_tblp_CON_contrato", array);
           // string idgeneralNew = IDNewx.ToString();
            string[,] arrayH = new string[,] { { "@idhistorialcontrato", "8", "0" }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", numcontrato }, { "@idtipo", "8", idtipo }, { "@nombretipo", "12", nombretipo }, { "@descripcioncontrato", "12", descripcioncontrato }, { "@numlicitacion", "12", numlicitacion }, { "@fechacontrato", "4", fechacontrato }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechaterminocontrato }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@contratadototal", "5", contratadototal }, { "@contratadototalindirecto", "5", contratadototalindirecto }, { "@anticipo", "5", anticipo }, { "@fechavencimientocontrato", "4", fechavencimientocontrato }, { "@cuentaconcatalogo", "2", cuentaconcatalogo }, { "@rutaarchivo", "12", rutaarchivo }, { "@observaciones", "12", observaciones }, { "@accion", "12", accion }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_CON_Hcontrato", arrayH);
            string[,] pArreglo = new string[,] { { "@idcontrato", "8", idcontrato.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_CON_contrato", pArreglo);




            //=======================================================================================================================================================================================================================
            //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra =====================================================================
            //=======================================================================================================================================================================================================================

            //OJO verificar campos pendientes
            
            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "3fa85f64-5717-4562-b3fc-2c963f66afa6" }, { "@organismo", "8", organismo }, { "@tipocontrato", "8", tipocontrato }, { "@municipio", "8", municipio }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", tipoobra }, { "@beneficiados", "8", beneficiados }, { "@inversion", "5", contratadototal }, { "@numerolicitacion", "12", numlicitacion }, { "@periodoinicioejecucion", "4", fechainiciocontrato }, { "@periodofinalejecucion", "4", fechaterminocontrato }, { "@fechaconclucion", "4", fechaterminocontrato }, { "@objeto", "12", objeto }, { "@contratista", "8", idempresa }, { "@estatus", "8", estatus }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "0b616d5d-5a2f-4b6c-b64f-b0498d50362d" }, { "@mongoid", "12", "NULLL" }, { "@ejercicio", "8", ejercicio }, { "@idobra", "8", idobra }, { "@idcontrato", "8", idcontrato }, { "@nocontrato", "12", numcontrato }, { "@procesotabla", "12", "tblp_CON_contratos" }, { "@rutaarchivo", "12", rutaarchivo }, { "@accion", "12", "E" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", idproyectoAPI }, { "@API", "2", "true" } };
            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

            //=======================================================================================================================================================================================================================
            //======================================================================  inserta en la tabla temporal para la API y valida qeu debe llevar la obra =====================================================================
            //=======================================================================================================================================================================================================================

         





            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funinsertarreprogramacion(string motivoreprogramacion, string fechainicio, string fechafin, string idcontrato, string contrato, string archivoreprogramacion, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@motivoreprogramacion", "12", motivoreprogramacion }, { "@fechainicio", "4", fechainicio }, { "@fechafin", "4", fechafin }, { "@idcontrato", "8", idcontrato }, { "@contrato", "12", contrato }, { "@archivoreprogramacion", "12", archivoreprogramacion }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_CON_reprogramacion", array);
            string idreprogramacion = IDNewx.ToString();
            
            string[,] arrayH = new string[,] { { "@idreprogramacion", "8", idreprogramacion },{ "@motivoreprogramacion", "12", motivoreprogramacion }, { "@fechainicio", "4", fechainicio }, { "@fechafin", "4", fechafin }, { "@idcontrato", "8", idcontrato }, { "@contrato", "12", contrato }, { "@archivoreprogramacion", "12", archivoreprogramacion }, { "@accion", "12", "A" }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_HCON_reprogramacion", arrayH);


            string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\Archivosreprogramaciones\";
            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            if (archivoreprogramacion != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_CON_contratos_tblp_CON_convenios " }, { "@rutaarchivo", "12", Rutaraiz + archivoreprogramacion }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================
            

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }


    }
    public Boolean funupdatereprogramacion(string idreprogramacion, string motivoreprogramacion, string fechainicio, string fechafin, string idcontrato, string contrato, string archivoreprogramacion, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idreprogramacion", "8", idreprogramacion } ,{ "@motivoreprogramacion", "12", motivoreprogramacion }, { "@fechainicio", "4", fechainicio }, { "@fechafin", "4", fechafin }, { "@idcontrato", "8", idcontrato }, { "@contrato", "12", contrato }, { "@archivoreprogramacion", "12", archivoreprogramacion }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funUpdateDelete(tx, "Update_tblp_CON_reprogramacion", array);
           // string idreprogramacion = IDNewx.ToString();

            string[,] arrayH = new string[,] { { "@idreprogramacion", "8", idreprogramacion }, { "@motivoreprogramacion", "12", motivoreprogramacion }, { "@fechainicio", "4", fechainicio }, { "@fechafin", "4", fechafin }, { "@idcontrato", "8", idcontrato }, { "@contrato", "12", contrato }, { "@archivoreprogramacion", "12", archivoreprogramacion }, { "@accion", "12", "M" }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_HCON_reprogramacion", arrayH);

            string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\Archivosreprogramaciones\";
            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            if (archivoreprogramacion != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_CON_contratos_tblp_CON_convenios " }, { "@rutaarchivo", "12", Rutaraiz + archivoreprogramacion }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }


    }
    public virtual void funDeletereprogramacion(string idreprogramacion, string motivoreprogramacion, string fechainicio, string fechafin, string idcontrato, string contrato, string archivoreprogramacion, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] arrayH = new string[,] { { "@idreprogramacion", "8", idreprogramacion }, { "@motivoreprogramacion", "12", motivoreprogramacion }, { "@fechainicio", "4", fechainicio }, { "@fechafin", "4", fechafin }, { "@idcontrato", "8", idcontrato }, { "@contrato", "12", contrato }, { "@archivoreprogramacion", "12", archivoreprogramacion }, { "@accion", "12", "E" }, { "@idusuario", "12", idusuario }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_HCON_reprogramacion", arrayH);

            string[,] pArreglo = new string[,] { { "@idreprogramacion", "8", idreprogramacion } };
            funUpdateDelete(tx, "Delete_tblp_CON_idreprogramacion", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertempresa(string empresa)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@empresa", "12", empresa } };
            funInsert(tx, "Insert_tblp_CON_empresanew", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();

        }


    }


    public Boolean funupdatetemporal(string idtemporal, string idproyectoAPI)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idtemporal", "", idtemporal }, { "@idproyectoAPI", "8", idproyectoAPI }, { "@fechaenvio", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@enviadoapi", "2", "true" }  };
            funUpdateDelete(tx, "Update_tblp_TemporalDocumentos", array);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funupdatetemporalelimina(string idtemporal)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idtemporal", "", idtemporal }, { "@fechaeliminar", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funUpdateDelete(tx, "[Update_tblp_TemporalDocumentosfechaeliminar]", array);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }




    public Boolean funupdateavance(string idvisitaobra, string idavanceAPI)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idvisitaobra", "", idvisitaobra }, { "@idavanceAPI", "8", idavanceAPI }, { "@fechaenvio", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }};
            funUpdateDelete(tx, "Update_tblp_visitaavanceAPI", array);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    #endregion
    #region Licitaciones_KGS 
    public virtual void funeliminarlicitaciones(string idlicitacionfecha, string idhlicitacionfecha, string idadjudicaciones, string fechainicio, string fechatermino, string costooficina, string hora, string lugar, string archivouno, string archivodos, string idstatusfallo, string diferimientofechanueva, string diferimientohoranueva, string observaciones, string idstatuslicitacion, string fechamovimiento, string idusuario, string accion, string nombrearchivovideo, string urlvideo)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglo = new string[,] { { "@idlicitacionfecha", "8", idlicitacionfecha } };
            string[,] arrayse = new string[,] { { "@idhlicitacionfecha", "8", idhlicitacionfecha }, { "@idlicitacionfecha", "8", idlicitacionfecha }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@fechainicio", "4", fechainicio }, { "@fechatermino", "4", fechatermino }, { "@costooficina", "5", costooficina }, { "@hora", "12", hora }, { "@lugar", "12", lugar }, { "@archivouno", "12", archivouno }, { "@archivodos", "12", archivodos }, { "@idstatusfallo", "8", idstatusfallo }, { "@diferimientofechanueva", "4", diferimientofechanueva }, { "@diferimientohoranueva", "12", diferimientohoranueva }, { "@observaciones", "12", observaciones }, { "@idstatuslicitacion", "8", idstatuslicitacion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@nombrearchivovideo", "12", nombrearchivovideo }, { "@urlvideo", "12", urlvideo } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacionfechas", arrayse);
           funUpdateDelete(tx, "Delete_tblp_SP_adjudicacionfechas", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertlictaciones(string idlicitacionfecha ,string idhlicitacionfecha, string idadjudicaciones, string fechainicio, string fechatermino, string costooficina, string hora, string lugar, string archivouno, string archivodos, string idstatusfallo, string diferimientofechanueva, string diferimientohoranueva, string observaciones, string idstatuslicitacion, string fechamovimiento, string idusuario, string accion,string nombrearchivovideo,string urlvideo)
    {
        cnn.Open();
        SqlTransaction tx;

        string ruta = "";
        //documento gestión de obras
        if (Convert.ToInt64(idstatuslicitacion) == 1)
        {
            ruta = "Prepublicacion";
        }
        //documento dictamen
        if (Convert.ToInt16(idstatuslicitacion) == 2)
        {
            ruta = "Bases";
        }
        //documentos autorizadas
        if (Convert.ToInt16(idstatuslicitacion) == 3)
        {
            ruta = "Publicacion";
        }
        //documento disponibilidad presupuestal
        if (Convert.ToInt16(idstatuslicitacion) == 4)
        {
            ruta = "Visita_trabajo";
        }
        if (Convert.ToInt16(idstatuslicitacion) == 5)
        {
            ruta = "Juntaaclaraciones";
        }
        if (Convert.ToInt16(idstatuslicitacion) == 6)
        {
            ruta = "Entregapropuesta";
        }
        if (Convert.ToInt16(idstatuslicitacion) == 7)
        {
            ruta = "Fallo";
        }


        string Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\adjudicaciones\"  + ruta + "\\";


        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@idlicitacionfecha", "8", idlicitacionfecha },  { "@idadjudicaciones", "8", idadjudicaciones },   { "@fechainicio", "4", fechainicio }, { "@fechatermino", "4", fechatermino }, { "@costooficina", "5", costooficina }, { "@hora", "12", hora }, { "@lugar", "12", lugar }, { "@archivouno", "12", archivouno }, { "@archivodos", "12", archivodos }, { "@idstatusfallo", "8", idstatusfallo }, { "@diferimientofechanueva", "4", diferimientofechanueva }, { "@diferimientohoranueva", "12", diferimientohoranueva }, { "@observaciones", "12", observaciones }, { "@idstatuslicitacion", "8", idstatuslicitacion }, { "@nombrearchivovideo", "12", nombrearchivovideo }, { "@urlvideo", "12", urlvideo } };
            funInsert(tx, "Insert_tblp_SP_adjudicacionfechas", array);
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayse = new string[,] { { "@idhlicitacionfecha", "8", idhlicitacionfecha }, { "@idlicitacionfecha", "8", idgeneralNew }, { "@idadjudicaciones", "8", idadjudicaciones },  { "@fechainicio", "4", fechainicio }, { "@fechatermino", "4", fechatermino }, { "@costooficina", "5", costooficina }, { "@hora", "12", hora }, { "@lugar", "12", lugar }, { "@archivouno", "12", archivouno }, { "@archivodos", "12", archivodos }, { "@idstatusfallo", "8", idstatusfallo }, { "@diferimientofechanueva", "4", diferimientofechanueva }, { "@diferimientohoranueva", "12", diferimientohoranueva }, { "@observaciones", "12", observaciones }, { "@idstatuslicitacion", "12", idstatuslicitacion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@nombrearchivovideo", "12", nombrearchivovideo }, { "@urlvideo", "12", urlvideo } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacionfechas", arrayse);

            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================


            if (archivouno != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones_Seguimiento" }, { "@rutaarchivo", "12", Rutaraiz + archivouno }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            if (archivodos != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones_Seguimiento" }, { "@rutaarchivo", "12", Rutaraiz + archivodos }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            if (nombrearchivovideo != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "Licitaciones_Seguimiento" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivovideo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatelictaciones(string idlicitacionfecha, string idhlicitacionfecha, string idadjudicaciones, string fechainicio, string fechatermino, string costooficina, string hora, string lugar, string archivouno, string archivodos, string idstatusfallo, string diferimientofechanueva, string diferimientohoranueva, string observaciones, string idstatuslicitacion, string fechamovimiento, string idusuario, string accion, string nombrearchivovideo, string urlvideo)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idlicitacionfecha", "8", idlicitacionfecha }, { "@fechainicio", "4", fechainicio }, { "@fechatermino", "4", fechatermino }, { "@costooficina", "5", costooficina }, { "@hora", "12", hora }, { "@lugar", "12", lugar }, { "@archivouno", "12", archivouno }, { "@archivodos", "12", archivodos }, { "@idstatusfallo", "8", idstatusfallo },  { "@observaciones", "12", observaciones }, { "@idstatuslicitacion", "8", idstatuslicitacion },{ "@nombrearchivovideo", "12", nombrearchivovideo }, { "@urlvideo", "12", urlvideo } };
            funUpdateDelete(tx, "Update_tblp_SP_adjudicacionfechas", array);
         
            string[,] arrayse = new string[,] { { "@idhlicitacionfecha", "8", idhlicitacionfecha }, { "@idlicitacionfecha", "8", idlicitacionfecha }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@fechainicio", "4", fechainicio }, { "@fechatermino", "4", fechatermino }, { "@costooficina", "5", costooficina }, { "@hora", "12", hora }, { "@lugar", "12", lugar }, { "@archivouno", "12", archivouno }, { "@archivodos", "12", archivodos }, { "@idstatusfallo", "8", idstatusfallo }, { "@diferimientofechanueva", "4", diferimientofechanueva }, { "@diferimientohoranueva", "12", diferimientohoranueva }, { "@observaciones", "12", observaciones }, { "@idstatuslicitacion", "8", idstatuslicitacion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion }, { "@nombrearchivovideo", "12", nombrearchivovideo }, { "@urlvideo", "12", urlvideo } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacionfechas", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region LicitacionesContratista_KGS 
    public virtual void funeliminarlicitacionescontratista(string idcontratistaslic, string idhcontratistaslic, string idadjudicaciones, string idempresa, string empresa, string fechamovimiento, string idusuario, string accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            

    string[,] pArreglo = new string[,] { { "@idcontratistaslic", "8", idcontratistaslic } };

            string[,] arrayse = new string[,] { { "@idhcontratistaslic", "8", idhcontratistaslic }, { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacioncontratistas", arrayse);


            funUpdateDelete(tx, "Delete_tblp_SP_adjudicacioncontratistas", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertlictacionescontratista(string idcontratistaslic, string idhcontratistaslic, string idadjudicaciones, string idempresa,  string empresa, string fechamovimiento, string idusuario, string accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }};
            funInsert(tx, "Insert_tblp_SP_adjudicacioncontratistas", array);
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayse = new string[,] { { "@idhcontratistaslic", "8", idhcontratistaslic }, { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacioncontratistas", arrayse);



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatelictacionescontratista(string idcontratistaslic, string idhcontratistaslic, string idadjudicaciones, string idempresa,  string empresa, string fechamovimiento, string idusuario, string accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idcontratistaslic", "8", idcontratistaslic }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa } };
            funUpdateDelete(tx, "Update_tblp_SP_adjudicacioncontratistas", array);

            string[,] arrayse = new string[,] { { "@idhcontratistaslic", "8", idhcontratistaslic }, { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            funInsert(tx, "Insert_tblp_SP_Hadjudicacioncontratistas", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion
    #region visitaobra_ADG
    public Boolean funinsertarvisitaobra(string idgeneral,string idcontrato,string idaccion,string idsupervisor,string supervisor,string fechaultimavisita,string avancefisico,string idstatus,string estatus,string observaciones, GridView grvfotos,string diferenciaavance, string idusuario, string accion,string fecha)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idaccion", "12", idaccion }, { "@idsupervisor", "8", idsupervisor }, { "@supervisor", "12", supervisor }, { "@fechaultimavisita", "4", fechaultimavisita }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus }, { "@observaciones", "12", observaciones }, { "@diferenciaavance", "5", diferenciaavance }, { "@idusuario", "8", idusuario }, { "@fecha", "4", fecha } };
            funInsert(tx, "Insert_tblp_SEG_visitaobra", array);
            string idvisitaobraNew = IDNewx.ToString();
            IDNewtwitter = Convert.ToInt64(IDNewx.ToString());
            string[,] arrayse = new string[,] { { "@idvisitaobra", "8", idvisitaobraNew }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idaccion", "12", idaccion }, { "@idsupervisor", "8", idsupervisor }, { "@supervisor", "12", supervisor }, { "@fechaultimavisita", "4", fechaultimavisita }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus }, { "@observaciones", "12", observaciones }, { "@movimiento", "12", "A" }, { "@idusuario", "8", idusuario }, { "@fecha", "4", fecha }, { "@diferenciaavance", "5", diferenciaavance } };
            funInsert(tx, "Insert_tblp_SEG_Hvisitaobra", arrayse);
            string[,] arraysupdate = new string[,] { { "@idgeneral", "8", idgeneral }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus } };
            funUpdateDelete(tx, "Update_SEG_tblgeneral", arraysupdate);
            string[,] arrayc = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", "" }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", idaccion }, { "@idproyecto", "12", "" }, { "@idpoa", "12", "" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", idcontrato }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2000" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", idstatus }, { "@estatus", "12", estatus }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", idvisitaobraNew }, { "@idsupervisor", "8", idsupervisor }, { "@supervisor", "12", supervisor }, { "@fechaultimavisita", "4", fechaultimavisita }, { "@avancefisico", "5", avancefisico }, { "@idgeneral", "8", idgeneral }, { "@fecha_alta", "4", fecha }, { "@tipo_proceso", "12", "SEG" }, { "@idpago", "8", "0" }, { "@nofactura", "12", "" }, { "@FechaPago", "4", "01/01/2000" }, { "@ImporteLiquidopagar", "5", "0" }, { "@idformapago", "8", "0" }, { "@formapago", "12", "" }, { "@tipopago", "12", "" } };
            funInsert(tx, "Insert_Timelineobrapublica", arrayc);
            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            Int64 idvisitaobranew = 0;
            string idaccionnew = "";
            string fotonew = "";
            foreach (GridViewRow row in grvfotos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvfotos.DataKeys[i].Values[0]) <= 0)
                    {
                        idvisitaobranew = Convert.ToInt64(idvisitaobraNew);
                        idaccionnew = Convert.ToString(idaccion);
                        fotonew = Convert.ToString(grvfotos.DataKeys[i].Values[3]);
                        funInsertgalerias(tx, idvisitaobranew.ToString(), idaccionnew.ToString(), fotonew.ToString(), idusuario.ToString());
                    }

                }
                i += 1;
            }
            tx.Commit();

            //---------------------------------------------------comentado con fecha 29012020
            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================



            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================

            //SelectCommand1 rs = new SelectCommand1();
            //SelectCommand1 rs1 = new SelectCommand1();
            //DataSet ds = new DataSet();
            //DataSet dsS = new DataSet();
            //string consultaavances = "";
            //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero, G.municipio from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
            //Adapter.SelectCommand.CommandText = consultaavances;
            //Adapter.Fill(ds);
            //rs.Table = ds.Tables[0];

            //if (rs.Table.Rows.Count > 0)
            //{

            //    string consultaultimafoto = "";
            //    for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
            //    {
            //        //============================consultamos la  última foto ==================================================   

            //        consultaultimafoto = "select foto from tblp_SEG_galeria where idgaleria = (select max(idgaleria) from tblp_SEG_galeria where idvisitaobra=" + idvisitaobraNew + ")";
            //        Adapter.SelectCommand.CommandText = consultaultimafoto;
            //        Adapter.Fill(dsS);
            //        rs1.Table = dsS.Tables[0];
            //        if (rs1.Table.Rows.Count > 0)
            //        {
            //            string rutaarchivofilename = "http://187.157.115.235/SIOP/Uploads/avancesfisicos/" + rs1.Table.Rows[0]["foto"].ToString();
            //            string rutatwiter = "~/Uploads/avancesfisicos/" + rs1.Table.Rows[0]["foto"].ToString();
            //            string rutaarchivotwitter = System.Web.HttpContext.Current.Server.MapPath(rutatwiter);
            //            //string urlstringfoto = "https://api.telegram.org/bot{0}/sendPhoto?chat_id={1}&caption={2}&photo={3}&parse_mode=html";



            //            string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
            //            string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
            //            string chatId = "@sopchiapas";
            //            string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
            //                          + "<strong>Módulo:Supervisión de Obras</strong>" + "\n"
            //                          + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
            //                          + "<strong>Municipio:</strong>" + rs.Table.Rows[ii]["municipio"].ToString() + "\n"

            //                          + "<strong>Fecha Última Visita de Obra:</strong>" + Convert.ToDateTime(rs.Table.Rows[ii]["fechaultimavisita"].ToString()).ToShortDateString() + "\n"
            //                          + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
            //                          + "Foto Agregada:<a href='" + rutaarchivofilename + "'>" + rutaarchivofilename + "</a> " + "\n"
            //                          + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

            //            //Estructuramos los campos que se van a mandar
            //            urlString = String.Format(urlString, apiToken, chatId, text);
            //            //urlstringfoto = String.Format(urlstringfoto, apiToken, chatId, text, System.Web.HttpUtility.UrlEncode(rutaarchivofilename));
            //            //urlstringfoto = String.Format(urlstringfoto, apiToken, chatId, text, rutaarchivofilename);
            //            WebRequest request = WebRequest.Create(urlString);
            //            Stream rst = request.GetResponse().GetResponseStream();
            //            StreamReader reader = new StreamReader(rst);



            //        }
            //    }
            //}
            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================

            //---------------------------------------------------comentado con fecha 29012020


            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }

    public void mensajetelegram( string idvisitaobraNew)
    {
       
        
            

        

    }
    public Boolean funactualizarvisitaobra(string idvisitaobra, string idaccion, string observaciones, GridView grvfotos, string idusuario,string accion, string fecha_mov, string idgeneral, string idcontrato, string idsupervisor, string supervisor, string fechaultimavisita , string diferenciaavance, string avancefisico, string idstatus,string estatus)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

           
            string[,] arrayse = new string[,] { { "@idvisitaobra", "8", idvisitaobra },  { "@observaciones", "12", observaciones }, { "@idusuario", "8", idusuario }, { "@fecha", "4", fecha_mov } };
            funUpdateDelete(tx, "Update_tblp_SEG_visitaobra", arrayse);

            string[,] arraysh = new string[,] { { "@idvisitaobra", "8", idvisitaobra }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idaccion", "12", idaccion }, { "@idsupervisor", "8", idsupervisor }, { "@supervisor", "12", supervisor }, { "@fechaultimavisita", "4", fechaultimavisita }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus }, { "@observaciones", "12", observaciones }, { "@movimiento", "12", "A" }, { "@idusuario", "8", idusuario }, { "@fecha", "4", fecha_mov }, { "@diferenciaavance", "5", diferenciaavance } };
            funInsert(tx, "Insert_tblp_SEG_Hvisitaobra", arraysh);


            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            Int64 idvisitaobranew = 0;
            string idaccionnew = "";
            string fotonew = "";
            foreach (GridViewRow row in grvfotos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvfotos.DataKeys[i].Values[0]) <= 0)
                    {
                        idvisitaobranew = Convert.ToInt64(idvisitaobra);
                        idaccionnew = Convert.ToString(idaccion);
                        fotonew = Convert.ToString(grvfotos.DataKeys[i].Values[3]);
                        funInsertgalerias(tx, idvisitaobranew.ToString(), idaccionnew.ToString(), fotonew.ToString(), idusuario.ToString());

                        //=======================================================================================================================================================================================================================
                        //====================================================================== para generar los respaldos =====================================================================
                        //=======================================================================================================================================================================================================================

                        if (fotonew != "")
                        {
                            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\avancesfisicos\";
                            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_galeria" }, { "@rutaarchivo", "12", Rutaraiz + fotonew }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                        }
                        //=======================================================================================================================================================================================================================
                        //======================================================================   para generar los respaldos ===================================================================================================================
                        //=======================================================================================================================================================================================================================

                    }
                }
                i += 1;
            }

            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertgalerias(SqlTransaction tx, string idvisitaobranew, string idaccionnew, string fotonew, string idusuario)
    {
        try
        {
            string[,] array = new string[,] { { "@idvisitaobranew", "8", idvisitaobranew }, { "@idaccionnew", "12", idaccionnew }, { "@fotonew", "12", fotonew },  { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_SEG_galerias", array);
            string idgaleria = IDNewx.ToString();
            string[,] arrayx = new string[,] { { "@idgaleria", "8", idgaleria }, { "@idvisitaobranew", "8", idvisitaobranew }, { "@idaccionnew", "12", idaccionnew }, { "@fotonew", "12", fotonew }, { "@idusuario", "8", idusuario }, { "@movimiento", "12", "AFOTO" }, { "@fecha_mov", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_SEG_Hgalerias", arrayx);


            if (fotonew != "")
            {
                //=======================================================================================================================================================================================================================
                //====================================================================== para generar los respaldos =====================================================================
                //=======================================================================================================================================================================================================================
                String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\avancesfisicos\";
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_galeria" }, { "@rutaarchivo", "12", Rutaraiz + fotonew }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                //=======================================================================================================================================================================================================================
                //======================================================================   para generar los respaldos ===================================================================================================================
                //=======================================================================================================================================================================================================================
            }





            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }


    }
    public virtual void funDeletefotos(string eliminarfotos, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {


        
            string[,] pArreglo = new string[,] { { "@eliminarfotos", "12", eliminarfotos.ToString() }, { "@idusuario", "8", idusuario } };
            funUpdateDelete(tx, "Delete_tblp_SEG_fotos", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funeliminarvisitaobra(string idvisitaobra,  string idusuario, string idgeneral, string idaccion, string idcontrato, string observaciones, string idsupervisor, string supervisor, string fechaultimavisita, string diferenciaavance, string avancefisico, string idstatus, string estatus)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] arraysh = new string[,] { { "@idvisitaobra", "8", idvisitaobra }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idaccion", "12", idaccion }, { "@idsupervisor", "8", idsupervisor }, { "@supervisor", "12", supervisor }, { "@fechaultimavisita", "4", fechaultimavisita }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus }, { "@observaciones", "12", observaciones }, { "@movimiento", "12", "E" }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@diferenciaavance", "5", diferenciaavance } };
            funInsert(tx, "Insert_tblp_SEG_Hvisitaobra", arraysh);



            string[,] pArreglo = new string[,] { { "@idvisitaobra", "8", idvisitaobra }, { "@idusuario", "8", idusuario } };
            //string[,] arrayse = new string[,] { { "@idhcontratistaslic", "8", idhcontratistaslic }, { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            //funInsert(tx, "Insert_tblp_SP_Hadjudicacioncontratistas", arrayse);
            funUpdateDelete(tx, "Delete_tblp_SEG_idvisitaobra", pArreglo);
            
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funUpdateSEGtblgeneral(string idgeneral, string avancefisico, string idstatus,string estatus)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglosz = new string[,] { { "@idgeneral", "8", idgeneral }, { "@avancefisico", "5", avancefisico }, { "@idstatus", "8", idstatus }, { "@estatus", "12", estatus } };
            //string[,] pArreglo = new string[,] { { "@idvisitaobra", "8", idvisitaobra } };
            //string[,] arrayse = new string[,] { { "@idhcontratistaslic", "8", idhcontratistaslic }, { "@idcontratistaslic", "8", idcontratistaslic }, { "@idadjudicaciones", "8", idadjudicaciones }, { "@idempresa", "8", idempresa }, { "@empresa", "12", empresa }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario }, { "@accion", "12", accion } };
            //funInsert(tx, "Insert_tblp_SP_Hadjudicacioncontratistas", arrayse);
            funUpdateDelete(tx, "Update_SEG_tblgeneral", pArreglosz);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean funactualizarobracordenada(string idgeneral, string latitud, string longuitud)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idgeneral", "8", idgeneral }, { "@latitud", "5", latitud }, { "@longuitud", "5", longuitud } };
            funUpdateDelete(tx, "Update_tblp_SEG_coordenadas", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();
        }
    }

    //public Boolean funinsertfotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP,string nombrearchivoC, string idgeneral, string idcontrato, string idusuario,string nombreobra,string nombreaccion)
    //{
    //    cnn.Open();
    //    SqlTransaction tx;

    //    tx = cnn.BeginTransaction();
    //    try
    //    {
    //        //insertamos las fotos
    //        string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
    //        funInsert(tx, "Insert_tblp_SEG_fotofichas", array);
    //        string idfichas = IDNewx.ToString();

    //        string[,] arrayh = new string[,] { { "@idficha", "18", idfichas }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "A" } };
    //        funInsert(tx, "Insert_tblp_SEG_Hfotofichas", arrayh);

    //        tx.Commit();
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        tx.Rollback();
    //        throw new System.InvalidOperationException(e.ToString());
    //        return false;

    //    }
    //    finally
    //    {
    //        cnn.Close();

    //    }

    //}
    //public Boolean funupdatefotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral,string idcontrato, string idusuario)
    //{
    //    cnn.Open();
    //    SqlTransaction tx;

    //    tx = cnn.BeginTransaction();
    //    try
    //    {
    //        string[,] arrayh = new string[,] { { "@idficha", "8", idficha }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "M" } };
    //        funInsert(tx, "Insert_tblp_SEG_Hfotofichas", arrayh);


    //        //insertamos las fotos
    //        string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idficha", "8", idficha }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
    //        funUpdateDelete(tx, "Update_SEG_fotofichas", array);


    //        tx.Commit();
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        tx.Rollback();
    //        throw new System.InvalidOperationException(e.ToString());
    //        return false;

    //    }
    //    finally
    //    {
    //        cnn.Close();

    //    }

    //}
    //public Boolean funinsertfotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string montocontratociva, string montoconvenio, string beneficiarios, string fechainiciocontrato, string fechafincontrato, string fechainicioreprogramacion, string fechafinreprogramacion, string Hddconvenio, string Hddreprogramacion, string idreprogramacion)
    //{
    //    cnn.Open();
    //    SqlTransaction tx;

    //    tx = cnn.BeginTransaction();
    //    try
    //    {
    //        //insertamos las fotos
    //        string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
    //        funInsert(tx, "Insert_tblp_SEG_fotofichas", array);
    //        //actualizamos el monto contratado,fecha de incio y fecha fin en contratos y tblp_SP_general
    //        //string[,] arrayC = new string[,] { { "@montocontratociva", "5", montocontratociva }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@idcontrato", "8", idcontrato } };
    //        //funUpdateDelete(tx, "Update_SEG_contrato", arrayC);
    //        ////actualizamos tblp_SP_general
    //        //string[,] arrayT = new string[,] {  { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@beneficiarios", "8", beneficiarios }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
    //        //funUpdateDelete(tx, "Update_SEG_tblpgeneral", arrayT);
    //        //if (Hddconvenio=="C")
    //        //{
    //        //    string[,] arrayCON = new string[,] { { "@montoconvenio", "5", montoconvenio }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
    //        //    funUpdateDelete(tx, "Update_SEG_convenio", arrayCON);
    //        //}
    //        //if (Hddreprogramacion=="R")
    //        //{
    //        //    string[,] arrayR = new string[,] {  { "@fechainicioreprogramacion", "4", fechainicioreprogramacion }, { "@fechafinreprogramacion", "4", fechafinreprogramacion }, { "@idcontrato", "8", idcontrato }, { "@idreprogramacion", "8", idreprogramacion } };
    //        //    funUpdateDelete(tx, "Update_SEG_reprogramacion", arrayR);
    //        //}

    //        tx.Commit();
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        tx.Rollback();
    //        throw new System.InvalidOperationException(e.ToString());
    //        return false;

    //    }
    //    finally
    //    {
    //        cnn.Close();

    //    }

    //}
    //public Boolean funupdatefotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string montocontratociva, string montoconvenio, string beneficiarios, string fechainiciocontrato, string fechafincontrato, string fechainicioreprogramacion, string fechafinreprogramacion, string Hddconvenio, string Hddreprogramacion, string idreprogramacion)
    //{
    //    cnn.Open();
    //    SqlTransaction tx;

    //    tx = cnn.BeginTransaction();
    //    try
    //    {
    //        //insertamos las fotos
    //        string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idficha", "8", idficha } };
    //        funUpdateDelete(tx, "Update_SEG_fotofichas", array);
    //        //actualizamos el monto contratado,fecha de incio y fecha fin en contratos 
    //        //string[,] arrayC = new string[,] { { "@montocontratociva", "5", montocontratociva }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@idcontrato", "8", idcontrato } };
    //        //funUpdateDelete(tx, "Update_SEG_contrato", arrayC);
    //        ////actualizamos tblp_SP_general
    //        //string[,] arrayT = new string[,] { { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@beneficiarios", "8", beneficiarios }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
    //        //funUpdateDelete(tx, "Update_SEG_tblpgeneral", arrayT);
    //        //if (Hddconvenio == "C")
    //        //{
    //        //    string[,] arrayCON = new string[,] { { "@montoconvenio", "5", montoconvenio }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
    //        //    funUpdateDelete(tx, "Update_SEG_convenio", arrayCON);
    //        //}
    //        //if (Hddreprogramacion == "R")
    //        //{
    //        //    string[,] arrayR = new string[,] { { "@fechainicioreprogramacion", "4", fechainicioreprogramacion }, { "@fechafinreprogramacion", "4", fechafinreprogramacion }, { "@idcontrato", "8", idcontrato }, { "@idreprogramacion", "8", idreprogramacion } };
    //        //    funUpdateDelete(tx, "Update_SEG_reprogramacion", arrayR);
    //        //}

    //        tx.Commit();
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        tx.Rollback();
    //        throw new System.InvalidOperationException(e.ToString());
    //        return false;

    //    }
    //    finally
    //    {
    //        cnn.Close();

    //    }

    //}

    //*****************  cambios de obra accion y benefciados KGS 04062020

    public Boolean funinsertfotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string idusuario, string nombreobra, string nombreaccion, string beneficiarios)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            //insertamos las fotos
            //insertamos las fotos
            string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_SEG_fotofichas", array);
            string idfichas = IDNewx.ToString();

            string[,] arrayh = new string[,] { { "@idficha", "8", idfichas }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "A" } };
            funInsert(tx, "Insert_tblp_SEG_Hfotofichas", arrayh);
            //actualizamos el monto contratado,fecha de incio y fecha fin en contratos y tblp_SP_general
            //string[,] arrayC = new string[,] { { "@montocontratociva", "5", montocontratociva }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@idcontrato", "8", idcontrato } };
            //funUpdateDelete(tx, "Update_SEG_contrato", arrayC);
            //actualizamos tblp_SP_general
            string[,] arrayT = new string[,] { { "@obra", "12", nombreobra }, { "@accion", "12", nombreaccion }, { "@beneficiarios", "8", beneficiarios }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
            funUpdateDelete(tx, "Update_SEG_tblpgeneralFT", arrayT);
            //if (Hddconvenio=="C")
            //{
            //    string[,] arrayCON = new string[,] { { "@montoconvenio", "5", montoconvenio }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
            //    funUpdateDelete(tx, "Update_SEG_convenio", arrayCON);
            //}
            //if (Hddreprogramacion=="R")
            //{
            //    string[,] arrayR = new string[,] {  { "@fechainicioreprogramacion", "4", fechainicioreprogramacion }, { "@fechafinreprogramacion", "4", fechafinreprogramacion }, { "@idcontrato", "8", idcontrato }, { "@idreprogramacion", "8", idreprogramacion } };
            //    funUpdateDelete(tx, "Update_SEG_reprogramacion", arrayR);
            //}

            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\fichatecnica\";
            if (nombrearchivoA != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoA }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoD != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoD }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoP != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoP }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoC != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }



            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos ===================================================================================================================
            //=======================================================================================================================================================================================================================


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatefotoficha(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string idusuario, string nombreobra, string nombreaccion, string beneficiarios)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            //insertamos las fotos
            string[,] arrayh = new string[,] { { "@idficha", "8", idficha }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "M" } };
            funInsert(tx, "Insert_tblp_SEG_Hfotofichas", arrayh);


            //insertamos las fotos
            string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idficha", "8", idficha }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funUpdateDelete(tx, "Update_SEG_fotofichas", array);


            //actualizamos el monto contratado,fecha de incio y fecha fin en contratos 
            //string[,] arrayC = new string[,] { { "@montocontratociva", "5", montocontratociva }, { "@fechainiciocontrato", "4", fechainiciocontrato }, { "@fechaterminocontrato", "4", fechafincontrato }, { "@idcontrato", "8", idcontrato } };
            //funUpdateDelete(tx, "Update_SEG_contrato", arrayC);
            //actualizamos tblp_SP_general
            string[,] arrayT = new string[,] { { "@obra", "12", nombreobra }, { "@accion", "12", nombreaccion }, { "@beneficiarios", "8", beneficiarios }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
            funUpdateDelete(tx, "Update_SEG_tblpgeneralFT", arrayT);
            //if (Hddconvenio == "C")
            //{
            //    string[,] arrayCON = new string[,] { { "@montoconvenio", "5", montoconvenio }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato } };
            //    funUpdateDelete(tx, "Update_SEG_convenio", arrayCON);
            //}
            //if (Hddreprogramacion == "R")
            //{
            //    string[,] arrayR = new string[,] { { "@fechainicioreprogramacion", "4", fechainicioreprogramacion }, { "@fechafinreprogramacion", "4", fechafinreprogramacion }, { "@idcontrato", "8", idcontrato }, { "@idreprogramacion", "8", idreprogramacion } };
            //    funUpdateDelete(tx, "Update_SEG_reprogramacion", arrayR);
            //}


            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\fichatecnica\";
            if (nombrearchivoA != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoA }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoD != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoD }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoP != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoP }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoC != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_SEG_fotofichas" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    //*****************  cambios de obra accion y benefciados KGS 04062020
    public Boolean funInsertmetassupervisor(string idgeneral, string idunidad, string unidad,string descripcion,string cantidad)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglosI = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idunidad", "8", idunidad }, { "@unidad", "12", unidad }, { "@descripcion", "12", descripcion }, { "@cantidad", "12", cantidad } };
            funInsert(tx, "Insert_tblp_SEG_metassupervisor", pArreglosI);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funinsertarnuevameta(string idgeneral, string idunidad, string unidad, string descripcion, string cantidad)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglosI = new string[,] { { "@idgeneral", "8", idgeneral }, { "@idunidad", "8", idunidad }, { "@unidad", "12", unidad }, { "@descripcion", "12", descripcion }, { "@cantidad", "12", cantidad } };
            funInsert(tx, "Insert_tblp_SEG_metassupervisor", pArreglosI);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatemeta(string idmetasupervision, string idgeneral, string idunidad, string unidad, string descripcion, string cantidad)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglosI = new string[,] { { "@idmetasupervision", "8", idmetasupervision },{ "@idgeneral", "8", idgeneral }, { "@idunidad", "8", idunidad }, { "@unidad", "12", unidad }, { "@descripcion", "12", descripcion }, { "@cantidad", "12", cantidad } };
            funUpdateDelete(tx, "Update_tbl_generalmetabyidmeta", pArreglosI);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdatemetaseguimiento(string idmetasupervision, string idgeneral, string ultimoavance, string comentario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] pArreglosI = new string[,] { { "@idmetasupervision", "8", idmetasupervision }, { "@idgeneral", "8", idgeneral }, { "@ultimoavance", "5", ultimoavance },  { "@comentario", "12", comentario } };
            funInsert(tx, "Insert_tbl_generalmetaseguimientobyidmeta", pArreglosI);

            //string[,] pArreglosA = new string[,] { { "@idmetasupervision", "8", idmetasupervision }, { "@ultimoavance", "5", ultimoavance } };
            //funUpdateDelete(tx, "Update_tbl_generalmetabyidmeta", pArreglosA);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    public Boolean fundeleteidmeta(string idmetasupervision)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idmetasupervision", "8", idmetasupervision } };
            funUpdateDelete(tx, "Delete_idmeta", array);
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;
        }
        finally
        {
            cnn.Close();
        }
    }
    #endregion
    #region Pagos_KGS 

    public Boolean funInsertPagosP(GridView grvconcepto, string pFechaAlta, string pIDUsuario, string pAccion, string pidgeneral)
    {
        cnn.Open();
        SqlTransaction tx;
        tx = cnn.BeginTransaction();
        Int64 idpago;
        String Folio;
        DateTime FechaPago;
        String contrato;
        String nofactura;
        DateTime fechafactura;
        Decimal ObraBeneficioSocial1;
        Decimal ImpuestoSobreNomina2;
        Decimal ApaortacionICIC;
        Decimal InspeccionVigilancia5;
        Decimal ImporteTotal;
        Decimal Retenciones;
        Decimal ImporteLiquidopagar;
        Int64 idformapago;
        String formapago;
        Int64 idbanco;
        String banco;
        String numcuenta;
        String observaciones;
        String tipopago;
        Int64 idestimaciones;
        String numdocumentos;
        String concepto;
        DateTime fechacaptura;
        Int64 idestatus;
        String estatus;
        Int64 Idfuentefinanciamiento;
        String fuentefinanciamiento;
        Int64 iddatosfinanciero;
        Boolean ahorro;
        Decimal importeahorro;
        Int64 idtipomovimiento;
        String descripcionmovimiento;
        Int64 idgeneral;
        Decimal importeestimacion;
        Decimal ivaestimacion;
        Decimal totalestimacion;
        Decimal amortizacion;
        Decimal poranticipo;
        Decimal totaldecucciones;
        Decimal aportacionsig;
        Decimal adicionalnomina;
        Int64 iddpendenciaconv;
        String dependenciaconv;


        Int16 i = 0;

        try
        {

            foreach (GridViewRow row in grvconcepto.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    idpago = Convert.ToInt64(grvconcepto.DataKeys[i].Values[0]);
                    Folio = Convert.ToString(grvconcepto.DataKeys[i].Values[1]);
                    FechaPago = Convert.ToDateTime(grvconcepto.DataKeys[i].Values[2]);
                    contrato = Convert.ToString(grvconcepto.DataKeys[i].Values[3]);
                    nofactura = Convert.ToString(grvconcepto.DataKeys[i].Values[4]);
                    fechafactura = Convert.ToDateTime(grvconcepto.DataKeys[i].Values[5]);
                    ObraBeneficioSocial1 = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[6]);
                    ImpuestoSobreNomina2 = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[7]);
                    ApaortacionICIC = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[8]);
                    InspeccionVigilancia5 = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[9]);
                    ImporteTotal = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[10]);
                    Retenciones = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[11]);
                    ImporteLiquidopagar = Convert.ToDecimal(grvconcepto.DataKeys[i].Values[12]);
                    idformapago = Convert.ToInt64(grvconcepto.DataKeys[i].Values[13]);
                    formapago = Convert.ToString(grvconcepto.DataKeys[i].Values[14]);
                    idbanco = Convert.ToInt64(grvconcepto.DataKeys[i].Values[15]);
                    banco = Convert.ToString(grvconcepto.DataKeys[i].Values[16]);
                    numcuenta = Convert.ToString(grvconcepto.DataKeys[i].Values[17]);
                    observaciones = Convert.ToString(grvconcepto.DataKeys[i].Values[18]);
                    tipopago = Convert.ToString(grvconcepto.DataKeys[i].Values[19]);
                    idestimaciones = Convert.ToInt64(grvconcepto.DataKeys[i].Values[20]);
                    numdocumentos = Convert.ToString(grvconcepto.DataKeys[i].Values[21]);
                    concepto = Convert.ToString(grvconcepto.DataKeys[i].Values[22]);
                    fechacaptura = Convert.ToDateTime(grvconcepto.DataKeys[i].Values[23]);
                    idestatus = Convert.ToInt64(grvconcepto.DataKeys[i].Values[24]);
                    estatus = Convert.ToString(grvconcepto.DataKeys[i].Values[25]);
                    Idfuentefinanciamiento = Convert.ToInt64(grvconcepto.DataKeys[i].Values[26]);
                    fuentefinanciamiento = Convert.ToString(grvconcepto.DataKeys[i].Values[27]);
                    iddatosfinanciero = Convert.ToInt64(grvconcepto.DataKeys[i].Values[28]);
                    ahorro = Convert.ToBoolean(grvconcepto.DataKeys[i].Values[29]);
                    importeahorro = Convert.ToInt64(grvconcepto.DataKeys[i].Values[30]);
                    idtipomovimiento= Convert.ToInt64(grvconcepto.DataKeys[i].Values[31]);
                    descripcionmovimiento= Convert.ToString(grvconcepto.DataKeys[i].Values[32]);
                    idgeneral = Convert.ToInt64(grvconcepto.DataKeys[i].Values[33]);
                    importeestimacion = Convert.ToInt64(grvconcepto.DataKeys[i].Values[34]);
                    ivaestimacion = Convert.ToInt64(grvconcepto.DataKeys[i].Values[35]);
                    totalestimacion = Convert.ToInt64(grvconcepto.DataKeys[i].Values[36]);
                    amortizacion = Convert.ToInt64(grvconcepto.DataKeys[i].Values[37]);
                    poranticipo = Convert.ToInt64(grvconcepto.DataKeys[i].Values[38]);
                    totaldecucciones = Convert.ToInt64(grvconcepto.DataKeys[i].Values[39]);
                    aportacionsig = Convert.ToInt64(grvconcepto.DataKeys[i].Values[40]);
                    adicionalnomina = Convert.ToInt64(grvconcepto.DataKeys[i].Values[41]);
                    iddpendenciaconv = Convert.ToInt64(grvconcepto.DataKeys[i].Values[42]);
                    dependenciaconv = Convert.ToString(grvconcepto.DataKeys[i].Values[43]);


                    String tipo = "P";
                    if (tipopago == "A")
                    {
                        tipo = "Anticipo";
                    }
                    else
                    {
                        tipo = "Estimación";
                    }


                    funInsertConceptos(tx, idpago.ToString(), Folio.ToString(), FechaPago.ToString(), contrato.ToString(), nofactura.ToString(), fechafactura.ToString(), ObraBeneficioSocial1.ToString(), ImpuestoSobreNomina2.ToString(), ApaortacionICIC.ToString(), InspeccionVigilancia5.ToString(), ImporteTotal.ToString(), Retenciones.ToString(), ImporteLiquidopagar.ToString(), idformapago.ToString(), formapago.ToString(), idbanco.ToString(), banco.ToString(), numcuenta.ToString(), observaciones.ToString(), tipopago.ToString(), idestimaciones.ToString(), numdocumentos.ToString(), concepto.ToString(), fechacaptura.ToString(), idestatus.ToString(), estatus.ToString(), Idfuentefinanciamiento.ToString(), fuentefinanciamiento.ToString(), iddatosfinanciero.ToString(), pFechaAlta.ToString(), pIDUsuario.ToString(), pAccion, ahorro.ToString(), importeahorro.ToString(),idtipomovimiento.ToString(),descripcionmovimiento.ToString(),idgeneral.ToString(),importeestimacion.ToString(), ivaestimacion.ToString(), totalestimacion.ToString(), amortizacion.ToString(), poranticipo.ToString(), totaldecucciones.ToString(), aportacionsig.ToString(), adicionalnomina.ToString(),iddpendenciaconv.ToString(), dependenciaconv.ToString());
                    String IDPagosNew = IDNewx.ToString();


                    //actualiza estatus de estimacion a pagado
                    if (tipopago == "E")
                    {
                        if (idestatus == 3 && idtipomovimiento==1) //estatus pago (3 - PAGADO 1- TOTAL)
                        {
                            //(5 - PAGADO)  estatus estimacion
                            string[,] arrayP = new string[,] { { "@idestimacion", "8", idestimaciones.ToString() }, { "@idestatusestimacion", "8", "5" }, { "@estatusestimacion", "12", "PAGADO" } };
                            funUpdateDelete(tx, "Update_tble_estimacionEstatus", arrayP);
                        }
                    }

                    //actualiza estatus de estimacion a pagado


                    //para generar el time line pagos
                    string[,] arraytm = new string[,] { { "@idtimeline", "8", "0" }, { "@idobra", "8", "0" }, { "@idpeticion", "8", "0" }, { "@nocontrolpeticiones", "12", "" }, { "@fecharecepcion", "4", "01/01/2000" }, { "@numerooficio", "12", "" }, { "@nocontrolobra", "12", "" }, { "@idanteproyecto", "8", "0" }, { "@descripcion_obra", "12", "" }, { "@annio", "12", "" }, { "@clavepresupuestal", "12", "" }, { "@totales", "5", "0" }, { "@folio", "12", "" }, { "@idaccion", "12", "0" }, { "@idproyecto", "12", "0" }, { "@idpoa", "12", "0" }, { "@rubro", "12", "" }, { "@subrubro", "12", "" }, { "@accion", "12", "" }, { "@municipio", "12", "" }, { "@localidad", "12", "" }, { "@idoficioobra", "8", "0" }, { "@idtipooficio", "8", "0" }, { "@nombredocumento", "12", "" }, { "@archivogestion", "12", "" }, { "@idadjudicaciones", "8", "0" }, { "@idconvocatoria", "12", "" }, { "@nconvocatoria", "12", "" }, { "@descripcion_convocatoria", "12", "" }, { "@nolicitacion", "12", "" }, { "@fechapublicacion", "4", "01/01/2000" }, { "@idcontrato", "8", "0" }, { "@numcontrato", "12", "" }, { "@fechacontrato", "4", "01/01/2000" }, { "@fechainiciocontrato", "4", "01/01/2000" }, { "@fechaterminocontrato", "4", "01/01/2000" }, { "@empresa", "12", "" }, { "@idestimacion", "8", "0" }, { "@fechaingreso", "4", "01/01/2019" }, { "@noestimacion", "12", "" }, { "@idestatus", "8", idestatus.ToString() }, { "@estatus", "12", estatus }, { "@idtipoestimacion", "8", "0" }, { "@tipoestimacion", "12", "" }, { "@importeliquido", "5", "0" }, { "@totaldeducciones", "5", "0" }, { "@liquidopagar", "5", "0" }, { "@idvisitaobra", "8", "0" }, { "@idsupervisor", "8", "0" }, { "@supervisor", "12", "" }, { "@fechaultimavisita", "4", "01/01/2000" }, { "@avancefisico", "5", "0" }, { "@idgeneral", "8", idgeneral.ToString() }, { "@fecha_alta", "4", pFechaAlta }, { "@tipo_proceso", "12", "PAG" }, { "@idpago", "8", idgeneralNew }, { "@nofactura", "12", nofactura }, { "@FechaPago", "4", FechaPago.ToString() }, { "@ImporteLiquidopagar", "5", ImporteLiquidopagar.ToString() }, { "@idformapago", "8", idformapago.ToString() }, { "@formapago", "12", formapago }, { "@tipopago", "12", tipo } };
                    funInsert(tx, "Insert_Timelineobrapublica", arraytm);
                    //para generar el time line pago



                }
                i += 1;

            }
            tx.Commit();
          
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funupdatePagosP(string idpago, string Folio, string FechaPago, string contrato, string nofactura, string fechafactura, string ObraBeneficioSocial1, string ImpuestoSobreNomina2, string ApaortacionICIC, string InspeccionVigilancia5, string ImporteTotal, string Retenciones, string ImporteLiquidopagar, string idformapago, string formapago, string idbanco, string banco, string numcuenta, string observaciones, string tipopago, string idestimaciones, string numdocumentos, string concepto, string fechacaptura, string idestatus, string estatus, string fechaalta, string idusuario, string accion, string Idfuentefinanciamiento, string fuentefinanciamiento, string iddatosfinanciero, string pahorro,string pimporteahorro, string idtipomovimiento, string descripcionmovimiento,string idgeneral, string importeestimacion, string ivaestimacion, string totalestimacion, string amortizacion, string poranticipo, string totaldecucciones, string aportacionsig, string adicionalnomina, string iddpendenciaconv, string dependenciaconv)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpago", "8", idpago }, { "@Folio", "12", Folio }, { "@FechaPago", "4", FechaPago }, { "@nofactura", "12", nofactura }, { "@fechafactura", "4", fechafactura }, { "@ImporteLiquidopagar", "5", ImporteLiquidopagar }, { "@idformapago", "8", idformapago }, { "@formapago", "12", formapago }, { "@idbanco", "8", idbanco }, { "@banco", "12", banco }, { "@numcuenta", "12", numcuenta }, { "@observaciones", "12", observaciones }, { "@numdocumentos", "12", numdocumentos }, { "@idestatus", "12", idestatus }, { "@estatus", "12", estatus }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@fuentefinanciamiento", "12", fuentefinanciamiento }, { "@iddatosfinanciero", "8", iddatosfinanciero }, { "@ahorro", "2", pahorro }, { "@importeahorro", "5", pimporteahorro }, { "@idgeneral", "8", idgeneral }, { "@importeestimacion", "5", importeestimacion }, { "@ivaestimacion", "5", ivaestimacion }, { "@totalestimacion", "5", totalestimacion }, { "@amortizacion", "5", amortizacion }, { "@poranticipo", "5", poranticipo }, { "@totaldecucciones", "5", totaldecucciones }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina }, { "@iddpendenciaconv", "8", iddpendenciaconv }, { "@dependenciaconv", "12", dependenciaconv } };
            funUpdateDelete(tx, "Update_tblp_PRE_Pagos", array);

            string[,] arrayH = new string[,] { { "@idhpago", "8", "0" }, { "@idpago", "8", idpago }, { "@Folio", "12", Folio }, { "@FechaPago", "4", FechaPago }, { "@contrato", "12", contrato }, { "@nofactura", "12", nofactura }, { "@fechafactura", "4", fechafactura }, { "@ObraBeneficioSocial1", "5", ObraBeneficioSocial1 }, { "@ImpuestoSobreNomina2", "5", ImpuestoSobreNomina2 }, { "@ApaortacionICIC", "5", ApaortacionICIC }, { "@InspeccionVigilancia5", "5", InspeccionVigilancia5 }, { "@ImporteTotal", "5", ImporteTotal }, { "@Retenciones", "5", Retenciones }, { "@ImporteLiquidopagar", "5", ImporteLiquidopagar }, { "@idformapago", "8", idformapago }, { "@formapago", "12", formapago }, { "@idbanco", "8", idbanco }, { "@banco", "12", banco }, { "@numcuenta", "12", numcuenta }, { "@observaciones", "12", observaciones }, { "@tipopago", "12", tipopago }, { "@idestimaciones", "8", idestimaciones }, { "@numdocumentos", "12", numdocumentos }, { "@FechaAccion", "4", fechaalta }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@concepto", "12", concepto }, { "@fechacaptura", "4", fechacaptura }, { "@idestatus", "8", idestatus }, { "@estatus", "12", estatus }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@fuentefinanciamiento", "12", fuentefinanciamiento }, { "@iddatosfinanciero", "8", iddatosfinanciero }, { "@ahorro", "2", pahorro }, { "@importeahorro", "5", pimporteahorro }, { "@idtipomovimiento", "8", idtipomovimiento }, { "@descripcionmovimiento", "12", descripcionmovimiento }, { "@idgeneral", "8", idgeneral }, { "@importeestimacion", "5", importeestimacion }, { "@ivaestimacion", "5", ivaestimacion }, { "@totalestimacion", "5", totalestimacion }, { "@amortizacion", "5", amortizacion }, { "@poranticipo", "5", poranticipo }, { "@totaldecucciones", "5", totaldecucciones }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina }, { "@iddpendenciaconv", "8", iddpendenciaconv }, { "@dependenciaconv", "12", dependenciaconv } };
            funInsert(tx, "Insert_tblp_PRE_HPagos", arrayH);



            //actualiza estatus de estimacion a pagado
            if (tipopago == "E")
            {
                if (idestatus == "3" && idtipomovimiento == "1") //estatus pago (3 - PAGADO 1- TOTAL)
                {
                    //(5 - PAGADO)  estatus estimacion
                    string[,] arrayP = new string[,] { { "@idestimacion", "8", idestimaciones.ToString() }, { "@idestatusestimacion", "8", "5" }, { "@estatusestimacion", "12", "PAGADO" } };
                    funUpdateDelete(tx, "Update_tble_estimacionEstatus", arrayP);
                }
            }

            //actualiza estatus de estimacion a pagado


            tx.Commit();

            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================


            if (idestatus == "3")
            {

                SelectCommand1 rs = new SelectCommand1();
                SelectCommand1 rs1 = new SelectCommand1();
                DataSet ds = new DataSet();
                DataSet dsS = new DataSet();
                string consultaavances = "";
                //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
                consultaavances = "select top 1 V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where G.contrato = '" + contrato + "' order by idvisitaobra desc ";

                //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
                //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
                Adapter.SelectCommand.CommandText = consultaavances;
                Adapter.Fill(ds);
                rs.Table = ds.Tables[0];

                if (rs.Table.Rows.Count > 0)
                {

                    //telegram======================================================================================
                    //for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
                    //{
                    //    //============================consultamos la  última foto ==================================================   


                    //    string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                    //    string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                    //    string chatId = "@sopchiapas";
                    //    string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
                    //                  + "<strong>Módulo : Pagos</strong>" + "\n"
                    //                  + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
                    //                  + "<strong>Concepto:</strong>" + concepto + "\n"
                    //                  + "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                    //                  + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
                    //                  + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

                    //                  + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

                    //    //Estructuramos los campos que se van a mandar
                    //    urlString = String.Format(urlString, apiToken, chatId, text);

                    //    WebRequest request = WebRequest.Create(urlString);
                    //    Stream rst = request.GetResponse().GetResponseStream();
                    //    StreamReader reader = new StreamReader(rst);


                    //}

                    //este codigo si funciono 09/09/2020

                   

                    //string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                    //string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                    //string chatId = "@Sop2019Bot2020";
                    //string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n";
                    ////+ "<strong>Módulo : Pagos</strong>" + "\n"
                    ////+ "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
                    ////+ "<strong>Concepto:</strong>" + concepto + "\n"
                    ////+ "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                    ////+ "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
                    ////+ "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

                    ////+ "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

                    ////Estructuramos los campos que se van a mandar
                    //urlString = String.Format(urlString, apiToken, chatId, text);

                    //WebRequest request = WebRequest.Create(urlString);
                    //Stream rst = request.GetResponse().GetResponseStream();
                    //StreamReader reader = new StreamReader(rst);




                    //telegram======================================================================================

                }


            }

            ////======================================================================================================================================================================
            ////======================================================================mandar dato por telegram  ======================================================================
            ////======================================================================================================================================================================

            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }


    public Boolean funupdateavancefinancieroinsert(string contrato, string avance,string fechaalta, string idusuario, string accion, GridView grvconcepto)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] arraysupdate = new string[,] { { "@contrato", "12", contrato }, { "@avancefinanciero", "5", avance }};
            funUpdateDelete(tx, "Update_tblgeneralAvanceFin", arraysupdate);

            tx.Commit();

          
            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================
            Int64 idpago;
            String Folio;
            DateTime FechaPago;
            String contratop;
            String nofactura;
            DateTime fechafactura;
            Decimal ObraBeneficioSocial1;
            Decimal ImpuestoSobreNomina2;
            Decimal ApaortacionICIC;
            Decimal InspeccionVigilancia5;
            Decimal ImporteTotal;
            Decimal Retenciones;
            Decimal ImporteLiquidopagar;
            Int64 idformapago;
            String formapago;
            Int64 idbanco;
            String banco;
            String numcuenta;
            String observaciones;
            String tipopago;
            Int64 idestimaciones;
            String numdocumentos;
            String concepto;
            DateTime fechacaptura;
            Int64 idestatus;
            String estatus;
            Int64 Idfuentefinanciamiento;
            String fuentefinanciamiento;
            Int64 iddatosfinanciero;
            Int16 i = 0;
            Int16 ia = 0;

            foreach (GridViewRow row in grvconcepto.Rows)
            {
                idpago = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[0]);
                Folio = Convert.ToString(grvconcepto.DataKeys[ia].Values[1]);
                FechaPago = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[2]);
                contratop = Convert.ToString(grvconcepto.DataKeys[ia].Values[3]);
                nofactura = Convert.ToString(grvconcepto.DataKeys[ia].Values[4]);
                fechafactura = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[5]);
                ObraBeneficioSocial1 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[6]);
                ImpuestoSobreNomina2 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[7]);
                ApaortacionICIC = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[8]);
                InspeccionVigilancia5 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[9]);
                ImporteTotal = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[10]);
                Retenciones = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[11]);
                ImporteLiquidopagar = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[12]);
                idformapago = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[13]);
                formapago = Convert.ToString(grvconcepto.DataKeys[ia].Values[14]);
                idbanco = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[15]);
                banco = Convert.ToString(grvconcepto.DataKeys[ia].Values[16]);
                numcuenta = Convert.ToString(grvconcepto.DataKeys[ia].Values[17]);
                observaciones = Convert.ToString(grvconcepto.DataKeys[ia].Values[18]);
                tipopago = Convert.ToString(grvconcepto.DataKeys[ia].Values[19]);
                idestimaciones = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[20]);
                numdocumentos = Convert.ToString(grvconcepto.DataKeys[ia].Values[21]);
                concepto = Convert.ToString(grvconcepto.DataKeys[ia].Values[22]);
                fechacaptura = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[23]);
                idestatus = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[24]);
                estatus = Convert.ToString(grvconcepto.DataKeys[ia].Values[25]);
                Idfuentefinanciamiento = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[26]);
                fuentefinanciamiento = Convert.ToString(grvconcepto.DataKeys[ia].Values[27]);
                iddatosfinanciero = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[28]);

                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (idestatus == 3)
                    {
                        //======================================================================================================================================================================
                        //======================================================================mandar dato por telegram  ======================================================================
                        //======================================================================================================================================================================

                        SelectCommand1 rs = new SelectCommand1();
                        SelectCommand1 rs1 = new SelectCommand1();
                        DataSet ds = new DataSet();
                        DataSet dsS = new DataSet();
                        string consultaavances = "";
                        //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
                        consultaavances = "select top 1 G.idgeneral,G.avancefisico,G.estatus,C.numcontrato, G.avancefinanciero, G.municipio from tblp_CON_contratos C  inner join tblp_SP_general G on C.idcontrato = G.idcontrato  where G.contrato = '" + contratop + "'  ";

                        //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
                        //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
                        Adapter.SelectCommand.CommandText = consultaavances;
                        Adapter.Fill(ds);
                        rs.Table = ds.Tables[0];
  //====================================telegram======================================
                        //if (rs.Table.Rows.Count > 0)
                        //{


                        //    for (int iia = 0; iia < rs.Table.Rows.Count; iia++)
                        //    {
                        //        //============================consultamos la  última foto ==================================================   


                        //        string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                        //        string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                        //        string chatId = "@sopchiapas";
                        //        string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
                        //                      + "<strong>Módulo : Pagos</strong>" + "\n"
                        //                      + "<strong>Contrato:</strong>" + rs.Table.Rows[iia]["numcontrato"].ToString() + "\n"
                        //                      + "<strong>Municipio:</strong>" + rs.Table.Rows[iia]["municipio"].ToString() + "\n"
                        //                      + "<strong>Concepto:</strong>" + concepto + "\n"
                        //                      + "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                        //                      + "<strong>Avance Físico:</strong>" + rs.Table.Rows[iia]["avancefisico"].ToString() + "%" + "\n"
                        //                      + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[iia]["avancefinanciero"].ToString() + "%" + "\n"

                        //                      + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[iia]["idgeneral"].ToString();

                        //        //Estructuramos los campos que se van a mandar
                        //        urlString = String.Format(urlString, apiToken, chatId, text);

                        //        WebRequest request = WebRequest.Create(urlString);
                        //        Stream rst = request.GetResponse().GetResponseStream();
                        //        StreamReader reader = new StreamReader(rst);


                        //    }
                        //}
   //====================================telegram======================================

                    }
                }
                ia += 1;
            }
            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================
          


            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funupdateavancefinancieroinsertaccion(string contrato, string avance, string fechaalta, string idusuario, string accion, GridView grvconcepto,string idgeneral)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] arraysupdate = new string[,] { { "@contrato", "12", contrato }, { "@avancefinanciero", "5", avance }, { "@idgeneral", "8", idgeneral } };
            funUpdateDelete(tx, "Update_tblgeneralAvanceFinAccion", arraysupdate);

            tx.Commit();


            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================
            Int64 idpago;
            String Folio;
            DateTime FechaPago;
            String contratop;
            String nofactura;
            DateTime fechafactura;
            Decimal ObraBeneficioSocial1;
            Decimal ImpuestoSobreNomina2;
            Decimal ApaortacionICIC;
            Decimal InspeccionVigilancia5;
            Decimal ImporteTotal;
            Decimal Retenciones;
            Decimal ImporteLiquidopagar;
            Int64 idformapago;
            String formapago;
            Int64 idbanco;
            String banco;
            String numcuenta;
            String observaciones;
            String tipopago;
            Int64 idestimaciones;
            String numdocumentos;
            String concepto;
            DateTime fechacaptura;
            Int64 idestatus;
            String estatus;
            Int64 Idfuentefinanciamiento;
            String fuentefinanciamiento;
            Int64 iddatosfinanciero;
            Int16 i = 0;
            Int16 ia = 0;

            foreach (GridViewRow row in grvconcepto.Rows)
            {
                idpago = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[0]);
                Folio = Convert.ToString(grvconcepto.DataKeys[ia].Values[1]);
                FechaPago = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[2]);
                contratop = Convert.ToString(grvconcepto.DataKeys[ia].Values[3]);
                nofactura = Convert.ToString(grvconcepto.DataKeys[ia].Values[4]);
                fechafactura = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[5]);
                ObraBeneficioSocial1 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[6]);
                ImpuestoSobreNomina2 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[7]);
                ApaortacionICIC = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[8]);
                InspeccionVigilancia5 = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[9]);
                ImporteTotal = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[10]);
                Retenciones = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[11]);
                ImporteLiquidopagar = Convert.ToDecimal(grvconcepto.DataKeys[ia].Values[12]);
                idformapago = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[13]);
                formapago = Convert.ToString(grvconcepto.DataKeys[ia].Values[14]);
                idbanco = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[15]);
                banco = Convert.ToString(grvconcepto.DataKeys[ia].Values[16]);
                numcuenta = Convert.ToString(grvconcepto.DataKeys[ia].Values[17]);
                observaciones = Convert.ToString(grvconcepto.DataKeys[ia].Values[18]);
                tipopago = Convert.ToString(grvconcepto.DataKeys[ia].Values[19]);
                idestimaciones = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[20]);
                numdocumentos = Convert.ToString(grvconcepto.DataKeys[ia].Values[21]);
                concepto = Convert.ToString(grvconcepto.DataKeys[ia].Values[22]);
                fechacaptura = Convert.ToDateTime(grvconcepto.DataKeys[ia].Values[23]);
                idestatus = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[24]);
                estatus = Convert.ToString(grvconcepto.DataKeys[ia].Values[25]);
                Idfuentefinanciamiento = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[26]);
                fuentefinanciamiento = Convert.ToString(grvconcepto.DataKeys[ia].Values[27]);
                iddatosfinanciero = Convert.ToInt64(grvconcepto.DataKeys[ia].Values[28]);

                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (idestatus == 3)
                    {
                        //======================================================================================================================================================================
                        //======================================================================mandar dato por telegram  ======================================================================
                        //======================================================================================================================================================================

                        SelectCommand1 rs = new SelectCommand1();
                        SelectCommand1 rs1 = new SelectCommand1();
                        DataSet ds = new DataSet();
                        DataSet dsS = new DataSet();
                        string consultaavances = "";
                        //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
                        consultaavances = "select top 1 G.idgeneral,G.avancefisico,G.estatus,C.numcontrato, G.avancefinanciero, G.municipio from tblp_CON_contratos C  inner join tblp_SP_general G on C.idcontrato = G.idcontrato  where G.contrato = '" + contratop + "'  ";

                        //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
                        //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
                        Adapter.SelectCommand.CommandText = consultaavances;
                        Adapter.Fill(ds);
                        rs.Table = ds.Tables[0];
                        //====================================telegram======================================
                        //if (rs.Table.Rows.Count > 0)
                        //{


                        //    for (int iia = 0; iia < rs.Table.Rows.Count; iia++)
                        //    {
                        //        //============================consultamos la  última foto ==================================================   


                        //        string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                        //        string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                        //        string chatId = "@sopchiapas";
                        //        string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
                        //                      + "<strong>Módulo : Pagos</strong>" + "\n"
                        //                      + "<strong>Contrato:</strong>" + rs.Table.Rows[iia]["numcontrato"].ToString() + "\n"
                        //                      + "<strong>Municipio:</strong>" + rs.Table.Rows[iia]["municipio"].ToString() + "\n"
                        //                      + "<strong>Concepto:</strong>" + concepto + "\n"
                        //                      + "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                        //                      + "<strong>Avance Físico:</strong>" + rs.Table.Rows[iia]["avancefisico"].ToString() + "%" + "\n"
                        //                      + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[iia]["avancefinanciero"].ToString() + "%" + "\n"

                        //                      + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[iia]["idgeneral"].ToString();

                        //        //Estructuramos los campos que se van a mandar
                        //        urlString = String.Format(urlString, apiToken, chatId, text);

                        //        WebRequest request = WebRequest.Create(urlString);
                        //        Stream rst = request.GetResponse().GetResponseStream();
                        //        StreamReader reader = new StreamReader(rst);


                        //    }
                        //}
                        //====================================telegram======================================

                    }
                }
                ia += 1;
            }
            //======================================================================================================================================================================
            //======================================================================mandar dato por telegram  ======================================================================
            //======================================================================================================================================================================



            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funupdateavancefinanciero(string contrato, string avance, string fechaalta, string idusuario, string accion, string idestatus, string estatus, string concepto)//MANDAR EL IDGENERAL COMO PARAMETRO
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] arraysupdate = new string[,] { { "@contrato", "12", contrato }, { "@avancefinanciero", "5", avance } };
            funUpdateDelete(tx, "Update_tblgeneralAvanceFin", arraysupdate);

            tx.Commit();


            // volver11/11/2019
            ////======================================================================================================================================================================
            ////======================================================================mandar dato por telegram  ======================================================================
            ////======================================================================================================================================================================


            if (idestatus == "3")
            {

                SelectCommand1 rs = new SelectCommand1();
                SelectCommand1 rs1 = new SelectCommand1();
                DataSet ds = new DataSet();
                DataSet dsS = new DataSet();
                string consultaavances = "";
                //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
                consultaavances = "select top 1 G.idgeneral,G.avancefisico,G.estatus,C.numcontrato, G.avancefinanciero, G.municipio from tblp_CON_contratos C  inner join tblp_SP_general G on C.idcontrato = G.idcontrato  where G.contrato = '" + contrato + "' ";

                //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
                //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
                Adapter.SelectCommand.CommandText = consultaavances;
                Adapter.Fill(ds);
                rs.Table = ds.Tables[0];

//================================telegram=============================================
                //if (rs.Table.Rows.Count > 0)
                //{


                //    for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
                //    {
                //        //============================consultamos la  última foto ==================================================   


                //        string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                //        string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                //        string chatId = "@sopchiapas";
                //        string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
                //                      + "<strong>Módulo : Pagos</strong>" + "\n"
                //                      + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
                //                      + "<strong>Municipio:</strong>" + rs.Table.Rows[ii]["municipio"].ToString() + "\n"
                //                      + "<strong>Concepto:</strong>" + concepto + "\n"
                //                      + "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                //                      + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
                //                      + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

                //                      + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

                //        //Estructuramos los campos que se van a mandar
                //        urlString = String.Format(urlString, apiToken, chatId, text);

                //        WebRequest request = WebRequest.Create(urlString);
                //        Stream rst = request.GetResponse().GetResponseStream();
                //        StreamReader reader = new StreamReader(rst);


                //      }
                //}
//================================telegram=============================================

            }

            ////======================================================================================================================================================================
            ////======================================================================mandar dato por telegram  ======================================================================
            ////======================================================================================================================================================================
            // volver11/11/2019

            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funupdateavancefinancieroaccion(string contrato, string avance, string fechaalta, string idusuario, string accion, string idestatus, string estatus, string concepto,string idgeneral)//MANDAR EL IDGENERAL COMO PARAMETRO
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] arraysupdate = new string[,] { { "@contrato", "12", contrato }, { "@avancefinanciero", "5", avance }, { "@idgeneral", "8", idgeneral } };
            funUpdateDelete(tx, "Update_tblgeneralAvanceFinAccion", arraysupdate);

            tx.Commit();


            // volver11/11/2019
            ////======================================================================================================================================================================
            ////======================================================================mandar dato por telegram  ======================================================================
            ////======================================================================================================================================================================


            if (idestatus == "3")
            {

                SelectCommand1 rs = new SelectCommand1();
                SelectCommand1 rs1 = new SelectCommand1();
                DataSet ds = new DataSet();
                DataSet dsS = new DataSet();
                string consultaavances = "";
                //consultaavances = "select V.idgeneral,V.idvisitaobra,V.fechaultimavisita,V.avancefisico,V.estatus,C.numcontrato, G.avancefinanciero from tblp_SEG_visitaobra V inner join tblp_CON_contratos C on V.idcontrato = C.idcontrato inner join tblp_SP_general G on V.idgeneral = G.idgeneral  where V.idvisitaobra = " + idvisitaobraNew;
                consultaavances = "select top 1 G.idgeneral,G.avancefisico,G.estatus,C.numcontrato, G.avancefinanciero, G.municipio from tblp_CON_contratos C  inner join tblp_SP_general G on C.idcontrato = G.idcontrato  where G.contrato = '" + contrato + "' ";

                //string[,] arrayT = new string[,] { { "@idvisitaobra", "8", "6" } };
                //ds = funGetbyStoresPropios("Get_All_SEG_galeriatelegrambyidvisita", arrayT);
                Adapter.SelectCommand.CommandText = consultaavances;
                Adapter.Fill(ds);
                rs.Table = ds.Tables[0];

                //================================telegram=============================================
                //if (rs.Table.Rows.Count > 0)
                //{


                //    for (int ii = 0; ii < rs.Table.Rows.Count; ii++)
                //    {
                //        //============================consultamos la  última foto ==================================================   


                //        string urlString = "https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}&parse_mode=html";
                //        string apiToken = "911293239:AAH5BisYBT4kdD8_hfG5jwRexPVUfovreG8";
                //        string chatId = "@sopchiapas";
                //        string text = "<strong>Secretaría de Obras Públicas</strong>" + "\n"
                //                      + "<strong>Módulo : Pagos</strong>" + "\n"
                //                      + "<strong>Contrato:</strong>" + rs.Table.Rows[ii]["numcontrato"].ToString() + "\n"
                //                      + "<strong>Municipio:</strong>" + rs.Table.Rows[ii]["municipio"].ToString() + "\n"
                //                      + "<strong>Concepto:</strong>" + concepto + "\n"
                //                      + "<strong>Estatus Concepto:</strong>" + estatus + "\n"
                //                      + "<strong>Avance Físico:</strong>" + rs.Table.Rows[ii]["avancefisico"].ToString() + "%" + "\n"
                //                      + "<strong>Avance Fínanciero:</strong>" + rs.Table.Rows[ii]["avancefinanciero"].ToString() + "%" + "\n"

                //                      + "<strong>Ficha Técnica:</strong>http://187.157.115.235/portalsiop/Fichatecnicatelegram.aspx?idmapa=" + rs.Table.Rows[ii]["idgeneral"].ToString();

                //        //Estructuramos los campos que se van a mandar
                //        urlString = String.Format(urlString, apiToken, chatId, text);

                //        WebRequest request = WebRequest.Create(urlString);
                //        Stream rst = request.GetResponse().GetResponseStream();
                //        StreamReader reader = new StreamReader(rst);


                //      }
                //}
                //================================telegram=============================================

            }

            ////======================================================================================================================================================================
            ////======================================================================mandar dato por telegram  ======================================================================
            ////======================================================================================================================================================================
            // volver11/11/2019

            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public Boolean funInsertConceptos(SqlTransaction tx, string idpago, string Folio, string FechaPago, string contrato, string nofactura, string fechafactura, string ObraBeneficioSocial1, string ImpuestoSobreNomina2, string ApaortacionICIC, string InspeccionVigilancia5, string ImporteTotal, string Retenciones, string ImporteLiquidopagar, string idformapago, string formapago, string idbanco, string banco, string numcuenta, string observaciones, string tipopago, string idestimaciones, string numdocumentos, string concepto, string fechacaptura, string idestatus, string estatus,string Idfuentefinanciamiento,string fuentefinanciamiento,string iddatosfinanciero, string fechaalta, string idusuario, string accion, string pahorro, string pimporteahorro, string idtipomovimiento,string descripcionmovimiento,string idgeneral,string importeestimacion , string ivaestimacion, string totalestimacion , string amortizacion, string poranticipo, string totaldecucciones, string aportacionsig, string adicionalnomina, string iddpendenciaconv, string dependenciaconv)
    {
        try
        {
            

            string[,] array = new string[,] { { "@idpago", "8", idpago }, { "@Folio", "12", Folio }, { "@FechaPago", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(FechaPago))  }, { "@contrato", "12", contrato }, { "@nofactura", "12", nofactura }, { "@fechafactura", "4", fechafactura }, { "@ObraBeneficioSocial1", "5", ObraBeneficioSocial1 }, { "@ImpuestoSobreNomina2", "5", ImpuestoSobreNomina2 }, { "@ApaortacionICIC", "5", ApaortacionICIC }, { "@InspeccionVigilancia5", "5", InspeccionVigilancia5 }, { "@ImporteTotal", "5", ImporteTotal }, { "@Retenciones", "5", Retenciones }, { "@ImporteLiquidopagar", "5", ImporteLiquidopagar }, { "@idformapago", "8", idformapago }, { "@formapago", "12", formapago }, { "@idbanco", "8", idbanco }, { "@banco", "12", banco }, { "@numcuenta", "12", numcuenta }, { "@observaciones", "12", observaciones }, { "@tipopago", "12", tipopago }, { "@idestimaciones", "8", idestimaciones }, { "@numdocumentos", "12", numdocumentos }, { "@concepto", "12", concepto }, { "@fechacaptura", "4", fechacaptura }, { "@idestatus", "8", idestatus }, { "@estatus", "12", estatus }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@fuentefinanciamiento", "12", fuentefinanciamiento }, { "@iddatosfinanciero", "8", iddatosfinanciero }, { "@ahorro", "2", pahorro }, { "@importeahorro", "5", pimporteahorro }, { "@idtipomovimiento", "8", idtipomovimiento }, { "@descripcionmovimiento", "12", descripcionmovimiento }, { "@idgeneral", "8", idgeneral }, { "@importeestimacion", "5", importeestimacion }, { "@ivaestimacion", "5", ivaestimacion }, { "@totalestimacion", "5", totalestimacion }, { "@amortizacion", "5", amortizacion }, { "@poranticipo", "5", poranticipo }, { "@totaldecucciones", "5", totaldecucciones }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina }, { "@iddpendenciaconv", "8", iddpendenciaconv }, { "@dependenciaconv", "12", dependenciaconv } };
            funInsert(tx, "Insert_tblp_PRE_Pagos", array);
             idgeneralNew = IDNewx.ToString();

            string[,] arrayH = new string[,] { { "@idhpago", "8", "0" }, { "@idpago", "8", idgeneralNew }, { "@Folio", "12", Folio }, { "@FechaPago", "4", FechaPago }, { "@contrato", "12", contrato }, { "@nofactura", "12", nofactura }, { "@fechafactura", "4", fechafactura }, { "@ObraBeneficioSocial1", "5", ObraBeneficioSocial1 }, { "@ImpuestoSobreNomina2", "5", ImpuestoSobreNomina2 }, { "@ApaortacionICIC", "5", ApaortacionICIC }, { "@InspeccionVigilancia5", "5", InspeccionVigilancia5 }, { "@ImporteTotal", "5", ImporteTotal }, { "@Retenciones", "5", Retenciones }, { "@ImporteLiquidopagar", "5", ImporteLiquidopagar }, { "@idformapago", "8", idformapago }, { "@formapago", "12", formapago }, { "@idbanco", "8", idbanco }, { "@banco", "12", banco }, { "@numcuenta", "12", numcuenta }, { "@observaciones", "12", observaciones }, { "@tipopago", "12", tipopago }, { "@idestimaciones", "8", idestimaciones }, { "@numdocumentos", "12", numdocumentos }, { "@FechaAccion", "4", fechaalta }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@concepto", "12", concepto }, { "@fechacaptura", "4", fechacaptura }, { "@idestatus", "8", idestatus }, { "@estatus", "12", estatus }, { "@Idfuentefinanciamiento", "8", Idfuentefinanciamiento }, { "@fuentefinanciamiento", "12", fuentefinanciamiento }, { "@iddatosfinanciero", "8", iddatosfinanciero }, { "@ahorro", "2", pahorro }, { "@importeahorro", "5", pimporteahorro }, { "@idtipomovimiento", "8", idtipomovimiento }, { "@descripcionmovimiento", "12", descripcionmovimiento }, { "@idgeneral", "8", idgeneral }, { "@importeestimacion", "5", importeestimacion }, { "@ivaestimacion", "5", ivaestimacion }, { "@totalestimacion", "5", totalestimacion }, { "@amortizacion", "5", amortizacion }, { "@poranticipo", "5", poranticipo }, { "@totaldecucciones", "5", totaldecucciones }, { "@aportacionsig", "5", aportacionsig }, { "@adicionalnomina", "5", adicionalnomina }, { "@iddpendenciaconv", "8", iddpendenciaconv }, { "@dependenciaconv", "12", dependenciaconv } };
            funInsert(tx, "Insert_tblp_PRE_HPagos", arrayH);

            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }

    }

    #endregion
    #region Auditorias_KGS 

    public virtual void funeliminarauditoria(string IDAuditoria, string IDHAuditoria, string Numeroaudi, string idanio, string anio, string Fechavencimiento, string Nombre, string SituacionAct, string IDOrganoAuditor, string OrganoAuditor, string IDStatusAuditoria, string estatus, string UltimaActualizacion, string FechaCreacion, string FechaIncioSolventacion, string FechaFinSolventacion, string IDOrigen, string Origen, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] pArreglo = new string[,] { { "@IDAuditoria", "8", IDAuditoria } };

            string[,] arrayse = new string[,] { { "@IDHAuditoria", "8", IDHAuditoria }, { "@IDAuditoria", "8", IDAuditoria }, { "@Numeroaudi", "12", Numeroaudi }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@Fechavencimiento", "4", Fechavencimiento }, { "@Nombre", "12", Nombre }, { "@SituacionAct", "12", SituacionAct }, { "@IDOrganoAuditor", "8", IDOrganoAuditor }, { "@OrganoAuditor", "12", OrganoAuditor }, { "@IDStatusAuditoria", "8", IDStatusAuditoria }, { "@estatus", "12", estatus }, { "@UltimaActualizacion", "4", UltimaActualizacion }, { "@FechaCreacion", "4", FechaCreacion }, { "@FechaIncioSolventacion", "4", FechaIncioSolventacion }, { "@FechaFinSolventacion", "4", FechaFinSolventacion }, { "@IDOrigen", "8", IDOrigen }, { "@Origen", "12", Origen }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoria", arrayse);

            funUpdateDelete(tx, "Delete_tblp_ADT_Auditoria", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertauditorias(string IDAuditoria, string IDHAuditoria, string Numeroaudi, string idanio, string anio, string Fechavencimiento, string Nombre, string SituacionAct, string IDOrganoAuditor, string OrganoAuditor, string IDStatusAuditoria, string estatus, string UltimaActualizacion, string FechaCreacion, string FechaIncioSolventacion, string FechaFinSolventacion, string IDOrigen, string Origen, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@IDAuditoria", "8", IDAuditoria }, { "@Numeroaudi", "12", Numeroaudi }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@Fechavencimiento", "12", Fechavencimiento }, { "@Nombre", "12", Nombre }, { "@SituacionAct", "12", SituacionAct }, { "@IDOrganoAuditor", "8", IDOrganoAuditor }, { "@OrganoAuditor", "12", OrganoAuditor }, { "@IDStatusAuditoria", "8", IDStatusAuditoria }, { "@estatus", "12", estatus }, { "@UltimaActualizacion", "4", UltimaActualizacion }, { "@FechaCreacion", "4", FechaCreacion }, { "@FechaIncioSolventacion", "4", FechaIncioSolventacion }, { "@FechaFinSolventacion", "4", FechaFinSolventacion }, { "@IDOrigen", "8", IDOrigen }, { "@Origen", "12", Origen }};
            funInsert(tx, "Insert_tblp_ADT_Auditoria", array);
            string idgeneralNew = IDNewx.ToString();

            string[,] arrayse = new string[,] { { "@IDHAuditoria", "8", IDHAuditoria }, { "@IDAuditoria", "8", idgeneralNew }, { "@Numeroaudi", "12", Numeroaudi }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@Fechavencimiento", "12", Fechavencimiento }, { "@Nombre", "12", Nombre }, { "@SituacionAct", "12", SituacionAct }, { "@IDOrganoAuditor", "8", IDOrganoAuditor }, { "@OrganoAuditor", "12", OrganoAuditor }, { "@IDStatusAuditoria", "8", IDStatusAuditoria }, { "@estatus", "12", estatus }, { "@UltimaActualizacion", "4", UltimaActualizacion }, { "@FechaCreacion", "4", FechaCreacion }, { "@FechaIncioSolventacion", "4", FechaIncioSolventacion }, { "@FechaFinSolventacion", "4", FechaFinSolventacion }, { "@IDOrigen", "8", IDOrigen }, { "@Origen", "12", Origen }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoria", arrayse);



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatauditorias(string IDAuditoria, string IDHAuditoria, string Numeroaudi, string idanio, string anio, string Fechavencimiento, string Nombre, string SituacionAct, string IDOrganoAuditor, string OrganoAuditor, string IDStatusAuditoria, string estatus, string UltimaActualizacion, string FechaCreacion, string FechaIncioSolventacion, string FechaFinSolventacion, string IDOrigen, string Origen, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@IDAuditoria", "8", IDAuditoria }, { "@Numeroaudi", "12", Numeroaudi }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@Fechavencimiento", "12", Fechavencimiento }, { "@Nombre", "12", Nombre }, { "@SituacionAct", "12", SituacionAct }, { "@IDOrganoAuditor", "8", IDOrganoAuditor }, { "@OrganoAuditor", "12", OrganoAuditor }, { "@IDStatusAuditoria", "8", IDStatusAuditoria }, { "@estatus", "12", estatus },{ "@UltimaActualizacion", "4", UltimaActualizacion }, { "@FechaIncioSolventacion", "4", FechaIncioSolventacion }, { "@FechaFinSolventacion", "4", FechaFinSolventacion }, { "@IDOrigen", "8", IDOrigen }, { "@Origen", "12", Origen } };
            funUpdateDelete(tx, "Update_tblp_ADT_Auditoria", array);

            string[,] arrayse = new string[,] { { "@IDHAuditoria", "8", IDHAuditoria }, { "@IDAuditoria", "8", IDAuditoria }, { "@Numeroaudi", "12", Numeroaudi }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@Fechavencimiento", "12", Fechavencimiento }, { "@Nombre", "12", Nombre }, { "@SituacionAct", "12", SituacionAct }, { "@IDOrganoAuditor", "8", IDOrganoAuditor }, { "@OrganoAuditor", "12", OrganoAuditor }, { "@IDStatusAuditoria", "8", IDStatusAuditoria }, { "@estatus", "12", estatus }, { "@UltimaActualizacion", "4", UltimaActualizacion }, { "@FechaCreacion", "4", FechaCreacion }, { "@FechaIncioSolventacion", "4", FechaIncioSolventacion }, { "@FechaFinSolventacion", "4", FechaFinSolventacion }, { "@IDOrigen", "8", IDOrigen }, { "@Origen", "12", Origen }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoria", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    // Contratos Asociados
    public virtual void funeliminarauditoriacontratos(string IDAuditoriaContratos, string IDHAuditoriaContratos, string IDAuditoria, string IDContrato, string Contrato, string Monto, string Observacion, string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            
            string[,] pArreglo = new string[,] { { "@IDAuditoriaContratos", "8", IDAuditoriaContratos } };

            string[,] arrayse = new string[,] { { "@IDHAuditoriaContratos", "8", IDHAuditoriaContratos }, { "@IDAuditoriaContratos", "8", IDAuditoriaContratos }, { "@IDAuditoria", "8", @IDAuditoria }, { "@IDContrato", "8", IDContrato }, { "@Contrato", "12", @Contrato }, { "@Monto", "5", Monto }, { "@Observacion", "12", Observacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaContratos", arrayse);

            funUpdateDelete(tx, "Delete_tblp_ADT_AuditoriaContratos", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertauditoriascontratos( string IDAuditoriaContratos, string IDHAuditoriaContratos, string IDAuditoria , string IDContrato , string Contrato , string Monto , string  Observacion , string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

           

            string[,] array = new string[,] { { "@IDAuditoriaContratos", "8", IDAuditoriaContratos }, { "@IDAuditoria", "8", @IDAuditoria }, { "@IDContrato", "8", IDContrato }, { "@Contrato", "12", @Contrato }, { "@Monto", "5", Monto }, { "@Observacion", "12", Observacion }, { "@fechacaptura", "4", fechacaptura } };
            funInsert(tx, "Insert_tblp_ADT_AuditoriaContratos", array);
            string idgeneralNew = IDNewx.ToString();

            string[,] arrayse = new string[,] { { "@IDHAuditoriaContratos", "8", IDHAuditoriaContratos }, { "@IDAuditoriaContratos", "8", IDAuditoriaContratos }, { "@IDAuditoria", "8", @IDAuditoria }, { "@IDContrato", "8", IDContrato }, { "@Contrato", "12", @Contrato }, { "@Monto", "5", Monto }, { "@Observacion", "12", Observacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaContratos", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatauditoriascontratos(string IDAuditoriaContratos, string IDHAuditoriaContratos, string IDAuditoria, string IDContrato, string Contrato, string Monto, string Observacion, string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
       
            string[,] array = new string[,] { { "@IDAuditoriaContratos", "8", IDAuditoriaContratos }, { "@IDContrato", "8", @IDContrato }, { "@Contrato", "12", @Contrato }, { "@Monto", "5", Monto }, { "@Observacion", "12", Observacion }};
            funUpdateDelete(tx, "Update_tblp_ADT_AuditoriaContratos", array);

            string[,] arrayse = new string[,] { { "@IDHAuditoriaContratos", "8", IDHAuditoriaContratos }, { "@IDAuditoriaContratos", "8", IDAuditoriaContratos }, { "@IDAuditoria", "8", @IDAuditoria }, { "@IDContrato", "8", IDContrato }, { "@Contrato", "12", @Contrato }, { "@Monto", "5", Monto }, { "@Observacion", "12", Observacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaContratos", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    // Contratos Asociados


    // Contratos observaciones
    public virtual void funeliminarauditoriaobservaciones(string IDHAuditoriaObserva, string IDAuditoriaObserva, string IDAuditoria , string Descripcion , string Numeroobserva , string Situacionact , string Acuerdos, string solventado , string MontoInicial , string MontoObservado , string MontoProceso , string MontoSolventado , string MontoPendiente , string IDtipoobservacion , string tipoobservacion, string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
                                                                                                                                                                                                                                                                                                                                                                            
            string[,] pArreglo = new string[,] { { "@IDAuditoriaObserva", "8", IDAuditoriaObserva } };

            string[,] arrayse = new string[,] { { "@IDHAuditoriaObserva", "8", IDHAuditoriaObserva }, { "@IDAuditoriaObserva", "8", IDAuditoriaObserva }, { "@IDAuditoria", "8", @IDAuditoria }, { "@Descripcion", "12", Descripcion }, { "@Numeroobserva", "12", Numeroobserva }, { "@Situacionact", "12", Situacionact }, { "@Acuerdos", "12", Acuerdos }, { "@solventado", "2", solventado }, { "@MontoInicial", "5", MontoInicial }, { "@MontoObservado", "5", MontoObservado }, { "@MontoProceso", "5", MontoProceso }, { "@MontoSolventado", "5", MontoSolventado }, { "@MontoPendiente", "5", MontoPendiente }, { "@IDtipoobservacion", "8", IDtipoobservacion }, { "@tipoobservacion", "12", tipoobservacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaObservaciones", arrayse);

            funUpdateDelete(tx, "Delete_tblp_ADT_AuditoriaObservaciones", pArreglo);
            tx.Commit();
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertauditoriasobservaciones(string IDHAuditoriaObserva, string IDAuditoriaObserva, string IDAuditoria, string Descripcion, string Numeroobserva, string Situacionact, string Acuerdos, string solventado, string MontoInicial, string MontoObservado, string MontoProceso, string MontoSolventado, string MontoPendiente, string IDtipoobservacion, string tipoobservacion, string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {



            string[,] array = new string[,] { { "@IDAuditoriaObserva", "8", "0" }, { "@IDAuditoria", "8", @IDAuditoria }, { "@Descripcion", "12", Descripcion }, { "@Numeroobserva", "12", Numeroobserva }, { "@Situacionact", "12", Situacionact }, { "@Acuerdos", "12", Acuerdos }, { "@solventado", "2", solventado }, { "@MontoInicial", "5", MontoInicial }, { "@MontoObservado", "5", MontoObservado }, { "@MontoProceso", "5", MontoProceso }, { "@MontoSolventado", "5", MontoSolventado }, { "@MontoPendiente", "5", MontoPendiente }, { "@IDtipoobservacion", "8", IDtipoobservacion }, { "@tipoobservacion", "12", tipoobservacion }, { "@fechacaptura", "4", fechacaptura } };
            funInsert(tx, "Insert_tblp_ADT_AuditoriaObservaciones", array);
            string idgeneralNew = IDNewx.ToString();

            string[,] arrayse = new string[,] { { "@IDHAuditoriaObserva", "8", IDHAuditoriaObserva }, { "@IDAuditoriaObserva", "8", idgeneralNew }, { "@IDAuditoria", "8", @IDAuditoria }, { "@Descripcion", "12", Descripcion }, { "@Numeroobserva", "12", Numeroobserva }, { "@Situacionact", "12", Situacionact }, { "@Acuerdos", "12", Acuerdos }, { "@solventado", "2", solventado }, { "@MontoInicial", "5", MontoInicial }, { "@MontoObservado", "5", MontoObservado }, { "@MontoProceso", "5", MontoProceso }, { "@MontoSolventado", "5", MontoSolventado }, { "@MontoPendiente", "5", MontoPendiente }, { "@IDtipoobservacion", "8", IDtipoobservacion }, { "@tipoobservacion", "12", tipoobservacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaObservaciones", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatauditoriasobservaciones(string IDHAuditoriaObserva, string IDAuditoriaObserva, string IDAuditoria, string Descripcion, string Numeroobserva, string Situacionact, string Acuerdos, string solventado, string MontoInicial, string MontoObservado, string MontoProceso, string MontoSolventado, string MontoPendiente, string IDtipoobservacion, string tipoobservacion, string fechacaptura, string fechaAccion, string idusuario, string Accion)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            string[,] array = new string[,] { { "@IDAuditoriaObserva", "8", IDAuditoriaObserva }, { "@IDAuditoria", "8", @IDAuditoria }, { "@Descripcion", "12", Descripcion }, { "@Numeroobserva", "12", Numeroobserva }, { "@Situacionact", "12", Situacionact }, { "@Acuerdos", "12", Acuerdos }, { "@solventado", "2", solventado }, { "@MontoInicial", "5", MontoInicial }, { "@MontoObservado", "5", MontoObservado }, { "@MontoProceso", "5", MontoProceso }, { "@MontoSolventado", "5", MontoSolventado }, { "@MontoPendiente", "5", MontoPendiente }, { "@IDtipoobservacion", "8", IDtipoobservacion }, { "@tipoobservacion", "12", tipoobservacion } };
            funUpdateDelete(tx, "Update_tblp_ADT_AuditoriaObservaciones", array);

            string[,] arrayse = new string[,] { { "@IDHAuditoriaObserva", "8", IDHAuditoriaObserva }, { "@IDAuditoriaObserva", "8", IDAuditoriaObserva }, { "@IDAuditoria", "8", @IDAuditoria }, { "@Descripcion", "12", Descripcion }, { "@Numeroobserva", "12", Numeroobserva }, { "@Situacionact", "12", Situacionact }, { "@Acuerdos", "12", Acuerdos }, { "@solventado", "2", solventado }, { "@MontoInicial", "5", MontoInicial }, { "@MontoObservado", "5", MontoObservado }, { "@MontoProceso", "5", MontoProceso }, { "@MontoSolventado", "5", MontoSolventado }, { "@MontoPendiente", "5", MontoPendiente }, { "@IDtipoobservacion", "8", IDtipoobservacion }, { "@tipoobservacion", "12", tipoobservacion }, { "@fechacaptura", "4", fechacaptura }, { "@fechaAccion", "4", fechaAccion }, { "@idusuario", "8", idusuario }, { "@Accion", "12", Accion } };
            funInsert(tx, "Insert_tblp_ADT_HAuditoriaObservaciones", arrayse);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    // Contratos Observaciones

    #endregion
    #region contratosliberados_KGS 

    public Boolean funGuardarLiberado(GridView grvcontratos)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idgeneral = 0;
       

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvcontratos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtRow = (row.FindControl("txthojas") as TextBox);
                    idgeneral = Convert.ToInt64(grvcontratos.DataKeys[i].Values[1]);
                  
                    CheckBox chkRow = (row.FindControl("chkdocumento") as CheckBox);
                    if (chkRow.Checked == true)
                    {

                        string[,] arraysupdate = new string[,] { { "@idgeneral", "8", idgeneral.ToString() }, { "@Liberado", "2", "True" } };
                        funUpdateDelete(tx, "Update_tblgeneralLiberado", arraysupdate);


                    }
                    else
                    {
                        string[,] arraysupdate = new string[,] { { "@idgeneral", "8", idgeneral.ToString() }, { "@Liberado", "2", "False" } };
                        funUpdateDelete(tx, "Update_tblgeneralLiberado", arraysupdate);

                    }
                }
                i += 1;

            }

            tx.Commit();
            return true;

        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funGuardarsinejercer(GridView grvcontratos)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idgeneral = 0;


        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvcontratos.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtRow = (row.FindControl("txthojas") as TextBox);
                    idgeneral = Convert.ToInt64(grvcontratos.DataKeys[i].Values[1]);

                    CheckBox chkRow = (row.FindControl("chkdocumento") as CheckBox);
                    if (chkRow.Checked == true)
                    {

                        string[,] arraysupdate = new string[,] { { "@idgeneral", "8", idgeneral.ToString() }, { "@sinejercer", "2", "True" } };
                        funUpdateDelete(tx, "Update_tblgeneralsinejercer", arraysupdate);


                    }
                    else
                    {
                        string[,] arraysupdate = new string[,] { { "@idgeneral", "8", idgeneral.ToString() }, { "@sinejercer", "2", "False" } };
                        funUpdateDelete(tx, "Update_tblgeneralsinejercer", arraysupdate);

                    }
                }
                i += 1;

            }

            tx.Commit();
            return true;

        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }
    #endregion

    #region COPLADE_KGS 

    public Boolean funUpdateproyecto(string Idcoplade, string Idsector, string sector, string Idsubcomite, string subcomite, string Idgrupodetrabajo, string grupodetrabajo, string Iddependencia, string dependencia, string proyecto, string clavemunicipio, string nombremunicipio, string clavelocalidad, string nombrelocalidad, string presupuesto, string Idrubro, string rubro, string Idsubrubro, string subrubro,  string Idbenefeciario, string beneficiario, string beneficiariototal, string beneficiariofemenino, string beneficiariomasculino, string Idtipoproyecto, string tipoproyecto, string Idejercicio, string ejercicio, string Idtipo, string TipoCoplade, string Idfuente, string fuentefinancia, string IdPeticion, string Idobra, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
           
            string[,] array = new string[,] { { "@Idcoplade", "8", Idcoplade }, { "@Idsector", "8", Idsector }, { "@sector", "12", sector }, { "@Idsubcomite", "8", Idsubcomite }, { "@subcomite", "12", subcomite }, { "@Idgrupodetrabajo", "8", Idgrupodetrabajo }, { "@grupodetrabajo", "12", grupodetrabajo }, { "@Iddependencia", "8", Iddependencia }, { "@dependencia", "12", dependencia }, { "@proyecto", "12", proyecto }, { "@clavemunicipio", "12", clavemunicipio }, { "@nombremunicipio", "12", nombremunicipio }, { "@clavelocalidad", "12", clavelocalidad }, { "@nombrelocalidad", "12", nombrelocalidad }, { "@presupuesto", "5", presupuesto }, { "@Idrubro", "8", Idrubro }, { "@rubro", "12", rubro }, { "@Idsubrubro", "8", Idsubrubro }, { "@subrubro", "12", subrubro }, { "@Idbenefeciario", "8", Idbenefeciario }, { "@beneficiario", "12", beneficiario }, { "@beneficiariototal", "8", beneficiariototal }, { "@beneficiariofemenino", "8", beneficiariofemenino }, { "@beneficiariomasculino", "8", beneficiariomasculino }, { "@Idtipoproyecto", "8", Idtipoproyecto }, { "@tipoproyecto", "12", tipoproyecto }, { "@Idejercicio", "8", Idejercicio }, { "@ejercicio", "12", ejercicio }, { "@Idtipo", "8", Idtipo }, { "@TipoCoplade", "12", TipoCoplade }, { "@Idfuente", "8", Idfuente }, { "@fuentefinancia", "12", fuentefinancia } };
            funUpdateDelete(tx, "Update_registrocoplade", array);
            string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", Idcoplade }, { "@Idsector", "8", Idsector }, { "@sector", "12", sector }, { "@Idsubcomite", "8", Idsubcomite }, { "@subcomite", "12", subcomite }, { "@Idgrupodetrabajo", "8", Idgrupodetrabajo }, { "@grupodetrabajo", "12", grupodetrabajo }, { "@Iddependencia", "8", Iddependencia }, { "@dependencia", "12", dependencia }, { "@proyecto", "12", proyecto }, { "@clavemunicipio", "12", clavemunicipio }, { "@nombremunicipio", "12", nombremunicipio }, { "@clavelocalidad", "12", clavelocalidad }, { "@nombrelocalidad", "12", nombrelocalidad }, { "@presupuesto", "5", presupuesto }, { "@Idrubro", "8", Idrubro }, { "@rubro", "12", rubro }, { "@Idsubrubro", "8", Idsubrubro }, { "@subrubro", "12", subrubro }, { "@Idbenefeciario", "8", Idbenefeciario }, { "@beneficiario", "12", beneficiario }, { "@beneficiariototal", "8", beneficiariototal }, { "@beneficiariofemenino", "8", beneficiariofemenino }, { "@beneficiariomasculino", "8", beneficiariomasculino }, { "@Idtipoproyecto", "8", Idtipoproyecto }, { "@tipoproyecto", "12", tipoproyecto }, { "@Idejercicio", "8", Idejercicio }, { "@ejercicio", "12", ejercicio }, { "@Idtipo", "8", Idtipo }, { "@TipoCoplade", "12", TipoCoplade }, { "@Idfuente", "8", Idfuente }, { "@fuentefinancia", "12", fuentefinancia }, { "@IdPeticion", "8", IdPeticion }, { "@Idobra", "8", Idobra }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
            funUpdateDelete(tx, "Insert_tblp_COP_Hcoplade", arrayH);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertarproyecto(string Idcoplade, string Idsector, string sector, string Idsubcomite, string subcomite, string Idgrupodetrabajo, string grupodetrabajo, string Iddependencia, string dependencia, string proyecto, string clavemunicipio, string nombremunicipio, string clavelocalidad, string nombrelocalidad, string presupuesto, string Idrubro, string rubro, string Idsubrubro, string subrubro, string Idbenefeciario, string beneficiario, string beneficiariototal, string beneficiariofemenino, string beneficiariomasculino, string Idtipoproyecto, string tipoproyecto, string Idejercicio, string ejercicio, string Idtipo, string TipoCoplade, string Idfuente, string fuentefinancia,string IdPeticion ,string Idobra ,	 string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            
            string[,] array = new string[,] { { "@Idcoplade", "8", Idcoplade }, { "@Idsector", "8", Idsector }, { "@sector", "12", sector }, { "@Idsubcomite", "8", Idsubcomite }, { "@subcomite", "12", subcomite }, { "@Idgrupodetrabajo", "8", Idgrupodetrabajo }, { "@grupodetrabajo", "12", grupodetrabajo }, { "@Iddependencia", "8", Iddependencia }, { "@dependencia", "12", dependencia }, { "@proyecto", "12", proyecto }, { "@clavemunicipio", "12", clavemunicipio }, { "@nombremunicipio", "12", nombremunicipio }, { "@clavelocalidad", "12", clavelocalidad }, { "@nombrelocalidad", "12", nombrelocalidad }, { "@presupuesto", "5", presupuesto }, { "@Idrubro", "8", Idrubro }, { "@rubro", "12", rubro }, { "@Idsubrubro", "8", Idsubrubro }, { "@subrubro", "12", subrubro }, { "@Idbenefeciario", "8", Idbenefeciario }, { "@beneficiario", "12", beneficiario }, { "@beneficiariototal", "8", beneficiariototal }, { "@beneficiariofemenino", "8", beneficiariofemenino }, { "@beneficiariomasculino", "8", beneficiariomasculino }, { "@Idtipoproyecto", "8", Idtipoproyecto }, { "@tipoproyecto", "12", tipoproyecto }, { "@Idejercicio", "8", Idejercicio }, { "@ejercicio", "12", ejercicio }, { "@Idtipo", "8", Idtipo }, { "@TipoCoplade", "12", TipoCoplade }, { "@Idfuente", "8", Idfuente }, { "@fuentefinancia", "12", fuentefinancia }, { "@IdPeticion", "8", IdPeticion }, { "@Idobra", "8", Idobra } };
            funInsert(tx, "Insert_tbl_proyecto", array);
            string idgeneralNew = IDNewx.ToString();
            string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idgeneralNew }, { "@Idsector", "8", Idsector }, { "@sector", "12", sector }, { "@Idsubcomite", "8", Idsubcomite }, { "@subcomite", "12", subcomite }, { "@Idgrupodetrabajo", "8", Idgrupodetrabajo }, { "@grupodetrabajo", "12", grupodetrabajo }, { "@Iddependencia", "8", Iddependencia }, { "@dependencia", "12", dependencia }, { "@proyecto", "12", proyecto }, { "@clavemunicipio", "12", clavemunicipio }, { "@nombremunicipio", "12", nombremunicipio }, { "@clavelocalidad", "12", clavelocalidad }, { "@nombrelocalidad", "12", nombrelocalidad }, { "@presupuesto", "5", presupuesto }, { "@Idrubro", "8", Idrubro }, { "@rubro", "12", rubro }, { "@Idsubrubro", "8", Idsubrubro }, { "@subrubro", "12", subrubro }, { "@Idbenefeciario", "8", Idbenefeciario }, { "@beneficiario", "12", beneficiario }, { "@beneficiariototal", "8", beneficiariototal }, { "@beneficiariofemenino", "8", beneficiariofemenino }, { "@beneficiariomasculino", "8", beneficiariomasculino }, { "@Idtipoproyecto", "8", Idtipoproyecto }, { "@tipoproyecto", "12", tipoproyecto }, { "@Idejercicio", "8", Idejercicio }, { "@ejercicio", "12", ejercicio }, { "@Idtipo", "8", Idtipo }, { "@TipoCoplade", "12", TipoCoplade }, { "@Idfuente", "8", Idfuente }, { "@fuentefinancia", "12", fuentefinancia }, { "@IdPeticion", "8", IdPeticion }, { "@Idobra", "8", Idobra }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
            funInsert(tx, "Insert_tblp_COP_Hcoplade", arrayH);
            

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    public virtual void funGuardararchivo(GridView grvdocts,  string archivozip, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;

        String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\coplade\";

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);


                        CheckBox chkRow = (row.FindControl("chkdocumento") as CheckBox);
                        if (chkRow.Checked == true)
                        {
                            string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@rutaactagrupo", "12", archivozip } };
                            funUpdateDelete(tx, "Update_actagrupocoplade", array);
                            string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@rutaactagrupo", "12", archivozip }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                            funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdateactagrupo", arrayH);

                            //=======================================================================================================================================================================================================================
                            //====================================================================== para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================

                            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_COP_coplade" }, { "@rutaarchivo", "12", Rutaraiz + archivozip }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                            //=======================================================================================================================================================================================================================
                            //======================================================================   para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================



                        }

                    }
                    i += 1;

                }
            }

            tx.Commit();
           

        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    public virtual void funGuardararchivoSEC(GridView grvdocts, string archivozip, string accion, string idusuario, string fechamovimiento)
    {
        cnn.Open();
        SqlTransaction tx;
        Int64 idcoplade = 0;
        String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\coplade\";

        tx = cnn.BeginTransaction();
        try
        {
            Int16 i = 0;
            foreach (GridViewRow row in grvdocts.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {


                        idcoplade = Convert.ToInt64(grvdocts.DataKeys[i].Values[0]);


                        CheckBox chkRow = (row.FindControl("chkdocumentoSEC") as CheckBox);
                        if (chkRow.Checked == true)
                        {
                            string[,] array = new string[,] { { "@Idcoplade", "8", idcoplade.ToString() }, { "@rutaactasectorial", "12", archivozip } };
                            funUpdateDelete(tx, "Update_actasectorialcoplade", array);
                            string[,] arrayH = new string[,] { { "@IdHcoplade", "8", "0" }, { "@Idcoplade", "8", idcoplade.ToString() }, { "@rutaactasectorial", "12", archivozip }, { "@accion", "12", accion }, { "@idusuario", "8", idusuario }, { "@fechamovimiento", "4", fechamovimiento } };
                            funUpdateDelete(tx, "Insert_tblp_COP_HcopladeUpdateactasectorial", arrayH);

                            //=======================================================================================================================================================================================================================
                            //====================================================================== para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================

                            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "tblp_COP_coplade" }, { "@rutaarchivo", "12", Rutaraiz + archivozip }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

                            //=======================================================================================================================================================================================================================
                            //======================================================================   para generar los respaldos =====================================================================
                            //=======================================================================================================================================================================================================================




                        }

                    }
                    i += 1;

                }
            }

            tx.Commit();


        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());

        }
        finally
        {
            cnn.Close();

        }
    }

    #endregion

    #region PeticionesCoordinacion_KGS

    public Boolean funeliminarpeticionescoordinacion(string idpeticionc, string nocontrolpeticiones, string fecharecepcion, string idpromovente, string promovente, string idtipoorganizacion, string organizacion, string idestatuspeticion, string estatuspeticion, string fechacaptura, string tiposolicitud, string fechapeticion, string nombrepeticionario, string cargopeticionario, string peticionariouorganizacion, string observacionespeticion , string idtipopeticion, string nombretipopeticion, string Direccion, string Telefono, string Idanio , string anio, string CveMunicipio, string MNP_Nombre, string IDLocalidad, string LCL_Nombre, string Expediente, string Descripcion , string fechamovimiento, string pidusuario, string pAccion, string Email, string archivoevidencia,string Idgeneral)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] arrayse = new string[,] { { "@idhpeticionc", "8", "0" }, { "@idpeticionc", "8", idpeticionc }, { "@nocontrolpeticiones", "12", nocontrolpeticiones }, { "@fecharecepcion", "4", fecharecepcion }, { "@idpromovente", "8", idpromovente }, { "@promovente", "12", promovente }, { "@idtipoorganizacion", "8", idtipoorganizacion }, { "@organizacion", "12", organizacion }, { "@idestatuspeticion", "8", idestatuspeticion }, { "@estatuspeticion", "12", estatuspeticion }, { "@fechacaptura", "4", fechacaptura }, { "@tiposolicitud", "12", tiposolicitud }, { "@fechapeticion", "4", fechapeticion }, { "@nombrepeticionario", "12", nombrepeticionario }, { "@Direccion", "12", Direccion }, { "@Telefono", "12", Telefono }, { "@Idanio", "8", Idanio }, { "@anio", "12", anio }, { "@CveMunicipio", "12", CveMunicipio }, { "@MNP_Nombre", "12", MNP_Nombre }, { "@IDLocalidad", "8", IDLocalidad }, { "@LCL_Nombre", "12", LCL_Nombre }, { "@Expediente", "12", Expediente }, { "@Descripcion", "12", Descripcion }, { "@Email", "12", Email }, { "@observacionespeticion", "12", observacionespeticion }, { "@idtipopeticion", "8", idtipopeticion }, { "@nombretipopeticion", "12", nombretipopeticion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", pidusuario }, { "@Accion", "12", pAccion }, { "@archivoevidencia", "12", archivoevidencia }, { "@Idgeneral", "8", Idgeneral }  };
            funInsert(tx, "Insert_tblp_PET_HpeticionesCoordinacion", arrayse);

            string[,] pArreglo = new string[,] { { "@idpeticionc", "8", idpeticionc.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_PET_peticionesCoordinacion", pArreglo);
                        tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());


        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funInsertpeticionescoordinacion(string idpeticionc, string nocontrolpeticiones, string fecharecepcion, string idpromovente, string promovente, string idtipoorganizacion, string organizacion, string idestatuspeticion, string estatuspeticion, string fechacaptura, string tiposolicitud, string fechapeticion, string nombrepeticionario, string cargopeticionario, string peticionariouorganizacion, string observacionespeticion, string idtipopeticion, string nombretipopeticion, string Direccion, string Telefono, string Idanio, string anio, string CveMunicipio, string MNP_Nombre, string IDLocalidad, string LCL_Nombre, string Expediente, string Descripcion, string fechamovimiento, string pidusuario, string pAccion, string Email, string archivoevidencia, string Idgeneral)
    {


        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionc", "8", idpeticionc }, { "@nocontrolpeticiones", "12", nocontrolpeticiones }, { "@fecharecepcion", "4", fecharecepcion }, { "@idpromovente", "8", idpromovente }, { "@promovente", "12", promovente }, { "@idtipoorganizacion", "8", idtipoorganizacion }, { "@organizacion", "12", organizacion }, { "@idestatuspeticion", "8", idestatuspeticion }, { "@estatuspeticion", "12", estatuspeticion }, { "@fechacaptura", "4", fechacaptura }, { "@tiposolicitud", "12", tiposolicitud }, { "@fechapeticion", "4", fechapeticion }, { "@nombrepeticionario", "12", nombrepeticionario }, { "@Direccion", "12", Direccion }, { "@Telefono", "12", Telefono }, { "@Idanio", "8", Idanio }, { "@anio", "12", anio }, { "@CveMunicipio", "12", CveMunicipio }, { "@MNP_Nombre", "12", MNP_Nombre }, { "@IDLocalidad", "8", IDLocalidad }, { "@LCL_Nombre", "12", LCL_Nombre }, { "@Expediente", "12", Expediente }, { "@Descripcion", "12", Descripcion }, { "@Email", "12", Email }, { "@observacionespeticion", "12", observacionespeticion }, { "@idtipopeticion", "8", idtipopeticion }, { "@nombretipopeticion", "12", nombretipopeticion }, { "@archivoevidencia", "12", archivoevidencia }, { "@Idgeneral", "8", Idgeneral } };
            funInsert(tx, "Insert_tblp_PET_peticionesCoordinacion", array);

            String idpeticioncnew = IDNewx.ToString();


            string[,] arrayse = new string[,] { { "@idhpeticionc", "8", "0" }, { "@idpeticionc", "8", idpeticionc }, { "@nocontrolpeticiones", "12", nocontrolpeticiones }, { "@fecharecepcion", "4", fecharecepcion }, { "@idpromovente", "8", idpromovente }, { "@promovente", "12", promovente }, { "@idtipoorganizacion", "8", idtipoorganizacion }, { "@organizacion", "12", organizacion }, { "@idestatuspeticion", "8", idestatuspeticion }, { "@estatuspeticion", "12", estatuspeticion }, { "@fechacaptura", "4", fechacaptura }, { "@tiposolicitud", "12", tiposolicitud }, { "@fechapeticion", "4", fechapeticion }, { "@nombrepeticionario", "12", nombrepeticionario }, { "@Direccion", "12", Direccion }, { "@Telefono", "12", Telefono }, { "@Idanio", "8", Idanio }, { "@anio", "12", anio }, { "@CveMunicipio", "12", CveMunicipio }, { "@MNP_Nombre", "12", MNP_Nombre }, { "@IDLocalidad", "8", IDLocalidad }, { "@LCL_Nombre", "12", LCL_Nombre }, { "@Expediente", "12", Expediente }, { "@Descripcion", "12", Descripcion }, { "@Email", "12", Email }, { "@observacionespeticion", "12", observacionespeticion }, { "@idtipopeticion", "8", idtipopeticion }, { "@nombretipopeticion", "12", nombretipopeticion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", pidusuario }, { "@Accion", "12", pAccion }, { "@archivoevidencia", "12", archivoevidencia }, { "@Idgeneral", "8", Idgeneral } };
            funInsert(tx, "Insert_tblp_PET_HpeticionesCoordinacion", arrayse);


            tx.Commit();

           


            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatepeticionescoordinacion(string idpeticionc, string nocontrolpeticiones, string fecharecepcion, string idpromovente, string promovente, string idtipoorganizacion, string organizacion, string idestatuspeticion, string estatuspeticion, string fechacaptura, string tiposolicitud, string fechapeticion, string nombrepeticionario, string cargopeticionario, string peticionariouorganizacion, string observacionespeticion, string idtipopeticion, string nombretipopeticion, string Direccion, string Telefono, string Idanio, string anio, string CveMunicipio, string MNP_Nombre, string IDLocalidad, string LCL_Nombre, string Expediente, string Descripcion, string fechamovimiento, string pidusuario, string pAccion, string Email, string archivoevidencia, string Idgeneral)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            string[,] array = new string[,] { { "@idpeticionc", "8", idpeticionc }, { "@nocontrolpeticiones", "12", nocontrolpeticiones }, { "@fecharecepcion", "4", fecharecepcion }, { "@idpromovente", "8", idpromovente }, { "@promovente", "12", promovente }, { "@idtipoorganizacion", "8", idtipoorganizacion }, { "@organizacion", "12", organizacion }, { "@idestatuspeticion", "8", idestatuspeticion }, { "@estatuspeticion", "12", estatuspeticion }, { "@fechacaptura", "4", fechacaptura }, { "@tiposolicitud", "12", tiposolicitud }, { "@fechapeticion", "4", fechapeticion }, { "@nombrepeticionario", "12", nombrepeticionario }, { "@Direccion", "12", Direccion }, { "@Telefono", "12", Telefono }, { "@Idanio", "8", Idanio }, { "@anio", "12", anio }, { "@CveMunicipio", "12", CveMunicipio }, { "@MNP_Nombre", "12", MNP_Nombre }, { "@IDLocalidad", "8", IDLocalidad }, { "@LCL_Nombre", "12", LCL_Nombre }, { "@Expediente", "12", Expediente }, { "@Descripcion", "12", Descripcion }, { "@Email", "12", Email }, { "@observacionespeticion", "12", observacionespeticion }, { "@idtipopeticion", "8", idtipopeticion }, { "@nombretipopeticion", "12", nombretipopeticion }, { "@archivoevidencia", "12", archivoevidencia }, { "@Idgeneral", "8", Idgeneral } };
            funUpdateDelete(tx, "Update_tblp_PET_peticionesCoordinacion", array);

            string[,] arrayse = new string[,] { { "@idhpeticionc", "8", "0" }, { "@idpeticionc", "8", idpeticionc }, { "@nocontrolpeticiones", "12", nocontrolpeticiones }, { "@fecharecepcion", "4", fecharecepcion }, { "@idpromovente", "8", idpromovente }, { "@promovente", "12", promovente }, { "@idtipoorganizacion", "8", idtipoorganizacion }, { "@organizacion", "12", organizacion }, { "@idestatuspeticion", "8", idestatuspeticion }, { "@estatuspeticion", "12", estatuspeticion }, { "@fechacaptura", "4", fechacaptura }, { "@tiposolicitud", "12", tiposolicitud }, { "@fechapeticion", "4", fechapeticion }, { "@nombrepeticionario", "12", nombrepeticionario }, { "@Direccion", "12", Direccion }, { "@Telefono", "12", Telefono }, { "@Idanio", "8", Idanio }, { "@anio", "12", anio }, { "@CveMunicipio", "12", CveMunicipio }, { "@MNP_Nombre", "12", MNP_Nombre }, { "@IDLocalidad", "8", IDLocalidad }, { "@LCL_Nombre", "12", LCL_Nombre }, { "@Expediente", "12", Expediente }, { "@Descripcion", "12", Descripcion }, { "@Email", "12", Email }, { "@observacionespeticion", "12", observacionespeticion }, { "@idtipopeticion", "8", idtipopeticion }, { "@nombretipopeticion", "12", nombretipopeticion }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", pidusuario }, { "@Accion", "12", pAccion }, { "@archivoevidencia", "12", archivoevidencia }, { "@Idgeneral", "8", Idgeneral } };
            funInsert(tx, "Insert_tblp_PET_HpeticionesCoordinacion", arrayse);

            tx.Commit();

           

            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }


    public Boolean funinsertarseguminetocoordinacion( GridView grvarch, string fechamovimiento, string idusuario, string accion, string ideeliminados)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
           
            //crea oficios
            Int64 idarchivoseguimiento = 0;
            Int64 idpeticionc = 0;
            string ruta = "";
            DateTime fechapublicacion= Convert.ToDateTime("01/01/1900");
            string descripcionseguimiento = "";
            DateTime fechadocumento = Convert.ToDateTime("01/01/1900");
            Int64 idstatusseguimiento = 0;
            string estatus = "";
           

            //insertar seguimiento
            Int16 i = 0;
            foreach (GridViewRow row in grvarch.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    
                    idarchivoseguimiento = Convert.ToInt64(grvarch.DataKeys[i].Values[0]);
                    idpeticionc = Convert.ToInt64(grvarch.DataKeys[i].Values[1]);
                    ruta = Convert.ToString(grvarch.DataKeys[i].Values[2]);
                    fechapublicacion = Convert.ToDateTime(grvarch.DataKeys[i].Values[3]);
                    descripcionseguimiento = Convert.ToString(grvarch.DataKeys[i].Values[4]);
                    fechadocumento = Convert.ToDateTime(grvarch.DataKeys[i].Values[5]);
                    idstatusseguimiento = Convert.ToInt64(grvarch.DataKeys[i].Values[6]);
                    estatus = Convert.ToString(grvarch.DataKeys[i].Values[7]);
                    if (idarchivoseguimiento == 0)
                         {
                        string[,] array = new string[,] { { "@idarchivoseguimiento", "8", "0" }, { "@idpeticionc", "8", idpeticionc.ToString() }, { "@ruta", "12", ruta }, { "@fechapublicacion", "4", fechapublicacion.ToString() }, { "@descripcionseguimiento", "12", descripcionseguimiento }, { "@fechadocumento", "4", fechadocumento.ToString() }, { "@idstatusseguimiento", "8", idstatusseguimiento.ToString() }, { "@estatus", "12", estatus } };
                    funInsert(tx, "Insert_tblp_PET_seguimientoCoordinacion", array);

                    String idseguimientonew = IDNewx.ToString();


                    string[,] arrayse = new string[,] { { "@idHarchivoseguimiento", "8", "0" }, { "@idarchivoseguimiento", "8", idseguimientonew }, { "@idpeticionc", "8", idpeticionc.ToString() }, { "@ruta", "12", ruta }, { "@fechapublicacion", "4", fechapublicacion.ToString() }, { "@descripcionseguimiento", "12", descripcionseguimiento }, { "@fechadocumento", "4", fechadocumento.ToString() }, { "@idstatusseguimiento", "8", idstatusseguimiento.ToString() }, { "@estatus", "12", estatus }, { "@fechamovimiento", "4", fechamovimiento }, { "@idusuario", "8", idusuario.ToString() }, { "@Accion", "12", accion.ToString() } };
                    funInsert(tx, "Insert_tblp_PET_HseguimientoCoordinacion", arrayse);


                    }

                }
                i += 1;

            }
            
            string[,] pArreglo = new string[,] { { "@eliminaridarchivoseguimiento", "12", ideeliminados.ToString() } };
            funUpdateDelete(tx, "Delete_tblp_PET_seguimientoCoordinacion", pArreglo);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }


    #endregion


    #region fotofichatecnica_KGS

    public Boolean funinsertfotofichatecnica(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            
            //insertamos las fotos
            string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_SEG_FotoFichaTecnica", array);
            string idfichas = IDNewx.ToString();

            string[,] arrayh = new string[,] { { "@idFichaFoto", "8", idfichas }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "A" } };
            funInsert(tx, "Insert_tblp_SEG_HFotoFichaTecnica", arrayh);

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos ===================================================================================================================
            //=======================================================================================================================================================================================================================

            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\fotosfichatecnica\";
            if (nombrearchivoA != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoA }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoD != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoD }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoP != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoP }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoC != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }


            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos ===================================================================================================================
            //=======================================================================================================================================================================================================================

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    public Boolean funupdatefotofichaTecnica(string idficha, string nombrearchivoA, string nombrearchivoD, string nombrearchivoP, string nombrearchivoC, string idgeneral, string idcontrato, string idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {
            //insertamos las fotos
            string[,] arrayh = new string[,] { { "@idFichaFoto", "8", idficha }, { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idgeneral", "8", idgeneral }, { "@idcontrato", "8", idcontrato }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@Accion", "12", "M" } };
            funInsert(tx, "Insert_tblp_SEG_HFotoFichaTecnica", arrayh);


            //insertamos las fotos
            string[,] array = new string[,] { { "@antes", "12", nombrearchivoA }, { "@durante", "12", nombrearchivoD }, { "@despues", "12", nombrearchivoP }, { "@croquis", "12", nombrearchivoC }, { "@idFichaFoto", "8", idficha }, { "@idusuario", "8", idusuario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funUpdateDelete(tx, "Update_tblp_SEG_FotoFichaTecnica", array);


            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos ===================================================================================================================
            //=======================================================================================================================================================================================================================

            String Rutaraiz = @"C:\inetpub\wwwroot\SIOP\Uploads\fotosfichatecnica\";
            if (nombrearchivoA != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoA }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoD != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoD }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoP != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoP }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }
            if (nombrearchivoC != "")
            {
                string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0" }, { "@tipocontrato", "8", "0" }, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", "" }, { "@procesotabla", "12", "FotoFichaTecnica" }, { "@rutaarchivo", "12", Rutaraiz + nombrearchivoC }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
                funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);
            }


            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos ===================================================================================================================
            //=======================================================================================================================================================================================================================


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }
    #endregion


    #region proyectos_KGS
    public Boolean funinsertarobraproyecto( string idproyecto, string nombreproyecto,string comentario, GridView grvnotaseguimiento, string eliminararchivos,string Idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDProyecto", "8", idproyecto }, { "@Nombreproyecto", "12", nombreproyecto }, { "@Comentario", "12", comentario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_PROY_Proyectos", array);
            string idnew = IDNewx.ToString();

            string[,] arrayh = new string[,] { { "@IDHProyecto", "8", "0" }, { "@IDProyecto", "8", idnew }, { "@Nombreproyecto", "12", nombreproyecto }, { "@Comentario", "12", comentario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "12", Idusuario }, { "@accion", "12", "A" } };
            funInsert(tx, "Insert_tblp_PROY_HProyectos", arrayh);
            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            Int64 idproyectonew = 0;
            string nombrearchivonew = "";
            string comentarionew = "";
            foreach (GridViewRow row in grvnotaseguimiento.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvnotaseguimiento.DataKeys[i].Values[0]) <= 0)
                    {
                        idproyectonew = Convert.ToInt64(idnew);
                        nombrearchivonew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[2]);
                        comentarionew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[3]);
                        funInsertarchivos(tx, idproyectonew.ToString(), nombrearchivonew.ToString(), comentarionew.ToString(), Idusuario.ToString());
                    }
                }
                i += 1;
            }

            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdateobraproyecto(string idproyecto, string nombreproyecto, string comentario, GridView grvnotaseguimiento, string eliminararchivos, string Idusuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDProyecto", "8", idproyecto }, { "@Nombreproyecto", "12", nombreproyecto }, { "@Comentario", "12", comentario }  };
            funUpdateDelete(tx, "Update_tblp_PROY_Proyectos", array);
            

            string[,] arrayh = new string[,] { { "@IDHProyecto", "8", "0" }, { "@IDProyecto", "8", idproyecto }, { "@Nombreproyecto", "12", nombreproyecto }, { "@Comentario", "12", comentario }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "12", Idusuario }, { "@accion", "12", "M" } };
            funInsert(tx, "Insert_tblp_PROY_HProyectos", arrayh);
            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================
            Int16 i = 0;
            Int64 IDArchivoproy = 0;
            Int64 idproyectonew = 0;
            string nombrearchivonew = "";
            string comentarionew = "";
            foreach (GridViewRow row in grvnotaseguimiento.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {//checamos si no existe el registro en la base insertamos               
                    if (Convert.ToInt64(grvnotaseguimiento.DataKeys[i].Values[0]) <= 0)
                    {
                        idproyectonew = Convert.ToInt64(grvnotaseguimiento.DataKeys[i].Values[1]);
                        nombrearchivonew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[2]);
                        comentarionew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[3]);
                        funInsertarchivos(tx, idproyectonew.ToString(), nombrearchivonew.ToString(), comentarionew.ToString(), Idusuario.ToString());
                    }
                    else
                    {
                        IDArchivoproy = Convert.ToInt64(grvnotaseguimiento.DataKeys[i].Values[0]);
                        idproyectonew = Convert.ToInt64(grvnotaseguimiento.DataKeys[i].Values[1]);
                        nombrearchivonew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[2]);
                        comentarionew = Convert.ToString(grvnotaseguimiento.DataKeys[i].Values[3]);
                        funupdatearchivos(tx, IDArchivoproy.ToString(), idproyectonew.ToString(), nombrearchivonew.ToString(), comentarionew.ToString(), Idusuario.ToString());
                    }
                }
                i += 1;
            }

            //======================================================================================================================================================================
            //======================================================================agregar actualizar galeria======================================================================
            //======================================================================================================================================================================

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funInsertarchivos(SqlTransaction tx, string idproyectonew, string nombrearchivonew, string comentarionew, string Idusuario)
    {
        try
        {
            string[,] array = new string[,] { { "@IDProyecto", "8", idproyectonew }, { "@nombrearchivo", "12", nombrearchivonew }, { "@Comentario", "12", comentarionew }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funInsert(tx, "Insert_tblp_PROY_ProyectosArchivos", array);
            string idgaleria = IDNewx.ToString();
            string[,] arrayx = new string[,] { { "@IDHarchivoProy", "8", "0" }, { "@IDArchivoproy", "8", idgaleria }, { "@IDProyecto", "8", idproyectonew }, { "@nombrearchivo", "12", nombrearchivonew }, { "@Comentario", "12", comentarionew }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "A" } };
            funInsert(tx, "Insert_tblp_PROY_HProyectosArchivos", arrayx);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }


    }
    public Boolean funupdatearchivos(SqlTransaction tx, string IDArchivoproy, string idproyectonew, string nombrearchivonew, string comentarionew, string Idusuario)
    {
        try
        {
            string[,] array = new string[,] { { "@IDArchivoproy", "8", IDArchivoproy }, { "@IDProyecto", "8", idproyectonew }, { "@nombrearchivo", "12", nombrearchivonew }, { "@Comentario", "12", comentarionew }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) } };
            funUpdateDelete(tx, "Update_tblp_PROY_ProyectosArchivosbyid", array);
           
            string[,] arrayx = new string[,] { { "@IDHarchivoProy", "8", "0" }, { "@IDArchivoproy", "8", IDArchivoproy }, { "@IDProyecto", "8", idproyectonew }, { "@nombrearchivo", "12", nombrearchivonew }, { "@Comentario", "12", comentarionew }, { "@fecha", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "M" } };
            funInsert(tx, "Insert_tblp_PROY_HProyectosArchivos", arrayx);
            return true;
        }
        catch (Exception e)
        {
            throw new System.InvalidOperationException("Error de datos");
            return false;

        }


    }
    #endregion



    #region DocuemntoArchivo_KGS
    public Boolean funinsertarDocumentoArchivo(string IDarchivo, string descripcionarchivo, string nombrearchivo, string Idusuario , string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivo", "8", IDarchivo }, { "@descripcionarchivo", "12", descripcionarchivo }, { "@nombrearchivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }   };
            funInsert(tx, "Insert_tblp_SP_DocumentoArchivo", array);
            string idnew = IDNewx.ToString();

            string[,] arrayh = new string[,] { { "@IDHarchivo", "8", "0" }, { "@IDarchivo", "8", idnew }, { "@descripcionarchivo", "12", descripcionarchivo }, { "@nombrearchivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "A" }, { "@usuario", "12", usuario } };
            funInsert(tx, "[dbo].[Insert_tblp_SP_HDocumentoArchivo]", arrayh);
            
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdateDocumentoArchivo(string IDarchivo, string descripcionarchivo, string nombrearchivo, string Idusuario, string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivo", "8", IDarchivo }, { "@descripcionarchivo", "12", descripcionarchivo }, { "@nombrearchivo", "12", nombrearchivo } };
            funUpdateDelete(tx, "Update_tblp_SP_DocumentoArchivo", array);


            string[,] arrayh = new string[,] { { "@IDHarchivo", "8", "0" }, { "@IDarchivo", "8", IDarchivo }, { "@descripcionarchivo", "12", descripcionarchivo }, { "@nombrearchivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "M" }, { "@usuario", "12", usuario } };
            funInsert(tx, "[dbo].[Insert_tblp_SP_HDocumentoArchivo]", arrayh);

          
            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funeliminarDocumentoArchivo(string IDarchivo, string descripcionarchivo, string nombrearchivo, string Idusuario, string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivo", "8", IDarchivo }};
            funUpdateDelete(tx, "Delete_tblp_SP_DocumentoArchivo", array);


            string[,] arrayh = new string[,] { { "@IDHarchivo", "8", "0" }, { "@IDarchivo", "8", IDarchivo }, { "@descripcionarchivo", "12", descripcionarchivo }, { "@nombrearchivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "E" }, { "@usuario", "12", usuario } };
            funInsert(tx, "[dbo].[Insert_tblp_SP_HDocumentoArchivo]", arrayh);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }


    #endregion



    #region ArchivoUnicoContrato_KGS
    public Boolean funinsertarArchivounicocontrato(string IDarchivo, string descripcionarchivo, string nombrearchivo, string  idanio, string anio, string Idusuario, string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivounivo", "8", IDarchivo }, { "@descripcionarchivounivo", "12", descripcionarchivo }, { "@nombrearchivounivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funInsert(tx, "Insert_tblp_SP_ArchivoContrato", array);
            string idnew = IDNewx.ToString();

            string[,] arrayh = new string[,] { { "@IDHarchivounivo", "8", "0" }, { "@IDarchivounivo", "8", idnew }, { "@descripcionarchivounivo", "12", descripcionarchivo }, { "@nombrearchivounivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "A" }, { "@usuario", "12", usuario } };
            funInsert(tx, "Insert_tblp_SP_HArchivoContrato", arrayh);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funupdateArchivounicocontrato(string IDarchivo, string descripcionarchivo, string nombrearchivo, string idanio, string anio, string Idusuario, string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivounivo", "8", IDarchivo }, { "@descripcionarchivounivo", "12", descripcionarchivo }, { "@nombrearchivounivo", "12", nombrearchivo }, { "@idanio", "8", idanio }, { "@anio", "12", anio } };
            funUpdateDelete(tx, "Update_tblp_SP_ArchivoContrato", array);


            string[,] arrayh = new string[,] { { "@IDHarchivounivo", "8", "0" }, { "@IDarchivounivo", "8", IDarchivo }, { "@descripcionarchivounivo", "12", descripcionarchivo }, { "@nombrearchivounivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "M" }, { "@usuario", "12", usuario } };
            funInsert(tx, "Insert_tblp_SP_HArchivoContrato", arrayh);


            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }
    public Boolean funeliminarfunupdateArchivounicocontrato(string IDarchivo, string descripcionarchivo, string nombrearchivo, string idanio, string anio, string Idusuario, string usuario)
    {
        cnn.Open();
        SqlTransaction tx;

        tx = cnn.BeginTransaction();
        try
        {

            //insertamos las fotos
            string[,] array = new string[,] { { "@IDarchivounivo", "8", IDarchivo } };
            funUpdateDelete(tx, "Delete_tblp_SP_ArchivoContrato", array);


            string[,] arrayh = new string[,] { { "@IDHarchivounivo", "8", "0" }, { "@IDarchivounivo", "8", IDarchivo }, { "@descripcionarchivounivo", "12", descripcionarchivo }, { "@nombrearchivounivo", "12", nombrearchivo }, { "@fechacaptura", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idanio", "8", idanio }, { "@anio", "12", anio }, { "@fechamovimiento", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@idusuario", "8", Idusuario }, { "@accion", "12", "E" }, { "@usuario", "12", usuario } };
            funInsert(tx, "[dbo].[Insert_tblp_SP_HArchivoContrato]", arrayh);

            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }
    }


    #endregion

    #region tablatemporal_KGS
    public Boolean funInsertTemporalDocumentos(string rutaarchivo, string Rutaraiz, string nombreprocesoobra )
    {
        cnn.Open();
        SqlTransaction tx;
      

        tx = cnn.BeginTransaction();
        try
        {

            //=======================================================================================================================================================================================================================
            //====================================================================== para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================

            string[,] arrayapi = new string[,] { { "@idtemporal", "8", "0" }, { "@idorganismo", "12", "" }, { "@organismo", "8", "0"}, { "@tipocontrato", "8", "0"}, { "@municipio", "8", "0" }, { "@modalidad", "8", "0" }, { "@tipoobra", "8", "0" }, { "@beneficiados", "8", "0" }, { "@inversion", "5", "0" }, { "@numerolicitacion", "12", "" }, { "@periodoinicioejecucion", "4", "01/01/1900" }, { "@periodofinalejecucion", "4", "01/01/1900" }, { "@fechaconclucion", "4", "01/01/1900" }, { "@objeto", "12", "" }, { "@contratista", "8", "0" }, { "@estatus", "8", "0" }, { "@actual", "2", "true" }, { "@supervisor", "12", "NINGUNO" }, { "@supervisorexterno", "12", "NINGUNO" }, { "@guidusuario", "12", "" }, { "@mongoid", "12", "NULL" }, { "@ejercicio", "8", "0" }, { "@idobra", "8", "0" }, { "@idcontrato", "8", "0" }, { "@nocontrato", "12", ""}, { "@procesotabla", "12", nombreprocesoobra }, { "@rutaarchivo", "12", Rutaraiz + rutaarchivo }, { "@accion", "12", "A" }, { "@fechaaccion", "4", String.Format("{0:d} {0:H:mm:ss}", Convert.ToDateTime(DateTime.Now)) }, { "@orden", "8", "1" }, { "@enviadoapi", "2", "false" }, { "@fechaenvio", "4", "01/01/1900" }, { "@idproyectoAPI", "8", "0" }, { "@API", "2", "false" } };
            funInsert(tx, "Insert_tblp_TemporalDocumentos", arrayapi);

            //=======================================================================================================================================================================================================================
            //======================================================================   para generar los respaldos =====================================================================
            //=======================================================================================================================================================================================================================



            tx.Commit();
            return true;
        }
        catch (Exception e)
        {
            tx.Rollback();
            throw new System.InvalidOperationException(e.ToString());
            return false;

        }
        finally
        {
            cnn.Close();

        }

    }

    #endregion
    //------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------SICOFIN-----------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------
}