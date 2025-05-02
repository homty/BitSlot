using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int boardHeight, boardWidth;
    [SerializeField] private GameObject[] gamePieces;
    [SerializeField] private float[] spawnChances;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private float betAmount = 1f;
    [SerializeField] private float minBetAmount = 1f;
    [SerializeField] private TextMeshProUGUI betText;
    [SerializeField] private GameObject explosionPrefab; // 💥 Explosion prefab reference

    private List<Vector2Int> matchedPositions = new List<Vector2Int>();
    private GameObject _board;
    private GameObject[,] _gameBoard;
    private Vector3 _offset = new Vector3(0, 0, -1);
    private float playerBalance = 0f;

    void Start()
    {
        _board = GameObject.Find("GameBoard");
        _gameBoard = new GameObject[boardHeight, boardWidth];
        UpdateBalanceUI();
        UpdateBetUI();

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
        //Horizontal
        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth - 2; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i, j + 1], _gameBoard[i, j + 2]))
                {
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j + 1], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j + 2], 0.1f));

                    AddToBalance(_gameBoard[i, j]);
                }
            }
        }

        //Vertical
        for (int j = 0; j < boardWidth; j++)
        {
            for (int i = 0; i < boardHeight - 2; i++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j], _gameBoard[i + 2, j]))
                {
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 1, j], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 2, j], 0.1f));

                    AddToBalance(_gameBoard[i, j]);
                }
            }
        }

        //Diagonal Left-Right
        for (int i = 0; i < boardHeight - 2; i++)
        {
            for (int j = 0; j < boardWidth - 2; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j + 1], _gameBoard[i + 2, j + 2]))
                {
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 1, j + 1], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 2, j + 2], 0.1f));

                    AddToBalance(_gameBoard[i, j]);
                }
            }
        }

        //Diagonal Right-Left
        for (int i = 0; i < boardHeight - 2; i++)
        {
            for (int j = 2; j < boardWidth; j++)
            {
                if (AreMatching(_gameBoard[i, j], _gameBoard[i + 1, j - 1], _gameBoard[i + 2, j - 2]))
                {
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i, j], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 1, j - 1], 0.1f));
                    StartCoroutine(AnimateMatchEffect(_gameBoard[i + 2, j - 2], 0.1f));

                    AddToBalance(_gameBoard[i, j]);
                }
            }
        }

        for (int col = 0; col < boardWidth; col++)
        {
            string targetName = _gameBoard[0, col].name;
            bool isFullMultiplierColumn = true;

            for (int row = 0; row < boardHeight; row++)
            {
                GameObject piece = _gameBoard[row, col];
                if (piece.name != targetName)
                {
                    isFullMultiplierColumn = false;
                    break;
                }

                SymbolInfo info = piece.GetComponent<SymbolInfo>();
                if (info == null || !info.isMultiplier)
                {
                    isFullMultiplierColumn = false;
                    break;
                }
            }

            if (isFullMultiplierColumn)
            {
                Debug.Log($"💥 JACKPOT! Column {col} has 5 matching multipliers: {targetName}");

                for (int row = 0; row < boardHeight; row++)
                {
                    StartCoroutine(AnimateMatchEffect(_gameBoard[row, col], 0.1f));
                }

            }
        }

        bool isFiveBonusAcross = true;
        GameObject[] bonusPositions = new GameObject[boardWidth];

        for (int col = 0; col < boardWidth; col++)
        {
            bool foundBonusInColumn = false;
            for (int row = 0; row < boardHeight; row++)
            {
                GameObject piece = _gameBoard[row, col];
                if (piece == null) continue;

                SymbolInfo info = piece.GetComponent<SymbolInfo>();
                if (info != null && info.isBonus)
                {
                    bonusPositions[col] = piece;
                    foundBonusInColumn = true;
                    break;
                }
            }

            if (!foundBonusInColumn)
            {
                isFiveBonusAcross = false;
                break;
            }
        }

        if (isFiveBonusAcross)
        {
            Debug.Log("🎁 BONUS MATCH! 5 bonus symbols from left to right across columns!");

            foreach (GameObject bonusPiece in bonusPositions)
            {
                if (bonusPiece != null)
                {
                    StartCoroutine(AnimateMatchEffect(bonusPiece, 0.1f));
                }
            }

        }
    }


    private void AddToBalance(GameObject piece)
    {
        SymbolInfo info = piece.GetComponent<SymbolInfo>();
        if (info != null && !info.isMultiplier && !info.isBonus)
        {
            playerBalance += info.value;
            UpdateBalanceUI();
        }
    }

    private void TriggerExplosion(GameObject piece)
    {
        if (explosionPrefab == null || piece == null) return;

        SymbolInfo info = piece.GetComponent<SymbolInfo>();
        if (info == null) return;

        Color symbolColor = info.tokenColor;

        GameObject explosion = Instantiate(explosionPrefab, piece.transform.position, Quaternion.identity);

        ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = symbolColor;
        }
        else
        {
            Renderer rend = explosion.GetComponent<Renderer>();
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                rend.material.color = symbolColor;
            }
        }

        Destroy(explosion, 1f);
    }

    private void UpdateBalanceUI()
    {
        if (balanceText != null)
            balanceText.text = $"{playerBalance:F2}USDT";
    }

    private void UpdateBetUI()
    {
        if (betText != null)
            betText.text = $"{betAmount:F2} USDT";
    }

    public void IncreaseBet()
    {
        betAmount += 5f;
        UpdateBetUI();
    }

    public void DecreaseBet()
    {
        if (betAmount > minBetAmount)
        {
            betAmount -= 5f;
            UpdateBetUI();
        }
        else
        {
            Debug.Log("Minimum bet amount reached.");
        }
    }

    private bool AreMatching(GameObject a, GameObject b, GameObject c)
    {
        if (a == null || b == null || c == null) return false;

        SymbolInfo aInfo = a.GetComponent<SymbolInfo>();
        SymbolInfo bInfo = b.GetComponent<SymbolInfo>();
        SymbolInfo cInfo = c.GetComponent<SymbolInfo>();

        if (aInfo == null || bInfo == null || cInfo == null)
            return false;

        if (aInfo.isMultiplier || bInfo.isMultiplier || cInfo.isMultiplier ||
            aInfo.isBonus || bInfo.isBonus || cInfo.isBonus)
            return false;

        return a.name == b.name && b.name == c.name;
    }

    private IEnumerator AnimateMatchEffect(GameObject piece, float delay)
    {
        if (piece == null) yield break;

        Vector3 originalScale = piece.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            piece.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float shakeDuration = 0.3f;
        float shakeMagnitude = 0.05f;
        elapsedTime = 0f;
        Vector3 originalPos = piece.transform.position;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            piece.transform.position = originalPos + randomOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = originalPos;
        piece.transform.localScale = originalScale;

        TriggerExplosion(piece);

        int foundRow = -1;
        int foundCol = -1;

        for (int i = 0; i < boardHeight; i++)
        {
            for (int j = 0; j < boardWidth; j++)
            {
                if (_gameBoard[i, j] == piece)
                {
                    matchedPositions.Add(new Vector2Int(i, j));
                    foundRow = i;
                    foundCol = j;
                    break;
                }
            }
            if (foundRow != -1) break;
        }

        if (foundRow != -1 && foundCol != -1)
        {
            Destroy(piece);
            _gameBoard[foundRow, foundCol] = null;
        }

        yield return new WaitForSeconds(delay);

        if (AllAnimationsComplete())
        {
            StartCoroutine(RespawnMatchedSymbols());
        }
    }

    private bool AllAnimationsComplete()
    {
        return matchedPositions.Count >= 3;
    }

    private IEnumerator RespawnMatchedSymbols()
    {
        yield return new WaitForSeconds(0.2f); 

        foreach (Vector2Int pos in matchedPositions)
        {
            if (_gameBoard[pos.x, pos.y] != null)
            {
                Destroy(_gameBoard[pos.x, pos.y]);
            }

            GameObject gridPosition = _board.transform.Find(pos.x + " " + pos.y).gameObject;
            GameObject pieceType = GetRandomPiece();
            GameObject thisPiece = Instantiate(pieceType, gridPosition.transform.position + _offset, Quaternion.identity);
            thisPiece.name = pieceType.name;
            thisPiece.transform.parent = gridPosition.transform;
            _gameBoard[pos.x, pos.y] = thisPiece;
        }

        matchedPositions.Clear();

        yield return new WaitForSeconds(0.3f); 
        CheckForMatches();
    }
}
