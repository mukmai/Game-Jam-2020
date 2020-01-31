using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [SerializeField] private Transform upDoor;
    [SerializeField] private Transform downDoor;
    public RoomManager room;
    int state = 0;

    void Open()
    {
        if (state == 0)
        {
            state = 1;
            room.BeginRoom();
            StartCoroutine(MoveOverSeconds(0.25f));
        }
    }

    public void Lock()
    {
        if (state == 0)
        {
            state = 2;
        }
    }

    public void Unlock()
    {
        if (state == 2)
        {
            state = 0;
        }
        Open();
    }
    
    public IEnumerator MoveOverSeconds (float seconds)
    {
        float elapsedTime = 0;
        Vector3 upStart = upDoor.position;
        Vector3 downStart = downDoor.position;
        float moveScale = transform.localScale.x * 1.1f;
        while (elapsedTime < seconds)
        {
            upDoor.position = Vector3.Lerp(upStart, upStart + moveScale * Vector3.up, (elapsedTime / seconds));
            downDoor.position = Vector3.Lerp(downStart, downStart + moveScale * Vector3.down, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        upDoor.position = upStart + moveScale * Vector3.up;
        downDoor.position = downStart + moveScale * Vector3.down;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Open();
        }
    }
}
