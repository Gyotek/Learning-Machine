using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;
    private void Awake() { instance = this; }
        
    public enum Mode {Training, Run};

    public Mode mode;

    public Text textMode;
    public Text textButton;

    private void Start()
    {
        mode = Mode.Training;
    }

    public void ToggleMode()
    {
        if (mode == Mode.Training)
        {
            mode = Mode.Run;
            textMode.text = "Run Mode";
            textButton.text = "Switch to \n Training";

        }
        else if (mode == Mode.Run)
        {
            mode = Mode.Training;
            textMode.text = "Training Mode";
            textButton.text = "Switch to \n Run";
        }
        Physics.IgnoreLayerCollision(9, 9, mode == Mode.Training);
    }
}
