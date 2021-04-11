using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;
using WebAPITE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace WebAPITE.Controllers
{
    public class ValuesController : ApiController
    {
        readonly string _conn;
        public ValuesController()
        {
            var builder = new OracleConnectionStringBuilder
            {
                DataSource = "(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.134.171.33)(PORT=1527))(CONNECT_DATA=(SERVICE_NAME=ISCODB)))",
                UserID = "PC",
                Password = "iscpc169!!!!"
            };
            _conn = builder.ConnectionString;
        }
        // GET api/values
        public IHttpActionResult Get(string USERNAME)
        {
            try
            {
                if (!string.IsNullOrEmpty(USERNAME))
                {
                    USERNAME = USERNAME.Replace(".", "-");
                    About about = new About();
                    about.USERNAME = USERNAME.Trim();
                    about.SYSTEM = GetQuery(" USERNAME='" + USERNAME + "' and hr ='SYSTEM' ORDER BY INPUT_DATE DESC", "IPS");
                    about.Port1433 = GetQuery(" USERNAME='" + USERNAME + "' and hr ='1433' ORDER BY INPUT_DATE DESC", "IPS");
                    about.Port445 = GetQuery(" USERNAME='" + USERNAME + "' and hr ='445' ORDER BY INPUT_DATE DESC", "IPS");
                    about.Port3389 = GetQuery(" USERNAME='" + USERNAME + "' and hr ='3389' ORDER BY INPUT_DATE DESC", "IPS");
                    about.USB = GetQuery(" USERNAME='" + USERNAME + "' and hr ='USB' ORDER BY INPUT_DATE DESC", "IPS");
                    about.USBPROTECT = GetQuery(" USERNAME='" + USERNAME + "' and hr ='USBPROTECT' ORDER BY INPUT_DATE DESC", "IPS");
                    about.SYMANTECTUPDATE = GetQuery(" USERNAME='" + USERNAME + "' and hr ='SYMANTECTUPDATE' ORDER BY INPUT_DATE DESC", "IPS");
                    about.VIRUS_PROTECTION = GetQuery(" USERNAME='" + USERNAME + "' and hr ='VIRUS_PROTECTION' ORDER BY INPUT_DATE DESC", "IPS");

                    string output = JsonConvert.SerializeObject(about);
                    return Json(about);
                }
                else
                {
                    throw new Exception("USERNAME is null");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string GetQuery(string where, string column)
        {

            string sql = "select * from MES1.C_PC_CONTROL_T Where ";
            sql = sql + where;
            using (var conn = new OracleConnection(_conn))
            {
                conn.Open();
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        string name = "";
                        foreach (DataRow row in dt.Rows)
                        {
                            name = row[column].ToString();
                        }
                        return name;
                    }
                }
            }

        }

        // GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// POST api/values
        //public void Post([FromBody] string value)
        //{
        //}
        //// PUT api/values/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}
        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //    //string sql = "SELECT  cust_kp_no,EMP_NO,city_lomo,city_word,pcname,username,window,ips  FROM TREND_LINE_PACKAGE_DETAIL";
        //    //using (var conn = new OracleConnection(_conn))
        //    //{
        //    //    conn.Open();
        //    //    using (var cmd = new OracleCommand(sql, conn))
        //    //    {
        //    //        using (var reader = cmd.ExecuteReader())
        //    //        {
        //    //            var dt = new DataTable();
        //    //            dt.Load(reader);
        //    //            foreach (DataRow row in dt.Rows)
        //    //            {
        //    //                string name = row["cust_kp_no"].ToString();
        //    //                string description = row["EMP_NO"].ToString();
        //    //                string icoFileName = row["city_lomo"].ToString();
        //    //                string installScript = row["pcname"].ToString();
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //}
    }
}
