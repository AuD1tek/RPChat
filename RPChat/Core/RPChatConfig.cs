using Rocket.API;

namespace RPChat.Core
{
    public class RPChatConfig : IRocketPluginConfiguration
    {
        public bool DisableGlobalChat;
        public bool DisableLocalChat;
        public bool DisableGroupChat;



        public string   NRPChatColor;
        public bool     NRPChatUseRichTextFormatting;

        public string   SMSChatColor;
        public bool     SMSChatUseRichTextFormating;
        public ushort   SMSChatItemId;
        public uint     SMSChatCost;
        public string   SMSChatIcon;

        public string   ADChatColor;
        public bool     ADChatUseRichTextFormating;
        public ushort   ADChatItemId;
        public uint     ADChatCost;
        public string   ADChatIcon;
        public float    ADChatRadius;

        public string   DNChatColor;
        public bool     DNChatUseRichTextFormating;
        public ushort   DNChatItemId;
        public uint     DNChatCost;
        public string   DNChatIcon;
        public float    DNChatRadius;


        public string   MEChatColor;
        public float    MEChatRadius;
        public bool     MEChatUseRichTextFormating;

        public string   SChatColor;
        public float    SChatRadius;
        public bool     SChatUseRichTextFormating;

        public string   WChatColor;
        public float    WChatRadius;
        public bool     WChatUseRichTextFormating;

        public string   TRYChatColor;
        public float    TRYChatRadius;
        public bool     TRYChatUseRichTextFormating;


        public void LoadDefaults()
        {
            DisableGlobalChat   = true;
            DisableLocalChat    = true;
            DisableGroupChat    = true;


            NRPChatColor                    = "";
            NRPChatUseRichTextFormatting    = true;

            SMSChatColor                = "";
            SMSChatUseRichTextFormating = true;
            SMSChatItemId               = 0;
            SMSChatCost                 = 0;
            SMSChatIcon                 = "";

            ADChatColor                 = "";
            ADChatUseRichTextFormating  = true;
            ADChatItemId                = 0;
            ADChatCost                  = 0;
            ADChatIcon                  = "";
            ADChatRadius                = 0;


            DNChatColor                 = "";
            DNChatUseRichTextFormating  = true;
            DNChatItemId                = 0;
            DNChatCost                  = 0;
            DNChatIcon                  = "";
            DNChatRadius                = 0;


            MEChatColor                 = "";
            MEChatRadius                = 0;
            MEChatUseRichTextFormating  = true;

            SChatColor                  = "";
            SChatRadius                 = 0;
            SChatUseRichTextFormating   = true;

            WChatColor                  = "";
            WChatRadius                 = 0;
            WChatUseRichTextFormating   = true;

            TRYChatColor                = "";
            TRYChatRadius               = 0;
            TRYChatUseRichTextFormating = true;

        }
    }
}
