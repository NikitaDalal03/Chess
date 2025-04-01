using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yudiz.StarterKit.UI;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject chesspiece;
    private IChessAI selectedAI;

    //same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];

    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    private string winner;


    public void Start()
    {
        chessPiecePosition();

        // Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public void SelectAI(int aiLevel)
    {
        switch (aiLevel)
        {
            case 1:
                selectedAI = new RandomAI();
                break;
            //case 2:
            //    selectedAI = new DefensiveAI();
            //    break;
            //case 2:
            //    selectedAI = new StrategicAI();
            //    break;
        }
    }

    // Store positions and pieces together
    public List<(GameObject piece, Vector2 position)> GetOpponentPieces(string currentPlayer)
    {
        List<(GameObject, Vector2)> opponentPieces = new List<(GameObject, Vector2)>();

        // Get the player's pieces
        GameObject[] pieces = (currentPlayer == "white") ? playerBlack : playerWhite;

        foreach (var piece in pieces)
        {
            Chessman chessman = piece.GetComponent<Chessman>();
            int x = chessman.GetXBoard();
            int y = chessman.GetYBoard();

            // Store both the piece and its current position
            opponentPieces.Add((piece, new Vector2(x, y)));
        }
        return opponentPieces;
    }


    public void chessPiecePosition()
    {
        playerWhite = new GameObject[] { Create("white_rook", 0, 0), Create("white_knight", 1, 0),
            Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1),
            Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1),
            Create("white_pawn", 6, 1), Create("white_pawn", 7, 1) };
        playerBlack = new GameObject[] { Create("black_rook", 0, 7), Create("black_knight", 1, 7),
            Create("black_bishop", 2, 7), Create("black_queen", 3, 7), Create("black_king", 4, 7),
            Create("black_bishop", 5, 7), Create("black_knight", 6, 7), Create("black_rook", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6),
            Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6),
            Create("black_pawn", 6, 6), Create("black_pawn", 7, 6) };
    }


    public GameObject GetPiece(string pieceName)
    {
        foreach (var piece in playerBlack)
        {
            if (piece.name == pieceName)
                return piece;
        }

        foreach (var piece in playerWhite)
        {
            if (piece.name == pieceName)
                return piece;
        }

        return null;
    }


    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            ResetGame();
        }


        if (currentPlayer == "black" && selectedAI != null && !isAITurn)
        {
            StartCoroutine(HandleAITurnDelay(5f));  
        }
    }

    private bool isAITurn = false;


    private IEnumerator HandleAITurnDelay(float delay)
    {
        isAITurn = true;
        yield return new WaitForSeconds(delay); 
        selectedAI.MakeMove(this);  
        //NextTurn(); 
        isAITurn = false;
    }

    public void NextTurn()
    {
        // Proceed to next turn
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            Timer.instance.StartBlackTimer();
        }
        else
        {
            currentPlayer = "white";
            Timer.instance.StartWhiteTimer();
        }
    }


    public void Winner(string playerWinner)
    {
        winner = playerWinner;
        gameOver = true;
        Debug.Log(playerWinner + " wins!");

        UIManager.Instance.ShowScreen(ScreenName.GameWinScreen);
    }

    public string GetWinner()
    {
        return winner;
    }

    public void ResetGame()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                positions[x, y] = null;
            }
        }

        foreach (GameObject piece in playerBlack)
        {
            Destroy(piece);
        }

        foreach (GameObject piece in playerWhite)
        {
            Destroy(piece);
        }

        //Reset the game state
        playerBlack = new GameObject[16];
        playerWhite = new GameObject[16];

        //Reinitialize player pieces to their starting positions
        chessPiecePosition();

        //Set positions for the newly instantiated pieces
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }

        //Reset turn to white and game over flag
        currentPlayer = "white";
        gameOver = false;

        Timer.instance.ResetWhiteTimer();
        Timer.instance.ResetBlackTimer();
    }
}
