using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    float timer = 0;
    private void OnEnable()
    {
        Time.timeScale = 0;
        GameManager.Inst.player.input.Disable();
        GameManager.Inst.consoleManager.ConsoleUI.SetActive(false);
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        GameManager.Inst.player.input.Enable();
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
            timer = 0;
        }
    }
}
