/*The file was created by tool.
-----------------------------------------------
Please don't change it manually!!!
Please don't change it manually!!!
Please don't change it manually!!!
-----------------------------------------------
*/
using Dot.Net.Server;


namespace Game.Net.Proto
{
    public static class C2SProto_Parser
    {
        public static void RegisterParser(ServerNetListener serverNetListener)
        {
            serverNetListener.RegisterMessageParser(C2SProto.C2S_LOGIN,Parse_LoginRequest);
            serverNetListener.RegisterMessageParser(C2SProto.C2S_SHOP_LIST,Parse_ShopListRequest);
        }

        private static object Parse_LoginRequest(int messageID,byte[] msgBytes)
        {
            return LoginRequest.Parser.ParseFrom(msgBytes);
        }

        private static object Parse_ShopListRequest(int messageID,byte[] msgBytes)
        {
            return ShopListRequest.Parser.ParseFrom(msgBytes);
        }

    }
}