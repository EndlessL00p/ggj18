using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherLook : MonoBehaviour {
    
    public Sprite Hair = null;
    public Sprite Head = null;
    public Sprite Arm = null;
    public Sprite Face = null;
    public Sprite Fringe = null;

    //private Student teacher = null;
    //public Student MyStudent
    //{
    //    get
    //    {
    //        if (teacher) return teacher;
    //        return gameObject.GetComponent<Student>();
    //    }
    //}

    private SpriteRenderer _HeadSprite = null;
    private SpriteRenderer _LArmSprite = null;
    private SpriteRenderer _RArmSprite = null;
    private SpriteRenderer _FringeSprite = null;
    private SpriteRenderer _FaceSprite = null;
    //private SpriteRenderer _HairSprite = null;

    // Use this for initialization
    void Start () {
    //    teacher = gameObject.GetComponent<Student>();

        _HeadSprite = gameObject.transform.Find("Torso/Head").GetComponent<SpriteRenderer>();
        _FaceSprite = gameObject.transform.Find("Torso/Head/Face").GetComponent<SpriteRenderer>();
        _LArmSprite = gameObject.transform.Find("Torso/ArmLeft").GetComponent<SpriteRenderer>();
        _RArmSprite = gameObject.transform.Find("Torso/ArmRight").GetComponent<SpriteRenderer>();
        _FringeSprite = gameObject.transform.Find("Torso/Head/Fringe").GetComponent<SpriteRenderer>();
        //    _HairSprite = gameObject.transform.Find("Torso/Head/Hair").GetComponent<SpriteRenderer>();

        RandomizeAppearance();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RandomizeAppearance()
    {
        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        if (db != null)
        {
            SkinSet skin = db.GetRandomSkinSet();
            Head = skin.HeadRear;
            Arm = skin.Arm;
            Fringe = db.GetRandomTeacherHair();
            Face = db.GetRandomTeacherFace();
            //Hair = db.GetRandomHair();
        }

        ApplyLook();
    }

    public void ApplyLook()
    {
        if (Head != null && _HeadSprite != null) _HeadSprite.sprite = Head;
        if (Hair != null && _FringeSprite != null) _FringeSprite.sprite = Fringe;
        if (Hair != null && _FaceSprite != null) _FaceSprite.sprite = Face;
        if (Arm != null && _LArmSprite != null) _LArmSprite.sprite = Arm;
        if (Arm != null && _RArmSprite != null) _RArmSprite.sprite = Arm;
    }
}
