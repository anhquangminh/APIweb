using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPITE.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace WebAPITE.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(string USERNAME)
        {
            List<About> list = new List<About>();
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
                list.Add(about);
            }

            return View(list);
        }
        readonly string _conn;
        public HomeController()
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

        private string GetQuery(string where, string column)
        {
            if (!string.IsNullOrEmpty(where)|| !string.IsNullOrEmpty(column))
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
            else
            {
                return "";
            }

        }
    }
}
