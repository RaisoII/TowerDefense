using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    public Cell[,] cells;
    private int width, height;
    private float cellSize;


    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        cells = new Cell[width, height];

        // Inicializar las celdas de la grilla
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(new Vector2(x * cellSize, y * cellSize), false); // Por defecto, todas las celdas son caminables
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return cells[x, y];
        }
        return null;
    }

    public void SetCellWalkable(int x, int y, bool walkable)
    {

        Cell cell = GetCell(x, y);
        
        if (cell != null)
        {
            cell.walkable = walkable;
        }
    }

    public bool IsWalkable(int x,int y)
    {
        Cell cell = GetCell(x,y);

        if(cell == null)
            return false;

        return cell.walkable;
    }

    public int GetWidth() => width;
    public int  GetHeight() => height;
    public float GetCellSize() => cellSize;
}
