using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LightBulbControll : MonoBehaviour
{

    WireVerification verification;
    GameObject bulb;
    public GameObject lightBulbLight;
    private bool lightState = false;
    private bool lightStateFirstEnter = true;

    // Start is called before the first frame update
    void Start()
    {
       verification = FindObjectOfType<WireVerification>(); 
       bulb = GameObject.FindGameObjectWithTag("light");

    }

    // Update is called once per frame
    void Update()
    {
        if(lightState == true){
            if(verification.verify() == false){
                turnOffLight();
                lightState = false;
            }
        }
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (lightStateFirstEnter)
        {
            lightState = !lightState;
            switchLight(lightState);
            lightStateFirstEnter = false;
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
        foreach( Material mat in bulb.GetComponent<MeshRenderer>().materials){
               mat.EnableKeyword("_EMISSION");
        }
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

