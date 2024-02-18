using MessagePack;
using PluginDataReader.Models;

namespace ExtensibleSaveFormat
{
    public static class ExtendedSave
    {
        /// <summary> Marker that indicates the extended save region on cards </summary>
        public const string Marker = "KKEx";
        /// <summary> Version of the extended save data on cards </summary>
        public const int DataVersion = 3;

        public static Dictionary<string, PluginData> ChaFileLoadFileHook(BlockHeader header, BinaryReader br)
        {
            BlockHeader.Info info = header.SearchInfo(Marker);

            if (info != null && info.version == DataVersion.ToString())
            {
                long originalPosition = br.BaseStream.Position;
                long basePosition = originalPosition - header.lstInfo.Sum(x => x.size);

                br.BaseStream.Position = basePosition + info.pos;

                byte[] data = br.ReadBytes((int)info.size);

                br.BaseStream.Position = originalPosition;

                try
                {
                    return MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(data);
                }
                catch (Exception)
                {
                    return new Dictionary<string, PluginData>();
                }
            }
            //else
            //{
            return new Dictionary<string, PluginData>();
            //}
        }
        public static Dictionary<string, PluginData> ChaFileCoordinateLoadHook(BinaryReader br)
        {
            try
            {
                string marker = br.ReadString();
                int version = br.ReadInt32();

                int length = br.ReadInt32();

                if (marker == Marker && version == DataVersion && length > 0)
                {
                    byte[] bytes = br.ReadBytes(length);
                    var dictionary = MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(bytes);

                    return dictionary;
                }
                else
                    return new Dictionary<string, PluginData>();
            }
            catch (EndOfStreamException)
            {
                // Incomplete/non-existant data
                return new Dictionary<string, PluginData>();
            }
            catch (InvalidOperationException)
            {
                // Invalid/unexpected deserialized data
                return new Dictionary<string, PluginData>();
            }
        }

        public static Dictionary<string, PluginData> SceneInfoLoadHook(BinaryReader br)
        {
            try
            {
                string marker = br.ReadString();
                int version = br.ReadInt32();

                int length = br.ReadInt32();

                if (marker.Equals(Marker) && length > 0)
                {
                    byte[] bytes = br.ReadBytes(length);
                    return MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(bytes);
                }
            }
            catch (EndOfStreamException)
            {
                /* Incomplete/non-existant data */
            }
            catch (InvalidOperationException)
            {
                /* Invalid/unexpected deserialized data */
            }
            return new Dictionary<string, PluginData>();
        }
    }
}
