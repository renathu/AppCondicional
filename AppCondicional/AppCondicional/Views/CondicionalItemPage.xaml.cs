using AppCondicional.Models;
using AppCondicional.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace AppCondicional.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(IdCondicional), nameof(IdCondicional))]
    [QueryProperty(nameof(IdCondicionalItem), nameof(IdCondicionalItem))]
    public partial class CondicionalItemPage : ContentPage
    {
        private CondicionalItemViewModel condicionalItemViewModel = new CondicionalItemViewModel();

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

        string idCondicionalItem = "";
        public string IdCondicionalItem
        {
            get => idCondicionalItem;
            set
            {
                idCondicionalItem = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged();
            }
        }

        private string pathImage = string.Empty;

        private bool backPage = true;

        public CondicionalItemPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (string.IsNullOrEmpty(IdCondicionalItem) == false)
            {
                condicionalItemViewModel.Load(Convert.ToInt32(IdCondicional));
                var condicionalItemInfo = condicionalItemViewModel.Items.First(f => f.Id == Convert.ToInt32(IdCondicionalItem));

                tbxProduto.Text = condicionalItemInfo.Descricao;
                tbxCodigoBarra.Text = condicionalItemInfo.CodigoBarra;
                tbxPreco.Text = condicionalItemInfo.Preco.ToString("N2");
                imageProduto.Source = ImageSource.FromFile(condicionalItemInfo.Foto);
                pathImage = condicionalItemInfo.Foto;
            }

            tbxProduto.Focus();

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

                await Shell.Current.GoToAsync($"..?IdCondicional={IdCondicional}");
            }
        }

        private async void btnTirarFoto_Clicked(object sender, EventArgs e)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);

                if (results.ContainsKey(Permission.Location))
                    status = results[Permission.Location];
            }

            if (status != PermissionStatus.Granted)
            {
                return;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Atenção", "Nenuma Câmera disponível.", "OK");
                return;
            }

            var armazenamento = new StoreCameraMediaOptions()
            {
                SaveToAlbum = true
            };
            var foto = await CrossMedia.Current.TakePhotoAsync(armazenamento);

            if (foto == null)
                return;

            pathImage = foto.AlbumPath;
            imageProduto.Source = ImageSource.FromStream(() =>
            {
                var stream = foto.GetStream();
                foto.Dispose();
                return stream;
            });
        }

        private async void btnSalvar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxProduto.Text))
            {
                await DisplayAlert("Atenção", "Informe o produto", "OK");
                return;
            }

            decimal preco = 0m;
            if (string.IsNullOrEmpty(tbxPreco.Text) == false && decimal.TryParse(tbxPreco.Text, out preco) == false)
            {
                await DisplayAlert("Atenção", "Preço inválido", "OK");
                return;
            }

            CondicionalItem condicionalItem = new CondicionalItem();
            condicionalItem.IdCondicional = Convert.ToInt32(idCondicional);
            condicionalItem.Descricao = tbxProduto.Text;
            condicionalItem.CodigoBarra = tbxCodigoBarra.Text;
            condicionalItem.Preco = preco;
            condicionalItem.Foto = pathImage;

            if (string.IsNullOrEmpty(IdCondicionalItem) == true)
            {
                condicionalItemViewModel.Inserir(condicionalItem);
            }
            else
            {
                condicionalItem.Id = Convert.ToInt32(IdCondicionalItem);
                condicionalItemViewModel.Atualizar(condicionalItem);
            }

            backPage = false;

            await Shell.Current.GoToAsync($"..?IdCondicional={IdCondicional}");
        }

        private void btnExcluirFoto_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pathImage) == false)
            {
                pathImage = null;
                imageProduto.Source = null;
            }
        }

        private async void btnCodigoBarra_Clicked(object sender, EventArgs e)
        {
            var optionsCustom = new MobileBarcodeScanningOptions();
            
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Aproxime a câmera do código de barra",
                BottomText = "Toque na tela para focar"
            };

            var scanResults = await scanner.Scan(optionsCustom);

            if (scanResults != null)
            {
                tbxCodigoBarra.Text = scanResults.Text;
            }
        }
    }
}