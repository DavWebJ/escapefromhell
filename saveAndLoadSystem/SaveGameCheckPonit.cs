using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameCheckPonit : MonoBehaviour,IDataPersistance
{

    public Vector3 currentPostion;

    public void LoadData(GameData data)
    {
        //
    }

    public void SaveData(ref GameData data)
    {
        data.playerPos = currentPostion;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
            currentPostion = other.transform.position;
            print("position saved: " + currentPostion);
            SaveAndLoadManager.instance.saveGame();
        }
    }
}
