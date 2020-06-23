using System.Collections.Generic;

namespace Kubika.Saving
{
    [System.Serializable]
    public class PlayerProgress
    {
        public string nextLevelKubicode;
        public List<string> beatenLevels = new List<string>();
        public List<string> goldenLevels = new List<string>();
    }
}