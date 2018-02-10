using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentLook : MonoBehaviour {
    
    public Sprite Hair = null;
    public Sprite Head = null;
    public Sprite Arm = null;
    public Sprite Face = null;

    private Student student = null;
    public Student MyStudent
    {
        get
        {
            if (student) return student;
            return gameObject.GetComponent<Student>();
        }
    }

    private SpriteRenderer _HeadSprite = null;
    private SpriteRenderer _LArmSprite = null;
    private SpriteRenderer _RArmSprite = null;
    private SpriteRenderer _HairSprite = null;

    // Use this for initialization
    void Start () {
        student = gameObject.GetComponent<Student>();

        _HeadSprite = gameObject.transform.Find("Torso/Head").GetComponent<SpriteRenderer>();
        _HairSprite = gameObject.transform.Find("Torso/Head/Hair").GetComponent<SpriteRenderer>();
        _LArmSprite = gameObject.transform.Find("Torso/ArmLeft").GetComponent<SpriteRenderer>();
        _RArmSprite = gameObject.transform.Find("Torso/ArmRight").GetComponent<SpriteRenderer>();

        if (student.IsEndStudent || student.isStartStudent)
            LoadAppearancePrefs();
        else
            RandomizeAppearance();

        ApplyLook();
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

            Hair = db.GetRandomHair();
        }

        ApplyLook();
    }

    public void ApplyLook()
    {
        if (Head != null && _HeadSprite != null) _HeadSprite.sprite = Head;
        if (Hair != null && _HairSprite != null) _HairSprite.sprite = Hair;
        if (Arm != null && _LArmSprite != null) _LArmSprite.sprite = Arm;
        if (Arm != null && _RArmSprite != null) _RArmSprite.sprite = Arm;
    }

    private void LoadAppearancePrefs()
    {
        CharacterCustomizer.CharacterType Type = MyStudent.IsEndStudent ? CharacterCustomizer.CharacterType.Crush : CharacterCustomizer.CharacterType.Player;
        int iType = (int)Type;
        int HeadIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Head{0}", iType));
        int FaceIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Face{0}", iType));
        int FringeIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Fringe{0}", iType));
        //int GlassesIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Glasses{0}", iType));

        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        if (db != null)
        {
            Hair = db.HairTypes[FringeIdx].Hair;
            Head = db.SkinTypes[HeadIdx].HeadRear;
            Arm = db.SkinTypes[HeadIdx].Arm;
        }
    }
}
