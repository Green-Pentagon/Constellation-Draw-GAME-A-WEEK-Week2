using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerBehaviour : MonoBehaviour
{
    public SpriteShapeController spriteShapeController;
    private Spline ShapeSpline;
    private Rigidbody2D rigidbody;

    private float propulsionMultiplier = 15.0f; //multiplier to force of propulsion
    private int currentVerticeIndex = 1;
    private bool trailEmitting = false;

        // Start is called before the first frame update
        void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

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

        if (AddVertice)
        {
            currentVerticeIndex++;
            ShapeSpline.InsertPointAt(currentVerticeIndex, transform.position);
        }
        else if (RemoveVertice && currentVerticeIndex >= 1)
        {
            ShapeSpline.RemovePointAt(currentVerticeIndex);
            currentVerticeIndex--;
        }

    }

    private void FixedUpdate()
    {

        //movement
        float x_movement = Input.GetAxis("Horizontal");
        float y_movement = Input.GetAxis("Vertical");
        
            if ((x_movement != 0.0f || y_movement != 0.0f)) //MOVEMENT TRIGGERED
            {
                rigidbody.AddForce(new Vector2(x_movement, y_movement) * propulsionMultiplier, ForceMode2D.Force);

            }

        ShapeSpline.SetPosition(currentVerticeIndex, transform.position);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Refill")
        {
            Destroy(collision.gameObject);
        }
    }
}
