using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using PatrinWebLabs.Models;
using System.Collections;
using System.Web;
using System.Net;
using PagedList;
using System.Linq;
using System.Web.UI;
using System.Security.Claims;
using System.Net.NetworkInformation;

namespace PatrinWebLabs.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ClaimsIdentity user = (ClaimsIdentity)(User.Identity);
            string type = string.Empty;
            string right = string.Empty;

            IEnumerable<Claim> claims = user.Claims;

            try
            {
                type = claims.ElementAt(4).Type;
                right = claims.ElementAt(4).Value;
            }
            catch (Exception ex)
            {
                return Redirect("/Account/Login");
            }
            if (type == "Rights" && right == "Администратор")
            {
                string browser = HttpContext.Request.Browser.Browser;
                string user_agent = HttpContext.Request.UserAgent;


                Employess load = new Employess();
                Tablets load_tab = new Tablets();
                Defects load_def = new Defects();

                ArrayList allData = new ArrayList();

                load.LoadData();
                load_tab.PingAndLoadTablets();
                load_def.LoadDefects();

                allData.Add(load);
                allData.Add(load_def);
                allData.Add(load_tab);

                if (user_agent.Contains("Chrome") || (user_agent.Contains("Mozilla")))
                    return View(allData);
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Сайт в данном браузере работает некорректно! Дальнейшая работа невозможна!');window.location.href = 'Index';</script>");
                }
            }

            else
            {
                return View("ErrorRight");
            }


        }

        [HttpPost]
        public ActionResult Update(string inputID, string inputName, string inputPlaceN, string inputLine, string inputTurno)
        {
            if (inputID == "" || inputName == "" || inputPlaceN == "" || inputLine == "" || inputTurno == "")
                return Content("<script language='javascript' type='text/javascript'>alert('Одно из полей пусто!');window.location.href = 'Index';</script>");
            else
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection())
                    {
                        cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                        cn.Open();

                        using (SqlCommand command = cn.CreateCommand())
                        {

                            command.CommandText = string.Format("UPDATE Employee SET Employee.ID='{0}', Employee.PlaceN='{1}', Employee.Line='{2}', Employee.Name='{3}', Employee.Turno='{4}' WHERE Employee.ID='{0}'", inputID, inputPlaceN, inputLine, inputName, inputTurno);
                            command.ExecuteNonQuery();

                        }
                        cn.Close();
                        Employess load = new Employess();
                        Tablets load_tab = new Tablets();
                        Defects load_def = new Defects();

                        ArrayList allData = new ArrayList();

                        load.LoadData();
                        load_tab.PingAndLoadTablets();
                        load_def.LoadDefects();

                        allData.Add(load);
                        allData.Add(load_def);
                        allData.Add(load_tab);
                        return View("Index", allData);
                    }
                }
                catch
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе обновление!');window.location.href = 'Index';</script>");
                }

            }
        }
        public ActionResult Index(string inputID, string inputName, string inputPlaceN, string inputLine, string inputTurno)
        {
            if (inputID == "" || inputName == "" || inputPlaceN == "" || inputLine == "" || inputTurno == "")
                return Content("<script language='javascript' type='text/javascript'>alert('Одно из полей пусто!');window.location.href = 'Index';</script>");
            else
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection())
                    {
                        cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                        cn.Open();

                        using (SqlCommand command = cn.CreateCommand())
                        {

                            command.CommandText = string.Format("INSERT INTO Employee (ID, PlaceN, Line, Name, Turno) VALUES ('{0}','{1}','{2}','{3}','{4}')", inputID, inputName, inputPlaceN, inputLine, inputTurno);
                            command.ExecuteNonQuery();

                        }
                        cn.Close();
                        Employess load = new Employess();
                        Tablets load_tab = new Tablets();
                        Defects load_def = new Defects();

                        ArrayList allData = new ArrayList();

                        load.LoadData();
                        load_tab.PingAndLoadTablets();
                        load_def.LoadDefects();

                        allData.Add(load);
                        allData.Add(load_def);
                        allData.Add(load_tab);
                        return View(allData);
                    }
                }
                catch
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе добавления!');window.location.href = 'Index';</script>");
                }

            }
        }
        public FileContentResult GetChart1()
        {
            var dates = new List<Tuple<int, string>>(
                  new[]
                         {
                           new Tuple<int, string> (65, "Январь"),
                           new Tuple<int, string> (69, "Февраль"),
                           new Tuple<int, string> (90, "Март"),
                           new Tuple<int, string> (81, "Апрель"),
                           new Tuple<int, string> (81, "Май"),
                           new Tuple<int, string> (55, "Июнь"),
                           new Tuple<int, string> (66, "Июль"),
                           new Tuple<int, string> (89, "Август"),
                           new Tuple<int, string> (91, "Сентябрь"),
                           new Tuple<int, string> (97, "Октябрь"),
                           new Tuple<int, string> (87, "Ноябрь"),
                           new Tuple<int, string> (84, "Декабрь"),
                         }
                  );
            string Text = "Индекс IMQ";
            string Name = "Индекс IMQ - кривая";
            var chart = new Chart();
            chart.Width = 900;
            chart.Height = 300;
            chart.BackColor = Color.FromArgb(141, 172, 215);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.CadetBlue;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(Text));
            chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(dates, SeriesChartType.Line, Color.Red, Name));
            chart.ChartAreas.Add(CreateChartArea());
            var ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        public FileContentResult GetChart2()
        {
            var dates = new List<Tuple<int, string>>(
                  new[]
                         {
                           new Tuple<int, string> (89, "Январь"),
                           new Tuple<int, string> (76, "Февраль"),
                           new Tuple<int, string> (98, "Март"),
                           new Tuple<int, string> (67, "Апрель"),
                           new Tuple<int, string> (89, "Май"),
                           new Tuple<int, string> (55, "Июнь"),
                           new Tuple<int, string> (89, "Июль"),
                           new Tuple<int, string> (89, "Август"),
                           new Tuple<int, string> (99, "Сентябрь"),
                           new Tuple<int, string> (94, "Октябрь"),
                           new Tuple<int, string> (87, "Ноябрь"),
                           new Tuple<int, string> (84, "Декабрь"),
                         }
                  );
            string Text = "Показатель бездефектности производства";
            string Name = "Индекс A-Defects - кривая";
            var chart = new Chart();
            chart.Width = 900;
            chart.Height = 300;
            chart.BackColor = Color.FromArgb(229, 43, 80);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.DarkOrchid;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(Text));
            chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(dates, SeriesChartType.Line, Color.AliceBlue, Name));
            chart.ChartAreas.Add(CreateChartArea());
            var ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        public FileContentResult GetChart3()
        {
            var dates = new List<Tuple<int, string>>(
                  new[]
                         {
                          new Tuple<int, string> (89, "Январь"),
                           new Tuple<int, string> (76, "Февраль"),
                           new Tuple<int, string> (98, "Март"),
                           new Tuple<int, string> (89, "Апрель"),
                           new Tuple<int, string> (89, "Май"),
                           new Tuple<int, string> (55, "Июнь"),
                           new Tuple<int, string> (89, "Июль"),
                           new Tuple<int, string> (76, "Август"),
                           new Tuple<int, string> (89, "Сентябрь"),
                           new Tuple<int, string> (89, "Октябрь"),
                           new Tuple<int, string> (87, "Ноябрь"),
                           new Tuple<int, string> (84, "Декабрь"),
                         }
                  );
            string Text = "Показатель FPY";
            string Name = "Индекс FPY - кривая";
            var chart = new Chart();
            chart.Width = 900;
            chart.Height = 300;
            chart.BackColor = Color.FromArgb(255, 204, 0);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.WhiteSmoke;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(Text));
            chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(dates, SeriesChartType.Line, Color.DarkOliveGreen, Name));
            chart.ChartAreas.Add(CreateChartArea());
            var ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }
        [NonAction]
        public Title CreateTitle(string Text)
        {
            Title title = new Title();
            title.Text = Text;
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);

            return title;
        }
        [NonAction]
        public Series CreateSeries(IList<Tuple<int, string>> results,
       SeriesChartType chartType,
       Color color, string Name)
        {
            var seriesDetail = new Series();
            seriesDetail.Name = Name;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint point;

            foreach (var result in results)
            {
                point = new DataPoint();
                point.AxisLabel = result.Item2;
                point.YValues = new double[] { result.Item1 };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Result Chart";

            return seriesDetail;
        }
        [NonAction]
        public Legend CreateLegend()
        {
            var legend = new Legend();
            legend.Name = "Result Chart";
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }
        [NonAction]
        public ChartArea CreateChartArea()
        {
            var chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            return chartArea;
        }

        public ActionResult FAQ()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AjaxUpdateDefect(string Str, string Str1)
        {
            string def = Str;
            string date = Str1;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("UPDATE Defects SET Defects.Defect_Status='Подтвержден' WHERE Defects.Defect_Name_of_Defect='{0}'", Str);
                    command.ExecuteNonQuery();
                }
                cn.Close();
            }
            return Json("");
        }

        [HttpPost]
        public JsonResult AjaxDelete(string Str, string Str2, string Str3, string Str4, string Str5)
        {

            string tmp = Str + " " + Str2 + " " + Str3 + " " + Str4 + " " + Str5;
            string[] substrings = tmp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmp.Contains("mp4"))
            {
                if (substrings.Length == 0)
                {
                    return Json("Ни одного видеофайла не выбрано не выбрано!");
                }
                else
                {
                    InstructionsVideos inst_delete = new InstructionsVideos();
                    for (int i = 0; i < substrings.Length; i++)
                    {
                        inst_delete.DeleteVideos(substrings[i]);
                    }
                    return Json("Удаление " + substrings.Length + " видеофайлов успешно!");
                }
            }
            else
            {
                if (substrings.Length == 0)
                {
                    return Json("Ни одной инструкции не выбрано!");
                }
                else
                {
                    InstructionsVideos inst_delete = new InstructionsVideos();
                    for (int i = 0; i < substrings.Length; i++)
                    {
                        inst_delete.DeleteInstruction(substrings[i]);
                    }

                    return Json("Удаление " + substrings.Length + " инструкций успешно!");
                }

            }
        }
        public JsonResult AjaxDeleteIndex(string Str, string Str2, string Str3, string Str4, string Str5)
        {
            string tmp = Str + " " + Str2 + " " + Str3 + " " + Str4 + " " + Str5;
            string[] substrings = tmp.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (substrings.Length == 0)
            {
                return Json("Ни одного работника не выбрано!");
            }
            else
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                    cn.Open();

                    using (SqlCommand command = cn.CreateCommand())
                    {

                        command.CommandText = string.Format("DELETE FROM Auth WHERE ID='{0}'", substrings[7]);
                        command.ExecuteNonQuery();
                        command.CommandText = string.Format("DELETE FROM Employee WHERE ID='{0}'", substrings[7]);
                        command.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                return Json("Удаление работника из базы успешно!");
            }
        }
        public ActionResult Defects(int? page)
        {
            Defects load_def = new Defects();

            bool check = load_def.CheckNewDefects();

            if (check)
            {
                ViewBag.Message = "OK";
            }
            ArrayList allData = new ArrayList();
            load_def.LoadDefects();


            allData.Add(load_def);
            var model = load_def.Data2.ToList();

            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(model.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Employees()
        {
            Employess load = new Employess();
            ArrayList allData = new ArrayList();
            load.LoadAllData();
            allData.Add(load);
            return View(allData);
        }

        [OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult InstructionsAndTests()
        {
            bool check = true;
            InstructionsVideos load_inst = new InstructionsVideos();
            ArrayList inst_data = new ArrayList();
            load_inst.LoadInstructions(check);
            inst_data.Add(load_inst);

            return View(inst_data);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string PlaceN)
        {
            try
            {

                string placeNumber = PlaceN;
                if (upload.ContentType == "video/mp4")
                {
                    if (upload != null && PlaceN != "")
                    {
                        string URL = "http://patrin.ddns.net:90/Videos/";
                        string fileName = Path.GetFileName(upload.FileName);
                        upload.SaveAs(Server.MapPath("~/TempFiles/" + fileName));
                        using (var client = new WebClient())
                        {
                            client.Credentials = new NetworkCredential("Patrin48", "18swlgnm");
                            client.UploadFile("http://patrin.ddns.net:90/Videos/", Server.MapPath("~/TempFiles/" + fileName));
                        }
                        string connectStr = "Data Source=patrin.ddns.net,1433;Integrated Security=False;User ID=sa;Password=18swlgnm;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        SqlConnection connection = new SqlConnection(connectStr);
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand(string.Format("IF EXISTS (SELECT * FROM Employee WHERE PlaceN = {1}) UPDATE Employee SET VideoURL = {0} WHERE PlaceN={1}", "'" + URL + fileName + "'", placeNumber), connection);
                        cmd.ExecuteNonQuery();
                        System.IO.File.Delete(Server.MapPath("~/TempFiles/" + fileName));
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл загружен успешно! Обновите страницу!'); window.location.href = 'InstructionsAndTests' ; </script>");
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл еще не загружен или не выбран! Выберите заново! Также укажите № рабочего места!');window.location.href = 'InstructionsAndTests';</script>");
                    }
                }
                else
                {
                    string URL = "http://patrin.ddns.net:90/Instructions/";
                    if (upload != null && PlaceN != "")
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        upload.SaveAs(Server.MapPath("~/TempFiles/" + fileName));
                        using (var client = new WebClient())
                        {
                            client.Credentials = new NetworkCredential("Patrin48", "18swlgnm");
                            client.UploadFile("http://patrin.ddns.net:90/Instructions/", Server.MapPath("~/TempFiles/" + fileName));
                        }
                        string connectStr = "Data Source=patrin.ddns.net,1433;Integrated Security=False;User ID=sa;Password=18swlgnm;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                        SqlConnection connection = new SqlConnection(connectStr);
                        connection.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = new SqlCommand(string.Format("IF EXISTS (SELECT * FROM Instructions WHERE PlaceN = {1}) UPDATE Instructions SET InstURL = {0} WHERE PlaceN={1} ELSE INSERT INTO Instructions VALUES({0}, {1})", "'" + URL + fileName + "'", placeNumber), connection);

                        cmd.ExecuteNonQuery();
                        System.IO.File.Delete(Server.MapPath("~/TempFiles/" + fileName));
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл загружен успешно! Обновите страницу!'); window.location.href = 'InstructionsAndTests' ; </script>");
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл еще не загружен или не выбран! Выберите заново! Также укажите № рабочего места!');window.location.href = 'InstructionsAndTests';</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка при загрузке в БД! " + ex.Message + "');window.location.href = 'InstructionsAndTests';</script>");
            }
        }
        [HttpPost]
        public JsonResult GetEmployee(string employeeData)
        {

            string ID = string.Empty;
            string Turno = string.Empty;
            string employeeName = employeeData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0] + " " + employeeData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1];
            string Line = employeeData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[2];
            string PlaceN = employeeData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[3];
            string connect = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
            string query = string.Format("SELECT E.ID, E.Name,E.PlaceN,E.Line,E.Turno FROM Employee E WHERE E.Name='{0}' AND  E.Line='{1}' AND E.Turno='{2}'", employeeName, Line, PlaceN);

            using (SqlConnection conn = new SqlConnection(connect))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            ID = rdr[0].ToString();
                            employeeName = rdr[1].ToString();
                            PlaceN = rdr[2].ToString();
                            Line = rdr[3].ToString();
                            Turno = rdr[4].ToString();
                        }
                    }
                }
            }
            return Json(ID + " " + employeeName + " " + PlaceN + " " + Line + " " + Turno);
        }
        [HttpGet]
        public ActionResult Videos()
        {
            bool check = false;
            InstructionsVideos load_inst = new InstructionsVideos();
            ArrayList inst_data = new ArrayList();
            load_inst.LoadInstructions(check);
            inst_data.Add(load_inst);
            return View("InstructionsAndTests", inst_data);
        }

        public ActionResult Tablets()
        {
            ArrayList allData = new ArrayList();
            try
            {
                Tablets load_tab = new Tablets();
                load_tab.PingAndLoadTablets();

                allData.Add(load_tab);
                Ping myPing = new Ping();
                PingReply reply = null;

                foreach (var data in load_tab.Data3)
                {
                    reply = myPing.Send(data.IP, 1000);

                    if (reply.Status.ToString() != "TimedOut")
                    {
                        data.Tablets_Status = "В сети";

                    }
                    else
                    {
                        data.Tablets_Status = "Не в сети";
                    }
                }
                
            }
            catch
            {

            }

            return PartialView(allData);
        }
    }
}
