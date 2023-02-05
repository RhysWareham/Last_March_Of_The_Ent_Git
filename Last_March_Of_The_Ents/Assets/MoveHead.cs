using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveHead : MonoBehaviour
{
    public Vector2[] points;
    public float speed;
    [SerializeField] private int lastPointPassed = 0;
    [SerializeField] private int targetPoint = 0;
    private int move = 0;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private bool leaningRight = true;
    [SerializeField] private bool canMoveRight = true;
    [SerializeField] private bool canMoveLeft = true;
    private Vector2 currentPosition = Vector2.zero;
    [SerializeField] private Vector2 targetPosition = new Vector2();
    [Range(-1, 1)]
    public float windStrength = 0;
    public float gravitySpeed;
    private bool dead = true;

    private Vector3 setupTransform;

    [SerializeField] private GameObject waistBone;
    [SerializeField] private Vector3 waistBoneSetupPos;

    [SerializeField] private Rigidbody2D body;

    [SerializeField] private TimerScript timer;

    private void Start()
    {
        lastPointPassed = ((points.Length - 1) / 2);
        targetPoint = lastPointPassed + 1;
        setupTransform = transform.localPosition;
        waistBoneSetupPos = waistBone.transform.localPosition;
    }

    public void SetupCharacter()
    {
        dead = false;
        body.gravityScale = 0;
        waistBone.transform.localPosition = waistBoneSetupPos;
        transform.localPosition = setupTransform;

        lastPointPassed = ((points.Length - 1) / 2);
        targetPoint = lastPointPassed + 1;

        move = 0;
        movingRight = true;
        canMoveRight = true;
        canMoveLeft = true;
        leaningRight = true;

        currentPosition= Vector2.zero;
    }

    void Update()
    {
        if (targetPoint == -1)
        {
            targetPoint = 0;
        }
        else if (targetPoint == 9)
        {
            targetPoint = 8;
        }
        if (GameControllerScript.gameCanStart)
        {
            if (!dead)
            {
                move = 0;

                currentPosition = transform.localPosition;

                //If head is on right side
                if (currentPosition.x > points[4].x)
                {
                    leaningRight = true;

                    //If current target point is further right than x
                    if (points[targetPoint].x > currentPosition.x)
                    {
                        targetPosition = points[targetPoint];
                    }
                    else
                    {
                        if (targetPoint + 1 < points.Length - 1)
                        {
                            targetPosition = points[targetPoint + 1];
                        }
                        else
                        {
                            //Can Fall
                            //KillPlayer();
                        }
                    }
                }
                //If head is on Left side
                else if (currentPosition.x < points[4].x)
                {
                    leaningRight = false;

                    //If current target point is further left than x
                    if (points[targetPoint].x < currentPosition.x)
                    {
                        //Target position should stay as the targetPoint
                        targetPosition = points[targetPoint];
                    }
                    else
                    {
                        if (targetPoint - 1 > 0)
                        {
                            targetPosition = points[targetPoint - 1];
                        }
                        else
                        {
                            //Can Fall
                            //KillPlayer();
                        }
                    }
                }

                if (lastPointPassed != 0 && lastPointPassed != points.Length - 1)
                {
                    canMoveLeft = true;
                    canMoveRight = true;
                }

                if (Input.GetKey(KeyCode.RightArrow) && canMoveRight)
                {
                    move = 1;
                    canMoveLeft = true;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && canMoveLeft)
                {
                    move = -1;
                    canMoveRight = true;
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
                    Vector2 direction = (targetPosition - currentPosition).normalized;

                    transform.localPosition = Vector2.MoveTowards(currentPosition, targetPosition, (speed * Time.deltaTime) + windStrength);


                    //If distance between current position and target position is less than 0.1f
                    if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
                    {
                        //If moving right
                        if (movingRight)
                        {
                            //If not at the end
                            if (lastPointPassed < points.Length - 2)
                            {
                                lastPointPassed = targetPoint;
                                targetPoint++;
                            }
                            else
                            {
                                canMoveRight = false;
                                //The ent is falling over!!!!!!!!!!!!!!!
                                KillPlayer();
                            }
                        }
                        else
                        {
                            //If not at the end
                            if (lastPointPassed > 1)
                            {
                                lastPointPassed = targetPoint;
                                targetPoint--;
                            }
                            else
                            {
                                canMoveLeft = false;
                                //The ent is falling over!!!!!!!!!!!!!!!
                                KillPlayer();
                            }
                        }
                    }
                }
                else if (GameControllerScript.gravity != 0)
                {
                    transform.localPosition = Vector2.MoveTowards(currentPosition, targetPosition, (GameControllerScript.gravity * Time.deltaTime) + windStrength);

                    //If distance between current position and target position is less than 0.1f
                    if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
                    {
                        //If moving right
                        if (leaningRight)
                        {
                            //If not at the end
                            if (lastPointPassed < points.Length - 1)
                            {
                                lastPointPassed = targetPoint;
                                targetPoint++;
                            }
                            else
                            {
                                canMoveRight = false;
                                //The ent is falling over!!!!!!!!!!!!!!!
                                KillPlayer();
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
                                canMoveLeft = false;
                                //The ent is falling over!!!!!!!!!!!!!!!
                                KillPlayer();
                            }
                        }
                    }
                }
            }
        }
    }

    private void KillPlayer()
    {
        dead = true;
        body.gravityScale = 1;
        GameControllerScript.gameCanStart = false;

        timer.OnDeath();
        Debug.Log("DEAD!");
    }

}
