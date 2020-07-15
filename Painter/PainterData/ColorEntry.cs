using System;
using System.Collections.Generic;


// Painter namespace for backwards savegame serialization compatibility.
namespace Painter
{
    /// <summary>
    /// Colour entry for building setting color dictionary serialization into savegames.
    /// </summary>
    [Serializable]
    public class ColorEntry
    {
        // Key/value pair.
        public ushort Key;
        public SerializableColor Value;


        /// <summary>
        /// Converts a key/value pair to serializable form.
        /// </summary>
        /// <param name="key">Key to serialize</param>
        /// <param name="value">Value to serialize</param>
        public ColorEntry(ushort key, SerializableColor value)
        {
            Key = key;
            Value = value;
        }


        /// <summary>
        /// Serializes a ColorEntry key/value pair.
        /// </summary>
        /// <param name="keyValuePair">Key/value pair to serialize</param>
        public static implicit operator ColorEntry(KeyValuePair<ushort, SerializableColor> keyValuePair)
        {
            return new ColorEntry(keyValuePair.Key, keyValuePair.Value);
        }


        /// <summary>
        /// Deserializes a ColorEntry key/value pair.
        /// </summary>
        /// <param name="entry">ColourEntry to deserialize</param>
        public static implicit operator KeyValuePair<ushort, SerializableColor>(ColorEntry entry)
        {
            return new KeyValuePair<ushort, SerializableColor>(entry.Key, entry.Value);
        }
    }
}
