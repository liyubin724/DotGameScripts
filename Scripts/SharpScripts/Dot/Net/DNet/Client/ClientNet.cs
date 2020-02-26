using Dot.Net.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dot.Net.Client
{
    public class ClientNet 
    {
        private IMessageWriter messageWriter = null;
        private IClientNetDataReceiver dataReceiver = null;

        private ClientNetSession netSession = null;

        public ClientNet(IMessageWriter writer,IClientNetDataReceiver receiver)
        {
            messageWriter = writer;
            dataReceiver = receiver;
        }



    }
}
