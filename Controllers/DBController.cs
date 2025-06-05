using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using InterventionalCostings.Inventory_Item_Classes;
using InterventionalCostings.CustomViews;

using EnumEmbolisationSystem = InterventionalCostings.Static_Data.StaticData.EnumEmbolisationSystem;
using EnumDilator = InterventionalCostings.Static_Data.StaticData.EnumDilator;
using EnumRadOrReferrer = InterventionalCostings.Static_Data.StaticData.EnumRadOrReferrer;
using EnumCatheter = InterventionalCostings.Static_Data.StaticData.EnumCatheter;
using EnumBalloon = InterventionalCostings.Static_Data.StaticData.EnumBalloon;
using EnumWire = InterventionalCostings.Static_Data.StaticData.EnumWire;
using EnumDressing = InterventionalCostings.Static_Data.StaticData.EnumDressing;
using EnumStent = InterventionalCostings.Static_Data.StaticData.EnumStent;
using EnumMisc = InterventionalCostings.Static_Data.StaticData.EnumMisc;
using EnumCoil = InterventionalCostings.Static_Data.StaticData.EnumCoil;
using EnumContrast = InterventionalCostings.Static_Data.StaticData.EnumContrast;
using EnumSnare = InterventionalCostings.Static_Data.StaticData.EnumSnare;
using EnumSheath = InterventionalCostings.Static_Data.StaticData.EnumSheath;
using EnumItemOrder = InterventionalCostings.Static_Data.StaticData.EnumItemOrder;
using EnumMiscData = InterventionalCostings.Static_Data.StaticData.EnumMiscData;
using EnumNurses = InterventionalCostings.Static_Data.StaticData.EnumNurses;
using EnumCaseStats = InterventionalCostings.Static_Data.StaticData.EnumCaseStatsData;
using EnumCaseItemCountStats = InterventionalCostings.Static_Data.StaticData.EnumCaseItemCountStats;


using System.Windows;
using System.Threading.Tasks;
using System.Threading;
using InterventionalCostings.Static_Data;

namespace InterventionalCostings
{
    public interface IUpdateProgressBar
    {

        void UpdateProgressBar(int newValue);
        void UpdateCurrentTask(string updateText);
        void UpdateTaskDetails(string updateText);
        void UpdateProgressMax(Double newValue);
    }

    class DBController
    {
        private static List<List<string>> selectedRows = new List<List<string>>();

        private static List<CaseStatsData> ListOfCaseStatsData = new List<CaseStatsData>();
        private static List<CaseItemCountHistoryStatsData> ListOfCaseItemCountStatsData = new List<CaseItemCountHistoryStatsData>();
        

        public static string ConnectionString
        {
            get
            {
                return InventoryConnectionString;
            }
        }

        public static string InventoryConnectionString {
            get {
                //return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\rossco\Desktop\SoftwareDevelopment\MedVentory\Inventory.mdf;Integrated Security=True;Connect Timeout=30";
                return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\GITHUB REPOS\MedVentory\Databases\Inventory.mdf"";Integrated Security=True;Connect Timeout=30";
            }
        }

        public static string TestConnectionString
        {
            get { 

                    //return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\rossco\Desktop\SoftwareDevelopment\MedVentory\TestInventory.mdf;Integrated Security=True;Connect Timeout=30";
                    return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\GITHUB REPOS\MedVentory\Databases\Inventory.mdf"";Integrated Security=True;Connect Timeout=30";
            }
        }



