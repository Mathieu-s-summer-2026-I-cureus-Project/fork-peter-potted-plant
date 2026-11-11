using UnityEngine;

public class KazooProcessor : MonoBehaviour
{
    public rigRevive rigReviveScript;
    public playerStateManager playerStateManager;

    [Header("Tuning Parameters")]
    [Tooltip("Smoothing factor (0.0 to 1.0). Lower = smoother, higher = faster response.")]
    [SerializeField] private float smoothFactor = 0.25f;

    [Tooltip("Minimum positive slope to trigger 'Blow Start'")]
    [SerializeField] private float blowStartSlope = 0.35f;

    [Tooltip("Negative slope threshold to confirm drop/release")]
    [SerializeField] private float releaseSlope = -0.2f;

    // [Tooltip("Minimum temperature rise (°C) to confirm a valid breath")]
    // [SerializeField] private float minTempRise = 0.35f;

    // Internal State Tracking
    private enum BreathState { Idle, Rising, Falling }
    private BreathState currentState = BreathState.Idle;

    private float filteredTemp;
    private float lastFilteredTemp;
    private float currentPeak = float.MinValue;
    private float currentTrough = float.MaxValue;

    public void ProcessTemperature(float rawTemp, float deltaTime)
    {
        // 1. Smooth the raw signal using EMA
        filteredTemp = Mathf.Lerp(filteredTemp, rawTemp, smoothFactor);

        // 2. Calculate rate of change (°C per second)
        float slope = (filteredTemp - lastFilteredTemp) / Mathf.Max(deltaTime, 0.001f);
        lastFilteredTemp = filteredTemp;

        // 3. State Machine & Event Dispatch
        switch (currentState)
        {
            case BreathState.Idle:
            case BreathState.Falling:
                // Detect start of breath (sharp rise)
                if (slope > blowStartSlope)
                {
                    currentState = BreathState.Rising;
                    currentPeak = filteredTemp;
                    OnBreathStart();
                }
                break;

            case BreathState.Rising:
                // Track highest value reached during this breath
                if (filteredTemp > currentPeak)
                {
                    currentPeak = filteredTemp;
                }

                // Detect peak transition (slope turns negative or drops below cutoff)
                if (slope < releaseSlope)
                {
                    currentState = BreathState.Falling;
                    OnBreathPeakReached(currentPeak);
                    OnBreathDrop();
                }
                break;
        }
    }

    // --- Game Event Callbacks ---
    private void OnBreathStart() {
        if (!playerStateManager.isDowned) {
        playerStateManager.StartRevivePulse();
        Debug.Log("Breath started");
        }
    }
    private void OnBreathPeakReached(float peakTemp) => Debug.Log($"Peak hit at: {peakTemp:F2}°C");
    private void OnBreathDrop()  {
        // rigReviveScript.reviveRight = 0.0f;
        // rigReviveScript.reviveLeft = 1.0f;
        Debug.Log("Breath dropped / cooling down.");
    }



}
