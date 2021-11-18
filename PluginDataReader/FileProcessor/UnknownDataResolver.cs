using ExtensibleSaveFormat;
using Newtonsoft.Json;
using System.Collections;
using System.Xml;

namespace PluginDataReader.FileProcessor
{
    public static class UnknownDataResolver
    {
        public static string ResolveUnknownData(PluginData pluginData, string ExtID)
        {
            string result = JsonConvert.SerializeObject(new Dictionary<string, object>());
            if (null != pluginData && pluginData.data is Dictionary<string, object> dict)
            {
                Console.WriteLine($"\nTry resolve unknown extended data: {ExtID}");

                result = JsonConvert.SerializeObject(DeserializeDataRecursively(dict));
            }

            Console.WriteLine(result);
            return result;
        }

        internal static object? DeserializeDataRecursively(dynamic? o)
        {
            switch (o)
                {
                case byte[] b when b.Length == 0:
                    return $"byte[0]";

                case byte[] b:
                    try
                    {
                        return DeserializeDataRecursively(MessagePack.MessagePackSerializer.Deserialize<dynamic>(b));
                    }
                    catch (MessagePack.MessagePackSerializationException)
                    {
                        return $"byte[{b.Length}]";
                    }

                case string s when s.Length > 500:
                    return $"string[{s.Length}] (Too big to display)";

                case IDictionary dict when dict.Count != 0:
                    Dictionary<object, object?> newDict = new();
                    foreach (DictionaryEntry kvp in dict)
                    {
                        object? tmp = kvp.Value;
                        tmp = DeserializeDataRecursively(tmp);
                        newDict.Add(kvp.Key, tmp);
                    }

                    return newDict;

                case object[] oArray when oArray.Length != 0:
                    List<object?> list = new();
                    foreach (object _o in oArray)
                    {
                        list.Add(DeserializeDataRecursively(_o));
                    }

                    return list.ToArray();

                case XmlElement xmlElement:
                    return JsonConvert.DeserializeObject<dynamic>((string)JsonConvert.SerializeXmlNode(xmlElement));
                default:
                    return o;
            }
        }
    }
}
