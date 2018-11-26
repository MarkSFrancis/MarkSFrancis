﻿using System.IO;
using System.Text;

namespace Phnx.Serialization
{
    /// <summary>
    /// XML (Extensible Markup Language) serialization for transferring a value and its members to and from a <see cref="string"/>
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        /// <summary>
        /// Create a new <see cref="System.Xml.Serialization.XmlSerializer"/> for a given type
        /// </summary>
        /// <typeparam name="T">The type of value to serialize</typeparam>
        /// <returns>An XML serializer</returns>
        private System.Xml.Serialization.XmlSerializer CreateSerializer<T>()
        {
            return new System.Xml.Serialization.XmlSerializer(typeof(T));
        }

        /// <summary>
        /// Serialize an object, and append it to a stream
        /// </summary>
        /// <typeparam name="T">The type of value to serialize</typeparam>
        /// <param name="value">The value to serialize</param>
        /// <param name="outputStream">The stream to serialize the value to</param>
        public void Serialize<T>(T value, Stream outputStream)
        {
            var formatter = CreateSerializer<T>();

            formatter.Serialize(outputStream, value);
        }

        /// <summary>
        /// Serialize an object to a <see cref="string"/>
        /// </summary>
        /// <typeparam name="T">The type of value to serialize</typeparam>
        /// <param name="value">The value to serialize</param>
        /// <returns><paramref name="value"/> serialized</returns>
        public string Serialize<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                Serialize(value, stream);

                var bytes = stream.ToArray();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        /// <summary>
        /// Deserialize an object from the data in a stream
        /// </summary>
        /// <typeparam name="T">The type of value to deserialize</typeparam>
        /// <param name="inputStream">The stream to deserialize the value from</param>
        /// <returns>The value that was deserialized</returns>
        public T Deserialize<T>(Stream inputStream)
        {
            var formatter = CreateSerializer<T>();

            return (T)formatter.Deserialize(inputStream);
        }

        /// <summary>
        /// Deserialize an object from the data in a <see cref="string"/>
        /// </summary>
        /// <typeparam name="T">The type of value to deserialize</typeparam>
        /// <param name="xml">The <see cref="string"/> to deserialize the value from</param>
        /// <returns>The value that was deserialized</returns>
        public T Deserialize<T>(string xml)
        {
            var xmlAsBytes = Encoding.UTF8.GetBytes(xml);

            using (var stream = new MemoryStream(xmlAsBytes))
            {
                return Deserialize<T>(stream);
            }
        }
    }
}
