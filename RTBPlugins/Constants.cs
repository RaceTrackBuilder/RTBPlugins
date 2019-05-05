using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTBPlugins
{
    /// <summary>
    /// Games currently supported by RTB.
    /// </summary>
    public enum GameEngines
    {
        None = 0,
        AssettoCorsa = 1,
        rFactor = 2,
        All = 9999
    }

    /// <summary>
    /// Input method for obtaining heights by IPluginHeight plugins. RTB will pass in latitude/longitude coordinates or X/Z values (in meters).
    /// </summary>
    public enum InputMethods
    {
        /// <summary>
        /// Your plugin expects and will receive latitude/longitude coordinates.
        /// </summary>
        LatitudeLongitude,
        /// <summary>
        /// Your plugin expects and will receive X/Z values (in meters).
        /// </summary>
        MetersXZ
    }

    /// <summary>
    /// A value to return if the height is invalid or cannot be obtained for some reason.
    /// </summary>
    public class Constants
    {
        public const float HEIGHT_UNKNOWN = -10000;
    }
}
