using System;
using UnityEngine;


// Painter namespace for backwards savegame serialization compatibility.
namespace Painter
{
    /// <summary>
    /// Colour for serialization into savegames.
    /// </summary>
    [Serializable]
    public class SerializableColor
    {
        // Color components.
        public byte r;
        public byte g;
        public byte b;
        public byte a;


        /// <summary>
        /// Converts a color to serialized form.
        /// </summary>
        /// <param name="color">Color to serialize</param>
        public SerializableColor(Color32 color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        /// <summary>
        /// Deserializes a color.
        /// </summary>
        /// <param name="color">Deserialized colour</param>
        public static implicit operator Color32(SerializableColor color)
        {
            return new Color32(color.r, color.g, color.b, color.a);
        }


        /// <summary>
        /// Deserializes a color.
        /// </summary>
        /// <param name="color">Color to deserialize</param>
        public static implicit operator Color(SerializableColor color)
        {
            return new Color32(color.r, color.g, color.b, color.a);
        }

        /// <summary>
        /// Serializes a color.
        /// </summary>
        /// <param name="color">Color to serialize</param>
        public static implicit operator SerializableColor(Color32 color)
        {
            return new SerializableColor(color);
        }


        /// <summary>
        /// Serializes a color.
        /// </summary>
        /// <param name="color">Color to serialize</param>
        public static implicit operator SerializableColor(Color color)
        {
            return new SerializableColor(color);
        }
    }
}
