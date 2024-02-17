using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerTankRefill : MonoBehaviour
{
    public GameObject OxygenTank;
    public Object Player;
    private PlayerBehaviour PlayerBehaviour;
    float[] SpawnArea = { -45.0f, -70.0f,
                           45.0f, 70.0f };
    int minimumCount = 1;
    int maximumCount = 100;

    IEnumerator SpawnTanks()
    {
        for (int i = 1; i <= maximumCount; i++)
        {
            Instantiate(OxygenTank, new Vector3((Random.Range(SpawnArea[0], SpawnArea[2])), (Random.Range(SpawnArea[1], SpawnArea[3])), 0.0f) , transform.rotation);
        }
        yield return null;
    }

    IEnumerator DestroyAllInstances()
    {
        GameObject[] refills = GameObject.FindGameObjectsWithTag("Refill");
        for (int i = refills.Length-1; i > -1; i--)
        {
            Destroy(refills[i]);
        }
        yield return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerBehaviour = Player.GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        int TankCount = GameObject.FindGameObjectsWithTag("Refill").Length;
        if (PlayerBehaviour.PlayerDead())
        {
            StartCoroutine(DestroyAllInstances());
        }
        else
        {
            if (TankCount < minimumCount)
            {
                StartCoroutine(SpawnTanks());
            }
        }
        
        

    }
}
