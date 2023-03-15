using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigTimerManager : MonoBehaviour
{
    int bigTimerStartingTime = 0;
    int bigTimerValue;
    [SerializeField]
    ProgressBar bigTimerProgressBar;

    bool isBigTimerRunning;

    [SerializeField]
    Image buttonRunPauseBigTimerImage;

    [SerializeField]
    Sprite buttonRunBigTimerSprite;
    [SerializeField]
    Sprite buttonPauseBigTimerSprite;

    [SerializeField]
    ModalWindowManager bigTimerModalWindow;
    [SerializeField]
    CustomInputField hrsBigTimerInputField;
    [SerializeField]
    CustomInputField minsBigTimerInputField;
    [SerializeField]
    CustomInputField secsBigTimerInputField;
    bool editingBigTimerInputField;

    [SerializeField]
    AudioSource bigTimerGavelSound;


    private void Start()
    {
        bigTimerStartingTime = PlayerPrefs.GetInt("bigTimerValue", 0);


        bigTimerValue = bigTimerStartingTime;
        isBigTimerRunning = false;

        bigTimerProgressBar.maxValue = bigTimerValue;
        bigTimerProgressBar.ChangeValue(bigTimerValue);
        bigTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(bigTimerValue);
    }

    public void OnClickBigTimer()
    {

        if (isBigTimerRunning)
        {
            PauseBigTimer();
        }
        else if (bigTimerValue > 0)
        {
            RunBigTimer();
            AppManager.Instance.userTimer.PauseUserTimer();
        }

    }

    private void RunBigTimer()
    {
        isBigTimerRunning = true;
        buttonRunPauseBigTimerImage.sprite = buttonPauseBigTimerSprite;
    }

    public void PauseBigTimer()
    {
        isBigTimerRunning = false;
        buttonRunPauseBigTimerImage.sprite = buttonRunBigTimerSprite;
    }

    public void ResetBigTimer()
    {
        //PauseBigTimer();
        
        bigTimerValue = bigTimerStartingTime;
        bigTimerProgressBar.ChangeValue(bigTimerStartingTime);
        bigTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(bigTimerStartingTime);
        if (!isBigTimerRunning)
        {
            if (buttonRunPauseBigTimerImage.sprite == buttonPauseBigTimerSprite)
                isBigTimerRunning = true;
        }
    }

    public void OnClickProgressBar()
    {
        bigTimerModalWindow.OpenWindow();
        editingBigTimerInputField = true;
    }

    public void OnClickBigTimerModalWindowCancel()
    {
        editingBigTimerInputField = false;
        bigTimerModalWindow.CloseWindow();
    }

    //
    //Bisogna sistemare il value max quando switchi user, se metto primo piu grande secondo sminchia, bisogna aggiungere campo max value al timer
    //

    int bigTimerHrsValue = 0;
    int bigTimerMinsValue = 0;
    int bigTimerSecsValue = 0;
    int bigTimerTotalValue = 0;
    public void OnClickBigTimerModalWindowOk()
    {
        editingBigTimerInputField = false;
        if (int.TryParse(hrsBigTimerInputField.inputText.text, out bigTimerHrsValue))
        {
            if (bigTimerHrsValue < 0)
            {
                //fai apparire notifica hours sbagliate
                return;
            }
        }
        if (int.TryParse(minsBigTimerInputField.inputText.text, out bigTimerMinsValue))
        {
            if (bigTimerMinsValue < 0)
            {
                //fai apparire notifica minutes sbagliate
                return;
            }
        }
        if (int.TryParse(secsBigTimerInputField.inputText.text, out bigTimerSecsValue))
        {
            if (bigTimerSecsValue < 0)
            {
                //fai apparire notifica seconds sbagliate
                return;
            }
        }
        bigTimerTotalValue = bigTimerHrsValue * 60 * 60 + bigTimerMinsValue * 60 + bigTimerSecsValue;
        if (bigTimerTotalValue > bigTimerProgressBar.maxValue)
        {
            bigTimerProgressBar.maxValue = bigTimerTotalValue;
            bigTimerStartingTime = bigTimerTotalValue;
        }
        bigTimerValue = bigTimerTotalValue;
        bigTimerProgressBar.ChangeValue(bigTimerTotalValue);
        bigTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(bigTimerTotalValue);

        bigTimerModalWindow.CloseWindow();
    }

    float countSec = 1;
    private void Update()
    {
        if (isBigTimerRunning && !editingBigTimerInputField)
        {
            countSec -= Time.deltaTime;
            if (countSec <= 0)
            {
                bigTimerValue--;
                countSec = 1;
            }
            if (bigTimerValue <= 0)
            {
                bigTimerValue = 0;
                bigTimerGavelSound.Play();
                isBigTimerRunning = false;
            }
            bigTimerProgressBar.ChangeValue(bigTimerValue);
            bigTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(bigTimerValue);
        }
    }
}
