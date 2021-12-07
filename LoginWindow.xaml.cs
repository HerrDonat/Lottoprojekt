using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lottoprojekt
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private string UserId;

        private void LoginUserBtn(object sender, RoutedEventArgs e)
        {
           // SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Workspace\Lottoprojekt\Database.mdf;Integrated Security=True");
            SqlConnection sqlCon = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Otto\source\repos\Lottoprojekt2\Database1.mdf; Integrated Security = True; Connect Timeout = 30");
            try
            {
                if (sqlCon.State == System.Data.ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                String query = "SELECT Id FROM Users WHERE Username=@Username AND Password=@Password";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                sqlCmd.Parameters.AddWithValue("@Password", txtPassword.Password);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    UserId = reader.GetValue(0).ToString();     //Diese Variable an MainWindow übergeben
                }




                //int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (UserId != null)
                {
                    MessageBox.Show("Herzlich Willkommen, " + txtUsername.Text + "!");
                    MainWindow dashboard = new MainWindow(UserId);
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect.");
                }
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
    }
}
