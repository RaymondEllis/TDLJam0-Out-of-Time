using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Power;
	public float JetPower;

	public float RotateVisual;

	public Transform Visual;

	public Colorizer Colorizer;

	private Rigidbody2D rb;

	private bool faceRight;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		var c = Colorizer.CurrentColor;

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
		Colorizer.MixWith(potion);

	}
}
