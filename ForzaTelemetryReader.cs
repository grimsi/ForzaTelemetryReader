using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ForzaTelemetryReader.Structs;

namespace MicrosoftGames.Forza
{
    public class TelemetryReader
    {
        public TelemetryReader(int port)
        {
            _port = port;
        }

        private readonly int _port;
        private Thread _listenerThread;
        private bool _stopThread;

        public TelemetryData TelemetryData { get; private set; } = new();

        public void StartListener()
        {
            if (_listenerThread != null && _listenerThread.IsAlive)
            {
                Console.WriteLine("Error: Listener already started!");
                return;
            }
            
            var progress = new Progress<TelemetryData>();

            progress.ProgressChanged += (_, telemetryPacket) =>
            {
                TelemetryData = telemetryPacket;
            };

            _listenerThread = new Thread(() => ListenForData(progress)) { IsBackground = true };
            _stopThread = false;
            _listenerThread.Start();
        }

        public void StopListener()
        {
            _stopThread = true;
        }
        
        private void ListenForData(IProgress<TelemetryData> progress)
        {
            var udpClient = new UdpClient(_port);
            var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, _port);
            
            while(!_stopThread)
            {
                try
                {
                    var rawTelemetryData = udpClient.Receive(ref remoteIpEndPoint);
                    progress.Report(new TelemetryData(rawTelemetryData));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            udpClient.Close();
        }
    }
}