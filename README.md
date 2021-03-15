[![Nuget](https://img.shields.io/nuget/v/ForzaTelemetryReader)](https://www.nuget.org/packages/ForzaTelemetryReader/) [![Nuget](https://img.shields.io/nuget/dt/ForzaTelemetryReader)](https://www.nuget.org/packages/ForzaTelemetryReader/)

# ForzaTelemetryReader
Library for reading and parsing live telemetry data from Forza video games.
Currently Forza Motorsport 7 (in "Sled" and "Dash" mode) and Forza Horizon 4 supported.

## Functionality

Both Forza Motorsport 7 and Forza Horizon 4 have a feature called "Data out" that can be enabled via the in-game settings.
Enabling this feature will make the game send UDP packets with live telemetry data to a given IP and Port.
The game will send a telemetry packet every frame. So if you have the game running at 60fps, the TelemetryReader will receive 60 packets per second.

_Hint: According to the official documentation it is not possible to stream data to the machine the game is running on.
This not correct, although you have to manually execute a command in order to enable localhost streaming. For more details, see ["Installation"](#installation)._

[Official documentation from the Forza Motorsport 7 developers ](https://forums.forzamotorsport.net/turn10_postsm926839_Forza-Motorsport-7--Data-Out--feature-details.aspx#post_926839).

## Installation

This library is published on NuGet as [ForzaTelemetryReader](https://www.nuget.org/packages/ForzaTelemetryReader/). You can simply install it just as any other NuGet package.

### Optional: Enable localhost streaming
As mentioned above, the streaming of telemetry data to the machine the game is running on is not officially supported due to restrictions in UWP.
These restrictions can however be manually disabled. Simply run the following commands in powershell with admin privileges (<kbd>Win</kbd> + <kbd>X</kbd> → _Windows Powershell (Administrator)_):

Forza Motorsport 7:
```powershell
CheckNetIsolation.exe LoopbackExempt -a -n="Microsoft.ApolloBaseGame_8wekyb3d8bbwe"

```

Forza Horizon 4:
```powershell
CheckNetIsolation.exe LoopbackExempt -a -n="Microsoft.SunriseBaseGame_8wekyb3d8bbwe"
```

## Usage

This library is easy to integrate in just a few lines of code.
The telemetry client is running in its own background thread, so you don't have to worry about blocking anything else.

This is a minimal usage example:
```c#
using System;
using System.Threading;
using MicrosoftGames.Forza;

namespace Forza_Telemetry_Test
{
    class Program
    {
        private static void Main()
        {
            // the port defined in the in-game settings
            const int port = 5685;

            // initialize a new TelemetryReader with the port
            var telemetryReader = new TelemetryReader(port);

            // start listening to telemetry packages
            telemetryReader.StartListener();

            // listen for one minute
            for(int i = 0; i < 60; i++)
            {
                // print some example data to the console
                Console.SetCursorPosition(0,0);
                Console.WriteLine($"Game: {telemetryReader.TelemetryData.GameTitle}");
                Console.WriteLine($"Timestamp: {telemetryReader.TelemetryData.TimestampMS}");
                Console.WriteLine($"Is in Race: {telemetryReader.TelemetryData.IsRaceOn}");
                Console.WriteLine($"RPM: {(int) telemetryReader.TelemetryData.CurrentEngineRpm}"); // cast from float to int
                Console.WriteLine($"KMH: {(int) (telemetryReader.TelemetryData.Speed * 3.6)}");    // Speed is in m/s, so multiply with 3.6 to get km/h
                Console.WriteLine($"Gear: {telemetryReader.TelemetryData.Gear}");
                
                // the listener wont be affected since it is running in a background thread
                Thread.Sleep(1000);
            }
            
            // stop listening and unblock the port
            telemetryReader.StopListener();
        }
    }
}
```
