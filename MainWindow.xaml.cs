using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lottoprojekt
{
    public partial class MainWindow : Window
    {
        public MainWindow(string UserId)
        {
            InitializeComponent();
            this.Width = 1920; // << Weite
            this.Height = 1080; // << Höhe

            LoginWindow l = new LoginWindow();
            UserEID.Text = UserId;              //Name des Users muss hier rein

            if (UserEID.Text == "1") //Wenn User Mitarbeiter ist, dann
            {
                LadeTip.Visibility = Visibility.Hidden;
                MitarbeiterText.Visibility = Visibility.Visible;
                LottoBoxOne.IsEnabled = false;
                LottoBoxTwo.IsEnabled = false;
                LottoBoxThree.IsEnabled = false;
                LottoBoxFour.IsEnabled = false;
                LottoBoxFive.IsEnabled = false;
                LottoBoxSix.IsEnabled = false;
                LottoBoxSuper.IsEnabled = false;
            }
            else
            {
                ApplyNumbers.Visibility = Visibility.Hidden;
            }
        }
        int korrekteTipps = 0;
        bool superZahl = false;
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Workspace\Lottoprojekt\Database.mdf;Integrated Security=True");
        //SqlConnection sqlCon = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Otto\source\repos\Lottoprojekt2\Database1.mdf; Integrated Security = True; Connect Timeout = 30");
        private void StarteZiehung(object sender, RoutedEventArgs e)        //Ziehung von zufälligen Zahlen wird gestartet
        {
            var rand = new Random();
            int random1 = rand.Next(1, 50);
            int random2 = rand.Next(1, 50);
            int random3 = rand.Next(1, 50);
            int random4 = rand.Next(1, 50);
            int random5 = rand.Next(1, 50);
            int random6 = rand.Next(1, 50);
            int random7 = rand.Next(0, 10);
            MessageBoxResult result = MessageBox.Show("Möchten Sie eine neue Ziehung starten?", "Ziehung starten", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //Ziehung starten
                    random1 = rand.Next(1, 50);
                    while (random6 == random5 || random6 == random4 || random6 == random3 || random6 == random2 || random6 == random1 || random5 == random4 || random5 == random3 || random5 == random2 || random5 == random1 || random4 == random3 || random4 == random2 || random4 == random1 || random3 == random2 || random3 == random1 || random2 == random1)
                    {
                        if (random2 == random1)     //Hier wird verhindert dass bei einer Ziehung eine Zahl doppelt vorkommt
                        {
                            random2 = rand.Next(1, 50);
                        }
                        if (random3 == random2 || random3 == random1)
                        {
                            random3 = rand.Next(1, 50);
                        }
                        if (random4 == random3 || random4 == random2 || random4 == random1)
                        {
                            random4 = rand.Next(1, 50);
                        }
                        if (random5 == random4 || random5 == random3 || random5 == random2 || random5 == random1)
                        {
                            random5 = rand.Next(1, 50);

                        }
                        if (random6 == random5 || random6 == random4 || random6 == random3 || random6 == random2 || random6 == random1)
                        {
                            random6 = rand.Next(1, 50);
                        }
                    }
                    LottoBoxOne.Text = random1.ToString();
                    LottoBoxTwo.Text = random2.ToString();
                    LottoBoxThree.Text = random3.ToString();
                    LottoBoxFour.Text = random4.ToString();
                    LottoBoxFive.Text = random5.ToString();
                    LottoBoxSix.Text = random6.ToString();
                    LottoBoxSuper.Text = random7.ToString();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Die Ziehung wurde abgebrochen.");
                    break;
                default:
                    break;
            }
            //SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLSERVER; Initial Catalog=LoginDB; Integrated Security=True;"); //Verbindung zu DB wird hergestellt
            try
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String query = "INSERT INTO PulledNumbers VALUES ('" + DateTime.Now + "','" + this.LottoBoxOne.Text + "', '" + this.LottoBoxTwo.Text + "', '" + this.LottoBoxThree.Text + "', '" + this.LottoBoxFour.Text + "', '" + this.LottoBoxFive.Text + "', '" + this.LottoBoxSix.Text + "', '" + this.LottoBoxSuper.Text + "')";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.ExecuteNonQuery();       //Derselbe Datensatz wird immer 2 mal in der DB gespeichert, bug?
                sqlCmd.CommandType = CommandType.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlCon.Close();     //Verbindung zur DB wird beendet
            }
        }

        private void Gewinnpruefung(object sender, RoutedEventArgs e)
        {
            if (UserEID.Text == "1")
            {
                MessageBox.Show("Sie sind als Mitarbeiter angemeldet!");
            }
            else
            {

                int[] tippsKunde = new int[6];
                int[] zahlenZiehung = new int[6];
                int getippteSuperZahl = 0;
                int gezogeneSuperZahl = 0;
                String query = "SELECT TOP 1 * FROM Customer INNER JOIN PulledNumbers ON Customer.Datum = PulledNumbers.Datum WHERE Customer.UserId = " + UserEID.Text + " ORDER BY Customer.Id DESC, PulledNumbers.Id DESC";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataReader dataReader = sqlCmd.ExecuteReader())
                    {
                        while (dataReader.Read())       //Ziehung und Tipps in Variablen speichern
                        {
                            tippsKunde[0] = Convert.ToInt32(dataReader.GetValue(3));
                            tippsKunde[1] = Convert.ToInt32(dataReader.GetValue(4));
                            tippsKunde[2] = Convert.ToInt32(dataReader.GetValue(5));
                            tippsKunde[3] = Convert.ToInt32(dataReader.GetValue(6));
                            tippsKunde[4] = Convert.ToInt32(dataReader.GetValue(7));
                            tippsKunde[5] = Convert.ToInt32(dataReader.GetValue(8));
                            getippteSuperZahl = Convert.ToInt32(dataReader.GetValue(9));
                            zahlenZiehung[0] = Convert.ToInt32(dataReader.GetValue(12));
                            zahlenZiehung[1] = Convert.ToInt32(dataReader.GetValue(13));
                            zahlenZiehung[2] = Convert.ToInt32(dataReader.GetValue(14));
                            zahlenZiehung[3] = Convert.ToInt32(dataReader.GetValue(15));
                            zahlenZiehung[4] = Convert.ToInt32(dataReader.GetValue(16));
                            zahlenZiehung[5] = Convert.ToInt32(dataReader.GetValue(17));
                            gezogeneSuperZahl = Convert.ToInt32(dataReader.GetValue(18));

                        }
                    }
                    if (zahlenZiehung[0] != 0 && zahlenZiehung[1] != 0) //Wenn leer, kann davon ausgegangen werden dass heute noch keine Ziehung durchgeführt wurde
                    {
                        foreach (int item in tippsKunde)        //Tipps werden mit Ziehung verglichen
                        {
                            if (item == zahlenZiehung[0] || item == zahlenZiehung[1] || item == zahlenZiehung[2] || item == zahlenZiehung[3] || item == zahlenZiehung[4] || item == zahlenZiehung[5])
                            {
                                korrekteTipps++;
                            }
                        }
                        if (gezogeneSuperZahl == getippteSuperZahl)
                        {
                            superZahl = true;
                        }
                        int gewinnKlasse = BerechneGewinnKlasse(korrekteTipps, superZahl);
                        if (gewinnKlasse != 0)
                        {
                            MessageBox.Show("Sie haben bei der heutigen Ziehung folgende Gewinnklasse erreicht: " + gewinnKlasse);
                        }
                        else
                        {
                            MessageBox.Show("Sie hatten in Bezug auf die letzte Ziehung leider keine einzige Zahl richtig.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Die heutige Ziehung wurde noch nicht gestartet, bitte warten Sie auf einen Mitarbeiter.");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();     //Verbindung zur DB wird beendet
                }
            }
        }

        private void LadeTipHoch(object sender, RoutedEventArgs e)      //Tipp des Kunden wird hochgeladen und in der DB gespeichert
        {
            try
            {
                int number1 = Int32.Parse(LottoBoxOne.Text);
                int number2 = Int32.Parse(LottoBoxTwo.Text);
                int number3 = Int32.Parse(LottoBoxThree.Text);
                int number4 = Int32.Parse(LottoBoxFour.Text);
                int number5 = Int32.Parse(LottoBoxFive.Text);
                int number6 = Int32.Parse(LottoBoxSix.Text);
                int numberS = Int32.Parse(LottoBoxSuper.Text);  //Kontrollieren, ob die Tipps vom Kunden im gültigen Bereich liegen, evtl auf doppelte Zahlen prüfen
                if (((number1 <= 49 && number1 >= 1) && (number2 <= 49 && number2 >= 1) && (number3 <= 49 && number3 >= 1) && (number4 <= 49 && number4 >= 1) && (number5 <= 49 && number5 >= 1) && (number6 <= 49 && number6 >= 1) && (numberS <= 9 && numberS >= 0)) && number1 != number2 && number1 != number3 && number1 != number4 && number1 != number5 && number1 != number6 && number2 != number3 && number2 != number4 && number2 != number5 && number2 != number6 && number3 != number4 && number3 != number5 && number3 != number6 && number4 != number5 && number4 != number6 && number5 != number6)
                {
                    try
                    {
                        if (sqlCon.State == ConnectionState.Closed)
                        {
                            sqlCon.Open();
                        }                                                                                       //Hier muss die ID gegen Daten der DB ausgetauscht werden, damit immer die Ziehungen derjenigen Person zugewiesen werden kann, die angemeldet ist
                        String query = "INSERT INTO Customer VALUES ('" + UserEID.Text + "', '" + DateTime.Now + "', '" + this.LottoBoxOne.Text + "', '" + this.LottoBoxTwo.Text + "', '" + this.LottoBoxThree.Text + "', '" + this.LottoBoxFour.Text + "', '" + this.LottoBoxFive.Text + "', '" + this.LottoBoxSix.Text + "', '" + this.LottoBoxSuper.Text + "')";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);      //Eingegebene Tipps vom Kunden werden in der DB gespeichert
                        sqlCmd.ExecuteNonQuery();
                        sqlCmd.CommandType = CommandType.Text;
                        MessageBox.Show("Ihr Tipp wurde erfolgreich abgegeben und gilt für die Ziehung am " + DateTime.Now.ToString("MM/dd/yyyy"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        sqlCon.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Sie haben falsche Werte in die Felder eingegeben");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Nicht alle Werte sind im korrekten Bereich.");
            }
        }

        private int BerechneGewinnKlasse(int korrekteTipps, bool superZahl)
        {
            if (korrekteTipps == 2 && superZahl)
            {
                return 9;
            }
            else if (korrekteTipps == 3 && superZahl)
            {
                return 7;
            }
            else if (korrekteTipps == 3)
            {
                return 8;
            }
            else if (korrekteTipps == 4 && superZahl)
            {
                return 5;
            }
            else if (korrekteTipps == 4)
            {
                return 6;
            }
            else if (korrekteTipps == 5 && superZahl)
            {
                return 3;
            }
            else if (korrekteTipps == 5)
            {
                return 4;
            }
            else if (korrekteTipps == 6 && superZahl)
            {
                return 1;
            }
            else if (korrekteTipps == 6)
            {
                return 2;
            }
            return 0;
        }

        private void ZeigeLetzteZiehungen(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlCon.Open();
                string query = "select Datum, number1, number2, number3, number4, number5, number6, numberSuper from PulledNumbers";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.ExecuteNonQuery();
                SqlDataAdapter dataAd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("tblPickByDate");
                dataAd.Fill(dt);
                dataGrid1.ItemsSource = dt.DefaultView;
                dataAd.Update(dt);
                sqlCon.Close();
            }
            catch (Exception)
            {

            }
        }

        private void ZeigeLetzteTipps(object sender, RoutedEventArgs e)
        {
            if (UserEID.Text == "1")
            {
                MessageBox.Show("Sie sind als Mitarbeiter angemeldet!");
            }
            else
            {
                try
                {
                    sqlCon.Open();
                    string query = "select Datum, number1, number2, number3, number4, number5, number6, numberSuper from Customer WHERE UserId = " + UserEID.Text;      //ID des Kunden einfügen, der gerade angemeldet ist
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter dataAd = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("tblPickByDate");
                    dataAd.Fill(dt);
                    dataGrid1.ItemsSource = dt.DefaultView;
                    dataAd.Update(dt);
                    sqlCon.Close();
                }
                catch (Exception)
                {

                }
            }
        }
        private void Statistik(object sender, RoutedEventArgs e)
        {
            sqlCon.Open();
            var query = "SELECT number1, number2, number3, number4, number5, number6 FROM PulledNumbers";
            SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            sqlCmd.ExecuteNonQuery();
            DataTable dt = new DataTable("tblPickByDate");
            dt.Columns.Add("1", typeof(int));
            dt.Columns.Add("2", typeof(int));
            dt.Columns.Add("3", typeof(int));
            dt.Columns.Add("4", typeof(int));
            dt.Columns.Add("5", typeof(int));
            dt.Columns.Add("6", typeof(int));
            dt.Columns.Add("7", typeof(int));
            dt.Columns.Add("8", typeof(int));
            dt.Columns.Add("9", typeof(int));
            dt.Columns.Add("10", typeof(int));
            dt.Columns.Add("11", typeof(int));
            dt.Columns.Add("12", typeof(int));
            dt.Columns.Add("13", typeof(int));
            dt.Columns.Add("14", typeof(int));
            dt.Columns.Add("15", typeof(int));
            dt.Columns.Add("16", typeof(int));
            dt.Columns.Add("17", typeof(int));
            dt.Columns.Add("18", typeof(int));
            dt.Columns.Add("19", typeof(int));
            dt.Columns.Add("20", typeof(int));
            dt.Columns.Add("21", typeof(int));
            dt.Columns.Add("22", typeof(int));
            dt.Columns.Add("23", typeof(int));
            dt.Columns.Add("24", typeof(int));
            dt.Columns.Add("25", typeof(int));
            dt.Columns.Add("26", typeof(int));
            dt.Columns.Add("27", typeof(int));
            dt.Columns.Add("28", typeof(int));
            dt.Columns.Add("29", typeof(int));
            dt.Columns.Add("30", typeof(int));
            dt.Columns.Add("31", typeof(int));
            dt.Columns.Add("32", typeof(int));
            dt.Columns.Add("33", typeof(int));
            dt.Columns.Add("34", typeof(int));
            dt.Columns.Add("35", typeof(int));
            dt.Columns.Add("36", typeof(int));
            dt.Columns.Add("37", typeof(int));
            dt.Columns.Add("38", typeof(int));
            dt.Columns.Add("39", typeof(int));
            dt.Columns.Add("40", typeof(int));
            dt.Columns.Add("41", typeof(int));
            dt.Columns.Add("42", typeof(int));
            dt.Columns.Add("43", typeof(int));
            dt.Columns.Add("44", typeof(int));
            dt.Columns.Add("45", typeof(int));
            dt.Columns.Add("46", typeof(int));
            dt.Columns.Add("47", typeof(int));
            dt.Columns.Add("48", typeof(int));
            dt.Columns.Add("49", typeof(int));
            dataGrid1.ItemsSource = dt.DefaultView;
            //using (SqlDataReader dataReader = sqlCmd.ExecuteReader())
            //{
            //    int[] Stats = new int[100];
            //    while (dataReader.Read())
            //    {
            //        for (int i = 0; i < 100; i++)
            //        {
            //            Stats[i]= Convert.ToInt32(dataReader.GetValue(i));
            //        }
            //    }
            //}
            sqlCon.Close();
        }
    }
}
