using UnityEngine;
using TMPro;

public abstract class Timer : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private bool showHours = true;
    [SerializeField] private bool showMinutes = true;
    [SerializeField] private bool showSeconds = true;

    [Space(10)]
    [Header("References")]
    [SerializeField] protected TextMeshProUGUI timerText;
    [SerializeField] private float letterFontSize = 24f;

    private float timeRemaining;
    private bool runTimer;

    public virtual void StartTimer(float seconds)
    {
        timeRemaining = seconds;
        runTimer = true;
        SetTimerText();
    }

    private void Update()
    {
        if (!runTimer) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndTimer();
        }

        SetTimerText();
    }

    public void ForceEndTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining = 0;
            EndTimer();
        }
    }

    protected virtual void EndTimer()
    {
        runTimer = false;
    }

    protected virtual void SetTimerText()
    {
        int _hours = 0;
        int _minutes = 0;

        if (timeRemaining > 3600)
        {
            _hours = Mathf.FloorToInt(timeRemaining / 3600f); // Calculate total hours
        }

        if (timeRemaining >= 60)
        {
            _minutes = Mathf.FloorToInt((timeRemaining % 3600) / 60f); // Calculate remaining minutes
        }

        int _seconds = Mathf.FloorToInt(timeRemaining % 60f); // Calculate remaining seconds

        string formattedText = "";

        if (showHours && _hours > 0)
        {
            formattedText += $"{_hours:F0}<size={letterFontSize}>h</size> ";
        }

        if (showMinutes && (showHours || _minutes > 0)) // Show minutes if enabled or if minutes exist
        {
            formattedText += $"{_minutes:F0}<size={letterFontSize}>m</size> ";
        }

        if (showSeconds)
        {
            formattedText += $"{_seconds:F0}<size={letterFontSize}>s</size>";
        }

        timerText.text = formattedText.Trim(); // Trim to remove trailing space
    }
}

