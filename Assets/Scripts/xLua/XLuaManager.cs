using UnityEngine;
using XLua;

/// <summary>
/// 说明：xLua管理类
/// 
/// @by wsh 2017-08-30
/// </summary>

public class XLuaManager : MonoSingleton<XLuaManager>
{
    LuaEnv luaEnv = null;

    protected override void Init()
    {
        base.Init();

        luaEnv = new LuaEnv();
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);
            LoadMain();
        }
    }
    
    public void ReloadMain()
    {
        try
        {
            if (luaEnv != null)
            {
                luaEnv.DoString("package.loaded['Main'] = nil");
            }
        }
        catch (System.Exception ex)
        {
            string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
            Debug.LogError(msg, null);
        }
        LoadMain();
    }

    void LoadMain()
    {
        try
        {
            if (luaEnv != null)
            {
                luaEnv.DoString("require('Main')");
            }
        }
        catch (System.Exception ex)
        {
            string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
            Debug.LogError(msg, null);
        }
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        Debug.Log("Load xLua script : " + filepath);
        // TODO：此处从项目资源管理器加载lua脚本
        TextAsset textAsset = (TextAsset)Resources.Load("xlua/" + filepath.Replace(".","/") + ".lua");
        //TextAsset textAsset = (TextAsset)ResourceMgr.instance.SyncLoad(ResourceMgr.RESTYPE.XLUA_SCRIPT, filepath).resObject;
        if (textAsset != null)
        {
            return textAsset.bytes;
        }
        return null;
    }

    private void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();

            if (Time.frameCount % 100 == 0)
            {
                luaEnv.FullGc();
            }
        }
    }

    public override void Dispose()
    {
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
