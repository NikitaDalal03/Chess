using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yudiz.StarterKit.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text whiteTimerText;
    [SerializeField] TMP_Text blackTimerText;
    [SerializeField] TextMeshProUGUI player1NameText;
    [SerializeField] TextMeshProUGUI player2NameText;
    [SerializeField] GameObject game;

    private float whiteCountdown = 120f; 
    private float blackCountdown = 120f;

    private bool whiteTimerRunning = false; 
    private bool blackTimerRunning = false;
    private bool isWhiteTurn = true;

    public static Timer instance;

    private void Start()
    {
        game = GameObject.FindGameObjectWithTag("GameController");
        instance = this;
    }

    void Update()
    {
        // Update timer based on whose turn it is
        if (isWhiteTurn && whiteTimerRunning)
        {
            whiteCountdown -= Time.deltaTime;
            DisplayTime(whiteCountdown, whiteTimerText);

            if (whiteCountdown <= 0f)
            {
                whiteTimerRunning = false;
                whiteCountdown = 0f;
                game.GetComponent<Game>().NextTurn();
            }
        }
        else if (!isWhiteTurn && blackTimerRunning)
        {
            blackCountdown -= Time.deltaTime;
            DisplayTime(blackCountdown, blackTimerText);

            if (blackCountdown <= 0f)
            {
                blackTimerRunning = false;
                blackCountdown = 0f;
                Debug.Log("WhiteTurn");
                game.GetComponent<Game>().NextTurn();
            }
        }
    }

    void DisplayTime(float timeToDisplay, TMP_Text timerUI)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartWhiteTimer()
    {
        player1NameText.text = NameScreen.player1Name;
        ResetBlackTimer();
        isWhiteTurn = true;
        whiteTimerText.enabled = true;
        blackTimerText.enabled = false;
        whiteTimerRunning = true;
        blackTimerRunning = false;
        player1NameText.enabled = true;
        player2NameText.enabled = false;
    }

    public void StartBlackTimer()
    {
        player2NameText.text = NameScreen.player2Name;
        ResetWhiteTimer();
        isWhiteTurn = false;
        blackTimerText.enabled = true;
        whiteTimerText.enabled = false;
        blackTimerRunning = true;
        whiteTimerRunning = false;
        player2NameText.enabled = true;
        player1NameText.enabled = false;
    }

    public void ResetWhiteTimer()
    {
        whiteCountdown = 120f;
        whiteTimerRunning = false;
        whiteTimerText.enabled = false;
        player1NameText.enabled = false;
    }

    public void ResetBlackTimer()
    {
        blackCountdown = 120f;
        blackTimerRunning = false;
        blackTimerText.enabled = false;
        player2NameText.enabled = false;
    }
}
