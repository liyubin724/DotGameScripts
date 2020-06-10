using DotEngine.Log;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILogger = DotEngine.Log.ILogger;

public class Startup : MonoBehaviour
{
    public TextAsset logConfig = null;

    void Start()
    {
        if(logConfig!=null)
        {
            string configText = logConfig.text;
            configText = configText.Replace("#OUTPUT_DIR#", "D:/WorkSpace/DotGameProject/DotGameScripts/DotFramewrokProject");
            ILogger logger = Log4NetLogger.Initalize(configText);
            if(logger!=null)
            {
                LogUtil.LimitLevel = LogLevelType.Debug;
                LogUtil.SetLogger(logger);
            }
        }

        AppFacade.Startup();
    }
}
