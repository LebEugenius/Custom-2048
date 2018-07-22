using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{  
    public Team teamControl;
    public int teamPower;
    public bool locked;
    int xCoor;
    int yCoor;

    SpriteRenderer sprite;
    TextMesh powerText;
    Animator anim;

    public bool Move(MoveDirection dir)
    {
        if (teamControl == Team.Empty)
            return false;

        Cell cell = this;
        switch (dir)
        {
            case MoveDirection.Up:
                cell = GetUpCell(this);
                break;
            case MoveDirection.Down:
                cell = GetDownCell(this);
                break;
            case MoveDirection.Left:
                cell = GetLeftCell(this);
                break;
            case MoveDirection.Right:
                cell = GetRightCell(this);
                break;
            default:
                break;
        }

        if (cell == this)
            return false;

        if (cell.teamControl == Team.Empty)
        {    
            cell.SetTeam(teamControl, teamPower);
            SetTeam(Team.Empty, 0);
        }
        else if (cell.teamControl == teamControl)
        {
            cell.CombineTeams(this);
            SetTeam(Team.Empty, 0);
        }

        return true;
    }
    public Cell GetUpCell(Cell originalCell)
    {
        FieldGenerator field = FieldGenerator.Instance;
        //if it is not final cell
        if (yCoor != field.ySize - 1)
        {
            //Check next cell for empty or same as original
            Cell upCell = field.cells[xCoor, yCoor + 1];

            if ((!upCell.locked && upCell.teamControl == originalCell.teamControl && upCell.teamPower == originalCell.teamPower) || upCell.teamControl == Team.Empty)
            {
                Cell nextCell = upCell.GetUpCell(originalCell);
                //return next cell
                if (nextCell != this)
                    return nextCell;
            }
        }

        //return this cell if it is empty or same as original
        if (!locked && teamControl == originalCell.teamControl && teamPower == originalCell.teamPower || teamControl == Team.Empty)
            return this;

        //return previous one or original
        if (this != originalCell)
            return field.cells[xCoor, yCoor - 1];
        else
            return originalCell;
    }
    public Cell GetDownCell(Cell originalCell)
    {
        FieldGenerator field = FieldGenerator.Instance;
        //if it is not final cell
        if (yCoor != 0)
        {
            //Check next cell for empty or same as original
            //If next cell is not valid, return previous cell
            Cell downCell = field.cells[xCoor, yCoor - 1];

            if ((!downCell.locked && downCell.teamControl == originalCell.teamControl && downCell.teamPower == originalCell.teamPower) || downCell.teamControl == Team.Empty)
            {
                Cell nextCell = downCell.GetDownCell(originalCell);
                //return next cell
                if (nextCell != this)
                    return nextCell;
            }
        }

        //return this cell if it is empty or same as original
        if ((!locked && teamControl == originalCell.teamControl && teamPower == originalCell.teamPower) || teamControl == Team.Empty)
            return this;

        //return previous one or original
        if (this != originalCell)
            return field.cells[xCoor, yCoor + 1];
        else
            return originalCell;
    }
    public Cell GetRightCell(Cell originalCell)
    {
        FieldGenerator field = FieldGenerator.Instance;
        //if it is not final cell
        if (xCoor != field.xSize - 1)
        {
            //Check next cell for empty or same as original
            //If next cell is not valid, return previous cell
            Cell rightpCell = field.cells[xCoor + 1, yCoor];

            if ((!rightpCell.locked && rightpCell.teamControl == originalCell.teamControl && rightpCell.teamPower == originalCell.teamPower) || rightpCell.teamControl == Team.Empty)
            {
                Cell nextCell = rightpCell.GetRightCell(originalCell);
                //return next cell
                if (nextCell != this)
                    return nextCell;
            }
        }

        //return this cell if it is empty or same as original
        if ((!locked && teamControl == originalCell.teamControl && teamPower == originalCell.teamPower) || teamControl == Team.Empty)
            return this;

        //return previous one or original
        if (this != originalCell)
            return field.cells[xCoor - 1, yCoor];
        else
            return originalCell;
    }
    public Cell GetLeftCell(Cell originalCell)
    {
        FieldGenerator field = FieldGenerator.Instance;
        //if it is not final cell
        if (xCoor != 0)
        {
            //Check next cell for empty or same as original
            //If next cell is not valid, return previous cell
            Cell leftCell = field.cells[xCoor - 1, yCoor];

            if ((!leftCell.locked && leftCell.teamControl == originalCell.teamControl && leftCell.teamPower == originalCell.teamPower) || leftCell.teamControl == Team.Empty)
            {
                Cell nextCell = leftCell.GetLeftCell(originalCell);
                //return next cell
                if (nextCell != this)
                    return nextCell;
            }
        }

        //return this cell if it is empty or same as original
        if ((!locked && teamControl == originalCell.teamControl && teamPower == originalCell.teamPower) || teamControl == Team.Empty)
            return this;

        //return previous one or original
        if (this != originalCell)
            return field.cells[xCoor + 1, yCoor];
        else
            return originalCell;
    }

    public void Initialize(int x, int y)
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        powerText = GetComponentInChildren<TextMesh>();
        anim = GetComponent<Animator>();
        xCoor = x;
        yCoor = y;

        SetTeam(Team.Empty, 0);
    }

    public void CombineTeams(Cell alliedCell)
    {
        if (locked)
            return;

        if (alliedCell.teamPower == teamPower)
        {
            locked = true;
            teamPower *= 2;
            GameManager.Instance.score += teamPower;
            SetTeam(teamControl, teamPower);
        } 
    }

    public void SetTeam(Team team)
    {
        SetTeam(team, 2);
    }

    public void SetTeam(Team team, int power)
    {
        teamControl = team;
        teamPower = power;

        ChangeColor(teamControl);

        if (teamControl != Team.Empty)
        {
            anim.SetTrigger("Scale");
            powerText.text = teamPower.ToString();
        }
        else
            powerText.text = "";
    }

    public void ChangeColor(Team team)
    {
        float change = Mathf.Log(teamPower, 2) * 0.1f;

        Color color = new Color() { a = 1 };
        switch (team)
        {
            case Team.Red:
                color.r = 1;
                color.g = change;
                color.b = 0;          
                break;
            case Team.Green:
                color.r = 0;
                color.g = 1;
                color.b = change;
                break;
            case Team.Blue:
                color.r = change;
                color.g = 0;
                color.b = 1;
                break;
            case Team.Empty:
                color = Color.white;
                break;
            default:
                break;
        }

        sprite.color = color;
    }
}

public enum Team
{
    Red,
    Green,
    Blue,
    Empty
}

