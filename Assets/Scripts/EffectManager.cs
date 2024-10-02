using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSort
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance;

        [SerializeField] private ParticleSystem sparkle1;

        private void Start()
        {
            instance = this;

            sparkle1.gameObject.SetActive(false);
        }

        public void PlayEffect(Transform pos)
        {
            sparkle1.transform.position = pos.position;
            sparkle1.gameObject.SetActive(true);

            sparkle1.Play();
        }

        public void HideEffect(bool isHide)
        {
            sparkle1.gameObject.SetActive(isHide);
        }
    }
}

