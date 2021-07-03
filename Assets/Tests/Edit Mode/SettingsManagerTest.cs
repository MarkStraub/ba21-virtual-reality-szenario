using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SettingsManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void LoadFile()
    {
        SettingsManager settingsManager = new SettingsManager();
        Assert.AreNotEqual(null, settingsManager.GetStringValueByName("language"), message: "Language can not be NULL");
        Assert.AreNotEqual("", settingsManager.GetStringValueByName("language"), message: "Language can not be Empty");

    }

    [Test]
    public void TestBoolean()
    {
        SettingsManager settingsManager = new SettingsManager(autoSave: false);
        Assert.AreEqual(true, settingsManager.GetBooleanValueByName("th_1"), message: "Level should not be completed at the beginning");
        settingsManager.SetValueByName("th_1", false);
        Assert.AreEqual(false, settingsManager.GetBooleanValueByName("th_1"), message: "Level should now be completed");
    }

}
