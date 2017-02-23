using System;
using System.Collections.Generic;
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
namespace WpfBankTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            AdminManageWindow amw = new AdminManageWindow();
            if (comoBoxUserType.SelectedIndex == 0)
            {
                using (var context = new BankInfoManagementEntities())
                {
                    
                    var q = from t in context.EmploeeInfo
                            where t.YNo==textBoxUserName.Text
                            where t.YPassword == pwdBoxUserPassword.Password
                            where t.YPosition==1
                            select t;
                    if (q.ToList().Count() != 0) 
                    {
                        MessageBox.Show("登陆成功","提示",MessageBoxButton.OK,MessageBoxImage.Information);
                        this.Close();
                        amw.Show();
                    }
                    else
                    {
                        MessageBox.Show("登陆失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
  
                    }

                }
            else
            {
                AssistantWindow aw = new AssistantWindow();
                using (var context = new BankInfoManagementEntities())
                {

                    var q = from t in context.EmploeeInfo
                            where t.YNo == textBoxUserName.Text
                            where t.YPassword == pwdBoxUserPassword.Password
                            where t.YPosition == 0
                            select t;
                    if (q.ToList().Count() != 0)
                    {
                        MessageBox.Show("登陆成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        aw.Show();
                    }
                    else
                    {
                        MessageBox.Show("登陆失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }

            }

          
            }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
           
        }
        public class PasswordBoxMonitor : DependencyObject
        {
            public static bool GetIsMonitoring(DependencyObject obj)
            {
                return (bool)obj.GetValue(IsMonitoringProperty);
            }

            public static void SetIsMonitoring(DependencyObject obj, bool value)
            {
                obj.SetValue(IsMonitoringProperty, value);
            }

            public static readonly DependencyProperty IsMonitoringProperty =
                DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));



            public static int GetPasswordLength(DependencyObject obj)
            {
                return (int)obj.GetValue(PasswordLengthProperty);
            }

            public static void SetPasswordLength(DependencyObject obj, int value)
            {
                obj.SetValue(PasswordLengthProperty, value);
            }

            public static readonly DependencyProperty PasswordLengthProperty =
                DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

            private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var pb = d as PasswordBox;
                if (pb == null)
                {
                    return;
                }
                if ((bool)e.NewValue)
                {
                    pb.PasswordChanged += PasswordChanged;
                }
                else
                {
                    pb.PasswordChanged -= PasswordChanged;
                }
            }

            static void PasswordChanged(object sender, RoutedEventArgs e)
            {
                var pb = sender as PasswordBox;
                if (pb == null)
                {
                    return;
                }
                SetPasswordLength(pb, pb.Password.Length);
            }
        }

        private void x_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
           
        }

     
}
