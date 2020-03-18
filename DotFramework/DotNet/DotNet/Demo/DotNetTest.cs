//This class is used to test the netframework

/*
using Dot.Core.Proxy;
using Dot.Log;
using Dot.Net.Client;
using Dot.Net.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Net.Demo
{
    [Serializable]
    public class NetMessage
    {
        public string message;
    }

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

        private ClientNet clientNet = null;
        private string clientInputStr = string.Empty;
        private List<string> clientReceivedList = new List<string>();

        private ClientNet clientNet1 = null;
        private string clientInputStr1 = string.Empty;
        private List<string> clientReceivedList1 = new List<string>();

        private int index = 0;
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Start Server"))
                    {
                        serverNet = NetManager.GetInstance().StartupAsServer(1100);
                        serverNet.RegisterHandler(1000, (netID, messageID, message) =>
                        {
                            Debug.Log("message = " + ((string)message));
                            try
                            {
                                NetMessage netMess = JsonConvert.DeserializeObject<NetMessage>((string)message);
                                serverReceivedList.Add(netMess.message);
                            }
                            catch
                            {

                            }

                            NetMessage nm = new NetMessage()
                            {
                                message = "--->" + index.ToString()
                            };
                            index++;
                            serverNet.SendMessage<NetMessage>(netID, 10001, nm);
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
                        clientNet = NetManager.GetInstance().CreateClientNet(0);
                        clientNet.Connect("127.0.0.1", 1100);
                        clientNet.RegisterHandler(10001, (message) =>
                        {
                            NetMessage netMess = JsonConvert.DeserializeObject<NetMessage>((string)message);
                            clientReceivedList.Add(netMess.message);
                        });
                    }
                    GUILayout.Label("Input:");
                    clientInputStr = GUILayout.TextArea(clientInputStr);
                    if (GUILayout.Button("Send"))
                    {
                        clientNet.SendMessage<NetMessage>(1000, new NetMessage()
                        {
                            message = clientInputStr
                        });
                    }
                    GUILayout.Label("Received:");
                    GUILayout.TextArea(string.Join("\r\n", clientReceivedList.ToArray()));

                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    if (GUILayout.Button("Start Client"))
                    {
                        clientNet1 = NetManager.GetInstance().CreateClientNet(1);
                        clientNet1.Connect("127.0.0.1", 1100);
                        clientNet1.RegisterHandler(10001, (message) =>
                        {
                            NetMessage netMess = JsonConvert.DeserializeObject<NetMessage>((string)message);
                            clientReceivedList1.Add(netMess.message);
                        });
                    }
                    GUILayout.Label("Input:");
                    clientInputStr1 = GUILayout.TextArea(clientInputStr1);
                    if (GUILayout.Button("Send"))
                    {
                        clientNet1.SendMessage<NetMessage>(1000, new NetMessage()
                        {
                            message = clientInputStr1
                        });
                    }
                    GUILayout.Label("Received:");
                    GUILayout.TextArea(string.Join("\r\n", clientReceivedList1.ToArray()));

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();



        }
    }

}
*/