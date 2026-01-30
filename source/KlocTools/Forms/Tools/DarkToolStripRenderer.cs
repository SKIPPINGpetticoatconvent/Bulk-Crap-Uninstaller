using System.Drawing;
using System.Windows.Forms;

namespace Klocman.Forms.Tools
{
    public class DarkToolStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkToolStripRenderer(ProfessionalColorTable colorTable) : base(colorTable)
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.TextColor = ThemeController.Palette.DarkForeground;
            }
            else
            {
                e.TextColor = ThemeController.Palette.DarkForeground;
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = ThemeController.Palette.DarkForeground;
            base.OnRenderArrow(e);
        }
    }
}
