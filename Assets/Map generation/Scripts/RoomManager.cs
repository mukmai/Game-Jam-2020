﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class RoomManager : MonoBehaviour
{
    public DoorControl[] doors;
    public EnemyMovement[] enemys;
    public int enemyCount = 0;
    public bool roomBegan = false;

    private void Start()
    {
        foreach (var door in doors)
        {
            door.room = this;
        }

        foreach (var enemy in enemys)
        {
            enemy.room = this;
            enemy.Sleep();
        }

        enemyCount = enemys.Length;
    }

    private void Update()
    {
        if (roomBegan && enemyCount == 0)
        {
            EndRoom();
        }
    }

    public void BeginRoom()
    {
        if (!roomBegan)
        {
            Debug.Log("Room begin");
            foreach (DoorControl door in doors)
            {
                door.Lock();
            }

            roomBegan = true;
            foreach (var enemy in enemys)
            {
                enemy.WakeUp();
            }
        }
    }

    public void EnemyDead()
    {
        enemyCount--;
    }

    void EndRoom()
    {
        Debug.Log("Room clear");
        foreach (DoorControl door in doors)
        {
            door.Unlock();
        }

        enabled = false;
    }
}
