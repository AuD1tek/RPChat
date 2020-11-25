using Rocket.API;

namespace RPChat.Core
{
    public class RPChatConfig : IRocketPluginConfiguration
    {
        public bool IgnoreAdmin;
        public bool DisableGlobalChat;
        public bool DisableLocalChat;
        public bool DisableGroupChat;


        public string   NRPChatColor;

        public string   SMSChatColor;
        public ushort   SMSChatItemId;
        public uint     SMSChatCost;
        public string   SMSChatIcon;

        public string   ADChatColor;
        public ushort   ADChatItemId;
        public uint     ADChatCost;
        public string   ADChatIcon;
        public float    ADChatRadius;

        public string   DNChatColor;
        public ushort   DNChatItemId;
        public uint     DNChatCost;
        public string   DNChatIcon;
        public float    DNChatRadius;


        public string   MEChatColor;
        public float    MEChatRadius;

        public string   SChatColor;
        public float    SChatRadius;

        public string   WChatColor;
        public float    WChatRadius;

        public string   TRYChatColor;
        public float    TRYChatRadius;


        public void LoadDefaults()
        {
            IgnoreAdmin = true;
            DisableGlobalChat   = true;
            DisableLocalChat    = true;
            DisableGroupChat    = true;


            NRPChatColor = "";

            SMSChatColor    = "";
            SMSChatItemId   = 0;
            SMSChatCost     = 0;
            SMSChatIcon     = "";

            ADChatColor     = "";
            ADChatItemId    = 0;
            ADChatCost      = 0;
            ADChatIcon      = "";
            ADChatRadius    = 0;


            DNChatColor     = "";
            DNChatItemId    = 0;
            DNChatCost      = 0;
            DNChatIcon      = "";
            DNChatRadius    = 0;


            MEChatColor     = "";
            MEChatRadius    = 0;

            SChatColor      = "";
            SChatRadius     = 0;

            WChatColor      = "";
            WChatRadius     = 0;

            TRYChatColor    = "";
            TRYChatRadius   = 0;

        }
    }
}
