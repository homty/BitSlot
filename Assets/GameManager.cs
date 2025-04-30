using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int boardHeight, boardWidth;
    [SerializeField] private GameObject[] gamePieces;
    [SerializeField] private float[] spawnChances;

    private GameObject _board;
    private GameObject[,] _gameBoard;
    private Vector3 _offset = new Vector3(0, 0, -1);
    private List<GameObject> _matchLines;

    void Start()
    {
        _board = GameObject.Find("GameBoard");
        _gameBoard = new GameObject[boardHeight, boardWidth];
        _matchLines = new List<GameObject>();

        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                GameObject gridPosition = _board.transform.Find(i + " " + j).gameObject;
                GameObject pieceType = GetRandomPiece();
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
                    Destroy(gridPosition.transform.GetChild(0).gameObject);
                }

                GameObject pieceType = GetRandomPiece();
                GameObject thisPiece = Instantiate(pieceType, gridPosition.transform.position + _offset, Quaternion.identity);
                thisPiece.name = pieceType.name;
                thisPiece.transform.parent = gridPosition.transform;
                _gameBoard[i, j] = thisPiece;
            }
        }

        CheckForMatches();
    }

    private void CheckForMatches()
    {
        // Очистить предыдущие линии
        foreach (GameObject line in _matchLines)
        {
            Destroy(line);
        }
        _matchLines.Clear();

        // Горизонтальные совпадения
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth - 2; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i, j + 1], _gameBoard[i, j + 2]))
                {
                    DrawLine(_gameBoard[i, j].transform.position, _gameBoard[i, j + 2].transform.position);
                }
            }
        }

        // Вертикальные совпадения
        for (int j = 0; j < boardWidth; j++)
        {
            for (int i = 0; i < boardHeight - 2; i++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j], _gameBoard[i + 2, j]))
                {
                    DrawLine(_gameBoard[i, j].transform.position, _gameBoard[i + 2, j].transform.position);
                }
            }
        }

        // Диагонали ↘
        for (int i = 0; i < boardHeight - 2; i++)
        {
            for (int j = 0; j < boardWidth - 2; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j + 1], _gameBoard[i + 2, j + 2]))
                {
                    DrawLine(_gameBoard[i, j].transform.position, _gameBoard[i + 2, j + 2].transform.position);
                }
            }
        }

        // Диагонали ↙
        for (int i = 0; i < boardHeight - 2; i++)
        {
            for (int j = 2; j < boardWidth; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j - 1], _gameBoard[i + 2, j - 2]))
                {
                    DrawLine(_gameBoard[i, j].transform.position, _gameBoard[i + 2, j - 2].transform.position);
                }
            }
        }
    }

    private bool AreMatching(GameObject a, GameObject b, GameObject c)
    {
        return a != null && b != null && c != null &&
               a.name == b.name && b.name == c.name;
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject("MatchLine");
        myLine.transform.position = start;

        LineRenderer lr = myLine.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.positionCount = 2;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.useWorldSpace = true;

        lr.sortingLayerName = "Line"; 
        lr.sortingOrder = 1;              

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        _matchLines.Add(myLine);
    }


}
