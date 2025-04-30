using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LinesNamespace
{
    [System.Serializable]
    public class LinesManager
    {
        private List<string> linesTypes = new List<string> { "N_0_1", "S_0_1", "N_0_2", "N_1", "S_1", "N_2", "S_2", "N_3", "S_3", "N_4", "S_4", "N_5", "S_5", "N_6", "S_6", "N_7", "S_scream" };
        public List<float> linesDurations = new List<float> { 15, 5, 11, 5, 8, 5, 11, 5, 5, 5, 11, 7, 11, 4, 5, 4, 3 };
        public List<string> subtitles = new List<string> {
            "結印是釋放忍術必要的技巧，包含結印的正確性和速度，往往都決定了一個忍者的好壞。佐助，對你來說這算是非常簡單的吧！",
            "當然，我可是班上成績最好的。",
            "那你就試試看依序使用不同忍術，擊倒這些靶子吧，如果你忘記怎麼做，可以看一下那邊的秘笈。",
            "佐助！！！你想要逃跑嗎",
            "呦，你這個吊車尾的，這次又換你了嗎，小櫻已經跟我說過了，你們不要再管我了",
            "為什麼，為什麼你會變成這個樣子呢",
            "我會變成什麼樣子，關你什麼事呢。老實說吧，我已經不想和你們木葉的人在一起了，回去吧",
            "大家都是拼上性命，要來把你帶回去的...",
            "真是辛苦你們了，可以回去了",
            "你這傢伙，把我們夥伴當作什麼了！！！",
            "什麼夥伴，呵，夥伴能夠幫助我變強嗎？總之，我要去大蛇丸那邊",
            "你以為大蛇丸會無條件地幫你嗎？我們怎麼可能讓你去送死呢",
            "那種事情我才不在乎，只要可以達成我的目標，要怎樣都無所謂，如果你堅持要阻止我，那就沒辦法了",
            "你小子，即使使用蠻力，我也要把你帶回去！",
            "這次我們一定要做個了斷，我要打敗你",
            "我一定要把你帶回去",
            "你也差不多該清醒過來了吧！",
            "怎麼回事，這個紅色的查克拉，這股力量是哪來的",
            "放棄吧，我是不會回頭的",
            "來吧，鳴人，就由我來斬斷這份羈絆",
            "攻擊都沒有效果，是那個紅色的查克拉在保護著他嗎",
            "可惡，這到底是什麼鬼，能躲開就很不容易了",
            "冷靜一點，保持距離作戰，他就打不到我了",
            "真是的，沒辦法啊，一但借用了這股力量，最後會變成什麼樣子我都不知道",
            "鳴人，你真的很特別，但是憑你是沒有辦法阻止我的",
            "鳴人，我...",
            "卡卡西來了，憑我目前的狀態，無法打贏他，還是快走吧"
        };

        public LinesManager()
        {
            if (linesTypes.Count != linesDurations.Count)
            {
                Debug.LogError($"Mismatching the count of linesTypes and linesDurations with {linesTypes.Count} and {linesDurations.Count}");
            }
        }

        // TODO: Find the object by more effective way
        public void Play(string line)
        {
            if (linesTypes.Contains(line))
            {
                GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    // check if the object has a tag which is in the ninjutsuNames list
                    if (obj.tag == line)
                    {
                        obj.SetActive(true);
                        return;
                    }
                }
                Debug.LogError($"Failed to find the lines (line)");
            }
        }

        public void Stop(string line)
        {
            if (linesTypes.Contains(line))
            {
                GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                foreach (GameObject obj in allObjects)
                {
                    // check if the object has a tag which is in the ninjutsuNames list
                    if (obj.tag == line)
                    {
                        obj.SetActive(false);
                        return;
                    }
                }
                Debug.LogError($"Failed to find the lines (line)");
            }
        }
    }
}

namespace AnimationNamespace
{
    public class AnimationManager
    {
        private Animator animator;
        public AnimationManager()
        {
            // set the animator
            GameObject naruto = GameObject.FindGameObjectWithTag("naruto");
            animator = naruto.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Failed to set the animator from naruto object");
            }
        }

        private List<string> animationTypes = new List<string> { "running", "clapping", "backflip", "frontflip", "defeated", "angry", "uppercut", "stumble_backward", "boxing", "CastingSpell" };

        public void SetAnimation(string animation, bool active)
        {
            Debug.Log("set animation: " + animation);
            if (animationTypes.Contains(animation))
            {
                animator.SetBool(animation, active);
            }
            else
            {
                Debug.LogError($"Animation: {animation} not existed in current animations");
            }
        }
    }
}

