using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Lab3Server
{
    internal class ClientConnection : INotifyPropertyChanged
    {
        private static int ClientNumber = 0;

        private Socket Sock;
        private SocketAsyncEventArgs SockAsyncEventArgs;
        private byte[] buff;

        public ClientConnection(Socket AcceptedSocket)
        {
            ClientNumber++;
            buff = new byte[1024];
            Sock = AcceptedSocket;
            SockAsyncEventArgs = new SocketAsyncEventArgs();
            SockAsyncEventArgs.Completed += SockAsyncEventArgs_Completed;
            SockAsyncEventArgs.SetBuffer(buff, 0, buff.Length);

            ReceiveAsync(SockAsyncEventArgs);
        }


        ~ClientConnection()
        {
            Console.WriteLine("destructor");
            try { Sock.Close(); }
            catch (Exception) { }
        }


        private void SockAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success) return;
            var sqea = new SocketAsyncEventArgs();
            sqea.Completed += SockAsyncEventArgs_Completed;
            sqea.SetBuffer(buff, 0, buff.Length);
            try
            {
                ReceiveAsync(sqea);
            }
            catch (Exception)
            {
                Console.WriteLine("error ProcessSend");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success || e.Buffer.Length <= 0) return;
            switch (e.Buffer[0])
            {

            }
        }

        public void SendBytes(byte[] data)
        {
            SocketAsyncEventArgs enew = new SocketAsyncEventArgs();
            enew.Completed += SockAsyncEventArgs_Completed;
            enew.SetBuffer(data, 0, data.Length);
            SendAsync(enew);
        }

        private void ReceiveAsync(SocketAsyncEventArgs e)
        {
            bool willRaiseEvent = Sock.ReceiveAsync(e);
            if (!willRaiseEvent)
                ProcessReceive(e);
        }

        private void SendAsync(SocketAsyncEventArgs e)
        {
            bool willRaiseEvent = Sock.SendAsync(e);
            if (!willRaiseEvent)
                ProcessSend(e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
