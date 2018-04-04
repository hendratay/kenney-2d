using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public Image healthBar;
	public const float MAX_HEALTH = 100;
	public float currentHealth = MAX_HEALTH;

	public void TakeDamage(float amount)
	{
		currentHealth -= amount;
		healthBar.fillAmount = currentHealth / MAX_HEALTH;
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			Debug.Log("Player is Dead !");
		}
	}
}
