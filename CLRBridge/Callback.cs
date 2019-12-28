using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace CLRBridge
{
    using Handle = IntPtr;
    using static CLRBridge.Primitive;
    public static class Callback
    {
#pragma warning disable IDE0001
        public delegate Handle CallbackHandler(IntPtr context, uint argCount);

        private static CallbackHandler handler;

        public static void SetCallbackHandler(IntPtr handler)
        {
            Callback.handler = Marshal.GetDelegateForFunctionPointer<CallbackHandler>(handler); ;
        }

        private static object ReceiveInvocation(IntPtr context, object[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                HandleTable.Set(new IntPtr(i + 1), args[i]);
            }
            return HandleTable.Get(handler(context, (uint)args.Length));
        }

        public static Handle CreateDelegate(Handle delegateType, IntPtr context, out Handle exception)
        {
            try
            {
                exception = Handle.Zero;
                var typeObj = (Type)HandleTable.Get(delegateType);
                return AddRef(CompileHandlerInvocation(typeObj, args => ReceiveInvocation(context, args)));
            }
            catch (Exception e)
            {
                exception = AddRef(e);
                return Handle.Zero;
            }
        }
#pragma warning restore IDE0001

        private delegate object ChangeTypeMethod(object value, Type convertionType);

        private static Delegate CompileHandlerInvocation(Type delegateType, Func<object[], object> handler)
        {
            var invokeFun = delegateType.GetMethod("Invoke");
            var parameters = invokeFun.GetParameters().Select(parm => Expression.Parameter(parm.ParameterType, parm.Name)).ToArray();
            var target = handler.Target == null ? null : Expression.Constant(handler.Target);
            var parametersObj = parameters.Select(parm => Expression.Convert(parm, typeof(object)));
            var call = Expression.Call(target, handler.Method, Expression.NewArrayInit(typeof(object), parametersObj));
            ChangeTypeMethod method = Convert.ChangeType;
            var changeType = Expression.Call(method.Method, call, Expression.Constant(invokeFun.ReturnType));
            var body = invokeFun.ReturnType == typeof(void) ? (Expression)call : Expression.Convert(changeType, invokeFun.ReturnType);
            return Expression.Lambda(delegateType, body, parameters).Compile();
        }
    }
}
