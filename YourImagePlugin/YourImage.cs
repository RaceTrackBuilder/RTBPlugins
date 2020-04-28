using RTBPlugins;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace YourImagePlugin
{
    public class YourImage : IPluginImage
    {
        public bool IsFetchingData { get; set; } = false;
        internal static bool Cancel = false;

        /// <summary>
        /// This plugin will not modify the Assetto Corsa Mask.
        /// </summary>
        public bool CreatesAssettoCorsaMask { get { return false; } }
        /// <summary>
        /// This plugin expects the Assetto Corsa Mask to be white.
        /// </summary>
        public bool UseWhiteMaskTexture { get { return true; } }

        public string Description
        {
            get { return "Rainbow Image API"; }
        }
        public string About
        {
            get { return "Create a rainbow image."; }
        }

        public void Initialize()
        {
            // Nothing required.
        }

        public bool UsesLatitudeLongitude { get { return false; } }
        public double Latitude { get { return 0; } }
        public double Longitude { get { return 0; } }
        public bool SupportsInterruptions { get { return false; } }

        public void SizeUpdated(int width, int height)
        {
            // Do nothing.
        }

        public void SetCoverage(ref ImageMapInformation map, double desiredWidth, double desiredHeight)
        {
            map.CoverageX = Math.Max(desiredWidth, desiredHeight);
            map.CoverageZ = map.CoverageX;
        }

        public void Create(string xpacksFolder, ImageMapInformation map, UpdateCallback updateCallback, CompletedCallback completedCallback)
        {
            IsFetchingData = true;
            Cancel = false;

            try
            {
                // Create the Main texture.
                CreateTexture(xpacksFolder, map);
                if (!Cancel) completedCallback?.Invoke(true);
            }
            catch
            {
                completedCallback?.Invoke(false); // Report in that something went wrong.
            }

            IsFetchingData = false;
        }

        public void Resume(string xpacksFolder, UpdateCallback updateCallback, CompletedCallback completedCallback)
        {
            // Interrupting is not supported so nothing to do here.
        }

        private void CreateTexture(string xpacksFolder, ImageMapInformation map)
        {
            SharpDX.Direct3D11.Device DX11_Device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.None);
            string filename = "", filenameIntermediate = "";
            
            int width, height;

            filename = Path.Combine(xpacksFolder, map.Filename);
            filenameIntermediate = Path.Combine(xpacksFolder, map.FilenameIntermediate);
            width = map.Width;
            height = map.Height;

            // Create the file to the destination folder.
            Texture2DDescription textureDesc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R8G8B8A8_UNorm,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Dynamic,
                BindFlags = BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None
            };
            var tex = new Texture2D(DX11_Device, textureDesc);

            double centrex = width * 0.5;
            byte[] buffer = new byte[4 * width * height];
            int bufferoffset = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double dx = Math.Abs(centrex - x) / centrex * 1.1;
                    double dy = 1.2 - (y / (double)height * 1.1);
                    double radius = Math.Sqrt(dx * dx + dy * dy);
                    double rainbow = radius * 2.0 - 1.0;
                    if (rainbow < 0 || rainbow > 1)
                        buffer[bufferoffset] = (int)0;
                    else
                    {
                        var color = HSL2RGB(rainbow, 0.5, 0.5);
                        buffer[bufferoffset] = color.R;
                        buffer[bufferoffset + 1] = color.G;
                        buffer[bufferoffset + 2] = color.B;
                        buffer[bufferoffset + 3] = color.A;
                    }

                    bufferoffset += 4;
                }
            }

            // Copy the Buffer.
            DataStream ds;
            DataBox db = DX11_Device.ImmediateContext.MapSubresource(tex, 0, 0, MapMode.WriteDiscard, SharpDX.Direct3D11.MapFlags.None, out ds);
            Marshal.Copy(buffer, 0, db.DataPointer, buffer.Length); // Write with Marshal as it is quicker than DataStream.
            DX11_Device.ImmediateContext.UnmapSubresource(tex, 0);

            // Save the image.
            ImageFileFormat iff = filename.ToLower().EndsWith(".jpg") ? ImageFileFormat.Jpg : ImageFileFormat.Png;
            string folder = Path.GetDirectoryName(filename);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder); // Create the folder if it doesn't exist.
            Texture2D.ToFile(DX11_Device.ImmediateContext, tex, iff, filename);

            // Dispose.
            DX11_Device.Dispose(); DX11_Device = null;
        }

        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;

            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            Color color = new Color();
            color.R = Convert.ToByte(r * 255.0f);
            color.G = Convert.ToByte(g * 255.0f);
            color.B = Convert.ToByte(b * 255.0f);
            color.A = 1;
            return color;
        }

        public bool ValidateNewProjectSettings(out string errorMessage)
        {
            errorMessage = ""; // Valid
            return true;
        }

        public bool ValidateProjectSettings(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        public void RenderNewProjectSettings(Panel panel, SizeUpdatedCallback suc)
        {
            // do nothing.
        }

        public void RenderProjectSettings(Panel panel)
        {
            // do nothing.
        }

        public void AcceptNewProjectSettings()
        {
            // Nothing to do.
        }

        public void AcceptProjectSettings()
        {
            // Nothing to do.
        }

        public void TargetUpdated(GameEngines target)
        {
            // do nothing.
        }

        public List<GameEngines> GetSupportedEngines()
        {
            List<GameEngines> support = new List<GameEngines>();
            support.Add(GameEngines.All);
            return support;
        }

        public void Stop()
        {
            Cancel = true;
            while (IsFetchingData)
                Thread.Sleep(50);
        }

        public void Save(string filename, bool exit)
        {
            // do nothing.
        }

        public void Load(string filename)
        {
            // do nothing.
        }

    }
}
