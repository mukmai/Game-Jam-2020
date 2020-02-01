using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Health player;

    public Health[] robots;
    public int boltCount = 0;
    private List<Health> survivers = new List<Health>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        survivers.Clear();
        foreach (var robot in robots)
        {
            if (robot.currHealth > 0)
            {
                survivers.Add(robot);
            }
        }
        if (player.currHealth > 0)
        {
            survivers.Add(player);
        }
    }

    public Transform EnemyGetTarget()
    {
        Transform res = transform;
        if (survivers.Count > 0)
        {
            int rand = Random.Range(0, survivers.Count);
            res = survivers[rand].transform;
        }

        return res;
    }
}
