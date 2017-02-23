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
using System.Windows.Shapes;

namespace WpfBankTest
{
    /// <summary>
    /// btnCalculateInterest.xaml 的交互逻辑
    /// </summary>
    public partial class btnCalculateInterest : Window
    {
        public btnCalculateInterest()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void x_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCheckInterest_Click(object sender, RoutedEventArgs e)
        {

            using (var context = new BankInfoManagementEntities())
            {

                var q = from t in context.CreditCardInfo
                        where t.CNo == textBoxCheckCNo.Text
                        where t.CPassword == textBoxCheckPwd.Password
                        select t;
                if (q.ToList().Count() != 0)
                {
                    MessageBox.Show("登录成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (radioBtnLCZQ.IsChecked == true)
                    {
                        var q1 = from t in context.OperationLog
                                 where t.CDate == datePicker2.SelectedDate
                                 where t.CNo == textBoxCheckCNo.Text
                                 where t.CStyle.Contains("零存整取")
                                 select new
                                 {
                                     账号 = t.CNo,
                                     客户姓名 = t.CName,
                                     操作类型 = t.CStyle,
                                     日期 = t.CDate,
                                     金额 = t.CMoney,
                                     存款时长 = t.CTimeLen,
                                     利率 = t.CRate,
                                     利息= t.Interest

                                 };
                        if (q1.ToList().Count == 0)
                        {
                            MessageBox.Show("无存款记录", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            dataGrid2.ItemsSource = q1.ToList();
                        }
                    }
                    else if (radioBtnDQCK.IsChecked == true)
                    {
                        var q2 = from t in context.OperationLog
                                 where t.CDate == datePicker2.SelectedDate
                                 where t.CNo == textBoxCheckCNo.Text
                                 where t.CStyle.Contains("定存")
                                 select new
                                 {
                                     账号 = t.CNo,
                                     客户姓名 = t.CName,
                                     操作类型 = t.CStyle,
                                     日期 = t.CDate,
                                     金额 = t.CMoney,
                                      利率 = t.CRate,
                                     利息= t.Interest
                                 };
                        if (q2.ToList().Count == 0)
                        {
                            MessageBox.Show("无记录", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            dataGrid2.ItemsSource = q2.ToList();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("登录失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }

        }
    }
}
