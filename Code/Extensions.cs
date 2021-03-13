using System;
using UnityEngine;


namespace Repaint
{
	/// <summary>
	/// Extensions for colorizer functionality.
	/// </summary>
	internal static class Extensions
	{
		/// <summary>
		/// Colorizes/inverts a material's ACI.
		/// </summary>
		/// <param name="material">Material to convert</param>
		/// <param name="invert">True if inverting, false otherwise</param>
		internal static void UpdateACI(this Material material, bool invert)
		{
			try
			{
				// Convert material textures to 2D.
				Texture2D texture2dACI = material.GetTexture("_ACIMap")?.MakeReadable();
				Color[] pixelsACI = texture2dACI?.GetPixels();

				// Null check in case things failed.
				if (pixelsACI == null)
                {
					Logging.Message("couldn't get readable ACIMap");
					return;
                }

				Color[] pixelsXYS = material.GetTexture("_XYSMap")?.MakeReadable()?.GetPixels();

				// Null check in case things failed again.
				if (pixelsXYS == null)
                {
					Logging.Message("couldn't get readable XYS map");
					return;
                }

				// Invert colormap.
				if (invert)
				{
					// Iterate through each pixel and invet
					for (int i = 0; i < pixelsACI.Length; i++)
					{
						// Convert gamma values to linear and invert.
						float linearACT = Mathf.GammaToLinearSpace(pixelsACI[i].g);
						float linearXYS = Mathf.GammaToLinearSpace(pixelsXYS[i].b);
						float g = 1f - Mathf.LinearToGammaSpace(linearACT * linearXYS);

						// Apply our new color.
						pixelsACI[i] = new Color(pixelsACI[i].r, g, pixelsACI[i].b);
					}
				}
				else
				{
					// Non-inverted colorization - iterate through each pixel and apply.
					for (int j = 0; j < pixelsACI.Length; j++)
					{
						// Convert gamma values to linear and apply new color.
						float g2 = Mathf.LinearToGammaSpace(1f - Mathf.GammaToLinearSpace(pixelsXYS[j].b));
						pixelsACI[j] = new Color(pixelsACI[j].r, g2, pixelsACI[j].b);
					}
				}

				// Create new 2D texture from our result and apply to prefab's ACI map.
				Texture2D newTexture = new Texture2D(texture2dACI.width, texture2dACI.height, texture2dACI.format, mipmap: true);
				newTexture.SetPixels(pixelsACI);
				newTexture.Apply();
				newTexture.Compress(highQuality: true);
				material.SetTexture("_ACIMap", newTexture);

				// Destroy our temporary texture.
				UnityEngine.Object.Destroy(texture2dACI);
			}
			catch (Exception e)
			{
				// Don't let a failure stop us.
				Logging.LogException(e, "exception updating ACI");
			}
		}


		/// <summary>
		/// Converts a texture to Texture2D.
		/// </summary>
		/// <param name="texture">Texture to convert</param>
		/// <returns>Converted 2D texture</returns>
		private static Texture2D MakeReadable(this Texture texture)
		{
			// Null check.
			if (texture == null)
            {
				return null;
            }

			// Create new temporary texture from given texture.
			RenderTexture temporary = RenderTexture.GetTemporary(texture.width, texture.height, 0);

			// Null check in case things failed.
			if (temporary == null)
			{
				return null;
			}

			Graphics.Blit(texture, temporary);

			// Convert to 2D texture and release temporary texture.
			Texture2D result = temporary.ToTexture2D();
			RenderTexture.ReleaseTemporary(temporary);

			return result;
		}

		/// <summary>
		/// Convert a RenderTexture to 2D texture.
		/// </summary>
		/// <param name="renderTexture">RenderTexture to convert</param>
		/// <returns>Converted 2D texture</returns>
		private static Texture2D ToTexture2D(this RenderTexture renderTexture)
		{
			// Backup currently active texture.
			RenderTexture activeTexture = RenderTexture.active;

			// Set active render texture to current texture and copy to new 2D texture.
			RenderTexture.active = renderTexture;
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height);
			texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
			texture2D.Apply();

			// Restore previously active texture.
			RenderTexture.active = activeTexture;

			return texture2D;
		}
	}
}
