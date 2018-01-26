﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassDirection
{
    LEFT = 0,
    RIGHT,
    UP,
    DOWN
}

[System.Serializable]
public class ConnectedStudent
{
    public PassDirection Direction;
    public GameObject Student;
}

public class Student : MonoBehaviour
{
    private Vector3[] DirectionVectors = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };

    //public ArrayList<string> test;
    public List<ConnectedStudent> Students = new List<ConnectedStudent>();

	// Use this for initialization
	void Start ()
    {
        //test = new ArrayList<string>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
