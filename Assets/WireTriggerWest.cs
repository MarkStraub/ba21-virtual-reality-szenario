using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class WireTriggerWest : MonoBehaviour

{

    private GameObject connector;
    public GameObject connectorPrefab;
    public GameObject triggerIsolator;
    private bool collisonOccured = false;
    private Vector3 boundsConnectorMin;
    private Vector3 boundsConnectorMax;

    void OnCollisionEnter(Collision collision)
    {
        WireUtility utility = FindObjectOfType<WireUtility>(); 

        Debug.Log("entered East Collision");
        Debug.Log( "colliding wire (name) : " + collision.collider.gameObject.name );

        //makes sure, collision is only fired once
        if (collisonOccured){
            return;
        }

        // get colliding object
        GameObject collidingConductor = collision.gameObject;

        
        if(collidingConductor.name.Contains("east")){

            collisonOccured = true;
            
            // add connector to scene
            connector = utility.AddConnector(collision, triggerIsolator, this.gameObject, connectorPrefab);
            boundsConnectorMin = utility.getConnectorBoundsMin(connector);
            boundsConnectorMax = utility.getConnectorBoundsMax(connector);
            // add new collider conductor to scene
            GameObject newCollidingConductor = utility.AddNewConductor(new Vector3(boundsConnectorMax.x, connector.transform.position.y, connector.transform.position.z), collidingConductor);
            // add new trigger conductor to scene
            GameObject newTriggerConductor = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, this.transform.position.y, this.transform.position.z), this.gameObject);
            // join Objects
            utility.joinThreeObjects(newCollidingConductor, newTriggerConductor, connector);
            
        }  

        if(collidingConductor.name.Contains("south")){

            collisonOccured = true;

            // add connector to scene
            connector = utility.AddConnector(collision, triggerIsolator, this.gameObject, connectorPrefab);
            boundsConnectorMin = utility.getConnectorBoundsMin(connector);
            boundsConnectorMax = utility.getConnectorBoundsMax(connector);
            // add new collider conductor to scene
            GameObject newCollidingConductor = utility.AddNewConductor(new Vector3(connector.transform.position.x, connector.transform.position.y, boundsConnectorMin.z), collidingConductor);
            // add new trigger conductor to scene
            GameObject newTriggerConductor = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, this.transform.position.y, this.transform.position.z), this.gameObject);
            utility.joinThreeObjects(newCollidingConductor, newTriggerConductor, connector);
            
        }  

         /*if(collidingConductor.name.Contains("west")){

            collisonOccured = true;
            
            // add connector to scene
            connector = utility.AddConnector(collision, triggerIsolator, this.gameObject, connectorPrefab);
            boundsConnectorMin = utility.getConnectorBoundsMin(connector);
            boundsConnectorMax = utility.getConnectorBoundsMax(connector);
            // add new collider conductor to scene
            GameObject newCollidingConductor = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, connector.transform.position.y, connector.transform.position.z), collidingConductor);
            // add new trigger conductor to scene
            GameObject newTriggerConductor = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, this.transform.position.y, this.transform.position.z), this.gameObject);
            utility.joinThreeObjects(newCollidingConductor, newTriggerConductor, connector);
            
        } */
        
         if(collidingConductor.name.Contains("north")){

            collisonOccured = true;

            // add connector to scene
            connector = utility.AddConnector(collision, triggerIsolator, this.gameObject, connectorPrefab);
            boundsConnectorMin = utility.getConnectorBoundsMin(connector);
            boundsConnectorMax = utility.getConnectorBoundsMax(connector);
            // add new collider conductor to scene
            GameObject newCollidingConductor = utility.AddNewConductor(new Vector3(connector.transform.position.x, connector.transform.position.y, boundsConnectorMax.z), collidingConductor);
            // add new trigger conductor to scene
            GameObject newTriggerConductor = utility.AddNewConductor(new Vector3(boundsConnectorMin.x, this.transform.position.y, this.transform.position.z), this.gameObject);
            utility.joinThreeObjects(newCollidingConductor, newTriggerConductor, connector);
            
        } 

    }

}
    





