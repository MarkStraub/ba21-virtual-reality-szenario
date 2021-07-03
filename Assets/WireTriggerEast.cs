using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireTriggerEast : MonoBehaviour
{

    private GameObject connector;
    public GameObject connectorPrefab;
    public GameObject triggerIsolator;
    private bool collisonOccured = false;
    private Vector3 boundsConnectorMin;
    private Vector3 boundsConnectorMax;
 

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("entered East Collision");
        Debug.Log( "colliding wire (name) : " + collision.collider.gameObject.name );

       WireUtility utility = FindObjectOfType<WireUtility>();

        //makes sure, collision is only fired once
        if (collisonOccured){
            return;
        }

        // get colliding object
        GameObject collidingConductor = collision.gameObject;

         if(collidingConductor.name.Contains("north")){

            collisonOccured = true;

            // add connector to scene
            connector = utility.AddConnector(collision, triggerIsolator, this.gameObject, connectorPrefab);
            boundsConnectorMin = utility.getConnectorBoundsMin(connector);
            boundsConnectorMax = utility.getConnectorBoundsMax(connector);

            // add new collider conductor to scene
            GameObject newCollidingConductor = utility.AddNewConductor(new Vector3(connector.transform.position.x, connector.transform.position.y, boundsConnectorMax.z), collidingConductor);
            
            // add new trigger conductor to scene
            GameObject newTriggerConductor = utility.AddNewConductor(new Vector3(boundsConnectorMax.x, this.transform.position.y, this.transform.position.z), this.gameObject);
            utility.joinThreeObjects(newCollidingConductor, newTriggerConductor, connector);
            
        }  


    }

}



