using AppCondicional.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static AppCondicional.Datas.Database;

namespace AppCondicional.ViewModels
{
    public class CondicionalItemViewModel
    {
        private DataBase dataBase = new DataBase();

        private ObservableCollection<CondicionalItem> items;
        public ObservableCollection<CondicionalItem> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }

        private bool load = false;

        public CondicionalItemViewModel()
        {

        }

        public void Load(int codigoCondicional)
        {
            var listValues = dataBase.GetCondicionalItem(codigoCondicional);

            if (listValues != null && listValues.Count > 0)
            {
                Items = new ObservableCollection<CondicionalItem>(listValues);
            }
            else
            {
                Items = new ObservableCollection<CondicionalItem>();
            }

            load = true;
        }

        internal void Inserir(CondicionalItem condicionalItem)
        {
            dataBase.Inserir(condicionalItem);

            if (load)
            {
                Items.Add(condicionalItem);
            }
        }

        internal void Atualizar(CondicionalItem condicionalItem)
        {
            dataBase.Atualizar(condicionalItem);

            if (load)
            {
                var item = Items.FirstOrDefault(f => f.Id == condicionalItem.Id);
                if (item != null)
                {
                    item.Descricao = condicionalItem.Descricao;
                    item.Preco = condicionalItem.Preco;
                    item.CodigoBarra = condicionalItem.CodigoBarra;
                    item.Preco = condicionalItem.Preco;
                    item.Foto = condicionalItem.Foto;
                }
            }
        }

        internal void Excluir(CondicionalItem condicionalItem)
        {
            dataBase.DeletarCondicionalItem(condicionalItem.Id);

            items.Remove(condicionalItem);
        }
    }
}
