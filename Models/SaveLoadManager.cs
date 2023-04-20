using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Media;

namespace Sudoku.Models
{
    public class SaveLoadManager
    {
        private static SaveLoadManager instance;      

        const string save = "save";
        string dataDir;

        public static SaveLoadManager Instance()
        {
            if (instance == null)
                instance = new SaveLoadManager();
            return instance;
        }      

        private SaveLoadManager()
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string dataPath = projectDirectory + "\\Saves";

            if (!Directory.Exists(dataPath))            
                Directory.CreateDirectory(dataPath);                      

            dataDir = dataPath + "\\" + save + ".txt";
        }

        public bool LoadGame()
        {
            if(File.Exists(dataDir))
            {
                string json = File.ReadAllText(dataDir);

                Dictionary<string, object> dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                SudokuLogics.Instance().Matrix = JsonSerializer.Deserialize<int[][]>(dict["Matrix"].ToString());
                SudokuLogics.Instance().SolvedMatrix = JsonSerializer.Deserialize<int[][]>(dict["Solved"].ToString());

                Director.Instance().GameLevel = (GameLevel)int.Parse(dict["GameLevel"].ToString());
                Director.Instance().Mistakes = int.Parse(dict["Mistakes"].ToString());
                Director.Instance().Time = int.Parse(dict["Time"].ToString());

                return true;
            }           
            return false;   
        }

        public void SaveGame()
        {
           // SudokuLogics.Instance().Print(SudokuLogics.Instance().Matrix);

            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                ["Matrix"] = SudokuLogics.Instance().Matrix,
                ["Solved"] = SudokuLogics.Instance().SolvedMatrix,
                ["GameLevel"] = (int)Director.Instance().GameLevel,
                ["Mistakes"] = Director.Instance().Mistakes,
                ["Time"] = Director.Instance().Time
            };

             string json = JsonSerializer.Serialize(dict);                 
             File.WriteAllText(dataDir, json);
        }
    }
}
