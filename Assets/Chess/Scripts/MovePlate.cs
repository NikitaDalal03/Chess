using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.StarterKit.UI;


public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;    

    Chessman referenceChessman = null;    

    // Location on the board
    public int matrixX;        
    public int matrixY;    

    // false: movement, true: attacking
    public bool attack = false;    

    public void Start()
    {

        if (reference != null)
        {
            referenceChessman = reference.GetComponent<Chessman>();
        }

        if (attack)
        {
            // Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game gameScript = controller.GetComponent<Game>();

        // Destroy the victim Chesspiece if attacking
        if (attack)
        {
            GameObject cp = gameScript.GetPosition(matrixX, matrixY);

            if (cp != null)
            {
                if (cp.name == "white_king") gameScript.Winner("black");
                if (cp.name == "black_king") gameScript.Winner("white");

                Destroy(cp);
            }
        }

        // Set the Chesspiece's original location to be empty
        gameScript.SetPositionEmpty(referenceChessman.GetXBoard(), referenceChessman.GetYBoard());

        // Move reference chess piece to this position
        referenceChessman.SetXBoard(matrixX);
        referenceChessman.SetYBoard(matrixY);
        referenceChessman.SetCoords();

        // Update the matrix with the new piece position
        gameScript.SetPosition(reference);

        // Check if the move resulted in check
        CheckManager checkManager = controller.GetComponent<CheckManager>();
        bool whiteInCheck = checkManager.IsKingInCheck(true);
        bool blackInCheck = checkManager.IsKingInCheck(false);

        // Highlight the king in check
       // gameScript.HighlightKingInCheck(whiteInCheck, blackInCheck);

        // Switch Current Player
        gameScript.NextTurn();

        // Destroy the move plates
        referenceChessman.DestroyMovePlates();
    }



    public void SetCoords(int x, int y)
    {
         matrixX = x;
         matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
         reference = obj;

         if (reference != null)
         {
             referenceChessman = reference.GetComponent<Chessman>();
         }
    }

    public GameObject GetReference()
    {
         return reference;
    }
}
