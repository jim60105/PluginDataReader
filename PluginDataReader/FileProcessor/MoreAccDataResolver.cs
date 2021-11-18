using ExtensibleSaveFormat;
using Newtonsoft.Json;
using System.Xml;

namespace PluginDataReader.FileProcessor
{
    public static class MoreAccDataResolver
    {
        public const string ExtID = "moreAccessories";

        public static string ResolveMoreAccData(PluginData pluginData)
        {
            XmlNode? node = null;
            Console.WriteLine($"\nTry resolve MoreAcc v1.x extended data: {ExtID}");
            if (pluginData != null && pluginData.data.TryGetValue("additionalAccessories", out object? xmlData))
            {
                XmlDocument doc = new();
                doc.LoadXml((string)xmlData);
                node = doc.FirstChild;
            }

            string result = JsonConvert.SerializeXmlNode(node);
            Console.WriteLine(result);
            return result;
        }
    }
}
