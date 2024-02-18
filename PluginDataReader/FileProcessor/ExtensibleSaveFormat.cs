using MessagePack;
using PluginDataReader.Models;

namespace ExtensibleSaveFormat
{
    public static class ExtendedSave
    {
        /// <summary> Marker that indicates the extended save region on cards </summary>
        public const string marker = "KKEx";
        /// <summary> Version of the extended save data on cards </summary>
        public const int dataVersion = 3;

        public static Dictionary<string, PluginData> ChaFileLoadFileHook(BlockHeader header, BinaryReader br)
        {
            BlockHeader.Info info = header.SearchInfo(marker);

            if (info != null && info.version == dataVersion.ToString())
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
                    return [];
                }
            }
            //else
            //{
            return [];
            //}
        }
        public static Dictionary<string, PluginData> ChaFileCoordinateLoadHook(BinaryReader br)
        {
            try
            {
                string marker = br.ReadString();
                int version = br.ReadInt32();

                int length = br.ReadInt32();

                if (marker == ExtendedSave.marker && version == dataVersion && length > 0)
                {
                    byte[] bytes = br.ReadBytes(length);
                    var dictionary = MessagePackSerializer.Deserialize<Dictionary<string, PluginData>>(bytes);

                    return dictionary;
                }
                else
                    return [];
            }
            catch (EndOfStreamException)
            {
                // Incomplete/non-existant data
                return [];
            }
            catch (InvalidOperationException)
            {
                // Invalid/unexpected deserialized data
                return [];
            }
        }

        public static Dictionary<string, PluginData> SceneInfoLoadHook(BinaryReader br)
        {
            try
            {
                string marker = br.ReadString();
                int version = br.ReadInt32();

                int length = br.ReadInt32();

                if (marker.Equals(ExtendedSave.marker) && length > 0)
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
            return [];
        }
    }
}
