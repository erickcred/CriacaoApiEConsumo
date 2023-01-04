using System.Text;
using System.Net.Security;
using System.Net;
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
using System.Net.Http.Headers;
using ConsumoPrimeiraApi.Service.Interface;
using Refit;

namespace ConsumoPrimeiraApi.Controllers
{
    [Controller]
    [Route("/categorias/")]
    public class CategoriaController : Controller
    {
        private ICategoriaService _cliente;
        public CategoriaController(ICategoriaService iCategoriaServices)
        {
            _cliente = iCategoriaServices;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // HttpClient cliente = new HttpClient();
            // cliente.BaseAddress = new Uri("https://localhost:5001/api/");
            // cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HttpResponseMessage res = await cliente.GetAsync("/api/categorias");
            // if (res.IsSuccessStatusCode)
            // {
            //     var dados = await res.Content.ReadAsStringAsync();
            //     var categorias = JsonConvert.DeserializeObject<List<Categoria>>(dados);
                
            //     return View("Index", categorias);
            // }

            // return View("Index", new List<Categoria>());
            var categorias = await _cliente.Index();
            return View("Index", categorias);
        }

        [HttpGet("editar/{id:int}")]
        public async Task<IActionResult> Editar([FromRoute] int id)
        {
            // HttpClient cliente = new HttpClient();
            // cliente.BaseAddress = new Uri("https://localhost:5001/api/");
            // cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HttpResponseMessage res = await cliente.GetAsync($"/api/categorias/{id}");
            // if (res.IsSuccessStatusCode)
            // {
            //     var dado = await res.Content.ReadAsStringAsync();
            //     var categoria = JsonConvert.DeserializeObject<Categoria>(dado);
            //     return View("Editar", categoria);
            // }
            // return RedirectToAction("Index");

            try
            {
                var categoria = await _cliente.Editar(id);
                return View("Editar", categoria);
            } catch (ApiException ex)
            {

                return RedirectToAction("Index");
            }
        }

        [HttpPost("atualizar")]
        public async Task<IActionResult> Atualizar([FromForm] Categoria model)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:5001/api/");
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var categoria = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(categoria, Encoding.UTF8, "application/json");

            HttpResponseMessage res = await cliente.PutAsync($"/api/categorias", httpContent);
            return RedirectToAction("Index");
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            return View("Cadastrar");
        }

        [HttpPost("criar")]
        public async Task<IActionResult> Criar([FromForm] Categoria model)
        {
            // if (ModelState.IsValid)
            // {
            //     HttpClient cliente = new HttpClient();
            //     cliente.BaseAddress = new Uri("https://localhost:5001/api/");

            //     var categoria = JsonConvert.SerializeObject(model);
            //     var httpContent = new StringContent(categoria, Encoding.UTF8, "application/json");
            //     HttpResponseMessage res = await cliente.PostAsync("/api/categorias/criar", httpContent);

            //     if (res.StatusCode == System.Net.HttpStatusCode.Created)
            //     {
            //         return RedirectToAction("Index");
            //     }
            // }
            // return RedirectToAction("Cadastrar");
            if (!ModelState.IsValid)
                return RedirectToAction("Cadastrar");

            try
            {
                await _cliente.Criar(model);
                ModelState.AddModelError("", "Cadastro feito com Sucesso!");
                return RedirectToAction("Index");
            } catch (ApiException ex)
            {
                ModelState.AddModelError("", ex + " Erro ao cadastrar Categoria!");
                return RedirectToAction("Cadastrar");
            } catch (Exception ex)
            {
                ModelState.AddModelError("", ex + " Erro ao cadastrar Categoria!");
                return RedirectToAction("Cadastrar");
            }
        }

        [HttpGet("deletar/{id:int}")]
        public async Task<IActionResult> Deletar([FromRoute] int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:5001/api/");

            var res = await cliente.DeleteAsync($"/api/categorias/{id}");
            return RedirectToAction("Index");
        }
    }
}