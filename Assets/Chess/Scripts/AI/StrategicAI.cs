using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicAI : IChessAI
{
    public void MakeMove(Game game)
    {
        //Get the player's pieces
        string currentPlayer = game.GetCurrentPlayer();
        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;

        //Prioritize attacking the opponent's king
        GameObject opponentKing = GetOpponentKing(game, currentPlayer);

        foreach (var piece in playerPieces)
        {
            Chessman chessman = piece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();

            //Look for an attack on the opponent's king
            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var movePlate in movePlates)
            {
                MovePlate mpScript = movePlate.GetComponent<MovePlate>();

                //Access the coordinates directly using matrixX and matrixY
                int matrixX = mpScript.matrixX;
                int matrixY = mpScript.matrixY;

                //If the move plate's reference matches the opponent's king, make the move
                if (mpScript.GetReference() != null && mpScript.GetReference() == opponentKing)
                {
                    mpScript.OnMouseUp();
                    return;
                }
            }
        }

        //If no attacking move found, perform a defensive move (fallback to defensive AI)
        DefensiveAI defensiveAI = new DefensiveAI();
        defensiveAI.MakeMove(game);
    }

    private GameObject GetOpponentKing(Game game, string currentPlayer)
    {
        string opponent = currentPlayer == "white" ? "black" : "white";
        GameObject[] opponentPieces = opponent == "white" ? game.playerWhite : game.playerBlack;

        foreach (var piece in opponentPieces)
        {
            if (piece.name.Contains(opponent + "_king"))
            {
                return piece;
            }
        }
        return null;
    }

}

