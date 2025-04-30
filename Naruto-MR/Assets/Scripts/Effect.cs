using UnityEngine;

namespace EffectNamespace
{
    [System.Serializable]
    public class EffectSetting
    {
        public GameObject prefab;
        public Vector3 scale = Vector3.one;
        public float lifeTime = 2f; // 秒數

        public Transform spawnPoint; // 發射點
    }

    public class Effect : MonoBehaviour
    {
        public EffectSetting fireballEffect;
        public EffectSetting thunderSlideEffect;
        public EffectSetting waterfallEffect;
        public EffectSetting windSlamEffect;
        public EffectSetting mudwallEffect;
        public EffectSetting muddropEffect;


        public Transform spawnPoint; // 發射點
        public Transform playHead; // 頭盔

        public float launchForce = 5f;

        public void SpawnEffect(EffectSetting setting, string effect_name)
        {
            if (spawnPoint == null)
            {
                spawnPoint = transform;
                Debug.LogWarning("spawnPoint 未設定，已自動設為當前物件");
            }

            if (setting.spawnPoint != null)
            {
                spawnPoint = setting.spawnPoint;
            }

            if (playHead == null)
            {
                playHead = Camera.main.transform;
                Debug.LogWarning("playHead 未設定，已自動設為主攝影機");
            }

            if (setting != null && setting.prefab != null)
            {
                GameObject effect = Instantiate(setting.prefab, spawnPoint.position, Quaternion.identity);

                // 設定特效大小
                effect.transform.localScale = setting.scale;

                // 朝向 頭盔面向方向 發射
                Vector3 direction = playHead.forward.normalized;
                effect.transform.rotation = Quaternion.LookRotation(direction);

                Rigidbody rb = effect.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * launchForce, ForceMode.Impulse);
                }

                // 設定特效自動銷毀
                Destroy(effect, setting.lifeTime);

                if (effect_name == "thunderSlide") {
                    // add the effect to the sub object of the main camera
                    GameObject mainCamera = GameObject.FindGameObjectWithTag("LeftHandAnchor");
                    if (mainCamera != null)
                    {
                        effect.transform.SetParent(mainCamera.transform);
                    }
                    else
                    {
                        Debug.LogWarning("未找到主攝影機物件，特效不會自動銷毀！");
                    }
                }

                Debug.Log($"生成特效：{setting.prefab.name}");
            }
            else
            {
                Debug.LogWarning("未指定特效 prefab！");
            }
        }
    }
}
