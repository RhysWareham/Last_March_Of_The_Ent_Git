using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAimingScript : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Transform head;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float timeCounter = 0;
    [Range(0, 1)]
    public float strengthOfGravity = 0;
    bool Direction = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Direction = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Direction = false;
        }
        if (Direction)
            timeCounter += Time.deltaTime * strengthOfGravity;
        else
            timeCounter -= Time.deltaTime * strengthOfGravity;

        float x = Mathf.Cos(timeCounter) * radius;
        float y = Mathf.Sin(timeCounter) * radius;
        float z = 0;
        transform.position = new Vector3(x, y, z);

        //head.transform.eulerAngles = new Vector3(head.transform.rotation.eulerAngles.x, head.transform.rotation.eulerAngles.y, z);
        var direction = transform.position - head.transform.position;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.AngleAxis((angle * -1) + 90, Vector3.forward);

        //head.transform.up = (transform.position - head.transform.position);
        //head.transform.eulerAngles = new Vector3(head.transform.rotation.eulerAngles.x, head.transform.rotation.eulerAngles.y, z - 90);
    }
}
