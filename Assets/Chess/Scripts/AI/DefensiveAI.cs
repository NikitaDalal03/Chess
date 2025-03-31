using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveAI : IChessAI
{
    public void MakeMove(Game game)
    {
        string currentPlayer = game.GetCurrentPlayer();
        GameObject[] playerPieces = currentPlayer == "white" ? game.playerWhite : game.playerBlack;

        MovePlate bestMove = null;
        int bestScore = int.MinValue;

        foreach (var piece in playerPieces)
        {
            if (piece == null || piece.Equals(null)) // Skip destroyed piece
                continue;

            Chessman chessman = piece.GetComponent<Chessman>();
            chessman.InitiateMovePlates();

            GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
            foreach (var movePlate in movePlates)
            {
                if (movePlate == null || movePlate.Equals(null))
                    continue;

                MovePlate mpScript = movePlate.GetComponent<MovePlate>();

                // Evaluate move for defensive strategy
                int moveScore = EvaluateMove(game, chessman, mpScript);
                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestMove = mpScript;
                }
            }
        }

        if (bestMove != null)
        {
            bestMove.OnMouseUp(); // Execute the best move
        }
        else
        {
            RandomMove(game); // Fallback to random move if no good move is found
        }
    }

    private int EvaluateMove(Game game, Chessman piece, MovePlate move)
    {
        int score = 0;

        // Protect own pieces
        if (ProtectsPiece(game, move))
        {
            score += 30;
        }

        // Protect the king
        if (ProtectsKing(game, move))
        {
            score += 50;
        }

        // Attack opponent's king
        if (AttacksOpponentKing(game, move))
        {
            score += 80;
        }

        // Avoid putting king in danger
        if (IsKingSafeAfterMove(game, piece, move.matrixX, move.matrixY))
        {
            score += 20;
        }
        else
        {
            score -= 100; // Heavy penalty if king is in danger
        }

        // Capture opponent's attacking piece
        if (move.attack && move.GetReference() != null)
        {
            Chessman target = move.GetReference().GetComponent<Chessman>();
            score += GetPieceValue(target.type);
        }

        return score;
    }

    private bool ProtectsPiece(Game game, MovePlate move)
    {
        GameObject[] playerPieces = game.GetCurrentPlayer() == "white" ? game.playerWhite : game.playerBlack;
        foreach (var piece in playerPieces)
        {
            if (piece != null && Vector3.Distance(move.transform.position, piece.transform.position) < 1.5f)
            {
                return true;
            }
        }
        return false;
    }

    private bool ProtectsKing(Game game, MovePlate move)
    {
        string kingName = game.GetCurrentPlayer() == "white" ? "white_king" : "black_king";
        GameObject king = GameObject.Find(kingName);

        if (king != null && Vector3.Distance(move.transform.position, king.transform.position) < 1.5f)
        {
            return true;
        }
        return false;
    }

    private bool AttacksOpponentKing(Game game, MovePlate move)
    {
        string opponentKingName = game.GetCurrentPlayer() == "white" ? "black_king" : "white_king";
        GameObject opponentKing = GameObject.Find(opponentKingName);

        if (move.attack && move.GetReference() == opponentKing)
        {
            return true;
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

        GameObject[] opponentPieces = game.GetOpponentPieces(player);
        foreach (GameObject opponentPiece in opponentPieces)
        {
            if (opponentPiece != null && opponentPiece.activeSelf)
            {
                Chessman opponentChessman = opponentPiece.GetComponent<Chessman>();
                opponentChessman.InitiateMovePlates();

                GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
                foreach (var movePlate in movePlates)
                {
                    MovePlate mpScript = movePlate.GetComponent<MovePlate>();
                    if (mpScript.GetReference() == king)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void RandomMove(Game game)
    {
        RandomAI randomAI = new RandomAI();
        randomAI.MakeMove(game);
    }
}