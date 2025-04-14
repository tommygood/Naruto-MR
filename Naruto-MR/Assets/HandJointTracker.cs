using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;
using UnityEngine.XR.Management;
using UnityEngine.SubsystemsImplementation;

public class HandJointTracker : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    // track each hand's joints position and rotation
    private Dictionary<XRHandJointID, Pose> leftHandJoints = new Dictionary<XRHandJointID, Pose>();
    private Dictionary<XRHandJointID, Pose> rightHandJoints = new Dictionary<XRHandJointID, Pose>();

    void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
        }
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
            leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
            rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal)
            )
        {
            float distance_indexdistal = leftIndexDistal.position.x - rightIndexDistal.position.x;
            float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_indexdistal > 0f && distance_indexdistal < 0.05f) &&
                (distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f)
                
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
           leftHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose leftIndexTip) &&
           rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightIndexTip) &&
           leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
           rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal) &&
           leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPalm) &&
           rightHandJoints.TryGetValue(XRHandJointID.Palm, out Pose rightPalm)
           )
       {
            float distance_middletip = leftMiddleTip.position.x - rightMiddleTip.position.x;
            float distance_indextip = leftIndexTip.position.x - rightIndexTip.position.x;
            float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
            float distance_palm = leftPalm.position.y - rightPalm.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_middletip > 0f && distance_middletip < 0.05f) &&
                (distance_indextip < -0.10f && distance_indextip > -0.12f) &&
                (distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f) &&
                (distance_palm < -0.10f && distance_palm > -0.12f)
                
            )
            {
                return true; // Replace with actual gesture name
            }
        }
        return false;
    }

    bool IsGestureI() {
        // check the distance between left and right ThumbMetacarpal joint
       // check the rotation of left and right ThumbMetacarpal joint

        if (leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
            rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal)
            )
        {
            float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f)
                
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
       if (leftHandJoints.TryGetValue(XRHandJointID.MiddleTip, out Pose leftMiddleTip) &&
           rightHandJoints.TryGetValue(XRHandJointID.MiddleTip, out Pose rightMiddleTip) &&
           leftHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose leftIndexTip) &&
           rightHandJoints.TryGetValue(XRHandJointID.IndexTip, out Pose rightIndexTip) &&
           leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftThumbMetacarpal) &&
           rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightThumbMetacarpal) &&
           leftHandJoints.TryGetValue(XRHandJointID.Palm, out Pose leftPalm) &&
           rightHandJoints.TryGetValue(XRHandJointID.Palm, out Pose rightPalm)
           )
       {
            float distance_middletip = leftMiddleTip.position.x - rightMiddleTip.position.x;
            float distance_indextip = leftIndexTip.position.x - rightIndexTip.position.x;
            float distance_thumbmetacarpal = leftThumbMetacarpal.position.y - rightThumbMetacarpal.position.y;
            float distance_palm = leftPalm.position.y - rightPalm.position.y;
            // Debug.Log($"Distance between left and right IndexTip: {distance_indextip}");
            //Debug.Log($"Distance between left and right ThumbMetacarpal: {distance_thumbmetacarpal}");
            if ((distance_middletip > 0f && distance_middletip < 0.05f) &&
                (distance_indextip < -0.10f && distance_indextip > -0.12f) &&
                (distance_thumbmetacarpal < -0.10f && distance_thumbmetacarpal > -0.12f) &&
                (distance_palm < -0.10f && distance_palm > -0.12f)
                
            )
            {
                return true; // Replace with actual gesture name
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

        // gesture Ne 
        if (IsGestureNe())
        {
            return "Ne"; // Replace with actual gesture name
        }

       // gesture "Tora"
       /*
         if (IsGestureTora())
         {
              return "Tora"; // Replace with actual gesture name
         }
       
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
       

        // gesture "Mi"
       if (IsGestureMi())
        {
                return "Mi"; // Replace with actual gesture name
        }

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
                if (leftHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose leftPose) &&
                    rightHandJoints.TryGetValue(XRHandJointID.ThumbMetacarpal, out Pose rightPose))
                {
                    float distanceX = (leftPose.position.x - rightPose.position.x);
                    float distanceY = (leftPose.position.y - rightPose.position.y);
                    float distanceZ = (leftPose.position.z - rightPose.position.z);
                    Debug.Log($"Distance between left and right IndexTip: X: {distanceX}, Y: {distanceY}, Z: {distanceZ}");
                }
            }
            // Debug.Log($"{hand.handedness} {jointID}: {pose.position}");
            string gesture = GetGesture();
            if (gesture != "None")
            {
                Debug.Log($"~~~~~~~~~~~~Gesture detected: {gesture}");
            }
        }
    }
    }
}
