using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CubeTestSynchronized : MonoBehaviour
{
    // MOVE LERP
    Vector3 currentPos;
    Vector3 basePos;
    float currentTime;
    public float moveTime;
    float time;


    public IEnumerator Move(Vector3 nextPosition)
    {
        basePos = transform.position;
        currentTime = 0;

        while (currentTime <= 1)
        {
            currentTime += Time.deltaTime;
            currentTime = (currentTime / moveTime);

            currentPos = Vector3.Lerp(basePos, nextPosition, currentTime);

            transform.position = currentPos;
            yield return transform.position;
        }
    }
}
