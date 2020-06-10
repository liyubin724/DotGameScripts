using DotEngine.Crypto;
using DotEngine.Framework;
using DotEngine.Log;
using DotEngine.Net.Client;
using DotEngine.Net.Server;
using DotEngine.Net.Services;
using Game.Net.Proto;
using Game.Net.Protos;

namespace Game.Command
{
    public class LaunchNetCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            AppFacade.GetInstance().RemoveCommand(AppConst.LAUNCH_NET);

            AESKey aesKey = AESCrypto.CreateKey();

            ClientNetService clientNetService = AppFacade.GetInstance().RetrieveService<ClientNetService>(ClientNetService.NAME);
            ServerNetService serverNetService = AppFacade.GetInstance().RetrieveService<ServerNetService>(ServerNetService.NAME);

            ServerNetListener serverNetListener = serverNetService.CreateNet(new ServerMessageParser()
            {
                SecretKey = aesKey.Key,
                SecretVector = aesKey.IV,
            });
            serverNetListener.RegisterMessageHandler(10001, (netID,messageID,message) =>
            {
                LoginRequest lr = (LoginRequest)message;
                LogUtil.LogError("ServerNet",$"userAcc = {lr.UserAccount},password = {lr.Password}");

                LoginResponse lResponse = new LoginResponse()
                {
                    Result = true,
                };
                serverNetListener.SendMessage(netID, 1, lResponse);
            });
            serverNetListener.RegisterMessageHandler(10002, (netID, messageID, message) =>
            {
                ShopListRequest slr = (ShopListRequest)message;
                LogUtil.LogInfo("ServerNet", $"Recevice message ShopListRequest->{slr.ShopType},{slr.PageNumber}");

                ShopListResponse slResponse = new ShopListResponse()
                {
                    Names = "Test",
                };
                serverNetListener.SendMessage(netID, 2, slResponse);
            });

            ClientNet clientNet = clientNetService.CreateNet(AppConst.GAME_NET_ID, new ClientMessageParser()
            {
                SecretKey = aesKey.Key,
                SecretVector = aesKey.IV,
            });
            clientNet.RegisterMessageHandler(1, (messageID,message) =>
            {
                LoginResponse lr = (LoginResponse)message;
                if(lr.Result)
                {
                    LogUtil.LogInfo("ClientNet", $"Recevice mesage LoginResponse->result={lr.Result}");
                    ShopListRequest slr = new ShopListRequest()
                    {
                        ShopType = 1,
                        PageNumber = 11,
                    };
                    clientNet.SendMessage(10002, slr);
                }else
                {
                    LogUtil.LogInfo("ClientNet", $"Recevice mesage LoginResponse->result={lr.Result}");
                }
            });
            clientNet.RegisterMessageHandler(2, (messageID, message) =>
             {
                 ShopListResponse slr = (ShopListResponse)message;
                 LogUtil.LogInfo("ClientNet", $"Recevice message ShopListResponse->names = {slr.Names}");
             });
            clientNet.Connect("127.0.0.1", 9999);

            clientNet.NetConnectedSuccess += (net) =>
            {
                LoginRequest loginRequest = new LoginRequest()
                {
                    UserAccount = 888,
                    Password = "TTF001",
                };
                clientNet.SendMessage(10001, loginRequest);
            };

            
        }
    }
}
