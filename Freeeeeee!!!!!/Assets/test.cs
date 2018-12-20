using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    
	public GameObject gameObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.position = gameObject.transform.position;
		this.transform.position += new Vector3(-3.5f, 0, 3.0f);
	}
}
