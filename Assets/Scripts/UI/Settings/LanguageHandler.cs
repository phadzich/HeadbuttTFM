using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;

public class LanguageHandler : MonoBehaviour
{
    private bool _active = false;

    public void ChangeLocale(int localeID)
    {
        if (_active) return;
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID)
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];

        _active = false;
    }

}
