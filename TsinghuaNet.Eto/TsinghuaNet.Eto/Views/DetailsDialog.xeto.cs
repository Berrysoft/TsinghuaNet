using Eto.Forms;
using Eto.Serialization.Xaml;
using TsinghuaNet.ViewModels;

namespace TsinghuaNet.Eto.Views
{
    public class DetailsDialog : Dialog
    {
        private DetailViewModel Model;

        public DetailsDialog()
        {
            XamlReader.Load(this);
            Model = new DetailViewModel();
            DataContext = Model;
        }
    }
}
