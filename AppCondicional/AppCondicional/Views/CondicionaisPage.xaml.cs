using AppCondicional.Models;
using AppCondicional.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static AppCondicional.Datas.Database;

namespace AppCondicional.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CondicionaisPage : ContentPage
    {
        private CondicionaisViewModel condicionais;        

        public CondicionaisPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            condicionais = new CondicionaisViewModel();
            condicionais.Load();

            BindingContext = condicionais;
        }        

        private async void btnAdicionar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CondicionalPage));
        }

        private void MenuItemExcluir_Clicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            var condicional = menuItem.BindingContext as Condicional;

            condicionais.Excluir(condicional);
        }

        private async void MenuItemEditar_Clicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            var condicional = menuItem.BindingContext as Condicional;

            await Shell.Current.GoToAsync($"{nameof(CondicionalPage)}?IdCondicional={condicional.Id}");
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var condicional = (Condicional)e.Item;

            await Shell.Current.GoToAsync($"{nameof(CondicionalItensPage)}?IdCondicional={condicional.Id}");
        }        
    }
}