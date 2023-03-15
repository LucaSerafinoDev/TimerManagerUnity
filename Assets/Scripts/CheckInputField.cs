using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Michsky.UI.ModernUIPack;
using System;

public class CheckInputField : MonoBehaviour
{
    public enum InputFieldType
    {
        Hours,
        Minutes,
        Seconds,
        Users
    }

    public InputFieldType inputFieldType;

    public TMP_InputField inputField;
    public NotificationManager notification;


    // Start is called before the first frame update
    void Start()
    {
        inputField.onValidateInput += OnValidateInput;
        if(notification != null)
            notification.onOpen.AddListener(CloseNotification);
    }

    private char OnValidateInput(string text, int charIndex, char addedChar)
    {
        text = text + addedChar;
        bool charApproved = false;
        switch (inputFieldType)
        {
            case InputFieldType.Hours:
                {
                    if (Char.IsDigit(addedChar) && (text.Length <= 1 || (text.Length == 2 && Char.GetNumericValue(text[0]) <= 2 && Char.GetNumericValue(text[1]) <= 3)))
                        charApproved = true;
                    break;
                }
            case InputFieldType.Minutes:
                {
                    if (Char.IsDigit(addedChar) && (text.Length <= 1 || (text.Length == 2 && Char.GetNumericValue(text[0]) <= 5 && Char.GetNumericValue(text[1]) <= 9)))
                        charApproved = true;
                    break;
                }
            case InputFieldType.Seconds:
                {
                    if (Char.IsDigit(addedChar) && (text.Length <= 1 || (text.Length == 2 && Char.GetNumericValue(text[0]) <= 5 && Char.GetNumericValue(text[1]) <= 9)))
                        charApproved = true;
                    break;
                }
            case InputFieldType.Users:
                {
                    if (text.Length <= 10)
                        charApproved = true;
                    break;
                }

        }

        if(charApproved)
            return addedChar;
        else
        {
            if(inputFieldType != InputFieldType.Users)
            {
                notification.title = "MAX 23:59:59";
                notification.UpdateUI();
                notification.OpenNotification();
            }
            return '\0';
        }
    }

    public void CloseNotification()
    {
        StartCoroutine(CloseNotificationAfter(1f));
    }

    private IEnumerator CloseNotificationAfter(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        notification.CloseNotification();
    }
}
