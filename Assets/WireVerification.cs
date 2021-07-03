using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireVerification : MonoBehaviour
{   
    private Dictionary<string, string> pairSolutionSet;
    private Dictionary<string, (string,string)> trippleSolutionSet;
    List<List<string>> allConnectedBodies;
    private int numberOfConnectors;
    private int numberOfConductors;


    public bool verify(){

        createSolutionSets();
        return getAllConnectors();

    }

    void createSolutionSets(){
        pairSolutionSet = new Dictionary<string, string>();
        pairSolutionSet.Add("N_west", "N_east");
        pairSolutionSet.Add("N_east", "N_west");
        pairSolutionSet.Add("L_west", "L_north");
        pairSolutionSet.Add("L_north", "L_west");
        pairSolutionSet.Add("N_north", "L_east");
        pairSolutionSet.Add("L_east", "N_north");
        

        trippleSolutionSet = new Dictionary<string, (string, string)>();
        trippleSolutionSet.Add("PE_west", ("PE_north", "PE_east"));
        trippleSolutionSet.Add("PE_east", ("PE_west", "PE_north"));
        trippleSolutionSet.Add("PE_north", ("PE_west", "PE_east"));

        numberOfConnectors = 4;
        numberOfConductors = 9;
    }

    public bool getAllConnectors(){

        GameObject [] connectorList = GameObject.FindGameObjectsWithTag("Connector");
        Debug.Log("BA21 - Found " + connectorList.Length + "Connectors");
        /*int i = 1;
        foreach(GameObject connector in connectorList){
            Debug.Log("Name Conector" + i + ": " + connector.name);
            i++;
        }*/
        return checkNumberOfConnectors(connectorList);

    }

    public bool checkNumberOfConnectors(GameObject[] list){
        if(list.Length == numberOfConnectors){
            getListOfConnectedBodies(list);
             return checkNumberOfConductors();

        } else{
           return false;
        }
    }

    public void getListOfConnectedBodies(GameObject[] list){

        Debug.Log("entered ListCB");

        allConnectedBodies = new List<List<string>>();

        foreach(GameObject connector in list){
            Debug.Log("entered FE1");
             FixedJoint[] fixedJoints = connector.GetComponents<FixedJoint>();
             List<FixedJoint> fixedJointsList = new List<FixedJoint>(fixedJoints);
             allConnectedBodies.Add(extractConnectedBodiesFromFixedJoint(fixedJointsList));
             /*int i = 1;
             foreach(FixedJoint fJ in fixedJoints){
                 Debug.Log("entered FE2");
                    Debug.Log("Name FixedJoint" + i + ": " + fJ.connectedBody.gameObject.name);
                    i++;
             }*/
            
        }
       
             foreach(List<string> tagList in allConnectedBodies){
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
                 Debug.Log("added cB " + fJ.connectedBody.gameObject.tag);
             }
        return connectedBodies;
    }

    public bool checkNumberOfConductors(){
        bool passed = true;
        int attachedConnectors = 0;
        foreach(List<string> tagList in allConnectedBodies){
                Debug.Log("Anzahl Tags : " + tagList.Count);
                if(tagList.Count < 2 | tagList.Count > 4){
                    passed = false; 
                }
                foreach(string tag in tagList){
                        attachedConnectors++;
                    }    
             }

        if (passed && (attachedConnectors == numberOfConductors)){
            return checkConductorConnections();
        } else{
            return false;
        }
    }

    public bool checkConductorConnections(){

        bool passed = true;

            foreach(List<string> tagList in allConnectedBodies){
                Debug.Log("Anzahl Listenelemente: " + tagList.Count);
                if(tagList.Count == 2){
                    bool connectionVerification = checkConductorPair(tagList);
                    if(connectionVerification == false){
                        passed = false;
                    }
                }
                if(tagList.Count == 3){
                    bool connectionVerification = checkConductorTripple(tagList);
                    if(connectionVerification == false){
                        passed = false;
                    }
                }
            }

        if (passed){
            return true;
        } else{
            return false;
        }
        
    }

    public bool checkConductorPair(List<string> tagList){

        string tag1 = tagList[0];
        string tag2 = tagList[1];

        Debug.Log("tag 1/2: " + tag1 + ", " + tag2);

        if((pairSolutionSet.ContainsKey(tag1) && pairSolutionSet[tag1].Equals(tag2))){
            return true;
        } else {
            Debug.Log("PAIR FAILURE DETECTED");
            return false;
        }

    }

    public bool checkConductorTripple(List<string> tagList){

        string tag1 = tagList[0];
        string tag2 = tagList[1];
        string tag3 = tagList[2];

        Debug.Log("tag 1/2/3: " + tag1 + ", " + tag2 + ", " + tag3);

        if(trippleSolutionSet.ContainsKey(tag1) && (trippleSolutionSet[tag1].Equals((tag2,tag3)) | (trippleSolutionSet[tag1].Equals((tag3,tag2))))){
            return true;
        } else {
            Debug.Log("FAILURE DETECTED");
            return false;
        }

    }
}

