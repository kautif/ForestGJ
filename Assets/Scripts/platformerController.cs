using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformerController : MonoBehaviour
{
    private Rigidbody rigidPlayer;
    private CapsuleCollider playerCollider;
    private Transform spawnPoint = null;
    private GameObject[] spawns = null;
    [SerializeField] private float gravityRate = -9.81f;
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float JumpForce = 450.0f;
    [SerializeField] private float FlyForce = 350.0f;
    [SerializeField] private float bumpForce = 40.0f;
    private int turnDirection = 0;
    [SerializeField] public int featherCount = 0;
    private int maxFeathers = 5;
    [SerializeField] private int score = 0;

    private Vector3 rayHitPoint = Vector3.zero;


    
    [SerializeField] bool grounded = false;
    // Start is called before the first frame update
    Dictionary<string, int> layers = new Dictionary<string, int>();

    void Start()
    {
        layers.Add("Terrain", 6);
        Physics.gravity = new Vector3(0,gravityRate,0);
        rigidPlayer = this.GetComponent<Rigidbody>();
        playerCollider = this.GetComponent<CapsuleCollider>();
        //Always check last spawn point so that we can add checkpoints.
        if(spawnPoint == null && spawns == null){
            spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            foreach (GameObject spawn in spawns)
            {
                
                spawnPoint = spawn.transform;
                
            }
            Debug.Log(spawns);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        
        //if(!grounded)
            //this.transform.Translate(Vector3.down * (gravityRate * Time.deltaTime));
        Physics.gravity = new Vector3(0,gravityRate,0);
        //rigidPlayer.velocity = Vector3.down * (gravityRate * Time.deltaTime);
        
        //Move
        this.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed,0,0));

        Turn();

        //Jump and glide
        if(IsGroundedRay()){
            if(Input.GetAxis("Vertical") > 0.00f){
                rigidPlayer.AddRelativeForce(Vector3.up * Time.deltaTime * JumpForce, ForceMode.Impulse);
            }
        } else {
            if(Input.GetAxis("Vertical") > 0.00f){
                rigidPlayer.AddRelativeForce(Vector3.up * Time.deltaTime * FlyForce, ForceMode.Acceleration);
                //rigidPlayer.AddRelativeForce(Vector3.up * Time.deltaTime * FlyForce, ForceMode.Acceleration);
            }
        }
        //Debug.Log(Vector3.down * playerCollider.bounds.extents.y);
        

    }

    void Turn() {
        if(Input.GetAxis("Horizontal") > 0){
            if(turnDirection == 0){
                this.transform.GetChild(1).Rotate(0,90,0);
            } else if (turnDirection == -1){
                this.transform.GetChild(1).Rotate(0,180,0);
            }
            turnDirection = 1; // right
        } else if (Input.GetAxis("Horizontal") < 0){
            if(turnDirection == 0){
                this.transform.GetChild(1).Rotate(0,-90,0);
            }
            else if(turnDirection == 1) {
                this.transform.GetChild(1).Rotate(0,-180,0);
            }
            turnDirection = -1; // left
        }
    }
    void Flight(){
        if(featherCount >= 2){
            //glide code?
        } if (featherCount >=4){
            //Fly Code.
        }
    }

    private bool IsGroundedRay() {
        float extraHeightTest = 0.01f;
        RaycastHit rayHit;
        Color rayColor = Color.white;

        if(Physics.Raycast(playerCollider.bounds.center, Vector3.down, out rayHit,playerCollider.bounds.extents.y + extraHeightTest)){
            //Debug.Log("HIT" + rayHit.collider.transform.position);
        }
        if(rayHit.collider != null){
            rayColor = Color.green;
            grounded = true;
        } else {
            rayColor = Color.red;
            grounded = false;
        }

        Debug.DrawRay(playerCollider.bounds.center, Vector3.down * (playerCollider.bounds.extents.y + extraHeightTest), rayColor);
        
        return rayHit.collider != null;
        
    }
    /*
    void OnDrawGizmosSelected() {
        if(this != null){
            if(grounded){
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.red;
            }
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z -10);
            Gizmos.DrawLine(pos, new Vector3(this.transform.position.x, (playerCollider.bounds.extents.y + 0.01f), this.transform.position.z - 10));
        }
    }
    */

    void DeathCheck(){
        if(featherCount <= 0){
            score-=50;
            this.transform.GetChild(1).gameObject.SetActive(false);
            if(spawnPoint != null){
                rigidPlayer.velocity = Vector3.zero;
                this.transform.position = spawnPoint.position;
            }
            featherCount = 0;
            this.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    void OnCollisionEnter(Collision collision){
        /*
        if(collision.collider.gameObject.layer == layers["Collectable"]){
            grounded = true;
        } else {
            grounded = false;
        }
        */
        if(collision.collider.gameObject.tag == "Hazard"){
            score -= 10;
            featherCount--;
            
            //Cause feathers or feather to drop.
            //Screen animation for hit? Blood particle animation?
            Vector3 bumpDistance = Vector3.zero;
            //The opposite of this is a cool effect that pulls the player in.
            bumpDistance.x = this.transform.position.x - collision.collider.transform.position.x;
            bumpDistance.y = (this.transform.position.y - collision.collider.transform.position.y) * 0.5f;
            
            rigidPlayer.AddForce(bumpDistance * Time.deltaTime * bumpForce, ForceMode.Impulse);
            DeathCheck();
            
        }
        if(collision.collider.gameObject.tag == "Feather"){
            Destroy(collision.collider.gameObject,0);
            //Add to feathers logic.
            if(featherCount >= maxFeathers){
                score += 100;
            } else {
                score += 100;
                featherCount++;
            }
        }
    }
    

}
