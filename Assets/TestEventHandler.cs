using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPEUI;
public class TestEventHandler : SPEUIEventHandler {


	
	// Update is called once per frame
	void Update () {
		
	}
    [UIEventCall]
    public void Test(SPEUIBase sender)
    {
        Debug.Log("fsaefse");
    }
}
