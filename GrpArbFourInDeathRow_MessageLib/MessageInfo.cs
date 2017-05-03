namespace GrpArbFourInDeathRow_MessageLib
{
    public class MessageInfo : Message
    {
        public  MessageInfo FromJson(string jsonMsg)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageInfo>(jsonMsg);
        }
    }
}