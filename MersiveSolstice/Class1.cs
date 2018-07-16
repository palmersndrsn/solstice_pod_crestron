using System;
using System.Text;
using Crestron.SimplSharp;                          				
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.CrestronIO;
using Newtonsoft.Json;


namespace MersiveSolstice
{
    public class MDisplayInformation
    {
        public string m_displayName { get; set; }
        public string m_productName { get; set; }
        public string m_productVariant { get; set; }
        public int m_productHardwareVersion { get; set; }
    }

    public class MStatistics
    {
        public int m_currentPostCount { get; set; }
        public int m_currentBandwith { get; set; }
        public int m_currentLiveSourceCount { get; set; }
        public int m_connectedUsers { get; set; }
        public int m_timeSinceLastConnectionInitialize { get; set; }
    }

    public class StatsMain
    {
        public string m_displayId { get; set; }
        public string m_serverVersion { get; set; }
        public MDisplayInformation m_displayInformation { get; set; }
        public MStatistics m_statistics { get; set; }
    }
    public class Solstice
        {

        public string displayName { get; set; }
        public string productName { get; set; }
        public string productVariant { get; set; }
        
        public int productHardwareVersion { get; set; }
        public int currentPostCount { get; set; }
        public int currentBandwith { get; set; }
        public int currentLiveSourceCount { get; set; }
        public int connectedUsers { get; set; }
        public int timeSinceLastConnectionInitialize { get; set; }

        public Solstice()
        {
        }

        public void Connect(string url)
        {
            HttpClient myClient = new HttpClient();
            myClient.AllowAutoRedirect = true;
            myClient.Verbose = true;

            HttpClientRequest myRequest = new HttpClientRequest();
            HttpClientResponse myResponse;

            try
            {

                myRequest.Header.SetHeaderValue("content-type", "application/json");
                myRequest.RequestType = RequestType.Get;

                myClient.TimeoutEnabled = true;
                myClient.Timeout = 30;
                myRequest.KeepAlive = false;

                myRequest.Url = new UrlParser(url);
                myResponse = myClient.Dispatch(myRequest);

                CrestronConsole.PrintLine(myResponse.ContentString);
                CrestronConsole.PrintLine("{0}", myResponse.Code);

                //deserialize json
                StatsMain ObjStatsMain = JsonConvert.DeserializeObject<StatsMain>(myResponse.ContentString);

                //CrestronConsole.PrintLine("Display ID " + ObjStatsMain.m_displayId + "/n");
                //CrestronConsole.PrintLine("Server Version " + ObjStatsMain.m_serverVersion + "/n");
                //CrestronConsole.PrintLine("Time since last connect: {0}", ObjStatsMain.m_statistics.m_timeSinceLastConnectionInitialize + "/n");
                //CrestronConsole.PrintLine("Current Bandwidth: {0}", ObjStatsMain.m_statistics.m_currentBandwith + "/n");
                
                //set resposne to each 
                this.displayName = ObjStatsMain.m_displayInformation.m_displayName;
                this.productName = ObjStatsMain.m_displayInformation.m_productName;
                this.productVariant = ObjStatsMain.m_displayInformation.m_productVariant;

                this.productHardwareVersion = ObjStatsMain.m_displayInformation.m_productHardwareVersion;
                this.currentPostCount = ObjStatsMain.m_statistics.m_currentPostCount;
                this.currentBandwith = ObjStatsMain.m_statistics.m_currentBandwith;
                this.currentLiveSourceCount = ObjStatsMain.m_statistics.m_currentLiveSourceCount;
                this.connectedUsers = ObjStatsMain.m_statistics.m_connectedUsers;
                this.timeSinceLastConnectionInitialize = convertLongToShort(ObjStatsMain.m_statistics.m_timeSinceLastConnectionInitialize);


            }
            catch (Exception e)
            {
                ErrorLog.Error("Exception in Mersive Connect: {0}", e.Message);
            }
            
        }
        public int convertLongToShort(int signedLong)
        {
           return signedLong / 1000;
        }
    }
}