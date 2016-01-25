using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkLibrary;

class Invoke {

    private readonly IInvokable _invoke;

    public Invoke(IInvokable invoke) {
        _invoke = invoke;
    }

    public void HandleInvoke(TcpClient client, string values) {
        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "PlayerJoined":
                _invoke.PlayerConnected(data.Values);
                break;
        }
    }

}