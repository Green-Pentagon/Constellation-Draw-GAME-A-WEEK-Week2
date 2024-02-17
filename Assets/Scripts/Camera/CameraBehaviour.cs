using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Object Player;
    private Transform PlayerTransform;
    private PlayerBehaviour PlayerBehaviour;

    private Camera Camera;
    private float lerpDelay = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTransform = Player.GetComponent<Transform>();
        PlayerBehaviour = Player.GetComponent<PlayerBehaviour>();
        Camera = gameObject.GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(PlayerTransform.position.x, PlayerTransform.position.y , transform.position.z), lerpDelay * Time.deltaTime);
        //transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, transform.position.z);
        //transform.rotation = PlayerTransform.rotation;
        if (PlayerBehaviour.PlayerDead())
        {
            Camera.orthographicSize = 80;
            transform.position = new Vector3(0.0f, 0.0f, transform.position.z);
        }
        
    }


}
