namespace PluginDataReader.FileProcessor
{
    internal static class TokenGuesser
    {
        public struct Token
        {
            //https://github.com/IllusionMods/DragAndDrop/blob/v1.2/src/DragAndDrop.Koikatu/DragAndDrop.cs#L12
            public const string StudioToken = "【KStudio】";
            public const string CharaToken = "【KoiKatuChara";
            public const string SexToken = "sex";
            public const string CoordinateToken = "【KoiKatuClothes】";
            //private const string PoseToken = "【pose】";
        }

        /// <summary>
        /// 偵測token。BinaryReader之Position必須處在pngData之後。
        /// </summary>
        /// <param name="binaryReader"></param>
        /// <returns></returns>
        public static string? GuessToken(BinaryReader binaryReader)
        {
            long position = binaryReader.BaseStream.Position;
            try
            {
                int r = binaryReader.ReadInt32();
                if (r is not 101 and not 100)
                {
                    return Token.StudioToken;
                }
                string token = binaryReader.ReadString();
                if (token.Contains(Token.CharaToken))
                {
                    // 這裡不知道角色性別，直接給1(女性)
                    // 跨性別讀取基本上夠完善，我想可以略過判別
                    return Token.CharaToken;
                }
                else if (token == Token.CoordinateToken)
                {
                    return Token.CoordinateToken;
                }
            }
            finally
            {
                binaryReader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return null;
        }
    }
}
