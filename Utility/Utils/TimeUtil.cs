///====================================================================================================
///
///     TimeUtil by
///     - CantyCanadian
///     
///====================================================================================================

using System;

namespace Canty
{
    public static class TimeUtil
    {
        /// <summary>
        /// Epoch registered when the game starts (0 = Jan 1st 1970)
        /// </summary>
        public static TimeSpan GameStartEpoch = CurrentEpoch();

        public static TimeSpan CurrentEpoch()
        {
            return DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gives an int containing how many milliseconds passed since the game started.
        /// </summary>
        /// <returns>How many milliseconds.</returns>
        public static double MillisecondsSinceStartOfSoftware()
        {
            TimeSpan epoch = CurrentEpoch();
            return (epoch - GameStartEpoch).TotalMinutes * 60000 + (epoch - GameStartEpoch).TotalSeconds * 1000 + (epoch - GameStartEpoch).TotalMilliseconds;
        }
    }
}