using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightBulbControllComponent : MonoBehaviour
{

    ComponentVerification verification;
    GameObject bulb;
    public GameObject lightBulbLight;
    private bool lightState = false;
    private bool lightStateFirstEnter = true;

    // Start is called before the first frame update
    void Start()
    {
       verification = FindObjectOfType<ComponentVerification>(); 
       bulb = GameObject.FindGameObjectWithTag("light");
       Debug.Log("bulb name: " + bulb.name);

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered trigger");

        if(other.gameObject.tag == "hand"){

            Debug.Log("registered hand collision");

        if (lightStateFirstEnter)
        {
            lightState = !lightState;
            switchLight(lightState);
            lightStateFirstEnter = false;
        }
        }
    }

       private void OnTriggerExit(Collider other)
    {
        lightStateFirstEnter = true;
    }

       private void switchLight(bool lightState)
    {
        if (lightState) turnOnLight();
        else turnOffLight();
    }

    public void turnOnLight(){
        Debug.Log("Turn on Light Entered");

        if(verification.verify()){
        Debug.Log("Conductor RIGHT connected");
        int i = 1;
        foreach( Material mat in bulb.GetComponent<MeshRenderer>().materials){
               mat.EnableKeyword("_EMISSION");
               Debug.Log("material changed: " + i);
               i++;
        }
        Debug.Log("light: " + lightBulbLight.name);
        lightBulbLight.SetActive(true);
        } else{
            Debug.Log("Connductors are not connected the right way");
        }
    }


    public void turnOffLight(){

        if(verification.verify()){
        foreach( Material mat in bulb.GetComponent<MeshRenderer>().materials){
               mat.DisableKeyword("_EMISSION");
        }
        lightBulbLight.SetActive(false);
        } else{
            Debug.Log("Connductors are not connected the right way");
        }
        
    }

}

