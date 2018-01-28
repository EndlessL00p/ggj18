using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

//////////////////////////////////////////////////////////////////////////////////////////////
// Note: For changing game states, in this project, it's only turning on/off the UI object. //
//       Game Manager will load the whole scene in the first place due to the scale of the  //
//       project is not too big.                                                            //
//////////////////////////////////////////////////////////////////////////////////////////////

public class GameStateManager : MonoBehaviour
{
    //inherit from base class of the FSM to store current game state
    public BaseState currGameState;
    //list to store all game state
    private List<BaseState> gameStates;

    //get all UI element (To do)
    private GameObject ingameUI;
    private GameObject pauseUI;
    private GameObject winUI;

    //list to store all UI objects
    public List<GameObject> uiObjList;
    
    public string[] Levels;// = { "LV0", "LV1", "LV2", "LV3", "LV4", "LV5" };

    //getter for other classes to get current gameState
    public GameStateID currState
    {
        get { return currGameState.StateID; }
    }

    // Use this for initialization
    void Awake()
    {
        gameStates = new List<BaseState>();
        gameStates.Add(new SplashState());
        gameStates.Add(new MainMenuState());
        //gameStates.Add(new PauseState());
        gameStates.Add(new GameplayState());
        
        //gameStates.Add(new WinState());
        
        uiObjList = new List<GameObject>();
        ingameUI = GameObject.Find("IngameUI");
        pauseUI = GameObject.Find("PauseUI");
        winUI = GameObject.Find("WinUI");
        uiObjList.Add(ingameUI);
        uiObjList.Add(pauseUI);
        uiObjList.Add(winUI);

        SwitchGameState(GameStateID.Gameplay);
    }

    // Update is called once per frame
    void Update()
    {
        //if current game state is not null
        if (currGameState != null)
        {
            //run current game state's update
            currGameState.Update();
        }
    }

    //function that can switch game state
    public void SwitchGameState(GameStateID StateID)
    {
        //shut down current state before activate the other one
        if (currGameState != null)
        {
            currGameState.Shutdown();
        }

        //find each state in the list to see if it matches the one player entered
        for (int i = 0; i < gameStates.Count; i++)
        {
            //if matches, switch current to that state and break the for loop
            if (StateID == gameStates[i].StateID)
            {
                currGameState = gameStates[i];
                break;
            }
        }

        //Start current state
        currGameState.Start();
    }
}


/////////////////////////////////////////////////////////////////////
//Section below is the Game State classes                          //
//We are basically build a layer above Unity's core to control FSM //
//Therefore we need a constructor                                  //
/////////////////////////////////////////////////////////////////////

//Splash State class : inherit from FSM base class
public class SplashState :  BaseState
{
    //get game manager
    GameStateManager gm;

    public SplashState()
    {
        stateID = GameStateID.Splash;
    }

    public override void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
        gm.uiObjList[0].SetActive(true); //switch on ingameUI
        gm.uiObjList[1].SetActive(false);
        gm.uiObjList[2].SetActive(false);
    }

    public override void Update()
    {
        ShowLog();
    }

    //Display current sate in console
    private void ShowLog()
    {
        Debug.Log("This is Splash Screen");
    }
}

public class MainMenuState : BaseState
{
    GameStateManager gm;

    public MainMenuState()
    {
        stateID = GameStateID.MainMenu;
    }

    public override void Start()
    {
        gm.uiObjList[0].SetActive(true); //switch on ingameUI
        gm.uiObjList[1].SetActive(false);
        gm.uiObjList[2].SetActive(false);
    }

    public override void Update()
    {
        ShowLog();
    }

    private void ShowLog()
    {
        Debug.Log("This is Main Menu");
    }
}

//public class PauseState : BaseState
//{
//    GameStateManager gm;

//    public PauseState()
//    {
//        stateID = GameStateID.PauseMenu;
//    }

