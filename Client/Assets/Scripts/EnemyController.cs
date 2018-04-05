using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject player;
	bool playerInTerritory;

	private Rigidbody2D rb2d;
	public float speed = 3f;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerInTerritory = false;
	}

	void Update()
	{
		if(playerInTerritory == true)
		{
			MoveToPlayer();
		}
		else {
			Patrol();
		}
	}

	void MoveToPlayer()
	{
		rb2d.velocity = new Vector2(3f * speed * Time.deltaTime, 0);
	}

	void Patrol()
	{

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			playerInTerritory = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.tag == "Player")
		{
			playerInTerritory = false;
		}
	}
}
