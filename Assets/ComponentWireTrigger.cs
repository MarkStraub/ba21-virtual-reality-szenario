using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ComponentWireTrigger : MonoBehaviour
{

     private bool collisonOccured = false;
        private Vector3 boundsConnectorMin;
        private Vector3 boundsConnectorMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 void OnCollisionEnter(Collision collision)
    { 

        Debug.Log("entered Wire Component Collision");
        Debug.Log( "colliding object (name) : " + collision.collider.gameObject.name );

        //makes sure, collision is only fired once
        if (collisonOccured){
            return;
        }

        // get colliding object
        GameObject collidingObject = collision.collider.gameObject;

        Debug.Log("registered: " + collidingObject.name + " as Collider");

        
        if(collidingObject.name.Contains("component")){

            Debug.Log(collidingObject.name + " contains _COMPONENT_ in name");
            Debug.Log("Position of " + collidingObject.name + " is: " + collidingObject.transform.position);

            collisonOccured = true;

            // get center of colliding objects collider
            Collider c = collidingObject.GetComponent<Collider>();
            boundsConnectorMax = c.bounds.max;
            Vector3 center = c.bounds.center;

            Debug.Log("calculated Min/Max: " + boundsConnectorMax);
            Debug.Log("calculated center " + center);

            //add new conductor to scene
            GameObject newObject = Instantiate(this.gameObject, new Vector3(center.x, center.y, boundsConnectorMax.z), this.gameObject.transform.rotation);
            Collider collider = collidingObject.GetComponent<Collider>();	            
            Destroy(collider);	            
            this.gameObject.SetActive(false); 
            Debug.Log("registered: " + newObject.transform.position);            
            
            //add fixed joint to conductor
            newObject.AddComponent<FixedJoint>();
            newObject.GetComponent<FixedJoint>().connectedBody = collidingObject.GetComponent<Rigidbody>();

            //get parent-object of colliding object
            GameObject parentObject = collidingObject.transform.parent.gameObject;

            //add configurable joint
            ConfigurableJoint cJ = parentObject.AddComponent<ConfigurableJoint>();
            cJ.xMotion = ConfigurableJointMotion.Locked;
            cJ.yMotion = ConfigurableJointMotion.Limited;
            cJ.zMotion = ConfigurableJointMotion.Locked;
            cJ.angularXMotion = ConfigurableJointMotion.Locked;
            cJ.angularYMotion = ConfigurableJointMotion.Locked;
            cJ.angularZMotion = ConfigurableJointMotion.Locked;

            Debug.Log("added Configurable Joint");
            
            // set linear limit of configurable joint
            SoftJointLimit limit = cJ.linearLimit; 
            limit.limit = 0.5f; 
            cJ.linearLimit = limit; 
        
        }  

        if(collidingObject.name.Contains("screwdriver")){

            Debug.Log(collidingObject.name + " contains _SCREWDRIVER_ in name");

            // gett all fixed joints from object
            FixedJoint[] connectorJoints = this.gameObject.GetComponents<FixedJoint>();
            
            if(connectorJoints.Length == 0){

                Debug.Log("no Fixed Joint found");

            } else{
                
                foreach (FixedJoint fJ in connectorJoints){

                // get component & conductor
                GameObject conductor = fJ.gameObject;
                GameObject component = fJ.connectedBody.gameObject;

                //destroy fixed joint
                Destroy(fJ);

                //change position of conductor
                conductor.transform.position = new Vector3(conductor.transform.position.x, conductor.transform.position.y, (conductor.transform.position.z + 0.3f));
                component.AddComponent<BoxCollider>();

                Debug.Log("enterd Remove CJ");

                // get and remove configurable joint from parent object
                GameObject parentObject = component.transform.parent.gameObject;
                ConfigurableJoint cJ = parentObject.GetComponent<ConfigurableJoint>();
                Destroy(cJ);

                Debug.Log("Destroyed " + fJ.connectedBody.gameObject.name);

                }

            }
        }

        
 

            
            
        } 

    }


