using AppCondicional.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using static AppCondicional.Datas.Database;

namespace AppCondicional.ViewModels
{
    public class CondicionaisViewModel
    {
        private DataBase dataBase = new DataBase();
       
        private ObservableCollection<Condicional> items;
        public ObservableCollection<Condicional> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }

        private bool load = false;

        public CondicionaisViewModel()
        {

        }

        public void Load()
        {
            var listValues = dataBase.GetCondicionais();

            if (listValues != null && listValues.Count > 0)
            {
                Items = new ObservableCollection<Condicional>(listValues.OrderByDescending(f => f.DataHora));
            }
            else
            {
                Items = new ObservableCollection<Condicional>();
            }

            load = true;
        }

        internal void Inserir(Condicional condicional)
        {
            dataBase.Inserir(condicional);

            if(load)
            {
                Items.Add(condicional);
            }
        }

        internal void Atualizar(Condicional condicional)
        {
            dataBase.Atualizar(condicional);

            if (load)
            {
                var item = Items.FirstOrDefault(f => f.Id == condicional.Id);
                if(item != null)
                {
                    item.Descricao = condicional.Descricao;
                    item.DataHora = condicional.DataHora;
                    item.Nome = condicional.Nome;
                    item.Endereco = condicional.Endereco;
                    item.Situacao = condicional.Situacao;
                }
            }
        }

        internal void Excluir(Condicional condicional)
        {
            dataBase.DeletarCondicional(condicional.Id);
            items.Remove(condicional);
        }
    }
}
