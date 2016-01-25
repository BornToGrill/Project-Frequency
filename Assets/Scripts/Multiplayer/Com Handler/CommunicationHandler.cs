using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NetworkLibrary;
using TcpClient = NetworkLibrary.TcpClient;
using UdpClient = NetworkLibrary.UdpClient;

class CommunicationHandler {


    private readonly TcpClient _tcpClient;
    private readonly UdpClient _udpClient;

    private readonly DataProcessor _processor;

    public CommunicationHandler(IInvokable invoke) {
        Socket connSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        connSocket.Connect(IPAddress.Parse("127.0.0.1"), 9500); //TODO: Settings
        _tcpClient = new TcpClient(connSocket);
        _tcpClient.DataReceived += _tcpClient_DataReceived;
        _tcpClient.Start();

        _processor = new DataProcessor(invoke);
    }

    private void _tcpClient_DataReceived(TcpDataReceivedEventArgs e) {
        if (e.ReceivedString.Length > 0)
            _processor.ProcessData(e.Sender, e.ReceivedString);
    }
}

