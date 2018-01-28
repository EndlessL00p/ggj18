using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Classroom))]
public class ClassroomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Classroom myScript = target as Classroom;
        if (GUILayout.Button("Generate Student Links"))
        {
            GenerateStudentLinks();
        }

        if (GUILayout.Button("Fix Student Names"))
        {
            FixStudentNames();
        }

        if (GUILayout.Button("Randomize Student Appearances"))
        {
            Classroom.RandomizeStudentAppearances();
        }
    }

    private void ResetStudentLinks()
    {
        var students = GameObject.FindObjectsOfType<Student>();
        foreach (var s in students)
        {
            s.Students.Clear();
        }
    }

    private void FixStudentNames()
    {
        var students = GameObject.FindObjectsOfType<Student>();
        foreach (var s in students)
        {
            s.gameObject.name = s.gameObject.transform.parent.gameObject.name + "_Student";
            EditorUtility.SetDirty(s);
        }
    }

    

    private void GenerateStudentLinks()
    {
        PassDirection pd;
        float dist;

        var students = GameObject.FindObjectsOfType<Student>();
        foreach (var s in students)
        {
            float[] dists = { float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue };
            GameObject[] links = { null, null, null, null };

            foreach (var t in students)
            {
                if (t == s)
                {
                    continue;
                }

                GetDirectionAndDistanceToOtherStudent(s, t, out pd, out dist);
                if (pd != PassDirection.NONE)
                {
                    int i = (int)pd;
                    if (dist < dists[i])
                    {
                        dists[i] = dist;
                        links[i] = t.gameObject;
                    }
                }
            }

            s.Students.Clear();
            for (int j = 0; j < 4; j++)
            {
                if (links[j] != null)
                {
                    ConnectedStudent cs = new ConnectedStudent();
                    cs.Direction = (PassDirection)j;
                    cs.Student = links[j];
                    s.Students.Add(cs);
                }
            }
            EditorUtility.SetDirty(s);
        }
    }

    private void GetDirectionAndDistanceToOtherStudent(Student s1, Student s2, out PassDirection passDirection, out float distance)
    {
        var dir = s2.gameObject.transform.position - s1.gameObject.transform.position;
        distance = dir.magnitude;
        dir = dir.normalized;

        float biggestD = -1.0f;
        passDirection = PassDirection.NONE;
        foreach (PassDirection pd in System.Enum.GetValues(typeof(PassDirection)))
        {
            if (pd >= PassDirection.NONE) continue;

            int i = (int)pd;
            float d = Vector3.Dot(dir, Student.DirectionVectors[i]);
            if (d > biggestD)
            {
                biggestD = d;
                passDirection = pd;
            }
        }
    }
}
