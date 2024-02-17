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
    private float propulsionMultiplier = 14.0f; //multiplier to force of propulsion
    private float LifeMaxCapacity = 10.0f; //o2 tank max capacity
    private float LifeRemaining;           //o2 tank remaining capacity
    private float LifeDecay = 1.0f;        //how many units of o2 in tank decay every tick
    private float LifeDecayDelay = 1.0f;   //delay between tank o2 decay
    private float LifeDecayDelayTimer;     //timer between next decay of o2 tank
    private bool trailEmitting = true;

    public bool PlayerDead()
    {
        return (Vector2.Equals(rigidbody.velocity , Vector2.zero) && LifeRemaining <= 0.0f);
    }


    void DecayLife(bool forMovement)
    {
        
        if (LifeDecayDelayTimer > 0.0f && forMovement) //if using for propulsion, decay o2 at a rate
        {
            LifeDecayDelayTimer -= Time.fixedDeltaTime;
        }
        else // if using for refilling suit OR delay from propulsion is over.
        {
            //NOTE: if this is exploited or allows the user to get more o2 from a fixed capacity (which it currently does) extra check can be added to fix this.
            LifeRemaining -= LifeDecay;
            LifeDecayDelayTimer = LifeDecayDelay;
        }
        
    }

    void UpdateSpriteAndTrail()
    {
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b,(LifeRemaining/LifeMaxCapacity));
        //TrailRenderer.startWidth = (LifeRemaining / LifeMaxCapacity);
    }


        // Start is called before the first frame update
        void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        TrailRenderer = GetComponent<TrailRenderer>();

        LifeRemaining = LifeMaxCapacity;
        LifeDecayDelayTimer = LifeDecayDelay;
    }

    // Update is called once per frame
    void Update()
    {

        LifeReadout.text = "Life Remaining: " + (int)LifeRemaining + "/" + LifeMaxCapacity;

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
        


        if (LifeRemaining > 0.0f) //if alive and able to move/refill
        {
            

            if ((x_movement != 0.0f || y_movement != 0.0f)) //MOVEMENT TRIGGERED
            {
                rigidbody.AddForce(new Vector2(x_movement, y_movement) * propulsionMultiplier, ForceMode2D.Force);
                //rigidbody.AddRelativeForce(new Vector2(x_movement, y_movement) * propulsionMultiplier, ForceMode2D.Force);
                DecayLife(true);
                UpdateSpriteAndTrail();
            }


        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Refill")
        {
            LifeRemaining = LifeMaxCapacity;
            Destroy(collision.gameObject);
        }
    }
}
