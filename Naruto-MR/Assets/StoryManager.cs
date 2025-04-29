using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinesNamespace
{
    [System.Serializable]
    public class LinesManager
    {
        private List<string> linesTypes = new List<string> { "N_0_1", "S_0_1", "N_0_2", "N_1", "N_2", "N_3", "N_4", "N_5", "S_1", "S_2", "S_3", "S_4", "S_5", "S_scream" };
        public List<float> linesDurations = new List<float> { 18, 2, 8, 17, 15, 15, 5, 5, 5, 5, 5, 5, 5, 3 };

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

        private List<string> animationTypes = new List<string> { "running", "clapping", "backflip", "frontflip", "defeated", "angry", "uppercut", "stumble_backward" };

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

    public int currentLevel = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        linesManager = new LinesNamespace.LinesManager();
        animationManager = new AnimationNamespace.AnimationManager();
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
        linesManager.Play("N_1");
        yield return Sleep(linesManager.linesDurations[3]);
        linesManager.Play("S_1");
        yield return Sleep(linesManager.linesDurations[4]);
        linesManager.Play("N_3");
        animationManager.SetAnimation("angry", true);
        yield return Sleep(linesManager.linesDurations[5]);
        animationManager.SetAnimation("angry", false);

    }
}
