using UnityEngine;
using System.Collections;

public class MetakBehavior : MonoBehaviour {
	public int boja;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y<-2){		
			Destroy(gameObject);
		}
	}
}
