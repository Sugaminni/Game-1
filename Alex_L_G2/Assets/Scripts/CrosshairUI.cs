using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;
    public RectTransform leftBar;
    public RectTransform rightBar;

    [Header("Tuning")]
    public float restGap = 10f;    // px from center at rest
    public float maxExtra = 24f;   // expansion on fire
    public float expandSpeed = 40f;
    public float shrinkSpeed = 20f;

    float targetGap, currentGap;

    // Initialize the crosshair
    void Start()
    {
        currentGap = targetGap = restGap;
        ApplyPositions(currentGap);
    }

    void Update()
    {
        // Smooth towards target gap
        float speed = (currentGap < targetGap) ? expandSpeed : shrinkSpeed;
        currentGap = Mathf.MoveTowards(currentGap, targetGap, speed * Time.deltaTime);
        ApplyPositions(currentGap);

        // Slowly return to rest
        if (Mathf.Approximately(currentGap, targetGap) && targetGap > restGap)
            targetGap = Mathf.Max(restGap, targetGap - shrinkSpeed * Time.deltaTime);
    }

    public void Kick()
    {
        targetGap = Mathf.Min(restGap + maxExtra, currentGap + maxExtra * 0.5f);
    }

    // Update the positions of the crosshair bars based on the current gap
    void ApplyPositions(float gap)
    {
        if (topBar)    topBar.anchoredPosition    = new Vector2(0f,  gap);
        if (bottomBar) bottomBar.anchoredPosition = new Vector2(0f, -gap);
        if (leftBar)   leftBar.anchoredPosition   = new Vector2(-gap, 0f);
        if (rightBar)  rightBar.anchoredPosition  = new Vector2( gap, 0f);
    }
}
