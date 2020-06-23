using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kubika.Game
{
    public class _MOONfaces : MonoBehaviour
    {
        public ParticleSystem PS;
        public UnityEvent OnClick;

        public void ActivatePSFB()
        {
            PS.Play();
            StartCoroutine(TransitionTime());
        }

        IEnumerator TransitionTime()
        {
            yield return new WaitForSeconds(PS.main.duration*.025f);
            OnClick.Invoke();
        }

        public void OpenLevelEditor()
        {
            UIManager.instance.ButtonCallback("WORLDMAP_LevelEditor");
        }
        
        public void OpenLevelEditorLevels()
        {
            UIManager.instance.ButtonCallback("WORLDMAP_LevelEditorLevelsList");
        }
    }
}
