using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RTBPlugins
{
    /// <summary>
    /// Helps keep track of plugins that are loaded during initialisation.
    /// </summary>
    public class PlugIn
    {
        /// <summary>
        /// The DLL filename (without the full path).
        /// </summary>
        public string Filename;
        /// <summary>
        /// A short description of the plugin.
        /// </summary>
        public string Description;
        /// <summary>
        /// List of Target Games that can be supported.
        /// </summary>
        public List<GameEngines> SupportedEngines = new List<GameEngines>();
    }

    /// <summary>
    /// RTB uses this to load a list of plugins during intialisation.
    /// </summary>
    public class Helper
    {
        public static void GetPlugins(string pluginFolder, List<PlugIn> pluginImages, List<PlugIn> pluginHeights)
        {
            if (!Directory.Exists(pluginFolder)) return; // No folder exists.

            string mapsFolder = Path.Combine(pluginFolder, "ImageMap");
            DirectoryInfo d = new DirectoryInfo(mapsFolder);
            FileInfo[] Files = d.GetFiles("*.dll"); // Get all dll files.
            foreach (FileInfo file in Files)
            {
                try
                {
                    string filename = Path.Combine(mapsFolder, file.Name);
                    var plugin = (IPluginImage)Helper.Load(filename);
                    pluginImages.Add(new PlugIn() { Filename = file.Name, Description = plugin.Description, SupportedEngines = plugin.GetSupportedEngines() });
                }
                catch (Exception ex)
                {
                    // do nothing.
                }
            }

            string heightFolder = Path.Combine(pluginFolder, "Height");
            d = new DirectoryInfo(heightFolder);
            Files = d.GetFiles("*.dll"); // Get all dll files.
            foreach (FileInfo file in Files)
            {
                try
                {
                    string filename = Path.Combine(heightFolder, file.Name);
                    var plugin = (IPluginHeight)Helper.Load(filename);
                    pluginHeights.Add(new PlugIn() { Filename = file.Name, Description = plugin.Description, SupportedEngines = plugin.GetSupportedEngines() });
                }
                catch (Exception ex)
                {
                    // do nothing.
                }
            }

            // Sort them.
            pluginImages = pluginImages.OrderBy(a => a.Description).ToList();
            pluginHeights = pluginHeights.OrderBy(a => a.Description).ToList();
        }

        /// <summary>
        /// Loads a plugin.
        /// </summary>
        /// <param name="dllFile"></param>
        /// <returns></returns>
        public static IPlugin Load(string dllFile)
        {
            AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
            Assembly assembly = Assembly.Load(an);

            Type pluginType = typeof(IPlugin);
            Type[] types = assembly.GetTypes();
            List<Type> pluginTypes = new List<Type>();

            foreach (Type type in types)
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    continue;
                }
                else
                {
                    if (type.GetInterface(pluginType.FullName) != null)
                    {
                        pluginTypes.Add(type);
                    }
                }
            }
            
            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginTypes[0]);
            return plugin;
        }
    }
}
