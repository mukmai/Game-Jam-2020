using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private StateManager _stateManager;
    private void Start()
    {
        _stateManager = GameObject.Find("/StateManager").GetComponent<StateManager>();
    }

    private void OnDestroy()
    {
        _stateManager.WinGame();
    }
}
