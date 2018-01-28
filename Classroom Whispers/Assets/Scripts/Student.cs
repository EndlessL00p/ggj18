using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PassDirection
{
    LEFT = 0,
    RIGHT,
    UP,
    DOWN,
    NONE
}

[System.Serializable]
public class ConnectedStudent
{
    public PassDirection Direction;
    public GameObject Student;
}

public class Student : MonoBehaviour
{
    private string[] PassAnimations = { "ReachLeft", "ReachRight", "ReachUp", "ReachBack", "Idle" };
    private PassDirection[] OppositeDirection = { PassDirection.RIGHT, PassDirection.LEFT, PassDirection.DOWN, PassDirection.UP };
    public static Vector3[] DirectionVectors = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    //public ArrayList<string> test;
    public List<ConnectedStudent> Students = new List<ConnectedStudent>();

    public bool isStartStudent;
    public bool isEndStudent;

    private bool isHoldingNote;

    public GameObject NoteObject = null;

    string lastTrigger = "";
    const float idleResetTimer = 0.5f;
    float animTimer = 0.0f;

    private Animator anim = null;

    private ContextSensitiveActionIndicators indicators = null;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        indicators = gameObject.FindNeareastComponentInHierarchy<ContextSensitiveActionIndicators>();

        NoteObject = gameObject.transform.GetChild(1).gameObject; // todo rewrite
        if (NoteObject != null)
        {
            if (isStartStudent == true && isEndStudent == false)
            {
                isHoldingNote = true;
                NoteObject.SetActive(true);
                indicators.SetHasPlayerFocus(true);
            }
            else
            {
                isHoldingNote = false;
                NoteObject.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        anim.ResetTrigger("Idle");
        
        if (isHoldingNote == true)
        {
            NoteObject.SetActive(true);            
        }
        else
        {
            NoteObject.SetActive(false);
        }

        if (animTimer > 0)
        {
            animTimer -= Time.deltaTime;
        }
        else if (animTimer < 0)
        {
            if (lastTrigger != "")
            {
                anim.ResetTrigger(lastTrigger);
                lastTrigger = "";
            }
            anim.SetTrigger("Idle");
            animTimer = 0;
        }        
    }


    ///////////////////////
    ///Getter and Setter///
    ///////////////////////

    public bool IsHoldingNote
    {
        set { isHoldingNote = value; }
        get { return isHoldingNote; }
    }

    public bool IsEndStudent
    {
        set { isEndStudent = value; }
        get { return isEndStudent; }
    }

    //pass note
    public void PassNote(PassDirection a_Direction)
    {
        if (isHoldingNote != true)
            return;

        Student s = GetConnectedStudent(a_Direction);

        if (s != null)
        {
            PlayPassAnimation(a_Direction);
            isHoldingNote = false;
            s.RecieveNote(this, a_Direction);

            indicators.SetHasPlayerFocus(false);

            AudioManager.Instance.PlayActionSFX("PassNote");
        }
    }

    public void RecieveNote(Student a_PassingStudent, PassDirection a_Direction)
    {
        PassDirection oppDir = OppositeDirection[(int)a_Direction];
        PlayPassAnimation(oppDir);
        IsHoldingNote = true;

        indicators.SetHasPlayerFocus(true);
    }    

    private void PlayPassAnimation(PassDirection a_Direction)
    {
        if (lastTrigger != "")
        {
            anim.ResetTrigger(lastTrigger);
            anim.SetTrigger("Idle"); // need to pass through idle
            lastTrigger = "";
        }

        int idir = (int)a_Direction;
        string animTrigger = PassAnimations[idir];
        
        anim.SetTrigger(animTrigger);
        animTimer = idleResetTimer;
        lastTrigger = animTrigger;
    }

    //Get connected student
    private Student GetConnectedStudent(PassDirection a_Direction)
    {
        foreach(ConnectedStudent cs in this.Students)
        {
            if (cs.Direction == a_Direction)
            {
                return cs.Student.GetComponent<Student>();
            }
        }

        return null;
    }

    //Show linked students
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (ConnectedStudent cs in Students)
        {
            if (cs.Student != null)
            {
                Vector3 dir = cs.Student.gameObject.transform.position - this.gameObject.transform.position;
                Gizmos.DrawRay(this.gameObject.transform.position, dir);
            }
        }
    }
    
}

public static class GameObjectExtensions
{
    public static T FindNeareastComponentInHierarchy<T>(this GameObject obj) where T: MonoBehaviour
    {
        Transform target = obj.transform;
        while (target != null)
        {
            T s = target.GetComponentInChildren<T>();
            if (s != null)
            {
                return s;
            }

            if (target != obj.transform.root)
            {
                target = target.parent;
            }
            else
            {
                break;
            }
        }

        return default(T);
    }
}

