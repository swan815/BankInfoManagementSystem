using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfBankTest
{
    /// <summary>
    /// AdminManageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdminManageWindow : Window
    {
        private BankInfoManagementEntities Context = new BankInfoManagementEntities();
        private BankInfoManagementEntities Context1 = new BankInfoManagementEntities();
        public AdminManageWindow()
        {
            InitializeComponent();
            var q1 = from t in Context.EmploeeInfo
                     select t;
            dataGrid2.ItemsSource = q1.ToList();
        }

      
        private void menuItemChangeRate_Click(object sender, RoutedEventArgs e)
        {
            tabItemChangeRate.IsSelected = true;
            
        }


        private void btnSavePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Context.SaveChanges();
                MessageBox.Show("保存成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "保存失败");

            }
        }

        private void menuItemClose_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void x_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tabItemChangeRate_Loaded(object sender, RoutedEventArgs e)
        {
            Frame1.Source = new Uri(@"Pages/ChangeInterest.xaml", UriKind.Relative);
        }

        private void btnAddEmployee_Clicl(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = db.Camcon();
            conn.Open();
            textBoxAYPwd.Password = "123456";
            string OpenNewAccount = "insert into EmploeeInfo(YNo,YPassword,YName,YPosition,YSex,YPhoneNum,YID,YWorkDate) values('" + textBoxAYNo.Text + "','" + textBoxAYPwd.Password + "','" + textBoxAYName.Text + "','" + Convert.ToInt32(textAYPosition.Text) + "','" + textBoxAYSex.Text
                                                                   + "','" + textBoxAYPhone.Text + "','" + textBoxOpenID.Text + "','" +datePickerWorkDate.SelectedDate + "')";
            SqlCommand comm1 = new SqlCommand(OpenNewAccount, conn);
            int z = comm1.ExecuteNonQuery();
            if (z > 0)
            {
                MessageBox.Show("添加职员成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("添加职员失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnSaveChangeSalary_Click(object sender, RoutedEventArgs e)
        {

            
        }

        private void TabItemChangeSalary_Loaded(object sender, RoutedEventArgs e)
        {
            var q3 = from t in Context1.EmploeeInfo
                     select t;
            dataGrid3.ItemsSource = q3.ToList();
        }
    }
}
