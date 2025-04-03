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
        if (controller == null)
        {
            Debug.LogError("GameController not found!");
            return;
        }

        // Destroy the captured piece if it's an attack move
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp != null)
            {
                if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                Destroy(cp);
            }
        }

        // Clear the previous position
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetYBoard());

        if (reference == null)
        {
            Debug.LogError("Reference chess piece is null!");
            return;
        }

        // Move piece to new position
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        // Update the chessboard
        controller.GetComponent<Game>().SetPosition(reference);
        controller.GetComponent<Game>().RecordMove(reference, matrixX, matrixY);

        // Switch turn
        controller.GetComponent<Game>().NextTurn();

        // 🔹 Call CheckForCheck() after switching turns
        CheckManager checkManager = FindObjectOfType<CheckManager>();
        if (checkManager != null)
        {
            checkManager.CheckForCheck();
        }
        else
        {
            Debug.LogError("CheckManager not found in the scene!");
        }

        // Destroy move plates
        reference.GetComponent<Chessman>().DestroyMovePlates();
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
