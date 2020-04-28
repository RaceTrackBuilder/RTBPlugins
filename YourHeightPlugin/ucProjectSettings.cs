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
            get
            {
                float f = 100f * trkHeightMultiplier.Value;
                //MessageBox.Show(String.Format("ucProjectSettings.HeightMultiplier get {0}\t{1}", f, trkHeightMultiplier.Value));
                return 100f * trkHeightMultiplier.Value;
            }
            set
            {
                int i = (int)(value * 0.01f);
                trkHeightMultiplier.Value = (int)(value * 0.01f);
                //MessageBox.Show(String.Format("ucProjectSettings.HeightMultiplier set {0}\t{1}\t{2}", value, i, trkHeightMultiplier.Value));
            }
        }

        public ucProjectSettings()
        {
            InitializeComponent();
        }

        private void trkHeightMultiplier_Scroll(object sender, EventArgs e)
        {
            //MessageBox.Show(String.Format("trkHeightMultiplier_Scroll() {0}", trkHeightMultiplier.Value));
        }
    }
}
