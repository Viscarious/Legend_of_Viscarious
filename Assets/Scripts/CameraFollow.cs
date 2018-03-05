using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraFollow : MonoBehaviour {

    private const float SMOOTHING = 5.0f;

    [SerializeField] Transform target;
    [SerializeField] float smoothing = SMOOTHING;

    Vector3 cameraOffset;

    private void Awake()
    {
        Assert.IsNotNull(target);
    }

    // Use this for initialization
    void Start ()
    {
        cameraOffset = this.transform.position - target.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 targetCamPos = target.position + cameraOffset;
        transform.position = Vector3.Lerp(this.transform.position, targetCamPos, smoothing * Time.deltaTime);
	}


}
