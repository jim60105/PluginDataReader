using ExtensibleSaveFormat;
using System.Text.Json;

namespace PluginDataReader.FileProcessor
{
    public static class UnknownDataResolver
    {
        public static string ResolveUnknownData(PluginData pluginData, string ExtID)
        {
            string result = JsonSerializer.Serialize(new Dictionary<string, object>());
            if (null != pluginData && pluginData.data is Dictionary<string, object> dict)
            {
                Console.WriteLine($"\nTry resolve unknown extended data: {ExtID}");

                result = JsonSerializer.Serialize(DeserializeDictionary(dict) as Dictionary<string, object>);
            }

            Console.WriteLine(result);
            return result;
        }

        private static object DeserializeDictionary(dynamic o)
        {
            if (o is not IDictionary<string, object> dict
                || dict.Count == 0
                || dict.First().Value is not byte[])
            {
                return o;
            }

            Dictionary<string, object> newDict = new();
            foreach (var kvp in dict)
            {
                object tmp = kvp.Value;
                if (kvp.Value is byte[] b
                    && b.Length > 0)
                {
                    if (b.Length > 1000000)
                    {
                        newDict.Add(kvp.Key, $"byte[{(kvp.Value as byte[])?.Length ?? 0}] (It is too big to be parsed. Usually, it may contain some graphics.)");
                        continue;
                    }
                    try
                    {
                        tmp = MessagePack.MessagePackSerializer.Deserialize<dynamic>(b);
                    }
                    catch (MessagePack.MessagePackSerializationException) { }
                }
                tmp = DeserializeDictionary(tmp);
                newDict.Add(kvp.Key, tmp);
            }

            return newDict;
        }

    }
}
