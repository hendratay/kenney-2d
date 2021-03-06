﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : PhysicsObject {

	public bool LocalPlayer = false;
	public Text coinText;
	private int coin;
	public Text playerName;
	
	public float maxSpeed = 7f;
	public float jumpTakeOffSpeed = 7f;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();	
		animator = GetComponent<Animator>();

		if(SceneManager.GetActiveScene () == SceneManager.GetSceneByName("Level 01"))
		{
			coin = 0;
			CountCoin();
		}
	}

	protected override void ComputeVelocity()
	{
		if(!LocalPlayer)
		{
			animator.SetBool("grounded", true);
			animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
			return;
		}

		Vector3 move = Vector2.zero;
		move.x = Input.GetAxis("Horizontal");

		// TODO : make when move when keydown
		if(SceneManager.GetActiveScene () == SceneManager.GetSceneByName("BattleGround"))
		{
			var destination = move + transform.position;
			Network.Move(transform.position, destination);
		}

		if(Input.GetKeyDown(KeyCode.F))
		{
			Fire();	
			if(SceneManager.GetActiveScene () == SceneManager.GetSceneByName("BattleGround"))
			{
				Network.Shoot(transform.position);
			}
		}

		if(Input.GetButtonDown("Jump") && grounded)
		{
			velocity.y = jumpTakeOffSpeed;
		}
		else if(Input.GetButtonUp("Jump"))
		{
			if(velocity.y > 0)
				velocity.y = velocity.y * .5f;
		}

		animator.SetBool("grounded", grounded);
		animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

		bool flipSprite = spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f);
		if(flipSprite)
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}

		targetVelocity = move * maxSpeed;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "CoinBronze")
		{
			other.gameObject.SetActive(false);
			coin += 1;
			CountCoin();
		}
		if(other.tag == "Exit")
		{
			SceneManager.LoadScene("Town");
		}
		if(other.tag == "Exit Town")
		{
			SceneManager.LoadScene("Level 01");
		}
		if(other.tag == "Water")
		{
			PlayerHealth ph = GetComponent<PlayerHealth>();
			ph.TakeDamage(100);
		}
	}

	public void Fire()
	{
		GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
		Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
		rigidbody.velocity = new Vector3(6f, 0f, 0f);
		Destroy(bullet, 2f);
	}

	void CountCoin()
	{
		coinText.text = coin.ToString();
	}
}