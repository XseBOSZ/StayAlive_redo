using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDiff : MonoBehaviour
{
    UIControler UIC;
    EQManager EQM;
    private void Awake()
    {
        EQM = gameObject.GetComponent<EQManager>();
        UIC = gameObject.GetComponent<UIControler>();
    }
    public void TakeAndDiffItem(string tag, RaycastHit hit)
    {
        switch (tag)
        {
            case "Untagged":
                UIC.DisablePopUp();
                break;
            case "item":
                var item = hit.transform.GetComponent<SciptableHolder>();
                UIC.EnablePopUp("Press [E] to pick up: " + item.ScriptableReference.Name);
                EQM.lookingAt = item.ScriptableReference;
                EQM.lookingAtObj = hit.transform.gameObject;
                break;
        }
    }
}
