using DotEngine.Crypto;
using DotEngine.Framework;
using DotEngine.Net.Client;
using DotEngine.Net.Server;
using DotEngine.Net.Services;
using Game.Net.Proto;
using Game.Net.Protos;
using System.Security.Cryptography;

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
                UnityEngine.Debug.LogError($"userAcc = {lr.UserAccount},password = {lr.Password}");
            });
            serverNetListener.RegisterMessageHandler(10002, (netID, messageID, message) =>
            {

            });

            ClientNet clientNet = clientNetService.CreateNet(AppConst.GAME_NET_ID, new ClientMessageParser()
            {
                SecretKey = aesKey.Key,
                SecretVector = aesKey.IV,
            });
            clientNet.RegisterMessageHandler(1, (messageID,message) =>
            {

            });
            clientNet.RegisterMessageHandler(2, (messageID, message) =>
             {

             });
            clientNet.Connect("127.0.0.1", 9999);

            clientNet.NetConnectedSuccess += (net) =>
            {
                UnityEngine.Debug.LogError("SSSSSSSSSSSSSSS");
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
