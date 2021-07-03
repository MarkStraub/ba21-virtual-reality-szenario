using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTrigger : MonoBehaviour
{

    private Vector3 boundsConnectorMin;
    private Vector3 boundsConnectorMax;
    WireUtility utility;

    void OnCollisionEnter(Collision collision)
    {
       utility = FindObjectOfType<WireUtility>(); 

        // get colliding object
        GameObject collidingObject = collision.gameObject;

        // get min/max of connector
        boundsConnectorMin = utility.getConnectorBoundsMin(this.gameObject);
        boundsConnectorMax = utility.getConnectorBoundsMax(this.gameObject);


        if(collidingObject.name.Contains("Screwdriver")){

            // get all Fixed Joints of Game Object
            FixedJoint[] connectorJoints = this.gameObject.GetComponents<FixedJoint>();

            // delet all Fixed Joints and add new Collider
            foreach (FixedJoint fJ in connectorJoints){
                GameObject connectedObject = fJ.connectedBody.gameObject;
                Destroy(fJ);
                transformCollider(connectedObject);
                Debug.Log("Destroyed " + fJ.connectedBody.gameObject.name);
            }

            //remove connector from scene
            this.gameObject.SetActive(false);

        } 
 

        if(collidingObject.name.Contains("east")){

            if(checkConnectedObjects("east")){
            // add new collider conductor to scene
            GameObject newcollidingObject = utility.AddNewConductor(new Vector3(boundsConnectorMax.x, this.transform.position.y, this.transform.position.z), collidingObject);
            utility.joinTwoObjects(newcollidingObject, this.gameObject);
            } else {
                Debug.Log("Connector already contains connection with a conductor from east");
            }
            
        }  

        if(collidingObject.name.Contains("south")){

            if(checkConnectedObjects("south")){
            // add new collider conductor to scene
            GameObject newcollidingObject = utility.AddNewConductor(new Vector3(this.transform.position.x, this.transform.position.y, boundsConnectorMin.z), collidingObject);
            utility.joinTwoObjects(newcollidingObject, this.gameObject);
            } else {
                Debug.Log("Connector already contains connection with a conductor from south");
            }
        }  

         if(collidingObject.name.Contains("west")){
        
            if(checkConnectedObjects("west")){
            // add new collider conductor to scene
            GameObject newcollidingObject = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, this.transform.position.y, this.transform.position.z), collidingObject);
            utility.joinTwoObjects(newcollidingObject, this.gameObject);
            } else {
                Debug.Log("Connector already contains connection with a conductor from west");
            }
        } 
        
         if(collidingObject.name.Contains("north")){

            if(checkConnectedObjects("north")){
            // add new collider conductor to scene
            GameObject newcollidingObject = utility.AddNewConductor(new Vector3(this.transform.position.x, this.transform.position.y, boundsConnectorMax.z), collidingObject);
            utility.joinTwoObjects(newcollidingObject, this.gameObject);
            } else {
                Debug.Log("Connector already contains connection with a conductor from north");
            }
        }  

    }

    public void transformCollider(GameObject conductor){


        // change position of colliding conductor

        if(conductor.name.Contains("west")){
            conductor.transform.position = new Vector3((conductor.transform.position.x - 0.07f), conductor.transform.position.y, conductor.transform.position.z);
            utility.addCollider(conductor);
        }

        if(conductor.name.Contains("east")){
            conductor.transform.position = new Vector3((conductor.transform.position.x + 0.07f), conductor.transform.position.y, conductor.transform.position.z);
            utility.addCollider(conductor);
        }

        if(conductor.name.Contains("north")){
            conductor.transform.position = new Vector3(conductor.transform.position.x, conductor.transform.position.y, (conductor.transform.position.z + 0.07f));
            utility.addCollider(conductor);
        }

        if(conductor.name.Contains("south")){
            conductor.transform.position = new Vector3(conductor.transform.position.x, conductor.transform.position.y, (conductor.transform.position.z - 0.07f));
            utility.addCollider(conductor);
        }

        
    }

    public bool checkConnectedObjects(string direction){

        // get all Fixed Joints of Game Object
        FixedJoint[] connectorJoints = this.gameObject.GetComponents<FixedJoint>();

        bool checkDirection = true;

        foreach (FixedJoint fJ in connectorJoints){
            GameObject connectedObject = fJ.connectedBody.gameObject;
            if(connectedObject.tag.Contains(direction)){
                checkDirection = false;
            } 
         }

         return checkDirection;

    }


}
