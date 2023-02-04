using System.Buffers.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class LeaningControl : MonoBehaviour
{
    public KeyCode rightButton = KeyCode.RightArrow;
    public KeyCode leftButton = KeyCode.LeftArrow;
    public float leanSpeed = 10.0f;
    public float gravity = 9.8f;
    public float windSpeed = 5.0f;
    public float windDirection = 1.0f;
    public float headRadius = 1.0f;
    public Transform headTransform;

    private Rigidbody2D rb2d;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 headStartPosition;
    private Vector3 headOvalPosition;
    private float angle = 0.0f;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        headStartPosition = headTransform.position;
    }

    private void Update()
    {
        currentPosition = transform.position;

        if (Input.GetKey(rightButton))
        {
            rb2d.AddForce(Vector2.right * leanSpeed);
        }
        else if (Input.GetKey(leftButton))
        {
            rb2d.AddForce(Vector2.left * leanSpeed);
        }

        rb2d.AddForce(windDirection * windSpeed * Vector2.right * Time.deltaTime);
        rb2d.AddForce(Vector2.down * gravity * Time.deltaTime);

        angle += Time.deltaTime * windSpeed;
        headOvalPosition = headStartPosition + new Vector3(headRadius * Mathf.Cos(angle), headRadius * Mathf.Sin(angle), 0.0f);
        headTransform.position = headOvalPosition;

        if (currentPosition.x > startPosition.x + 0.5f || currentPosition.x < startPosition.x - 0.5f)
        {
            windDirection = -windDirection;
        }
    }
}
