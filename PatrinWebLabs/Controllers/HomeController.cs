using PagedList;
using PatrinWebLabs.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.DataVisualization.Charting;

namespace PatrinWebLabs.Controllers
{

    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Logger.InitLogger();
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
                    Logger.Log.Error("Попытка несанкционированного доступа!" + ex.Message);
                    return Redirect("/Account/Login");
                }
                if (type == "Rights" && right == "Администратор" || right == "Работник ОК")
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
                    {
                        Logger.Log.Info("Загружена главная страница");
                        return View(allData);
                    }
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
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка загрузки главной страницы!" + ex.Message + "');window.location.href = 'Index';</script>");
            }

        }

        [HttpPost]
        public ActionResult Update(string inputID, string inputName, string inputPlaceN, string inputLine, string inputTurno)
        {
            Logger.InitLogger();
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
                        Logger.Log.Info("Обновлена БД работников");
                        return View("Index", allData);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);
                    return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе обновление!" + ex.Message + "');window.location.href = 'Index';</script>");
                }

            }
        }
        public ActionResult Index(string inputID, string inputName, string inputPlaceN, string inputLine, string inputTurno)
        {
            Logger.InitLogger();
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
                            command.CommandText = string.Format("INSERT INTO Employee (ID, PlaceN, Line, Name, Turno) VALUES ('{0}','{1}','{2}','{3}','{4}')", inputID, inputPlaceN, inputLine, inputName, inputTurno);
                            command.ExecuteNonQuery();
                            command.CommandText = string.Format("INSERT INTO Auth (ID, Password, Powers) VALUES ('{0}','{1}','{2}')", inputID, "0000", "Работник");
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
                        Logger.Log.Info("Добавление работника в БД");
                        return View(allData);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message);
                    return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе добавления!" + ex.Message + "');window.location.href = 'Index';</script>");
                }

            }
        }
        public FileContentResult GetChart1()
        {
            Logger.InitLogger();
            var dates = new List<Tuple<int, string>>(
                  new[]
                         {
                           new Tuple<int, string> (78, "Понедельник"),
                           new Tuple<int, string> (88, "Вторник"),
                           new Tuple<int, string> (90, "Среда"),
                           new Tuple<int, string> (81, "Четверг"),
                           new Tuple<int, string> (81, "Пятница"),
                           new Tuple<int, string> (0, "Суббота"),
                         }
                  );
            string Text = "Индекс IMQ";
            string Name = "Индекс IMQ - кривая";
            var chart = new Chart()
            {
                Width = 900,
                Height = 300,
                BackColor = Color.FromArgb(141, 172, 215),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BackSecondaryColor = Color.CadetBlue,
                BackGradientStyle = GradientStyle.TopBottom,
                BorderlineWidth = 1,
                Palette = ChartColorPalette.BrightPastel,
                BorderlineColor = Color.FromArgb(26, 59, 105),
                RenderType = RenderType.BinaryStreaming
            };
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
            Logger.InitLogger();
            var dates = new List<Tuple<int, string>>(
                  new[]
                         {
                          new Tuple<int, string> (54, "Понедельник"),
                           new Tuple<int, string> (72, "Вторник"),
                           new Tuple<int, string> (88, "Среда"),
                           new Tuple<int, string> (77, "Четверг"),
                           new Tuple<int, string> (89, "Пятница"),
                           new Tuple<int, string> (0, "Суббота"),
                         }
                  );
            string Text = "Показатель бездефектности производства";
            string Name = "Индекс A-Defects - кривая";
            var chart = new Chart()
            {
                Width = 900,
                Height = 300,
                BackColor = Color.FromArgb(229, 43, 80),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BackSecondaryColor = Color.DarkOrchid,
                BackGradientStyle = GradientStyle.TopBottom,
                BorderlineWidth = 1,
                Palette = ChartColorPalette.BrightPastel,
                BorderlineColor = Color.FromArgb(26, 59, 105),
                RenderType = RenderType.BinaryStreaming
            };
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
            var chart = new Chart()
            {
                Width = 900,
                Height = 300,
                BackColor = Color.FromArgb(255, 204, 0),
                BorderlineDashStyle = ChartDashStyle.Solid,
                BackSecondaryColor = Color.WhiteSmoke,
                BackGradientStyle = GradientStyle.TopBottom,
                BorderlineWidth = 1,
                Palette = ChartColorPalette.BrightPastel,
                BorderlineColor = Color.FromArgb(26, 59, 105),
                RenderType = RenderType.BinaryStreaming
            };
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
            Title title = new Title()
            {
                Text = Text,
                ShadowColor = Color.FromArgb(32, 0, 0, 0),
                Font = new Font("Trebuchet MS", 14F, FontStyle.Bold),
                ShadowOffset = 3,
                ForeColor = Color.FromArgb(26, 59, 105)
            };
            return title;
        }
        [NonAction]
        public Series CreateSeries(IList<Tuple<int, string>> results,
       SeriesChartType chartType,
       Color color, string Name)
        {
            var seriesDetail = new Series()
            {
                Name = Name,
                IsValueShownAsLabel = false,
                Color = color,
                ChartType = chartType,
                BorderWidth = 2
            };
            seriesDetail["DrawingStyle"] = "Cylinder";
            seriesDetail["PieDrawingStyle"] = "SoftEdge";
            DataPoint point;

            foreach (var result in results)
            {
                point = new DataPoint()
                {
                    AxisLabel = result.Item2,
                    YValues = new double[] { result.Item1 }
                };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = "Result Chart";

            return seriesDetail;
        }
        [NonAction]
        public Legend CreateLegend()
        {
            var legend = new Legend()
            {
                Name = "Result Chart",
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent,
                Font = new Font(new FontFamily("Trebuchet MS"), 9),
                LegendStyle = LegendStyle.Row
            };
            return legend;
        }
        [NonAction]
        public ChartArea CreateChartArea()
        {
            var chartArea = new ChartArea()
            {
                Name = "Result Chart",
                BackColor = Color.Transparent
            };
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
            Logger.InitLogger();
            Logger.Log.Info("Загружена страница FAQ");
            return View();
        }

        [HttpPost]
        public JsonResult AjaxUpdateDefect(string Str, string Str1)
        {
            try
            {
                Logger.InitLogger();
                string def = Str;
                string date = Str1;
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                    cn.Open();

                    using (SqlCommand command = cn.CreateCommand())
                    {

                        command.CommandText = string.Format("UPDATE Defects SET Defects.Defect_Status='Подтвержден' WHERE Defects.Defect_Name_of_Defect='{0}' AND Defects.Defect_Date='{1}' ", Str, Str1);
                        command.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                Logger.Log.Info("Обновлена таблица БД Дефекты");
                return Json("");
            }
            catch (Exception ex)
            {
                return Json("Ошибка при обнлвении статуса дефекта!"+ex.Message);
            }
        }

        [HttpPost]
        public JsonResult AjaxDelete(string Str, string Str2, string Str3, string Str4, string Str5)
        {
            try
            {
                Logger.InitLogger();
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
                        Logger.Log.Info("Удаление видеофайла");
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
                        Logger.Log.Info("Удаление производственной инструкции");
                        return Json("Удаление " + substrings.Length + " инструкций успешно!");
                    }

                }
            }
            catch (Exception ex)
            {
                return Json("Ошибка при удалении медиаданных!" + ex.Message);
            }
        }
        public JsonResult AjaxDeleteIndex(string Str, string Str2, string Str3, string Str4, string Str5)
        {
            try
            { 
            Logger.InitLogger();
            string tmp = Str + " " + Str2 + " " + Str3 + " " + Str4 + " " + Str5;
            string[] substrings = tmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

                            command.CommandText = string.Format("DELETE FROM Auth WHERE ID='{0}'", substrings[2]);
                            command.ExecuteNonQuery();
                            command.CommandText = string.Format("DELETE FROM Employee WHERE ID='{0}'", substrings[2]);
                            command.ExecuteNonQuery();
                        }
                        cn.Close();
                    }
                    Logger.Log.Info("Удален работник из БД");
                    return Json("Удаление работника из базы успешно!");
                }
            }
            catch (Exception ex)
            {
                return Json("Удаление работника из базы невозможно!" + ex.Message);
            }
        }
        public ActionResult Defects(int? page)
        {
            try
            {
                Logger.InitLogger();
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
                    Logger.Log.Error("Попытка несанкционированный доступ!" + ex.Message);
                    return Redirect("/Account/Login");
                }
                if (type == "Rights" && right == "Администратор" || right == "Работник ОК" || right == "Ремонтная служба")
                {
                    Defects load_def = new Defects();

                    bool check = load_def.CheckNewDefects();

                    if (check)
                    {
                        ViewBag.Message = "OK";
                    }
                    ArrayList allData = new ArrayList();
                    load_def.LoadDefects();

                    IEnumerable<string> workplaces = new string[] {};

                    using (SqlConnection cn = new SqlConnection())
                    {
                        cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                        cn.Open();

                        using (SqlCommand command = cn.CreateCommand())
                        {

                            command.CommandText = string.Format("SELECT NameWorkPlace,PlaceN FROM WorkPlaces WHERE WorkPlaces.NameWorkPlace IS NOT NULL");
                            using (SqlDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    workplaces = workplaces.Concat(new string[] { dr[0].ToString()+ "—" + dr[1].ToString() });
                                }
                            }
                        }
                        cn.Close();
                    }

                    ViewBag.Notifications = workplaces;

                    allData.Add(load_def);
                    var model = load_def.Data2.ToList();

                    int pageNumber = page ?? 1;
                    int pageSize = 5;
                    return View(model.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View("ErrorRight");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе загрузки страницы!" + ex.Message + "');window.location.href = 'Index';</script>");

            }

        }
        public ActionResult Employees()
        {
            try
            {
                Logger.InitLogger();
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
                    Logger.Log.Error("Попытка несанкционированный доступ!" + ex.Message);
                    return Redirect("/Account/Login");
                }
                if (type == "Rights" && right == "Работник ОК")
                {
                    Employess load = new Employess();
                    ArrayList allData = new ArrayList();
                    load.LoadAllData();
                    allData.Add(load);
                    return View(allData);
                }
                else
                {
                    return View("ErrorRight");
                }
            }
            catch (Exception ex)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе загрузки страницы!" + ex.Message + "');window.location.href = 'Index';</script>");
            
            }
        }

        //[OutputCache(Duration = 30, Location = OutputCacheLocation.Downstream)]
        public ActionResult InstructionsAndTests()
        {
            try
            {
                Logger.InitLogger();
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
                    Logger.Log.Error("Попытка несанкционированный доступ!" + ex.Message);
                    return Redirect("/Account/Login");
                }
                if (type == "Rights" && right == "Мастер" || right == "Работник ОК")
                {
                    bool check = true;
                    InstructionsVideos load_inst = new InstructionsVideos();
                    ArrayList inst_data = new ArrayList();
                    load_inst.LoadInstructions(check);
                    inst_data.Add(load_inst);

                    return View(inst_data);
                }
                else
                {
                    return View("ErrorRight");
                }
            }
            catch (Exception ex)
            {
                    return Content("<script language='javascript' type='text/javascript'>alert('Ошибка в ходе загрузки страницы!" + ex.Message + "');window.location.href = 'Index';</script>");
            }
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string PlaceN)
        {
            Logger.InitLogger();
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
                        Logger.Log.Info("Загружен файл" + fileName);
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл загружен успешно! Обновите страницу!'); window.location.href = 'InstructionsAndTests' ; </script>");
                    }
                    else
                    {
                        Logger.Log.Error("Файл не загружен!");
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл еще не загружен или не выбран! Выберите заново! Также укажите № рабочего места!');window.location.href = 'InstructionsAndTests';</script>");
                    }
                }
                else
                {
                    string URL = "http://patrin.ddns.net:90/Instructions/";
                    if (upload != null && PlaceN != "")
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        if (fileName.Contains("jpg")|| fileName.Contains("JPG"))
                        {
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
                            Logger.Log.Info("Загружен файл" + fileName);
                            return Content("<script language='javascript' type='text/javascript'>alert('Файл загружен успешно! Обновите страницу!'); window.location.href = 'InstructionsAndTests' ; </script>");
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Выберите файл формата *JPG!'); window.location.href = 'InstructionsAndTests' ; </script>");
                        }
                    }
                    else
                    {
                        Logger.Log.Error("Файл не загружен!");
                        return Content("<script language='javascript' type='text/javascript'>alert('Файл еще не загружен или не выбран! Выберите заново! Также укажите № рабочего места!');window.location.href = 'InstructionsAndTests';</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка загрузки в БД" + ex.Message);
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка при загрузке в БД!" + ex.Message + "');window.location.href = 'InstructionsAndTests';</script>");
            }
        }
        [HttpPost]
        public JsonResult GetEmployee(string employeeData)
        {
            Logger.InitLogger();
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
            Logger.Log.Info("Загрузка данных о работнике");
            return Json(ID + " " + employeeName + " " + PlaceN + " " + Line + " " + Turno);
        }
        [HttpGet]
        public ActionResult Videos()
        {
            Logger.InitLogger();
            bool check = false;
            InstructionsVideos load_inst = new InstructionsVideos();
            ArrayList inst_data = new ArrayList();
            load_inst.LoadInstructions(check);
            inst_data.Add(load_inst);
            return View("InstructionsAndTests", inst_data);
        }

        public ActionResult Tablets()
        {
            Logger.InitLogger();
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
                    reply = myPing.Send(data.IP, 5000);

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
            Logger.Log.Info("Выполнен AJAX-запрос, планшеты опрошены");
            return PartialView(allData);
        }
        public ActionResult Notification(string Type, string inputDescription, string Notifications)
        {
            try
            {
                if (Type == "" || inputDescription == "")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Поля оповещения не заполнены!');window.location.href = 'Defects';</script>");
                }
                else
                {
                    string id = Notifications.Split(new Char[] { '—' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    var applicationID = "AAAACy-503E:APA91bEFVnmz9bEExoaO_97h7zfu9gVNTriNrkaS76G5X3GyS5oLenSchuAFDOnXv9qU7dRlLn7e1gl1dI5kNWHvWy0Le0vvu7R6N_ke_jJGkhJxVSfXcIiHypBuV8JDUMrxFLrACOQu";
                    var senderId = "48045347697";
                    string deviceId = "/topics/"+id;
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json";
                    var data = new
                    {
                        to = deviceId,
                        notification = new
                        {
                            body =  Type + " " + inputDescription,
                            title = Type,
                        },
                        priority = "high"

                    };

                    var serializer = new JavaScriptSerializer();
                    var json = serializer.Serialize(data);
                    byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (WebResponse tResponse = tRequest.GetResponse())
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    string sResponseFromServer = tReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Оповещение успешно отправлено!');window.location.href = 'Defects';</script>");
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при отправке оповещения" + ex.Message);
                return Content("<script language='javascript' type='text/javascript'>alert('Ошибка при загрузке в БД!" + ex.Message + "');window.location.href = 'Defects';</script>");
            }
        }
        [HttpGet]
        public ActionResult Tests()
        {
            ArrayList allData = new ArrayList();
            string path = Server.MapPath("~/Content/client_secret.json");
            string path2 = Server.MapPath("~/Content/");
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT DISTINCT Tests.TestNum FROM Tests");
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Tests test = new Tests();
                            test.LoadTestResults(path, path2, (Convert.ToString(dr[0])));
                            allData.Add(test);
                        }
                    }
                }
                cn.Close();
            }
            return View(allData);
        }
        public ActionResult TestsLoad()
        {
            string[] URL = new string[10];
            string request = "https://script.google.com/macros/s/AKfycbxF-FZXfFdrX_JoDBb1h8AvctzEgq0K3tN-s_J4JwTJ/dev?URL=";
            int i = 0;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT TestURL FROM Tests");
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        URL[i] = dr[0].ToString();
                        i++;
                    }
                }
                return Json(request+URL[0]);
            }
        }

    }
}
