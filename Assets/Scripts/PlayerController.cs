using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Power;
	public float JetPower;

	private Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		var move = Input.GetAxis("Horizontal") * Power;
		rb.AddForce(Vector2.right * move * Time.fixedDeltaTime);


		var jet = Input.GetAxis("Vertical");
		if (jet > 0f)
			rb.AddForce(Vector2.up * jet * JetPower * Time.fixedDeltaTime);
	}
}
