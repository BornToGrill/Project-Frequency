using System;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary;
using UnityEngine;

class Invokable {

    private readonly IInvokable _invoke;
    private const char ValueDelimiter = '|';

    public Invokable(IInvokable invoke) {
        _invoke = invoke;
    }

    public void HandleInvoke(TcpClient client, string values) {
        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "SetPlayers":
                SetPlayers(data.Values);
                break;
            case "Authenticated":
                Authenticated(data.Values);
                break;
            case "StartGame":
                Debug.Log("TODO: Add game start");
                break;
            case "CreateUnit":
                CreateUnit(data.Values);
                break;
            case "MoveToEmpty":
                Move(_invoke.MoveToEmpty, data.Values);
                break;
            case "MoveToMerge":
                Move(_invoke.MoveToMerge, data.Values);
                break;
            case "MoveToAttack":
                Move(_invoke.MoveToAttack, data.Values);
                break;
            default:
                Debug.LogError("Invalid message send to Notify\n" + values);
                break;
        }
    }


    private void SetPlayers(string values) {
        string[] players =
            values.Split(new[] { ")" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart('(')).ToArray();
        List<string> names = new List<string>();
        List<int> ids = new List<int>();
        foreach (string player in players) {
            string[] split = player.Split(ValueDelimiter);
            names.Add(split[0]);
            ids.Add(Int32.Parse(split[1]));
        }
        _invoke.SetPlayers(names.ToArray(), ids.ToArray());
    }

    private void Authenticated(string values) {
        string[] data = values.Split(ValueDelimiter);
        _invoke.Authenticated(data[0], Int32.Parse(data[1]));
    }

    private void CreateUnit(string values) {
        string[] data = values.Split(ValueDelimiter);
        int[] start = data[0].Split(':').Select(x => x.Trim('(',')')).Select(c => Int32.Parse(c)).ToArray();
        _invoke.CreateUnit(start[0], start[1], data[1], Int32.Parse(data[2]));
    }

    private void Move(Action<int,int,int,int> action, string values) {
        string[] positions = values.Split(ValueDelimiter).Select(x => x.Trim('(', ')')).ToArray();
        int[] start = positions[0].Split(':').Select(x => Int32.Parse(x)).ToArray();
        int[] stop = positions[1].Split(':').Select(x => Int32.Parse(x)).ToArray();
        action.Invoke(start[0], start[1], stop[0], stop[1]);
    }



}