using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSensitiveActionIndicators : MonoBehaviour {

    private GameObject[] ArrowIndicators = new GameObject[4];

    public float ArrowMinOffset = 0.7f;
    public float ArrowMaxOffset = 1.0f;
    public float ArrowPulseTime = 1.4f;
    private float ArrowAnimTime = 0.0f;
    private float ArrowDir = 1.0f;

    bool Enabled = false;
    bool[] ArrowsEnabled = { false, false, false, false };
    int ArrowSelected = -1;

    private Student student = null;    

	// Use this for initialization
	void Start () {
        //Student s = Student.FindNeareastStudentInHierarchy(gameObject);
        student = gameObject.FindNeareastComponentInHierarchy<Student>();
        if (student != null)
        {
            foreach (var cs in student.Students)
            {
                if (cs.Direction < PassDirection.NONE)
                {
                    int idir = (int)cs.Direction;
                    ArrowsEnabled[idir] = true;
                }
            }
        }

        for (int i=0; i < 4; ++i)
        {
            ArrowIndicators[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }
	
    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            ArrowAnimTime = 0;
            ArrowDir = 1.0f;
        }

        Enabled = enabled;
    }

	// Update is called once per frame
	void Update () {        
        AnimateArrows();
    }

    private void AnimateArrows()
    {
        if (ArrowSelected > -1)
        {

        }
        else
        {
            float arrowOffset = QuadraticInOut(ArrowAnimTime, ArrowMinOffset, ArrowMaxOffset - ArrowMinOffset, ArrowPulseTime);
            for (int i=0; i < 4; ++i)
            {
                ArrowIndicators[i].SetActive(Enabled && ArrowsEnabled[i]);
                ArrowIndicators[i].gameObject.transform.localPosition = Student.DirectionVectors[i] * arrowOffset;
            }            
        }

        ArrowAnimTime += Time.deltaTime * ArrowDir;
        if (ArrowAnimTime > ArrowPulseTime)
        {
            ArrowAnimTime = ArrowPulseTime;
            ArrowDir = -1.0f;
        }
        else if (ArrowAnimTime < 0)
        {
            ArrowDir = 1.0f;
            ArrowAnimTime = 0.0f;
        }        
    }

    private float QuadraticInOut(float t, float b, float c, float d)
    {
        // t - current time
        // d - anim duration
        // c - change in duration
        // b - start value

        t /= d / 2;
        if (t < 1) return c / 2 * t * t + b;
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b;
    }
}
