using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RTBPlugins
{
    /// <summary>
    /// Type of image being produced by the plugin.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// The main color that is visible on the terrain. This is where the Google Maps Image will be loaded.
        /// </summary>
        Main,
        /// <summary>
        /// For Assetto Corsa this is where the Mask used to blend four different textures is stored.
        /// </summary>
        Mask
    }
    /// <summary>
    /// WARNING: THIS WILL CHANGE
    /// Currently this is used to provide progress during the capturing of images. RTB Google Maps will call this every now and again 
    /// to provide an updated image that will  be refreshed.
    /// I will break this function into two, one for progress and one for Completion. Maybe even one for failure. brpbrp
    /// </summary>
    /// <param name="imageType"></param>
    /// <param name="complete"></param>
    /// <param name="success"></param>
    /// <param name="percentage"></param>
    public delegate void CompletedCallback(ImageType imageType, bool complete, bool success, float percentage);

    /// <summary>
    /// Your Image Plugin may determine a new size for the Venue. If it does, this this callback function is used to upodate the Sliders on the New Venue screen.
    /// eg. RTB's Google Maps allows the user to scroll and pan a simple Map on a popup window. After they click ok the size of the desired Venue has been changed and the Plugin calls this function.
    /// </summary>
    /// <param name="width">Desired width (x) in meters.</param>
    /// <param name="height">Desired height (depth really - z) in meters.</param>
    public delegate void SizeUpdatedCallback(double width, double height);

    /// <summary>
    /// Interface for creating an Image Map plugin.
    /// </summary>
    public interface IPluginImage : IPlugin
    {
        /// <summary>
        /// Set to true if your plugin will create the Assetto Corsa background mask as well as the main texture.
        /// Set to false if your plugin only creates the main texture.
        /// </summary>
        bool CreatesAssettoCorsaMask { get; }

        /// <summary>
        /// Use a White texture as the first Mask Texture.
        /// RTB sets this to True for Google Maps plugin, to False for the Random Color plugin.
        /// </summary>
        bool UseWhiteMaskTexture { get; }

        /// <summary>
        /// Add controls to a panel that is displayed on the RTB New Venue screen.
        /// </summary>
        /// <param name="panel"></param>
        void RenderNewProjectSettings(Panel panel, SizeUpdatedCallback sizeUpdatedCallback);

        /// <summary>
        /// Create an image using the parameters passed in the map variable.
        /// </summary>
        /// <param name="map">Parameters that define the required image.</param>
        /// <param name="completedCallback">Callback function when image is complete.</param>
        void Create(ImageMapInformation map, CompletedCallback completedCallback);

        /// <summary>
        /// The user has changed the size of the Image they want to create. You may want to respond by updating some text on the settings panel.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SizeUpdated(int width, int height);

        /// <summary>
        /// The plugin requires and uses a lognitude and latitude.
        /// </summary>
        bool UsesLatitudeLongitude { get; }
        /// <summary>
        /// Latitude of the centre of the image.
        /// </summary>
        double Latitude { get; }
        /// <summary>
        /// Longitude of the centre of the image.
        /// </summary>
        double Longitude { get; }

        /// <summary>
        /// Set the ImageMap's CoverageX and CoverageZ in this function. Coverage is how many meters the image represents across the width (CoverageX) and height (CoverageZ).
        /// Users enter a desired width/height on RTB's front end when they create a new Venue, however the image needs to cover at least this amount.
        /// For example: The RTB Google Maps plugin makes an image which is slightly bigger than the size of the RTB Terrain mesh, so the user can add a little more terrain on each of the sides.
        /// This gets called before RTB makes a Material (on which the image will be a texture).
        /// </summary>
        void SetCoverage(ref ImageMapInformation map, double desiredWidth, double desiredHeight);

        /// <summary>
        /// When the user targets a specific engine, this is called since you may wish to support different engines in various ways.
        /// </summary>
        /// <param name="target"></param>
        void TargetUpdated(GameEngines target);

        /// <summary>
        /// The user wants to exit or start a new Venue, so cancel the image processing.
        /// RTB will not resume anything until this function completes.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Contains all the information required to create an Image.
    /// </summary>
    public class ImageMapInformation
    {
        /// <summary>
        /// Path to the file that is to be created for the Main image.
        /// </summary>
        public string Filename;
        /// <summary>
        /// Path to the intermediate file that could be occasionally updated to provide the user with an "in-progress" view.
        /// Note that this MUST be in jpg format.
        /// </summary>
        public string FilenameIntermediate;
        /// <summary>
        /// Path to the file that is to be created for the Mask image.
        /// </summary>
        public string FilenameMask;
        /// <summary>
        /// Path to the intermediate file for the Mask that could be occasionally updated to provide the user with an "in-progress" view.
        /// Note that this MUST be in jpg format.
        /// </summary>
        public string FilenameIntermediateMask;
        /// <summary>
        /// Latitude for the center of the image.
        /// </summary>
        public double Latitude;
        /// <summary>
        /// Longitude for the center of the image.
        /// </summary>
        public double Longitude;
        /// <summary>
        /// Meters of coverage in the X axis (along the latitude).
        /// </summary>
        public double CoverageX;
        /// <summary>
        /// Meters of coverage in the Z axis (along the longitude).
        /// </summary>
        public double CoverageZ;
        /// <summary>
        /// Width of the image.
        /// </summary>
        public int Width;
        /// <summary>
        /// Height of the image.
        /// </summary>
        public int Height;
        /// <summary>
        /// Width of the Mask image.
        /// </summary>
        public int WidthMask;
        /// <summary>
        /// Height of the Mask image.
        /// </summary>
        public int HeightMask;
    }
}
