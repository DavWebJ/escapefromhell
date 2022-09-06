using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{

    [Header("Data Save Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;


    private GameData gameData;
    private List<IDataPersistance> dataPersistancesObject;
    private FileDataHandler dataHandler;
    public static SaveAndLoadManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("there is more than one save systemin the scene");
        

        }

        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,useEncryption);
        this.dataPersistancesObject = FindAllDataPersistenceObjects();
        loadGame();
    }

    public void newGame()
    {
        this.gameData = new GameData();
        return;
    }

    public bool checkIfGameDataExist()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("no data to load");
            return false;
        }

        return true;
    }

    public void loadGame()
    {
        this.gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("there is no data to load, start new game by default");
            newGame();
        }

        foreach (IDataPersistance datapersistneceObj in dataPersistancesObject)
        {
            datapersistneceObj.LoadData(gameData);
        }

       
    }

    public void saveGame()
    {
        foreach (IDataPersistance datapersistneceObj in dataPersistancesObject)
        {
            datapersistneceObj.SaveData(ref gameData);
        }

        // save data
        dataHandler.Save(gameData);
        
    }

    //private void OnApplicationQuit()
    //{
    //    saveGame();
    //}

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistancesObjects);
    }
}
