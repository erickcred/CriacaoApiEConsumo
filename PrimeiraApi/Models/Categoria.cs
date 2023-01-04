using System;
using System.Collections.Generic;

namespace PrimeiraApi.Models
{
    public class Categoria
    {
        public Categoria()
        {
            // Produtos = new List<Produto>();
        }
        public int Id { get; set; }
        public string Nome { get; set; }

        // public List<Produto> Produtos { get; set; }
    }
}