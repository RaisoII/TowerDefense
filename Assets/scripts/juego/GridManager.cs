using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width;      // Ancho de la grilla
    [SerializeField] private int height;     // Alto de la grilla
    [SerializeField] private float cellSize; // Tamaño de cada celda

    private Grid grid;

    private void Awake()
    {
        // Crear la grilla
        grid = new Grid(width, height, cellSize);
        // Ejemplo: Configurar celdas no caminables
        //grid.SetCellWalkable(1, 1, false);
        //grid.SetCellWalkable(2, 3, false);
    }

    public void SetCellsWalkables(List<Vector2> listPosWalkables)
    {
        foreach(Vector2 posWalkable in listPosWalkables)
        {
            int indexX = Mathf.FloorToInt(posWalkable.x / cellSize);
            int indexY = Mathf.FloorToInt(posWalkable.y / cellSize);
            grid.SetCellWalkable(indexX, indexY, true);
        }
    }

    public bool IsWalkable(Vector2 pos)
    {
        // Ajustar la posición del mouse al centro de la celda más cercana
        float adjustedX = Mathf.Round(pos.x / cellSize) * cellSize;
        float adjustedY = Mathf.Round(pos.y / cellSize) * cellSize;

        int indexX = Mathf.FloorToInt(adjustedX / cellSize);
        int indexY = Mathf.FloorToInt(adjustedY / cellSize);
        return grid.IsWalkable(indexX, indexY);
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell cell = grid.GetCell(x, y);
                    Gizmos.color = cell.walkable ? Color.green : Color.red;
                    Gizmos.DrawWireCube(cell.pos, Vector2.one * cellSize);
                }
            }
        }
    }

    public Grid GetGrid() => grid;
}
