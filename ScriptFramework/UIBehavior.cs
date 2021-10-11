using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

namespace GraphyFW
{
    public class UIBehavior
    {
        
        private Action luaEnter;
        private Action luaResume;
        private Action luaPausse;
        private Action luaExit;

        private LuaTable scriptEnv;
        private LuaEnv refLuaEnv;

        private string luaCode;

        private string luaCodeName;
        public UIBehavior(string luaCode, string luaCodeName)
        {
            this.luaCode = luaCode;
            this.luaCodeName = luaCodeName;
        }

        private void Start()
        {
            refLuaEnv = XluaConfig.luaEnv;

            scriptEnv = XluaConfig.luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = XluaConfig.luaEnv.NewTable();
            meta.Set("__index", XluaConfig.luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

             XluaConfig.luaEnv.DoString(luaCode, luaCodeName, scriptEnv);

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("enter", out luaEnter);
            scriptEnv.Get("resume", out luaResume);
            scriptEnv.Get("pause", out luaPausse);
            scriptEnv.Get("exit", out luaExit);

        }
        public void Enter()
        {
            if (luaEnter != null)
            {
                luaEnter();
            }
        }

        public void Resume()
        {
            if (luaResume != null)
            {
                luaResume();
            }
        }

        public void Pause()
        {
            if (luaPausse != null)
            {
                luaPausse();
            }
        }

        public void Exit()
        {
            if (luaExit != null)
            {
                luaExit();
            }
        }


    }


}
