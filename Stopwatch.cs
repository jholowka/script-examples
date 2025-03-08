using UnityEngine;
using TMPro;

public abstract class Stopwatch : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private bool showHours = true;
    [SerializeField] private bool showMinutes = true;
    [SerializeField] private bool showSeconds = true;
    [Tooltip("Should the stopwatch start running immediately or does it need to be started through some code event?")]
    [SerializeField] private bool startImmediate;

    [Space(10)]
    [Header("References")]
    [SerializeField] private float letterFontSize = 24f;
    [SerializeField] private TextMeshProUGUI stopwatchText;

    public float TimePassed { get; private set; } = 0f;
    private const float maxTime = 36000; // 10 hours

    protected bool runStopwatch;
    protected bool pauseStopwatch;

    private void Awake()
    {
        runStopwatch = startImmediate ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!runStopwatch) return;
        if (pauseStopwatch) return;

        TimePassed += Time.deltaTime;
        UpdateText(TimePassed);
    }

    protected virtual void UpdateText(float timePassed)
    {
        int _hours = 0;
        int _minutes = 0;
        if (timePassed > 3600)
        {
            _hours = Mathf.FloorToInt(timePassed / 3600f); // Calculate total hours
        }

        if (timePassed >= 60)
        {
            _minutes = Mathf.FloorToInt((timePassed % 3600) / 60f); // Calculate remaining minutes
        }

        int _seconds = Mathf.FloorToInt(timePassed % 60f); // Calculate remaining seconds

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

        stopwatchText.text = formattedText.Trim(); // Trim to remove trailing space
    }

    public virtual void Pause()
    {
        pauseStopwatch = true;
    }

    public virtual void Unpause()
    {
        pauseStopwatch = false;
    }
}
