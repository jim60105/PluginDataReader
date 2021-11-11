using ExtensibleSaveFormat;

namespace Sideloader.AutoResolver
{
    /// <summary>
    /// Automatically resolves ID conflicts by saving GUID to the card and changing item IDs at runtime
    /// </summary>
    public static class UniversalAutoResolver
    {
        /// <summary>
        /// Extended save ID
        /// </summary>
        public const string UARExtID = "com.bepis.sideloader.universalautoresolver";
        /// <summary>
        /// Extended save ID used in EmotionCreators once upon a time, no longer used but must still be checked for cards that still use it
        /// </summary>
        public const string UARExtIDOld = "EC.Core.Sideloader.UniversalAutoResolver";

        public static (string, string, int)[]? ResolveSideloaderData(Dictionary<string, PluginData> dictionary)
        {
            if (!dictionary.TryGetValue(UARExtIDOld, out var extData))
            {
                _ = dictionary.TryGetValue(UARExtID, out extData);
            }

            if (extData != null && extData.data.ContainsKey("info"))
            {
                Console.WriteLine("\nFound sideloader mod data:");
                var tmpExtInfo = (object[])extData.data["info"];
                List<ResolveInfo> extInfo = tmpExtInfo.Select(x => ResolveInfo.Deserialize((byte[])x)).ToList<ResolveInfo>();

                Console.WriteLine($"Sideloader marker found, external info count: {extInfo.Count}");

                foreach (ResolveInfo info in extInfo)
                {
                    Console.WriteLine($"External info: {info.GUID} : {info.Property} : {info.Slot}");
                }
                return extInfo.Select(p => (p.GUID, p.Property, p.Slot)).ToArray();
            }

            return null;
        }
    }
}
