using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewUserName : MonoBehaviour
{
    public string userName;
    public TextMeshProUGUI userNameText;

    public void DeleteNewUser()
    {
        NewUserManager.Instance.usersList.Remove(this);
        gameObject.transform.SetParent(null);
        Destroy(gameObject);
        NewUserManager.Instance.FormatNewUserList();
    }
}
