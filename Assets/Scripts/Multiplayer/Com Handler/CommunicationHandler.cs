using System;
using System.Net;
using System.Net.Sockets;
using NetworkLibrary;
using TcpClient = NetworkLibrary.TcpClient;
using UdpClient = NetworkLibrary.UdpClient;

class CommunicationHandler : IDisposable {


    private readonly TcpClient _tcpClient;
    private readonly UdpClient _udpClient;

    private DataProcessor _processor;

    public Invoke Invoke { get; private set; }
    public Notify Notify { get; private set; }

    private string _guid;

    public CommunicationHandler(TcpClient serverCon, IInvokable invoke, INotifiable notify, ILobby lobby) {
        _tcpClient = serverCon;
        _tcpClient.DataReceived += TcpClient_DataReceived;
        _tcpClient.Start();

        _processor = new DataProcessor(invoke, notify, lobby);
    }

    public void SetGuid(string guid) {
        _guid = guid;
        Invoke = new Invoke(_tcpClient, guid);
        Notify = new Notify(_tcpClient, guid);
    }

    public void SetProcessor(DataProcessor processor) {
        _processor = processor;
    }

    public void SendTcp(string message) {
        _tcpClient.Send(message);
    }

    private void TcpClient_DataReceived(TcpDataReceivedEventArgs e) {
        UnityEngine.Debug.Log(e.ReceivedString);
        if (e.ReceivedString.Length > 0)
            _processor.ProcessData(e.Sender, e.ReceivedString);
    }


    protected void Dispose(bool disposing) {
        if (disposing) {
            if(_tcpClient != null)
                _tcpClient.Dispose();
            if (_udpClient != null)
                _udpClient.Dispose();
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

