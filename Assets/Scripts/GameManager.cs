using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] FieldGenerator field;
    [SerializeField] private FieldGenerator field2;

    public Team difficulty;
    [Range(4, 8)] public int size;
    public int score;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (difficulty == Team.Empty)
            difficulty = Team.Red;
        field.ySize = field.xSize = size;
        field2.ySize = field2.xSize = size;
        field2.GenerateField();
        field.GenerateField();
        StartGame();
    }

    void StartGame()
    {
        PlaceNewWarriors(((int)difficulty + 1) * 2);
    }
    public void PlaceNewWarriors(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PlaceNewWarrior();
        }
    }
    public void PlaceNewWarrior()
    {
        Cell randomCell;
        List<Cell> emptyCells = field.GetEmptyCells();

        if (emptyCells.Count == 0)
            return;

        randomCell = emptyCells[Random.Range(0, emptyCells.Count)];
        randomCell.SetTeam((Team)Random.Range(0, (int)difficulty + 1), Random.Range(1, 3) * 2);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            MoveWarriors(KeyCode.W);
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            MoveWarriors(KeyCode.S);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            MoveWarriors(KeyCode.D);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            MoveWarriors(KeyCode.A);     
        }
    }

    public void MoveWarriors(KeyCode key)
    {
        bool moved = false;

        switch (key)
        {
            case KeyCode.W:
            case KeyCode.UpArrow:
                moved = MoveWarriorsUp();
                break;
            case KeyCode.S:
            case KeyCode.DownArrow:
                moved = MoveWarriorsDown();
                break;
            case KeyCode.A:
            case KeyCode.LeftArrow:
                moved = MoveWarriorsLeft();
                break;
            case KeyCode.D:
            case KeyCode.RightArrow:
                moved = MoveWarriorsRight();
                break;
        }

        if(moved)
            PlaceNewWarrior();

        UnlockAllCells();
    }

    public bool MoveWarriorsUp()
    {
        int moved = 0;
        for (int i = 0; i < field.xSize; i++)
        {
            for (int j = field.ySize - 2; j >= 0; j--)
            {
                if (field.cells[i, j].Move(MoveDirection.Up))
                    moved++;
            }
        }
        return moved > 0;
    }
    public bool MoveWarriorsDown()
    {
        int moved = 0;
        for (int i = 0; i < field.xSize; i++)
        {
            for (int j = 0; j < field.ySize; j++)
            {
                if (field.cells[i, j].Move(MoveDirection.Down))
                    moved++;
            }
        }
        return moved > 0;
    }

    public bool MoveWarriorsRight()
    {
        int moved = 0;
        for (int i = field.xSize - 1; i >= 0 ; i--)
        {
            for (int j = field.ySize - 1; j >= 0; j--)
            {
                if (field.cells[i, j].Move(MoveDirection.Right))
                    moved++;
            }
        }
        return moved > 0;
    }
    public bool MoveWarriorsLeft()
    {
        int moved = 0;
        for (int i = 0; i < field.xSize; i++)
        {
            for (int j = field.ySize - 1; j >= 0; j--)
            {
                if (field.cells[i, j].Move(MoveDirection.Left))
                    moved++;
            }
        }
        return moved > 0;
    }

    public void UnlockAllCells()
    {
        for (int i = 0; i < field.xSize; i++)
        {
            for (int j = 0; j < field.ySize; j++)
            {
                field.cells[i, j].locked = false;
            }
        }
    }
}

public enum MoveDirection
{
    Up,
    Down,
    Left,
    Right
}

