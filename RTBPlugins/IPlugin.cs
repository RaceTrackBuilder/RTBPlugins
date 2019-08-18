using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RTBPlugins
{
    /// <summary>
    /// Base interface from which all other Plugins will be derived.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Very brief description that will be displayed in RTB's Dropdown listboxes on the New Venue and Edit Venue screens.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Information about your plugin; maybe put your name and website here. I don't currently use this but it might in future be displayed somewhere in an attempt to boost ones ego, profile whatever.
        /// </summary>
        string About { get; }


        // Note: The rendering for New Projects is done differently for the two plugin types because the Image plugin needs to respond back to RTB when the Plugin has changed the size of the new Venue.
        // IPluginImage as  --> void RenderNewProjectSettings(Panel panel, SizeUpdatedCallback sizeUpdatedCallback);
        // IPluginHeight as --> void RenderNewProjectSettings(Panel panel);


        /// <summary>
        /// Display your controls on a panel to be displayed when the user edits an existing Venue.
        /// Only items that can be changed after the Venue is created should be displayed.
        /// </summary>
        /// <param name="panel">The Winforms panel onto which your control(s) will be placed.</param>
        void RenderProjectSettings(Panel panel);

        /// <summary>
        /// Validate the settings on the New Project window.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool ValidateNewProjectSettings(out string errorMessage);

        /// <summary>
        /// Validate the settings on the New Project window.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        bool ValidateProjectSettings(out string errorMessage);

        /// <summary>
        /// Called when the user clicks OK on the New Venue window, right before RTB creates the new Venue object. 
        /// Any settings on the New Venue plugin panel should be stored and used in your plugin.
        /// Perform one-off actions (such as loading data).
        /// </summary>
        void AcceptNewProjectSettings();

        /// <summary>
        /// Called when the user clicks OK on the Edit Venue window. 
        /// Any settings on the Edit Venue plugin panel should be stored and used in your plugin.
        /// </summary>
        void AcceptProjectSettings();

        /// <summary>
        /// For transferring the current API stored in RTB into the plugin. This will be deprecated in a future release. For now just create an empty function.
        /// </summary>
        void TransferGoogleAPI(string googleAPI);

        /// <summary>
        /// List of Game Engines that this plugin will support.
        /// </summary>
        /// <returns></returns>
        List<GameEngines> GetSupportedEngines();

        /// <summary>
        /// Load Venue specific settings from your own binary file. These settings are specific to the current project and will be stored inside a file named "[YourPluginName].bin".
        /// ALSO Perform one-off actions here (such as loading data or initialise variables).
        /// </summary>
        /// <param name="filename">Image plugins will be stored in RTBProject\Plugins\Image\[YourPluginName].bin. Height plugins it will be stored in RTBProject\Plugins\Height\[YourPluginName].bin.</param>
        void Load(string filename);
    }
}
