using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlantRoots : MonoBehaviour
{
    [SerializeField] private Transform rootLeft;
    [SerializeField] private Transform rootRight;

    private Vector3 initialLeftPos = Vector3.zero;
    private Vector3 initialRightPos = Vector3.zero;

    [SerializeField] private Vector3 desiredPositionLeft;
    [SerializeField] private Vector3 desiredPositionRight;

    public float speed = 1;

    private int leftDirection = 0;
    private int rightDirection = 0;

    private bool canDigRight = true;
    private bool canDigLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        initialLeftPos = rootLeft.position;
        initialRightPos = rootRight.position;

        desiredPositionRight = new Vector3(rootRight.position.x, -0.75f, 0.3f);
        desiredPositionLeft = new Vector3(rootLeft.position.x, -0.77f, 0.3f);
    }

    public void ResetRoots()
    {
        rootLeft.position = initialLeftPos;
        rootRight.position = initialRightPos;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameControllerScript.gameCanStart)
        {
            leftDirection = rightDirection = 0;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rightDirection = 1;
                leftDirection = -1;

                if (Vector3.Distance(rootRight.position, desiredPositionRight) > 0.1f && canDigRight)
                {
                    rootRight.position = Vector3.MoveTowards(rootRight.position, desiredPositionRight, speed * Time.deltaTime * rightDirection);

                    if(Vector3.Distance(rootLeft.position, initialLeftPos) > 0.1f)
                    {
                        rootLeft.position = Vector3.MoveTowards(rootLeft.position, initialLeftPos, speed * Time.deltaTime);
                    }

                    canDigLeft = true;
                }
                else
                {
                    canDigRight = false;
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rightDirection = -1;
                leftDirection = 1;

                if (Vector3.Distance(rootLeft.position, desiredPositionLeft) > 0.1f && canDigLeft)
                {
                    rootLeft.position = Vector3.MoveTowards(rootLeft.position, desiredPositionLeft, speed * Time.deltaTime * leftDirection);
                
                    if (Vector3.Distance(rootRight.position, initialRightPos) > 0.1f)
                    {
                        rootRight.position = Vector3.MoveTowards(rootRight.position, initialRightPos, speed * Time.deltaTime);
                    }
                
                    canDigRight = true;
                }
                else
                {
                    canDigLeft = false;
                }
            }
        }
        }
}
