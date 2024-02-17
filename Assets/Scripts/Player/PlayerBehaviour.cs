using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerBehaviour : MonoBehaviour
{
    
    public SpriteShapeController ORIGINALCopySSC;
    private SpriteShapeSaveAndLoad SpriteShapeSaveAndLoad;
    private SpriteShapeController spriteShapeController;
    private Spline ShapeSpline;
    private Rigidbody2D rigidbody;

    private float propulsionMultiplier = 15.0f; //multiplier to force of propulsion
    private int currentVerticeIndex = 1;
    private bool drawing = true;

        // Start is called before the first frame update
        void Start()
    {
        //LOAD SAVE
        SpriteShapeSaveAndLoad = GetComponent<SpriteShapeSaveAndLoad>();
        SpriteShapeSaveAndLoad.Load();


        rigidbody = GetComponent<Rigidbody2D>();

        //instantiate a new copy of shape from prefab
        spriteShapeController = Instantiate(ORIGINALCopySSC, Vector3.zero, transform.rotation);

        ShapeSpline = spriteShapeController.spline;
        ShapeSpline.SetPosition(0, transform.position);
        ShapeSpline.InsertPointAt(currentVerticeIndex, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        bool AddVertice = Input.GetKeyDown(KeyCode.Mouse0);
        bool RemoveVertice = Input.GetKeyDown(KeyCode.Mouse1);
        bool FinaliseShape = Input.GetKeyDown(KeyCode.Return); //double check this works for regular enter key!

        if (drawing)
        {
            if (AddVertice)
            {
                currentVerticeIndex++;
                ShapeSpline.InsertPointAt(currentVerticeIndex, transform.position);
            }
            else if (RemoveVertice && currentVerticeIndex > 1)
            {
                ShapeSpline.RemovePointAt(currentVerticeIndex);
                currentVerticeIndex--;
            }
            else if (FinaliseShape)
            {
                drawing = false;


                GameObject[] shapes = GameObject.FindGameObjectsWithTag("SpriteShape");
                Spline[] shapeSplines = new Spline[shapes.Length];
                Vector3 TemporaryVector;
                float[] transformPositions = new float[3];
                float[][,] verticePositions = new float[shapes.Length][,];

                //float[][][] splineInformation = new float[shapes.Length][][];

                int index = 0;

                foreach (GameObject shape in shapes)
                {
                    TemporaryVector = shape.GetComponent<SpriteShapeController>().transform.position;
                    transformPositions[0] = TemporaryVector.x;
                    transformPositions[1] = TemporaryVector.y;
                    transformPositions[2] = TemporaryVector.z;


                    //grab the shape's spin- i mean spline
                    shapeSplines[index] = (shape.GetComponent<SpriteShapeController>().spline);
                    verticePositions[index] = new float[shapeSplines[index].GetPointCount(), 3];

                    

                    for (int verticeIndex = 0; verticeIndex < shapeSplines[index].GetPointCount(); verticeIndex++)
                    {
                        TemporaryVector = shapeSplines[index].GetPosition(verticeIndex);
                        verticePositions[index][verticeIndex, 0] = TemporaryVector.x;
                        verticePositions[index][verticeIndex, 1] = TemporaryVector.y;
                        verticePositions[index][verticeIndex, 2] = TemporaryVector.z;

                    }

                    //splineInformation[index] = new float[shapeSplines[index].GetPointCount()][];
                    //splineInformation[index][0] = transformPositions;
                    
                    //perhaps instead utilise the new keyword??? this may need to be possiblt moved about...
                    //splineInformation[index][0][0] = transformPositions[0];
                    //splineInformation[index][0][1] = transformPositions[1];
                    //splineInformation[index][0][2] = transformPositions[2];

                    //for (int j = 0; j < shapeSplines[index].GetPointCount(); j++)
                    //{
                    //    splineInformation[index][j + 1][0] = verticePositions[index,0];
                    //    splineInformation[index][j + 1][1] = verticePositions[index,1];
                    //    splineInformation[index][j + 1][2] = verticePositions[index,2];
                    //}
                    

                    index++;
                }

                SpriteShapeSaveAndLoad.Save(transformPositions, verticePositions);
            }
        }
        
        

    }

    private void FixedUpdate()
    {

        //movement
        float x_movement = Input.GetAxis("Horizontal");
        float y_movement = Input.GetAxis("Vertical");
        
        if (drawing)
        {
            if ((x_movement != 0.0f || y_movement != 0.0f)) //MOVEMENT TRIGGERED
            {
                rigidbody.AddForce(new Vector2(x_movement, y_movement) * propulsionMultiplier, ForceMode2D.Force);

            }
        }
        try
        {
            ShapeSpline.SetPosition(currentVerticeIndex, transform.position);
        }
        catch
        {

        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Refill")
        {
            Destroy(collision.gameObject);
        }
    }
}
