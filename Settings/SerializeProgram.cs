using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Settings
{
    public class SerializeProgram
    {
        public static void Save(GameConfig gameConfig, string path)
        {
            string json = JsonConvert.SerializeObject(gameConfig, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static GameConfig Load(string path)
        {
            GameConfig config = JsonConvert.DeserializeObject<GameConfig>(File.ReadAllText(path));
            return config;
        }
    }
}
