using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkinSet
{
    public Sprite HeadRear;
    public Sprite HeadFace;
    public Sprite Arm;    
}

public class LookDatabase : MonoBehaviour {

    public List<SkinSet> SkinTypes = new List<SkinSet>();
    public List<Sprite> ShortHair = new List<Sprite>();
    public List<Sprite> LongHair = new List<Sprite>();
    public List<Sprite> TeacherHair = new List<Sprite>();
    public List<Sprite> TeacherFace = new List<Sprite>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public SkinSet GetRandomSkinSet()
    {
        int i = Random.RandomRange(0, SkinTypes.Count);
        return SkinTypes[i];
    }

    public Sprite GetRandomHair()
    {
        List<Sprite> hairset = ShortHair;
        if (Random.RandomRange(0,6) >= 3)
        {
            hairset = LongHair;
        }

        int i = Random.RandomRange(0, hairset.Count);
        return hairset[i];
    }
    public Sprite GetRandomTeacherHair()
    {
        int i = Random.RandomRange(0, TeacherHair.Count);
        return TeacherHair[i];
    }

    public Sprite GetRandomTeacherFace()
    {
        int i = Random.RandomRange(0, TeacherFace.Count);
        return TeacherFace[i];
    }
}
