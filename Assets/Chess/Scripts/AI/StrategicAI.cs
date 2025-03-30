using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategicAI : IChessAI
{
    public void MakeMove(Game game)
    {
        string currentPlayer = game.GetCurrentPlayer();
        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;
        GameObject opponentKing = GetOpponentKing(game, currentPlayer);

        MovePlate bestMove = null;
        int bestScore = int.MinValue;

        foreach (var piece in playerPieces)
        {
            if (piece == null || piece.Equals(null)) // Skip if piece is destroyed
                continue;

            Chessman chessman = piece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();

            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var movePlate in movePlates)
            {
                if (movePlate == null || movePlate.Equals(null)) // Skip if move plate is destroyed
                    continue;

                MovePlate mpScript = movePlate.GetComponent<MovePlate>();

                // Evaluate move only if movePlate is valid
                int moveScore = EvaluateMove(game, chessman, mpScript, opponentKing);
                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestMove = mpScript;
                }
            }
        }


        if (bestMove != null)
        {
            bestMove.OnMouseUp(); // Execute the best strategic move
        }
        else
        {
            RandomMove(game); // Fallback to random move
        }
    }

    private int EvaluateMove(Game game, Chessman piece, MovePlate move, GameObject opponentKing)
    {
        int score = 0;

        // Prioritize attacking opponent's king
        if (move.GetReference() != null && move.GetReference() == opponentKing)
        {
            score += 50;
        }

        // Protect its own pieces if possible
        if (ProtectsPiece(game, move))
        {
            score += 15;
        }

        // Capture high-value opponent pieces
        if (move.attack)
        {
            Chessman target = move.GetReference().GetComponent<Chessman>();
            score += GetPieceValue(target.type);
        }

        // Control center squares (increase priority if moving to the center)
        if (IsCenterSquare(move.matrixX, move.matrixY))
        {
            score += 10;
        }

        // Avoid putting the king in danger
        if (IsKingSafeAfterMove(game, piece, move.matrixX, move.matrixY))
        {
            score += 5;
        }
        else
        {
            score -= 50; // Big penalty for exposing the king
        }

        if (move == null || move.Equals(null))
        {
            return -1000; // Invalid move, penalize heavily
        }


        return score;
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

    private bool IsCenterSquare(int x, int y)
    {
        // Center squares of the board (higher control priority)
        return (x == 3 || x == 4) && (y == 3 || y == 4);
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

    private void RandomMove(Game game)
    {
        RandomAI randomAI = new RandomAI();
        randomAI.MakeMove(game);
    }
}
