/*The file was created by tool.
-----------------------------------------------
Please don't change it manually!!!
Please don't change it manually!!!
Please don't change it manually!!!
-----------------------------------------------
*/
using Dot.Net.Client;


namespace Game.Net.Proto
{
    public static class S2CProto_Parser
    {
        public static void RegisterParser(ClientNet clientNet)
        {
            clientNet.RegisterMessageParser(S2CProto.S2C_LOGIN,Parse_LoginResponse);
            clientNet.RegisterMessageParser(S2CProto.S2C_SHOP_LIST,Parse_ShopListResponse);
        }

        private static object Parse_LoginResponse(int messageID,byte[] msgBytes)
        {
            return LoginResponse.Parser.ParseFrom(msgBytes);
        }

        private static object Parse_ShopListResponse(int messageID,byte[] msgBytes)
        {
            return ShopListResponse.Parser.ParseFrom(msgBytes);
        }

    }
}