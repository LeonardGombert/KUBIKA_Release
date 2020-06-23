using Kubika.CustomLevelEditor;
using Kubika.Game;
using Kubika.Saving;
using System;
using UnityEngine;


public class DecorObject : MonoBehaviour
{
    public Decor decorParams;

    private void Start()
    {

    }

    public void CreateNewDecor()
    {
        decorParams = new Decor();

        //assign grid
        decorParams.parent = _Grid.instance.kuboGrid[transform.parent.gameObject.GetComponent<_CubeBase>().myIndex - 1];
        decorParams.position = transform.position;
        decorParams.rotation = transform.rotation.eulerAngles;

        SaveAndLoad.instance.activeDecor.Add(decorParams);
    }

    public void LoadDecor(Decor recoveredDecor)
    {
        decorParams = recoveredDecor;
        transform.position = decorParams.position;
        transform.rotation = Quaternion.Euler(decorParams.rotation);
        transform.parent = _Grid.instance.kuboGrid[decorParams.parent.nodeIndex - 1].cubeOnPosition.transform;
    }
}

[System.Serializable]
public class Decor
{
    public Node parent;
    public Vector3 position;
    public Vector3 rotation;
}
