using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;

public class UserTimer : MonoBehaviour
{
    public ProgressBar userTimerProgressBar;

    [SerializeField]
    Image buttonRunPauseUserTimerImage;

    [SerializeField]
    Sprite buttonRunUserTimerSprite;
    [SerializeField]
    Sprite buttonPauseUserTimerSprite;

    [SerializeField]
    Button buttonNextUser;
    [SerializeField]
    Button buttonPrevUser;

    [SerializeField]
    ModalWindowManager userTimerModalWindow;
    [SerializeField]
    CustomInputField hrsUserTimerInputField;
    [SerializeField]
    CustomInputField minsUserTimerInputField;
    [SerializeField]
    CustomInputField secsUserTimerInputField;

    bool isUserTimerRunning;
    bool editingUserTimerInputField;

    [SerializeField]
    AudioSource userTimerGavelSound;

    public void OnClickUserTimer()
    {
        if (isUserTimerRunning)
            PauseUserTimer();
        else if (AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer > 0)
        {
            RunUserTimer();
            AppManager.Instance.bigTimerManager.PauseBigTimer();
        }
    }

    //manca metodo gestione inputfield che spegne numero accende input field
    //endedit input field se numero maggiore di maxtimer allora nuovo max timer

    private void RunUserTimer()
    {
        isUserTimerRunning = true;

        buttonNextUser.interactable = false;
        buttonPrevUser.interactable = false;

        buttonRunPauseUserTimerImage.sprite = buttonPauseUserTimerSprite;
    }

    public void PauseUserTimer()
    {
        isUserTimerRunning = false;

        buttonNextUser.interactable = true;
        buttonPrevUser.interactable = true;

        buttonRunPauseUserTimerImage.sprite = buttonRunUserTimerSprite;
    }

    public void ResetUserTimer()
    {
        PauseUserTimer();
        userTimerProgressBar.ChangeValue(AppManager.Instance.usersTimerManager.userTimerStartingValue);
        AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer = AppManager.Instance.usersTimerManager.userTimerStartingValue;
        userTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(AppManager.Instance.usersTimerManager.userTimerStartingValue);
    }

    public void OnClickProgressBar()
    {
        userTimerModalWindow.OpenWindow();
        editingUserTimerInputField = true;
    }

    public void OnClickUserTimerModalWindowCancel()
    {
        editingUserTimerInputField = false;
        userTimerModalWindow.CloseWindow();
    }

    //
    //Bisogna impedire di editare input field mentre il timer va, bisogna dividere l'inputfield in 2
    //

    int userTimerHrsValue = 0;
    int userTimerMinsValue = 0;
    int userTimerSecsValue = 0;
    int userTimerTotalValue = 0;
    public void OnClickUserTimerModalWindowOk()
    {
        editingUserTimerInputField = false;
        if (int.TryParse(hrsUserTimerInputField.inputText.text, out userTimerHrsValue))
        {
            if (userTimerHrsValue < 0)
            {
                //fai apparire notifica hours sbagliate
                return;
            }
        }
        if (int.TryParse(minsUserTimerInputField.inputText.text, out userTimerMinsValue))
        {
            if (userTimerMinsValue < 0)
            {
                //fai apparire notifica hours sbagliate
                return;
            }
        }
        if (int.TryParse(secsUserTimerInputField.inputText.text, out userTimerSecsValue))
        {
            if (userTimerSecsValue < 0)
            {
                //fai apparire notifica hours sbagliate
                return;
            }
        }
        userTimerTotalValue = userTimerHrsValue * 60 * 60 + userTimerMinsValue * 60 + userTimerSecsValue;
        if (userTimerTotalValue > userTimerProgressBar.maxValue)
        {
            userTimerProgressBar.maxValue = userTimerTotalValue;
            PlayerPrefs.SetInt("userMaxTimer" + AppManager.Instance.usersTimerManager.usersHorizontalSelector.index, userTimerTotalValue);
            AppManager.Instance.usersTimerManager.userTimerStartingValue = userTimerTotalValue;
            AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].maxTimer = userTimerTotalValue;
        }
        AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer = userTimerTotalValue;
        userTimerProgressBar.ChangeValue(userTimerTotalValue);
        userTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(userTimerTotalValue);

        userTimerModalWindow.CloseWindow();
    }

    float countSec = 1;
    private void Update()
    {
        if (isUserTimerRunning && !editingUserTimerInputField)
        {
            countSec -= Time.deltaTime;
            if (countSec <= 0)
            {
                AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer--;
                countSec = 1;
            }
            if (AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer <= 0)
            {
                AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer = 0;
                userTimerGavelSound.Play();
                isUserTimerRunning = false;
            }
            userTimerProgressBar.ChangeValue(AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer);
            userTimerProgressBar.textPercent.text = "" + Utilities.FormattedBigTimerString(
                AppManager.Instance.usersTimerManager.usersList[AppManager.Instance.usersTimerManager.usersHorizontalSelector.index].currentTimer
                );
        }
    }
}
