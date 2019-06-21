using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : MonoBehaviour
{
	public float Rate = 0.01f;
	public Color BaseColor;

	public MeshRenderer[] Renderers;

	private Material Material;

	private float t;

	public Color CurrentColor => Color.Lerp(Color.black, BaseColor, t);

	public void Awake()
	{
		Debug.Assert(Renderers.Length > 0, "Missing mesh renderer!", this);

		Material = new Material(Renderers[0].sharedMaterial);
		Material.color = BaseColor;

		for (int i = 0; i < Renderers.Length; ++i)
			Renderers[i].material = Material;

		t = 1;
	}

	/// <summary>
	/// Destroys and sets potion as inactive, if successful.
	/// </summary>
	public bool MixWith(Potion potion)
	{
		if (!potion || !potion.isActiveAndEnabled)
			return false;

		MixWith(potion.Colorizer.CurrentColor);

		potion.gameObject.SetActive(false);
		Destroy(potion.gameObject);
		return true;
	}

	public void MixWith(Color color)
	{
		BaseColor = CurrentColor + color;
		t = 1;
	}

	public void Update()
	{
		if (t <= 0)
			return;

		t -= Rate * Time.deltaTime;
		Material.color = CurrentColor;
	}
}
