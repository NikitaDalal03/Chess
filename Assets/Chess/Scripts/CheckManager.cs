using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckManager : MonoBehaviour
{
    private Chessman[,] board;
    private Vector2Int whiteKingPos;
    private Vector2Int blackKingPos;

    public void UpdateBoard(Chessman[,] newBoard)
    {
        board = newBoard;
        LocateKings();
    }

    private void LocateKings()
    {
        Debug.Log("Under LocateKing");
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] != null)
                {
                    if (board[x, y].pieceType == PieceType.King)
                    {
                        if (board[x, y].player == "white")
                            whiteKingPos = new Vector2Int(x, y);
                        else
                            blackKingPos = new Vector2Int(x, y);
                    }
                }
            }
        }
    }

    public bool IsKingInCheck(bool isWhite)
    {
        Vector2Int kingPos = isWhite ? whiteKingPos : blackKingPos;
        bool inCheck = IsSquareUnderAttack(kingPos.x, kingPos.y, !isWhite);
        Debug.Log("Under IsKingInCheck Condition");
        HighlightKing(isWhite, inCheck);
        return inCheck;
    }

    public bool IsSquareUnderAttack(int x, int y, bool byWhite)
    {
        foreach (var piece in GetAllPieces(byWhite))
        {
            List<Vector2Int> moves = piece.GetAvailableMoves();
            foreach (var move in moves)
            {
                if (move.x == x && move.y == y)
                    return true;
            }
        }
        return false;
    }

    private List<Chessman> GetAllPieces(bool isWhite)
    {
        List<Chessman> pieces = new List<Chessman>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] != null && (board[x, y].player == "white") == isWhite)
                {
                    pieces.Add(board[x, y]);
                }
            }
        }
        return pieces;
    }

    private void HighlightKing(bool isWhite, bool inCheck)
    {
        Vector2Int kingPos = isWhite ? whiteKingPos : blackKingPos;
        Chessman king = board[kingPos.x, kingPos.y];

        if (king != null)
        {
            king.SetCheckHighlight(inCheck);
        }
    }

    public Vector2Int GetKingPosition(bool isWhite)
    {
        return isWhite ? whiteKingPos : blackKingPos;
    }

}
