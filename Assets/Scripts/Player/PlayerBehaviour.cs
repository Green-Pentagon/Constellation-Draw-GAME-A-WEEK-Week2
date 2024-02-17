using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public TextMeshProUGUI LifeReadout;
    
    private Rigidbody2D rigidbody;
    private SpriteRenderer SpriteRenderer;
    private TrailRenderer TrailRenderer;
    private float propulsionMultiplier = 15.0f; //multiplier to force of propulsion
    private bool trailEmitting = true;

        // Start is called before the first frame update
        void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TrailRenderer = GetComponent<TrailRenderer>();

    }

    // Update is called once per frame
    void Update()
    {


        bool toggleTrail = Input.GetKeyDown(KeyCode.Space);
        if (toggleTrail)
        {
            trailEmitting = !trailEmitting;
            TrailRenderer.emitting = trailEmitting;
        }
    }

    private void FixedUpdate()
    {
        float x_movement = Input.GetAxis("Horizontal");
        float y_movement = Input.GetAxis("Vertical");
        
            if ((x_movement != 0.0f || y_movement != 0.0f)) //MOVEMENT TRIGGERED
            {
                rigidbody.AddForce(new Vector2(x_movement, y_movement) * propulsionMultiplier, ForceMode2D.Force);

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
