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
          spheres[count].transform.localPosition = new Vector3(dd.X,dd.Y,dd.Z);
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

        int fingerRoot = 1;
        for (int i = 0; i < 5; i++)
        {
            var rootDirection = points[fingerRoot] - points[0];
            for (int j = 0; j < 3; j++)
            {
                var fingerDirection = points[fingerRoot + j + 1] - points[fingerRoot];

                //var rotation = Quaternion.FromToRotation(rootDirection, fingerDirection);
                //leftHandParts[i * 3 + j].rotation = rotation;

                var angle = Vector3.Angle(rootDirection,fingerDirection);
                if (i == 0)
                {
                    leftHandParts[i * 3 + j].localEulerAngles = new Vector3(0, 0,angle);
                }
                else
                {
                    leftHandParts[i * 3 + j].localEulerAngles = new Vector3(0, angle, 0);
                }

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
