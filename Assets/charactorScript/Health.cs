using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _initialized = false;
    [SerializeField] private GameObject healthBarPrefab;
    private GameObject _healthBar;
    public int maxHealth = 100;
    public int currHealth;
    private Image _bar;

    private void OnEnable()
    {
        if (!_initialized)
        {
            _initialized = true;
            _healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up, Quaternion.identity);
            currHealth = maxHealth;
            _bar = _healthBar.GetComponentsInChildren<Image>()[1];
            _healthBar.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialized)
        {
            _healthBar.transform.position = transform.position + Vector3.up;
            _healthBar.transform.rotation = Quaternion.identity;
        }
    }

    public void TakeDamage(int val)
    {
        currHealth = Math.Max(0, currHealth - val);
        _bar.fillAmount = (float) currHealth / maxHealth;
        if (currHealth == 0)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (gameObject.name == "player")
                {
                    // TODO: game over
                }
                else
                {
                    // shut down robot
                }
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                RoomManager room = GetComponent<EnemyMovement>().room;
                if (room)
                    room.EnemyDead();
                Destroy(gameObject);
            }
        }
    }

    public void Heal(int val)
    {
        currHealth = Math.Min(maxHealth, currHealth + val);
    }
}
