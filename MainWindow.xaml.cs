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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void UploadTipp(object sender, RoutedEventArgs e)
        {
            try
            {
                int number1 = Int32.Parse(numberOne.Text);
                int number2 = Int32.Parse(numberTwo.Text);
                int number3 = Int32.Parse(numberThree.Text);
                int number4 = Int32.Parse(numberFour.Text);
                int number5 = Int32.Parse(numberFive.Text);
                int number6 = Int32.Parse(numberSix.Text);
                int numberS = Int32.Parse(numberSuper.Text);
                if ((number1 <= 49 && number1 >= 1) && (number2 <= 49 && number2 >= 1) && (number3 <= 49 && number3 >= 1) && (number4 <= 49 && number4 >= 1) && (number5 <= 49 && number5 >= 1) && (number6 <= 49 && number6 >= 1) && (numberS <= 49 && numberS >= 1))
                {
                    SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLSERVER; Initial Catalog=LoginDB; Integrated Security=True;");
                    MessageBox.Show("Alle Werte im Bereich.");
                    try
                    {
                        if (sqlCon.State == ConnectionState.Closed)
                        {
                            sqlCon.Open();
                        }                                                                                                                               //Hier muss die ID gegen Daten der DB ausgetauscht werden, damit immer die Ziehungen derjenigen Person zugewiesen werden kann, die angemeldet ist
                        String query = "INSERT INTO tblPickByDate (UserID, Date, Pick1, Pick2, Pick3, Pick4, Pick5, Pick6, PickSuper) values('" + "1" + "' , '" + DateTime.Now + "' ,'" + this.numberOne.Text + "', '" + this.numberTwo.Text + "', '" + this.numberThree.Text + "', '" + this.numberFour.Text + "', '" + this.numberFive.Text + "', '" + this.numberSix.Text + "', '" + this.numberSuper.Text + "')";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
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

        private void history_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLSERVER; Initial Catalog=LoginDB; Integrated Security=True;");
            try
            {
                sqlCon.Open();
                string query = "select * from tblPickByDate ";
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
