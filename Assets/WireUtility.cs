using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class WireUtility : MonoBehaviour
{

    public GameObject AddNewConductor(Vector3 positionConductor, GameObject baseObject){
            GameObject newConductor = Instantiate(baseObject, new Vector3(positionConductor.x, positionConductor.y, positionConductor.z), baseObject.transform.rotation);
            Collider collider = newConductor.GetComponent<Collider>();
            collider.enabled = false;
            baseObject.gameObject.SetActive(false);

            return newConductor;
        }
        
   public GameObject AddConnector(Collision collision, GameObject isolator, GameObject baseObject, GameObject connectorPrefab){
            Debug.Log(collision.collider.gameObject.name + isolator.name + baseObject.name);
            
            Vector3 collisionPosition = collision.collider.ClosestPointOnBounds(baseObject.transform.position);
            Debug.Log(collisionPosition);
            Debug.Log(connectorPrefab.name);

            GameObject kabelHalter = GameObject.Find("wireHolder");

            // get center of colliding objects collider
            Collider c = kabelHalter.GetComponent<Collider>();
            Vector3 max = c.bounds.max;
            Vector3 min = c.bounds.min;

            if(collisionPosition.x < min.x){
                Debug.Log("collision is too far on the left");
                Debug.Log("collision position: " + collisionPosition.x);
                collisionPosition.x += (min.x - collisionPosition.x);
                Debug.Log("collision position adjusted: " + collisionPosition.x);
            }

            if(collisionPosition.x > max.x){
                Debug.Log("collision is too far on the right");
                Debug.Log("collision position: " + collisionPosition.x);
                collisionPosition.x += (max.x - collisionPosition.x);
                Debug.Log("collision position adjusted: " + collisionPosition.x);
            }

            Debug.Log(min.x + ", " + max.x);
            GameObject connector = Instantiate(connectorPrefab, new Vector3(collisionPosition.x, isolator.transform.position.y, isolator.transform.position.z), connectorPrefab.transform.rotation);

            
            return connector;
    }

    public Vector3 getConnectorBoundsMin(GameObject connector){
        Renderer rend = connector.GetComponent<Renderer>();
        Vector3 boundsConnectorMin = rend.bounds.min;

        return (boundsConnectorMin);
    }

    public Vector3 getConnectorBoundsMax(GameObject connector){
        Renderer rend = connector.GetComponent<Renderer>();
        Vector3 boundsConnectorMax = rend.bounds.max;

        return (boundsConnectorMax);
    }

    public void joinTwoObjects(GameObject conductor, GameObject connector){
        FixedJoint fJCollider = connector.gameObject.AddComponent<FixedJoint>();
        fJCollider.connectedBody = conductor.GetComponent<Rigidbody>();

    }

    public void joinThreeObjects(GameObject conductor1, GameObject conductor2, GameObject connector){
        FixedJoint fJTrigger = connector.gameObject.AddComponent<FixedJoint>();
        fJTrigger.connectedBody = conductor1.GetComponent<Rigidbody>();
        FixedJoint fJCollider = connector.gameObject.AddComponent<FixedJoint>();
        fJCollider.connectedBody = conductor2.GetComponent<Rigidbody>();

    }

    public void addCollider(GameObject conductor){
        BoxCollider collider = conductor.GetComponent<BoxCollider>();
        collider.enabled = true;

    }


}

