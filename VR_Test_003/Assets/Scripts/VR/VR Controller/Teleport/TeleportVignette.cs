using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VRTeleport
{
    public class TeleportVignette : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve blinkCurve = null;
        [SerializeField]
        private float blinkStartVal = 0f;
        [SerializeField]
        private float blinkExtremeVal = 0f;
        [SerializeField]
        private float blinkTime = 0f;


        private float blinkS = 0f;
        private bool isBlinking = false;

        private Vignette vignette = null;
        private Volume postProcVol = null;

        // Start is called before the first frame update
        void Start()
        {
            postProcVol = FindObjectOfType<Volume>();
            bool success = postProcVol.profile.TryGet<Vignette>(out vignette);

            if (!success)
            {
                Debug.LogError("No vignette component found");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isBlinking)
            {
                blinkS += Time.deltaTime / blinkTime;
                if (blinkS >= 1f)
                {
                    blinkS = 1f;
                    isBlinking = false;
                }

                vignette.intensity.value = Mathf.Lerp(blinkStartVal, blinkExtremeVal, blinkCurve.Evaluate(blinkS));
            }
        }

        public void Blink()
        {
            isBlinking = true;
            blinkS = 0f;
        }
    }

}