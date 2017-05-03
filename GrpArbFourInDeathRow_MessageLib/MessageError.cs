namespace GrpArbFourInDeathRow_MessageLib
{
    public class MessageError : Message
    {
        public  MessageError FromJson(string jsonMsg)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageError>(jsonMsg);
        }
    }


}