using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;


public class ControladorIdiomas : MonoBehaviour
{
    private bool _active = false;

    void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0); // Tomo el ingles como predeterminado
        ChangeLocale(ID);
    }

    public void ChangeLocale(int localeID) // Con esto corrijo el bug de presionar muchas veces el boton de idioma
    {
        if (_active)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }

    private IEnumerator SetLocale(int localeID) // Del enumerador tomo un idioma de la lista
    {
        _active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        PlayerPrefs.SetInt("LocaleKey", localeID);
        _active = false;
    }
}

