﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable]
public class MenuButtonEvent : UnityEvent<bool> { }

public class MenuButtonWatcher : MonoBehaviour
{
    public MenuButtonEvent menuButtonPress;
    private bool lastButtonState = false;
    private List<UnityEngine.XR.InputDevice> allDevices;
    private List<UnityEngine.XR.InputDevice> devicesWithMenuButton;

    void Start()
    {
        if (menuButtonPress == null)
        {
            menuButtonPress = new MenuButtonEvent();
        }
        allDevices = new List<UnityEngine.XR.InputDevice>();
        devicesWithMenuButton = new List<UnityEngine.XR.InputDevice>();
        InputTracking.nodeAdded += InputTracking_nodeAdded;
    }

    // check for new input devices when new XRNode is added
    private void InputTracking_nodeAdded(XRNodeState obj)
    {
        updateInputDevices();
    }

    void Update()
    {
        bool tempState = false;
        bool invalidDeviceFound = false;
        foreach (var device in devicesWithMenuButton)
        {
            bool buttonState = false;
            tempState = device.isValid // the device is still valid
                        && device.TryGetFeatureValue(CommonUsages.menuButton, out buttonState) // did get a value
                        && buttonState // the value we got
                        || tempState; // cumulative result from other controllers

            if (!device.isValid)
                invalidDeviceFound = true;
        }
        if (tempState != lastButtonState) // Button state changed since last frame
        {
            menuButtonPress.Invoke(tempState);
            lastButtonState = tempState;
        }

        if (invalidDeviceFound || devicesWithMenuButton.Count == 0) // refresh device lists
            updateInputDevices();
    }

    // find any devices supporting the desired feature usage
    void updateInputDevices()
    {
        Debug.Log("Update input devices");
        devicesWithMenuButton.Clear();
        UnityEngine.XR.InputDevices.GetDevices(allDevices);
        bool discardedValue;

        foreach (var device in allDevices)
        {
            if (device.TryGetFeatureValue(CommonUsages.menuButton, out discardedValue))
            {
                devicesWithMenuButton.Add(device); // Add any devices that have a primary button.
            }
        }
    }
}
