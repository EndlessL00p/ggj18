using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateID
{
    Splash,
    MainMenu,
    MainGame,
    PauseMenu,
    LV1,
    LV2,
    LV3,
    LV4,
    LV5,
    LV6,
    Win,
}

public abstract class BaseState
{
    protected GameStateID stateID;

    // getter for the game state Enum class
    public GameStateID StateID
    {
        get { return stateID; }
    }

    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Shutdown()
    {

    }
}
