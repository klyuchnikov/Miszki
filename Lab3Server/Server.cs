﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Lab3Server
{
    internal class Server : INotifyPropertyChanged
    {
        private Socket Sock;
        private SocketAsyncEventArgs AcceptAsyncArgs;
        private bool IsOpen { get; set; }

        private Server()
        {
            ListConnection = new List<ClientConnection>();
        }

        public static Server Current = new Server();


        public List<ClientConnection> ListConnection { get; set; }


        public void SendPropertiesChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("ListConnection"));
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (IsOpen)
            {
                if (e.SocketError == SocketError.Success)
                {
                    ClientConnection Client = new ClientConnection(e.AcceptSocket);
                    ListConnection.Add(Client);
                    SendPropertiesChanged();
                }
                e.AcceptSocket = null;
                AcceptAsync(AcceptAsyncArgs);
            }
        }

        private void AcceptAsync(SocketAsyncEventArgs e)
        {
            bool willRaiseEvent = Sock.AcceptAsync(e);
            if (!willRaiseEvent)
                AcceptCompleted(Sock, e);
        }

        public void Start(int Port)
        {
            Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            AcceptAsyncArgs = new SocketAsyncEventArgs();
            AcceptAsyncArgs.Completed += AcceptCompleted;
            Sock.Bind(new IPEndPoint(IPAddress.Any, Port));
            Sock.Listen(50);
            AcceptAsync(AcceptAsyncArgs);
            IsOpen = true;

        }

        public void Stop()
        {
            Sock.Close();
            IsOpen = false;
            ListConnection = new List<ClientConnection>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
