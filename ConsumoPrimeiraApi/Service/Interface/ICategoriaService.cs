using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ConsumoPrimeiraApi.Models;
using Newtonsoft.Json;
using Refit;

namespace ConsumoPrimeiraApi.Service.Interface
{
    public interface ICategoriaService
    {
        [Post("/api/categorias/criar")]
        Task<Categoria> Criar(Categoria categoria);

        async Task<List<Categoria>> Index()
        {
            List<Categoria> categorias = new List<Categoria>();

            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:5001/api/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage res = await cliente.GetAsync("/api/categorias");
            if (res.IsSuccessStatusCode)
            {
                var dados = await res.Content.ReadAsStringAsync();
                categorias = JsonConvert.DeserializeObject<List<Categoria>>(dados);
                
            }
            return categorias;
        }

        [Get("/api/categorias/{id}")]
        Task<Categoria> Editar(int id);
    }
}