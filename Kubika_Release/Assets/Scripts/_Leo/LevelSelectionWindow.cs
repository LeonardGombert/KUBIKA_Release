using Kubika.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectionWindow : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.rectTransform.position = Camera.main.WorldToScreenPoint(_Planete.instance.targetLevel.transform.position);

    }
}
