﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuboNode
{
    public int nodeIndex;
    public int nodeIndex0;
    public int nodeIndex1;
    public int nodeIndex2;

    public GameObject cubeOnPosition;

    public CubeTypes cubeType;

    public CubeLayers cubeLayers;
    public string savedCubeType;

    public FacingDirection facingDirection;

    public int xCoord, yCoord, zCoord;

    public Vector3 worldPosition;
    public Vector3 worldRotation;

    public static string ConvertTypeToString(CubeTypes cubetypes)
    {
        return cubetypes.ToString();
    }

    public static CubeTypes ConvertStringToCubeType(string cubetypes)
    {
        CubeTypes cubeType = CubeTypes.None;

        for (int i = 0; i < (int)CubeTypes.Count; i++)
        {
            cubeType = (CubeTypes)i;

            if (cubeType.ToString() == cubetypes)
                break;
        }
        return cubeType;
    }
}
