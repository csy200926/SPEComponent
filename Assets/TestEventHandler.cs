using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPEUI;
public class TestEventHandler : SPEUIEventHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [UIEventCall]
    public void Test(SPEUIBase sender)
    {
        Debug.Log("fsaefse");
    }
}
