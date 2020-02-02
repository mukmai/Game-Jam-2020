using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public Health player;

    public Health[] robots;
    public int boltCount = 0;
    private List<Health> survivers = new List<Health>();

    public Text boltText;

    public GameObject tvPrefab;
    
    
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

        boltText.text = ": " + boltCount;
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

    public void WinGame()
    {
        // TODO: do shit here
        
        StartCoroutine(GenTvCoroutine());
        
        
        Debug.Log("WOW");
    }
    
    IEnumerator GenTvCoroutine()
    {
        for(int i = 0; i< 50;i++)
        {
            Instantiate(tvPrefab, player.transform.position + Vector3.up*10, Quaternion.Euler(0,Random.Range(0,360),0));
            
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }
    

}
