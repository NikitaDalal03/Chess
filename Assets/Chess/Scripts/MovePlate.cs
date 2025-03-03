using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.StarterKit.UI;


public class MovePlate : MonoBehaviour
{
        // Some functions will need reference to the controller
        public GameObject controller;

        GameObject reference = null;

        Chessman referenceChessman = null;

        // Location on the board
        int matrixX;
        int matrixY;

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

            // Destroy the victim Chesspiece if attacking
            if (attack)
            {
                GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

                if (cp != null)  // Make sure the piece exists
                {
                    if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                    if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                    Destroy(cp);
                }
                else
                {
                    Debug.LogWarning("No piece at the target position!");
                }

            }

            // Set the Chesspiece's original location to be empty
            controller.GetComponent<Game>().SetPositionEmpty(referenceChessman.GetXBoard(),
                referenceChessman.GetYBoard());

            // Move reference chess piece to this position
            referenceChessman.SetXBoard(matrixX);
            referenceChessman.SetYBoard(matrixY);
            referenceChessman.SetCoords();

            // Update the matrix with the new piece position
            controller.GetComponent<Game>().SetPosition(reference);

            // Switch Current Player
            controller.GetComponent<Game>().NextTurn();

            // Destroy the move plates including self
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
