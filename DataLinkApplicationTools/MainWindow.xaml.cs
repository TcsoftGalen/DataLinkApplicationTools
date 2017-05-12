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

