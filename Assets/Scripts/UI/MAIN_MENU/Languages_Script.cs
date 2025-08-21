using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;

public class ControladorIdiomas : MonoBehaviour
{
    public static ControladorIdiomas Instance;

    public TMP_Dropdown ColorblindDropdown;
    public TMP_Dropdown CameraShakeDropdown;
    public TMP_Dropdown modeDropdown;

    public List<LocalizedString> opcionesColorBlind;
    public List<LocalizedString> opcionesCamaraShake;
    public List<LocalizedString> opcionesWindowMode;

    private bool _active = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);
    }

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
        PlayerPrefs.SetInt("LocaleKey", localeID);

        yield return UpdateDropdownOptions();

        _active = false;
    }

    private IEnumerator UpdateDropdownOptions()
    {
        ColorblindDropdown.options.Clear();
        for (int i = 0; i < opcionesColorBlind.Count; i++)
        {
            var handle = opcionesColorBlind[i].GetLocalizedStringAsync();
            yield return handle;
            ColorblindDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        ColorblindDropdown.RefreshShownValue();

        CameraShakeDropdown.options.Clear();
        for (int i = 0; i < opcionesCamaraShake.Count; i++)
        {
            var handle = opcionesCamaraShake[i].GetLocalizedStringAsync();
            yield return handle;
            CameraShakeDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        CameraShakeDropdown.RefreshShownValue();

        modeDropdown.options.Clear();
        for (int i = 0; i < opcionesWindowMode.Count; i++)
        {
            var handle = opcionesWindowMode[i].GetLocalizedStringAsync();
            yield return handle;
            modeDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        modeDropdown.RefreshShownValue();
    }
}
