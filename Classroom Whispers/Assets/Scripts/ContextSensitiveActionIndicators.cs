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
    public float SmallArrowScale = 0.8f;
    public float LargeArrowScale = 1.0f;

    bool HasPlayerFocus = false;
    bool[] ArrowsEnabled = { false, false, false, false };
    string[] ArrowIds = { "Arrow_L", "Arrow_R", "Arrow_U", "Arrow_D" };
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
            ArrowIndicators[i] = gameObject.transform.Find(ArrowIds[i]).gameObject;
        }
    }
	
    public void SetHasPlayerFocus(bool enabled)
    {
        if (enabled)
        {
            ArrowAnimTime = 0;
            ArrowDir = 1.0f;
        }

        HasPlayerFocus = enabled;
    }

	// Update is called once per frame
	void Update () {        
        AnimateArrows();
    }

    private void AnimateArrows()
    {
        if (!HasPlayerFocus)
        {
            // static arrows, is not player focus
            ArrowAnimTime = 0.0f;
            for (int i = 0; i < 4; ++i)
            {
                Vector3 scale = ArrowIndicators[i].transform.localScale;
                scale.x = SmallArrowScale;
                scale.y = SmallArrowScale;
                ArrowIndicators[i].transform.localScale = scale;
                ArrowIndicators[i].SetActive(ArrowsEnabled[i]);

                ArrowIndicators[i].gameObject.transform.localPosition = Student.DirectionVectors[i] * ArrowMinOffset;
            }
        }
        else
        {
            if (ArrowSelected > -1)
            {
                // playerholder for sporty / confirmable directions

            }
            else
            {
                // animating arrows, player has focus
                float arrowOffset = QuadraticInOut(ArrowAnimTime, ArrowMinOffset, ArrowMaxOffset - ArrowMinOffset, ArrowPulseTime);
                for (int i = 0; i < 4; ++i)
                {
                    Vector3 scale = ArrowIndicators[i].transform.localScale;
                    scale.x = LargeArrowScale;
                    scale.y = LargeArrowScale;
                    ArrowIndicators[i].transform.localScale = scale;

                    ArrowIndicators[i].SetActive(ArrowsEnabled[i]);
                    ArrowIndicators[i].gameObject.transform.localPosition = Student.DirectionVectors[i] * arrowOffset;
                }
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
