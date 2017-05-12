using System.Windows;
using System.Net.NetworkInformation;
using System.Management;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataLinkApplicationTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SynchronizationContext c;
        PublicTools pt;

        public MainWindow()
        {
            InitializeComponent();
            pt = new PublicTools();
            SysInfo.Content= pt.GetSystemInfo();
            c = SynchronizationContext.Current;

        }

       
        /// <summary>
        /// 一键安装按钮被按下 触发一键安装事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoInstallButtomClick(object sender, RoutedEventArgs e)
        {
            SynchronizationContext contexrt = SynchronizationContext.Current;
            LB.Items.Add("线程启动....");
            Thread thread = new Thread(new ParameterizedThreadStart(ResDB))
            {
                IsBackground = true
            };
            thread.Start(contexrt);
            
           

        }
        /// <summary>
        /// 线程执行方法，此方法调用PublicTools中的SwitchDataBase以在线程中还原数据库。
        /// 并使用context.send 更新UI
        /// </summary>
        /// <param name="sx"></param>
        private void  ResDB(object sx)
        {
            SynchronizationContext uicon = sx as SynchronizationContext;
            pt.SwitchDataBase(1);
            uicon.Send(FlashUI, pt.sql_Result);
            pt.SwitchDataBase(2);
            uicon.Send(FlashUI, pt.sql_Result);
            pt.SwitchDataBase(3);
            uicon.Send(FlashUI, pt.sql_Result);
            pt.SwitchDataBase(4);
            uicon.Send(FlashUI, pt.sql_Result);
            uicon.Send(FlashUI, "wooooooooooow!");
        }
        /// <summary>
        /// 线程回调方法。
        /// </summary>
        /// <param name="str">线程执行的结果</param>
       private void FlashUI(object str)
        {
            string text = str as string;
            LB.Items.Add(text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pt.CreateDir();
        }

        
       
    }
}

