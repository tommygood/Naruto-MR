using UnityEngine;

public class MudWallController : MonoBehaviour
{
    public float riseHeight = 3f;        // 上升的高度
    public float riseSpeed = 2f;         // 上升速度
    public float stayDuration = 5f;      // 停留秒數
    public float fallSpeed = 2f;         // 下降速度

    private Vector3 startPos;
    private Vector3 topPos;
    private Vector3 endPos;

    private enum State { Rising, Staying, Falling }
    private State currentState = State.Rising;

    private float stayTimer = 0f;

    void Start()
    {
        startPos = transform.position; // 地板位置
        topPos = startPos + Vector3.up * riseHeight;
        endPos = startPos + Vector3.down * 1f; // 完全消失
        transform.position = startPos; // 從地板下開始
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Rising:
                transform.position = Vector3.MoveTowards(transform.position, topPos, riseSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, topPos) < 0.01f)
                {
                    currentState = State.Staying;
                    stayTimer = stayDuration;
                }
                break;

            case State.Staying:
                stayTimer -= Time.deltaTime;
                if (stayTimer <= 0)
                {
                    currentState = State.Falling;
                }
                break;

            case State.Falling:
                transform.position = Vector3.MoveTowards(transform.position, endPos, fallSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, endPos) < 0.01f)
                {
                    Destroy(gameObject); // 下降後銷毀
                }
                break;
        }
    }
}
