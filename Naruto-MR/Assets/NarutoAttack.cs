using UnityEngine;
using AnimationNamespace;
using System.Collections;

public class NarutoAttack : MonoBehaviour
{
    public EffectNamespace.EffectSetting fireEffect;
    public EffectNamespace.EffectSetting thunderEffect;
    public EffectNamespace.EffectSetting waterEffect;
    public EffectNamespace.EffectSetting windEffect;
    public EffectNamespace.EffectSetting RasenganEffect;

    public Transform spawnPoint; // 發射點
    public Transform npc;        // 目標 NPC

    public float launchForce = 5f;

    AnimationManager animationManager;

    void Start()
    {
        animationManager = new AnimationNamespace.AnimationManager();
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogError("spawnPoint 未設定，已自動設為當前物件");
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("火特效");
            StartCoroutine(LongDistanceAttack(fireEffect, "clapping"));
        } else if (Input.GetKeyDown(KeyCode.X)) {
            Debug.Log("Rasengan");
            StartCoroutine(LongDistanceAttack(RasenganEffect, "CastingSpell"));
        }
    }

    // sleep for seconds
    private IEnumerator Sleep(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator LongDistanceAttack(EffectNamespace.EffectSetting setting, string animationName)
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
            Debug.LogWarning("spawnPoint 未設定，已自動設為當前物件");
        }

        if (setting != null && setting.prefab != null)
        {
            animationManager.SetAnimation("clapping", true);
            yield return Sleep(5f);
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
            animationManager.SetAnimation("clapping", false);

            // 設定特效自動銷毀
            Destroy(effect, setting.lifeTime);

            Debug.Log($"生成特效：{setting.prefab.name}");
        }
        else
        {
            Debug.LogWarning("未指定特效 prefab！");
        }
        yield return null;
    }

    public Transform spawnPoint2; // 發射點2

    public IEnumerator ShortDistanceAttack(EffectNamespace.EffectSetting setting, string animationName)
    {
        spawnPoint = spawnPoint2;

        if (setting != null && setting.prefab != null)
        {
            animationManager.SetAnimation("clapping", false); // this is to stop the clapping animation
            animationManager.SetAnimation("CastingSpell", true);
            yield return Sleep(5f);
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
            animationManager.SetAnimation("CastingSpell", false);

            // 設定特效自動銷毀
            Destroy(effect, setting.lifeTime);

            Debug.Log($"生成特效：{setting.prefab.name}");
        }
        else
        {
            Debug.LogWarning("未指定特效 prefab！");
        }
        yield return null;

        spawnPoint = null;
    }
    
}

