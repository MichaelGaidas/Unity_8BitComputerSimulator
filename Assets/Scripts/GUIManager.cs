using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public string codeStr;
    public bool start = false;
    public float delaySpeed = 3f;
    
    public InputField codeInputField;
    public InputField logInputField;

    public Button showCodeInputFieldBtn;
    public Button startProgramButton;
    public Button showLogButton;

    public Slider speedSlider;

    public Text sliderText;
    public Text hoverText;

    private bool showCodeInputField = false;
    private bool showLogInputField = false;

    void Awake()
    {
        codeInputField.onEndEdit.AddListener(delegate { OnFinishInput(); });
        showCodeInputFieldBtn.onClick.AddListener(ShowCodeInputButtonClick);
        startProgramButton.onClick.AddListener(StartProgramButtonClick);
        speedSlider.onValueChanged.AddListener(delegate { OnSliderChange(); });
        showLogButton.onClick.AddListener(ShowLogButtonClick);

    }

    private void OnFinishInput()
    {
        codeStr = codeInputField.text;
    }

  
    private void StartProgramButtonClick()
    {
        start = !start;
    }

    private void OnSliderChange()
    {
        sliderText.text = "Delay Speed: " + speedSlider.value;
        delaySpeed = speedSlider.value;
    }

    private void ShowCodeInputButtonClick()
    {
        showCodeInputField = !showCodeInputField;
        showLogInputField = false;

        codeInputField.gameObject.SetActive(showCodeInputField);
        speedSlider.gameObject.SetActive(showCodeInputField);
        sliderText.gameObject.SetActive(showCodeInputField);

        logInputField.gameObject.SetActive(false);
    }

    private void ShowLogButtonClick()
    {
        showLogInputField = !showLogInputField;
        showCodeInputField = false;

        logInputField.gameObject.SetActive(showLogInputField);
        
        codeInputField.gameObject.SetActive(false);
        speedSlider.gameObject.SetActive(false);
        sliderText.gameObject.SetActive(false);
    }

    public void UpdateLogInputField(string command)
    {
        if (logInputField.text == "")
            logInputField.text = command;
        else
            logInputField.text = logInputField.text + "\n" + command;
    }

    public void ClearLogInputField()
    {
        logInputField.text = "";
    }

    public void UpdateHoverText(string objectName)
    {
        hoverText.text = objectName;
    }

}
