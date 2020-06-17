using System;

namespace DotEngine.Context
{
    public class TypeContext : EnvContext<Type>
    {
        public static void Inject(TypeContext context, object injectObj)
        {
            ContextUtil.Inject<Type>(context, injectObj);
        }

        public static void Extract(TypeContext context, object extractObj)
        {
            ContextUtil.Extract<Type>(context, extractObj);
        }
    }
}
