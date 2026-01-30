using System.Drawing;
using System.Windows.Forms;

namespace Klocman.Forms.Tools
{
    public class DarkColorTable : ProfessionalColorTable
    {
        private static Color Background => ThemeController.Palette.DarkBackground; 
        private static Color Border => ThemeController.Palette.DarkBorder;
        private static Color Selected => ThemeController.Palette.DarkControlBackground;
        private static Color Hover => ThemeController.Palette.DarkControlBackground;
        private static Color Pressed => ThemeController.Palette.DarkAccent;

        public override Color ToolStripGradientBegin => Background;
        public override Color ToolStripGradientMiddle => Background;
        public override Color ToolStripGradientEnd => Background;
        public override Color MenuStripGradientBegin => Background;
        public override Color MenuStripGradientEnd => Background;
        public override Color StatusStripGradientBegin => Background;
        public override Color StatusStripGradientEnd => Background;

        public override Color MenuItemSelected => Selected;
        public override Color MenuItemBorder => Border;
        public override Color MenuItemSelectedGradientBegin => Selected;
        public override Color MenuItemSelectedGradientEnd => Selected;
        public override Color MenuItemPressedGradientBegin => Background;
        public override Color MenuItemPressedGradientEnd => Background;

        public override Color MenuBorder => Border;
        public override Color ToolStripDropDownBackground => Background;
        public override Color ImageMarginGradientBegin => Background;
        public override Color ImageMarginGradientMiddle => Background;
        public override Color ImageMarginGradientEnd => Background;

        public override Color ButtonSelectedGradientBegin => Selected;
        public override Color ButtonSelectedGradientMiddle => Selected;
        public override Color ButtonSelectedGradientEnd => Selected;
        public override Color ButtonSelectedBorder => Border;

        public override Color ButtonPressedGradientBegin => Pressed;
        public override Color ButtonPressedGradientMiddle => Pressed;
        public override Color ButtonPressedGradientEnd => Pressed;
        public override Color ButtonPressedBorder => Border;

        public override Color SeparatorDark => Border;
        public override Color SeparatorLight => Background;
    }
}
