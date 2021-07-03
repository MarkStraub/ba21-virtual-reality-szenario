using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LanguageManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CheckEnglish()
    {
        // Change Settings to English
        SettingsManager settingsManager = new SettingsManager(autoSave: false);
        settingsManager.SetValueByName("language", "en_US");

        LanguageManager languageManager = new LanguageManager(settingsManager);

        // Settings
        Assert.AreEqual("Back", languageManager.GetValue("back"), message: "Wrong translation for settings (en_US)");

        // Languages
        Assert.AreEqual("English", languageManager.GetValue("en"), message: "Wrong translation for en (en_US)");
        Assert.AreEqual("German", languageManager.GetValue("de"), message: "Wrong translation for de (en_US)");
        Assert.AreEqual("Swahili", languageManager.GetValue("sw"), message: "Wrong translation for sw (en_US)");

    }

    // A Test behaves as an ordinary method
    [Test]
    public void CheckGerman()
    {
        // Change Settings to English
        SettingsManager settingsManager = new SettingsManager(autoSave: false);
        settingsManager.SetValueByName("language", "de_CH");

        LanguageManager languageManager = new LanguageManager(settingsManager);

        // Settings
        Assert.AreEqual("Zurück", languageManager.GetValue("back"), message: "Wrong translation for settings (de_CH)");

        // Languages
        Assert.AreEqual("Englisch", languageManager.GetValue("en"), message: "Wrong translation for en (de_CH)");
        Assert.AreEqual("Deutsch", languageManager.GetValue("de"), message: "Wrong translation for en (de_CH)");
        Assert.AreEqual("Swahili", languageManager.GetValue("sw"), message: "Wrong translation for sw (de_CH)");

    }

    // A Test behaves as an ordinary method
    [Test]
    public void CheckSwahili()
    {
        // Change Settings to English
        SettingsManager settingsManager = new SettingsManager(autoSave: false);
        settingsManager.SetValueByName("language", "sw_TZ");

        LanguageManager languageManager = new LanguageManager(settingsManager);

        // Settings
        //Assert.AreEqual(null, languageManager.GetValue("settings"), message: "Wrong translation for settings (sw_TZ)");

        // Languages
        //Assert.AreEqual("Englisch", languageManager.GetValue("en"), message: "Wrong translation for en (sw_TZ)");
        //Assert.AreEqual("Swahili", languageManager.GetValue("sw"), message: "Wrong translation for sw (sw_TZ)");

    }
}
