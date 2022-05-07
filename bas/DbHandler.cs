using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;

public enum DbEnum
{
    MembershipDb=1,
    PrimaryDb=2
}

public class DbHandler
{
    private string _conString;
    private string _logDir;
    private string _lastError;
    public DbHandler(string connectstring, string strLogDir)
    {
        _conString = connectstring;

        _logDir = strLogDir;
    }
    
    public DbHandler(DbEnum dbsource)
    {
        switch (dbsource)
        {
            case DbEnum.MembershipDb:
                _conString = @ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                break;
            case DbEnum.PrimaryDb:
                _conString = @ConfigurationManager.ConnectionStrings["ApplicationPrimary"].ConnectionString;
                break;

        }
        

        _logDir = @ConfigurationManager.AppSettings["LogsFolder"];
    }
    public string GetLastError()
    {
        return _lastError;
    }
    public bool RunSql(string strSQL, object param = null, int? timeout_seconds = null)
    {
        _lastError = null;
        try
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Execute(strSQL, param, null, timeout_seconds);

                return true;

            }
        }
        catch (Exception e)
        {
            log_error(e, strSQL);
            return false;
        }


    }

    public T Load<T>(string strSQL)
    {
        _lastError = null;
        using (SqlConnection con = new SqlConnection(_conString))
        {
            try
            {
                return con.Query<T>(strSQL).FirstOrDefault();
            }
            catch (Exception e)
            {
                //log_error(e, strSQL).GetAwaiter().GetResult();
                log_error(e, strSQL);
                return default(T);
            }

        }
    }


    public System.Data.DataTable GetDataTable(string strSQL)
    {
        _lastError = null;
        System.Data.DataTable dt = new System.Data.DataTable();

        using (SqlConnection con = new SqlConnection(_conString))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSQL;


            cmd.Connection = con;


            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            try
            {

                adapter.Fill(dt);
            }
            catch (Exception e)
            {

                log_error(e, strSQL);
            }

            con.Close();
            return dt;

        }
    }


    private void log_error(Exception e, string strSQL, object param = null)
    {
        _lastError = e.Message;
        var filePath = string.Format("{0}\\sql-error-{1}.log", _logDir, DateTime.Now.ToString("yyyy.MM.dd"));

        try
        {
            System.IO.File.AppendAllText(filePath, "------------------------------" + Environment.NewLine);
            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString() + Environment.NewLine);
            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString() + Environment.NewLine);
            System.IO.File.AppendAllText(filePath, "SQL:" + strSQL + Environment.NewLine);
            System.IO.File.AppendAllText(filePath, "ERROR:" + e.Message + Environment.NewLine);
            if (param != null)
            {
                System.IO.File.AppendAllText(filePath, "PARAMS:" + param.ToString() + Environment.NewLine);
            }
        }catch
        {

        }
        

        //using (System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.Write, 4096, true))
        //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(stream))
        //{
        //    await sw.WriteLineAsync("------------------------------" + Environment.NewLine);
        //    await sw.WriteLineAsync(DateTime.Now.ToString() + Environment.NewLine);



        //    await sw.WriteLineAsync("SQL:" + strSQL + Environment.NewLine);
        //    if (param != null)
        //    {
        //        await sw.WriteLineAsync("PARAMS:" + param.ToString() + Environment.NewLine);
        //    }

        //    await sw.WriteLineAsync("ERROR:" + e.Message + Environment.NewLine);


        //}


    }

}

