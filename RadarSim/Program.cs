using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RadarSim
{
    class Program
    {
        public static void SendUDP(string message, IPEndPoint destination, Socket s)
        {
            byte[] sendbuf = Encoding.ASCII.GetBytes(message);
            s.SendTo(sendbuf, destination);
            Console.WriteLine(message);
        }
        static void Main(string[] args)
        {
            string destIP = "192.168.2.53";  // 192.168.2.53
            int destPort = 5000;
            int delay = 75; // delay in ms

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint destination = new IPEndPoint(IPAddress.Parse(destIP), destPort);

            // looping
            int minDeg = 1;
            int maxDeg = 89;
            int degrees = minDeg;
            int direction = 1;
            float distance = 0;

            while(true)
            {
                // Simulate the angle
                degrees += direction;
                if (degrees > maxDeg || degrees < minDeg)
                {
                    direction *= -1;
                }

                // Simulate any targets
                // target at 40-50 degrees
                distance = 0;
                if (degrees > 39 && degrees < 44)
                    distance = 1.75f;     
                SendUDP(degrees.ToString() + ", " + distance.ToString(), destination, s);
                Thread.Sleep(delay);
            }
        }
    }
}
