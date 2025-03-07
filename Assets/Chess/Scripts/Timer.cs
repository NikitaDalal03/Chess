using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float countdown = 10f;
    private bool timerRunning = true;
    public static Timer instance;


    private void Start()
    {
        instance = this;
    }


    void Update()
    {
        if (timerRunning)
        {
            countdown -= Time.deltaTime;
            DisplayTime(countdown);

            if (countdown <= 0f)
            {
                timerRunning = false;
                countdown = 0f;

                //UiManager.instance.SwitchScreen(GameScreens.GameOver);
                //SoundManager.inst.PlaySound(SoundName.GameOver);

            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        countdown = 280f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void StartTimer()
    {
        timerRunning = true;
    }
}

