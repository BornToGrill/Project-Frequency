﻿using System;
using System.Net;
using System.Net.Sockets;
using NetworkLibrary;
using UnityEngine;
using TcpClient = NetworkLibrary.TcpClient;
using UdpClient = NetworkLibrary.UdpClient;

class CommunicationHandler : IDisposable {


    private readonly TcpClient _tcpClient;
    private readonly UdpClient _udpClient;
    private IErrorHandler _errorHandler;

    private DataProcessor _processor;

    public Invoke Invoke { get; private set; }
    public Notify Notify { get; private set; }



    public CommunicationHandler(TcpClient serverCon, IErrorHandler error, IInvokable invoke, INotifiable notify, ILobby lobby) {
        _errorHandler = error;
        _tcpClient = serverCon;
        _tcpClient.DataReceived += TcpClient_DataReceived;
        _tcpClient.Disconnected += TcpClient_Disconnnected; 
        _tcpClient.Start();

        _processor = new DataProcessor(invoke, notify, lobby);
    }

    private void TcpClient_Disconnnected(TcpClient sender) {
        _errorHandler.ServerDisconnected();
    }

    public void SetGuid(string guid) {
        Invoke = new Invoke(_tcpClient, guid);
        Notify = new Notify(_tcpClient, guid);
    }

    public void SetProcessor(DataProcessor processor) {
        _processor = processor;
    }

    public void SetErrorHandler(IErrorHandler errorHandler) {
        _errorHandler = errorHandler;
    }

    public void SendTcp(string message) {
        _tcpClient.Send(message);
    }

    private void TcpClient_DataReceived(TcpDataReceivedEventArgs e) {
        try {
            Debug.Log(e.ReceivedString);
            if (e.ReceivedString.Length > 0)
                _processor.ProcessData(e.Sender, e.ReceivedString);
        }
        catch (Exception ex) {
            Debug.LogError("Error in com handler : " + ex.Message);
            throw;
        }
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

