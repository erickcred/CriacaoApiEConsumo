using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumoPrimeiraApi.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Estoque { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}