using ColossalFramework;
using ICities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Painter;

namespace Repaint
{
	public class SerializableDataExtension : SerializableDataExtensionBase
	{
		private static readonly string m_dataID = "PAINTER_COLOR_DATA";

		private Repaint Instance => Singleton<Repaint>.instance;

		private List<ColorEntry> ColorData
		{
			get
			{
				List<ColorEntry> list = new List<ColorEntry>();
				if (Instance.Colors != null)
				{
					foreach (KeyValuePair<ushort, SerializableColor> color in Instance.Colors)
					{
						list.Add(color);
					}
					return list;
				}
				return list;
			}
			set
			{
				Dictionary<ushort, SerializableColor> dictionary = new Dictionary<ushort, SerializableColor>();
				if (value != null)
				{
					foreach (ColorEntry item in value)
					{
						dictionary.Add(item.Key, item.Value);
					}
				}
				Instance.Colors = dictionary;
			}
		}

		public override void OnSaveData()
		{
			base.OnSaveData();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, ColorData);
				base.serializableDataManager.SaveData(m_dataID, memoryStream.ToArray());
			}
		}

		public override void OnLoadData()
		{
			base.OnLoadData();
			byte[] array = base.serializableDataManager.LoadData(m_dataID);
			if (array != null && array.Length != 0)
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				using (MemoryStream serializationStream = new MemoryStream(array))
				{
					ColorData = (binaryFormatter.Deserialize(serializationStream) as List<ColorEntry>);
				}
			}
		}
	}
}
