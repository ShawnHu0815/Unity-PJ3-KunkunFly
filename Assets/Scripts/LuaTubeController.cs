using UnityEngine;
using XLua;
using System.IO;
using System.Text;

public class LuaTubeController : MonoBehaviour
{
    public TextAsset luaScript;  // 可以在Inspector中指定，或者通过代码加载
    [Tooltip("如果不指定luaScript，将使用此路径从Lua文件夹加载")]
    public string luaScriptPath = "Game/Tube/TubeController.lua";
    
    private static LuaEnv sharedLuaEnv; // 共享的LuaEnv实例
    private LuaTable scriptEnv;
    private LuaFunction luaStart;
    private LuaFunction luaFixedUpdate;
    private LuaFunction luaOnTriggerEnter2D;
    private LuaFunction luaRandomHeight;
    private bool initialized = false;
    
    // 添加自定义Lua文件加载器，从指定目录加载
    private static byte[] CustomLuaLoader(ref string filepath)
    {
        // 首先尝试从开发目录加载
        string luaFilePath = Path.Combine(Application.dataPath, "Lua", filepath);
        
        if (File.Exists(luaFilePath))
        {
            return File.ReadAllBytes(luaFilePath);
        }
        
        // 在编辑器中可以从LuaScripts目录加载（用于开发和向后兼容）
        #if UNITY_EDITOR
        luaFilePath = Path.Combine(Application.dataPath, "LuaScripts", Path.GetFileName(filepath));
        if (File.Exists(luaFilePath))
        {
            return File.ReadAllBytes(luaFilePath);
        }
        #endif
        
        // 在发布版本中可以从StreamingAssets加载（用于发布后）
        luaFilePath = Path.Combine(Application.streamingAssetsPath, "Lua", filepath);
        if (File.Exists(luaFilePath))
        {
            return File.ReadAllBytes(luaFilePath);
        }
        
        Debug.LogWarning($"无法找到Lua文件: {filepath}");
        return null;
    }
    
    void Awake()
    {
        // 创建或获取 LuaEnv 实例
        if (sharedLuaEnv == null)
        {
            sharedLuaEnv = new LuaEnv();
            
            // 添加自定义加载器，优先级高于内置加载器
            sharedLuaEnv.AddLoader(CustomLuaLoader);
        }
        
        // 确保有一个有效的lua脚本
        if (!LoadLuaScript())
        {
            Debug.LogError("无法加载Lua脚本，LuaTubeController无法工作");
            return;
        }
        
        // 初始化Lua环境
        InitLuaEnvironment();
    }
    
    // 加载Lua脚本内容
    private bool LoadLuaScript()
    {
        // 检查在Inspector中指定的脚本是否有效
        if (luaScript != null)
        {
            return true;
        }
        
        // 尝试通过自定义加载器加载
        try 
        {
            byte[] luaBytes = CustomLuaLoader(ref luaScriptPath);
            if (luaBytes != null && luaBytes.Length > 0)
            {
                string luaCode = Encoding.UTF8.GetString(luaBytes);
                luaScript = new TextAsset(luaCode);
                return true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"尝试加载Lua脚本时出错: {e.Message}");
        }
        
        return false;
    }
    
    // 初始化Lua环境
    private void InitLuaEnvironment()
    {
        if (luaScript == null) return;
        
        try
        {            
            // 创建独立的脚本环境
            scriptEnv = sharedLuaEnv.NewTable();
            
            // 设置元表，允许访问全局变量
            LuaTable meta = sharedLuaEnv.NewTable();
            meta.Set("__index", sharedLuaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            
            // 注入self参数
            scriptEnv.Set("self", this);
            
            // 执行Lua脚本
            object[] results = sharedLuaEnv.DoString(luaScript.text, "LuaTubeScript", scriptEnv);
            
            // 获取返回的模块
            if (results != null && results.Length > 0 && results[0] is LuaTable)
            {
                LuaTable module = results[0] as LuaTable;
                
                // 获取函数引用
                luaStart = module.Get<LuaFunction>("Start");
                luaFixedUpdate = module.Get<LuaFunction>("FixedUpdate");
                luaOnTriggerEnter2D = module.Get<LuaFunction>("OnTriggerEnter2D");
                luaRandomHeight = module.Get<LuaFunction>("RandomHeight");
                
                if (luaStart != null)
                {
                    initialized = true;
                }
            }
            else
            {
                Debug.LogError("Lua脚本没有返回有效的模块");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("初始化Lua环境时出错: " + e.Message);
            Debug.LogError("详细错误: " + e.ToString());
        }
    }
    
    void Start()
    {
        if (initialized && luaStart != null)
        {
            try
            {
                luaStart.Call(gameObject);
            }
            catch (System.Exception e)
            {
                Debug.LogError("调用Lua Start函数时出错: " + e.Message);
            }
        }
    }
    
    void FixedUpdate()
    {
        if (initialized && luaFixedUpdate != null)
        {
            try
            {
                luaFixedUpdate.Call();
            }
            catch (System.Exception e)
            {
                Debug.LogError("调用Lua FixedUpdate函数时出错: " + e.Message);
                initialized = false; // 防止持续报错
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (initialized && luaOnTriggerEnter2D != null)
        {
            try
            {
                luaOnTriggerEnter2D.Call(other);
            }
            catch (System.Exception e)
            {
                Debug.LogError("调用Lua OnTriggerEnter2D函数时出错: " + e.Message);
            }
        }
    }
    
    public void RandomHeight()
    {
        if (initialized && luaRandomHeight != null)
        {
            try
            {
                luaRandomHeight.Call();
            }
            catch (System.Exception e)
            {
                Debug.LogError("调用Lua RandomHeight函数时出错: " + e.Message);
            }
        }
    }
    
    void OnDestroy()
    {
        // 释放资源
        if (luaStart != null)
        {
            luaStart.Dispose();
            luaStart = null;
        }
        
        if (luaFixedUpdate != null)
        {
            luaFixedUpdate.Dispose();
            luaFixedUpdate = null;
        }
        
        if (luaOnTriggerEnter2D != null)
        {
            luaOnTriggerEnter2D.Dispose();
            luaOnTriggerEnter2D = null;
        }
        
        if (luaRandomHeight != null)
        {
            luaRandomHeight.Dispose();
            luaRandomHeight = null;
        }
        
        if (scriptEnv != null)
        {
            scriptEnv.Dispose();
            scriptEnv = null;
        }
    }
} 