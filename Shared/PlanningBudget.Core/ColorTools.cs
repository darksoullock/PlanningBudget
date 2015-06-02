using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PlanningBudget.Core
{
    public class ColorTools
    {
        public static uint ColorToArgb(Color color)
        {

            return (uint)((color.A << 24) |
                            (color.R << 16) |
                            (color.G << 8) |
                            (color.B));
        }

        public static Color ColorFromArgb(uint color)
        {

            return Color.FromArgb(
                //(byte)((color >> 24) & 0xff),
                (byte)0xff,    // trasparent colors aren't available
                (byte)((color >> 16) & 0xff),
                (byte)((color >> 8) & 0xff),
                (byte)((color) & 0xff));
        }

        public static List<Color> GetColorsList()
        {
            var Colours = new List<Color>();
            for (uint i = 10000; i < 0x00ffffff; i += 454321)
            {
                Colours.Add(ColorTools.ColorFromArgb(i));
            }
            return Colours;
        }
    }
}
