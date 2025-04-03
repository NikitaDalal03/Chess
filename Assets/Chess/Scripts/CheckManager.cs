using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckManager : MonoBehaviour
{
    private Game gameController;
    private GameObject lastMovedPiece;

    void Start()
    {
        gameController = FindObjectOfType<Game>();
    }

    public void CheckForCheck()
    {
        if (gameController == null)
        {
            Debug.LogError("GameController not found!");
            return;
        }

        lastMovedPiece = GetLastMovedPiece();
        if (lastMovedPiece == null)
        {

            Debug.LogError("No last moved piece found!");
            return;
        }

        List<Vector2Int> attackablePositions = GetAttackRange(lastMovedPiece);

        // Iterate through all board positions to find the king
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject piece = gameController.GetPosition(x, y);
                if (piece == null) continue;

                string pieceName = piece.name;

                // If the piece is a king and its position is in attackable positions, highlight it
                if ((pieceName == "white_king" || pieceName == "black_king") &&
                    attackablePositions.Contains(new Vector2Int(x, y)))
                {
                    HighlightKing(piece);
                    Debug.Log(pieceName + " is in check!");
                }
            }
        }
    }

    private GameObject GetLastMovedPiece()
    {
        Dictionary<GameObject, List<Vector2Int>> pieceMovements = gameController.pieceMovements; 

        GameObject lastPiece = null;
        Vector2Int lastPosition = Vector2Int.zero;

        foreach (var entry in pieceMovements)
        {
            List<Vector2Int> moves = entry.Value;
            if (moves.Count > 0 && (lastPiece == null || moves[^1] != lastPosition))
            {
                lastPiece = entry.Key;
                lastPosition = moves[^1];
            }
        }

        return lastPiece;
    }

    private List<Vector2Int> GetAttackRange(GameObject piece)
    {
        List<Vector2Int> attackPositions = new List<Vector2Int>();

        MovePlate[] movePlates = FindObjectsOfType<MovePlate>();

        foreach (MovePlate movePlate in movePlates)
        {
            if (movePlate.GetReference() == piece && movePlate.attack)
            {
                attackPositions.Add(new Vector2Int(movePlate.matrixX, movePlate.matrixY));
            }
        }

        return attackPositions;
    }

    private void HighlightKing(GameObject king)
    {
        Debug.Log("Under Highlight king");
        SpriteRenderer sr = king.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
        }
    }
}
