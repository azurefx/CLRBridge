using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CLRBridge
{
    using Handle = IntPtr;
    using static CLRBridge.Primitive;
    public static class Meta
    {
        public static Handle GetType([MarshalAs(UnmanagedType.BStr)]string typeName, out Handle exception)
        {
            try
            {
                exception = Handle.Zero;
                return AddRef(Type.GetType(typeName, true));
            }
            catch (Exception e)
            {
                exception = AddRef(e);
                return Handle.Zero;
            }
        }

        public static Handle GetObjectType(Handle handle)
        {
            var obj = HandleTable.Get(handle);
            if (obj == null)
            {
                return Handle.Zero;
            }
            var type = obj.GetType();
            return AddRef(type);
        }

        public static Handle InvokeMember(Handle type, [MarshalAs(UnmanagedType.BStr)]string name, BindingFlags bindingFlags, Handle binder, Handle target, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)]Handle[] providedArgs, ulong argCount, out Handle exception)
        {
            try
            {
                Type typeObj = (Type)HandleTable.Get(type);
                Binder binderObj = (Binder)HandleTable.Get(binder);
                object targetObj = HandleTable.Get(target);
                object[] providedArgsObj = Array.ConvertAll(providedArgs, HandleTable.Get);
                object result = typeObj.InvokeMember(name, bindingFlags, binderObj, targetObj, providedArgsObj);
                exception = Handle.Zero;
                if (result != null)
                {
                    return AddRef(result);
                }
                else
                {
                    return Handle.Zero;
                }
            }
            catch (Exception e)
            {
                exception = AddRef(e);
                return Handle.Zero;
            }
        }
    }
}
