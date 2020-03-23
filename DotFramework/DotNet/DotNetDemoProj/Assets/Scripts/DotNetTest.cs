//This class is used to test the netframework

using Dot.Core.Proxy;
using Dot.Log;
using Dot.Net.Client;
using Dot.Net.Server;
using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Net.Proto;
using Google.Protobuf;

namespace Dot.Net.Demo
{
    public class DotNetTest : MonoBehaviour
    {
        public TextAsset log4netConfig = null;
        void Start()
        {
            LogUtil.Initalize(log4netConfig.text);
        }

        void Update()
        {
            UpdateProxy.GetInstance().DoUpdate(Time.deltaTime);
            UpdateProxy.GetInstance().DoUnscaleUpdate(Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            UpdateProxy.GetInstance().DoLateUpdate();
        }

        private ServerNetListener serverNet = null;
        private List<string> serverReceivedList = new List<string>();

        private ClientNet clientNet1 = null;
        private List<string> clientReceivedList1 = new List<string>();

        private ClientNet clientNet2 = null;
        private List<string> clientReceivedList2 = new List<string>();

        private int index = 0;
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Start Server"))
                    {
                        serverNet = NetManager.GetInstance().CreateServerNet(1100);
                        C2SProto_Parser.RegisterParser(serverNet);
                        serverNet.RegisterMessageHandler(C2SProto.C2S_LOGIN, (netID,messageID, message) =>
                        {
                            LoginRequest request = (LoginRequest)message;
                            serverReceivedList.Add($"netID = {netID},account = {request.UserAccount},password={request.Password}");

                            LoginResponse response = new LoginResponse();
                            response.Result = true;
                            serverNet.SendData(netID, S2CProto.S2C_LOGIN, response.ToByteArray());
                        });
                        serverNet.RegisterMessageHandler(C2SProto.C2S_SHOP_LIST, (netID, messageID, message) =>
                        {
                            ShopListRequest request = (ShopListRequest)message;
                            serverReceivedList.Add($"netID = {netID},ShopType = {request.ShopType},PageNumber={request.PageNumber}");

                            ShopListResponse response = new ShopListResponse();
                            response.Names = $"Test Shop ->{request.ShopType} ->{request.PageNumber}";
                            serverNet.SendData(netID, S2CProto.S2C_SHOP_LIST, response.ToByteArray());
                        });
                    }
                    GUILayout.Label("Received:");
                    GUILayout.TextArea(string.Join("\r\n", serverReceivedList.ToArray()));

                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Start Client"))
                    {
                        clientNet1 = NetManager.GetInstance().CreateClientNet();
                        S2CProto_Parser.RegisterParser(clientNet1);

                        clientNet1.Connect("127.0.0.1", 1100);
                        clientNet1.RegisterMessageHandler(S2CProto.S2C_LOGIN, (messageID,message) =>
                        {
                            LoginResponse response = (LoginResponse)message;
                            clientReceivedList1.Add($"netID = {clientNet1.UniqueID},Result={response.Result}");
                        });
                        clientNet1.RegisterMessageHandler(S2CProto.S2C_SHOP_LIST, (messageID, message) =>
                        {
                            ShopListResponse response = (ShopListResponse)message;
                            clientReceivedList1.Add($"netID = {clientNet1.UniqueID},names={response.Names}");
                        });
                    }

                    if(GUILayout.Button("Login"))
                    {
                        LoginRequest request = new LoginRequest();
                        request.UserAccount = 123456;
                        request.Password = "TTF001";
                        clientNet1.SendPBMessage<LoginRequest>(C2SProto.C2S_LOGIN,request);
                    }
                    if(GUILayout.Button("Get Shop List"))
                    {
                        ShopListRequest request = new ShopListRequest();
                        request.ShopType = 1;
                        request.PageNumber = 10;
                        clientNet1.SendPBMessage<ShopListRequest>(C2SProto.C2S_SHOP_LIST, request);
                    }
                    GUILayout.Label("Received:");
                    GUILayout.TextArea(string.Join("\r\n", clientReceivedList1.ToArray()));

                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Start Client"))
                    {
                        clientNet2 = NetManager.GetInstance().CreateClientNet();
                        S2CProto_Parser.RegisterParser(clientNet2);

                        clientNet2.Connect("127.0.0.1", 1100);
                        clientNet2.RegisterMessageHandler(S2CProto.S2C_LOGIN, (messageID, message) =>
                        {
                            LoginResponse response = (LoginResponse)message;
                            clientReceivedList2.Add($"netID = {clientNet2.UniqueID},Result={response.Result}");
                        });
                        clientNet2.RegisterMessageHandler(S2CProto.S2C_SHOP_LIST, (messageID, message) =>
                        {
                            ShopListResponse response = (ShopListResponse)message;
                            clientReceivedList2.Add($"netID = {clientNet2.UniqueID},names={response.Names}");
                        });
                    }

                    if (GUILayout.Button("Login"))
                    {
                        LoginRequest request = new LoginRequest();
                        request.UserAccount = 654321;
                        request.Password = "DDR001";
                        clientNet2.SendPBMessage<LoginRequest>(C2SProto.C2S_LOGIN, request);
                    }
                    if (GUILayout.Button("Get Shop List"))
                    {
                        ShopListRequest request = new ShopListRequest();
                        request.ShopType = 2;
                        request.PageNumber = 11;
                        clientNet2.SendPBMessage<ShopListRequest>(C2SProto.C2S_SHOP_LIST, request);
                    }
                    GUILayout.Label("Received:");
                    GUILayout.TextArea(string.Join("\r\n", clientReceivedList2.ToArray()));

                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if(GUILayout.Button("Clear Log"))
                    {
                        clientReceivedList1.Clear();
                        clientReceivedList2.Clear();
                        serverReceivedList.Clear();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();



        }
    }

}
