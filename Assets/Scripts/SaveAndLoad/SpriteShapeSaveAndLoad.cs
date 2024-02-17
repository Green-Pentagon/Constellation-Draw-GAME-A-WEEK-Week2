//tutorial used to aid in creation: https://youtu.be/iB6h1FqPW2k

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeSaveAndLoad : MonoBehaviour
{
    //public SpriteShapeController ORIGINALCopySSC;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(SpriteShapeController[] shapeInstance)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;

        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/save.dat");
        }


        SaveData data = new SaveData(shapeInstance);
        bf.Serialize(file, data);

        file.Close();
        Debug.Log("Save SUCCESS!");
    }

    public void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);

            foreach(SpriteShapeController shape in data.shapeControllers)
            {
                Instantiate(shape);
            }
            file.Close();
            Debug.Log("Load SUCCESS!");
        }
    }


}


[Serializable]
public class SaveData
{
    public SpriteShapeController[] shapeControllers;
    
    public SaveData(SpriteShapeController[] shapeInstance)
    {
        shapeControllers = shapeInstance;
    }
}