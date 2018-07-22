using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public static FieldGenerator Instance { get; private set; }
    public int xSize;
    public int ySize;

    [SerializeField] float cellSize;
    [SerializeField] float cellSpacing;

    [SerializeField] Cell cellPrefab;

    public Cell[,] cells;

    public List<Cell> GetEmptyCells()
    {
        List<Cell> emptyCells = new List<Cell>();
        foreach (var cell in cells)
        {
            if (cell.teamControl == Team.Empty)
                emptyCells.Add(cell);
        }
        return emptyCells;
    }

    private void Awake()
    {
        Instance = this;
    }
    public void GenerateField()
    {
        cells = new Cell[xSize, ySize];
        //sum of all cells and spaces minus half of size cz cell starts in middle

        var startX = ( ( xSize * cellSize ) + cellSpacing * ( xSize - 1 ) ) / 2 - ( cellSize / 2 );
        var startY = ( ( ySize * cellSize ) + cellSpacing * ( ySize - 1 ) ) / 2 - ( cellSize / 2 );

        for (var i = 0; i < xSize; i++)
        {
            for (var j = 0; j < ySize; j++)
            {
                var pos = new Vector2(i * cellSize - startX + cellSpacing * i, j * cellSize - startY + cellSpacing * j);
                var cell = Instantiate(cellPrefab, pos + (Vector2)transform.position, Quaternion.identity, transform);
                cell.transform.localScale = Vector3.one * cellSize;
                cell.Initialize(i, j);

                cells[i, j] = cell;
            }
        }
    }
}
