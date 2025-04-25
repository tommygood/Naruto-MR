using UnityEngine;

[System.Serializable]
public class EffectSetting
{
    public GameObject prefab;
    public Vector3 scale = Vector3.one;
    public float lifeTime = 2f; // 秒數
}

public class Effect : MonoBehaviour
{
    public EffectSetting fireEffect;
    public EffectSetting thunderEffect;
    public EffectSetting waterEffect;
    public EffectSetting windEffect;

    public Transform spawnPoint; // 發射點
    public Transform npc;        // 目標 NPC

    public float launchForce = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("火特效");
            SpawnEffect(fireEffect);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("雷特效");
            SpawnEffect(thunderEffect);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("水特效");
            SpawnEffect(waterEffect);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("風特效");
            SpawnEffect(windEffect);
        }
    }

    void SpawnEffect(EffectSetting setting)
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogWarning("spawnPoint 未設定，已自動設為當前物件");
        }

        if (setting != null && setting.prefab != null)
        {
            GameObject effect = Instantiate(setting.prefab, spawnPoint.position, Quaternion.identity);

            // 設定特效大小
            effect.transform.localScale = setting.scale;

            // 朝向 NPC 發射
            if (npc != null)
            {
                Vector3 direction = (npc.position - spawnPoint.position).normalized;
                effect.transform.rotation = Quaternion.LookRotation(direction);

                Rigidbody rb = effect.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * launchForce, ForceMode.Impulse);
                }
            }

            // 設定特效自動銷毀
            Destroy(effect, setting.lifeTime);

            Debug.Log($"生成特效：{setting.prefab.name}");
        }
        else
        {
            Debug.LogWarning("未指定特效 prefab！");
        }
    }
}
