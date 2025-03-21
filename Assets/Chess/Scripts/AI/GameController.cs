using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isAITurn = false;  
    public string currentPlayer = "white";  
    public Game game;  

    public enum AIType
    {
        Random,
        Defensive,
        Strategic
    }

    public AIType selectedAI = AIType.Defensive;  // Default to Defensive AI

    // Called to handle when the player moves
    public void OnPlayerMove()
    {
        // After the player makes a move, switch to AI's turn
        SwitchTurn();

        if (isAITurn)
        {
            AITurn();
        }
    }

    private void AITurn()
    {
        // Based on the selected AI, make the appropriate move
        switch (selectedAI)
        {
            case AIType.Random:
                RandomAI randomAI = new RandomAI();
                randomAI.MakeMove(game);  
                break;
            case AIType.Defensive:
                DefensiveAI defensiveAI = new DefensiveAI();
                defensiveAI.MakeMove(game);  
                break;
            case AIType.Strategic:
                StrategicAI strategicAI = new StrategicAI();
                strategicAI.MakeMove(game);  
                break;
        }

        // Switch turn after AI makes a move
        SwitchTurn();
    }

    private void SwitchTurn()
    {
        // Switch player turn
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }

        // Toggle AI turn flag
        isAITurn = !isAITurn;
    }

    // Method to handle button clicks for selecting the AI
    public void SelectRandomAI()
    {
        selectedAI = AIType.Random;
    }

    public void SelectDefensiveAI()
    {
        selectedAI = AIType.Defensive;
    }

    public void SelectStrategicAI()
    {
        selectedAI = AIType.Strategic;
    }

    // Method to start the game with selected AI
    public void StartGame()
    {
        // Initialize the game (reset positions, set the starting player, etc.)
        currentPlayer = "white"; // Player starts
        isAITurn = false; // It's the player's turn initially
    }
}
