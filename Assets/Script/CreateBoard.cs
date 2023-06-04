using UnityEngine;

public class CreateBoard : MonoBehaviour
{
    public GameObject tilePrefab; // Prefab untuk tiap kotak pada papan catur
    public float tileSize = 1f; // Ukuran kotak

    private int rows = 6; // Jumlah baris
    private int columns = 9; // Jumlah kolom

    private GameObject[,] tiles; // Array dua dimensi untuk menyimpan kotak-kotak pada papan catur

    void Start()
    {
        CreateChessboard();
    }

    void CreateChessboard()
    {
        // Inisialisasi array untuk menyimpan kotak-kotak pada papan catur
        tiles = new GameObject[rows, columns];

        // Looping untuk membuat kotak-kotak pada papan catur
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Membuat kotak baru dari prefab
                GameObject tile = Instantiate(tilePrefab, transform);

                // Menentukan posisi kotak berdasarkan indeks baris dan kolom
                float posX = col * tileSize;
                float posY = row * tileSize;
                tile.transform.localPosition = new Vector3(posX, posY, 0f);

                // Menyimpan kotak pada array
                tiles[row, col] = tile;
            }
        }
    }
}
