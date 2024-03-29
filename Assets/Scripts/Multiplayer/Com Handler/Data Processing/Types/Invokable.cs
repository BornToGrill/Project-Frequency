﻿using System;
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
            case "StartGame":
                Debug.Log("TODO: Add game start");
                break;
            case "CreateUnit":
                CreateUnit(data.Values);
                break;
            case "SplitUnit":
                SplitUnit(data.Values);
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
            case "Attack":
                Attack(data.Values);
                break;
            case "CashChanged":
                CashChanged(data.Values);
                break;
            default:
                Debug.LogError("Invalid message send to Invokable\n" + values);
                break;
        }
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

    private void CashChanged(string values) {
        int[] data = values.Split(ValueDelimiter).Select(x => Int32.Parse(x)).ToArray();
        _invoke.CashChanged(data[0], data[1]);
    }

    private void SplitUnit(string values) {
        string[] positions = values.Split(ValueDelimiter).Select(x => x.Trim('(', ')')).ToArray();
        int[] start = positions[0].Split(':').Select(x => Int32.Parse(x)).ToArray();
        int[] stop = positions[1].Split(':').Select(x => Int32.Parse(x)).ToArray();
        _invoke.SplitUnit(start[0], start[1], stop[0], stop[1], Int32.Parse(positions[2]));
    }

    private void Attack(string values) {
        string[] positions = values.Split(ValueDelimiter).Select(x => x.Trim('(', ')')).ToArray();
        int[] start = positions[0].Split(':').Select(x => Int32.Parse(x)).ToArray();
        int[] stop = positions[1].Split(':').Select(x => Int32.Parse(x)).ToArray();
        _invoke.Attack(start[0], start[1], stop[0], stop[1]);
    }

}