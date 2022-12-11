using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SimpleLamp.LampAPI
{
    public class MatrixStream : IDisposable, IMatrixMethods
    {
        public string LampIP { get; private protected set; }
        public int LampPORT  { get; private protected set; }

        public Queue<byte> matrixNetworkStreamReadHistory { get; private protected set; }

        private protected TcpClient matrixTcpClient;
        private protected NetworkStream matrixNetworkStream;
        private protected StringBuilder response;

        public MatrixStream(string LampIP = "192.168.4.1", int LampPORT = 80) 
        {
            this.LampIP = LampIP;
            this.LampPORT = LampPORT;

            this.matrixTcpClient = new TcpClient();
            this.response = new StringBuilder();
            this.matrixNetworkStreamReadHistory = new Queue<byte>();
        }

        public void Connect()
        {
            if (!SocketExtensions.GetServerAvailable(this.LampIP, this.LampPORT))
                throw new Exception();

            this.matrixTcpClient.Connect(this.LampIP, this.LampPORT);
            this.matrixNetworkStream = matrixTcpClient.GetStream();
        }

        public void Disconnect()
        {
            this.matrixNetworkStream.Close();
            this.matrixTcpClient.Dispose();
            this.matrixTcpClient = new TcpClient();
        }

        public void ForceLoadMatrixServer()
        {
            int bufferSize = 64;
            Task.Run(() =>
            {
                for (; ; )
                    this.matrixNetworkStream.Write(MatrixNativeUtil.CreateBuffer(bufferSize), 0, bufferSize);
            });
        }

        public void ClearMatrixNetworkStreamReadHistory()
        {
            this.matrixNetworkStreamReadHistory.Clear();
        }

        public bool GetConnectionState()
        {
            return this.matrixTcpClient.Connected;
        }

        public override string ToString()
        {
            return $"http://{this.LampIP}:{this.LampPORT}";
        }

        public void Dispose()
        {
            this.matrixTcpClient.Dispose();
            this.matrixNetworkStream.Dispose();
            this.response.Clear();
            this.matrixNetworkStreamReadHistory.Clear();

            GC.SuppressFinalize(response);
            GC.SuppressFinalize(matrixNetworkStreamReadHistory);
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        private protected void MatrixNetworkStream_DataReader()
        {
            try
            {
                this.response = new StringBuilder();
                byte[] data = new byte[256];

                do
                {
                    int bytes = this.matrixNetworkStream.Read(data, 0, data.Length);
                    response.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (this.matrixNetworkStream.DataAvailable);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: {0}", e.Message);
            }

            Debug.WriteLine("Запрос завершен...");
        }

        public void SetMode(byte modeID)
        {
            if (!this.GetConnectionState()) return;
            this.WriteStreamData(255, modeID);
        }

        public void SetBrightness(byte brightness)
        {
            if (!this.GetConnectionState()) return;
            this.WriteStreamData(0, brightness);
        }

        private protected void WriteStreamData(byte item1, byte item2)
        {
            this.matrixNetworkStream.Write(new byte[] { item1, item2 }, 0, 2);
        }
    }

    public interface IMatrixMethods
    {
        void SetMode(byte modeID);
        void SetBrightness(byte brightness);
    }
}
