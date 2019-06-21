using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Color_Extentions
{
	public static bool IsEmpty(this Color c)
	{
		return c.r <= 0 && c.g <= 0 && c.b <= 0;
	}


	/// <summary>
	/// Mathf.Max(0, c.r - amount)
	/// </summary>
	public static Color Subtract(this Color c, float amount)
	{
		return new Color(
			Mathf.Max(0, c.r - amount),
			Mathf.Max(0, c.g - amount),
			Mathf.Max(0, c.b - amount));
	}
	/// <summary>
	/// Mathf.Max(0, c.r - amount)
	/// </summary>
	public static Color SubtractAlpha(this Color c, float amount)
	{
		return new Color(
			Mathf.Max(0, c.r - amount),
			Mathf.Max(0, c.g - amount),
			Mathf.Max(0, c.b - amount),
			Mathf.Max(0, c.a - amount));
	}
}

public struct ColorMagic
{
	private Color value;
	private ColorMagic(Color c)
	{
		value = c;
	}

	public static implicit operator ColorMagic(Color c)
	{
		return new ColorMagic(c);
	}
	public static implicit operator Color(ColorMagic c)
	{
		return c.value;
	}
}