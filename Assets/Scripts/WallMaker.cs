using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaker : MonoBehaviour
{
    Transform player
    {
        get { return FindObjectOfType<PlayerController>().transform; }
    }
    public GameObject wallPrefab;

    public Transform lastBlock;

    private float offset = 0.70383f;
    private Camera cam;

    Vector3 lastBlockPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastBlockPosition = lastBlock.position;
        InvokeRepeating("Makewall",0,0.25f);
        cam = Camera.main;
    }

    // Update is called once per frame
    

    void Makewall()
    {
        float distance = Vector3.Distance(player.position, lastBlockPosition);
        if (distance > cam.orthographicSize * 2) return;
        int chance = Random.Range(1, 11);
        Vector3 newPos;
        if (chance > 5)
        {
            newPos = new Vector3(lastBlockPosition.x + offset, lastBlockPosition.y, lastBlockPosition.z + offset);
        }
        else
        {
            newPos = new Vector3(lastBlockPosition.x - offset, lastBlockPosition.y, lastBlockPosition.z + offset);
        }

        bool enableCrystal = chance % 3 == 2;
        var newBlock = Instantiate(wallPrefab,transform);
        newBlock.transform.position = newPos;
        newBlock.transform.GetChild(0).gameObject.SetActive(enableCrystal);
        lastBlockPosition = newBlock.transform.position;
    }
}
