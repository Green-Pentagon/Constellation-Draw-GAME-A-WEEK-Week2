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
                Vector3 rawTransformPosition;
                float[,] transformPositions = new float[shapes.Length,3];
                int index = 0;

                foreach (GameObject shape in shapes)
                {
                    shapeSplines[index] = (shape.GetComponent<SpriteShapeController>().spline);
                    rawTransformPosition = shape.GetComponent<SpriteShapeController>().transform.position;
                    
                    transformPositions[index,0] = rawTransformPosition.x;
                    transformPositions[index,1] = rawTransformPosition.y;
                    transformPositions[index,2] = rawTransformPosition.z;

                    index++;
                }

                SpriteShapeSaveAndLoad.Save(shapeSplines , transformPositions);
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
