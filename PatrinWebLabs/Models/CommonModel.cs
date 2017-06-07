using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Threading;


namespace PatrinWebLabs.Models
{


    public class Employess
    {
        public List<Employess> Data1 = new List<Employess>();

        public string ID { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string NameWorkPlace { get; set; }
        public string Turno { get; set; }
        public string Status { get; set; }

        public Employess()
        {

        }

        public void LoadData()
        {
            Employess temp;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT TOP 10 Employee.ID, Employee.Name, Employee.Line, WorkPlaces.NameWorkPlace,Employee.Turno, Employee.Status FROM Employee, WorkPlaces WHERE Employee.PlaceN=WorkPlaces.PlaceN ORDER BY Employee.Line");
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            temp = new Employess();
                            temp.ID = dr[0].ToString();
                            temp.Name = dr[1].ToString();
                            temp.Line = dr[2].ToString();
                            temp.NameWorkPlace = dr[3].ToString();
                            temp.Turno = dr[4].ToString();
                            if (dr[5].ToString() == "ONLINE")
                            {
                                temp.Status = "На рабочем месте";
                            }
                            else
                            {
                                temp.Status = "Отсутствует";
                            }
                            Data1.Add(temp);
                        }
                    }
                }
                cn.Close();
            }
        }

        public void LoadAllData()
        {
            Employess temp;
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT TOP 10 Employee.ID, Employee.Name, Employee.Line, WorkPlaces.NameWorkPlace,Employee.Turno, Employee.Status FROM Employee, WorkPlaces WHERE Employee.PlaceN=WorkPlaces.PlaceN ORDER BY Employee.Line");
                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            temp = new Employess();
                            temp.ID = dr[0].ToString();
                            temp.Name = dr[1].ToString();
                            temp.Line = dr[2].ToString();
                            temp.NameWorkPlace = dr[3].ToString();
                            temp.Turno = dr[4].ToString();
                            if (dr[5].ToString() == "ONLINE")
                            {
                                temp.Status = "На рабочем месте";
                            }
                            else
                            {
                                temp.Status = "Отсутствует";
                            }
                            Data1.Add(temp);
                        }
                    }
                }
                cn.Close();
            }

        }
    }
    public class Defects
    {
        Defects tmp;
        public List<Defects> Data2 = new List<Defects>();
        public string DefectName { get; set; }
        public string DefectLine { get; set; }
        public string DefectNameWorkPlace { get; set; }
        public string DefectTurno { get; set; }
        public string DefectDescription { get; set; }
        public string DefectDate { get; set; }
        public string DefectStatus { get; set; }

        public string DefectImage { get; set; }

        public bool CheckNewDefects()
        {
            using (SqlConnection cn = new SqlConnection())
            {
                
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    //command.CommandText = string.Format("SELECT D.Defect_Name_of_Defect,D.Defect_Line,WorkPlaces.NameWorkPlace,D.Defect_Turno,D.Defect_Description,D.Defect_Date,D.Defect_Status From Defects D, WorkPlaces WHERE D.Defect_WorkPlace=WorkPlaces.PlaceN");
                    command.CommandText = string.Format("SELECT D.Defect_Name_of_Defect,D.Defect_Line,D.Defect_WorkPlace,D.Defect_Turno,D.Defect_Description,D.Defect_Date,D.Defect_Status From Defects D WHERE D.Defect_Date >= DATEADD(day, -1, GETDATE())");
                    int num = command.ExecuteNonQuery();
                    bool check = false;
                    if (num>-1)
                    {
                        check = true;
                    }
                    cn.Close();
                    return check;
                }
            }
        }
        public void LoadDefects()
        {

            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    //command.CommandText = string.Format("SELECT D.Defect_Name_of_Defect,D.Defect_Line,WorkPlaces.NameWorkPlace,D.Defect_Turno,D.Defect_Description,D.Defect_Date,D.Defect_Status From Defects D, WorkPlaces WHERE D.Defect_WorkPlace=WorkPlaces.PlaceN");
                    command.CommandText = string.Format("SELECT D.Defect_Name_of_Defect,D.Defect_Line,D.Defect_WorkPlace,D.Defect_Turno,D.Defect_Description,D.Defect_Date,D.Defect_Status, D.Defect_Image From Defects D ORDER BY D.Defect_Date DESC");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tmp = new Defects();
                            tmp.DefectName = (Convert.ToString(reader[0]));
                            tmp.DefectLine = (Convert.ToString(reader[1]));
                            tmp.DefectNameWorkPlace = (Convert.ToString(reader[2]));
                            tmp.DefectTurno = (Convert.ToString(reader[3]));
                            tmp.DefectDescription = (Convert.ToString(reader[4]));
                            tmp.DefectDate = (Convert.ToString(reader[5]));
                            tmp.DefectStatus = (Convert.ToString(reader[6]));
                            tmp.DefectImage = (Convert.ToString(reader[7]));
                            Data2.Add(tmp);
                        }
                    }
                }
                cn.Close();
            }
        }
    }
    public class Tablets
    {

        Tablets tmp;
        public List<Tablets> Data3 = new List<Tablets>();
        public string IP { get; set; }
        public string LineTablet { get; set; }
        public string NameWorkPlaceTablet { get; set; }
        public string Tablets_Status { get; set; }

        public void PingAndLoadTablets()
        {

            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT Tablets.Tablets_IP, Tablets.Tablets_Line, WorkPlaces.NameWorkPlace, Tablets.Tablets_Status FROM Tablets, WorkPlaces WHERE Tablets.Tablets_WorkPlaceN = WorkPlaces.PlaceN");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tmp = new Tablets();
                            tmp.IP = (Convert.ToString(reader[0]));
                            tmp.LineTablet = (Convert.ToString(reader[1]));
                            tmp.NameWorkPlaceTablet = (Convert.ToString(reader[2]));
                            tmp.Tablets_Status = (Convert.ToString(reader[3]));
                            Data3.Add(tmp);
                        }
                    }
                }
                cn.Close();
            }
        }
    }
    public class InstructionsVideos
    {

        InstructionsVideos tmp;
        public List<InstructionsVideos> Data4 = new List<InstructionsVideos>();
        public string URL { get; set; }
        public string WorkPlace { get; set; }

        public void DeleteInstruction(string URL)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {
                    command.CommandText = string.Format("DELETE FROM Instructions WHERE InstURL = '{0}'", URL);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteVideos(string URL)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {
                    command.CommandText = string.Format("UPDATE Employee SET VideoURL=NULL WHERE VideoURL='{0}' ", URL);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void LoadInstructions(bool check)
        {
            if (check == true)
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                    cn.Open();

                    using (SqlCommand command = cn.CreateCommand())
                    {

                        command.CommandText = string.Format("SELECT I.InstURL,WorkPlaces.NameWorkPlace FROM Instructions I, WorkPlaces WHERE I.PlaceN=WorkPlaces.PlaceN ORDER BY I.PlaceN");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmp = new InstructionsVideos();
                                tmp.URL = (Convert.ToString(reader[0]));
                                tmp.WorkPlace = (Convert.ToString(reader[1]));
                                Data4.Add(tmp);
                            }
                        }
                        command.CommandText = string.Format("SELECT E.VideoURL, E.PlaceN FROM Employee E WHERE VideoURL IS NOT NULL ORDER BY E.PlaceN");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmp = new InstructionsVideos();
                                tmp.URL = (Convert.ToString(reader[0]));
                                tmp.WorkPlace = (Convert.ToString(reader[1]));
                                Data4.Add(tmp);
                            }
                        }
                    }
                    cn.Close();
                }
            }
            else
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                    cn.Open();

                    using (SqlCommand command = cn.CreateCommand())
                    {
                        command.CommandText = string.Format("SELECT E.VideoURL, E.PlaceN FROM Employee E WHERE VideoURL IS NOT NULL ORDER BY E.PlaceN");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tmp = new InstructionsVideos();
                                tmp.URL = (Convert.ToString(reader[0]));
                                tmp.WorkPlace = (Convert.ToString(reader[1]));
                                Data4.Add(tmp);
                            }
                        }
                    }
                    cn.Close();
                }
            }
        }
    }
    public class Tests
    {
        Tests tmp;
        public List<Tests> Data5 = new List<Tests>();
        public string TestURL { get; set; }
        public string EmployeeID { get; set; }
        public string RightAnswer { get; set; }
        public string Result { get; set; }
        public string TestName { get; set; }
        public string EmployeeName { get; set; }
        public string TestTableURL { get; set; }
        public double Persentage { get; set; }
        public string TableURL { get; set; }
        public int count { get; set; }

        public void LoadTestResults(string path, string path2, string id)
        {
            UserCredential credential;
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string ApplicationName = "WebResults";
            int rightPercentage = 0;
            int counter = 1;
            int numberOfTest = 0;
            double SumPercent = 0;
            using (var stream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {
               path2= Path.Combine(path2, "credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(path2, true)).Result;
            }
            
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
           
            string scriptComand = "https://script.google.com/macros/s/AKfycbznDBEAC3x5RdItbDoFqHqbDuyzM4nvPdU0UQK-3lQ8/dev?URL=";
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = @"Data Source=patrin.ddns.net,1433;Initial Catalog=OIProject;Persist Security Info=True;User ID=sa;Password=18swlgnm";
                cn.Open();

                using (SqlCommand command = cn.CreateCommand())
                {

                    command.CommandText = string.Format("SELECT Tests.TestNum, Tests.TestURL, Tests.TestRightAnswer, Employee.Name, Tests.TestTableURL, COUNT(TestNum) AS 'Количество' FROM Tests, Employee Where Tests.TestNum = Employee.ID AND TestNum="+ id +"GROUP BY TestNum, TestURL, TestRightAnswer, Name, TestTableURL ORDER BY TestNum");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tmp = new Tests();
                            tmp.EmployeeID = (Convert.ToString(reader[0]));
                            tmp.TestURL = (Convert.ToString(reader[1]));
                            tmp.RightAnswer = (Convert.ToString(reader[2]));
                            tmp.EmployeeName = (Convert.ToString(reader[3]));
                            tmp.TestTableURL = (Convert.ToString(reader[4]));
                            tmp.count = (Convert.ToInt32(reader[5]));
                            tmp.TableURL = "https://docs.google.com/spreadsheets/d/" + tmp.TestTableURL;
                            String spreadsheetId = scriptComand;
                            String range = "B1:B";
                            SpreadsheetsResource.ValuesResource.GetRequest request =
                                    service.Spreadsheets.Values.Get(tmp.TestTableURL, range);
                            ValueRange response = request.Execute();
                            IList<IList<Object>> values = response.Values;
                            tmp.TestName = values[0][0].ToString();
                            try
                            {
                                for (counter=1; counter < values.Count; counter++)
                                {
                                    if (values[counter][0].ToString() == tmp.RightAnswer)
                                    {
                                        rightPercentage++;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                counter++;
                            }
                            finally
                            {
                                for (int i=counter; i< values.Count; i++)
                                {
                                    if (values[i][0].ToString() == tmp.RightAnswer)
                                    {
                                        rightPercentage++;
                                    }
                                }
                            }
                            tmp.Persentage = ((double)(rightPercentage * 100) / (values.Count-2));
                            tmp.Persentage = Math.Round(tmp.Persentage, 0);
                            SumPercent += tmp.Persentage;
                            rightPercentage = 0;
                            Data5.Add(tmp);
                            numberOfTest++;
                        }
                    }
                }
                cn.Close();
            }
        }
    }
}



