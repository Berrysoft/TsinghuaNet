using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsinghuaNet.CrossPlatform.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TsinghuaNet.CrossPlatform.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        InfoViewModel viewModel;

        public InfoPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new InfoViewModel();
        }
    }
}