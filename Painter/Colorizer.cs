using ColossalFramework;
using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repaint
{
	[XmlRoot("PainterColorizer")]
	public class PainterColorizer
	{
		[XmlIgnore]
		private static readonly string configurationPath = Path.Combine(DataLocation.localApplicationData, "PainterColorizer.xml");

		public List<string> Colorized = new List<string>();

		public List<string> Inverted = new List<string>();

		public void OnPreSerialize()
		{
		}

		public void OnPostDeserialize()
		{
		}

		public void Save()
		{
			string path = configurationPath;
			PainterColorizer colorizer = Singleton<Repaint>.instance.Colorizer;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PainterColorizer));
			using (StreamWriter textWriter = new StreamWriter(path))
			{
				colorizer.OnPreSerialize();
				xmlSerializer.Serialize(textWriter, colorizer);
			}
		}

		public static PainterColorizer Load()
		{
			string path = configurationPath;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(PainterColorizer));
			try
			{
				using (StreamReader textReader = new StreamReader(path))
				{
					return xmlSerializer.Deserialize(textReader) as PainterColorizer;
				}
			}
			catch (Exception)
			{
				return new PainterColorizer();
			}
		}
	}
}
