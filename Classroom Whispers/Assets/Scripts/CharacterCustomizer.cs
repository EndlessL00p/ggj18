using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterCustomizer : MonoBehaviour
{
    public enum CharacterType
    {
        Player,
        Crush
    }

    public CharacterType Type;

    private Image _Head = null;
    private Image _Face = null;
    private Image _Fringe = null;
    private Image _Glasses = null;

    private int _HeadIdx = 0;
    private int _FaceIdx = 0;
    private int _FringeIdx = 0;
    private int _GlassesIdx = 0;

    // Use this for initialization
    void Start()
    {
        _Head = gameObject.transform.Find("Customisation_Rig/UI_Char_Head").GetComponent<Image>();
        _Face = gameObject.transform.Find("Customisation_Rig/UI_Char_Face").GetComponent<Image>();
        _Fringe = gameObject.transform.Find("Customisation_Rig/UI_Char_Fringe").GetComponent<Image>();
        _Glasses = gameObject.transform.Find("Customisation_Rig/UI_Char_Glasses").GetComponent<Image>();

        LoadPrefs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Handles wrapping negative indexes around the end of the list
    private int NextIndex(int i, int cnt)
    {
        return (i % cnt + cnt) % cnt;
    }

    public void SavePrefs()
    {
        UnityEngine.PlayerPrefs.SetInt(string.Format("Custom_Head{0}", (int)Type), _HeadIdx);
        UnityEngine.PlayerPrefs.SetInt(string.Format("Custom_Face{0}", (int)Type), _FaceIdx);
        UnityEngine.PlayerPrefs.SetInt(string.Format("Custom_Fringe{0}", (int)Type), _FringeIdx);
        UnityEngine.PlayerPrefs.SetInt(string.Format("Custom_Glasses{0}", (int)Type), _GlassesIdx);
    }

    private void LoadPrefs()
    {
        _HeadIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Head{0}", (int)Type));
        _FaceIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Face{0}", (int)Type));
        _FringeIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Fringe{0}", (int)Type));
        _GlassesIdx = UnityEngine.PlayerPrefs.GetInt(string.Format("Custom_Glasses{0}", (int)Type));

        SetFringe(_FringeIdx);
        SetSkin(_HeadIdx);
        SetFace(_FaceIdx);
        SetGlasses(_GlassesIdx);
    }

    public void NextHair()
    {
        SetFringe(_FringeIdx + 1);
        SavePrefs();
    }

    public void PrevHair()
    {
        SetFringe(_FringeIdx - 1);
        SavePrefs();
    }

    private void SetFringe(int i)
    {
        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        int cnt = db.HairTypes.Count;
        _FringeIdx = NextIndex(i, cnt);
        var hair = db.HairTypes[_FringeIdx];

        _Fringe.sprite = hair.Fringe;        
    }

    public void NextSkin()
    {
        SetFringe(_HeadIdx + 1);
        SavePrefs();
    }

    public void PrevSkin()
    {
        SetFringe(_HeadIdx - 1);
        SavePrefs();
    }

    private void SetSkin(int i)
    {
        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        int cnt = db.SkinTypes.Count;
        _HeadIdx = NextIndex(i, cnt);
        var skin = db.SkinTypes[_HeadIdx];

        _Head.sprite = skin.HeadFace;        
    }

    public void NextFace()
    {
        SetFace(_FaceIdx + 1);
        SavePrefs();
    }

    public void PrevFace()
    {
        SetFace(_FaceIdx - 1);
        SavePrefs();
    }

    private void SetFace(int i)
    {
        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        int cnt = db.StudentFace.Count;
        _FaceIdx = NextIndex(i, cnt);
        var face = db.StudentFace[_FaceIdx];

        _Face.sprite = face;
    }

    public void NextGlasses()
    {
        SetGlasses(_GlassesIdx + 1);
        SavePrefs();
    }

    public void PrevGlasses()
    {
        SetGlasses(_GlassesIdx - 1);
        SavePrefs();
    }

    private void SetGlasses(int i)
    {
        LookDatabase db = GameObject.FindObjectOfType<LookDatabase>();
        int cnt = db.Glasses.Count + 1;
        _GlassesIdx = NextIndex(i, cnt);

        if (_GlassesIdx == (cnt - 1))
        {
            _Glasses.gameObject.SetActive(false);
        }
        else
        {
            var face = db.Glasses[_GlassesIdx];
            _Glasses.sprite = face;
            _Glasses.gameObject.SetActive(true);
        }
    }
}
