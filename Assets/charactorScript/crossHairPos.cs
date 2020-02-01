using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossHairPos : MonoBehaviour
{
    public void setPos(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos + Vector3.up * 0.1f;
        transform.rotation = Quaternion.Euler(90,0,0);
    }
    
   public void cancelSetPos()
    {
        
        gameObject.SetActive(false);
    }
}
