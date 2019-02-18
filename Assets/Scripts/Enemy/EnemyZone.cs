using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour {


    private BoxCollider2D _returnTrigger;
    private List<EnemyScript> _enemies;

    void Start()
    {
        _returnTrigger = GetComponent<BoxCollider2D>();
        _enemies = new List<EnemyScript>(GetComponentsInChildren<EnemyScript>());

        EnemyScript.OnDeathCallback += OnDeathListener;
    }

    void OnDestroy()
    {
        EnemyScript.OnDeathCallback -= OnDeathListener;
    }
	
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            foreach (EnemyScript enemy in _enemies)
            {
                if (enemy.gameObject == collider.gameObject)
                {
                    enemy.Return();
                }
            }
        }
    }

    void OnDeathListener(GameObject died)
    {
        foreach (EnemyScript enemy in _enemies)
        {
            if (enemy.gameObject == died)
            {
                _enemies.Remove(enemy);
                break;
            }
        }
    }
}
