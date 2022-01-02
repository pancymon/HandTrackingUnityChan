using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public class HandToAnimation : MonoBehaviour
{
    public GameObject[] spheres;
    public Transform[] leftHandParts;
    public GameObject leftHand;

    public Vector3 MarkToVec3(NormalizedLandmark mark)
    {
        return new Vector3(mark.X,mark.Y,mark.Z);
    }

    public void Render(IList<Mediapipe.NormalizedLandmarkList> list)
    {
        int count = 0;
        Debug.LogWarning("hand point start");

      foreach(var d in list)
      {
        Debug.LogWarning("count of list: "+list.Count);

        foreach(var dd in d.Landmark)
        {  
          TriggerAnimation(d.Landmark);      
          Debug.LogWarning("count of marks: "+d.Landmark.Count);
          Debug.Log("x: "+dd.X+" y: "+dd.Y+" z: "+dd.Z);
          spheres[count].transform.position = new Vector3(dd.X,dd.Y,dd.Z);
          count++;
        }
      }

        Debug.LogWarning("hand point end");
    }

    public void TriggerAnimation(Google.Protobuf.Collections.RepeatedField<Mediapipe.NormalizedLandmark> marks)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < marks.Count; i++)
        {
            points.Add(MarkToVec3(marks[i]));
        }

        //int fingerRoot = 1;
        //for (int i = 0; i < 5; i++)
        //{
        //    var rootDirection = points[fingerRoot] - points[0];
        //    for (int j = 0; j < 3; j++)
        //    {
        //        var fingerDirection = points[fingerRoot + j + 1] - points[fingerRoot];
        //        var rotation = Quaternion.FromToRotation(rootDirection, fingerDirection);
        //        leftHandParts[i*3 + j].rotation = rotation;
        //    }
        //    fingerRoot += 4;
        //}

        var xaxis = (points[0] - points[9]).normalized;
        var yaxis = (points[5] - points[9]).normalized;
        var zaxis = Vector3.Cross(xaxis, yaxis).normalized;

        //transform.LookAt(zaxis,yaxis);

        transform.rotation = Quaternion.LookRotation(zaxis,yaxis);

        int fingerRoot = 1;

        // 5 fingers per hand.
        for (int i = 0; i < 5; i++)
        {
            var rootDirection = points[fingerRoot] - points[0];

            // Finger parts.
            for (int j = 0; j < 1; j++)
            {
                var fingerDirection = points[fingerRoot + j + 1] - points[fingerRoot+j];

                var yPlane = Vector3.ProjectOnPlane(fingerDirection, yaxis);
                var zPlane = Vector3.ProjectOnPlane(fingerDirection, zaxis);

                var yAngle = Vector3.Angle(xaxis, yPlane);
                var zAngle = Vector3.Angle(xaxis, zPlane);

                leftHandParts[i * 3 + j].localEulerAngles = new Vector3(0, yAngle, zAngle);
            }
            fingerRoot += 4;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
