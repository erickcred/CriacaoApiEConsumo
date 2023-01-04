using System.Text;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConsumoPrimeiraApi.Models;
using System.Net.Http;
using Newtonsoft.Json;
using ConsumoPrimeiraApi.Service.Interface;

namespace ConsumoPrimeiraApi.Controllers
{
    [Controller]
    [Route("/produtos/")]
    public class ProdutoController : Controller
    {
        private HttpClient _cliente;
        public ProdutoController()
        {
            _cliente = new HttpClient();
            _cliente.BaseAddress = new Uri("https://localhost:5001/api/");
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await _cliente.GetAsync("produtos");

            if (!res.IsSuccessStatusCode)
                return View("Index", new List<Produto>());

            var dados = await res.Content.ReadAsStringAsync();
            var produtos = JsonConvert.DeserializeObject<List<Produto>>(dados);

            return View("Index", produtos);
        }

        [HttpGet("cadastrar")]
        public async Task<IActionResult> Cadastrar()
        {
            HttpClient categoriaCliente = new HttpClient();
            categoriaCliente.BaseAddress = _cliente.BaseAddress;

            categoriaCliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await categoriaCliente.GetAsync("categorias");
            if (res.IsSuccessStatusCode)
            {
                var categorias = await res.Content.ReadAsStringAsync();
                ViewData["Categorias"] = JsonConvert.DeserializeObject<List<Categoria>>(categorias);
            }                

            return View("Cadastrar");
        }

        [HttpGet("editar/{id:int}")]
        public async Task<IActionResult> Editar([FromRoute] int id)
        {
            // Processo Categoria
            HttpClient categoriaCliente = new HttpClient();
            categoriaCliente.BaseAddress = _cliente.BaseAddress;

            categoriaCliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await categoriaCliente.GetAsync("categorias");
            if (res.IsSuccessStatusCode)
            {
                var categorias = await res.Content.ReadAsStringAsync();
                ViewData["Categorias"] = JsonConvert.DeserializeObject<List<Categoria>>(categorias);
            }

            // Processo Produto         
            _cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage resProduto = await _cliente.GetAsync($"produtos/{id}");
            if (!resProduto.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var dado = await resProduto.Content.ReadAsStringAsync();
            var produtos = JsonConvert.DeserializeObject<Produto>(dado);

            return View("Editar", produtos);
        }

        [HttpPost("atualizar")]
        public async Task<IActionResult> Atualizar([FromForm] Produto model)
        {
            _cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var produto = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(produto, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _cliente.PutAsync("produtos", httpContent);
            if (!res.IsSuccessStatusCode)
                return Redirect($"editar/{model.Id}");
            return RedirectToAction("Index");
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromForm] Produto model)
        {
            var produto = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(produto, Encoding.UTF8, "application/json");
            HttpResponseMessage res = await _cliente.PostAsync("produtos/criar", httpContent);

            if (!res.IsSuccessStatusCode)
                return RedirectToAction(nameof(Cadastrar));

            return RedirectToAction("Index");
        }

        [HttpGet("deletar/{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id)
        {
            var res = await _cliente.DeleteAsync($"produtos/{id}");
            return RedirectToAction("Index");
        }
    }
}