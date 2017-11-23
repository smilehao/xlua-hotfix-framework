using UnityEngine;
using XLua;
using UnityEngine.UI;

[Hotfix]
[LuaCallCSharp]
public class BaseClass : MonoBehaviour
{
    protected Text text = null;

    public Text TextCmp
    {
        get
        {
            return text;
        }
    }

    protected virtual void Awake()
    {
        text = transform.GetComponent<Text>();
        if (text == null)
        {
            text = gameObject.AddComponent<Text>();
        }
    }

    public virtual void SetText(string textValue)
    {
        if (text != null)
        {
            text.text = textValue;
        }
    }
    
    public virtual void ShowTextValue()
    {
        if (text != null)
        {
            Debug.Log(text.text);
        }
    }

    protected void TestProtectedFun()
    {
        Debug.Log("TestProtectedFun");
    }
}

[Hotfix]
[LuaCallCSharp]
public class DriveClass : BaseClass
{
    public override void SetText(string textValue)
    {
        textValue += "!";
        base.SetText(textValue);
    }

    public void BaseSetText(string ss)
    {
        base.SetText(ss);
    }
}

[Hotfix(HotfixFlag.Stateful)]
[LuaCallCSharp]
public class HotfixTest : MonoBehaviour {
    [HideInInspector]
    public BaseClass testClass = null;

    public int tick = 0; //如果是private的，在lua设置xlua.private_accessible(CS.HotfixTest)后即可访问

    // Use this for initialization
    void Start () {
        testClass = gameObject.AddComponent<DriveClass>();
    }

    // Update is called once per frame
    void Update () {
	    if (++tick % 50 == 0)
        {
            Debug.Log(">>>>>>>>Update in C#, tick = " + tick);
            if (testClass != null)
            {
                testClass.SetText("TTick " + tick.ToString());
                testClass.ShowTextValue();
            }
        }
	}

    public void TestCall()
    {
        Debug.Log("Call in c#");
    }
}
