using UnityEngine;
using System.Collections;

public class DemoMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // TODO：在项目中的合适位置启动xlua
        XLuaManager.instance.Startup();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
