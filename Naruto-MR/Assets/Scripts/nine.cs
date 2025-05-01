using UnityEngine;

public class KyuubiSpawner : MonoBehaviour
{
    [Header("Prefab 設定")]
    public GameObject kyuubiPrefabs;
    public GameObject effectPrefab;

    [Header("生成位置 (可空)")]
    public Transform spawnPoint;

    [Header("縮放大小")]
    public Vector3 kyuubiScale = Vector3.one;
    public Vector3 effectScale = Vector3.one;

    [Header("特效偏移 (Y 軸用)")]
    public float effectYOffset = 0f;

    private GameObject spawnedKyuubi;
    private GameObject spawnedEffect;

    public void SpawnKyuubiWithEffect()
    {
        // 計算生成位置
        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;
        // set a 0,0, -15 rotation
        rotation = Quaternion.Euler(0, 184, -10);
        // minus position x and z
        position = new Vector3(position.x-0f, position.y, position.z-10f);

        // 生成九尾
        spawnedKyuubi = Instantiate(kyuubiPrefabs, position, rotation);
        spawnedKyuubi.transform.localScale = kyuubiScale;

        // 取得九尾的 XZ 中心作為特效位置
        Vector3 kyuubiPos = spawnedKyuubi.transform.position;
        Vector3 effectPos = new Vector3(kyuubiPos.x, kyuubiPos.y + effectYOffset, kyuubiPos.z);

        // 生成特效
        //spawnedEffect = Instantiate(effectPrefab, effectPos, rotation);
        //spawnedEffect.transform.localScale = effectScale;

        // 5 秒後銷毀
        //Destroy(spawnedKyuubi, 5f);
        //Destroy(spawnedEffect, 5f);
    }
}
