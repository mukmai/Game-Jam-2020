using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bbBullet : MonoBehaviour
{
    public int damage = 30;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(damage);
            // TODO: add explosion
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // TODO: add explosion
            Destroy(gameObject);
        }
    }
}
