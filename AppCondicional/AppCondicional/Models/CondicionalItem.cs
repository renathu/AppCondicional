using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCondicional.Models
{
    [Table("TB_CONDICIONAL_ITEM")]
    public class CondicionalItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdCondicional { get; set; }
        public string Descricao { get; set; }

        public string CodigoBarra { get; set; }
        public decimal Preco { get; set; }
        public string Foto { get; set; }
    }
}
