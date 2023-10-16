using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tony
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private float frameDelayInSeconds;

        private SpriteRenderer s_renderer;
        private Coroutine animationRoutine;

        public void Start()
        {
            if (sprites.Count == 0)
            {
                Destroy(this);
                return;
            }
            s_renderer = this.GetComponent<SpriteRenderer>();
            animationRoutine = StartCoroutine(AnimRoutine());
        }

        public IEnumerator AnimRoutine()
        {
            int i = 0;
            while(true)
            {
                s_renderer.sprite = sprites[i];
                i = (i + 1) % sprites.Count;
                yield return new WaitForSeconds(frameDelayInSeconds);
            }
        }
    }
}

