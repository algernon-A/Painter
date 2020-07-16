using ColossalFramework;
using ICities;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Painter;


namespace Repaint
{
	/// <summary>
	/// Handles savegame data serialization.
	/// </summary>
	public class SerializableDataExtension : SerializableDataExtensionBase
	{
		// Data ID string.
		private static readonly string DataID = "PAINTER_COLOR_DATA";


		// Instance reference.
		private Repaint Instance => Singleton<Repaint>.instance;


		/// <summary>
		/// Color data list for serializing/deserializing dictionary.
		/// </summary>
		private List<ColorEntry> ColorData
		{
			get
			{
				List<ColorEntry> list = new List<ColorEntry>();

				// See if we have any custom colour settings.
				if (Instance.Colors != null)
				{
					// Iterate through each key/value pair in dictionary and add them to our list.
					foreach (KeyValuePair<ushort, SerializableColor> color in Instance.Colors)
					{
						list.Add(color);
					}

					// Done; return completed list.
					return list;
				}

				// Return empty list.
				return list;
			}
			set
			{
				// Create new disctionary.
				Dictionary<ushort, SerializableColor> dictionary = new Dictionary<ushort, SerializableColor>();

				if (value != null)
				{
					// Iterate through each item in the provided list and add to dictionary as a key/value pair.
					foreach (ColorEntry item in value)
					{
						dictionary.Add(item.Key, item.Value);
					}
				}

				// Activate the new dictionary.
				Instance.Colors = dictionary;
			}
		}


		/// <summary>
		/// Saves serialized settings data to savegame.
		/// </summary>
		public override void OnSaveData()
		{
			base.OnSaveData();

			using (MemoryStream memoryStream = new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, ColorData);
				base.serializableDataManager.SaveData(DataID, memoryStream.ToArray());
			}
		}


		/// <summary>
		/// Loads serialized settings data from savegame.
		/// </summary>
		public override void OnLoadData()
		{
			base.OnLoadData();

			// Attempt to read data.
			byte[] array = base.serializableDataManager.LoadData(DataID);

			// If we got some, deserialize it.
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
