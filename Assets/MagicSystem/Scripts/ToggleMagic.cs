using UnityEngine;

public class ToggleMagic : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    private void Awake()
    {
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        SetCursorState(CursorLockMode.Locked, false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (objectToDisable != null)
            {
                bool isActive = objectToDisable.activeSelf;
                objectToDisable.SetActive(!isActive);

                if (objectToDisable.activeSelf)
                {
                    SetCursorState(CursorLockMode.None, true);
                }
                else
                {
                    SetCursorState(CursorLockMode.Locked, false);
                }
            }
        }
    }

    private void SetCursorState(CursorLockMode lockMode, bool visible)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = visible;
    }
}
