namespace EwayBot.DAL.Constants
{
    public class Constants
    {
        //commands
        public static string Start = @"/start";
        public static string Info = @"/info";
        public static string SearchByStopName = @"/searchByStopName";
        public static string SearchStopByYourLocation = @"/searchStopByYourLocation";
        public static string SearchByTransportNumber = @"/searchByTransportNumber";

        //callbacks
        public static string StopMapCallback = @"/stopMapCallback";
        public static string TransportTypeCallback = @"/transportTypeCallback";
        public static string TransportNameCallback = @"/transportNameCallback";
        public static string LocationMapCallback = @"/locationMapCallback";


        public static string EditedMessagedId = @"/editedMessagedId";
        public static string PreviousTransportNameCallbackButton = @"/previousTransportNameCallbackButton";
        public static string NextTransportNameCallbackButton = @"/nextTransportNameCallbackButton";
    }
}
