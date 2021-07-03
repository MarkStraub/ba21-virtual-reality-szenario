using UnityEngine;

public class Settings
{
    /// <summary>
    /// Version of the settings
    /// YYYY.MM.DD.HHMM
    /// </summary>
    public string version = "2021.06.10.2115";

    /// <summary>
    /// General settings
    /// </summary>
    public string language = "en_US";

    /// <summary>
    /// Save the unit and the level 
    /// Level is the sub of the unit
    /// </summary>
    public string scene_name = "theory";
    public string scene_short_name = "th_1";
    public int level = 1;

    /// <summary>
    /// Save the unit and the level 
    /// Level is the sub of the unit
    /// </summary>
    // Theory
    public bool th_1 = true;
    public bool th_2 = false;
    public bool th_3 = false;
    public bool th_4 = false;
    public bool th_5 = false;
    public bool th_6 = false;
    public bool th_7 = false;
    public bool th_8 = false;

    // Workbench
    public bool wo_1 = false;
    public bool wo_2 = false;
    public bool wo_3 = false;
    public bool wo_4 = false;
    public bool wo_5 = false;
    public bool wo_6 = false;

    // House
    public bool ho_1 = false;
    public bool ho_2 = false;
    public bool ho_3 = false;
    public bool ho_4 = false;
}
