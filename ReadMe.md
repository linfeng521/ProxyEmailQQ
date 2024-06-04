# 项目介绍
局域网代理访问企业邮箱
## 背景
局域网有一个代理服务器（`pfsense`+`squid`),之前通过RemoteAPP共享，后发现有少部分无法打开，故开发一个本地程序(架构：`wpf`+`webview2`)其中webview2设置代理环境
## 技术点
1. async/await
2. webview2
3. dockpanel
4. nisi生成单文件程序
## 关键代码snippet
1. 初始化webview2代理环境
```csharp
// 如果代理工作设置代理
var environment = await CoreWebView2EnvironmCreateAsync(null, null, CoreWebView2EnvironmentOptions
{
    AdditionalBrowserArguments = $"--proxy-ser{proxyIp}:{proxyPort}"
});
// 初始化WebView2(确保 WebView2 控件已经初始化的异步方法)
await webView.EnsureCoreWebView2Async(environment);
// 处理导航事件
webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
webView.CoreWebView2.NavigationCompletedCoreWebView2_NavigationCompleted;

// 导航到指定网址
 webView.CoreWebView2.Navigate("https://mail.powerchina.cn");
```
2. NSIS单文件打包工具 v2021.12.21.3
 
https://www.52pojie.cn/thread-1696762-1-1.html