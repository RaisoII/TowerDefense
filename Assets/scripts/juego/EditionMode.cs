using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditionMode : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField]private GameObject gridPrefab;
    [SerializeField] private ArchiveManager archiveManager;
    [SerializeField] private ClickTerrain clickTerrain;
    [SerializeField] private WaveManager waveManager;
    private bool isEdition;
    private List<GameObject> listGridPrefab;
    private Grid grid; 
    private int width,height; 
    private float cellSize; 
        
    private void Awake()
    {
        listGridPrefab = new List<GameObject>();
        isEdition = false;
        enabled = false;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
            enabledCellPrefab();
        else if(Input.GetKey(KeyCode.Z))
            disabledCellPrefab();
    }

    public void CreateArchive()
    {
        SetActiveScripts(false);
        isEdition = true;
        createGridPrefab();
    }

    public void ListenerModeEdition()
    {
        isEdition = !isEdition;

        if(isEdition)
        {
            SetActiveScripts(false);
            createGridPrefab();
            List<Vector2> listValidated = archiveManager.GetListCellWalkable();  // tengo que verificar si existe el maldito archivo
            setCellsPrefabWalkables(listValidated);
        }
        else
        {
            enabled = false;
            SaveData();
            destroyGridPrefab();
            SetActiveScripts(true);
        }
    }

    private void setCellsPrefabWalkables(List<Vector2> listValidated)
    {
        foreach(Vector2 pos in listValidated)
        {
            foreach(GameObject cellPrefab in listGridPrefab)
            {
                if((Vector2)cellPrefab.transform.position == pos)
                {
                    cellPrefab.transform.GetChild(1).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void createGridPrefab()
    {
        grid = gridManager.GetGrid();
        width = grid.GetWidth();
        height = grid.GetHeight();
        cellSize = grid.GetCellSize();
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x * cellSize,y * cellSize);
                GameObject gridInstance = Instantiate(gridPrefab,pos,Quaternion.identity);
                listGridPrefab.Add(gridInstance);
                gridInstance.transform.localScale = new Vector2(cellSize,cellSize);
            }
        }

        enabled = true;
    }

    private void SaveData()
    {
        List<Vector2> listVector = new List<Vector2>();

        foreach(GameObject cellPrefab in listGridPrefab)
        {   
            GameObject child = cellPrefab.transform.GetChild(1).gameObject;
            
            if(child.activeSelf)
            {
                listVector.Add((Vector2)cellPrefab.transform.position);
            }
        }
        
        archiveManager.SetGridData(listVector);
    }

    private void destroyGridPrefab()
    {
        while(listGridPrefab.Count > 0)
        {
            Destroy(listGridPrefab[0]);
            listGridPrefab.RemoveAt(0);
        }
    }

    private void enabledCellPrefab()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return; // Salir si está sobre la UI

        Cell cell = GetCellFromMousePosition();
        
        if(cell == null)
            return;
        
        GameObject cellSearch = searchGameObjectCell(cell.GetPos());
        cellSearch.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void disabledCellPrefab()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return; // Salir si está sobre la UI

        Cell cell = GetCellFromMousePosition();
        
        if(cell == null)
            return;
        
        GameObject cellSearch = searchGameObjectCell(cell.GetPos());

        cellSearch.transform.GetChild(1).gameObject.SetActive(false);
    }

    private GameObject searchGameObjectCell(Vector2 pos)
    {
        GameObject cellSearch = null;
        
        foreach(GameObject cellPrefab in listGridPrefab)
        {
            if((Vector2)cellPrefab.transform.position == pos)
            {
                cellSearch = cellPrefab;
                break;
            }
        }

        return cellSearch;
    }

    public Cell GetCellFromMousePosition()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f; // Ignoramos la coordenada Z para asegurarnos de que esté en el plano 2D

        // Ajustar la posición del mouse al centro de la celda más cercana
        float adjustedX = Mathf.Round(mouseWorldPosition.x / cellSize) * cellSize;
        float adjustedY = Mathf.Round(mouseWorldPosition.y / cellSize) * cellSize;

        // Convertir la posición ajustada a índices de la grilla
        int x = Mathf.FloorToInt(adjustedX / cellSize);
        int y = Mathf.FloorToInt(adjustedY / cellSize);

        // Verificar si la posición está dentro de los límites de la grilla
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid.cells[x, y];
        }
        else
            return null; // Si está fuera de los límites, devolvemos null
    }

    public void SetActiveScripts(bool state)
    {
        clickTerrain.enabled = state;
        waveManager.enabled = state;
    }
}
