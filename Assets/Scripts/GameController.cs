using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public enum gameStates { starting, playerTurn, enemyTurn, transition };
    public gameStates currentGameState = gameStates.playerTurn;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;    // Ensures there's only one game manager only

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
