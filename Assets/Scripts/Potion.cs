using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
	public Colorizer Colorizer;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isActiveAndEnabled)
			return;

		var potion = collision.gameObject.GetComponent<Potion>();
		Colorizer.MixWith(potion);
	}
}
