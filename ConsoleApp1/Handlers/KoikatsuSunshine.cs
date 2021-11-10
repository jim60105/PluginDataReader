using ConsoleApp1.FileProcessor;
using ConsoleApp1.Models;
using ExtensibleSaveFormat;
using MessagePack;
using System.Text;

namespace ConsoleApp1.Handlers
{
    internal class KoikatsuSunshine
    {
        public static readonly Version ChaFileVersion = new("0.0.0");
        public static readonly Version ChaFileClothesVersion = new("0.0.2");

        public static Dictionary<string, PluginData>? Chara(BinaryReader binaryReader)
        {
			try
			{
				var loadProductNo = binaryReader.ReadInt32();
				if (loadProductNo > 100)
				{
					return null;
				}
				if (binaryReader.ReadString() != "【KoiKatuCharaSun】")
				{
					return null;
				}
				 var loadVersion = new Version(binaryReader.ReadString());
				if (0 > ChaFileVersion.CompareTo(loadVersion))
				{
					return null;
				}
				int num = binaryReader.ReadInt32();
				if (num != 0)
				{
					_ = binaryReader.ReadBytes(num);
				}
				int count = binaryReader.ReadInt32();
				BlockHeader blockHeader = MessagePackSerializer.Deserialize<BlockHeader>(binaryReader.ReadBytes(count));
				long num2 = binaryReader.ReadInt64();
				long position = binaryReader.BaseStream.Position;
                binaryReader.BaseStream.Seek(position + num2, SeekOrigin.Begin);

                var result = ExtendedSave.ChaFileLoadFileHook(blockHeader, binaryReader);
			
                return result;
			}
			catch (EndOfStreamException)
			{
				return null;
			}
		}

        public static Dictionary<string, PluginData>? Coordinate(BinaryReader binaryReader)
        {
            try
            {
                if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position == 0L)
                {
                    return null;
                }
                else
                {
                    var loadProductNo = binaryReader.ReadInt32();
                    if (loadProductNo > 100)
                    {
                        return null;
                    }
                    else if (binaryReader.ReadString() != "【KoiKatuClothes】")
                    {
                        return null;
                    }
                    else
                    {
                        var loadVersion = new Version(binaryReader.ReadString());
                        if (0 > ChaFileClothesVersion.CompareTo(loadVersion))
                        {
                            return null;
                        }
                        else
                        {
                            _ = binaryReader.ReadString();
                            int count = binaryReader.ReadInt32();
                            _ = binaryReader.ReadBytes(count);
                        }
                    }
                }

                return ExtendedSave.ChaFileCoordinateLoadHook(binaryReader);
            }
            catch (EndOfStreamException)
            {
                return null;
            }
        }

        public static Dictionary<string, PluginData>? Studio(BinaryReader binaryReader)
        {
            try
            {
                byte[] toCompareArray = Encoding.UTF8.GetBytes(TokenGuesser.Token.StudioToken);
                for (long i = binaryReader.BaseStream.Position; i < binaryReader.BaseStream.Length; i++)
                {
                    long pos = binaryReader.BaseStream.Position;
                    if (binaryReader.ReadByte() != toCompareArray[0]
                        || binaryReader.ReadByte() != toCompareArray[1]
                        || binaryReader.ReadByte() != toCompareArray[2])
                    {
                        continue;
                    }

                    binaryReader.BaseStream.Seek(pos, SeekOrigin.Begin);
                    byte[] getString = binaryReader.ReadBytes(toCompareArray.Length);
                    if (!getString.SequenceEqual(toCompareArray))
                    {
                        binaryReader.BaseStream.Seek(pos + 1, SeekOrigin.Begin);
                        i = pos; // for safe
                    }
                    else
                    {
                        return ExtendedSave.SceneInfoLoadHook(binaryReader);
                    }
                }
            }
            catch (EndOfStreamException)
            {
            }
            return null;
        }
    }
}
