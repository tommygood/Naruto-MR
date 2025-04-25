using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;
using UnityEngine.XR.Management;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UI;
using TMPro;
using EffectNamespace; // Assuming EffectNamespace is the namespace for Effect and EffectSetting classes

public class AudioManager {
    public void enableAudio(string name)
    {
        // find the object by the tag name
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // check if the object has a tag which is in the ninjutsuNames list
            if (obj.tag == name)
            {
                obj.SetActive(false);
                obj.SetActive(true);
                return;
            }
        }
        //Debug.LogError($"Audio object with tag {name} not found!");
        // FIXME: add a warning log if the object is not found
    }
}

public class NinjutsuGesture
{
    public string name; // Name of the ninjutsu
    public string atrribute; // Attribute of the ninjutsu
    public string[] gestures; // Array of gesture names
    public string[] current_gestures; // Array of current gesture names

    public EffectSetting ninjutusu_particle; // Particle system for the ninjutsu

    public int chakra_cost = 10; // Chakra cost for the ninjutsu

    // a function to implement the ninjutsu
    public void ActivateNinjutsu()
    {
        // Implement the logic to activate the ninjutsu here
        // set a gameobject which have a tag same as the ninjutsu name to active
        // public EffectSetting fireEffect;
        Effect effect = GameObject.FindFirstObjectByType<Effect>();

        // find the object from effect class by the ninjutsu name
        ninjutusu_particle = effect.GetType().GetField($"{atrribute}Effect").GetValue(effect) as EffectSetting;
        if (ninjutusu_particle != null)
        {
            
            effect.SpawnEffect(ninjutusu_particle);
            AudioManager audioManager = new AudioManager();
            // enable with name of the Audio-<ninjutsu_name>
            audioManager.enableAudio($"Audio-{name}");
        }
        else {
            Debug.LogError($"Ninjutsu particle for {name} is not set.");
        }
        Debug.Log($"Activating Ninjutsu: {name} with attribute: {atrribute}");
    }
}

public class GestureConfirmation
{
    // This class is used to confirm the gesture, which can reduce the false positive rate
    public string last_gesture = "None"; // Last detected gesture
    public int gesture_count = 0; // Count of the same gesture detected in a row

    private int gesture_count_threshold = 70; // Threshold for gesture confirmation, which means the gesture is confirmed if the same gesture is detected 3 times in a row

    public void Reset()
    {
        last_gesture = "None";
        gesture_count = 0;
    }

    public bool IsConfirmed()
    {
        return gesture_count >= gesture_count_threshold;
    }
}

