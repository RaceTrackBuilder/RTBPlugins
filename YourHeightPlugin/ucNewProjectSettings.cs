using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YourHeightPlugin
{
    public partial class ucNewProjectSettings : UserControl
    {
        internal float HeightMultiplier
        {
            get { return 100f * trkHeightMultiplier.Value; }
        }

        public ucNewProjectSettings()
        {
            InitializeComponent();
            try
            {
                // Load the setting from last time.
                trkHeightMultiplier.Value = Properties.Settings.Default.HeightMultiplier;
            }
            catch (Exception ex)
            {
                // Do nothing.
            }
        }

        internal void AcceptNewProjectSettings()
        {
            // Keep the setting for next time a New Project is created.
            Properties.Settings.Default.HeightMultiplier = trkHeightMultiplier.Value;
        }
    }
}
