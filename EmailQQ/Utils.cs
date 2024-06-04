using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmailQQ
{
    public class ProxyChecker
    {
        private readonly string _proxyIp;
        private readonly int _proxyPort;

        public ProxyChecker(string proxyIp, int proxyPort)
        {
            _proxyIp = proxyIp;
            _proxyPort = proxyPort;
        }

        public async Task<bool> IsProxyWorkingAsync()
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var connectTask = tcpClient.ConnectAsync(_proxyIp, _proxyPort);
                    var timeoutTask = Task.Delay(5000); // 设置连接超时时间为5秒

                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        // 连接超时
                        return false;
                    }

                    // 连接成功
                    return tcpClient.Connected;
                }
            }
            catch (Exception)
            {
                // 捕获所有异常并返回 false
                return false;
            }
        }
    }
}
