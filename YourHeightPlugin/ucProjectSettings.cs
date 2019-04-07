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
    public partial class ucProjectSettings : UserControl
    {
        internal float HeightMultiplier
        {
            get { return 100f * trkHeightMultiplier.Value; }
            set { trkHeightMultiplier.Value = (int)(value * 0.01f); }
        }

        public ucProjectSettings()
        {
            InitializeComponent();
        }
    }
}
