using System;
using UnityEngine;

namespace Repaint
{
	public static class Extensions
	{
		public static Texture2D MakeReadable(this Texture texture)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0);
			Graphics.Blit(texture, temporary);
			Texture2D result = temporary.ToTexture2D();
			RenderTexture.ReleaseTemporary(temporary);
			return result;
		}

		public static Texture2D ToTexture2D(this RenderTexture rt)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = rt;
			Texture2D texture2D = new Texture2D(rt.width, rt.height);
			texture2D.ReadPixels(new Rect(0f, 0f, rt.width, rt.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			return texture2D;
		}

		public static void UpdateACI(this Material material, bool invert)
		{
			try
			{
				Texture2D texture2D = material.GetTexture("_ACIMap").MakeReadable();
				Texture2D texture2D2 = material.GetTexture("_XYSMap").MakeReadable();
				Color[] pixels = texture2D.GetPixels();
				Color[] pixels2 = texture2D2.GetPixels();
				if (invert)
				{
					for (int i = 0; i < pixels.Length; i++)
					{
						float num = Mathf.GammaToLinearSpace(pixels[i].g);
						float num2 = Mathf.GammaToLinearSpace(pixels2[i].b);
						float g = 1f - Mathf.LinearToGammaSpace(num * num2);
						pixels[i] = new Color(pixels[i].r, g, pixels[i].b);
					}
				}
				else
				{
					for (int j = 0; j < pixels.Length; j++)
					{
						float g2 = Mathf.LinearToGammaSpace(1f - Mathf.GammaToLinearSpace(pixels2[j].b));
						pixels[j] = new Color(pixels[j].r, g2, pixels[j].b);
					}
				}
				Texture2D texture2D3 = new Texture2D(texture2D.width, texture2D.height, texture2D.format, mipmap: true);
				texture2D3.SetPixels(pixels);
				texture2D3.Apply();
				texture2D3.Compress(highQuality: true);
				material.SetTexture("_ACIMap", texture2D3);
				UnityEngine.Object.Destroy(texture2D);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}
	}
}
