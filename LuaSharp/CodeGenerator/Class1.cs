using LuaSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace LuaSharp.CodeGenerator
{
    public class FuncInfo
    {
        public FuncInfo Parent { get; set; }
        public List<FuncInfo> SubFuncs { get; set; }

        /// <summary>
        /// 使用的寄存器数量
        /// </summary>
        public int UsedRegsCount { get; set; }
        /// <summary>
        /// 最大寄存器数量
        /// </summary>
        public int MaxRegsCount { get; set; }

        /// <summary>
        /// start from 0
        /// </summary>
        public int ScopeLevel { get; set; }

        /// <summary>
        /// all vars
        /// </summary>
        public List<LocVarInfo> LocVars { get; set; }

        /// <summary>
        /// current level vars
        /// </summary>
        public Dictionary<string, LocVarInfo> LocNames { get; set; }
        public Dictionary<string, UpvalInfo> Upvalues { get; set; }
        public Dictionary<object, int> Constants { get; set; }
        public List<List<int>> Breaks { get; set; }
        public List<uint> Insts { get; set; }
        public int NumParams { get; set; }
        public bool IsVararg { get; set; }

        /// <summary>
        /// If not contain the key, add it to the constants and return the index of the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int IndexOfConstants(object key)
        {
            if (Constants.ContainsKey(key))
            {
                return Constants[key];
            }
            int idx = Constants.Count;
            Constants[key] = idx;
            return idx;
        }

        /// <summary>
        /// return  the index of the new register .
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CodeGenerateException"></exception>
        public int AllocReg()
        {
            UsedRegsCount++;
            if (UsedRegsCount >= 255)
            {
                throw new CodeGenerateException("Function or expression needs too many registers. Max Count is 255");
            }
            if (UsedRegsCount > MaxRegsCount)
            {
                MaxRegsCount = UsedRegsCount;
            }
            return UsedRegsCount - 1;
        }

        public int AllocRegs(int n)
        {
            if (n <= 0)
            {
                throw new CodeGenerateException("AllocRegs count must be greater than 0.");
            }
            for (int i = 0; i < n; i++)
            {
                AllocReg();
            }
            return UsedRegsCount - n;
        }


        public void FreeReg()
        {

            if (UsedRegsCount <= 0)
            {
                throw new CodeGenerateException("No register to free.");
            }
            UsedRegsCount--;
        }


        public void FreeRegs(int n)
        {
            if (n <= 0)
            {
                throw new CodeGenerateException("FreeRegs count must be greater than 0.");
            }
            for (int i = 0; i < n; i++)
            {
                FreeReg();
            }
        }


        public void EnterNewScope()
        {
            ScopeLevel++;
        }

        public int AddLocalVar(string name)
        {
            var newVar = new LocVarInfo(
                name,
                this.LocNames[name],
                this.ScopeLevel,
                this.AllocReg()
                );
            this.LocVars.Add(newVar);
            this.LocNames[name] = newVar;
            return newVar.Slot;

        }

        /// <summary>
        /// return slot or -1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int SlotOfLocVar(string name)
        {
            if (this.LocNames.ContainsKey(name))
            {
                return this.LocNames[name].Slot;
            }
            return -1;
        }
        public void ExitScope()
        {
            ScopeLevel--;
            List<LocVarInfo> locVarsToRemove = new List<LocVarInfo>();

            foreach (LocVarInfo locVar in LocNames.Values)
            {
                if (locVar.ScopeLevel > ScopeLevel)
                {
                    locVarsToRemove.Add(locVar);
                }
            }


            foreach (var locVarToRemove in locVarsToRemove)
            {
                RemoveLocVar(locVarToRemove);
            }
        }

        public void RemoveLocVar(LocVarInfo locVar)
        {
            FreeReg(); // Recycle register

            if (locVar.Prev == null)
            {
                LocNames.Remove(locVar.Name);
            }
            else if (locVar.Prev.ScopeLevel == locVar.ScopeLevel)
            {
                RemoveLocVar(locVar.Prev);
            }
            else
            {
                LocNames[locVar.Name] = locVar.Prev;
            }
        }



    }

    /// <summary>
    /// 局部变量
    /// 
    /// </summary>
    public class LocVarInfo
    {


        public LocVarInfo Prev { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 变量的作用域等级
        /// </summary>
        public int ScopeLevel { get; set; }
        /// <summary>
        /// 与局部变量绑定的寄存器索引
        /// </summary>
        public int Slot { get; set; }
        /// <summary>
        /// 是否被闭包捕获
        /// </summary>
        public bool Captured { get; set; }

        public LocVarInfo(string name, LocVarInfo prev, int scopeLevel, int slot)
        {
            Name = name;
            Prev = prev;
            ScopeLevel = scopeLevel;
            Slot = slot;
        }
    }

    public class UpvalInfo
    {
        public int LocVarSlot { get; set; }
        public int UpvalIndex { get; set; }
        public int Index { get; set; }
    }
}
