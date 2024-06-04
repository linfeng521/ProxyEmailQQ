using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace EmailQQ
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // 从 app.config 读取代理配置
        string proxyIp = ConfigurationManager.AppSettings["ProxyIP"];
        int proxyPort = int.Parse(ConfigurationManager.AppSettings["ProxyPort"]);
        bool isProxyWorking = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            CheckProxyCon();
            InitializeWebView();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取屏幕宽度和高度
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            // 设置窗口宽度和高度为屏幕宽度和高度的80%
            this.Width = screenWidth * 0.8;
            this.Height = screenHeight * 0.8;

            // 计算窗口的左上角位置，以使窗口居中
            this.Left = (screenWidth - this.Width) / 2;
            this.Top = (screenHeight - this.Height) / 2;
        }

        private async void CheckProxyCon()
        {
            // 检查代理是否正常
            var proxyChecker = new ProxyChecker(proxyIp, proxyPort);
            isProxyWorking = await proxyChecker.IsProxyWorkingAsync();
            if (!isProxyWorking)
            {
                StatusTextBlock.Text = "代理服务器不可用，请检查代理设置。";
                StatusTextBlock.Foreground = Brushes.DarkRed;
                return;
            }
            else
            {
                StatusTextBlock.Text = "Proxy服务器连接就绪";
                StatusTextBlock.Foreground = Brushes.DarkGreen;
            }
        }

        private async void InitializeWebView()
        {

                // 如果代理工作设置代理
                var environment = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions
                {
                    AdditionalBrowserArguments = $"--proxy-server={proxyIp}:{proxyPort}"
                });
                // 初始化WebView2
                await webView.EnsureCoreWebView2Async(environment);
                // 处理导航事件
                webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
                webView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;

                // 导航到指定网址
                webView.CoreWebView2.Navigate("https://mail.powerchina.cn");


           
        }

        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            // 更新状态栏中的URL
            UrlTextBlock.Text = $"URL: {e.Uri}";
        }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // 处理导航完成事件
            if (!e.IsSuccess)
            {
                var statusCode = e.WebErrorStatus;
            }
            else
            {
                StatusTextBlock.Text = "状态: 导航成功";
                StatusTextBlock.Foreground = Brushes.Green;

                // 更新状态栏中的URL
                UrlTextBlock.Text = $"网页: {webView.Source}";
            }
        }
    }
}
