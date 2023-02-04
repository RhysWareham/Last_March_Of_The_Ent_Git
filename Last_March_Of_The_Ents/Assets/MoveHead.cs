using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHead : MonoBehaviour
{
    public Vector2[] points;
    public float speed;
    private int lastPointPassed = 0;
    private int targetPoint = 0;
    private int move = 0;
    private bool movingRight = true;
    private Vector2 targetPosition = new Vector2();

    private void Start()
    {
        lastPointPassed = ((points.Length - 1) / 2);
        targetPoint = lastPointPassed + 1;
    }

    void Update()
    {
        move = 0;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1;
        }

        if (move != 0)
        {
            //If we were moving right, and move is now -1
            if (movingRight && move == -1)
            {
                //Set targetPos to be previous point
                targetPosition = points[targetPoint - 1];
                targetPoint = targetPoint - 1;

                movingRight = false;
            }
            else if (!movingRight && move == 1)
            {
                //Set targetPos to be next point
                targetPosition = points[targetPoint + 1];
                targetPoint = targetPoint + 1;

                movingRight = true;
            }

            targetPosition = points[targetPoint];
            Vector2 currentPosition = transform.localPosition;
            Vector2 direction = (targetPosition - currentPosition).normalized;

            transform.localPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

            
            //If distance between current position and target position is less than 0.1f
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                //If moving right
                if(movingRight)
                {
                    //If not at the end
                    if (lastPointPassed < points.Length - 1)
                    {
                        lastPointPassed = targetPoint;
                        targetPoint++;
                    }
                    else
                    {
                        //The ent is falling over!!!!!!!!!!!!!!!
                    }
                }
                else
                {
                    //If not at the end
                    if (lastPointPassed > 0)
                    {
                        lastPointPassed = targetPoint;
                        targetPoint--;
                    }
                    else
                    {
                        //The ent is falling over!!!!!!!!!!!!!!!
                    }
                }
            }
            
        }
    }
}
