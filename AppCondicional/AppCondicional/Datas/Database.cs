using AppCondicional.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppCondicional.Datas
{
    public class Database
    {
        public class DataBase
        {
            string pasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            public DataBase()
            {
                CriarBancoDeDados();
            }

            public bool CriarBancoDeDados()
            {
                if (File.Exists(System.IO.Path.Combine(pasta, "Condicional.db")) == false)
                {
                    try
                    {
                        using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                        {
                            conexao.CreateTable<Condicional>();
                            conexao.CreateTable<CondicionalItem>();
                            return true;
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool Inserir(Object item)
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        conexao.Insert(item);
                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    return false;
                }
            }

            public List<Condicional> GetCondicionais()
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        return conexao.Table<Condicional>().ToList();
                    }
                }
                catch (SQLiteException ex)
                {
                    return null;
                }
            }

            public bool Atualizar(Object item)
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        conexao.Update(item);

                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    return false;
                }
            }

            public bool DeletarCondicional(int id)
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        conexao.Execute("DELETE FROM TB_CONDICIONAL WHERE ID=?", id);
                        conexao.Execute("DELETE FROM TB_CONDICIONAL_ITEM WHERE IDCONDICIONAL=?", id);
                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    return false;
                }
            }

            public bool DeletarCondicionalItem(int id)
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        conexao.Execute("DELETE FROM TB_CONDICIONAL_ITEM WHERE ID=?", id);
                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    return false;
                }
            }

            public List<CondicionalItem> GetCondicionalItem(int Id)
            {
                try
                {
                    using (var conexao = new SQLiteConnection(System.IO.Path.Combine(pasta, "Condicional.db")))
                    {
                        return conexao.Query<CondicionalItem>("SELECT * FROM TB_CONDICIONAL_ITEM WHERE IDCONDICIONAL=?", Id);
                    }
                }
                catch (SQLiteException ex)
                {
                    return null;
                }                
            }
        }
    }
}
