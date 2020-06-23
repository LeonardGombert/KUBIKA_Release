using Kubika.Game;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCube : MonoBehaviour
{
    [FoldoutGroup("Node Info")] public LevelCube previousLevel;
    [FoldoutGroup("Node Info")] public LevelCube nextLevel;
    [FoldoutGroup("Node Info")] public LevelCube nextOptionalLevel;
    [FoldoutGroup("Node Info")] public LevelCube prevOptionalLevel;
    [FoldoutGroup("Node Info")] public float width = .5f;
    [FoldoutGroup("Node Info")] public bool isOptionalLevel;
    [FoldoutGroup("Node Info")] public bool isAnchorNode;

    [FoldoutGroup("Kubika Level Info")] public string kubicode;
    [FoldoutGroup("Kubika Level Info")] public string levelName;
    [FoldoutGroup("Kubika Level Info")] public int minimalMoves;
    [FoldoutGroup("Kubika Level Info")] public int previousPlayerScore;
    [FoldoutGroup("Kubika Level Info")] public bool isBeaten;
    [FoldoutGroup("Kubika Level Info")] public bool isSelected; //set by tapping on the object

    void Start()
    {
        if(!isAnchorNode)
        {
            gameObject.name = gameObject.name.Replace("Level Node ", "Worl");

            for (int i = 0; i < LevelsManager.instance.gameMasterList.Count; i++)
            {
                if (gameObject.name == LevelsManager.instance.gameMasterList[i].kubicode)//gameObject.name == i.ToString())
                {
                    kubicode = LevelsManager.instance.gameMasterList[i].kubicode;
                    levelName = LevelsManager.instance.gameMasterList[i].levelName;
                    minimalMoves = LevelsManager.instance.gameMasterList[i].minimumMoves;
                    //previousPlayerScore = LevelsManager.instance.masterList[i].prevPlayerScore;
                    //isBeaten = LevelsManager.instance.masterList[i].levelBeaten;
                }
            }
        }
    }
}
