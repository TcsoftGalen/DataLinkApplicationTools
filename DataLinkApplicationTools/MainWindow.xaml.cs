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

        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (NetworkInterface adapter in adapters)
            {
                //以太网
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                    foreach (UnicastIPAddressInformation ipadd in ipCollection)
                    {
                        if (ipadd.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && adapter.OperationalStatus == OperationalStatus.Up)
                        {

                            switch (adapter.Name)
                            {
                                case "以太网":
                                    LB.Items.Add("名称：" + adapter.Name);
                                    LB.Items.Add("地址：" + ipadd.Address.ToString());
                                    LB.Items.Add("状态：" + "已连接");
                                    break;
                                case "本地连接1":
                                    LB_2.Items.Add("名称：" + adapter.Name);
                                    LB_2.Items.Add("地址：" + ipadd.Address.ToString());
                                    LB_2.Items.Add("状态：" + "已连接");
                                    break;


                            }

                        }
                        else
                        {
                            
                        }
                    }
                }
                else
                {
                    LB_2.Items.Add(MethodClass.GetSystemVersion());
                    LB_2.Items.Add(MethodClass.GetIisServerVersion());
                    LB_2.Items.Add(MethodClass.GetSqlServerVersion(1));
                    LB_2.Items.Add("名称：" + adapter.Name);                 
                    LB_2.Items.Add("状态：" + "已连接");
                }
                
            }
        }
    }
}
