using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAI : IChessAI
{
    public void MakeMove(Game game)
    {
        string currentPlayer = game.GetCurrentPlayer();
        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;

        //Scan through all pieces of the current player to find the best defensive move
        foreach (var piece in playerPieces)
        {
            Chessman chessman = piece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();  //Generate move plates for the current piece

            //Check each move plate of the piece
            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var movePlate in movePlates)
            {
                MovePlate mpScript = movePlate.GetComponent<MovePlate>();

                //Access the coordinates directly using matrixX and matrixY
                int matrixX = mpScript.matrixX;
                int matrixY = mpScript.matrixY;

                //If the move plate is attacking (red), avoid that position
                if (mpScript.attack)
                {
                    continue; //Skip this move plate
                }

                //Check if the move would place the player's king in check
                if (IsKingInCheckAfterMove(game, chessman, matrixX, matrixY))
                {
                    continue; //Skip moves that place the king in check
                }

                // Check if the move helps protect a piece or defends the king
                if (IsDefensiveMove(game, chessman, mpScript))
                {
                    mpScript.OnMouseUp(); //Perform the defensive move
                    return;
                }
            }
        }
    }

    private bool IsKingInCheckAfterMove(Game game, Chessman chessman, int targetX, int targetY)
    {
        //Backup the current position of the chess piece
        int originalX = chessman.GetXBoard();
        int originalY = chessman.GetYBoard();

        //Simulate the move (move the piece to the target location temporarily)
        chessman.SetXBoard(targetX);
        chessman.SetYBoard(targetY);

        //Check if the king is in check
        bool isInCheck = IsKingInCheck(game, game.GetCurrentPlayer());

        //Revert the move
        chessman.SetXBoard(originalX);
        chessman.SetYBoard(originalY);

        return isInCheck;
    }

    private bool IsKingInCheck(Game game, string player)
    {
        //Identify the player's king
        string kingName = player == "white" ? "white_king" : "black_king";
        GameObject king = GameObject.Find(kingName);

        //Check if the king is in check by any opponent piece
        foreach (GameObject opponentPiece in game.GetOpponentPieces(player))
        {
            Chessman opponentChessman = opponentPiece.GetComponent<Chessman>();
            if (opponentChessman.CanAttackPosition(king.transform.position))
            {
                return true; //The king is in check
            }
        }
        return false; //The king is not in check
    }

    private bool IsDefensiveMove(Game game, Chessman chessman, MovePlate mpScript)
    {
        string currentPlayer = game.GetCurrentPlayer();
        GameObject opponentKing = currentPlayer == "white" ? GameObject.Find("black_king") : GameObject.Find("white_king");

        //If the move brings a piece close to the opponent's king, it might be a defensive position
        if (Vector3.Distance(mpScript.transform.position, opponentKing.transform.position) < 2.0f)
        {
            return true;
        }
        
        return false;
    }

}



