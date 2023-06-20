using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roamer : MonoBehaviour
{
    private float roamSpeed;
    private Vector3 roamDirection;
    private RaycastHit rayGlob;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        //Once the NPC sees a wall with the raycheck, the NPC changes directions 180 degrees.
        if(SeesWall()){
            // /Vector3 oppositeDirection = this.transform.position - wallHit.position;
        }
    }
    bool SeesWall(){
        //Raycheck that outputs its raycast hit to rayGlob;
        //Then returns the result so a boolean.
        return false; // Change this value after raycast.
    }
}
