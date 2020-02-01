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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialized)
        {
            _healthBar.transform.position = transform.position + Vector3.up;
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
                // TODO: player dead
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                GetComponent<EnemyMovement>().room.EnemyDead();
            }
            Destroy(_healthBar);
        }
    }

    public void Heal(int val)
    {
        currHealth = Math.Min(maxHealth, currHealth + val);
    }
}
