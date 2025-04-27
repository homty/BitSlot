using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float FieldHeight, FieldWidth;
    [SerializeField]
    private GameObject[] gamePieces;
    private GameObject _field;
    private GameObject[,] _SlotField;
    private Vector3 _offset = new Vector3(0, 0, -1);

    void Start()
    {
        _field = GameObject.Find("SlotField");
        _SlotField = new GameObject[(int)FieldHeight, (int)FieldWidth];
        for (int i = 0; i < FieldHeight; i++)
        {
            for (int j = 0; j < FieldWidth; j++)
            {
                GameObject gridPosition = _field.transform.Find(i + " " + j).gameObject;
                GameObject pieceType = gamePieces[Random.Range(0, gamePieces.Length)];
                GameObject thisPiece = Instantiate(pieceType, gridPosition.transform.position + _offset, Quaternion.identity);
                thisPiece.name = pieceType.name;
                thisPiece.transform.parent = gridPosition.transform;
                _SlotField[i, j] = thisPiece;
            }
        }
    }

    void Update()
    {
        
    }
}
