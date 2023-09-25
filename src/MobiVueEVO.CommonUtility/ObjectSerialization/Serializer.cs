using Nancy.Json;
using System;

using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace MobiVUE.Utility
{
    public static class Serializer
    {
        public enum SerializedFormat
        {
            /// <summary>
            /// Binary serialization format.
            /// </summary>
            Binary,

            /// <summary>
            /// Document serialization format.
            /// </summary>
            Document
        }

        public static class Json
        {
            public static T Deserialize<T>(string jsonData) => new JavaScriptSerializer().Deserialize<T>(jsonData);

            public static object DeserializeObject(string jsonData) => new JavaScriptSerializer().DeserializeObject(jsonData);

            public static T Load<T>(string path)
            {
                if (File.Exists(path))
                    return Deserialize<T>(File.ReadAllText(path));
                return default(T);
            }

            public static void Save<T>(T serializableObject, string path)
            {
                File.WriteAllText(path, Serialize(serializableObject));
            }

            public static string Serialize<T>(T dataObject) => new JavaScriptSerializer().Serialize(dataObject);

            //public static T ConvertToType<T>(object dataObject) => new JavaScriptSerializer().ConvertToType<T>(dataObject);
        }

        public static class Xml<T> where T : class // Specify that T must be a class.
        {
            #region Public Methods

            public static T Load(string path) => LoadFromDocumentFormat(null, path, null);

            public static T Load(string path, SerializedFormat serializedFormat)
            {
                T serializableObject = null;

                switch (serializedFormat)
                {
                    case SerializedFormat.Binary:
                        serializableObject = LoadFromBinaryFormat(path, null);
                        break;

                    case SerializedFormat.Document:
                    default:
                        serializableObject = LoadFromDocumentFormat(null, path, null);
                        break;
                }

                return serializableObject;
            }

            public static T Load(string path, Type[] extraTypes) => LoadFromDocumentFormat(extraTypes, path, null);

            public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory) => LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);

            public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat)
            {
                T serializableObject = null;

                switch (serializedFormat)
                {
                    case SerializedFormat.Binary:
                        serializableObject = LoadFromBinaryFormat(fileName, isolatedStorageDirectory);
                        break;

                    case SerializedFormat.Document:
                    default:
                        serializableObject = LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
                        break;
                }

                return serializableObject;
            }

            public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, System.Type[] extraTypes) => LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);

            public static void Save(T serializableObject, string path)
            {
                SaveToDocumentFormat(serializableObject, null, path, null);
            }

            public static void Save(T serializableObject, string path, SerializedFormat serializedFormat)
            {
                switch (serializedFormat)
                {
                    case SerializedFormat.Binary:
                        SaveToBinaryFormat(serializableObject, path, null);
                        break;

                    case SerializedFormat.Document:
                    default:
                        SaveToDocumentFormat(serializableObject, null, path, null);
                        break;
                }
            }

            public static void Save(T serializableObject, string path, Type[] extraTypes)
            {
                SaveToDocumentFormat(serializableObject, extraTypes, path, null);
            }

            public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory)
            {
                SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
            }

            public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat)
            {
                switch (serializedFormat)
                {
                    case SerializedFormat.Binary:
                        SaveToBinaryFormat(serializableObject, fileName, isolatedStorageDirectory);
                        break;

                    case SerializedFormat.Document:
                    default:
                        SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
                        break;
                }
            }

            public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, System.Type[] extraTypes)
            {
                SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
            }

            #endregion Public Methods

            #region Private Methods

            private static FileStream CreateFileStream(IsolatedStorageFile isolatedStorageFolder, string path)
            {
                FileStream fileStream = null;

                if (isolatedStorageFolder == null)
                    fileStream = new FileStream(path, FileMode.OpenOrCreate);
                else
                    fileStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder);

                return fileStream;
            }

            private static TextReader CreateTextReader(IsolatedStorageFile isolatedStorageFolder, string path)
            {
                TextReader textReader = null;

                if (isolatedStorageFolder == null)
                    textReader = new StreamReader(path);
                else
                    textReader = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, isolatedStorageFolder));

                return textReader;
            }

            private static TextWriter CreateTextWriter(IsolatedStorageFile isolatedStorageFolder, string path)
            {
                TextWriter textWriter = null;

                if (isolatedStorageFolder == null)
                    textWriter = new StreamWriter(path);
                else
                    textWriter = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder));

                return textWriter;
            }

            private static XmlSerializer CreateXmlSerializer(System.Type[] extraTypes)
            {
                Type ObjectType = typeof(T);

                XmlSerializer xmlSerializer = null;

                if (extraTypes != null)
                    xmlSerializer = new XmlSerializer(ObjectType, extraTypes);
                else
                    xmlSerializer = XmlSerializer.FromTypes(new[] { ObjectType })[0]; //new XmlSerializer(ObjectType);

                return xmlSerializer;
            }

            private static T LoadFromBinaryFormat(string path, IsolatedStorageFile isolatedStorageFolder)
            {
                T serializableObject = null;

                using (FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    serializableObject = binaryFormatter.Deserialize(fileStream) as T;
                }

                return serializableObject;
            }

            private static T LoadFromDocumentFormat(System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
            {
                T serializableObject = null;

                using (TextReader textReader = CreateTextReader(isolatedStorageFolder, path))
                {
                    XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                    serializableObject = xmlSerializer.Deserialize(textReader) as T;
                }

                return serializableObject;
            }

            private static void SaveToBinaryFormat(T serializableObject, string path, IsolatedStorageFile isolatedStorageFolder)
            {
                using (FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, serializableObject);
                }
            }

            private static void SaveToDocumentFormat(T serializableObject, System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
            {
                using (TextWriter textWriter = CreateTextWriter(isolatedStorageFolder, path))
                {
                    XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                    xmlSerializer.Serialize(textWriter, serializableObject);
                }
            }

            #endregion Private Methods
        }
    }
}