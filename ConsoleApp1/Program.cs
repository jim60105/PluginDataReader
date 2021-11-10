using ConsoleApp1.FileProcessor;
using ConsoleApp1.Handlers;
using ExtensibleSaveFormat;
using Sideloader.AutoResolver;
using static ConsoleApp1.FileProcessor.TokenGuesser;

string path = (@"C:\illusion\KoikatsuSunshine\UserData\coordinate\KKS\KKCoordeF_20211024023014778.png");
//string path = (@"C:\illusion\KoikatsuSunshine\UserData\chara\female\KoikatsuSun_F_20211029005453689_来栖 捺月.png");
//string path = (@"C:\illusion\KoikatsuSunshine\UserData\studio\scene\2021_1110_1011_01_139.png");
Dictionary<string, PluginData>? dictionary = null;

using (FileStream fileStream = new(path, FileMode.Open, FileAccess.Read))
using (BinaryReader binaryReader = new(fileStream))
{
    PngFile.SkipPng(binaryReader);
    switch (TokenGuesser.GuessToken(binaryReader))
    {
        case Token.CoordinateToken:
            dictionary = KoikatsuSunshine.Coordinate(binaryReader);
            break;
        case Token.CharaToken:
            dictionary = KoikatsuSunshine.Chara(binaryReader);
            break;
        case Token.StudioToken:
            dictionary = KoikatsuSunshine.Studio(binaryReader);
            break;
        case null:
        default:
            Console.WriteLine("Cannot detect the file.");
            break;
    }

    if (null != dictionary)
    {
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"Found PluginData {kvp.Key}");
        }
        UniversalAutoResolver.ResolveSideloaderData(dictionary);
    }
    else
    {
        Console.WriteLine("No Extended Data found");
    }
}


