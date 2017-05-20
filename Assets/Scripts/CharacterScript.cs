using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {

    public bool isPlayers;
    private int turns;
    public int hits = 3;
    //granny = transform.Find("Sporty_Granny " + (numGrannies + 1)).gameObject;
    //        granny.SetActive(true);

	// Use this for initialization
	void Start () {
		
	}
    
    public void Attack(GameObject op)
    {
        GameObject pickup = Resources.Load("Sphere", typeof(GameObject)) as GameObject;
        GameObject pickupClone = Instantiate(pickup, transform.position+Vector3.up, Quaternion.Euler(45, 45, 45));
        Rigidbody rb = pickupClone.GetComponent<Rigidbody>();
        Vector3 velocity = Vector3.zero;
        Vector3 v = op.transform.position - transform.position;
        v.y = 0;
        rb.MoveRotation(Quaternion.LookRotation(v));

        velocity += v;


        rb.AddForce(v * Time.deltaTime * 50000);

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
