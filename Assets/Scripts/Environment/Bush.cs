using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{

    private SpriteRenderer _sprite;
    private ParticleSystem _particles;

	void Start ()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _particles = GetComponent<ParticleSystem>();   
    }
	
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Weapon")
        {
            _sprite.enabled = false;
            _particles.Play();

            Destroy(gameObject, 1f);
        }
    }
}
