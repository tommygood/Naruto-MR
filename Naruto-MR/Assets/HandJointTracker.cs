using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;
using UnityEngine.XR.Management;
using UnityEngine.SubsystemsImplementation;

public class NinjutsuGesture
{
    public string name; // Name of the ninjutsu
    public string atrribute; // Attribute of the ninjutsu
    public string[] gestures; // Array of gesture names
    public string[] current_gestures; // Array of current gesture names
}

public class HandJointTracker : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    // track each hand's joints position and rotation
    private Dictionary<XRHandJointID, Pose> leftHandJoints = new Dictionary<XRHandJointID, Pose>();
    private Dictionary<XRHandJointID, Pose> rightHandJoints = new Dictionary<XRHandJointID, Pose>();

    private NinjutsuGesture[] ninjutsuGestures; // Array of Ninjutsu gestures

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
        // initialize the ninjutsu gesture
        ninjutsuGestures = new NinjutsuGesture[]
        {
            new NinjutsuGesture { name = "fireball", atrribute = "fire", gestures = new string[] { "Mi", "Saru", "I" }, current_gestures = new string[] { "Mi", "Saru", "I" } },
            new NinjutsuGesture { name = "waterfall", atrribute = "water", gestures = new string[] { "Tora", "Saru", "Ne", "I" }, current_gestures =  new string[] { "Tora", "Saru", "Ne", "I" } },
        };
        
    }

    void Update()
    {
        if (handSubsystem == null || !handSubsystem.running)
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
            float distance_indextip = leftLittleIntermediate.position.y - rightIndexIntermediate.position.y;
            
            //Debug.Log($"QQ Distance between left and right IndexTip: {distance_indextip}, {((distance_indextip > -0.09f && distance_indextip < 0f) ? "true" : "false")}");
            //Debug.Log($"QQ Distance between left and right MiddleTip: {distance_middletip}, {((distance_middletip > -0.17f && distance_middletip < -0.15f) ? "true" : "false")}");
            // print the condition is matched or not
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_middletip > 0.07f && distance_middletip < 0.1f) &&
                (distance_indextip > -0.09f && distance_indextip < 0f) 
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
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((leftPalmRotation.eulerAngles.x > 50f && leftPalmRotation.eulerAngles.x < 65f) &&
                (rightPalmRotation.eulerAngles.x > 50f && rightPalmRotation.eulerAngles.x < 65f) &&
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
            rightHandJoints.TryGetValue(XRHandJointID.IndexProximal, out Pose rightIndexProximal)
           )
       {
            float distance_indexdistal = leftIndexDistal.position.x - rightIndexDistal.position.x;
            float distance_indextip = rightIndexProximal.position.y - rightIndexTip.position.y;
            float distance_indextip_z = rightIndexProximal.position.z - rightIndexTip.position.z;
            //Debug.Log($"Distance between left and right IndexTip: {distance_indextip}, {((distance_indextip > 0.01f && distance_indextip < 0.03f) ? "true" : "false")}");
            //Debug.Log($"Distance between left and right IndexDistal: {distance_indexdistal}, {((distance_indexdistal > 0.06f && distance_indexdistal < 0.085f) ? "true" : "false")}");
            //Debug.Log($"Distance between left and right IndexTip Z: {distance_indextip_z}, {((distance_indextip_z > -0.01f && distance_indextip_z < 0f) ? "true" : "false")}");
            if ((distance_indexdistal > 0.06f && distance_indexdistal < 0.085f) &&
                (distance_indextip > 0.01f && distance_indextip < 0.03f) &&
                (distance_indextip_z > -0.01f && distance_indextip_z < 0f)
                
            )
            {
               return true;
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

        // gesture Ne 
        if (IsGestureNe())
        {
            return "Ne"; // Replace with actual gesture name
        }       

        
       // gesture "I"
        if (IsGestureI())
        {
                return "I"; // Replace with actual gesture name
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
                        ninjutsuGestures[j].current_gestures[ninjutsuGestures[j].current_gestures.Length - 1] = null;
                        //Debug.Log($"~~~~~~~~~~~~Gesture detected: {gesture}");
                    }
                }

                // check if the ninjutsu gesture is completed
                for (int j = 0; j < ninjutsuGestures.Length; j++)
                {
                    if (ninjutsuGestures[j].gestures[0] == null)
                    {
                        // the ninjutsu gesture is completed
                        Debug.Log($"~~~~~~~~~~~~Ninjutsu gesture completed: {ninjutsuGestures[j].name}");
                        // reset the gesture array
                        ninjutsuGestures[j].current_gestures = ninjutsuGestures[j].gestures;
                    }
                }
                Debug.Log($"~~~~~~~~~~~~Gesture detected: {gesture}");
            }
        }
    }
    }
}
