using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimerScript : MonoBehaviour
{
    private bool canStartTimer = false;
    private float timer = 0;

    private float timeToIncreaseGravity = 0;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI highScoreTimerText;
    [SerializeField] TextMeshProUGUI axeText;
    [SerializeField] TextMeshProUGUI highScoreAxeText;

    private InputAction _resetButton;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] MoveHead headScript;
    [SerializeField] TreeArmController armScript;
    [SerializeField] PlantRoots rootsScript;

    public GameObject axeObject;
    public float spawnMaxY;
    public float spawnMinY;
    public float spawnX;

    private float timeToThrowAxe = 0;
    private float currentTimeToThrowAxe = 0;
    private List<GameObject> axes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _resetButton = _playerInput.actions["Reset"];

        _resetButton.performed += Reset_Performed;
    }

    private void Reset_Performed(InputAction.CallbackContext obj)
    {
        canStartTimer = false;
        OnGameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (canStartTimer)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + (Mathf.Round(timer)).ToString();

            timeToIncreaseGravity += Time.deltaTime;

            if (timeToIncreaseGravity > 5)
            {
                timeToIncreaseGravity = 0;
                GameControllerScript.gravity += 0.5f;
            }
            timeToThrowAxe += Time.deltaTime;

            if (timeToThrowAxe > currentTimeToThrowAxe)
            {
                timeToThrowAxe = 0;
                currentTimeToThrowAxe = Random.Range(0.5f, 3);
                SpawnAxe();
            }

            axeText.text = "Axes Blocked: " + GameControllerScript.numOfAxesBlocked.ToString();

        }
    }

    private void SpawnAxe()
    {
        int leftOrRight = Random.Range(0,2) == 0 ? -1 : 1;
        Vector3 position = new Vector3(spawnX * leftOrRight, Random.Range(spawnMinY, spawnMaxY), 0.0f);
        GameObject axe = GameObject.Instantiate(axeObject, position, transform.rotation);

        axes.Add(axe);

        if(leftOrRight > 0)
        {
            axe.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void OnGameStart()
    {

        rootsScript.ResetRoots();
        headScript.SetupCharacter();
        armScript.ResetPlayer();
        canStartTimer = true;
        timer = 0;
        timerText.text = "Time: " + timer.ToString();
        timeToIncreaseGravity = 0;
        GameControllerScript.numOfAxesBlocked = 0;
        axeText.text = "Axes Blocked: " + GameControllerScript.numOfAxesBlocked.ToString();
        GameControllerScript.gravity = 0;
        GameControllerScript.gameCanStart = true;
        GameControllerScript.isHit = false;
    }

    public void OnDeath()
    {
        canStartTimer = false;
        if (timer > GameControllerScript.highscoreTime)
        {
            GameControllerScript.highscoreTime = Mathf.Round(timer);
            highScoreTimerText.text = "HighScore Timer: " + GameControllerScript.highscoreTime.ToString();
        }
        if(GameControllerScript.numOfAxesBlocked > GameControllerScript.highscoreAxes)
        {
            GameControllerScript.highscoreAxes = GameControllerScript.numOfAxesBlocked;
            highScoreAxeText.text = "HighScore Axes Blocked: " + GameControllerScript.highscoreAxes.ToString();
        }

        foreach(GameObject axe in axes)
        {
            if(axe != null)
            {
                Destroy(axe);

            }
        }
    }
}
