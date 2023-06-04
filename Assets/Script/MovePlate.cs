using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;
    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to black
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 3.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = controller.GetComponent<GameScripts>().GetPosition(matrixX, matrixY);

                       Destroy(cp);
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<GameScripts>().SetPositionEmpty(reference.GetComponent<ChessScript>().GetXBoard(),
            reference.GetComponent<ChessScript>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<ChessScript>().SetXBoard(matrixX);
        reference.GetComponent<ChessScript>().SetYBoard(matrixY);
        reference.GetComponent<ChessScript>().SetCoordinates();

        //Update the matrix
        controller.GetComponent<GameScripts>().SetPosition(reference);

        

        //Destroy the move plates including self
        reference.GetComponent<ChessScript>().DestroyMovePlates();
    }

    public void SetCoordinates(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
