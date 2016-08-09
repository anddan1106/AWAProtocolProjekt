using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    class Program
    {


        static void Main(string[] args)
        {
            // Create a game and run it on a new thread
            Game game = new Game();
            Thread serverThread = new Thread(game.Run);
            serverThread.Start();

            // Wait for sub-threads to close before closing main-thread (not nessesary bacause serverThread is a Foreground-thread)
            serverThread.Join();

        }

        

    }
}
