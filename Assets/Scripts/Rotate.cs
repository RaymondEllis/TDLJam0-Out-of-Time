using UnityEngine;

public class Rotate : MonoBehaviour
{
	public float Speed;

	private void FixedUpdate()
	{
		transform.Rotate(Vector3.forward, Speed * Time.fixedDeltaTime);
	}
}
