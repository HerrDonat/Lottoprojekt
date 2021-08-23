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
            SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLSERVER; Initial Catalog=LoginDB; Integrated Security=True;");
            //try
            //{
            //    if (sqlCon.State == System.Data.ConnectionState.Closed)
            //    {
            //        sqlCon.Open();
            //    }
            //    String query = "INSERT INTO tblPickByDate (UserID, UserName, Date, Pick1, Pick2, Pick3, Pick4, Pick5, Pick6, PickSuper) value('"+  +"' , '"+ DateTime.Now+"' ,'"+ this.numberOne.Text + "', '"+ this.numberTwo.Text + "', '" + this.numberThree.Text + "', '" + this.numberFour.Text + "', '" + this.numberFive.Text + "', '" + this.numberSix.Text + "', '"+ this.numberSuper.Text +"')";
            //    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
            //    sqlCmd.ExecuteNonQuery();
            //    sqlCmd.CommandType = System.Data.CommandType.Text;
            //    int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
            //    if (count == 1)
            //    {
            //        MainWindow dashboard = new MainWindow();
            //        dashboard.Show();
            //        this.Close();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Username or password is incorrect.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    sqlCon.Close();
            //}
        }
    }

}
