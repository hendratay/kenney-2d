using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : PhysicsObject {

	public Text scoreText;
	private int score;
	
	public float maxSpeed = 7f;
	public float jumpTakeOffSpeed = 7f;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();	
		animator = GetComponent<Animator>();

		//node.SubmitScore(1, 100);
		// score = 0;
		// CountScore();
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;

		move.x = Input.GetAxis("Horizontal");

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
			score += 1;
			CountScore();
		}
		if(other.tag == "Exit")
		{
			SceneManager.LoadScene("Town");
		}
	}
	
	void CountScore()
	{
		scoreText.text = "x " + score.ToString();
	}
}
