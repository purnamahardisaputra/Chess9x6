using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScripts : MonoBehaviour
{

    public GameObject chessPiece, panelLose;
    private GameObject[,] positions = new GameObject[9, 6];
    public GameObject[] pieces = new GameObject[24];
    public bool gameOver = false;
    public SpriteRenderer timesScale;
    public float time = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
        // pieces = new GameObject[]{
        //     Create("Knights", 0, 0),
        // };

        // for(int i = 0; i < pieces.Length; i++){
        //     SetPosition(pieces[i]);
        // }
    }

    public GameObject Create(string name, int x, int y){
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessScript script = obj.GetComponent<ChessScript>();
        script.name = name;
        script.SetXBoard(x);
        script.SetYBoard(y);
        script.getGameObject();
        return obj;
    }

    public void SetPosition(GameObject obj){
        ChessScript script = obj.GetComponent<ChessScript>();

        positions[script.GetXBoard(), script.GetYBoard()] = obj;
    }

    public void SetPosition(GameObject obj, int x, int y)
    {
        ChessScript script = obj.GetComponent<ChessScript>();

        script.SetXBoard(x);
        script.SetYBoard(y);
        positions[x, y] = obj;
    }


    public void SetPositionEmpty(int x, int y){
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y){
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y){
        if(x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)){
            return false;
        }
        return true;
    }

    public bool CanAttackOtherPlayers(GameObject chessPiece)
    {
        ChessScript script = chessPiece.GetComponent<ChessScript>();
        int x = script.GetXBoard();
        int y = script.GetYBoard();

        // Check for attack condition
        bool canAttack = false;

            Debug.Log(script.name);
        switch (script.name)
        {
            case "Knights":
                if (PositionOnBoard(x + 1, y + 2) && GetPosition(x + 1, y + 2) != null && GetPosition(x + 1, y + 2) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x + 1, y - 2) && GetPosition(x + 1, y - 2) != null && GetPosition(x + 1, y - 2) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x - 1, y + 2) && GetPosition(x - 1, y + 2) != null && GetPosition(x - 1, y + 2) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x - 1, y - 2) && GetPosition(x - 1, y - 2) != null && GetPosition(x - 1, y - 2) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x + 2, y + 1) && GetPosition(x + 2, y + 1) != null && GetPosition(x + 2, y + 1) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x + 2, y - 1) && GetPosition(x + 2, y - 1) != null && GetPosition(x + 2, y - 1) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x - 2, y + 1) && GetPosition(x - 2, y + 1) != null && GetPosition(x - 2, y + 1) != chessPiece)
                    canAttack = true;
                else if (PositionOnBoard(x - 2, y - 1) && GetPosition(x - 2, y - 1) != null && GetPosition(x - 2, y - 1) != chessPiece)
                    canAttack = true;
                break;
            case "Bishops":
                // Check diagonal positions for attack
                canAttack = CheckDiagonalAttack(x, y, 1, 1) ||
                            CheckDiagonalAttack(x, y, -1, -1) ||
                            CheckDiagonalAttack(x, y, 1, -1) ||
                            CheckDiagonalAttack(x, y, -1, 1);
                break;
            case "Rooks":
                // Check horizontal and vertical positions for attack
                canAttack = CheckLineAttack(x, y, 1, 0) ||
                            CheckLineAttack(x, y, -1, 0) ||
                            CheckLineAttack(x, y, 0, 1) ||
                            CheckLineAttack(x, y, 0, -1);
                break;
            case "Dragons":
                // Check horizontal, vertical, and diagonal positions for attack
                canAttack = CheckLineAttack(x, y, 1, 0) ||
                            CheckLineAttack(x, y, -1, 0) ||
                            CheckLineAttack(x, y, 0, 1) ||
                            CheckLineAttack(x, y, 0, -1) ||
                            CheckDiagonalAttack(x, y, 1, 1) ||
                            CheckDiagonalAttack(x, y, -1, -1) ||
                            CheckDiagonalAttack(x, y, 1, -1) ||
                            CheckDiagonalAttack(x, y, -1, 1);
                break;
        }

        return canAttack;
    }

    private bool CheckLineAttack(int x, int y, int xIncrement, int yIncrement)
    {
        GameScripts gameScripts = GetComponent<GameScripts>();

        x += xIncrement;
        y += yIncrement;

        while (gameScripts.PositionOnBoard(x, y))
        {
            GameObject piece = gameScripts.GetPosition(x, y);

            if (piece != null)
            {
                // Found a piece in the line, check if it can be attacked
                if (piece != this.gameObject && piece.GetComponent<ChessScript>().player != this.gameObject.GetComponent<ChessScript>().player)
                    return true;
                else
                    break; // Cannot attack further in this line
            }

            x += xIncrement;
            y += yIncrement;
        }

        return false;
    }

    private bool CheckDiagonalAttack(int x, int y, int xIncrement, int yIncrement)
    {
        GameScripts gameScripts = GetComponent<GameScripts>();

        x += xIncrement;
        y += yIncrement;

        while (gameScripts.PositionOnBoard(x, y))
        {
            GameObject piece = gameScripts.GetPosition(x, y);

            if (piece != null)
            {
                // Found a piece in the diagonal, check if it can be attacked
                if (piece != this.gameObject && piece.GetComponent<ChessScript>().player != this.gameObject.GetComponent<ChessScript>().player)
                    return true;
                else
                    break; // Cannot attack further in this diagonal
            }

            x += xIncrement;
            y += yIncrement;
        }

        return false;
    }

    private void Update() {
        if(!gameOver){
            time -= Time.deltaTime;
            if(time <= 0){
                gameOver = true;
            }
        }
        // scale sprite renderer to 0 with time
        timesScale.transform.localScale = new Vector3(time/3, 1, 1);

        if(gameOver){
            panelLose.SetActive(true);
        }
    }

    public void Restart(){
        // load scene ini ulang
        SceneManager.LoadScene(this.gameObject.scene.name);
    }
    
}
