using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCondicional.Models
{
    [Table("TB_CONDICIONAL")]
    public class Condicional
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataHora { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public int Situacao { get; set; }

    }
}
