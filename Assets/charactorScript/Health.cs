using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public float yOffset = 1;

    private Animator _animator;

    private void OnEnable()
    {
        if (!_initialized)
        {
            _initialized = true;
            _healthBar = Instantiate(healthBarPrefab, transform.position + Vector3.up, Quaternion.identity);
            currHealth = maxHealth;
            _bar = _healthBar.GetComponentsInChildren<Image>()[1];
            _healthBar.transform.SetParent(transform);

            _animator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialized)
        {
            _healthBar.transform.position = transform.position + Vector3.up * yOffset;
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
                    _animator.SetBool("isDead",true);
                    GameObject obj = GameObject.Find("/LevelManager");
                    obj.GetComponent<ChangeScene>().showLoseUI();
                    Debug.Log("fuund");
                }
                else
                {
                    // shut down robot
                    _animator.SetBool("isDead",true);
                    _healthBar.SetActive(false);
                    gameObject.GetComponent<RobotController>().Break();
                }
            }
            else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                RoomManager room = GetComponent<EnemyMovement>().room;
                if (room)
                    room.EnemyDead();
                gameObject.GetComponent<DeathDrop>().Die();
            }
        }
    }

    public void Heal(int val)
    {
        currHealth = Math.Min(maxHealth, currHealth + val);
        _bar.fillAmount = (float) currHealth / maxHealth;
    }

    public float SendHp()
    {
        return (float)currHealth / maxHealth;
    }
}
