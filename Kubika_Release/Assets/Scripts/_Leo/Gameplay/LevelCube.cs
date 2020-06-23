using Kubika.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCube : MonoBehaviour
{
    public LevelCube previousLevel;
    public LevelCube nextLevel;
    public LevelCube nextOptionalLevel;
    public LevelCube prevOptionalLevel;
    public float width = .5f;
    public bool isOptionalLevel;
    public bool isAnchorNode;

    public string kubicode;
    public string levelName;
    public int minimalMoves;
    public int previousPlayerScore;
    public bool isBeaten;
    public bool isSelected; //set by tapping on the object

    void Start()
    {
        if (!isAnchorNode)
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
