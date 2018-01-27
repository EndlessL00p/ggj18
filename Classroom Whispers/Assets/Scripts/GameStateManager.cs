using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        gameStates.Add(new SplashState() );
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

public class MainGameState : BaseState
{
    //get game manager
    GameStateManager gm;

    public MainGameState()
    {
        stateID = GameStateID.MainGame;
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
        Debug.Log("This is Main Game State");
    }
}