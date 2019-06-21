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


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		var move = Input.GetAxis("Horizontal") * Power;
		rb.AddForce(Vector2.right * move * Time.fixedDeltaTime);

		Visual.rotation = Quaternion.AngleAxis(RotateVisual * move * Mathf.Deg2Rad, Vector3.back);

		var jet = Input.GetAxis("Vertical");
		if (jet > 0f)
			rb.AddForce(Vector2.up * jet * JetPower * Time.fixedDeltaTime);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		var potion = collision.gameObject.GetComponent<Potion>();
		Colorizer.MixWith(potion);

	}
}
