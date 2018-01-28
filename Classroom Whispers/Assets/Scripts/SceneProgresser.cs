using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneProgresser : MonoBehaviour {

    public string SceneName = "";
    public int AfterCountdown = 4;

    private float _timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime;
        if ((int)_timer >= AfterCountdown)
        {
            SceneManager.LoadScene(SceneName);
        }

    }
}
