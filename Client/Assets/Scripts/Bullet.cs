using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            var health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }

    void onCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            var health = collision.gameObject.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}