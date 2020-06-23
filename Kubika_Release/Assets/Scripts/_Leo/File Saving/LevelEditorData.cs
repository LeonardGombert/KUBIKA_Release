using System.Collections.Generic;

namespace Kubika.Files
{
    [System.Serializable]
    public class LevelEditorData
    {
        public string levelName;
        public string Kubicode;
        public Biomes biome;
        public int minimumMoves;
        public bool lockRotate;
        public List<KuboNode> nodesToSave;
        //public List<Decor> decorToSave;
    }


    //user info file is used to store all of the user's level names, which you can then use to find the files
    [System.Serializable]
    public class UserLevels
    {
        public int numberOfUserLevels;
        public List<string> levelNames = new List<string>();
    }
}