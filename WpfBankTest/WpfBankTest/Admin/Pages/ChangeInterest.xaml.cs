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
    /// ChangeInterest.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeInterest : Page
    {
        private BankInfoManagementEntities context = new BankInfoManagementEntities();
        public ChangeInterest()
        {
            InitializeComponent();
            this.Unloaded+=ChangeInterest_Unloaded;
            var q = from t in context.RateInfo
                    select t;
            dataGrid1.ItemsSource = q.ToList();
            var q1 = from t in context.RateInfo
                     select new
                     {
                         id=t.RateType,
                         mc = t.RateType
                     };
            bm1.ItemsSource = q1.ToList();

        }

        private void ChangeInterest_Unloaded(object sender, RoutedEventArgs e)
        {
            context.Dispose();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                context.SaveChanges();
                MessageBox.Show("保存成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"保存失败");
            }
        }
    }
}
