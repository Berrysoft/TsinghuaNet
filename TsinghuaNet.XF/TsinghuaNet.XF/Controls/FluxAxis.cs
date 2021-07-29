using Syncfusion.SfChart.XForms;
using TsinghuaNet.Models;

namespace TsinghuaNet.XF.Controls
{
    public class FluxAxis : NumericalAxis
    {
        protected override void OnCreateLabels()
        {
            base.OnCreateLabels();

            for (int i = 0; i < VisibleLabels.Count; i++)
            {
                VisibleLabels[i].LabelContent = ByteSize.FromGigaBytes(VisibleLabels[i].Position).ToString();
            }
        }
    }
}
