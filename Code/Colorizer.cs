using ColossalFramework;
using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


namespace Repaint
{
	/// <summary>
	/// Painter 'Colorize'/'Invert' XML settings file.
	/// </summary>
	[XmlRoot("PainterColorizer")]
	public class PainterColorizer
	{
		[XmlIgnore]
		private static readonly string configurationPath = Path.Combine(DataLocation.localApplicationData, "PainterColorizer.xml");

		// Lists of colorized and inverted prefabs.
		public List<string> Colorized = new List<string>();
		public List<string> Inverted = new List<string>();


		/// <summary>
		/// Saves the current configuration to file.
		/// </summary>
		public void Save()
		{
			string path = configurationPath;

			// Write current colorizer to file.
			PainterColorizer colorizer = Singleton<Repaint>.instance.Colorizer;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PainterColorizer));
			using (StreamWriter textWriter = new StreamWriter(path))
			{
				xmlSerializer.Serialize(textWriter, colorizer);
			}
		}


		/// <summary>
		/// Loads the configuration file.
		/// </summary>
		/// <returns></returns>
		public static PainterColorizer Load()
		{
			string path = configurationPath;

			// Read file.
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PainterColorizer));
			try
			{
				using (StreamReader textReader = new StreamReader(path))
				{
					return xmlSerializer.Deserialize(textReader) as PainterColorizer;
				}
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);

				// If failed, return a new (empty) colorizer.
				return new PainterColorizer();
			}
		}
	}
}
