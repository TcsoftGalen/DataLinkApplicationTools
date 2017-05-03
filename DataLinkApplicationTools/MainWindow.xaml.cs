using System.Windows;
using System.Net.NetworkInformation;
using System.Management;

namespace DataLinkApplicationTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ComputerInformation cp;
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
             cp= new ComputerInformation();
            LB.Items.Add("系统版本："+cp.OS_ProductName+" "+cp.OS_CurrentBuildNumber);
            if (cp.OS_InstallationType == "Server")
            {
                LB.Items.Add("子版本：" + cp.OS_CSDVersion); 
            }
            LB.Items.Add("系统类型：" + cp.OS_InstallationType);
            LB.Items.Add("IIS版本："+cp.IIS_ProductString + " " + cp.IIS_VersionString);
            LB_2.Items.Add(cp.Net_Info);
        }

        private void CB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}

