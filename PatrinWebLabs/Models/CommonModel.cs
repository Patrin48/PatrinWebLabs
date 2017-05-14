using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
}



