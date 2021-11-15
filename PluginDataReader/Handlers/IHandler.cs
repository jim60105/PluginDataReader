using ExtensibleSaveFormat;

namespace PluginDataReader.Handlers
{
    internal interface IHandler
    {
        /// <summary>
        /// 本工具支援的角色存檔最高版本
        /// </summary>
        Version ChaFileVersion { get; }

        /// <summary>
        /// 本工具支援的服裝存檔最高版本
        /// </summary>
        Version ChaFileClothesVersion { get; }

        public  Dictionary<string, PluginData>? Chara(BinaryReader binaryReader);
        public  Dictionary<string, PluginData>? Coordinate(BinaryReader binaryReader);
        public  Dictionary<string, PluginData>? Studio(BinaryReader binaryReader);
    }
}