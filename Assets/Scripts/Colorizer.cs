using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Colorizer : MonoBehaviour
{
	public float Rate = 0.01f;
	public Color BaseColor;

	public MeshRenderer[] Renderers;
	public Color CurrentColor => BaseColor;

	private Material Material;

	private Queue<Color> absorbQueue;
	private Color absorb;


	public void Awake()
	{
		Debug.Assert(Renderers.Length > 0, "Missing mesh renderer!", this);


		Material = new Material(Renderers[0].sharedMaterial);
		Material.color = BaseColor;

		for (int i = 0; i < Renderers.Length; ++i)
			Renderers[i].material = Material;

		absorbQueue = new Queue<Color>();
		absorb = Color.black;
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
		absorbQueue.Enqueue(color);
	}

	public void Update()
	{
#if UNITY_EDITOR
		if (!UnityEditor.EditorApplication.isPlaying)
			return;
#endif

		if (absorb.IsEmpty() && absorbQueue.Count > 0)
			absorb = absorbQueue.Dequeue();

		if (!absorb.IsEmpty())
		{
			var n = absorb.Subtract(0.5f * Time.deltaTime);
			BaseColor += absorb - n;
			absorb = n;
		}

		BaseColor = BaseColor.Subtract(Rate * Time.deltaTime);
		Material.color = BaseColor;
	}

}
