using AppCondicional.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCondicional
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(CondicionalPage), typeof(CondicionalPage));
            Routing.RegisterRoute(nameof(CondicionalItensPage), typeof(CondicionalItensPage));
            Routing.RegisterRoute(nameof(CondicionalItemPage), typeof(CondicionalItemPage));
        }       
    }
}