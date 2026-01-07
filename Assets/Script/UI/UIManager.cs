using UnityEngine;

public enum PopupLocation
{
    Center,
    bottom,
    Top
}


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Popup Anchors")]
    [SerializeField] private Transform centerAnchor;
    [SerializeField] private Transform bottomAnchor;
    [SerializeField] private Transform topAnchor;

    // Track active popups by location
    private GameObject centerPopup;
    private GameObject bottomPopup;
    private GameObject topPopup;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Push a popup to the screen at a given location.
    /// </summary>
    public void PushPopup(GameObject popupPrefab, PopupLocation location, bool pauseGame)
    {
        if (popupPrefab == null)
        {
            Debug.LogWarning("UIManager: Popup prefab is null.");
            return;
        }

        // Check if a popup already exists at this location
        if (GetActivePopup(location) != null)
        {
            Debug.LogWarning($"UIManager: Popup already active at {location}. Ignoring request.");
            return;
        }

        // Instantiate popup
        Transform anchor = GetAnchor(location);
        GameObject popup = Instantiate(popupPrefab, anchor);

        // Track it
        SetActivePopup(location, popup);

        // Pause game if needed
        if (pauseGame)
            Time.timeScale = 0f;
    }

    /// <summary>
    /// Remove popup at a specific location.
    /// </summary>
    public void PopPopup(PopupLocation location)
    {
        GameObject popup = GetActivePopup(location);

        if (popup != null)
        {
            Destroy(popup);
            SetActivePopup(location, null);

            // Resume game if no popups remain
            if (!AnyPopupActive())
                Time.timeScale = 1f;
        }
    }

    // ---------------- Helper Methods ----------------

    private Transform GetAnchor(PopupLocation location)
    {
        return location switch
        {
            PopupLocation.Center => centerAnchor,
            PopupLocation.bottom => bottomAnchor,
            PopupLocation.Top => topAnchor,
            _ => centerAnchor
        };
    }

    private GameObject GetActivePopup(PopupLocation location)
    {
        return location switch
        {
            PopupLocation.Center => centerPopup,
            PopupLocation.bottom => bottomPopup,
            PopupLocation.Top => topPopup,
            _ => null
        };
    }

    private void SetActivePopup(PopupLocation location, GameObject popup)
    {
        switch (location)
        {
            case PopupLocation.Center: centerPopup = popup; break;
            case PopupLocation.bottom: bottomPopup = popup; break;
            case PopupLocation.Top: topPopup = popup; break;
        }
    }

    private bool AnyPopupActive()
    {
        return centerPopup != null || bottomPopup != null || topPopup != null;
    }
}
