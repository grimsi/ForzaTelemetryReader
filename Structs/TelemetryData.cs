using System;
using System.IO;
using ForzaTelemetryReader.Enums;

namespace ForzaTelemetryReader.Structs
{
    public class TelemetryData
    {
        public TelemetryData(byte[] rawTelemetryPacket)
        {
            GameTitle = GetGameTitleFromTelemetryPacket(rawTelemetryPacket);

            NormalizeTelemetryData(ref rawTelemetryPacket);

            using MemoryStream inputStream = new MemoryStream(rawTelemetryPacket);
            using BinaryReader reader = new BinaryReader(inputStream);
            IsRaceOn = reader.ReadInt32() != 0; // convert original s32 into bool
                
            TimestampMS = reader.ReadUInt32();
                
            EngineMaxRpm = reader.ReadSingle();
            EngineIdleRpm = reader.ReadSingle();
            CurrentEngineRpm = reader.ReadSingle();
                
            AccelerationX = reader.ReadSingle();
            AccelerationY = reader.ReadSingle();
            AccelerationZ = reader.ReadSingle();
                
            VelocityX = reader.ReadSingle();
            VelocityY = reader.ReadSingle();
            VelocityZ = reader.ReadSingle();
                
            AngularVelocityX = reader.ReadSingle();
            AngularVelocityY = reader.ReadSingle();
            AngularVelocityZ = reader.ReadSingle();
                
            Yaw = reader.ReadSingle();
            Pitch = reader.ReadSingle();
            Roll = reader.ReadSingle();
                
            NormalizedSuspensionTravelFrontLeft = reader.ReadSingle();
            NormalizedSuspensionTravelFrontRight = reader.ReadSingle();
            NormalizedSuspensionTravelRearLeft = reader.ReadSingle();
            NormalizedSuspensionTravelRearRight  = reader.ReadSingle();
                
            TireSlipRatioFrontLeft = reader.ReadSingle();
            TireSlipRatioFrontRight = reader.ReadSingle();
            TireSlipRatioRearLeft = reader.ReadSingle();
            TireSlipRatioRearRight = reader.ReadSingle();
                
            WheelRotationSpeedFrontLeft = reader.ReadSingle();
            WheelRotationSpeedFrontRight = reader.ReadSingle();
            WheelRotationSpeedRearLeft = reader.ReadSingle();
            WheelRotationSpeedRearRight = reader.ReadSingle();

            WheelOnRumbleStripFrontLeft = reader.ReadInt32();
            WheelOnRumbleStripFrontRight = reader.ReadInt32();
            WheelOnRumbleStripRearLeft = reader.ReadInt32();
            WheelOnRumbleStripRearRight = reader.ReadInt32();
                
            WheelInPuddleDepthFrontLeft = reader.ReadSingle();
            WheelInPuddleDepthFrontRight = reader.ReadSingle();
            WheelInPuddleDepthRearLeft = reader.ReadSingle();
            WheelInPuddleDepthRearRight = reader.ReadSingle();
                
            SurfaceRumbleFrontLeft = reader.ReadSingle();
            SurfaceRumbleFrontRight = reader.ReadSingle();
            SurfaceRumbleRearLeft = reader.ReadSingle();
            SurfaceRumbleRearRight = reader.ReadSingle();
                
            TireSlipAngleFrontLeft = reader.ReadSingle();
            TireSlipAngleFrontRight = reader.ReadSingle();
            TireSlipAngleRearLeft = reader.ReadSingle();
            TireSlipAngleRearRight = reader.ReadSingle();
                
            TireCombinedSlipFrontLeft = reader.ReadSingle();
            TireCombinedSlipFrontRight = reader.ReadSingle();
            TireCombinedSlipRearLeft = reader.ReadSingle();
            TireCombinedSlipRearRight = reader.ReadSingle();
                
            SuspensionTravelMetersFrontLeft = reader.ReadSingle();
            SuspensionTravelMetersFrontRight = reader.ReadSingle();
            SuspensionTravelMetersRearLeft = reader.ReadSingle();
            SuspensionTravelMetersRearRight = reader.ReadSingle();

            CarOrdinal = reader.ReadInt32();
            CarClass = reader.ReadInt32();
            CarPerformanceIndex = reader.ReadInt32();
            DrivetrainType = reader.ReadInt32();
            NumCylinders = reader.ReadInt32();

            // Only Forza Motorsport 7 in Dash Mode and Forza Horizon 4 contain the following information
            if (GameTitle != GameTitle.ForzaHorizon4 && GameTitle != GameTitle.ForzaMotorsport7DashMode) return;
            
            PositionX = reader.ReadSingle();
            PositionY = reader.ReadSingle();
            PositionZ = reader.ReadSingle();
                
            Speed = reader.ReadSingle();
            Power = reader.ReadSingle();
            Torque = reader.ReadSingle();
                
            TireTempFrontLeft = reader.ReadSingle();
            TireTempFrontRight = reader.ReadSingle();
            TireTempRearLeft = reader.ReadSingle();
            TireTempRearRight = reader.ReadSingle();
                
            Boost = reader.ReadSingle();
            Fuel = reader.ReadSingle();
            DistanceTraveled = reader.ReadSingle();
            BestLap = reader.ReadSingle();
            LastLap = reader.ReadSingle();
            CurrentLap = reader.ReadSingle();
            CurrentRaceTime = reader.ReadSingle();

            LapNumber = reader.ReadUInt16();
            RacePosition = reader.ReadByte();

            Accel = reader.ReadByte();
            Brake = reader.ReadByte();
            Clutch = reader.ReadByte();
            HandBrake = reader.ReadByte();
            Gear = reader.ReadByte();
            Steer = reader.ReadSByte();

            NormalizedDrivingLine = reader.ReadSByte();
            NormalizedAIBrakeDifference = reader.ReadSByte();
        }

