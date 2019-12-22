using System;
using System.Collections.Generic;

namespace CLRBridge
{
    using Handle = IntPtr;
    static class HandleTable
    {
        static Dictionary<Handle, object> table = new Dictionary<Handle, object>();
        public const long FreeStorageSize = 256;
        static Handle nextKey = new Handle(FreeStorageSize + 1);

        public static Handle Add(object obj)
        {
            Handle curKey = nextKey;
            nextKey = nextKey.IncrementByOne();
            table.Add(curKey, obj);
            return curKey;
        }

        public static Handle Set(Handle key, object obj)
        {
            var n = key.ToInt64();
            if (n > 0 && n <= FreeStorageSize)
            {
                table[key] = obj;
                return key;
            }
            return Handle.Zero;
        }

        public static object Get(Handle key)
        {
            return table.TryGetValue(key, out object value) ? value : null;
        }

        public static bool Remove(Handle key)
        {
            return table.Remove(key);
        }

        static Handle IncrementByOne(this Handle pointer)
        {
            unchecked
            {
                switch (Handle.Size)
                {
                    case sizeof(int):
                        return (new Handle(pointer.ToInt32() + 1));

                    default:
                        return (new Handle(pointer.ToInt64() + 1L));
                }
            }
        }
    }
}
