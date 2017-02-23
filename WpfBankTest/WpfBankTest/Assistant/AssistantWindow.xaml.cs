using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// AssistantWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AssistantWindow : Window
    {
       private BankInfoManagementEntities Context1 = new BankInfoManagementEntities();
       db Db = new db();
        public AssistantWindow()
        {
            InitializeComponent();
            
          
           
        }

        private void menuItemCK_Click(object sender, RoutedEventArgs e)
        {
            tabItemCK.IsSelected = true;
        }
        /// <summary>
        /// 存款界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitCK_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BankInfoManagementEntities())
            {
                var q = from t in context.CreditCardInfo
                        where t.CNo == textBoxCNo.Text
                        where t.CPassword == textBoxCPwd.Password
                        select t;
                if (q.ToList().Count == 0)
                {
                    MessageBox.Show("用户名或密码错误！！请重新输入！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBoxCNo.Text = "";
                    textBoxCPwd.Password = "";
                }
                else
                {
                        db Db = new db();
                        string Procedure_CK = "exec DepositOperation'" + textBoxCNo.Text + "','" + textBoxCPwd.Password + "','" + textBoxCName.Text + "','" +  Convert.ToDecimal(textBoxCMoney.Text) + "','" + comboBoxCKType.Text.ToString() + "','" + Convert.ToInt32(textBoxCTime.Text) + "'";
                        if (Db.SQlCommand(Procedure_CK) > 0)
                        {
                            MessageBox.Show("存款成功");

                        }
                        else
                        {
                            MessageBox.Show("存款失败");
                        }

                }

            } 
               }
        private void menuItemQk_Click(object sender, RoutedEventArgs e)
        {
            tabItemQK.IsSelected = true;
        }

        private void btnSubmitQK_Click(object sender, RoutedEventArgs e)
        {
            decimal y = 0, t1 = 0;
            using (var context = new BankInfoManagementEntities())
            {
                var q = from t in context.CreditCardInfo
                        where t.CNo == textBoxQNo.Text
                        where t.CPassword == textBoxQPwd.Password
                        select t;
                if (q.ToList().Count == 0)
                {
                    MessageBox.Show("用户名或密码错误！！请重新输入！", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBoxCNo.Text = "";
                    textBoxCPwd.Password = "";
                }
                else
                {
                    db Db = new db();
                    SqlConnection conn = db.Camcon();
                    conn.Open();

                    string str1 = "select CBalance from CreditCardInfo where CNo='" + textBoxQNo.Text + "'";
                    SqlCommand comm = new SqlCommand(str1, conn);
                    SqlDataReader sdr = comm.ExecuteReader();
                    sdr.Read();
                    y = Convert.ToDecimal(sdr["CBalance"].ToString());
                    sdr.Close();
                    t1 = y - Convert.ToDecimal(textBoxQMoney.Text);
                    if (t1 > 0)
                    {
                        textBoxQType.Text = "取款";
                        string Procedure_QK = "exec DepositOperation'" + textBoxQNo.Text + "','" + textBoxQPwd.Password + "','" + textBoxQName.Text + "','" + Convert.ToDecimal(textBoxQMoney.Text) + "','" + textBoxQType.Text + "','" + "0" + "'";
                        MessageBox.Show("\r\t卡上总余额" + y + "人民币\r\t可操作余额" + t1 + "人民币");
                        if (Db.SQlCommand(Procedure_QK) > 0)
                        {
                            MessageBox.Show("取款成功");

                        }
                        else
                        {
                            MessageBox.Show("取款失败");
                        }
                    }
                    else
                    {
                        MessageBox.Show("\r\t卡上总余额" + y + "人民币\r\t可操作余额" + y + "人民币" + "余额不足", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }

            } 
            
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new BankInfoManagementEntities())
            {
                var q = from t in context.OperationLog
                        where t.CDate == datePicker1.SelectedDate
                        select new
                        {
                            账号 = t.CNo,
                            客户姓名 = t.CName,
                            操作类型=t.CStyle,
                            日期=t.CDate,
                            金额=t.CMoney,
                            存款时长=t.CTimeLen,
                            利率=t.CRate

                        };
                dataGrid1.ItemsSource = q.ToList();
            var q1=(from t in context.OperationLog
                  where t.CStyle.Contains("存")
                  select t).Sum(x=>x.CMoney);
            textBoxInTotal.Text = q1.ToString() + "¥";
           var q2 = (from t in context.OperationLog
                     where t.CStyle.Contains("取")
                     select t).Sum(x => -x.CMoney);
           textBoxOutTotal.Text = q2.ToString() + "¥";
        }
                
        }
        /// <summary>
        /// 存取款查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckDepositRecord_Click(object sender, RoutedEventArgs e)
        {
           
            using (var context = new BankInfoManagementEntities())
                {
                    
                    var q = from t in context.CreditCardInfo
                            where t.CNo==textBoxCKLogNo.Text
                            where t.CPassword==pwdBoxCKLog.Password
                            select t;
                    if (q.ToList().Count() != 0) 
                    {
                        MessageBox.Show("登陆成功","提示",MessageBoxButton.OK,MessageBoxImage.Information);
                        if (radioButtonCK.IsChecked == true)
                        {
                            var q1 = from t in context.OperationLog
                                     where t.CDate == datePickerCK.SelectedDate
                                     where t.CNo == textBoxCKLogNo.Text
                                     where t.CStyle.Contains("存")
                                     select new
                                     {
                                         账号 = t.CNo,
                                         客户姓名 = t.CName,
                                         操作类型 = t.CStyle,
                                         日期 = t.CDate,
                                         金额 = t.CMoney,
                                         存款时长 = t.CTimeLen,
                                         利率 = t.CRate

                                     };
                            if (q1.ToList().Count == 0)
                            {
                                MessageBox.Show("无存款记录", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                dataGridCK.ItemsSource = q1.ToList();
                            }
                        }
                        else if (radioButtonQK.IsChecked==true)
                        {
                            var q2 = from t in context.OperationLog
                                     where t.CDate == datePickerCK.SelectedDate
                                     where t.CNo == textBoxCKLogNo.Text
                                     where t.CStyle.Contains("取")
                                     select new
                                     {
                                         账号 = t.CNo,
                                         客户姓名 = t.CName,
                                         操作类型 = t.CStyle,
                                         日期 = t.CDate,
                                         金额 = t.CMoney
                                     };
                            if (q2.ToList().Count == 0)
                            {
                                MessageBox.Show("无取款记录", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                dataGridCK.ItemsSource = q2.ToList();
                            }
                        }
                     
                    }
                    else
                    {
                        MessageBox.Show("登陆失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
  
                    }

         
        }

        private void btnVerifyChange_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BankInfoManagementEntities())
            {

                var q = from t in context.EmploeeInfo
                        where t.YNo == textBoxChangeYNo.Text
                        where t.YPassword == pwdBoxOldYPwd.Password
                        select t;
                if (q.ToList().Count() != 0)
                {
                  
	
                    foreach (var v in q)
                    {
                        v.YPassword = pwdBoxNewYPwd.Password;
                    }
                    context.SaveChanges();
                    MessageBox.Show("修改密码成功！");
               


                }
                else
                {
                    MessageBox.Show("修改密码失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void btnVerifyUpdate_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BankInfoManagementEntities())
            {

                var q = from t in context.CreditCardInfo
                        where t.CNo == textBoxChangeCNo.Text
                        where t.CPassword == textBoxChangeCPwd.Password
                        select new 
                        {
                            账号 = t.CNo,
                            客户姓名 = t.CName,
                           开户日期=t.CDate,
                            开户地址 = t.COpenAdress,
                           手机号=t.CPhoneNo
                        };
                dataGridPersonalInfo.ItemsSource = q.ToList();
                 var q1 = from t in context.CreditCardInfo
                        where t.CNo == textBoxChangeCNo.Text
                        select t;
                if (q1.ToList().Count() != 0)
                {
                    foreach (var v in q1)
                    {
                        v.CPassword = textBoxChangeNewPwd.Password;
                    }
                    context.SaveChanges();
                    MessageBox.Show("修改密码成功！");



                }
                else
                {
                    MessageBox.Show("修改密码失败，账户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void menuItemDayTotal_Click(object sender, RoutedEventArgs e)
        {
            tabItemDayTotal.IsSelected = true;
        }

        private void menuItemCKLog_Click(object sender, RoutedEventArgs e)
        {
            tabItemCheckCK.IsSelected = true;
        }

        private void menuItemChangeYPwd_Click(object sender, RoutedEventArgs e)
        {
            tabItemChangeKPwd.IsSelected = true;
        }

        private void menuItemChangeCPwd_Click(object sender, RoutedEventArgs e)
        {
            tabItemChangeYPwd.IsSelected = true;
        }

        private void menuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            tabItemHelp.IsSelected = true;
        }

        private void menuItemAboutSoftware_Click(object sender, RoutedEventArgs e)
        {
            tabItemAboutSoft.IsSelected = true;
        }

        private void menuItemCloseAssistant_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.ShowDialog();
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
        /// <summary>
        /// 储户查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            
             var q1 = from t in Context1.CreditCardInfo
                      where t.CNo==textBoxCheckAccount.Text where t.CPassword==pwdBoxCheckPwd.Password
                      select t;
             dataGridAccountInfo.ItemsSource = q1.ToList();
        }
        /// <summary>
        /// 确认修改储户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerifyChangeInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Context1.SaveChanges();
                MessageBox.Show("保存成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "保存失败");

            }
        }
        
        private void menuItemAccountSearch_Click(object sender, RoutedEventArgs e)
        {
            tabItemAccountSearch.IsSelected = true;
        }

        private void menuItemPersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            tabItemAccountSearch.IsSelected = true;
        }
        /// <summary>
        /// 确认开户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAccount_Click(object sender, RoutedEventArgs e)
        {
            DateTime d =Convert.ToDateTime(datePickerOpen.SelectedDate);
            using (var context = new BankInfoManagementEntities())
            {
                DeposInfo deposinfo = new DeposInfo()
                {
                    CName = textBoxKCName.Text,
                    CSex = textBoxOpenSex.Text,
                    CNo = textBoxKCNo.Text,
                    CPassword = textBoxKCPwd.Password,
                    CID = textBoxOpenID.Text,
                    CDate = d,
                    CAdress = textOpenAddress.Text,
                    CPhoneNum = textBoxPhoneNo.Text


                };
                try
                {


                    context.DeposInfo.Add(deposinfo);
                    context.SaveChanges();
                    MessageBox.Show("开户成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (Exception ex)
                {

                    MessageBox.Show("开户失败" + ex.ToString());
                }
            }
        
                
        }
        /// <summary>
        /// 生成银行卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateBankCard_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = db.Camcon();
            conn.Open();
            string str = "10103376 ";
            Random r = new Random();
            for (int i = 0; i < 4; i++)
            {
                str += r.Next(0, 9);
            }
            str += " ";
            for (int i = 0; i < 4; i++)
            {
                str += r.Next(0, 9);
            }
           textBoxKCNo.Text = str;
            
            string str1 = "select CNo from DeposInfo where CNo = '" + textBoxKCNo.Text + "'";
            SqlCommand comm = new SqlCommand(str1, conn);
            int j = Convert.ToInt32(comm.ExecuteScalar());
            conn.Close();
            if (j > 0)
            {
                MessageBox.Show("此账号已存在，请重新生成", "此账号已存在", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// 销户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelAccount_Click(object sender, RoutedEventArgs e)
        {

            string VerifyAccount = "select count(*) from CreditCardInfo where CNo = '" + textBoxXCNo.Text + "'and CPassword = '" + textBoxXCPwd.Password + "'";
            if (Db.SqlSelect(VerifyAccount) > 0)
            {
                string DeAccount = "delete from CreditCardInfo where CNo = '" + textBoxXCNo.Text + "'";
                if (Db.SQlCommand(DeAccount) > 0)
                {
                    MessageBox.Show("销户成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("删除账户失败", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
            else
            {
                MessageBox.Show("用户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        /// <summary>
        /// 确认挂失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportAccount_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxGCNo.Text.Length < 1 || textBoxGCPwd.Password.Length < 1)
            {
                MessageBox.Show("请填写账号或密码。");
                return;
            }
            string VerifyAccount = "select count(*) from CreditCardInfo where CNo = '" + textBoxGCNo.Text + "'and CPassword = '" + textBoxGCPwd.Password + "'";
            if (Db.SqlSelect(VerifyAccount) > 0)
            {
                string ReportLoss = "update  CreditCardInfo set CLoss='" + "是" + "' where CNo = '" + textBoxGCNo.Text + "'";
                if (Db.SQlCommand(ReportLoss) > 0)
                {
                    MessageBox.Show("挂失成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("挂失失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            else
            {
                MessageBox.Show("用户名或密码错误", "警告", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
        }
        /// <summary>
        /// 打印存款单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDepositRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\CKBill.txt"))
            {
                //File.Create(Application.StartupPath + "\\AlarmSet.txt");//创建该文件   

                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\CKBill.txt", FileMode.Create, FileAccess.Write);//创建写入文件    
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("                             存款单                                ");///开始写入值   
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("存款日期：" + DateTime.Now.ToShortDateString() + "                        " + "存款类型：" + comboBoxCKType.SelectedValue.ToString());
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("账号：" + textBoxCNo.Text + "    " + "用户姓名：" + textBoxCName.Text + "        " + "存款金额：" + textBoxCMoney.Text + "￥");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("币种：人民币" + "                " + "存款时长：" + textBoxCTime.Text + "年" + "          " + "存款利率：0.2%");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.Close();
                fs1.Close();
                MessageBox.Show("打印存款单成功！");
            }
        }

        private void btnQKRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\QKBill.txt"))
            {
                //File.Create(Application.StartupPath + "\\AlarmSet.txt");//创建该文件   

                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\QKBill.txt", FileMode.Create, FileAccess.Write);//创建写入文件    
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("                             取款单                                ");//开始写入值   
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("取款日期：" + DateTime.Now.ToShortDateString() + "        " + "币种：人民币" + "        " + "操作类型：" + textBoxQType.Text);
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("账号：" + textBoxQNo.Text + "    " + "用户姓名：" + textBoxQName.Text + "     " + "取款金额：" + textBoxQMoney.Text + "￥");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.Close();
                fs1.Close();
                MessageBox.Show("打印取款单成功！");
            }
        }
        /// <summary>
        /// 转账界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitZZ_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxZNo.Text == "")
            {
                MessageBox.Show("请输入账户！");
            }
            else if (textBoxZINo.Text == "")
            {
                MessageBox.Show("请输入对方银行卡号！");
            }
            else if (textBoxZMoney.Text == "")
            {
                MessageBox.Show("请输入转账金额！");
            }
            else if (textBoxZName.Text == "")
            {
                MessageBox.Show("请验证姓名！");
            }
            else if (textBoxZNo.Text == textBoxZINo.Text)
            {
                MessageBox.Show("对方账号与本用户相同，请重新输入");
                textBoxZINo.Text = "";
            }
            else if (Convert.ToDecimal(textBoxZMoney.Text) < 0)
            {
                MessageBox.Show("转账金额不得低于0元");
                textBoxZMoney.Text = "";
            }
            else
            {

                decimal b = Convert.ToDecimal(textBoxZMoney.Text);
                string SearchAccount1 = "select count(*) from CreditCardInfo where CNo='" + textBoxZNo.Text + "' and CLoss='" + "否" + "'";
                string SearchAccount2 = "select count(*) from CreditCardInfo where CNo='" + textBoxZINo.Text + "' and CLoss='" + "否" + "'";

                int i = Db.SqlSelect(SearchAccount1);
                int j = Db.SqlSelect(SearchAccount2);
                if (i > 0)
                {
                    if (j > 0)
                    {
                        string VerifyAccount = "select count(*) from CreditCardInfo where CNo = '" + textBoxZNo.Text + "'and CPassword = '" + textBoxZPwd.Password+ "'";
                        if (Db.SqlSelect(VerifyAccount) == 0)
                        {
                            MessageBox.Show("用户名或密码错误,请重新输入");
                            textBoxZPwd.Password = "";
                            b++;
                            if (b >= 3)
                            {
                                MessageBox.Show("密码错误超过三次，禁止操作！！");
                                
                            }
                        }
                        else
                        {
                            SqlConnection conn = db.Camcon();
                            conn.Open();

                            string str1 = "select CBalance from CreditCardInfo where CNo='" + textBoxZNo.Text + "'";
                            SqlCommand comm = new SqlCommand(str1, conn);
                            SqlDataReader sdr = comm.ExecuteReader();
                            sdr.Read();
                            decimal x = Convert.ToDecimal(sdr["CBalance"].ToString());
                            sdr.Close();
                            if (x > b)
                            {
                                if (MessageBox.Show("确定转账 ￥" + textBoxZMoney.Text + " 到用户：" + textBoxZINo.Text + " 吗？", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                                {
                                    string Procedure_ZZ = "exec TransferMoney'" + textBoxZNo.Text + "','" + textBoxZPwd.Password + "','" + textBoxZName.Text + "','" + textBoxZINo.Text+ "','" + Convert.ToDecimal(textBoxZMoney.Text) + "'";
                                    if (Db.SQlCommand(Procedure_ZZ) > 0)
                                    {
                                        MessageBox.Show("转账成功");

                                    }
                                    else
                                    {
                                        MessageBox.Show("转账失败");
                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("余额不足");
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("转出账户被冻结，无法转账", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("账户被冻结，无法操作", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }

        }

        private void btnZZRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\ZZBill.txt"))
            {
                //File.Create(Application.StartupPath + "\\AlarmSet.txt");//创建该文件   

                FileStream fs1 = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\ZZBill.txt", FileMode.Create, FileAccess.Write);//创建写入文件    
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("                             转账单                                ");//开始写入值   
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("转账日期：" + DateTime.Now.ToShortDateString() + "   " + "币种：人民币" + "   " + "操作类型：" + "  转账  ");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("转出账号：" + textBoxZNo.Text + "              " + "用户姓名：" + textBoxZName.Text);
                sw.WriteLine("-------------------------------------------------------------------");
                sw.WriteLine("转入账号：" + textBoxZINo.Text + "              " + "转账金额：" + textBoxZMoney.Text + "￥");
                sw.WriteLine("-------------------------------------------------------------------");
                sw.Close();
                fs1.Close();
                MessageBox.Show("打印转账单成功！");
            }
        }
        /// <summary>
        /// 转账记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckZZRecord_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new BankInfoManagementEntities())
                {
                    
                    var q = from t in context.CreditCardInfo
                            where t.CNo==textBoxZZLogNo.Text
                            where t.CPassword==pwdBoxZZLog.Password
                            select t;
                    if (q.ToList().Count() != 0)
                    {
                        MessageBox.Show("登陆成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                        var q1 = from t in context.TransferInfo
                                 where t.TDate == datePickerZZ.SelectedDate
                                 where t.OutNo == textBoxZZLogNo.Text
                                 select new
                                 {
                                     转出账号 = t.OutNo,
                                     客户姓名 = t.OutName,
                                     转入账户 = t.InNo,

                                     金额 = t.TransMoney,
                                     日期 = t.TDate

                                 };
                        if (q1.ToList().Count == 0)
                        {
                            MessageBox.Show("无存款记录", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            dataGridZZ.ItemsSource = q1.ToList();
                        }
                    }
                    }
        }

        private void btnCalculateInterest_Click(object sender, RoutedEventArgs e)
        {
            btnCalculateInterest ci = new btnCalculateInterest();
            ci.ShowDialog();

        }

       

      
    }
}
