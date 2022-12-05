using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject GamepadPrompt;
    [SerializeField] private GameObject KeyboardPrompt;

    public void Show(bool show){
        if(!show) {
            GamepadPrompt.SetActive(false);
            KeyboardPrompt.SetActive(false);
        }
        else if (ControlsPrompt.DisplayedControlType == ControlsPrompt.ControlType.Gamepad) {
            GamepadPrompt.SetActive(true);
            KeyboardPrompt.SetActive(false);
        }
        else {
            GamepadPrompt.SetActive(false);
            KeyboardPrompt.SetActive(true);
        }

        gameObject.SetActive(show);
    }


}
