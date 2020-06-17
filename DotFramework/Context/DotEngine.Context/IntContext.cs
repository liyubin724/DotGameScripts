namespace DotEngine.Context
{
    public class IntContext : EnvContext<int>
    {
        public static void Inject(IntContext context, object injectObj)
        {
            ContextUtil.Inject<int>(context, injectObj);
        }

        public static void Extract(IntContext context, object extractObj)
        {
            ContextUtil.Extract<int>(context, extractObj);
        }
    }
}
