using System;
using System.Runtime.InteropServices;

namespace CLRBridge
{
    using Handle = IntPtr;
    public static class Primitive
    {
        public static Handle AddRef(object obj)
        {
            return HandleTable.Add(obj);
        }

        public static bool Release(Handle handle)
        {
            return HandleTable.Remove(handle);
        }

#pragma warning disable IDE0001
        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetString(Handle handle)
        {
            object obj = HandleTable.Get(handle);
            if (obj is string)
            {
                return (string)obj;
            }
            else
            {
                try
                {
                    return obj.ToString();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static void FreeString(IntPtr ptr)
        {
            Marshal.FreeBSTR(ptr);
        }
#pragma warning restore IDE0001

        public static Handle PutString(Handle handle, [MarshalAs(UnmanagedType.BStr)]string value)
        {
            return HandleTable.Set(handle, value);
        }

        public static Handle PutBool(Handle handle, byte value)
        {
            return HandleTable.Set(handle, value != 0);
        }

        public static byte GetBool(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is bool ? (byte)((bool)value ? 1 : 0) : (byte)0;
        }

        public static Handle PutInt8(Handle handle, sbyte value)
        {
            return HandleTable.Set(handle, value);
        }

        public static sbyte GetInt8(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is sbyte ? (sbyte)value : (sbyte)0;
        }

        public static Handle PutUInt8(Handle handle, byte value)
        {
            return HandleTable.Set(handle, value);
        }

        public static byte GetUInt8(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is byte ? (byte)value : (byte)0;
        }

        public static Handle PutInt16(Handle handle, short value)
        {
            return HandleTable.Set(handle, value);
        }

        public static short GetInt16(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is short ? (short)value : (short)0;
        }

        public static Handle PutUInt16(Handle handle, ushort value)
        {
            return HandleTable.Set(handle, value);
        }

        public static ushort GetUInt16(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is ushort ? (ushort)value : (ushort)0;
        }

        public static Handle PutInt32(Handle handle, int value)
        {
            return HandleTable.Set(handle, value);
        }

        public static int GetInt32(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is int ? (int)value : 0;
        }

        public static Handle PutUInt32(Handle handle, uint value)
        {
            return HandleTable.Set(handle, value);
        }

        public static uint GetUInt32(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is uint ? (uint)value : 0;
        }

        public static Handle PutInt64(Handle handle, long value)
        {
            return HandleTable.Set(handle, value);
        }

        public static long GetInt64(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is long ? (long)value : 0;
        }

        public static Handle PutUInt64(Handle handle, ulong value)
        {
            return HandleTable.Set(handle, value);
        }

        public static ulong GetUInt64(Handle handle)
        {
            var value = HandleTable.Get(handle);
            return value is ulong ? (ulong)value : 0;
        }
    }
}