public class HandJointTracker : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    // track each hand's joints position and rotation
    private Dictionary<XRHandJointID, Pose> leftHandJoints = new Dictionary<XRHandJointID, Pose>();
    private Dictionary<XRHandJointID, Pose> rightHandJoints = new Dictionary<XRHandJointID, Pose>();

    private NinjutsuGesture[] ninjutsuGestures; // Array of Ninjutsu gestures

    private GestureConfirmation gestureConfirmation; // Gesture confirmation object

    public int chakra = 100; // Chakra stats

    public int ninjutsu_timeout = 5; // Ninjutsu timeout in seconds

    private float ninjutsu_timeout_count = 0; // Ninjutsu timeout count

    // find the MainCamera object in the scene by tag
    private GameObject MainCamera;

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        // print the name of the MainCamera object
        if (MainCamera != null)
        {
            Debug.Log($"MainCamera object found: {MainCamera.name}");
        }
        else
        {
            Debug.LogError("MainCamera object not found!");
        }

        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
        // initialize the ninjutsu gesture
        ninjutsuGestures = new NinjutsuGesture[]
        {
            new NinjutsuGesture { name = "fireball", atrribute = "fire", gestures = new string[] { "Mi", "Saru", "I" }, current_gestures = new string[] { "Mi", "Saru", "I" }, chakra_cost = 20 },
            new NinjutsuGesture { name = "waterfall", atrribute = "water", gestures = new string[] { "Tora", "Ne", "Saru", "I" }, current_gestures =  new string[] { "Tora", "Saru", "Ne", "I" }, chakra_cost = 25 },
            new NinjutsuGesture { name = "thunderSlide", atrribute = "thunder", gestures = new string[] { "Saru", "Ne" }, current_gestures = new string[] { "Ne", "I" }, chakra_cost = 15 },
            new NinjutsuGesture { name = "windSlam", atrribute = "wind", gestures = new string[] { "Mi", "I", "Ne" }, current_gestures = new string[] { "Uma", "I" }, chakra_cost = 10 },
        };
        // set the particle system for the ninjutsu gesture
        //SetNinjutsuParticle();

        // initialize the gesture confirmation object
        gestureConfirmation = new GestureConfirmation();
    }

    /*
    void SetNinjutsuParticle()
    {
        // Set the particle system for the ninjutsu
        // store a list of name of the ninjutsu gesture in a list
        List<string> ninjutsuNames = new List<string>();
        foreach (NinjutsuGesture ninjutsu in ninjutsuGestures)
        {
            ninjutsuNames.Add(ninjutsu.name);
        }
        // set the particle system for each ninjutsu gesture
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            // check if the object has a tag which is in the ninjutsuNames list
            if (ninjutsuNames.Contains(obj.tag))
            {
                // set the particle system for the ninjutsu gesture
                foreach (NinjutsuGesture ninjutsu in ninjutsuGestures)
                {
                    if (obj.tag == ninjutsu.name)
                    {
                        ninjutsu.ninjutusu_particle = obj;
                        break;
                    }
                }
            }
        }
    }
    */

    void Update()
    {
        if (ninjutsu_timeout_count > 0)
        {
            ninjutsu_timeout_count -= Time.deltaTime;
        }

        if (handSubsystem == null || !handSubsystem.running || ninjutsu_timeout_count > 0)
            return;

        var leftHand = handSubsystem.leftHand;
        var rightHand = handSubsystem.rightHand;

        if (leftHand.isTracked)
        {
            PrintJoints(leftHand);
        }

        if (rightHand.isTracked)
        {
            PrintJoints(rightHand);
        }
    }

    bool IsGestureNe() {
        // gesture Ne 
        // check the distance between left and right IndexTip joint
        // check the distance between left and right ThumbMetacarpal joint
        if (leftHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose leftIndexTip) &&
            rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightIndexTip) &&
            leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
            rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal)
            )
        {
            float distance_indextip = leftIndexTip.position.x - rightIndexTip.position.x;
            float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_indextip > 0f && distance_indextip < 0.05f) &&
                (distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f)
                
            )
            {
                return true; // Replace with actual gesture name
            }
       }
       return false;
    }

    bool IsGestureTora() {
        // check the distance between left and right IndexDistal joint
       // check the distance between left and right ThumbMetacarpal joint
        if (leftHandJoints.TryGetValue(XRHandJointID.IndexDistal, out Pose leftIndexDistal) &&
            rightHandJoints.TryGetValue(XRHandJointID.IndexDistal, out Pose rightIndexDistal) &&
            leftHandJoints.TryGetValue(XRHandJointID.ThumbProximal, out Pose leftThumbProximal) &&
            rightHandJoints.TryGetValue(XRHandJointID.ThumbProximal, out Pose rightThumbProximal)
            )
        {
            float distance_indexdistal = leftIndexDistal.position.x - rightIndexDistal.position.x;
            float distance_thumbmetacarpal = leftThumbProximal.position.y - rightThumbProximal.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_indexdistal > -0.01f && distance_indexdistal < 0.01f) &&
                (distance_thumbmetacarpal < 0.002f && distance_thumbmetacarpal > 0f)
                
            )
            {
                return true; // Replace with actual gesture name
            }
        }
        return false;
    }

    bool IsGestureSaru() {
       // check the distance between top and bottom of IndexTip joint and ThumbMetacarpal joint
       // check the distance between left and right Palm joint
       // check the rotation of left and right Palm joint
       
       if (leftHandJoints.TryGetValue(XRHandJointID.MiddleTip, out Pose leftMiddleTip) &&
           rightHandJoints.TryGetValue(XRHandJointID.MiddleTip, out Pose rightMiddleTip) &&
           leftHandJoints.TryGetValue(XRHandJointID.LittleIntermediate, out Pose leftLittleIntermediate) &&
           rightHandJoints.TryGetValue(XRHandJointID.IndexIntermediate, out Pose rightIndexIntermediate) &&
           leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
           rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal) &&
           leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPalm) &&
           rightHandJoints.TryGetValue(XRHandJointID.Palm, out Pose rightPalm)
           )
       {
            float distance_middletip = leftMiddleTip.position.x - rightMiddleTip.position.x;
            Vector3 rightDirection = MainCamera.transform.right.normalized;
            Vector3 diff = leftMiddleTip.position - rightMiddleTip.position;
            float signedDistance = Vector3.Dot(diff, rightDirection);
            float distance_indextip = leftLittleIntermediate.position.y - rightIndexIntermediate.position.y;
            Vector3 upDirection = MainCamera.transform.up.normalized;
            Vector3 diff2 = leftLittleIntermediate.position - rightIndexIntermediate.position;
            float signedDistance2 = Vector3.Dot(diff2, upDirection);
            
            //Debug.Log($"QQ Distance between left and right IndexTip: {distance_indextip}, {((distance_indextip > -0.09f && distance_indextip < 0f) ? "true" : "false")}");
            //Debug.Log($"QQ Distance between left and right MiddleTip: {distance_middletip}, {((distance_middletip > -0.17f && distance_middletip < -0.15f) ? "true" : "false")}");
            // print the condition is matched or not
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((signedDistance < 0.15f && signedDistance > 0.1f) &&
                (signedDistance2 > -0.09f && signedDistance2 < 0f) 
            )
            {
                Quaternion leftPalmRotation = leftPalm.rotation;
                Quaternion rightPalmRotation = rightPalm.rotation;
                // print the rotation of left and right Palm joint
                
                float angle = leftPalmRotation.eulerAngles.x + rightPalmRotation.eulerAngles.x;
                if (angle > 343 && angle < 355f)
                {
                    return true; // Replace with actual gesture name Left Palm Rotation: (355.83, 35.95, 96.82) Right Palm Rotation: (352.50, 215.55, 76.88)
                }
            }
        }
        return false;
    }

    bool IsGestureI() {
        // check the distance between left and right ThumbMetacarpal joint
       // check the rotation of left and right ThumbMetacarpal joint
       // TODO: a little conflict with gesture "Tora"

        if (leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPalm) &&
            rightHandJoints.TryGetValue(XRHandJointID.Palm, out Pose rightPalm)
            )
        {
            float distance_palm = leftPalm.position.z - rightPalm.position.z;
            // get rotation x of left and right Palm joint
            Quaternion leftPalmRotation = leftPalm.rotation;
            Quaternion rightPalmRotation = rightPalm.rotation;
            //Debug.Log($"Left Palm Rotation: {leftPalmRotation.eulerAngles.x}, Right Palm Rotation: {rightPalmRotation.eulerAngles.x}");
            //Debug.Log($"Distance between left and right palm: {distance_palm}");
            if ((leftPalmRotation.eulerAngles.x > 30f && leftPalmRotation.eulerAngles.x < 80f) &&
                (rightPalmRotation.eulerAngles.x > 30f && rightPalmRotation.eulerAngles.x < 80f) &&
                (distance_palm > 0f && distance_palm < 0.07f)
                
            )
            {
                return true; // Replace with actual gesture name
            }
        }
        return false;
    }

    bool IsGestureMi() {
         // check the distance between left and right MiddleTip joint
       // check the distance between top and bottom of IndexTip joint and ThumbMetacarpal joint
       // check the distance between left and right Palm joint
       // check the rotation of left and right Palm joint
       if (leftHandJoints.TryGetValue(XRHandJointID.IndexDistal, out Pose leftIndexDistal) &&
            rightHandJoints.TryGetValue(XRHandJointID.IndexDistal, out Pose rightIndexDistal) &&
            rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightIndexTip) &&
            rightHandJoints.TryGetValue(XRHandJointID.IndexProximal, out Pose rightIndexProximal) &&
            leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPalm) &&
           rightHandJoints.TryGetValue(XRHandJointID.Palm, out Pose rightPalm)
           )
       {
            float distance_indexdistal = leftIndexDistal.position.x - rightIndexDistal.position.x;
            float distance_indextip = rightIndexProximal.position.y - rightIndexTip.position.y;
            float distance_indextip_z = rightIndexProximal.position.z - rightIndexTip.position.z;
            float left_palm_x = leftPalm.rotation.eulerAngles.x;
            Vector3 rightDirection = MainCamera.transform.right.normalized;
            Vector3 diff = leftIndexDistal.position - rightIndexDistal.position;
            float signedDistance = Vector3.Dot(diff, rightDirection);
            Vector3 upDirection = MainCamera.transform.up.normalized;

            Vector3 diff2 = rightIndexProximal.position - rightIndexTip.position;
            float signedDistance2 = Vector3.Dot(diff2, upDirection);
            //Debug.Log($"Signed distance2: {signedDistance2}");
            Vector3 forwardDirection = MainCamera.transform.forward.normalized;
            Vector3 diff3 = rightIndexProximal.position - rightIndexTip.position;
            float signedDistance3 = Vector3.Dot(diff3, forwardDirection);

            //Debug.Log($"Signed distance: {signedDistance}. Signed distance2: {signedDistance2}. Signed distance3: {signedDistance3}");
            //Debug.Log($"Left Palm X: {left_palm_x}");
            //Debug.Log($"distance_indexdistal: {distance_indexdistal}, left indextip: {MainCamera.transform.InverseTransformPoint(leftIndexDistal.position).x}, right indextip: {MainCamera.transform.InverseTransformPoint(rightIndexDistal.position).x}");
            //Debug.Log($"Distance between left and right IndexTip: {distance_indextip}, {((distance_indextip > 0.0001f && distance_indextip < 0.04f) ? "true" : "false")}");
            //Debug.Log($"Distance between left and right IndexDistal: {distance_indexdistal}, {((distance_indexdistal > 0.02f && distance_indexdistal < 0.07f) ? "true" : "false")}"); // -0.03
            //Debug.Log($"Distance between left and right IndexTip Z: {distance_indextip_z}, {((distance_indextip_z > -0.07f && distance_indextip_z < 0f) ? "true" : "false")}"); // 0.06
            if ((signedDistance > 0.02f && signedDistance < 0.07f) &&
                ((signedDistance2 > 0.0001f && signedDistance2 < 0.04f)) &&
                ((signedDistance3 > -0.07f && signedDistance3 < 0f)) &&
                (left_palm_x > 280f && left_palm_x < 320f) 
            )
            {
                return true; // Replace with actual gesture name Left Palm Rotation: (355.83, 35.95, 96.82) Right Palm Rotation: (352.50, 215.55, 76.88)
            }
            
        }
        return false;
    }

    bool IsGestureUma() {
        // check the distance between left and right ThumbMetacarpal joint
         // check the distance between left and right IndexTip joint
         // check the distance between left and right IndexIntermediate joint
         if (leftHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose leftIndexTip) &&
             rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightIndexTip) &&
             leftHandJoints.TryGetValue(XRHandJointID.IndexIntermediate, out Pose leftIndexIntermediate) &&
             rightHandJoints.TryGetValue(XRHandJointID.IndexIntermediate, out Pose rightIndexIntermediate) &&
             leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
             rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal)
             )
         {
             float distance_indextip = leftIndexTip.position.x - rightIndexTip.position.x;
             float distance_indexintermediate = leftIndexIntermediate.position.x - rightIndexIntermediate.position.x;
             float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
             // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
             //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
             if ((distance_indextip > 0f && distance_indextip < 0.05f) &&
                 (distance_indexintermediate < -0.10f && distance_indexintermediate > -0.12f) &&
                 (distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f)
                 
             )
             {
                 return true; // Replace with actual gesture name
             }
         }
        return false;
    }

    string GetGesture() {
        // Implement your gesture recognition logic here
        // For example, you can check the positions of the joints and return a gesture name

        // The order of checking is important, so that the gesture can be detected correctly
        // TODO: can change the order by the usage frequency or detect success percentage of the gesture

        // gesture "Saru"
         if (IsGestureSaru())
         {
                return "Saru"; // Replace with actual gesture name
         }    
        
       // gesture "I"
        if (IsGestureI())
        {
                return "I"; // Replace with actual gesture name
        }

         // gesture Ne 
        if (IsGestureNe())
        {
            return "Ne"; // Replace with actual gesture name
        } 
       
        // gesture "Mi"
       if (IsGestureMi())
        {
                return "Mi"; // Replace with actual gesture name
        }

        // gesture "Tora"
       
         if (IsGestureTora())
         {
              return "Tora"; // Replace with actual gesture name
         }

/*
        // gesture "Uma"
         if (IsGestureUma()) {
                return "Uma"; // Replace with actual gesture name
         }
        */

        // Add more gesture checks as needed


        // If no gesture is detected, return "None" or an empty string
        return "None";
    }

    private TextMeshProUGUI textMesh; // TextMeshPro component for displaying the gesture name
    public float fadeDuration = 0.5f;
    public float visibleDuration = 0.5f;
    private bool fade_in_lock = false; // Lock for fade in and out


    System.Collections.IEnumerator FadeInAndOut(string gestureName)
    {
        if (fade_in_lock) yield break; // Prevent multiple coroutines from running at the same time
        fade_in_lock = true; // Lock the coroutine

        AudioManager audioManager = new AudioManager();
        audioManager.enableAudio("Audio-gesture");

        // find the word object in the scene by tag
        GameObject gestureWord = GameObject.FindGameObjectWithTag("GestureWord");

        // make the gestureWord object a child of the MainCamera object
        if (gestureWord != null && MainCamera != null)
        {
            gestureWord.transform.SetParent(MainCamera.transform);
            // set position to (0, 0, 38.5)
            gestureWord.transform.localPosition = new Vector3(40f, 30f, 38.5f); // Set the position to the anchor's position
            gestureWord.transform.localRotation = Quaternion.identity; // Set the rotation to the anchor's rotation
            
            // set the rotation to 0, 0, 0
            gestureWord.transform.localRotation = Quaternion.Euler(0, 0, 0); // Set the rotation to the anchor's rotation

            textMesh = gestureWord.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh == null)
            {
                Debug.LogError("TextMeshPro component not found on the gesture word object!");
            }
            else {
                textMesh.text = gestureName; // Set the text to the gesture name
                StartCoroutine(FadeRoutine());
            }
        }
        else
        {
            Debug.LogError("Gesture word or left hand anchor not found!");
        }
    }

    private System.Collections.IEnumerator FadeRoutine()
    {
        Color originalColor = textMesh.color;

        // Fade In
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, normalizedTime);
            yield return null;
        }
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(visibleDuration);

        // Fade Out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f - normalizedTime);
            yield return null;
        }
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        fade_in_lock = false; // Unlock the coroutine
    }

    void PrintJoints(XRHand hand)
    {
        // 26 is the total number of valid joints in Unity's XRHandJointID enum (0 to 25)
    for (int i = 0; i <= (int)XRHandJointID.LittleTip; i++)
    {
        XRHandJointID jointID = (XRHandJointID)i;
        if (jointID == XRHandJointID.Invalid)
            continue; // Skip invalid ID
        XRHandJoint joint = hand.GetJoint(jointID);

        if (joint.TryGetPose(out Pose pose))
        {
            // Store the joint pose in the appropriate dictionary
            if (hand.handedness == Handedness.Left)
            {
                leftHandJoints[jointID] = pose;
            }
            else if (hand.handedness == Handedness.Right)
            {
                rightHandJoints[jointID] = pose;
            }

            // print the position distance of left and right IndexProximal joint
            if (jointID == XRHandJointID.IndexProximal)
            {
                // count the distance of x, y and z between left and right IndexProximal joint
                // -0.17~-0.18f
                if (leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPose) &&
                    rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightPose) &&
                    rightHandJoints.TryGetValue(XRHandJointID.IndexProximal, out Pose rightPose2)
                    )
                {
                    float distanceX = (leftPose.position.x - rightPose.position.x);
                    float distanceY = (leftPose.position.y - rightPose.position.y);
                    float distanceZ = (leftPose.position.z - rightPose.position.z);
                    //Debug.Log($"Distance between left and right IndexTip: X: {distanceX}, Y: {distanceY}, Z: {distanceZ}");
                }
            }
            // Debug.Log($"{hand.handedness} {jointID}: {pose.position}");
            string gesture = GetGesture();
            if (gesture != "None")
            {
                // check if the gesture is the same as the last gesture
                if (gesture == gestureConfirmation.last_gesture)
                {
                    gestureConfirmation.gesture_count++;
                }
                else
                {
                    gestureConfirmation.gesture_count = 1;
                    gestureConfirmation.last_gesture = gesture;
                }
            }
            else
            {
                gestureConfirmation.gesture_count = 0;
            }
            if (gestureConfirmation.IsConfirmed())
            {
                // gesture is confirmed
                
                // fade in and out a word that is the same as the gesture name
                //FadeInAndOut(gesture);
                StartCoroutine(FadeInAndOut(gesture));


                //Debug.Log($"~~~~~~~~~~~~Gesture confirmed: {gesture}");
                // reset the gesture confirmation object
                gestureConfirmation.Reset();

                // pop the top element of the gesture array if the gesture is detected and the gesture is same as the current gesture's top element
                for (int j = 0; j < ninjutsuGestures.Length; j++)
                {
                    if (gesture == ninjutsuGestures[j].current_gestures[0])
                    {
                        // remove the top element of the gesture array
                        for (int k = 0; k < ninjutsuGestures[j].current_gestures.Length - 1; k++)
                        {
                            ninjutsuGestures[j].current_gestures[k] = ninjutsuGestures[j].current_gestures[k + 1];
                        }
                        ninjutsuGestures[j].current_gestures[ninjutsuGestures[j].current_gestures.Length - 1] = "None";
                        //Debug.Log($"~~~~~~~~~~~~Gesture detected: {gesture}");
                    }
                }
             
                /*
                // print the current gesture array
                for (int j = 0; j < ninjutsuGestures.Length; j++)
                {
                    string current_gestures = string.Join(", ", ninjutsuGestures[j].current_gestures);
                    Debug.Log($"~~~~~~~~~~~~Current gesture array: {current_gestures}");
                }
                */

                bool resetGesture = false;
                // check if the ninjutsu gesture is completed
                for (int j = 0; j < ninjutsuGestures.Length; j++)
                {
                    if (ninjutsuGestures[j].current_gestures[0] == "None")
                    {
                        // the ninjutsu gesture is completed
                        ninjutsuGestures[j].ActivateNinjutsu();
                        // reset the gesture array
                        resetGesture = true;
                        // set the ninjutsu timeout count
                        ninjutsu_timeout_count = ninjutsu_timeout;
                        // set the chakra cost
                        chakra -= ninjutsuGestures[j].chakra_cost;
                    }
                }
                // if the gesture is completed, reset the gesture array
                if (resetGesture)
                {
                    // reset the gesture array
                    for (int j = 0; j < ninjutsuGestures.Length; j++)
                    {
                        // copy by value, not by reference
                        ninjutsuGestures[j].current_gestures = (string[])ninjutsuGestures[j].gestures.Clone();
                    }
                }
                //Debug.Log($"~~~~~~~~~~~~Gesture detected: {gesture}");
            }
        }
    }
    }
}
