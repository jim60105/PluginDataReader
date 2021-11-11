using MessagePack;
using Sideloader.AutoResolver;

string path = (@"C:\illusion\KoikatsuSunshine\UserData\coordinate\KKS\KKCoordeF_20211024023014778.png");
var file = new ChaFileCoordinate();
Dictionary<string, PluginData>? dictionary = new Dictionary<string, PluginData>();
using (FileStream st = new FileStream(path, FileMode.Open, FileAccess.Read))
{
    using (BinaryReader binaryReader = new BinaryReader(st))
    {
        try
        {
            PngFile.SkipPng(binaryReader);
            if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position == 0L)
            {
                return;
            }
            else
            {
                var loadProductNo = binaryReader.ReadInt32();
                if (loadProductNo > 100)
                {
                    return;
                }
                else if (binaryReader.ReadString() != "【KoiKatuClothes】")
                {
                    return;
                }
                else
                {
                    var loadVersion = new Version(binaryReader.ReadString());
                    if (0 > ChaFileDefine.ChaFileClothesVersion.CompareTo(loadVersion))
                    {
                        return;
                    }
                    else
                    {
                        _ = binaryReader.ReadString();
                        int count = binaryReader.ReadInt32();
                        _ = binaryReader.ReadBytes(count);
                    }
                }
            }

            // ExtendedSave
            string marker = binaryReader.ReadString();
            int version = binaryReader.ReadInt32();

            int length = binaryReader.ReadInt32();

            if (marker == "KKEx" && version == 3 && length > 0)
            {
                byte[] bytes = binaryReader.ReadBytes(length);
                dictionary = MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(bytes);
            }
            else
                dictionary = new Dictionary<string, PluginData>(); //Overriding with empty data just in case there is some remnant from former loads.

        }
        catch (EndOfStreamException)
        {

        }
    }
}


// Sideloader mod
if (!dictionary.TryGetValue(UniversalAutoResolver.UARExtIDOld, out var extData))
{
    dictionary.TryGetValue(UniversalAutoResolver.UARExtID, out extData);
}
List<ResolveInfo> extInfo;


if (extData == null || !extData.data.ContainsKey("info"))
{
    Console.WriteLine("No sideloader marker found");
    extInfo = null;
}
else
{
    var tmpExtInfo = (object[])extData.data["info"];
    extInfo = tmpExtInfo.Select(x => ResolveInfo.Deserialize((byte[])x)).ToList<ResolveInfo>();

    Console.WriteLine($"Sideloader marker found, external info count: {extInfo.Count}");

    foreach (ResolveInfo info in extInfo)
        Console.WriteLine($"External info: {info.GUID} : {info.Property} : {info.Slot}");
}

/// <summary>
/// An object containing data saved to and loaded from cards.
/// </summary>
[MessagePackObject]
public class PluginData
{
    /// <summary>
    /// Version of the plugin data saved to the card. Get or set this if ever your plugin data format changes and use it to differentiate.
    /// </summary>
    [Key(0)]
    public int version;
    /// <summary>
    /// Dictionary of objects saved to or loaded loaded from the card.
    /// </summary>
    [Key(1)]
    public Dictionary<string, object> data = new Dictionary<string, object>();
}
/// <summary>
/// Contains information saved to the card for resolving ID conflicts
/// </summary>
[Serializable]
[MessagePackObject]
public class ResolveInfo
{
    /// <summary>
    /// GUID of the mod as defined in the manifest.xml
    /// </summary>
    [Key("ModID")]
    public string GUID { get; set; }
    /// <summary>
    /// ID of the item as defined in the mod's list files
    /// </summary>
    [Key("Slot")]
    public int Slot { get; set; }
    /// <summary>
    /// Resolved item ID. IDs greater than 100000000 are resolved IDs belonging to Sideloader. Use the resolved ID (local slot) to look up the original ID (slot)
    /// </summary>
    [Key("LocalSlot")]
    public int LocalSlot { get; set; }
    /// <summary>
    /// Property of the object as defined in Sideloader's StructReference.
    /// If ever you need to know what to use for this, enable debug resolve info logging and see what Sideloader generates at the start of the game.
    /// </summary>
    [Key("Property")]
    public string Property { get; set; }
    /// <summary>
    /// ChaListDefine.CategoryNo. Typically only used for hard mod resolving in cases where the GUID is not known.
    /// </summary>
    [Key("CategoryNo")]
    public ChaListDefine.CategoryNo CategoryNo { get; set; }

    /// <summary>
    /// Author of the mod as defined in the manifest.xml
    /// </summary>
    [Key("Author")]
    public string Author { get; set; }

    /// <summary>
    /// Website of the mod as defined in the manifest.xml
    /// </summary>
    [Key("Website")]
    public string Website { get; set; }

    /// <summary>
    /// Display name of the mod as defined in the manifest.xml
    /// </summary>
    [Key("Name")]
    public string Name { get; set; }

    internal static ResolveInfo Deserialize(byte[] data) => MessagePackSerializer.Deserialize<ResolveInfo>(data);

    internal byte[] Serialize() => MessagePackSerializer.Serialize(this);
}
