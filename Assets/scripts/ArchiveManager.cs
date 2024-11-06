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
        bool enabledScripts = true;
        nameFile = SceneManager.GetActiveScene().name;

        #if UNITY_WEBGL
            if (PlayerPrefs.HasKey(nameFile)) 
                LoadWalkableCells(); // Para WebGL usa PlayerPrefs
            else
            {
                enabledScripts = false;
                edition.CreateArchive();
                Debug.Log($"No se encontró un archivo de configuración de grilla para el nivel {nameFile}. Usando valores predeterminados.");
            }
        #else
            string filePath = Path.Combine(Application.persistentDataPath, $"{nameFile}.json");
            if (File.Exists(filePath)) 
                LoadWalkableCells(); // Para otras plataformas usa el archivo físico
            else
            {
                enabledScripts = false;
                edition.CreateArchive();
                Debug.Log($"No se encontró un archivo de configuración de grilla para el nivel {nameFile}. Usando valores predeterminados.");
            }
        #endif
        
        gridManager.SetCellsWalkables(listPosWalkables);
        
        if(enabledScripts)
            edition.SetActiveScripts(true);
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
        #if UNITY_WEBGL
            PlayerPrefs.SetString(nameFile, json);
            PlayerPrefs.Save();
        #else
            string path = Path.Combine(Application.persistentDataPath, nameFile + ".json");
            File.WriteAllText(path, json);
        #endif
    }

    // cargar datos en JSON
    private void LoadWalkableCells()
    {
        #if UNITY_WEBGL
            if (PlayerPrefs.HasKey(nameFile))
            {
                string json = PlayerPrefs.GetString(nameFile);
                gridData = JsonUtility.FromJson<GridData>(json);
            }
        #else
            string path = Path.Combine(Application.persistentDataPath, nameFile + ".json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                gridData = JsonUtility.FromJson<GridData>(json);
            }
        #endif

        listPosWalkables = gridData.cellsWalkable;
    }
    
    public List<Vector2> GetListCellWalkable() => listPosWalkables;
}
