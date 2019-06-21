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
}