        public TelemetryData()
        {
            
        }

        #region Custom helper values
        public GameTitle GameTitle { get; }

        #endregion
        
        #region Parsed telemetry values
        
        public bool IsRaceOn { get; } // = 1 when race is on. = 0 when in menus/race stopped …

        public uint TimestampMS { get; } //Can overflow to 0 eventually

        public float EngineMaxRpm { get; }
        public float EngineIdleRpm { get; }
        public float CurrentEngineRpm { get; }

        public float AccelerationX { get; } //In the car's local space { get; } X = right, Y = up, Z = forward
        public float AccelerationY { get; }
        public float AccelerationZ { get; }

        public float VelocityX { get; } //In the car's local space { get; } X = right, Y = up, Z = forward
        public float VelocityY { get; }
        public float VelocityZ { get; }

        public float AngularVelocityX { get; } //In the car's local space { get; } X = pitch, Y = yaw, Z = roll

        public float AngularVelocityY { get; }
        public float AngularVelocityZ { get; }

        public float Yaw { get; }
        public float Pitch { get; }
        public float Roll { get; }

        public float NormalizedSuspensionTravelFrontLeft { get; } // Suspension travel normalized: 0.0f = max stretch { get; } 1.0 = max compression

        public float NormalizedSuspensionTravelFrontRight { get; }
        public float NormalizedSuspensionTravelRearLeft { get; }
        public float NormalizedSuspensionTravelRearRight { get; }

        public float TireSlipRatioFrontLeft { get; } // Tire normalized slip ratio, = 0 means 100% grip and |ratio| > 1.0 means loss of grip.

        public float TireSlipRatioFrontRight { get; }
        public float TireSlipRatioRearLeft { get; }
        public float TireSlipRatioRearRight { get; }

        public float WheelRotationSpeedFrontLeft { get; } // Wheel rotation speed radians/sec.
        public float WheelRotationSpeedFrontRight { get; }
        public float WheelRotationSpeedRearLeft { get; }
        public float WheelRotationSpeedRearRight { get; }

