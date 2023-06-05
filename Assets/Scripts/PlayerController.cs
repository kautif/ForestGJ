using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public Transform[] MapNodes;
    private bool isTouching = false;
    // Start is called before the first frame update
    float mapNodesPlayerDist;
    float step;
    private Vector3 mapTarget;
    void Awake()
    {
        mapNodesPlayerDist = Vector3.Distance(MapNodes[0].transform.position, player.transform.position);
        /*
        Debug.Log($"{MapNodes[0]}: {MapNodes[0].transform.position}");
        Debug.Log($"{MapNodes[1]}: {MapNodes[1].transform.position}");
        Debug.Log($"{MapNodes[2]}: {MapNodes[2].transform.position}");
        Debug.Log($"{MapNodes[3]}: {MapNodes[3].transform.position}");*/
        step = 1 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100)) {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.collider.CompareTag("Map Node")) {
                    mapTarget = new Vector3(hit.transform.gameObject.transform.position.x - 1,
                        hit.transform.gameObject.transform.position.y,
                        hit.transform.gameObject.transform.position.z);
                }
            }
        }
        /*
        if (isTouching == true)
        {
            step = 0;
        }
        else 
        {
            step = 1 * Time.deltaTime;
        }

        transform.position = Vector3.MoveTowards(player.transform.position, MapNodes[0].transform.position, step); */

        transform.position = Vector3.MoveTowards(player.transform.position, mapTarget, step);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
