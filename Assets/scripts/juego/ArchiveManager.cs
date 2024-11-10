#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class ArchiveManager : MonoBehaviour
{
    [SerializeField] private EditionMode edition;
    [SerializeField] private GridManager gridManager;
    private List<Vector2> listPosWalkables;
    private string nameFile;
    public GridData gridData;

    private void Awake()
    {
        listPosWalkables = new List<Vector2>();
        gridData = new GridData(); 
        checkArchive();
    }

    public void checkArchive()
    {
        nameFile = SceneManager.GetActiveScene().name;
        
        TextAsset jsonFile = Resources.Load<TextAsset>(nameFile); // Intenta cargar el archivo desde Resources

        if (jsonFile != null) // Si el archivo existe en Resources
        {
            gridData = JsonUtility.FromJson<GridData>(jsonFile.text);
            listPosWalkables = gridData.cellsWalkable;
            edition.SetActiveScripts(true);
        }
        else
            edition.CreateArchive();

        gridManager.SetCellsWalkables(listPosWalkables);
    }

    public void SetGridData(List<Vector2> listVector)
    {
        listPosWalkables = listVector;
        gridData.cellsWalkable = listVector;
        SaveWalkableCells();
    }

     // Guardar datos en JSON
    public void SaveWalkableCells()
    {
        string json = JsonUtility.ToJson(gridData);

        #if UNITY_EDITOR
            // Guardar el archivo JSON en la carpeta Resources durante el modo de edición
            string path = Path.Combine(Application.dataPath, "Resources", nameFile + ".json");
            File.WriteAllText(path, json);
        
            // Refrescar el AssetDatabase para que Unity detecte el nuevo archivo
            AssetDatabase.Refresh();
        #else
            Debug.LogWarning("La función SaveWalkableCells solo está disponible en el Editor.");
        #endif
    }

    public List<Vector2> GetListCellWalkable() => listPosWalkables;
}
