using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomSFX
{
    public string SFXName;
    public AudioClip[] Clips;
 
    public void PlayRandomSample(AudioSource src)
    {
        if (Clips != null)
        {
            int n = Clips.Length;
            if (n > 0)
            {
                int i = Random.Range(0, n * 3) % n;
                AudioClip clip = Clips[i];
                src.PlayOneShot(clip);
            }
        }
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    public RandomSFX[] SFXGroups;    

    private AudioSource _ambientSource;
    private AudioSource _actionSource;

    private Dictionary<string, RandomSFX> _lookup = new Dictionary<string, RandomSFX>();

    // Use this for initialization
    void Start () {
        Instance = this;

        _ambientSource = gameObject.transform.Find("AmbientSFX").GetComponent<AudioSource>();
        _actionSource = gameObject.transform.Find("ActionSFX").GetComponent<AudioSource>();

        foreach(var sfx in SFXGroups)
        {
            _lookup.Add(sfx.SFXName, sfx);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private RandomSFX GetSFX(string sfxName)
    {
        RandomSFX sfx;
        if (_lookup.TryGetValue(sfxName, out sfx))
        {
            return sfx;
        }

        foreach (var sfx2 in SFXGroups)
        {
            if (sfx2.SFXName == sfxName)
            {
                return sfx2;
            }
        }

        return null;
    }

    public void PlayActionSFX(string sfxName)
    {
        RandomSFX sfx = GetSFX(sfxName);
        if (sfx != null)
        {
            sfx.PlayRandomSample(_actionSource);
        }
    }

    public void PlayAmbientSFX(string sfxName)
    {
        RandomSFX sfx = GetSFX(sfxName);
        if (sfx != null)
        {
            sfx.PlayRandomSample(_actionSource);
        }
    }
}
