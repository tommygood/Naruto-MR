using UnityEngine;

public class NewbiEffectTest : MonoBehaviour
{
    public GameObject effectPrefab;  // 需要在Inspector中設置特效Prefab
    public Transform spawnPoint;     // 發射點，通常設置為角色或武器的前端
    public Transform npc;            // 目標 NPC

    public float launchForce = 5f;   // 發射力量
    public float scaleMultiplier = 1f;  // 調整特效大小的乘數

    public OVRInput.Controller controller = OVRInput.Controller.RTouch;  // 設定控制器為右手觸發器

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (effectPrefab == null)
        {
            Debug.LogError("EffectPrefab is not assigned!");
        }

        if (spawnPoint == null)
        {
            spawnPoint = transform;  // 如果未指定，默認使用當前物件的Transform
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 當按下觸發器按鈕時觸發特效
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            Debug.Log("Trigger pressed! Activating attack effect.");
            SpawnAttackEffect();
        }
    }

    void SpawnAttackEffect()
    {
        if (effectPrefab != null && spawnPoint != null)
        {
            // 在發射點生成特效
            GameObject effect = Instantiate(effectPrefab, spawnPoint.position, Quaternion.identity);

            // 調整特效大小
            effect.transform.localScale = Vector3.one * scaleMultiplier;  // 設置大小，這裡可以使用 `scaleMultiplier` 調整

            // 如果有指定目標NPC，將特效朝向NPC
            if (npc != null)
            {
                Vector3 direction = (npc.position - spawnPoint.position).normalized;
                effect.transform.rotation = Quaternion.LookRotation(direction);

                // 為特效添加剛體並使其朝向NPC發射
                Rigidbody rb = effect.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(direction * launchForce, ForceMode.Impulse);
                }
            }

            // 可選：設定特效自動銷毀
            Destroy(effect, 2f);  // 2秒後銷毀特效
        }
        else
        {
            Debug.LogError("EffectPrefab or SpawnPoint is missing!");
        }
    }
}
