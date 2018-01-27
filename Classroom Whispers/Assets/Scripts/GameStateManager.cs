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

    //list to store all UI objects
    public List<GameObject> uiObjList;

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
        gameStates.Add(new MainGameState());
        SwitchGameState(GameStateID.MainGame);
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

        ///////////////////////////////////////////////////////////////////////////////
        //This section is for setting up the shortcut keys for developers            //
        //so the developer can enter each state easily without travers the gameloop  //
                                                                                     //
        if (Input.GetKeyDown(KeyCode.F1)) //Press F1 to enter Main Menu              //
        {                                                                            //
            SwitchGameState(GameStateID.Splash);                                     //
            currGameState.Update();                                                  //
        }                                                                            //
                                                                                     //
        if (Input.GetKeyDown(KeyCode.F2)) //Press F2 to enter Tutoriul               //
        {                                                                            //
            SwitchGameState(GameStateID.MainMenu);                                   //
            currGameState.Update();                                                  //
        }                                                                            //
                                                                                     //
        if (Input.GetKeyDown(KeyCode.F3)) //Press F2 to enter Tutoriul               //
        {                                                                            //
            SwitchGameState(GameStateID.MainGame);                                   //
            currGameState.Update();                                                  //
        }                                                                            //
                                                                                     //
        ///////////////////////////////////////////////////////////////////////////////



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
        base.Start();
        Time.timeScale = 0;
    }
}

public class MainGameState : BaseState
{
    //get game manager
    GameStateManager gm;
    List<Student> students;

    public MainGameState()
    {
        stateID = GameStateID.MainGame;
    }

    public override void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
        students = GameObject.FindObjectsOfType<Student>().ToList();
    }

    public override void Update()
    {
        UpdateInput();
        //ShowLog();
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

        if (currNoteHolder.IsEndStudent)
        {
            Debug.Log("Win");
        }

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

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }        
    }

    private bool CheckEndStudent()
    {
        foreach (Student s in students)
        {
            if (s.IsEndStudent == true && s.IsEndStudent == true)
            {
                return true;
            }
        }

        return false;
    }

    //Display current sate in console
    private void ShowLog()
    {
        Debug.Log("This is Main Game State");
    }
}