public class StoryManager : MonoBehaviour
{
    public LinesNamespace.LinesManager linesManager;
    public AnimationNamespace.AnimationManager animationManager;
    private NPCController npcController;
    public TextMeshProUGUI subtitle;

    public Color32 narutoColor = new Color32(255, 165, 0, 255);
    public Color32 sasukeColor = new Color32(30, 58, 138, 255);

    public int currentLevel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linesManager = new LinesNamespace.LinesManager();
        animationManager = new AnimationNamespace.AnimationManager();
        npcController = FindAnyObjectByType<NPCController>();
        if (currentLevel == 1)
        {
            StartCoroutine(Level1Init());
        }
        else if (currentLevel == 2)
        {
            StartCoroutine(Level2Init());
        }
    }

    private IEnumerator Sleep(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator Level1Init()
    {
        // set the position of naruto near to the main camera
        GameObject naruto = GameObject.FindGameObjectWithTag("naruto");
        if (naruto != null)
        {
            // find the main camera by tag
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (mainCamera != null)
            {
                // set the position of naruto to the main camera position
                Vector3 newPosition = mainCamera.transform.position + new Vector3(0, 0, 2);
                naruto.transform.position = newPosition;
                Debug.Log("Set the position of naruto to: " + newPosition);
            }
            else
            {
                Debug.LogError("Failed to find the main camera object");
            }
        }
        else
        {
            Debug.LogError("Failed to find the naruto object");
        }
        linesManager.Play("N_0_1");
        yield return Sleep(linesManager.linesDurations[0]);
        linesManager.Play("S_0_1");
        yield return Sleep(linesManager.linesDurations[1]);
        linesManager.Play("N_0_2");
        animationManager.SetAnimation("clapping", true);
        yield return Sleep(linesManager.linesDurations[2]);
        animationManager.SetAnimation("clapping", false);
    }

    private IEnumerator Level2Init()
    {
        npcController.isAttacking = true;
        linesManager.Play("N_1");
        subtitle.text = linesManager.subtitles[0];
        subtitle.color = narutoColor;
        yield return Sleep(linesManager.linesDurations[3]);
        npcController.isAttacking = false;
        linesManager.Play("S_1");
        subtitle.text = linesManager.subtitles[1];
        subtitle.color = sasukeColor;
        yield return Sleep(linesManager.linesDurations[4]);
        linesManager.Play("N_2");
        subtitle.text = linesManager.subtitles[2];
        subtitle.color = narutoColor;
        animationManager.SetAnimation("angry", true);
        yield return Sleep(linesManager.linesDurations[5]);
        linesManager.Play("S_2");
        subtitle.color = sasukeColor;
        subtitle.text = linesManager.subtitles[3];
        yield return Sleep(linesManager.linesDurations[6]);
        linesManager.Play("N_3");
        subtitle.text = linesManager.subtitles[4];
        subtitle.color = narutoColor;
        yield return Sleep(linesManager.linesDurations[7]);
        linesManager.Play("S_3");
        subtitle.text = linesManager.subtitles[5];
        subtitle.color = sasukeColor;
        yield return Sleep(linesManager.linesDurations[8]);
        animationManager.SetAnimation("angry", false);
        linesManager.Play("N_4");
        subtitle.color = narutoColor;
        subtitle.text = linesManager.subtitles[6];
        animationManager.SetAnimation("defeated", true);
        yield return Sleep(linesManager.linesDurations[9]);
        linesManager.Play("S_4");
        subtitle.text = linesManager.subtitles[7];
        subtitle.color = sasukeColor;
        yield return Sleep(linesManager.linesDurations[10]);
        linesManager.Play("N_5");
        subtitle.text = linesManager.subtitles[8];
        animationManager.SetAnimation("defeated", false);
        yield return Sleep(linesManager.linesDurations[11]);
        linesManager.Play("S_5");
        subtitle.text = linesManager.subtitles[9];
        yield return Sleep(linesManager.linesDurations[12]);
        linesManager.Play("N_6");
        animationManager.SetAnimation("defeated", true);
        yield return Sleep(linesManager.linesDurations[13]);
        linesManager.Play("S_6");
        subtitle.text = linesManager.subtitles[10];
        yield return Sleep(linesManager.linesDurations[14]);
        linesManager.Play("N_7");
        subtitle.text = linesManager.subtitles[11];
        yield return Sleep(linesManager.linesDurations[15]);
        animationManager.SetAnimation("defeated", false);
    }
}
