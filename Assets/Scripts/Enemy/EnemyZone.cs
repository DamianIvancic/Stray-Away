using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour {


    private BoxCollider2D _returnTrigger;
    private List<Crawler> _enemies;

 
    void Start()
    {
        _returnTrigger = GetComponent<BoxCollider2D>();
        _enemies = new List<Crawler>(GetComponentsInChildren<Crawler>());

    }

    void OnDestroy()
    {
        //EnemyScript.OnDeath.gameObjectCallback -= OnDeathListener;
    }
	
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            foreach (Crawler enemy in _enemies)
            {
                if (enemy.gameObject == collider.gameObject)
                {
                    //enemy.Return();
                }
            }
        }
    }

    void OnDeathListener(GameObject died)
    {
        foreach (Crawler enemy in _enemies)
        {
            if (enemy.gameObject == died)
            {
                _enemies.Remove(enemy);
                break;
            }
        }
    }

    public void TestListener()
    {
        Debug.Log("testListener");
    }
}
