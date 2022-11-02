using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManipulator : MonoBehaviour
{
    public UIControler UIC;
    public bool isLocked = false;
    private void Awake()
    {
        UIC = GetComponent<UIControler>();
    }
    public void LockCursor()
    {
        isLocked = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        isLocked = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void Update()
    {
        int activeGUIS = 0;
        for(int i = 0; i < UIC.UI_list.Length; i++)
        {
            if(UIC.UI_list[i].activeSelf == true)
            {
                activeGUIS++;
            }
        }
        if (activeGUIS > 0)
        {
            UnlockCursor();
        }
        else if (activeGUIS == 0)
        {
            LockCursor();
        }
    }
}
