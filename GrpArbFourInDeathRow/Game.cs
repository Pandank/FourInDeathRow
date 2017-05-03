using System.Threading;

namespace GrpArbFourInDeathRow
{
    public class Game
    {
        public void StartClient()
        {

            Program.Client myClient = new Program.Client();

            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
            clientThread.Join();
        }
    }
}