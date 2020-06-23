using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCubeVisualizer : MonoBehaviour
{
    private static LevelCubeVisualizer _instance;
    public static LevelCubeVisualizer instance { get { return _instance; } }

    public LineRenderer lineRenderer;
    public LevelCube[] levelNodes;
    public Vector3[] nodePositions;

    void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;

        InitializeProperties();
    }

    public void InitializeProperties()
    {
        levelNodes = GetComponentsInChildren<LevelCube>();
        lineRenderer.positionCount = levelNodes.Length;
        nodePositions = new Vector3[levelNodes.Length];
    }

    void Update()
    {
        SetLineRendererPositions();
    }

    void SetLineRendererPositions()
    {
        for (int i = 0; i < levelNodes.Length; i++)
        {
            nodePositions[i] = levelNodes[i].transform.position;
        }

        lineRenderer.SetPositions(nodePositions);
    }
}