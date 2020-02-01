using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class RoomManager : MonoBehaviour
{
    public DoorControl[] doors;
    public EnemyMovement[] enemys;
    public int enemyCount = 0;
    private bool roomBegan = false;

    private void Start()
    {
        foreach (var door in doors)
        {
            door.room = this;
        }

        foreach (var enemy in enemys)
        {
            enemy.room = this;
            enemy.gameObject.SetActive(false);
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
        foreach (DoorControl door in doors)
        {
            door.Lock();
        }

        roomBegan = true;
        foreach (var enemy in enemys)
        {
            enemy.gameObject.SetActive(true);
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
    }
}
