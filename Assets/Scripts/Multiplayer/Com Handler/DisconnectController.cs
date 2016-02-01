using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DisconnectController : MonoBehaviour {

    public void OnExit() {
        GameObject board = GameObject.Find("Board");
        if (board == null)
            return;
        StateController state = board.GetComponent<StateController>();
        if (state == null)
            return;
        state.ServerComs.Dispose();
    }

}

