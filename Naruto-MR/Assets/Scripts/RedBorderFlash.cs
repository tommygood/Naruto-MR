using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RedBorderFlashNamespace
{

    public class RedBorderFlash : MonoBehaviour
    {
        public Image[] redOverlayImages; // 拖入 4 個 UI Image：上下左右紅邊
        public float flashDuration = 3f; // 閃爍總長
        public float flashSpeed = 5f; // 閃爍頻率

        private Coroutine flashCoroutine;

        public void Play()
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashRedBorders());
        }

        IEnumerator FlashRedBorders()
        {
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed)) * 0.5f;

                foreach (Image img in redOverlayImages)
                {
                    if (img != null)
                    {
                        Color color = img.color;
                        color.a = alpha;
                        img.color = color;
                    }
                }

                yield return null;
            }

            // 結束後設為完全透明
            foreach (Image img in redOverlayImages)
            {
                if (img != null)
                {
                    Color color = img.color;
                    color.a = 0f;
                    img.color = color;
                }
            }

            flashCoroutine = null;
        }
    }

}