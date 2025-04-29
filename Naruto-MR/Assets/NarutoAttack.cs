using UnityEngine;

public class NarutoAttack : MonoBehaviour
{
    public EffectNamespace.EffectSetting fireEffect;
    public EffectNamespace.EffectSetting thunderEffect;
    public EffectNamespace.EffectSetting waterEffect;
    public EffectNamespace.EffectSetting windEffect;
    public EffectNamespace.EffectSetting Rasengan;

    public Transform spawnPoint; // 發射點
    public Transform npc;        // 目標 NPC

    public float launchForce = 5f;

    void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogError("spawnPoint 未設定，已自動設為當前物件");
        }
        SpawnEffect(fireEffect);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("火特效");
            SpawnEffect(fireEffect);
        }
    }

    void SpawnEffect(EffectNamespace.EffectSetting setting)
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

