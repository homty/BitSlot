using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int boardHeight, boardWidth;
    [SerializeField]
    private GameObject[] gamePieces;

    private GameObject _board;
    private GameObject[,] _gameBoard;
    private Vector3 _offset = new Vector3(0, 0, -1);
    [SerializeField]
    private float[] spawnChances;
    // Start is called before the first frame update
    void Start()
    {
        _board = GameObject.Find("GameBoard");
        _gameBoard = new GameObject[boardHeight, boardWidth];
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                GameObject gridPosition = _board.transform.Find(i + " " + j).gameObject;
                GameObject pieceType = gamePieces[Random.Range(0, gamePieces.Length)];
                GameObject thisPiece = Instantiate(pieceType, gridPosition.transform.position + _offset, Quaternion.identity);
                thisPiece.name = pieceType.name;
                thisPiece.transform.parent = gridPosition.transform;
                _gameBoard[i, j] = thisPiece;
            }
        }
    }
    private GameObject GetRandomPiece()
    {
        float total = 0;
        foreach (float chance in spawnChances)
        {
            total += chance;
        }

       float randomPoint = Random.value * total;

        for (int i = 0; i < spawnChances.Length; i++)
        {
            if (randomPoint < spawnChances[i])
            {
                return gamePieces[i];
            }
            else
            {
                randomPoint -= spawnChances[i];
            }
        }
    
        return gamePieces[gamePieces.Length - 1];
    }
    public void Spin()
    {
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                GameObject gridPosition = _board.transform.Find(i + " " + j).gameObject;
                if (gridPosition.transform.childCount > 0)
                {
                    GameObject destroyPiece = gridPosition.transform.GetChild(0).gameObject;
                    Destroy(destroyPiece);
                }
                GameObject pieceType = GetRandomPiece();
                GameObject thisPiece = Instantiate(pieceType, gridPosition.transform.position + _offset, Quaternion.identity);
                thisPiece.name = pieceType.name;
                thisPiece.transform.parent = gridPosition.transform;
                _gameBoard[i, j] = thisPiece;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
