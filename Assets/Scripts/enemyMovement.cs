using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    public GameObject player;

    public float speed = 10.0f;
    public float turnSpeed = 13.0f;


    public float horizontalInput;
    public float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * 1f * Time.deltaTime;

        transform.Translate(Vector3.forward * Time.deltaTime * turnSpeed * verticalInput);

        transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
    }
}
