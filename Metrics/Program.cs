using System;
using System.IO;
using SharpDX.DirectWrite;

class CodeAnalyzer
{
    static void Main(string[] args)
    {
        string path = @"D:\Work\DotNet\DirectECS";
        int classCount = 0;
        int structCount = 0;
        int lineCount = 0;
        int fileCount = 0;

        foreach (string file in Directory.EnumerateFiles(path, "*.cs", SearchOption.AllDirectories))
        {
            fileCount++;
            bool inMultilineComment = false;

            foreach (string line in File.ReadAllLines(file))
            {
                string trimmed = line.Trim();

                // Пропускаем пустые строки и комментарии
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    continue;
                }

                if (trimmed.StartsWith("//")) continue;

                // Обработка многострочных комментариев
                if (trimmed.StartsWith("/*"))
                {
                    inMultilineComment = true;
                }
                if (inMultilineComment)
                {
                    if (trimmed.EndsWith("*/"))
                    {
                        inMultilineComment = false;
                    }
                    continue;
                }

                lineCount++;

                // Подсчет классов и структур
                if (trimmed.StartsWith("public class ") || trimmed.StartsWith("class ") ||
                    trimmed.StartsWith("internal class ") || trimmed.StartsWith("private class "))
                {
                    classCount++;
                }
                else if (trimmed.StartsWith("public struct ") || trimmed.StartsWith("struct ") ||
                         trimmed.StartsWith("internal struct ") || trimmed.StartsWith("private struct "))
                {
                    structCount++;
                }
            }
        }

        Console.WriteLine($"Проанализировано файлов: {fileCount}");
        Console.WriteLine($"Всего строк кода: {lineCount}");
        Console.WriteLine($"Классов: {classCount}");
        Console.WriteLine($"Структур: {structCount}");


        var factory = new Factory();
        var fontCollection = factory.GetSystemFontCollection(false);

        int fontCount = fontCollection.FontFamilyCount;

        for (int i = 0; i < fontCount; i++)
        {
            var fontFamily = fontCollection.GetFontFamily(i);
            var familyNames = fontFamily.FamilyNames;

            int index;
            if (familyNames.FindLocaleName("en-us", out index))
            {
                string fontName = familyNames.GetString(index);
                Console.WriteLine(fontName);
            }
        }

    }
}