        public static List<List<string>> GetRadiologists(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumRadOrReferrer.ID).ToString().Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Prefix).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.FirstName).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Surname).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Specialty).Trim()
                                };
                                selectedRows.Add(row);
                            };
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetReferrers(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumRadOrReferrer.ID).ToString().Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Prefix).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.FirstName).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Surname).Trim(),
                                    reader.GetString((int)EnumRadOrReferrer.Specialty).Trim()



                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }                

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetDressings(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumDressing.ID).ToString().Trim(),
                                    reader.GetString((int)EnumDressing.Brand).Trim(),
                                    reader.GetString((int)EnumDressing.Title).Trim(),
                                    reader.GetDecimal((int)EnumDressing.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumDressing.RefNumber).Trim(),
                                    reader.GetString((int)EnumDressing.Category).Trim(),
                                    reader.GetInt32((int)EnumDressing.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetMiscItems(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumMisc.ID).ToString().Trim(),
                                    reader.GetString((int)EnumMisc.Brand).Trim(),
                                    reader.GetString((int)EnumMisc.Title).Trim(),
                                    reader.GetString((int)EnumMisc.SubTitle).Trim(),
                                    reader.GetDecimal((int)EnumMisc.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumMisc.RefNumber).Trim(),
                                    reader.GetString((int)EnumMisc.Category).Trim(),
                                    reader.GetInt32((int)EnumMisc.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetWires(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumWire.ID).ToString().Trim(),
                                    reader.GetString((int)EnumWire.Brand).Trim(),
                                    reader.GetString((int)EnumWire.Title).Trim(),
                                    reader.GetString((int)EnumWire.SubTitle).Trim(),
                                    reader.GetString((int)EnumWire.Diameter).Trim(),
                                    reader.GetString((int)EnumWire.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumWire.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumWire.RefNumber).Trim(),
                                    reader.GetString((int)EnumWire.Category).Trim(),
                                    reader.GetInt32((int)EnumWire.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);

                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetCatheters(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumCatheter.ID).ToString().Trim(),
                                    reader.GetString((int)EnumCatheter.Brand).Trim(),
                                    reader.GetString((int)EnumCatheter.Title).Trim(),
                                    reader.GetString((int)EnumCatheter.SubTitle).Trim(),
                                    reader.GetString((int)EnumCatheter.Diameter).Trim(),
                                    reader.GetString((int)EnumCatheter.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumCatheter.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumCatheter.RefNumber).Trim(),
                                    reader.GetString((int)EnumCatheter.Category).Trim(),
                                    reader.GetInt32((int)EnumCatheter.PackSize).ToString().Trim()
                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetStents(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                { 
                                    reader.GetInt32((int)EnumStent.ID).ToString().Trim(),
                                    reader.GetString((int)EnumStent.Brand).Trim(),
                                    reader.GetString((int)EnumStent.Title).Trim(),
                                    reader.GetString((int)EnumStent.SubTitle).Trim(),
                                    reader.GetString((int)EnumStent.Diameter).Trim(),
                                    reader.GetString((int)EnumStent.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumStent.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumStent.RefNumber).Trim(),
                                    reader.GetString((int)EnumStent.Category).Trim(),
                                    reader.GetInt32((int)EnumStent.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetSheaths(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumSheath.ID).ToString().Trim(),
                                    reader.GetString((int)EnumSheath.Brand).Trim(),
                                    reader.GetString((int)EnumSheath.Title).Trim(),
                                    reader.GetString((int)EnumSheath.Diameter).Trim(),
                                    reader.GetString((int)EnumSheath.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumSheath.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumSheath.RefNumber).Trim(),
                                    reader.GetString((int)EnumSheath.Category).Trim(),
                                    reader.GetInt32((int)EnumSheath.PackSize).ToString().Trim()
                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetDilators(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumDilator.ID).ToString().Trim(),
                                    reader.GetString((int)EnumDilator.Brand).Trim(),
                                    reader.GetString((int)EnumDilator.Title).Trim(),
                                    reader.GetString((int)EnumDilator.Diameter).Trim(),
                                    reader.GetString((int)EnumDilator.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumDilator.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumDilator.RefNumber).Trim(),
                                    reader.GetString((int)EnumDilator.Category).Trim(),
                                    reader.GetInt32((int)EnumDilator.PackSize).ToString().Trim()


                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetBalloons(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumBalloon.ID).ToString().Trim(),
                                    reader.GetString((int)EnumBalloon.Brand).Trim(),
                                    reader.GetString((int)EnumBalloon.Title).Trim(),
                                    reader.GetString((int)EnumBalloon.Diameter).Trim(),
                                    reader.GetString((int)EnumBalloon.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumBalloon.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumBalloon.RefNumber).Trim(),
                                    reader.GetString((int)EnumBalloon.Category).Trim(),
                                    reader.GetInt32((int)EnumBalloon.PackSize).ToString().Trim()
                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetEmbolisationSystems(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumEmbolisationSystem.ID).ToString().Trim(),
                                    reader.GetString((int)EnumEmbolisationSystem.Brand).Trim(),
                                    reader.GetString((int)EnumEmbolisationSystem.Title).Trim(),
                                    reader.GetString((int)EnumEmbolisationSystem.SubTitle).Trim(),
                                    reader.GetDecimal((int)EnumEmbolisationSystem.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumEmbolisationSystem.RefNumber).Trim(),
                                    reader.GetString((int)EnumEmbolisationSystem.Category).Trim(),
                                    reader.GetInt32((int)EnumEmbolisationSystem.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetCoils(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumCoil.ID).ToString().Trim(),
                                    reader.GetString((int)EnumCoil.Brand).Trim(),
                                    reader.GetString((int)EnumCoil.Title).Trim(),
                                    reader.GetString((int)EnumCoil.SubTitle).Trim(),
                                    reader.GetString((int)EnumCoil.Diameter).Trim(),
                                    reader.GetString((int)EnumCoil.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumCoil.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumCoil.RefNumber).Trim(),
                                    reader.GetString((int)EnumCoil.Category).Trim(),
                                    reader.GetInt32((int)EnumCoil.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetContrast(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumContrast.ID).ToString().Trim(),
                                    reader.GetString((int)EnumContrast.Brand).Trim(),
                                    reader.GetString((int)EnumContrast.Strength).Trim(),
                                    reader.GetString((int)EnumContrast.Volume).Trim(),
                                    reader.GetDecimal((int)EnumContrast.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumContrast.RefNumber).Trim(),
                                    reader.GetString((int)EnumContrast.Category).Trim(),
                                    reader.GetInt32((int)EnumContrast.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetSnares(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumSnare.ID).ToString().Trim(),
                                    reader.GetString((int)EnumSnare.Brand).Trim(),
                                    reader.GetString((int)EnumSnare.Title).Trim(),
                                    reader.GetString((int)EnumSnare.Diameter).Trim(),
                                    reader.GetString((int)EnumSnare.Dimensions).Trim(),
                                    reader.GetDecimal((int)EnumSnare.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumSnare.RefNumber).Trim(),
                                    reader.GetString((int)EnumSnare.Category).Trim(),
                                    reader.GetInt32((int)EnumSnare.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<List<string>> GetMiscData(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string id = reader.GetInt32((int)EnumMiscData.ID).ToString().Trim();

                                string syncCategories;
                                string caseLocations;
                                string dbTables;


                                if (reader.IsDBNull((int)EnumMiscData.SyncCategories))
                                {
                                    syncCategories = "";
                                }
                                else
                                {
                                    syncCategories = reader.GetString((int)EnumMiscData.SyncCategories).Trim();
                                }

                                if (reader.IsDBNull((int)EnumMiscData.CaseLocations))
                                {
                                    caseLocations = "";
                                }
                                else
                                {
                                    caseLocations = reader.GetString((int)EnumMiscData.CaseLocations).Trim();
                                }

                                if (reader.IsDBNull((int)EnumMiscData.DbTables))
                                {
                                    dbTables = "";
                                }
                                else
                                {
                                    dbTables = reader.GetString((int)EnumMiscData.DbTables).Trim();
                                }



                                List<string> row = new List<string>
                                {
                                    id,
                                    syncCategories,
                                    caseLocations,
                                    dbTables
                                };
                                selectedRows.Add(row);
  

                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static string GetEmailAddressFromUserName(string userName)
        {
            string ordererEmailAddress = "";

            string sqlQuery = string.Format("SELECT email " +
                "FROM InterventionalStaff " +
                "WHERE userName = '{0}'", userName.ToLower());


            //Debugging
            //MessageBox.Show("sqlQuery is: "+ sqlQuery);

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        ordererEmailAddress = (string)command.ExecuteScalar();

                        //Debugging
                        //MessageBox.Show("email address is: " + ordererEmailAddress);
                    }
                }
            }
            catch (SqlException e)
            {
                //Debugging
                //MessageBox.Show("error: "+ e.ToString());
                Console.WriteLine(e.ToString());
            }

            //Debugging
            //MessageBox.Show("email address is: " + ordererEmailAddress);

            return ordererEmailAddress;
        }

        public static List<List<string>> GetInterventionalNurses(string sqlQuery)
        {
            selectedRows.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumNurses.ID).ToString().Trim(),
                                    reader.GetString((int)EnumNurses.userName).Trim(),
                                    reader.GetString((int)EnumNurses.email).Trim(),
                                    reader.GetString((int)EnumNurses.firstName).Trim(),
                                    reader.GetString((int)EnumNurses.surname).Trim()
                                };
                                selectedRows.Add(row);
                            };
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return selectedRows;
        }

        public static List<CaseStatsData> GetCaseStatsData(string sqlQuery)
        {
            ListOfCaseStatsData.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
								CaseStatsData myCase = new CaseStatsData
                                (
                                    reader.GetInt32((int)EnumCaseStats.ID),
                                    reader.GetString((int)EnumCaseStats.date).Trim(),
                                    reader.GetDecimal((int)EnumCaseStats.cost),
                                    reader.GetString((int)EnumCaseStats.name).Trim(),
                                    reader.GetString((int)EnumCaseStats.rad).Trim()
                                );
                                ListOfCaseStatsData.Add(myCase);
                            };
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return ListOfCaseStatsData;
        }

        public static List<CaseItemCountHistoryStatsData> CaseItemCountHistoryStatsData(string sqlQuery)
        {
            ListOfCaseItemCountStatsData.Clear();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CaseItemCountHistoryStatsData item = new CaseItemCountHistoryStatsData
                                (
                                    reader.GetInt32((int)EnumCaseItemCountStats.ID),
                                    reader.GetString((int)EnumCaseItemCountStats.Date).Trim(),
                                    reader.GetString((int)EnumCaseItemCountStats.Name).Trim(),
                                    reader.GetString((int)EnumCaseItemCountStats.Description).Trim(),
                                    reader.GetInt32((int)EnumCaseItemCountStats.QuantityUsed),
                                    reader.GetDecimal((int)EnumCaseItemCountStats.UnitCost),
                                    reader.GetDecimal((int)EnumCaseItemCountStats.TotalCost)

                                );
                                ListOfCaseItemCountStatsData.Add(item);
                            };
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return ListOfCaseItemCountStatsData;
        }

        public static void AddTo_ItemsToOrder(SelectedItem item)
        {            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string refNum = item.RefNumber;

                int NewQuantity = Int32.Parse(item.ItemCount.Content.ToString()) + GetCurrentQuantity(item.RefNumber);

                string sqlQuery = string.Format(
                    "IF NOT EXISTS (SELECT 1 FROM ItemsToOrder WHERE RefNumber = @refNumber) " +
                    "BEGIN INSERT INTO ItemsToOrder (Description, RefNumber, QuantityUsed, PackSize, Cost, Category) VALUES (@description,@refNumber,@quantity,@packSize, @cost, @category) END " +
                    "ELSE BEGIN UPDATE ItemsToOrder SET QuantityUsed = @newQuantity WHERE RefNumber = @refNumber END");

                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@description", item.SelectedItemDescription.Content);
                    cmd.Parameters.AddWithValue("@refNumber", refNum);
                    cmd.Parameters.AddWithValue("@quantity", Int32.Parse(item.ItemCount.Content.ToString()));
                    cmd.Parameters.AddWithValue("@newQuantity", NewQuantity);
                    cmd.Parameters.AddWithValue("@packSize", Int32.Parse(item.PackSize.ToString()));
                    cmd.Parameters.AddWithValue("@cost", item.Cost);
                    cmd.Parameters.AddWithValue("@category", item.Category);



                    cmd.ExecuteScalar();
                }
            }
        }

        private static int GetCurrentQuantity(string refNumber)
        {
            int Quantity = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string sqlQuery = string.Format("SELECT QuantityUsed FROM ItemsToOrder WHERE RefNumber = @refNumber");

                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append(sqlQuery);

                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@refNumber", refNumber);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Quantity = reader.GetInt32(0);
                        };
                    }
                }
            }
            return Quantity;
        }

        private static int GetItemPackSize(string refNum, string dbTable)
        {
            int packSize = 0;

            try
            {
                string sqlQuery = @"SELECT FROM ";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<string> row = new List<string>
                                {
                                    reader.GetInt32((int)EnumSnare.ID).ToString().Trim(),
                                    reader.GetString((int)EnumSnare.Brand).Trim(),
                                    reader.GetString((int)EnumSnare.Title).Trim(),
                                    reader.GetDecimal((int)EnumSnare.Cost).ToString().Trim(),
                                    reader.GetString((int)EnumSnare.RefNumber).Trim(),
                                    reader.GetString((int)EnumSnare.Category).Trim(),
                                    reader.GetInt32((int)EnumSnare.PackSize).ToString().Trim()

                                };
                                selectedRows.Add(row);
                            };
                        }
                    }

                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return packSize;
        }

        public static List<ItemToOrder> GetListOfItemsToOrder()
        {
            List<ItemToOrder> ItemsToOrderList = new List<ItemToOrder>();
            string table = "ItemsToOrder";
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    if (CheckIfTableExists(table))
                    {
                        CreateTableCommonCode(connection, table, sb, ItemsToOrderList);
                    }
                    else
                    {
                        CreateTable(table);
                        CreateTableCommonCode(connection, table, sb, ItemsToOrderList);
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            //Deugging
            //MessageBox.Show("Number of Items: " + ItemsToOrderList.Count.ToString());

            return ItemsToOrderList;
        }

        public static void RemoveItemFromDatabase(string sqlQuery, string refNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@refNum", refNumber);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        MessageBoxResult result = MessageBox.Show("Error: " + e.Message, "Confirmation", MessageBoxButton.OK);
                    }
                }
                connection.Close();
            }
        }

        public static List<string> GetProcedureLocations(string sqlQuery)
        {
            //MiscDataRows.Clear();
            List<string> MiscDataRows = new List<string>();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    MiscDataRows.Add(reader.GetString(0).Trim());
                                }

                            };
                        }
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            MiscDataRows.RemoveAll(ClearEmptyListEntries);
            return MiscDataRows;
        }

        //This is the predicate - returns true if list entry is empty (and thus the empty entry is deleted by MiscDataRows.RemoveAll(ClearEmptyListEntries).
        private static bool ClearEmptyListEntries(string entry)
        {
            return entry == "";
        }

        public static List<string> GetSyncCategories(string sqlQuery)
        {
            //MiscDataRows.Clear();

            List<string> MiscDataRows = new List<string>();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");

                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    MiscDataRows.Add(reader.GetString(0));
                                }

                            };

                        }
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            return MiscDataRows;

        }

        public static List<string> GetDbTables(string sqlQuery)
        {
            //MiscDataRows.Clear();

            List<string> MiscDataRows = new List<string>();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConnectionString;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    //Use the passed in query.
                    sb.Append(sqlQuery);

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    MiscDataRows.Add(reader.GetString(0));
                                }

                            };

                        }
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

            MiscDataRows.RemoveAll(ClearEmptyListEntries);
            return MiscDataRows;

        }



        public static void PopulateDbTable(string table, List<List<string>> dataRows, TaskScheduler uiContext, SyncExcelToDb interfaceReference)
        {
            IUpdateProgressBar progress = interfaceReference;

            Task.Factory.StartNew(() => {

                progress.UpdateProgressMax(dataRows.Count);

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);

            int Count = 1;
            foreach (List<string> dataRow in dataRows)
            {
                Task.Factory.StartNew(() => {

                    progress.UpdateTaskDetails($"Inserting into {table}, record {Count}/{dataRows.Count}...");

                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                //Get query string and list of column titles from corresponding enum.
                (string, List<string>) QueryAndEnum = StaticData.GetNewTableQueryStringAndEnum(table);
                string SqlQuery = QueryAndEnum.Item1;
                List<string> columnList = QueryAndEnum.Item2;

                using (SqlConnection cnn = new SqlConnection(InventoryConnectionString))
                {
                    cnn.Open();
                    using (SqlCommand cmd = new SqlCommand(SqlQuery, cnn))
                    {
                        for (int i = 1; i < columnList.Count; i++)
                        {
                            cmd.Parameters.AddWithValue(columnList[i], dataRow[i-1]);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }

                Task.Factory.StartNew(() => {

                    progress.UpdateProgressBar(Count);
                    progress.UpdateTaskDetails($"Inserting into {table}, record {Count - 1}/{dataRows.Count}...COMPLETE!");

                }, CancellationToken.None, TaskCreationOptions.None, uiContext);

                Count++;
            }

            Task.Factory.StartNew(() => {

                progress.UpdateProgressBar(dataRows.Count);

            }, CancellationToken.None, TaskCreationOptions.None, uiContext);

        }

        public static bool CheckIfTableExists(string table)
        {
            bool tableExists = false;

            //string sqlQuery = string.Format($"SELECT count(*) as IsExists FROM dbo.{table} where id = object_id('[dbo].["{table}"]')");
            string sqlQuery = string.Format($"SELECT * FROM {table}");
            SqlConnection sqlCon = new SqlConnection(InventoryConnectionString);

            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon))
            {
                sqlCon.Open();
                try
                {
                    var check = sqlCmd.ExecuteScalar();

                    tableExists = (sqlCmd.ExecuteScalar() is Int32);
                    {
                        sqlCon.Close();
                    }
                }
                catch(Exception e)
                {
                    tableExists = false;

                    sqlCon.Close();
                }
            }

            return tableExists;
        }

        public static void CreateTable(string tableName)
        {
            SqlConnection sqlCon = new SqlConnection(ConnectionString);

            switch (tableName)
            {
                case "Radiologists":
                case "Referrers":

                    string RadRefSqlQuery = "CREATE TABLE " + tableName + "(";
                    RadRefSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    RadRefSqlQuery += "Prefix VARCHAR(50),";
                    RadRefSqlQuery += "FirstName VARCHAR(50),";
                    RadRefSqlQuery += "Surname VARCHAR(50),";
                    RadRefSqlQuery += "Specialty VARCHAR(50)";
                    RadRefSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(RadRefSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "Snares":
                case "Catheters":
                case "Stents":
                case "Wires":
                case "Coils":

                    string StentSqlQuery = "CREATE TABLE " + tableName + "(";
                    StentSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    StentSqlQuery += "Brand VARCHAR(50),";
                    StentSqlQuery += "Title VARCHAR(50),";
                    StentSqlQuery += "SubTitle VARCHAR(MAX),";
                    StentSqlQuery += "Diameter VARCHAR(50),";
                    StentSqlQuery += "Dimensions VARCHAR(50),";
                    StentSqlQuery += "Cost decimal(9,2),";
                    StentSqlQuery += "RefNumber VARCHAR(50),";
                    StentSqlQuery += "Category VARCHAR(50),";
                    StentSqlQuery += "PackSize int";
                    StentSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(StentSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "Dressings":

                    string DressingSqlQuery = "CREATE TABLE " + tableName + "(";
                    DressingSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    DressingSqlQuery += "Brand VARCHAR(50),";
                    DressingSqlQuery += "Title VARCHAR(MAX),";
                    DressingSqlQuery += "Cost decimal(9,2),";
                    DressingSqlQuery += "RefNumber VARCHAR(50),";
                    DressingSqlQuery += "Category VARCHAR(50),";
                    DressingSqlQuery += "PackSize int";
                    DressingSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(DressingSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "Balloons":
                case "Dilators":
                case "Sheaths":

                    string SnareSqlQuery = "CREATE TABLE " + tableName + "(";
                    SnareSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    SnareSqlQuery += "Brand VARCHAR(50),";
                    SnareSqlQuery += "Title VARCHAR(50),";
                    SnareSqlQuery += "Diameter VARCHAR(50),";
                    SnareSqlQuery += "Dimensions VARCHAR(50),";
                    SnareSqlQuery += "Cost decimal(9,2),";
                    SnareSqlQuery += "RefNumber VARCHAR(50),";
                    SnareSqlQuery += "Category VARCHAR(50),";
                    SnareSqlQuery += "PackSize int";
                    SnareSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(SnareSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "EmbolisationSystems":
                case "Misc":

                    string MiscSqlQuery = "CREATE TABLE " + tableName + "(";
                    MiscSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    MiscSqlQuery += "Brand VARCHAR(50),";
                    MiscSqlQuery += "Title VARCHAR(50),";
                    MiscSqlQuery += "SubTitle VARCHAR(MAX),";
                    MiscSqlQuery += "Cost decimal(9,2),";
                    MiscSqlQuery += "RefNumber VARCHAR(50),";
                    MiscSqlQuery += "Category VARCHAR(50),";
                    MiscSqlQuery += "PackSize int";
                    MiscSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(MiscSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "MiscData":

                    string MiscDataSqlQuery = "CREATE TABLE " + tableName + "(";
                    MiscDataSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    MiscDataSqlQuery += "DbTables VARCHAR(50),";
                    MiscDataSqlQuery += "CaseLocations VARCHAR(50),";
                    MiscDataSqlQuery += "SyncCategories VARCHAR(50)";
                    MiscDataSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(MiscDataSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "Contrast":

                    string ContrastSqlQuery = "CREATE TABLE " + tableName + "(";
                    ContrastSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    ContrastSqlQuery += "Brand VARCHAR(50),";
                    ContrastSqlQuery += "Strength NCHAR(10),";
                    ContrastSqlQuery += "Volume NCHAR(10),";
                    ContrastSqlQuery += "Cost decimal(9,2),";
                    ContrastSqlQuery += "RefNumber VARCHAR(50),";
                    ContrastSqlQuery += "Category VARCHAR(50),";
                    ContrastSqlQuery += "PackSize int";
                    ContrastSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(ContrastSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "ItemsToOrder":

                    string ItemsToOrderSqlQuery = "CREATE TABLE " + tableName + "(";
                    ItemsToOrderSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    ItemsToOrderSqlQuery += "Description VARCHAR(50),";
                    ItemsToOrderSqlQuery += "RefNumber VARCHAR(50),";
                    ItemsToOrderSqlQuery += "QuantityUsed int,";
                    ItemsToOrderSqlQuery += "PackSize int,";
                    ItemsToOrderSqlQuery += "Cost decimal(9,2),";
                    ItemsToOrderSqlQuery += "Category VARCHAR(50),";
                    ItemsToOrderSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(ItemsToOrderSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                case "InterventionalNurses":

                    string IrNursesSqlQuery = "CREATE TABLE " + tableName + "(";
                    IrNursesSqlQuery += "ID int IDENTITY(1,1) PRIMARY KEY,";
                    IrNursesSqlQuery += "UserName VARCHAR(50),";
                    IrNursesSqlQuery += "Email VARCHAR(100),";
                    IrNursesSqlQuery += "FirstName VARCHAR(50),";
                    IrNursesSqlQuery += "Surname VARCHAR(50),";
                    IrNursesSqlQuery += ")";

                    using (SqlCommand sqlCmd = new SqlCommand(IrNursesSqlQuery, sqlCon))
                    {
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                    break;

                default:
                    MessageBox.Show($"Table not found in DBController.CreateTable(): {tableName}");
                    break;
            }
        }

        public static void ClearTable(string table)
        {
            SqlConnection sqlCon = new SqlConnection(InventoryConnectionString);

            //ERROR RAISED: INCORRECT SYNTAX NEAR '*'.
            string sqlQuery = $"TRUNCATE TABLE {table}";

            using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlCon))
            {
                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }

        }

        private static void CreateTableCommonCode(SqlConnection connection, string table, StringBuilder sb, List<ItemToOrder> ItemsToOrderList)
        {

            string sqlQuery = $"SELECT * FROM {table}";

            sb.Append(sqlQuery);

            String sql = sb.ToString();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ItemToOrder item = new ItemToOrder(
                           reader.GetInt32((int)EnumItemOrder.ID),
                           reader.GetString((int)EnumItemOrder.Description).Trim(),
                           reader.GetString((int)EnumItemOrder.RefNumber).Trim(),
                           reader.GetInt32((int)EnumItemOrder.Quantity),
                           reader.GetInt32((int)EnumItemOrder.PackSize),
                           reader.GetDecimal((int)EnumItemOrder.Cost),
                           reader.GetString((int)EnumItemOrder.Category).Trim()
                        );

                        ItemsToOrderList.Add(item);
                    };
                }
            }

        }

		//Save relevant case data to CaseItemCountHistory & Case Stats
		public static void SaveCaseDataToDatabase(Case caseObject, string fileName)
		{
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				//Insert into CaseItemCountHistory
				string sqlQuery = string.Format("INSERT INTO CaseItemCountHistory (CaseDate, CaseName, ItemDescription, QuantityUsed, UnitCost, TotalCost) VALUES (@caseDate,@caseName,@itemDescription, @quantityUsed, @unitCost, @totalCost)");
				connection.Open();

				foreach (SelectedItem caseItem in caseObject.CaseItems)
				{
					using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
					{
						cmd.Parameters.AddWithValue("@caseDate", caseObject.CaseDate.Day + "/" + caseObject.CaseDate.Month + "/" + caseObject.CaseDate.Year);
						cmd.Parameters.AddWithValue("@caseName", fileName);
						cmd.Parameters.AddWithValue("@itemDescription", caseItem.SelectedItemDescription.Content.ToString());
						cmd.Parameters.AddWithValue("@quantityUsed", Int32.Parse(caseItem.ItemCount.Content.ToString()));
						cmd.Parameters.AddWithValue("@unitCost", caseItem.Cost);
						cmd.Parameters.AddWithValue("@totalCost", caseItem.Cost * Int32.Parse(caseItem.ItemCount.Content.ToString()));

						cmd.ExecuteScalar();
					}
				}

				//Insert into CaseStats
				string sqlQuery2 = string.Format("INSERT INTO CaseStats (CaseDate, CaseCost, CaseName, CaseRad) VALUES (@caseDate,@caseCost,@caseName, @caseRad)");

				using (SqlCommand cmd = new SqlCommand(sqlQuery2, connection))
				{
					cmd.Parameters.AddWithValue("@caseDate", caseObject.CaseDate.Day + "/" + caseObject.CaseDate.Month + "/" + caseObject.CaseDate.Year);
					cmd.Parameters.AddWithValue("@caseCost", caseObject.CaseCost);
					cmd.Parameters.AddWithValue("@caseName", fileName);
					cmd.Parameters.AddWithValue("@caseRad", caseObject.Radiologist);

					cmd.ExecuteScalar();
				}
			}
		}














	}//END OF CLASS

}
