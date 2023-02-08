using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    float timer = 0;
    private void OnEnable()
    {
        GameManager.Inst.isStopped[0] = true;
        GameManager.Inst.consoleManager.ConsoleUI.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            timer += Time.unscaledDeltaTime;
        }
        if(Input.GetMouseButtonUp(0))
        {
            timer = 0;
        }
        if(timer > 1)
        {
            gameObject.SetActive(false);
            GameManager.Inst.isStopped[0] = false;
            timer = 0;
        }
    }
}
