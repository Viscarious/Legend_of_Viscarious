using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRangedAttack : MonoBehaviour {

    //TODO: Make into serializable fields?
    [SerializeField] private Transform arrowSpawnLocation;
    [SerializeField] private GameObject arrow;

    // Use this for initialization
    void Start()
    {
        
    }

    /// <summary>
    /// Fire an arrow at the player
    /// </summary>
    public void FireArrow()
    {
        //1. create an arrow
        GameObject newArrow = Instantiate(arrow) as GameObject;
        //2. set the position to a specific location
        newArrow.transform.position = arrowSpawnLocation.position;
        //3. set the orientation
        newArrow.transform.rotation = transform.rotation;
        //4. apply a velocity to it.
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25.0f;
    }
}
