//tutorial used to aid in rasterization: https://youtu.be/iB6h1FqPW2k

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
    public GameObject CornerSprite;
    public GameObject PrefabSpriteShape;
    
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
            
            GameObject currentSpriteShape;
            Spline currentSpline;
            Vector3 currentCoordinate;

            for (int i = 0; i < data.splineCoords.Length;i++)
            {
                //create new sprite shape object and get its spline.
                currentSpriteShape = Instantiate(PrefabSpriteShape, new Vector3(0.0f, 0.0f, 0.0f), transform.rotation);
                currentSpline = currentSpriteShape.GetComponent<SpriteShapeController>().spline;

                for (int j = 0; j < (data.splineCoords[i].Length /3); j++)
                {
                    currentCoordinate = new Vector3(data.splineCoords[i][j, 0], data.splineCoords[i][j, 1], data.splineCoords[i][j, 2]);
                    
                    if (j <= currentSpline.GetPointCount() - 1)//if the spline has existing points to move, simply move them, else create new points
                    {
                        currentSpline.SetPosition(j, currentCoordinate);
                    }
                    else
                    {
                        currentSpline.InsertPointAt(j, currentCoordinate);
                    }

                    Instantiate(CornerSprite, currentCoordinate, transform.rotation);  //place debug sprite, can be removed or altered to fit theme later.
                }
            }


            file.Close();
            Debug.Log("Load SUCCESS!");
        }
    }


}

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