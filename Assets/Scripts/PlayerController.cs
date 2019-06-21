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

	[Header("Grab")]
	public SpringJoint2D GrabJoint;
	public float GrabDistance;
	public ContactFilter2D GrabContactFilter;
	public LineRenderer GrabLine;
	private Rigidbody2D Grabbed;

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

		if (Input.GetButtonDown("Jump"))
		{
			// Release
			if (Grabbed)
			{
				Debug.Log("Released");
				Grabbed = null;
				GrabJoint.enabled = false;
				GrabJoint.connectedBody = null;
				GrabLine.enabled = false;
			}
			// Grab
			else
			{
				var direction = faceRight ? Vector2.right : Vector2.left;
				var hits = new RaycastHit2D[4];
				var count = Physics2D.Raycast(transform.position, direction, GrabContactFilter, hits, GrabDistance);
				for (int i = 0; i < count; ++i)
				{
					var hit = hits[i];
					Debug.Log($"Grabbed {hit.transform.gameObject.name}");
					if (hit && hit.rigidbody != rb)
					{
						GrabJoint.autoConfigureDistance = true;
						GrabJoint.connectedBody = hit.rigidbody;
						GrabJoint.enabled = true;
						GrabJoint.autoConfigureDistance = false;
						Grabbed = hit.rigidbody;

						GrabLine.enabled = true;
						UpdateLinePositions();
						break;
					}
				}
				if (!Grabbed)
					Debug.Log($"Grab failed, #{count} hits.");
			}
		}

		var c = Colorizer.CurrentColor;

		if (c.b < Death || c.g < Death)
		{
			// ToDo : Proper death screen
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		transform.localScale = Vector3.one * c.b;


		var move = Input.GetAxis("Horizontal");
		if (move != 0f)
			faceRight = move > 0f;
		Visual.rotation = Quaternion.AngleAxis(RotateVisual * move, Vector3.back);
		Visual.localScale = new Vector3(faceRight ? 1f : -1f, 1f, 1f);
	}

	void FixedUpdate()
	{
		var c = Colorizer.CurrentColor;

		var move = Input.GetAxis("Horizontal");
		rb.AddForce(c.g * move * Power * Vector2.right * Time.fixedDeltaTime);


		var jet = Input.GetAxis("Vertical");
		if (jet > 0f)
			rb.AddForce(c.g * jet * JetPower * Vector2.up * Time.fixedDeltaTime);

		UpdateLinePositions();
	}

	private void UpdateLinePositions()
	{
		if (!Grabbed)
		{
			if (GrabLine.enabled)
				GrabLine.enabled = false;
			return;
		}
		GrabLine.SetPositions(new Vector3[] { transform.position, Grabbed.transform.position });
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
