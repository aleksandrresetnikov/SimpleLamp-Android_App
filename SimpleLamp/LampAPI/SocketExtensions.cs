using System.Diagnostics;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SimpleLamp.LampAPI
{
    internal static class SocketExtensions
    {
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
            finally { GC.Collect(); }
        }

        public static Boolean GetPing(String host)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = "ping.exe";
                p.StartInfo.Arguments = "-n 1 " + host;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("UTF-8");
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                Boolean status = output.Contains("TTL=");
                return status;
            }
        }

        public static bool GetServerAvailable(string host, int port)
        {
            try
            {
                TcpClient tcpClient = new TcpClient();
                Task invoke = tcpClient.ConnectAsync(host, port);
                Thread.Sleep(1000);
                NetworkStream networkStream = tcpClient.GetStream();

                return true;
            }
            catch (Exception) { return false; }
            finally  { GC.Collect(); }
        }
    }
}
