using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContainerBlockRandomSpawn : MonoBehaviour
{
    public GameObject blockPrefab;
    private GameObject currentBlock;
    private bool blockMoved = false;
    public TMP_Text scoreText; // Referensi ke TextMeshProUGUI
    public GameScripts gameScript; // Referensi ke script GameScripts
    private int score = 0; // Skor pemain

    void Start()
    {
        SpawnRandomBlock();
    }

    void SpawnRandomBlock()
    {
        int randomChessPiece = Random.Range(0, 4);
        Vector3 spawnPosition = transform.position;

        if (!blockMoved)
        {
            currentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            currentBlock = Instantiate(blockPrefab, GameObject.Find("Grid").transform);
        }

        switch (randomChessPiece)
        {
            case 0:
                currentBlock.GetComponent<ChessScript>().name = "Knights";
                break;
            case 1:
                currentBlock.GetComponent<ChessScript>().name = "Bishops";
                break;
            case 2:
                currentBlock.GetComponent<ChessScript>().name = "Rooks";
                break;
            case 3:
                currentBlock.GetComponent<ChessScript>().name = "Dragons";
                break;
        }

        currentBlock.GetComponent<ChessScript>().getGameObject();

        // Check if the current block can attack other pieces
        bool canAttack = currentBlock.GetComponent<ChessScript>().CanAttackOtherPieces();
        Debug.Log("Can attack: " + canAttack);
        if (canAttack)
        {
            Debug.Log("Current block can attack other pieces!");
            Debug.Log("Current block position: " + currentBlock.transform.position);
        }
    }

    void DestroyCurrentBlock()
    {
        if (currentBlock != null)
        {
            Destroy(currentBlock);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                GameObject obj = hit.collider.gameObject;

                if (obj.name == "Grid" && !blockMoved)
                {
                    // Debug.Log("Clicked on grid, cloning object");
                    GameObject clonedObject = Instantiate(currentBlock, obj.transform.position, Quaternion.Euler(0, 0, 0));
                    // tambah posisi z agar tidak menimpa objek lain
                    clonedObject.transform.position = new Vector3(clonedObject.transform.position.x, clonedObject.transform.position.y, -1);
                    //trim nama cloned object agar tidak ada (clone) di belakang nama
                    clonedObject.name = clonedObject.name.Replace("(Clone)", "");
                    // masukkan ke dalam dari container, dan jadikan child
                    clonedObject.transform.parent = GameObject.Find("Container").transform;
                    ChessScript chessScript = clonedObject.GetComponent<ChessScript>();

                    // Menambahkan objek ke dalam pieces pada GameScripts
                    List<GameObject> updatedPieces = new List<GameObject>(gameScript.pieces);
                    updatedPieces.Add(clonedObject);
                    gameScript.pieces = updatedPieces.ToArray();

                    // Mengatur posisi objek di dalam GameScripts
                    int x = (int)hit.point.x;
                    int y = (int)hit.point.y;
                    gameScript.SetPosition(clonedObject, x, y);

                    DestroyCurrentBlock();
                    SpawnRandomBlock();
                    if (gameScript.gameOver == false)
                    {
                        gameScript.time = 10f;
                    }

                    // Menambahkan skor berdasarkan jenis Block yang diletakkan
                    if (chessScript.name == "Rooks" || chessScript.name == "Bishops")
                    {
                        score += 2;
                    }
                    else if (chessScript.name == "Knights" || chessScript.name == "Dragons")
                    {
                        score += 1;
                    }

                    Debug.Log("Score: " + score);
                }
            }
        }

        Dictionary<string, List<GameObject>> blocksByName = new Dictionary<string, List<GameObject>>();

        // Memasukkan blok-blok ke dalam dictionary berdasarkan nama
        for (int i = 0; i < GameObject.Find("Container").transform.childCount; i++)
        {
            GameObject block = GameObject.Find("Container").transform.GetChild(i).gameObject;
            string blockName = block.name;

            if (!blocksByName.ContainsKey(blockName))
            {
                blocksByName[blockName] = new List<GameObject>();
            }

            blocksByName[blockName].Add(block);
        }

        // Menghapus semua blok dengan nama yang sama dan menghitung skor
        foreach (KeyValuePair<string, List<GameObject>> entry in blocksByName)
        {
            if (entry.Value.Count >= 3)
            {
                // Hapus semua blok dengan nama yang sama
                foreach (GameObject block in entry.Value)
                {
                    Destroy(block);
                }

                // Tambahkan skor berdasarkan jenis Block
                if (entry.Key == "Rooks" || entry.Key == "Bishops")
                {
                    score += entry.Value.Count * 2;
                }
                else if (entry.Key == "Knights" || entry.Key == "Dragons")
                {
                    score += entry.Value.Count;
                }

                Debug.Log("Score: " + score);
            }
        }
                scoreText.text = "Score: " + score;
    }
}
