using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	public const float MAX_HEALTH = 50;
	public float currentHealth = MAX_HEALTH;

	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Enemy is Dead !");
			gameObject.SetActive(false);
		}
	}
}
