using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public enum Windows
{
    Start,
    Game,
    Map,
    Home,
    EggVault
}

public class WindowManager : MonoBehaviour
{
    public List<GenericWindow> windows;

    public Windows defaultWindow;

    public Windows CurrentWindow { get; private set; }

    private void Start()
    {
        foreach (var window in windows)
        {
            window.Init(this);
            //window.Close();
            window.gameObject.SetActive(false);
        }

        CurrentWindow = defaultWindow;
        windows[(int)CurrentWindow].Open();
    }

    public void Open(Windows id)
    {
        windows[(int)CurrentWindow].Close();

        CurrentWindow = id;

        windows[(int)CurrentWindow].Open();
    }
}