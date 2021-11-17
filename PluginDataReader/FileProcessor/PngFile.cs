namespace PluginDataReader.FileProcessor
{
    public static class PngFile
    {
        public static long GetPngSize(BinaryReader br)
        {
            return GetPngSize(br.BaseStream);
        }

        public static long GetPngSize(Stream st)
        {
            if (st == null)
            {
                return 0L;
            }

            long position = st.Position;
            long num = 0L;
            try
            {
                byte[] array = new byte[8];
                byte[] array2 = new byte[8]
                {
                137,
                80,
                78,
                71,
                13,
                10,
                26,
                10
                };
                st.Read(array, 0, 8);
                for (int i = 0; i < 8; i++)
                {
                    if (array[i] != array2[i])
                    {
                        st.Seek(position, SeekOrigin.Begin);
                        return 0L;
                    }
                }

                int num2 = 0;
                bool flag = true;
                while (flag)
                {
                    byte[] array3 = new byte[4];
                    st.Read(array3, 0, 4);
                    Array.Reverse((Array)array3);
                    num2 = BitConverter.ToInt32(array3, 0);
                    byte[] array4 = new byte[4];
                    st.Read(array4, 0, 4);
                    if (BitConverter.ToInt32(array4, 0) == 1145980233)
                    {
                        flag = false;
                    }

                    if (num2 + 4 > st.Length - st.Position)
                    {
                        st.Seek(position, SeekOrigin.Begin);
                        return 0L;
                    }

                    st.Seek(num2 + 4, SeekOrigin.Current);
                }

                num = st.Position - position;
                st.Seek(position, SeekOrigin.Begin);
                return num;
            }
            catch (EndOfStreamException)
            {
                st.Seek(position, SeekOrigin.Begin);
                return 0L;
            }
        }

        public static void SkipPng(Stream st)
        {
            long pngSize = GetPngSize(st);
            st.Seek(pngSize, SeekOrigin.Current);
        }

        public static void SkipPng(BinaryReader br)
        {
            long pngSize = GetPngSize(br);
            br.BaseStream.Seek(pngSize, SeekOrigin.Current);
        }

        public static byte[] LoadPngBytes(string path)
        {
            using (FileStream st = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return LoadPngBytes(st);
            }
        }

        public static byte[] LoadPngBytes(Stream st)
        {
            using (BinaryReader br = new BinaryReader(st))
            {
                return LoadPngBytes(br);
            }
        }

        public static byte[] LoadPngBytes(BinaryReader br)
        {
            long pngSize = GetPngSize(br);
            if (pngSize == 0L)
            {
#pragma warning disable CS8603 // 可能有 Null 參考傳回。
                return null;
#pragma warning restore CS8603 // 可能有 Null 參考傳回。
            }

            return br.ReadBytes((int)pngSize);
        }
    }
}