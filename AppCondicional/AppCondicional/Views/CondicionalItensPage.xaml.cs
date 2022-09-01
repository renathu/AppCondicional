using AppCondicional.Models;
using AppCondicional.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCondicional.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(IdCondicional), nameof(IdCondicional))]
    public partial class CondicionalItensPage : ContentPage
    {
        private CondicionalItemViewModel condicionalItens;

        private CondicionaisViewModel Condicionais;

        string idCondicional = "";
        public string IdCondicional
        {
            get => idCondicional;
            set
            {
                idCondicional = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged();
            }
        }

        private ViewCell lastCell;

        private bool backPage = true;

        public CondicionalItensPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Condicionais = new CondicionaisViewModel();
            Condicionais.Load();

            var condicionalInfo = Condicionais.Items.First(f => f.Id == Convert.ToInt32(IdCondicional));

            lbCondicional.Text = condicionalInfo.Descricao;

            condicionalItens = new CondicionalItemViewModel();
            condicionalItens.Load(Convert.ToInt32(IdCondicional));

            lbQtdeItens.Text = condicionalItens.Items.Count.ToString();

            BindingContext = condicionalItens;

            Shell.Current.Navigating += Current_Navigating;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Shell.Current.Navigating -= Current_Navigating;
        }

        private async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
        {
            if (e.CanCancel && backPage)
            {
                e.Cancel();

                Shell.Current.Navigating -= Current_Navigating;

                await Shell.Current.GoToAsync("..");
            }
        }        

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.WhiteSmoke;
                lastCell = viewCell;
            }
        }        

        private async void btnAdicionar_Clicked(object sender, EventArgs e)
        {
            if (Condicionais.Items.First(f => f.Id == Convert.ToInt32(IdCondicional)).Situacao == 0)
            {
                backPage = false;

                await Shell.Current.GoToAsync($"{nameof(CondicionalItemPage)}?IdCondicional={IdCondicional}");
            }
            else
            {
                await DisplayAlert("Atenção", "Condicional finalizado não pode ser alterado", "OK");
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            if (Condicionais.Items.First(f => f.Id == Convert.ToInt32(IdCondicional)).Situacao == 0)
            {
                MenuItem menuItem = sender as MenuItem;

                var condicionalItem = menuItem.BindingContext as CondicionalItem;

                condicionalItens.Excluir(condicionalItem);

                lbQtdeItens.Text = condicionalItens.Items.Count.ToString();
            }
            else
            {
                await DisplayAlert("Atenção", "Condicional finalizado não pode ser alterado", "OK");
            }
        }

        private async void listViewItens_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (Condicionais.Items.First(f => f.Id == Convert.ToInt32(IdCondicional)).Situacao == 0)
            {
                var condicionalItem = (CondicionalItem)e.Item;
                backPage = false;

                await Shell.Current.GoToAsync($"{nameof(CondicionalItemPage)}?IdCondicional={IdCondicional}&IdCondicionalItem={condicionalItem.Id}");
            }
            else
            {
                await DisplayAlert("Atenção", "Condicional finalizado não pode ser alterado", "OK");
            }
        }
    }
}