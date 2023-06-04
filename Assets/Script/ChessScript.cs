using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessScript : MonoBehaviour
{
    public GameObject controller;
    public GameObject MovePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    public string player;

    public Sprite Knights, Bishops, Rooks, Dragons;

    public void getGameObject(){
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoordinates();

        switch (this.name)
        {
            case "Knights": this.GetComponent<SpriteRenderer>().sprite = Knights; player = "white"; break;
            case "Bishops": this.GetComponent<SpriteRenderer>().sprite = Bishops; player = "white"; break;
            case "Rooks": this.GetComponent<SpriteRenderer>().sprite = Rooks; player = "white"; break;
            case "Dragons": this.GetComponent<SpriteRenderer>().sprite = Dragons; player = "white"; break;
        }
    }

    public void SetCoordinates()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1f;
        y *= 1f;

        x += 0f;
        y += 0f;

        this.transform.position = new Vector3(x, y, -1);
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

    private void OnMouseUp() {
        DestroyMovePlates();

        InitiateMovePlates();
    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    private void Start() {
        
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "Knights":
                LMovePlate();
                break;
            case "Bishops":
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            case "Rooks":
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                break;
            case "Dragons":
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                break;
            
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement){
        GameScripts gameScripts = controller.GetComponent<GameScripts>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(gameScripts.PositionOnBoard(x, y) && gameScripts.GetPosition(x, y) == null){
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;

            }
            if(gameScripts.PositionOnBoard(x,y) && gameScripts.GetPosition(x,y).GetComponent<ChessScript>().player != player){
                MovePlateSpawn(x, y);
        }
    }

    public void LMovePlate(){
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void PointMovePlate(int x, int y){
        GameScripts gameScripts = controller.GetComponent<GameScripts>();

        if(gameScripts.PositionOnBoard(x,y)){

            GameObject obj = gameScripts.GetPosition(x, y);

            if(obj == null){
                MovePlateSpawn(x, y);
            }else if(obj.GetComponent<ChessScript>().player != player){
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY){
        float x = matrixX;
        float y = matrixY;

        x *= 1f;
        y *= 1f;

        x += 0f;
        y += 0f;

        GameObject obj = Instantiate(MovePlate, new Vector3(x, y, -2), Quaternion.identity);

        MovePlate movePlateScript = obj.GetComponent<MovePlate>();
        movePlateScript.SetReference(this.gameObject);
        movePlateScript.SetCoordinates(matrixX, matrixY);
    }
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1f;
        y *= 1f;

        x += 0f;
        y += 0f;

        GameObject obj = Instantiate(MovePlate, new Vector3(x, y, -2), Quaternion.identity);

        MovePlate movePlateScript = obj.GetComponent<MovePlate>();
        movePlateScript.attack = true;
        movePlateScript.SetReference(this.gameObject);
        movePlateScript.SetCoordinates(matrixX, matrixY);
    }

    public bool CanAttackOtherPieces()
    {
        GameScripts gameScripts = controller.GetComponent<GameScripts>();

        // Get the current position of the chess piece
        int x = GetXBoard();
        int y = GetYBoard();

        // Check if the chess piece can attack in any direction
        bool canAttack = CheckLineAttack(x, y, 1, 0) ||
                         CheckLineAttack(x, y, -1, 0) ||
                         CheckLineAttack(x, y, 0, 1) ||
                         CheckLineAttack(x, y, 0, -1) ||
                         CheckDiagonalAttack(x, y, 1, 1) ||
                         CheckDiagonalAttack(x, y, -1, -1) ||
                         CheckDiagonalAttack(x, y, 1, -1) ||
                         CheckDiagonalAttack(x, y, -1, 1);

        return canAttack;
    }

    private bool CheckLineAttack(int x, int y, int xIncrement, int yIncrement)
    {
        GameScripts gameScripts = controller.GetComponent<GameScripts>();

        x += xIncrement;
        y += yIncrement;

        while (gameScripts.PositionOnBoard(x, y))
        {
            GameObject piece = gameScripts.GetPosition(x, y);

            if (piece != null)
            {
                if (piece.GetComponent<ChessScript>().player != player)
                {
                    // Found an opponent's piece, can attack
                    gameScripts.gameOver = true;
                    Debug.Log("Found an opponent's piece, can attack");
                    return true;
                }
                else
                {
                    Debug.Log("Found own piece, cannot attack beyond it");
                    gameScripts.gameOver = true;
                    // Found own piece, cannot attack beyond it
                    break;
                }
            }

            x += xIncrement;
            y += yIncrement;
        }

        return false;
    }

    private bool CheckDiagonalAttack(int x, int y, int xIncrement, int yIncrement)
    {
        GameScripts gameScripts = controller.GetComponent<GameScripts>();

        x += xIncrement;
        y += yIncrement;

        while (gameScripts.PositionOnBoard(x, y))
        {
            GameObject piece = gameScripts.GetPosition(x, y);

            if (piece != null)
            {
                if (piece.GetComponent<ChessScript>().player != player)
                {
                    gameScripts.gameOver = true;
                    Debug.Log("Found an opponent's piece, can attack");
                    // Found an opponent's piece, can attack
                    return true;
                }
                else
                {
                    gameScripts.gameOver = true;
                    Debug.Log("Found own piece, cannot attack beyond it");
                    // Found own piece, cannot attack beyond it
                    break;
                }
            }

            x += xIncrement;
            y += yIncrement;
        }

        return false;
    }

}
