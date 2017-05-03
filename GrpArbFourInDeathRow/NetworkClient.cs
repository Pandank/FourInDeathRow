using System.Threading;

namespace GrpArbFourInDeathRow
{
    partial class Program
    {
        static void Mains(string[] args)
        {
            Client myClient = new Client();

            Thread clientThread = new Thread(myClient.Start);
            clientThread.Start();
            clientThread.Join();
        }
    }
}
