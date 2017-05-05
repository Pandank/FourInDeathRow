namespace GrpArbFourInDeathRow_MessageLib
{
    public class MessageGame : Message
    {
        public int CoordX { get; set; }  
        public int CoordY { get; set; }
        public int[,] BoardState { get; set; }

        public bool GameOver { get; set; }
        public bool IsFromServer { get; set; }


        public  MessageGame FromJson(string jsonMsg)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageGame>(jsonMsg);
        }
    }
}   