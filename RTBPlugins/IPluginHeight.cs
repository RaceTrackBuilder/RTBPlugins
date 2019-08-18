using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTBPlugins
{
    public interface IPluginHeight : IPlugin
    {
        /// <summary>
        /// Are the Coordinates using Latitude and Logitude or are they local values in Meters.
        /// </summary>
        InputMethods InputMethod { get; }

        /// <summary>
        /// Time in milliseconds to wait if a request was successful. This is used to prevent the service from making requests too rapidly.
        /// </summary>
        int TIMER_WAIT_SUCCESS { get; }

        /// <summary>
        /// Time in milliseconds to wait if a request failed. This is used to prevent the service from constantly making requests that fail.
        /// </summary>
        int TIMER_WAIT_FAILED { get; }

        /// <summary>
        /// Add controls to a panel that is displayed on the RTB New Venue screen.
        /// This is for settings that are available for New projects and may include some settings that are only available at the start of the project.
        /// </summary>
        /// <param name="panel"></param>
        void RenderNewProjectSettings(Panel panel);

        /// <summary>
        /// Get the height at a specified latitude/longitude or x/z coordinate.
        /// </summary>
        /// <param name="latitude_or_z"></param>
        /// <param name="longitude_or_x"></param>
        /// <returns></returns>
        double Fetch(double latitude_or_z, double longitude_or_x);

        /// <summary>
        /// The number of Coordinate Pairs to be sent to the Fetch(List<LatLong> latitude_longitude_pairs) function.
        /// Larger numbers will update the terrain less frequently, but with more points changing. Smaller numbers will have the terrain change more often, but with less points being moved.
        /// More importantly, RTB Google Heights uses this value to group the Google Height calls more efficiently. Google prefers fewer calls with more points and this also reduces impact towards Quotas.
        /// </summary>
        int MaximumPairCount { get; }

        /// <summary>
        /// Get the heights at a number (determined by MaximumPairCount) of latitude/longitude or x/z coordinate pairs.
        /// </summary>
        /// <param name="latitude_longitude_pairs"></param>
        /// <returns></returns>
        List<double> Fetch(List<LatLong> latitude_longitude_pairs);

        /// <summary>
        /// Save Venue specific settings. These settings are specific to the current project.
        /// </summary>
        /// <param name="filename">Height plugins it will be stored in RTBProject\Plugins\Height\[YourPluginName].bin.</param>
        void Save(string filename);
    }

    /// <summary>
    /// Holds the coordinates in either latitude/longitude format or X/Z format depending on the InputMethod.
    /// </summary>
    public class LatLong
    {
        public double latitude_or_z;
        public double longitude_or_x;
    }

    
}