        public int WheelOnRumbleStripFrontLeft { get; } // = 1 when wheel is on rumble strip, = 0 when off.
        public int WheelOnRumbleStripFrontRight { get; }
        public int WheelOnRumbleStripRearLeft { get; }
        public int WheelOnRumbleStripRearRight { get; }

        public float WheelInPuddleDepthFrontLeft { get; } // = from 0 to 1, where 1 is the deepest puddle
        public float WheelInPuddleDepthFrontRight { get; }
        public float WheelInPuddleDepthRearLeft { get; }
        public float WheelInPuddleDepthRearRight { get; }

        public float SurfaceRumbleFrontLeft { get; } // Non-dimensional surface rumble values passed to controller force feedback

        public float SurfaceRumbleFrontRight { get; }
        public float SurfaceRumbleRearLeft { get; }
        public float SurfaceRumbleRearRight { get; }

        public float TireSlipAngleFrontLeft { get; } // Tire normalized slip angle, = 0 means 100% grip and |angle| > 1.0 means loss of grip.

        public float TireSlipAngleFrontRight { get; }
        public float TireSlipAngleRearLeft { get; }
        public float TireSlipAngleRearRight { get; }

        public float TireCombinedSlipFrontLeft { get; } // Tire normalized combined slip, = 0 means 100% grip and |slip| > 1.0 means loss of grip.

        public float TireCombinedSlipFrontRight { get; }
        public float TireCombinedSlipRearLeft { get; }
        public float TireCombinedSlipRearRight { get; }

        public float SuspensionTravelMetersFrontLeft { get; } // Actual suspension travel in meters
        public float SuspensionTravelMetersFrontRight { get; }
        public float SuspensionTravelMetersRearLeft { get; }
        public float SuspensionTravelMetersRearRight { get; }

        public int CarOrdinal { get; } //Unique ID of the car make/model
        public int CarClass { get; } //Between 0 (D -- worst cars) and 7 (X class -- best cars) inclusive
        public int CarPerformanceIndex { get; } //Between 100 (slowest car) and 999 (fastest car) inclusive
        public int DrivetrainType { get; } //Corresponds to EDrivetrainType { get; } 0 = FWD, 1 = RWD, 2 = AWD
        public int NumCylinders { get; } //Number of cylinders in the engine

        //Position (meters)
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }

        public float Speed { get; } // meters per second
        public float Power { get; } // watts
        public float Torque { get; } // newton meter

        public float TireTempFrontLeft { get; }
        public float TireTempFrontRight { get; }
        public float TireTempRearLeft { get; }
        public float TireTempRearRight { get; }

        public float Boost { get; }
        public float Fuel { get; }
        public float DistanceTraveled { get; }
        public float BestLap { get; }
        public float LastLap { get; }
        public float CurrentLap { get; }
        public float CurrentRaceTime { get; }

        public ushort LapNumber { get; }
        public byte RacePosition { get; }

        public byte Accel { get; }
        public byte Brake { get; }
        public byte Clutch { get; }
        public byte HandBrake { get; }
        public byte Gear { get; }
        public sbyte Steer { get; }

        public sbyte NormalizedDrivingLine { get; }
        public sbyte NormalizedAIBrakeDifference { get; }
        
        #endregion

        #region Helper methods
        
        private void NormalizeTelemetryData(ref Byte[] rawTelemetryData)
        {
            if (GameTitle == GameTitle.ForzaHorizon4)
            {
                byte[] newData = new byte[311];
                Array.Copy(rawTelemetryData, 0, newData, 0, 232);
                Array.Copy(rawTelemetryData, 244, newData, 232, 79);
                rawTelemetryData = newData;
            }
        }

        private GameTitle GetGameTitleFromTelemetryPacket(Byte[] rawTelemetryPacket)
        {
            switch (rawTelemetryPacket.Length)
            { 
                case 232:
                    return GameTitle.ForzaMotorsport7SledMode;
                case 311:
                    return GameTitle.ForzaMotorsport7DashMode;
                case 324:
                    return GameTitle.ForzaHorizon4;
                default:
                    return GameTitle.Unknown;
            }
        }
        
        #endregion
    }
}