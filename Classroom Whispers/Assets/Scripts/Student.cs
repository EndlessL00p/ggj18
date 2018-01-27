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
    public static Vector3[] DirectionVectors = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    //public ArrayList<string> test;
    public List<ConnectedStudent> Students = new List<ConnectedStudent>();


    public bool isStartStudent;
    public bool isEndStudent;

    private bool isHoldingNote;

    private GameObject noteObj;

    // Use this for initialization
    void Start ()
    {
        noteObj = gameObject.transform.GetChild(0).gameObject;

        if (isStartStudent == true && isEndStudent == false)
        {
            isHoldingNote = true;
            noteObj.SetActive(true);
        }
        else
        {
            isHoldingNote = false;
            noteObj.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isHoldingNote == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Student s = GetConnectedStudent(PassDirection.LEFT);

                if (s != null)
                {


                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {

            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {

            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {

            }
        }

	}

    public bool IsHoldingNote
    {
        get { return isHoldingNote; }
    }

    private Student GetConnectedStudent(PassDirection a_Direction)
    {
        //this.Students.Where(x => x.Direction == a_Direction).FirstOrDefault();

        foreach(ConnectedStudent cs in this.Students)
        {
            if (cs.Direction == a_Direction)
            {
                return cs.Student.GetComponent<Student>();
            }
        }

        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var cs in Students)
        {
            if (cs.Student != null)
            {
                Vector3 dir = cs.Student.gameObject.transform.position - this.gameObject.transform.position;
                Gizmos.DrawRay(this.gameObject.transform.position, dir);
            }
        }
    }
}
