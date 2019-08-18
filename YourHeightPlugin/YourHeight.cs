using RTBPlugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace YourHeightPlugin
{
    public class YourHeight : IPluginHeight
    {
        /// <summary>
        /// User control for displaying the slider on the RTB's New Venue window.
        /// </summary>
        ucNewProjectSettings ucNewProjectSettings = new ucNewProjectSettings();

        /// <summary>
        /// User control for displaying the slider on the RTB's Venue Properties window.
        /// </summary>
        ucProjectSettings ucProjectSettings = new ucProjectSettings();

        /// <summary>
        /// The amount the height will be multiplied by in the GetWaveHeight function.
        /// </summary>
        float HeightMultiplier = 100;

        public InputMethods InputMethod { get { return InputMethods.MetersXZ; } }

        public int TIMER_WAIT_SUCCESS { get { return 0; } }

        public int TIMER_WAIT_FAILED { get { return 0; } }

        public int MaximumPairCount { get { return 1000; } }

        public string Description { get { return "Your Fantastic Height Plugin"; } }

        public string About { get { return "Initiate with semi-random heights."; } }

        public void RenderNewProjectSettings(Panel panel)
        {
            ucNewProjectSettings.Dock = DockStyle.Fill;
            panel.Controls.Add(ucNewProjectSettings);
        }

        public void RenderProjectSettings(Panel panel)
        {
            ucProjectSettings.HeightMultiplier = HeightMultiplier; // Set the height setting based on the current project's value.
            ucProjectSettings.Dock = DockStyle.Fill;
            panel.Controls.Add(ucProjectSettings);
        }

        public bool ValidateNewProjectSettings(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public bool ValidateProjectSettings(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public void AcceptNewProjectSettings()
        {
            // Save the Noise setting.
            ucNewProjectSettings.AcceptNewProjectSettings();
            Properties.Settings.Default.Save();

            HeightMultiplier = ucNewProjectSettings.HeightMultiplier;
        }

        public void AcceptProjectSettings()
        {
            HeightMultiplier = ucProjectSettings.HeightMultiplier;
        }

        public void TransferGoogleAPI(string googleAPI)
        {
            // Nothing to do.
        }

        public double Fetch(double latitude_or_z, double longitude_or_x)
        {
            return GetWaveHeight(latitude_or_z, longitude_or_x);
        }

        public List<double> Fetch(List<LatLong> latitude_longitude_pairs)
        {
            List<double> heights = new List<double>(latitude_longitude_pairs.Count);
            foreach (var ll in latitude_longitude_pairs)
            {
                heights.Add(GetWaveHeight(ll.latitude_or_z, ll.longitude_or_x));
            }
            return heights;
        }

        private double GetWaveHeight(double latitude, double longitude)
        {
            return (Math.Sin(latitude * 0.01) + Math.Sin(longitude * 0.01)) * HeightMultiplier;
        }

        public List<GameEngines> GetSupportedEngines()
        {
            List<GameEngines> support = new List<GameEngines>();
            support.Add(GameEngines.None);
            support.Add(GameEngines.AssettoCorsa);
            support.Add(GameEngines.rFactor);
            return support;
        }

        /// <summary>
        /// This is called when RTB Saves a project. Use it to store values that are specific to this project, that can then be reloaded via the Load() function.
        /// </summary>
        /// <param name="xml"></param>
        public void Save(string filename)
        {
            // Replace existing file if it exists.
            if (File.Exists(filename)) File.Delete(filename);

            // Create new file.
            using (FileStream fs = File.Create(filename))
            {
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.UTF8))
                {
                    bw.Write(HeightMultiplier);
                }
            }
        }

        /// <summary>
        /// This is called when RTB loads an existing project. Use it to load values specific to this project.
        /// </summary>
        /// <param name="xmlNode"></param>
        public void Load(string filename)
        {
            if (!File.Exists(filename)) return;
            using (FileStream fs = File.OpenRead(filename))
            {
                using (BinaryReader br = new BinaryReader(fs, System.Text.Encoding.UTF8))
                {
                    HeightMultiplier = br.ReadSingle();
                }
            }
        }
    }
}
