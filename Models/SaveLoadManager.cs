using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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

                Dictionary<string, int[][]> dict = JsonSerializer.Deserialize<Dictionary<string, int[][]>>(json);
                SudokuLogics.Instance().Matrix = dict["Matrix"];
                SudokuLogics.Instance().SolvedMatrix = dict["Solved"];
                return true;
            }           
            return false;   
        }

        public void SaveGame()
        {          
            Dictionary<string, int[][]> dict = new Dictionary<string, int[][]>
            {
                ["Matrix"] = SudokuLogics.Instance().Matrix,
                ["Solved"] = SudokuLogics.Instance().SolvedMatrix,
            };

             string json = JsonSerializer.Serialize(dict);                 
             File.WriteAllText(dataDir, json);
        }
    }
}
