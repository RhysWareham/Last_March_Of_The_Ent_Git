using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimerScript : MonoBehaviour
{
    private bool canStartTimer = false;
    private float timer = 0;

    private float timeToIncreaseGravity = 0;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI highScoreTimerText;


    private InputAction _resetButton;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] MoveHead headScript;
    [SerializeField] TreeArmController armScript;
    [SerializeField] PlantRoots rootsScript;

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
        GameControllerScript.gravity = 0;
        GameControllerScript.gameCanStart = true;
    }

    public void OnDeath()
    {
        canStartTimer = false;
        if (timer > GameControllerScript.highscoreTime)
        {
            GameControllerScript.highscoreTime = Mathf.Round(timer);
            highScoreTimerText.text = "HighScore Timer: " + GameControllerScript.highscoreTime.ToString();
        }
    }
}