//    public override void Start()
//    {
//        //get game manager
//        gm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
//        //set the index[1] object (Pause) in UI list to active
//        gm.uiObjList[1].SetActive(true);
//        //pause the time
//    }

//    public override void Update()
//    {
//        //Display current sate in console
//        ShowLog();
//    }

//    public override void Shutdown()
//    {
//        //set current inactive when shutdown
//        gm.uiObjList[1].SetActive(false);
//    }

//    //Display current sate in console
//    private void ShowLog()
//    {
//        Debug.Log("This is Pause Menu");
//    }
//}

//public class WinState : BaseState
//{
//    GameStateManager gm;

//    public WinState()
//    {
//        stateID = GameStateID.Win;
//    }

//    public override void Start()
//    {
//        //get game manager
//        gm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
//        //set the index[1] object (Pause) in UI list to active
//        gm.uiObjList[2].SetActive(true);
//        //pause the time

//    }

//    public override void Update()
//    {
//        //Display current sate in console
//        ShowLog();

//    }

//    public override void Shutdown()
//    {
//        //set current inactive when shutdown
//        gm.uiObjList[2].SetActive(false);
//    }

//    //Display current sate in console
//    private void ShowLog()
//    {
//        Debug.Log("This is Pause Menu");
//    }
//}

public class GameplayState : BaseState
{
    private GameStateManager gm;
    private List<Student> students;

    public GameplayState()
    {
        stateID = GameStateID.Gameplay;
    }

    public override void Start()
    {        
        gm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
        students = GameObject.FindObjectsOfType<Student>().ToList();
        gm.uiObjList[0].SetActive(true); //switch on ingameUI
        gm.uiObjList[1].SetActive(false);
        gm.uiObjList[2].SetActive(false);
    }

    public override void Update()
    {
        UpdateInput();
        ShowLog();
    }

    public void UpdateInput()
    {
        Student currNoteHolder = null;
        foreach (Student s in students)
        {
            if (s.IsHoldingNote)
            {
                currNoteHolder = s;
                break;
            }
        }

        if (gm.uiObjList[1].activeInHierarchy == false && gm.uiObjList[1].activeInHierarchy == false)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                currNoteHolder.PassNote(PassDirection.LEFT);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                currNoteHolder.PassNote(PassDirection.RIGHT);

            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                currNoteHolder.PassNote(PassDirection.UP);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                currNoteHolder.PassNote(PassDirection.DOWN);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) //pause
            {
                gm.uiObjList[1].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.X)) //prev stage
            {

            }

            if (Input.GetKeyDown(KeyCode.C)) //next stage
            {
                LoadNextScene();
            }

            if (Input.GetKeyDown(KeyCode.Z)) //reload
            {
                ReloadCurrentScene();
            }
        }

        if (gm.uiObjList[1].activeInHierarchy == true) //resume
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gm.uiObjList[1].SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Z)) //reload
            {
                ReloadCurrentScene();
            }
        }

        if (gm.uiObjList[2].activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //main menu
            {

            }

            if (Input.GetKeyDown(KeyCode.Z)) //reload
            {
                ReloadCurrentScene();
            }

            if (Input.GetKeyDown(KeyCode.X)) // prev stage
            {

            }

            if (Input.GetKeyDown(KeyCode.C)) // next stage
            {
                LoadNextScene();
            }
        }

        if (currNoteHolder.IsEndStudent)
        {
            gm.uiObjList[2].SetActive(true);
        }
    }

    public void LoadNextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int cnt = gm.Levels.Length;
        for (int i=0; i < cnt; ++i)
        {
            string sceneName = gm.Levels[i];
            if (sceneName == currentSceneName && i < (cnt - 1))
            {
                string nextSceneName = gm.Levels[i+1];
                SceneManager.LoadScene(nextSceneName);
                return;
            }
        }        
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override void Shutdown()
    {
        gm.uiObjList[0].SetActive(false);
    }

    //Display current sate in console
    private void ShowLog()
    {
        Debug.Log("This is Main Game State");
    }

}