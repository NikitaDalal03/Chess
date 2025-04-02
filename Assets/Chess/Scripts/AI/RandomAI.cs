using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : IChessAI
{
    private GameObject lastMovedPiece;  

    public void MakeMove(Game game)
    {
        string currentPlayer = game.GetCurrentPlayer();

        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;

        // Check if the player has any pieces to move
        if (playerPieces.Length == 0)
        {
            Debug.LogError("No pieces available for the player to move.");
            return;
        }

        GameObject selectedPiece = null;
        Chessman chessman = null;

        while (selectedPiece == null)
        {
            selectedPiece = playerPieces[Random.Range(0, playerPieces.Length)];

            chessman = selectedPiece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();

            // Get all move plates available on the board
            GameObject[] allMovePlates = GameObject.FindGameObjectsWithTag("MovePlate");

            if (allMovePlates.Length == 0)
            {
                Debug.Log("No valid moves for this piece. Trying another piece.");
                selectedPiece = null;
            }
            else
            {
                Debug.Log("Selected piece with valid move plates.");
                break;
            }
        }

        if (selectedPiece == null)
        {
            Debug.LogError("No piece found with valid moves.");
            return;
        }

        // If there was a previously moved piece, reset its color to the original one
        if (lastMovedPiece != null)
        {
            lastMovedPiece.GetComponent<SpriteRenderer>().color = Color.white;  
        }

        // Highlight the newly moved piece
        selectedPiece.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 1f); //blue

        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        GameObject movePlate = movePlates[Random.Range(0, movePlates.Length)];

        MovePlate mpScript = movePlate.GetComponent<MovePlate>();
        mpScript.OnMouseUp();

        // Store the reference to the last moved piece
        lastMovedPiece = selectedPiece;
    }
}
