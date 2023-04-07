using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class SaveSystemManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<DataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static SaveSystemManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than once Save System Manager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data
        this.gameData = dataHandler.Load();

        // Load save data
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing to defaults.");
            NewGame();
        }

        // Push loaded data to other scripts
        foreach (DataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

        Debug.Log("Loaded currency count = " + gameData.currency);
        Debug.Log("Loaded score count = " + gameData.score);
        Debug.Log("Loaded health count = " + gameData.playerHealth);
    }

    public void SaveGame()
    {
        // Pass the data to other scripts
        foreach (DataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        // Save the data to a file.
        Debug.Log("Saved currency count = " + gameData.currency);
        Debug.Log("Saved score count = " + gameData.score);
        Debug.Log("Saved health count = " + gameData.playerHealth);
        
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<DataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<DataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().
            OfType<DataPersistence>();

        return new List<DataPersistence>(dataPersistenceObjects);
    }
}