using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 diff;
    Transform player
    {
        get { return FindObjectOfType<PlayerController>().transform; }
    }
    // Start is called before the first frame update
    void Start()
    {
        diff = player.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position - diff;
    }
}
