using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumoPrimeiraApi.Models
{
    public class Categoria
    {        
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Campo Obrigatório!")]
        public string Nome { get; set; }
    }
}