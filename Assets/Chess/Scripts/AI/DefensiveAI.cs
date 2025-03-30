using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAI : IChessAI
{
    public void MakeMove(Game game)
    {
        string currentPlayer = game.GetCurrentPlayer();
        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;

        MovePlate bestDefensiveMove = null;
        int bestDefenseScore = int.MinValue;

        foreach (var piece in playerPieces)
        {
            Chessman chessman = piece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();

            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var movePlate in movePlates)
            {
                MovePlate mpScript = movePlate.GetComponent<MovePlate>();

                int defenseScore = EvaluateMoveDefense(game, chessman, mpScript);
                if (defenseScore > bestDefenseScore)
                {
                    bestDefenseScore = defenseScore;
                    bestDefensiveMove = mpScript;
                }
            }
        }

        if (bestDefensiveMove != null)
        {
            bestDefensiveMove.OnMouseUp(); // Execute the best defensive move
        }
        else
        {
            RandomMove(game); // Fallback to random move
        }
    }

    private int EvaluateMoveDefense(Game game, Chessman piece, MovePlate move)
    {
        int score = 0;

        // Check if the move protects a piece
        if (ProtectsPiece(game, move))
        {
            score += 20;
        }

        // Avoid check or block check
        if (IsKingSafeAfterMove(game, piece, move.matrixX, move.matrixY))
        {
            score += 15;
        }

        // If the move captures an attacking piece, prioritize it
        if (move.attack)
        {
            score += GetPieceValue(move.GetReference().GetComponent<Chessman>().type);
        }

        return score;
    }

    private bool IsKingSafeAfterMove(Game game, Chessman piece, int targetX, int targetY)
    {
        int originalX = piece.GetXBoard();
        int originalY = piece.GetYBoard();
        piece.SetXBoard(targetX);
        piece.SetYBoard(targetY);

        bool safe = !IsKingInCheck(game, game.GetCurrentPlayer());

        piece.SetXBoard(originalX);
        piece.SetYBoard(originalY);

        return safe;
    }

    private bool IsKingInCheck(Game game, string player)
    {
        string kingName = player == "white" ? "white_king" : "black_king";
        GameObject king = GameObject.Find(kingName);

        foreach (GameObject opponentPiece in game.GetOpponentPieces(player))
        {
            Chessman opponentChessman = opponentPiece.GetComponent<Chessman>();
            if (opponentChessman.CanAttackPosition(king.transform.position))
            {
                return true;
            }
        }
        return false;
    }

    private bool ProtectsPiece(Game game, MovePlate move)
    {
        GameObject[] playerPieces = game.GetCurrentPlayer() == "white" ? game.playerWhite : game.playerBlack;
        foreach (var piece in playerPieces)
        {
            if (Vector3.Distance(move.transform.position, piece.transform.position) < 1.5f)
            {
                return true;
            }
        }
        return false;
    }

    private int GetPieceValue(string pieceType)
    {
        switch (pieceType)
        {
            case "queen": return 9;
            case "rook": return 5;
            case "bishop": return 3;
            case "knight": return 3;
            case "pawn": return 1;
            default: return 0;
        }
    }

    private void RandomMove(Game game)
    {
        RandomAI randomAI = new RandomAI();
        randomAI.MakeMove(game);
    }
}
