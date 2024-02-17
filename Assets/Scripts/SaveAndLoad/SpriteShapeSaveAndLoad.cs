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
    public GameObject debugSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(float[][,] coords)
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


        SaveData data = new SaveData( coords);
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

            for(int i = 0; i < data.splineCoords.Length;i++)
            {
                for (int j = 0; j < (data.splineCoords[i].Length /3); j++)
                {
                    Instantiate(debugSprite, new Vector3(data.splineCoords[i][j, 0], data.splineCoords[i][j, 1], data.splineCoords[i][j, 2]), transform.rotation);  
                }
            }


            file.Close();
            Debug.Log("Load SUCCESS!");
        }
    }


}


//[Serializable]
//public class BORKEDSaveData
//{
//    public SpriteShapeController[] shapeControllers;
    
//    public BORKEDSaveData(SpriteShapeController[] shapeInstance)
//    {
//        shapeControllers = shapeInstance;
//    }
//}

[Serializable]
public class SaveData
{
    //public float[] splineTransforms;
    public float[][,] splineCoords;

    public SaveData( float[][,] coords)
    {
        splineCoords = coords;
    }


}