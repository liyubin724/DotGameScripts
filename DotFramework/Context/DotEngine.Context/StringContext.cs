namespace DotEngine.Context
{
    public class StringContext : EnvContext<string>
    {
        public static void Inject(StringContext context, object injectObj)
        {
            ContextUtil.Inject<string>(context, injectObj);
        }

        public static void Extract(StringContext context, object extractObj)
        {
            ContextUtil.Extract<string>(context, extractObj);
        }
    }
}
