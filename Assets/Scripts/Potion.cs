using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
	public MeshRenderer Renderer;

	public Color Color;

	private Material Material;

	private float t;

	public Color CurrentColor => Color.Lerp(Color.black, Color, t);

	public void Awake()
	{
		Material = new Material(Renderer.sharedMaterial);
		Material.color = Color;
		Renderer.material = Material;

		t = 1;
	}

	public void Update()
	{
		if (t <= 0)
			return;

		t -= 0.01f * Time.deltaTime;
		Material.color = CurrentColor;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isActiveAndEnabled)
			return;

		var potion = collision.gameObject.GetComponent<Potion>();
		if (potion && potion.isActiveAndEnabled)
		{
			var a = CurrentColor;
			var b = potion.CurrentColor;
			var mix = a + b;
			Color = mix;
			t = 1;
			collision.gameObject.SetActive(false);
			Destroy(collision.gameObject);
			return;
		}
	}
}
