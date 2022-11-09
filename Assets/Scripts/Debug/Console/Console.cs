using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Console : MonoBehaviour
{
    public TMP_InputField Input;

    public void SendCommand()
    {
        string Command = Input.text.ToString();
        var args = Command.Split(";");
        
    }

    void SplitCommand(string arg)
    {
        var function = arg.Split(" ");


    }

    void TranslateCommand(string functionWithValue)
    {
        if(functionWithValue[0].Equals("LoadScene"))
        {
            SceneManager.LoadScene(functionWithValue[1]);
        }



    }
}
