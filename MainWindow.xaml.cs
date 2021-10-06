
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
        public MainWindow()
        {
            InitializeComponent();


            LoginWindow l = new LoginWindow();
            UserEID.Text = l.UserID;              //Name des Users muss hier rein

            if (true) //Wenn User Mitarbeiter ist, dann
            {
                ApplyNumbers.Content = "Ziehung starten";
                MitarbeiterText.Visibility = Visibility.Visible;
                LottoBoxOne.IsEnabled = false;
                LottoBoxTwo.IsEnabled = false;
                LottoBoxThree.IsEnabled = false;
                LottoBoxFour.IsEnabled = false;
                LottoBoxFive.IsEnabled = false;
                LottoBoxSix.IsEnabled = false;
                LottoBoxSuper.IsEnabled = false;
            }
        }
        int korrekteTipps = 0;
        bool superZahl = false;


        private void StarteZiehung(object sender, RoutedEventArgs e) //Tipp des Kunden wird hochgeladen, nachdem die Werte überprüft wurden
        {       //TODO Funktionen aufteilen in StarteZiehung und LadeTipHoch, da momentan noch beides in derselben Funktion passiert
            var rand = new Random();
            int random1 = rand.Next(1, 50);
            int random2 = rand.Next(1, 50);
            int random3 = rand.Next(1, 50);
            int random4 = rand.Next(1, 50);
            int random5 = rand.Next(1, 50);
            int random6 = rand.Next(1, 50);
            int random7 = rand.Next(0, 10);
            if (ApplyNumbers.Content.ToString() == "Ziehung starten")
            {
                MessageBoxResult result = MessageBox.Show("Möchten Sie eine neue Ziehung starten?", "Ziehung starten", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        //Ziehung starten
                        random1 = rand.Next(1, 50);
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
                SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Workspace\Lottoprojekt\Database.mdf;Integrated Security=True");//Pfad, wo die DB gespeichert ist
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }                                                     //Bei dieser Zahl muss pro Ziehung immer + 1 gerechnet werden(Id der Ziehung)                                                                         //Hier muss die ID gegen Daten der DB ausgetauscht werden, damit immer die Ziehungen derjenigen Person zugewiesen werden kann, die angemeldet ist
                    String query = "INSERT INTO PulledNumbers VALUES ('" + 2 + "' ,'" + this.LottoBoxOne.Text + "', '" + this.LottoBoxTwo.Text + "', '" + this.LottoBoxThree.Text + "', '" + this.LottoBoxFour.Text + "', '" + this.LottoBoxFive.Text + "', '" + this.LottoBoxSix.Text + "', '" + this.LottoBoxSuper.Text + "')";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);//Verbindung klappt, allerdings versucht er immer 2 mal die Daten in die DB zu schreiben, weswegen immer eine Fehlermeldung kommt
                    sqlCmd.ExecuteNonQuery();
                    sqlCmd.CommandType = CommandType.Text;
                    int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlCon.Close();     //Verbindung zur DB wird beendet
                }
            } // Ziehung starten, sonst LadeTipHoch
            else
            {
                LadeTipHoch(null, null);
            }
        }

        private void Gewinnprüfung()
        {
            //BerechneGewinnKlasse();
        }

        private void LadeTipHoch(object sender, RoutedEventArgs e)      //Button muss erstellt werden
        {     
            int UserId = Convert.ToInt32(UserEID.Text);
            try
            {
                int number1 = Int32.Parse(LottoBoxOne.Text);
                int number2 = Int32.Parse(LottoBoxTwo.Text);
                int number3 = Int32.Parse(LottoBoxThree.Text);
                int number4 = Int32.Parse(LottoBoxFour.Text);
                int number5 = Int32.Parse(LottoBoxFive.Text);
                int number6 = Int32.Parse(LottoBoxSix.Text);
                int numberS = Int32.Parse(LottoBoxSuper.Text);  //Kontrollieren, ob die Tipps vom Kunden im gültigen Bereich liegen, evtl auf doppelte Zahlen prüfen
                if ((number1 <= 49 && number1 >= 1) && (number2 <= 49 && number2 >= 1) && (number3 <= 49 && number3 >= 1) && (number4 <= 49 && number4 >= 1) && (number5 <= 49 && number5 >= 1) && (number6 <= 49 && number6 >= 1) && (numberS <= 49 && numberS >= 1))
                {
                    SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Workspace\Lottoprojekt\Database.mdf;Integrated Security=True");
                    MessageBox.Show("Alle Werte im Bereich.");
                    try
                    {
                        if (sqlCon.State == ConnectionState.Closed)
                        {
                            sqlCon.Open();
                        }                                                                                       //Hier muss die ID gegen Daten der DB ausgetauscht werden, damit immer die Ziehungen derjenigen Person zugewiesen werden kann, die angemeldet ist
                        String query = "INSERT INTO PulledNumbers VALUES ('" + UserId + "' ,'" + this.LottoBoxOne.Text + "', '" + this.LottoBoxTwo.Text + "', '" + this.LottoBoxThree.Text + "', '" + this.LottoBoxFour.Text + "', '" + this.LottoBoxFive.Text + "', '" + this.LottoBoxSix.Text + "', '" + this.LottoBoxSuper.Text + "')";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);      //Eingegebene Tipps vom Kunden werden in der DB gespeichert
                        sqlCmd.ExecuteNonQuery();
                        sqlCmd.CommandType = CommandType.Text;
                        int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
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

        private int BerechneGewinnKlasse()      //Muss aufgerufen werden, rückgegebene Zahl ist die Gewinnklasse
        {
            if (korrekteTipps == 2 && superZahl)
            {
                return 9;
            }
            else if (korrekteTipps == 3)
            {
                return 8;
            }
            else if (korrekteTipps == 3 && superZahl)
            {
                return 7;
            }
            else if (korrekteTipps == 4)
            {
                return 6;
            }
            else if (korrekteTipps == 4 && superZahl)
            {
                return 5;
            }
            else if (korrekteTipps == 5)
            {
                return 4;
            }
            else if (korrekteTipps == 5 && superZahl)
            {
                return 3;
            }
            else if (korrekteTipps == 6)
            {
                return 2;
            }
            else if (korrekteTipps == 6 && superZahl)
            {
                return 1;
            }
            return 0;
        }

        private void ZeigeLetzteZiehungen(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLSERVER; Initial Catalog=LoginDB; Integrated Security=True;");
            try
            {
                sqlCon.Open();
                string query = "select * from tblPickByDate ";      //ID des Kunden einfügen, der gerade angemeldet ist
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

}
