using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComponentVerification : MonoBehaviour
{
    
    private Dictionary<string, string> solutionSet;
    private int numberOfConnections;
    List<List<string>> allConnectedObjects;

    public bool verify(){

        createSolutionSet();
        return getAllConnections();

    }

    void createSolutionSet(){
        solutionSet = new Dictionary<string, string>();
        solutionSet.Add("N_north", "L1");
        solutionSet.Add("L1", "N_north");
        solutionSet.Add("L_north", "C");
        solutionSet.Add("C", "L_north");
        solutionSet.Add("PE_north", "PE");
        solutionSet.Add("PE", "PE_north");

        numberOfConnections = 3;
    }

    public bool getAllConnections(){

        ComponentWireTrigger[] componentArray = GameObject.FindObjectsOfType<ComponentWireTrigger>();
        List<ComponentWireTrigger> componentList = new List<ComponentWireTrigger>(componentArray);
        List<GameObject> connectionList = new List<GameObject>();
        foreach(ComponentWireTrigger cWT in componentList){
            connectionList.Add(cWT.gameObject);
        }
        Debug.Log("BA21 - Found " + connectionList.Count + "Components");
        int i = 1;
        foreach(GameObject connector in connectionList){
            Debug.Log("Name Conector" + i + ": " + connector.name);
            i++;
        }
        return checkNumberOfOverallConnections(connectionList);

    }

    public bool checkNumberOfOverallConnections(List<GameObject> list){
        if(list.Count == numberOfConnections){
            getListOfConnectedBodies(list);
            return checkNumberOfConnectionsPerComponent();

        } else{
            Debug.Log("False Number of Overall Connections");
           return false;
        }

    }

    public void getListOfConnectedBodies(List<GameObject> list){

        Debug.Log("entered ListCB");

        allConnectedObjects = new List<List<string>>();

        foreach(GameObject component in list){
             Debug.Log("entered FE1");
             FixedJoint[] fixedJoints = component.GetComponents<FixedJoint>();
             List<FixedJoint> fixedJointsList = new List<FixedJoint>(fixedJoints);
              int i = 1;
             foreach(FixedJoint fJList in fixedJointsList){
                
                    Debug.Log("Name CB" + i + ": " + fJList.gameObject.name);
                    i++;
                
             }
             allConnectedObjects.Add(extractConnectedBodiesFromFixedJoint(fixedJointsList));
             
             /*int i = 1;
             foreach(FixedJoint fJ in fixedJoints){
                 Debug.Log("entered FE2");
                    Debug.Log("Name FixedJoint" + i + ": " + fJ.connectedBody.gameObject.name);
                    i++;
             }*/
            
        }
       
             foreach(List<string> tagList in allConnectedObjects){
                 int i = 1;
                 foreach(string tag in tagList){
                    Debug.Log("Name CB" + i + ": " + tag);
                    i++;
                 }
                
             }

    }

     public List<string> extractConnectedBodiesFromFixedJoint(List<FixedJoint> fixedJointList){
        Debug.Log("reached extract-method");
        List<string> connectedBodies = new List<string>();
        foreach(FixedJoint fJ in fixedJointList){ 
                 connectedBodies.Add(fJ.connectedBody.gameObject.tag);
                 connectedBodies.Add(fJ.gameObject.tag);
                 Debug.Log("added cB " + fJ.connectedBody.gameObject.tag);
             }
        return connectedBodies;
    }

    public bool checkNumberOfConnectionsPerComponent(){
        bool passed = true;
        foreach(List<string> tagList in allConnectedObjects){
                Debug.Log("Anzahl Tags : " + tagList.Count);
                if(tagList.Count != 2){
                    passed = false; 
                }
             }

        if (passed){
            return checkConnections();
        } else{
             Debug.Log("False Number of Connections per Component");
            return false;
        }
    }

    public bool checkConnections(){

        bool passed = true;

            foreach(List<string> tagList in allConnectedObjects){
                Debug.Log("Anzahl Listenelemente: " + tagList.Count);
                    bool connectionVerification = checkConductorPair(tagList);
                    if(connectionVerification == false){
                        passed = false;
                    }
            }

        if (passed){
            return true;
        } else{
             Debug.Log("False Connections");
            return false;
        }
        
    }

    public bool checkConductorPair(List<string> tagList){

        string tag1 = tagList[0];
        string tag2 = tagList[1];

        Debug.Log("tag 1/2: " + tag1 + ", " + tag2);

        if((solutionSet.ContainsKey(tag1) && solutionSet[tag1].Equals(tag2))){
            Debug.Log("True Connection found: " + tag1 + " " + tag2);
            return true;
        } else {
            Debug.Log("PAIR FAILURE DETECTED");
            return false;
        }

    }
    
}

