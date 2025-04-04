using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.StarterKit.UI;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    private Game game;

    public GameObject movePlate;

    //Position for this Chesspiece on the Board
    private int xBoard = -1;
    private int yBoard = -1;
 
    public string player;

    public PieceType pieceType;

    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;   
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Start()
    {
        //SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        //game = GetComponent<Game>();
        //movePlate = GetComponent<MovePlate>();
        game = controller.GetComponent<Game>();
    }


    public void Activate()
    {        
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; pieceType = PieceType.Queen; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; pieceType = PieceType.Knight; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; pieceType = PieceType.Bishop; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; pieceType = PieceType.King; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; pieceType = PieceType.Rook; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; pieceType = PieceType.Pawn; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; pieceType = PieceType.Queen; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; pieceType = PieceType.Knight; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; pieceType = PieceType.Bishop; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; pieceType = PieceType.King; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; pieceType = PieceType.Rook; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; pieceType = PieceType.Pawn; break;
        }
    }

    public void SetCoords()
    {    
         //Get the board value in order to convert to xy coords
         float x = xBoard;
         float y = yBoard;

         //Adjust by variable offset
         x *= 0.66f;
         y *= 0.66f;

         //Add constants (pos 0,0)
         x += -2.3f;
         y += -2.3f;

         this.transform.position = new Vector3(x, y, -1.0f);
        //Debug.Log(gameObject.name + " Set Coordinates: (" + GetXBoard() + ", " + GetYBoard() + ")");
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()   
    {
        if (!game.IsGameOver() && game.GetCurrentPlayer() == player)     
        {
            DestroyMovePlates();    

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                 LineMovePlate(1, 0);
                 LineMovePlate(0, 1);
                 LineMovePlate(1, 1);
                 LineMovePlate(-1, 0);
                 LineMovePlate(0, -1);
                 LineMovePlate(-1, -1);
                 LineMovePlate(-1, 1);
                 LineMovePlate(1, -1);
                 break;
            case "black_knight":  
            case "white_knight":  
                  LMovePlate();
                  break;      
            case "black_bishop":  
            case "white_bishop":  
                  LineMovePlate(1, 1);  
                  LineMovePlate(1, -1);  
                  LineMovePlate(-1, 1);  
                  LineMovePlate(-1, -1);  
                  break;  
            case "black_king":  
            case "white_king":
                  SurroundMovePlate();  
                  break;
            case "black_rook":  
            case "white_rook":  
                  LineMovePlate(1, 0);  
                  LineMovePlate(0, 1);  
                  LineMovePlate(-1, 0);
                  LineMovePlate(0, -1);   
                  break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                if (yBoard == 6)
                {
                    //if both the first square (yBoard - 1) and the second square (yBoard - 2) are empty
                    if (game.GetPosition(xBoard, yBoard - 1) == null &&
                        game.GetPosition(xBoard, yBoard - 2) == null)
                    {
                        PawnMovePlate(xBoard, yBoard - 2);
                    }
                }
                break;

            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                if (yBoard == 1)
                {
                    //if both the first square (yBoard + 1) and the second square (yBoard + 2) are empty
                    if (game.GetPosition(xBoard, yBoard + 1) == null &&
                        game.GetPosition(xBoard, yBoard + 2) == null)
                    {
                        PawnMovePlate(xBoard, yBoard + 2);
                    }
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = game;

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);    
        PointMovePlate(xBoard - 1, yBoard + 2);    
        PointMovePlate(xBoard + 2, yBoard + 1);    
        PointMovePlate(xBoard + 2, yBoard - 1);    
        PointMovePlate(xBoard + 1, yBoard - 2);    
        PointMovePlate(xBoard - 1, yBoard - 2);    
        PointMovePlate(xBoard - 2, yBoard + 1);    
        PointMovePlate(xBoard - 2, yBoard - 1);    
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);    
        PointMovePlate(xBoard, yBoard - 1);    
        PointMovePlate(xBoard - 1, yBoard + 0);    
        PointMovePlate(xBoard - 1, yBoard - 1);    
        PointMovePlate(xBoard - 1, yBoard + 1);    
        PointMovePlate(xBoard + 1, yBoard + 0);    
        PointMovePlate(xBoard + 1, yBoard - 1);    
        PointMovePlate(xBoard + 1, yBoard + 1);    
    }

    public void PointMovePlate(int x, int y)    
    {
        Game sc = game;
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
         Game sc = game;
         if (sc.PositionOnBoard(x, y))
         {
             if (sc.GetPosition(x, y) == null)
             {
                 MovePlateSpawn(x, y);
             }

             if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
             {
                 MovePlateAttackSpawn(x + 1, y);
             }

             if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
             {
                 MovePlateAttackSpawn(x - 1, y);
             }
         }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
         //Get the board value in order to convert to xy coords
         float x = matrixX;
         float y = matrixY;

         //Adjust by variable offset
         x *= 0.66f;
         y *= 0.66f;

         //Add constants (pos 0,0)
         x += -2.3f;
         y += -2.3f;

         //Set actual unity values
         GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

         MovePlate mpScript = mp.GetComponent<MovePlate>();
         mpScript.SetReference(gameObject);
         mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
         //Get the board value in order to convert to xy coords
         float x = matrixX;
         float y = matrixY;

         //Adjust by variable offset
         x *= 0.66f;
         y *= 0.66f;

         //Add constants (pos 0,0)
         x += -2.3f;
         y += -2.3f;

         //Set actual unity values
         GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

         MovePlate mpScript = mp.GetComponent<MovePlate>();
         mpScript.attack = true;
         mpScript.SetReference(gameObject);
         mpScript.SetCoords(matrixX, matrixY);
    }


    public List<Vector2Int> GetMoveHistory()
    {
        return controller.GetComponent<Game>().GetPieceMovementHistory(gameObject);
    }


    public bool isWhite;
    public virtual List<Vector2Int> GetAvailableMoves()
    {
        return new List<Vector2Int>(); 
    }

    public void SetCheckHighlight(bool inCheck)
    {
        if (inCheck)
        {
            GetComponent<SpriteRenderer>().color = Color.red;  // Change to a red color if in check
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;  // Reset to default
        }
    }
}

public enum PieceType
{
    None, Pawn, Rook, Knight, Bishop, Queen, King
}
