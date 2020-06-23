using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika.Game
{
    public class ParticleSystemAutoEnd : MonoBehaviour
    {
        ParticleSystem PS;

        private void Start()
        {
            PS = GetComponent<ParticleSystem>();
            StartCoroutine(SelfGameEnd());
        }
        // Update is called once per frame
        IEnumerator SelfGameEnd()
        {
            yield return new WaitForSeconds(PS.main.duration);
            Destroy(gameObject);
        }
    }
}
