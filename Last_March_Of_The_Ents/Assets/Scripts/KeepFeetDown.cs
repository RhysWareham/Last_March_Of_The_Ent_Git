using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class KeepFeetDown : MonoBehaviour
{
    Vector3 initialPos = Vector2.zero;
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        initialPos= transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition != initialPos)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, initialPos, speed * Time.deltaTime);
        }
    }
}
