using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject fireEffectPrefab;
    public GameObject thunderEffectPrefab;
    public GameObject waterEffectPrefab;
    public GameObject windEffectPrefab;

    public Transform spawnPoint; // 通常指向玩家身上某個位置，例如手或腳

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("火特效");  // 在控制台顯示狀態
            SpawnEffect(fireEffectPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("雷特效");  // 在控制台顯示狀態
            SpawnEffect(thunderEffectPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("水特效");  // 在控制台顯示狀態
            SpawnEffect(waterEffectPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("風特效");  // 在控制台顯示狀態
            SpawnEffect(windEffectPrefab);
        }
    }

    void SpawnEffect(GameObject effectPrefab)
        {
    if (spawnPoint == null)
    {
        // 如果 spawnPoint 沒有設定，則給它一個默認值
        spawnPoint = transform;  // 將當前物件的 transform 設為 spawnPoint
        Debug.LogWarning("spawnPoint 未設定，已自動設為當前物件");
    }

    if (effectPrefab != null)
    {
        GameObject effect = Instantiate(effectPrefab, spawnPoint.position, Quaternion.identity);
        effect.transform.SetParent(spawnPoint); // ✨ 讓特效跟著 spawnPoint（通常是玩家）移動
        Debug.Log($"生成特效：{effectPrefab.name}"); // 顯示已生成的特效名稱
    }
    else
    {
        Debug.LogWarning("未指定特效 prefab！");
    }
}

}
