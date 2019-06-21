using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float Power;
	public float JetPower;

	public float Death = 0.1f;

	public float RotateVisual;

	public UIScore UIScore;

	public Transform Visual;

	public Colorizer Colorizer;

	private Rigidbody2D rb;

	private bool faceRight = true;

	private double timmer;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		Debug.Assert(UIScore != null);
		Debug.Assert(Colorizer != null);
	}

	private void Update()
	{
		timmer += Time.deltaTime;
	}

	void FixedUpdate()
	{
		var c = Colorizer.CurrentColor;

		if (c.b < Death || c.g < Death)
		{
			// ToDo : Proper death screen
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		transform.localScale = Vector3.one * c.b;

		var move = Input.GetAxis("Horizontal");
		rb.AddForce(c.g * move * Power * Vector2.right * Time.fixedDeltaTime);

		if (move != 0f)
			faceRight = move > 0f;

		Visual.rotation = Quaternion.AngleAxis(RotateVisual * move, Vector3.back);
		Visual.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);

		var jet = Input.GetAxis("Vertical");
		if (jet > 0f)
			rb.AddForce(c.g * jet * JetPower * Vector2.up * Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var potion = collision.gameObject.GetComponent<Potion>();
		if (Colorizer.MixWith(potion))
			return;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		var exit = collision.gameObject.GetComponent<Exit>();
		if (exit)
		{
			gameObject.SetActive(false);
			UIScore.Show(timmer);
		}
	}
}
