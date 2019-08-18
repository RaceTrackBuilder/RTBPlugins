using System;
using System.Collections.Generic;
using System.IO;
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
    /// Call this now and again if your capture takes a long time in order to provide progress during the capturing of images. 
    /// RTB Google Maps will call this every now and again to provide an updated image that will be refreshed.
    /// </summary>
    /// <param name="imageType"></param>
    /// <param name="percentage"></param>
    public delegate void UpdateCallback(ImageType imageType, float percentage);

    /// <summary>
    /// Call this one the whole process is complete, or if it has failed.
    /// </summary>
    /// <param name="imageType"></param>
    /// <param name="success">True if successful</param>
    public delegate void CompletedCallback(bool success);

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
        /// <param name="xpacksFolder">Xpacks folder where images are contained.</param>
        /// <param name="map">Parameters that define the required image.</param>
        /// <param name="updateCallback">Callback function to show partial image as it is slowly being gathered. No need to do anything here if your plugin is fast.</param>
        /// <param name="completedCallback">Callback function when image is complete.</param>
        void Create(string xpacksFolder, ImageMapInformation map, UpdateCallback updateCallback, CompletedCallback completedCallback);

        /// <summary>
        /// Continue obtaining the image using the parameters passed in the map variable.
        /// This function is called when loading an existing project that was saved before the background image had fully been retrieved.
        /// Only needs to do something if you are supporting here interruptions.
        /// </summary>
        /// <param name="xpacksFolder">Xpacks folder where images are contained.</param>
        /// <param name="map">Parameters that define the required image.</param>
        /// <param name="updateCallback">Callback function to show partial image as it is slowly being gathered. No need to do anything here if your plugin is fast.</param>
        /// <param name="completedCallback">Callback function when image is complete.</param>
        void Resume(string xpacksFolder, UpdateCallback updateCallback, CompletedCallback completedCallback);

        /// <summary>
        /// The user has changed the size of the Image they want to create. You may want to respond by updating some text on the settings panel.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SizeUpdated(int width, int height);

        /// <summary>
        /// The plugin requires and uses a longitude and latitude.
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
        /// The plugin can Save a partial image then later the RTB project can be loaded and the obtaining of this image can be continued.
        /// This is useful when the obtaining of an image is a lengthy process (like getting 1,000s of images from google). The user may wish to Save, exit RTB, and then continue at a later point.
        /// Setting this to true indicates that your plugin will support this feature.
        /// </summary>
        bool SupportsInterruptions { get; }

        /// <summary>
        /// Set the ImageMap's CoverageX and CoverageZ in this function. Coverage is how many meters the image represents across the width (CoverageX) and height (CoverageZ).
        /// Users enter a desired width/height on RTB's front end when they create a new Venue, however the image needs to cover at least this amount.
        /// For example: The RTB Google Maps plugin makes an image which is slightly bigger than the size of the RTB Terrain mesh, so the user can add a little more terrain on each of the sides.
        /// This gets called before RTB makes a Material (on which the image will be a texture).
        /// </summary>
        /// <param name="map">Information about the image</param>
        /// <param name="desiredWidth">The minimum width that needs to be covered.</param>
        /// <param name="desiredHeight">The minimum height that needs to be covered.</param>
        void SetCoverage(ref ImageMapInformation map, double desiredWidth, double desiredHeight);

        /// <summary>
        /// When the user targets a specific engine, this is called since you may wish to support different engines in various ways.
        /// </summary>
        /// <param name="target"></param>
        void TargetUpdated(GameEngines target);

        /// <summary>
        /// Save Venue specific settings. These settings are specific to the current project.
        /// </summary>
        /// <param name="filename">Image plugins will be stored in RTBProject\Plugins\Image\[YourPluginName].bin.</param>
        /// <param name="exit">The Save is occuring right before exiting the Venue so save an partial image that can be recommenced when the Venue is next loaded.</param>
        void Save(string filename, bool exit);

        /// <summary>
        /// The user wants to exit or start a new Venue, so cancel the image processing.
        /// RTB will not resume anything until this function completes so make sure you can respond to this quickly.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Contains all the information required to create an Image.
    /// </summary>
    public class ImageMapInformation
    {
        /// <summary>
        /// Folder to the Xpacks.
        /// </summary>
        public string XPacksFolder;
        /// <summary>
        /// Path (relative to XpacksFolder) to the file that is to be created for the Main image.
        /// </summary>
        public string Filename;
        /// <summary>
        /// Path (relative to XpacksFolder) to the intermediate file that could be occasionally updated to provide the user with an "in-progress" view.
        /// Note that this MUST be in jpg format.
        /// </summary>
        public string FilenameIntermediate;
        /// <summary>
        /// Path (relative to XpacksFolder) to the file that is to be created for the Mask image.
        /// </summary>
        public string FilenameMask;
        /// <summary>
        /// Path (relative to XpacksFolder) to the intermediate file for the Mask that could be occasionally updated to provide the user with an "in-progress" view.
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

        public void Write(BinaryWriter bw)
        {
            bw.Write(Filename);
            bw.Write(FilenameIntermediate);
            bw.Write(FilenameMask);
            bw.Write(FilenameIntermediateMask);
            bw.Write(Latitude);
            bw.Write(Longitude);
            bw.Write(CoverageX);
            bw.Write(CoverageZ);
            bw.Write(Width);
            bw.Write(Height);
            bw.Write(WidthMask);
            bw.Write(HeightMask);
        }

        public static ImageMapInformation Read(BinaryReader br)
        {
            ImageMapInformation imi = new ImageMapInformation();
            imi.Filename = br.ReadString();
            imi.FilenameIntermediate = br.ReadString();
            imi.FilenameMask = br.ReadString();
            imi.FilenameIntermediateMask = br.ReadString();
            imi.Latitude = br.ReadDouble();
            imi.Longitude = br.ReadDouble();
            imi.CoverageX = br.ReadDouble();
            imi.CoverageZ = br.ReadDouble();
            imi.Width = br.ReadInt32();
            imi.Height = br.ReadInt32();
            imi.WidthMask = br.ReadInt32();
            imi.HeightMask = br.ReadInt32();
            return imi;
        }
    }
}
