using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Classroom : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Classroom.RandomizeStudentAppearances();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void RandomizeStudentAppearances()
    {
        var looks = GameObject.FindObjectsOfType<StudentLook>();
        foreach (var sl in looks)
        {
            if (sl.MyStudent.IsEndStudent || sl.MyStudent.isStartStudent)
                continue;

            sl.RandomizeAppearance();
        }
    }
}