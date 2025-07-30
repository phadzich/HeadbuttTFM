using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;

public class ControladorIdiomas : MonoBehaviour
{
    public static ControladorIdiomas Instance;

    public TMP_Dropdown TextSizeDropdown;
    public TMP_Dropdown subtitlesDropdown;
    public TMP_Dropdown modeDropdown;

    public List<LocalizedString> opcionesLocalizadas;
    public List<LocalizedString> opcionesSubtitulos;
    public List<LocalizedString> opcionesModo;

    private bool _active = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        TextSizeDropdown.options.Clear();
        for (int i = 0; i < opcionesLocalizadas.Count; i++)
        {
            var handle = opcionesLocalizadas[i].GetLocalizedStringAsync();
            yield return handle;
            TextSizeDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        TextSizeDropdown.RefreshShownValue();

        subtitlesDropdown.options.Clear();
        for (int i = 0; i < opcionesSubtitulos.Count; i++)
        {
            var handle = opcionesSubtitulos[i].GetLocalizedStringAsync();
            yield return handle;
            subtitlesDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        subtitlesDropdown.RefreshShownValue();

        modeDropdown.options.Clear();
        for (int i = 0; i < opcionesModo.Count; i++)
        {
            var handle = opcionesModo[i].GetLocalizedStringAsync();
            yield return handle;
            modeDropdown.options.Add(new TMP_Dropdown.OptionData(handle.Result));
        }
        modeDropdown.RefreshShownValue();
    }
}
