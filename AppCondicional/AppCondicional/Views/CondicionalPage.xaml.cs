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
    public partial class CondicionalPage : ContentPage
    {
        private CondicionaisViewModel condicionaisViewModel = new CondicionaisViewModel();

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

        private bool backPage = true;

        public CondicionalPage()
        {
            InitializeComponent();

            cbxSituacao.SelectedIndex = 0;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(string.IsNullOrEmpty(IdCondicional) == false)
            {
                condicionaisViewModel.Load();
                var condicionalInfo = condicionaisViewModel.Items.First(f => f.Id == Convert.ToInt32(IdCondicional));

                tbxDescricao.Text = condicionalInfo.Descricao;
                datePicker.Date = condicionalInfo.DataHora;
                tbxNome.Text = condicionalInfo.Nome;
                tbxEndereco.Text = condicionalInfo.Endereco;
                cbxSituacao.SelectedIndex = condicionalInfo.Situacao;
            }

            tbxDescricao.Focus();

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

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxDescricao.Text))
            {
                await DisplayAlert("Atenção", "Informe a descrição", "OK");
                return;
            }

            Condicional condicional = new Condicional();
            condicional.Descricao = tbxDescricao.Text;
            condicional.DataHora = datePicker.Date;
            condicional.Nome = tbxNome.Text;
            condicional.Endereco = tbxEndereco.Text;
            condicional.Situacao = cbxSituacao.SelectedIndex;

            backPage = false;

            if (string.IsNullOrEmpty(IdCondicional) == true)
            {
                condicionaisViewModel.Inserir(condicional);              

                await Shell.Current.GoToAsync($"../{nameof(CondicionalItensPage)}?IdCondicional={condicional.Id}");
            }
            else
            {
                condicional.Id = Convert.ToInt32(IdCondicional);
                condicionaisViewModel.Atualizar(condicional);

                await Shell.Current.GoToAsync("..");
            }
        }
    }
}