using System.Xml;
using System.ComponentModel;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrimeiraApi.Data;
using PrimeiraApi.Models;

namespace PrimeiraApi.Controllers
{
    [ApiController]
    [Route("/api/produtos/")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetAll([FromServices] Contexto contexto)
        {
            var produtos = contexto.Produtos
                            .AsNoTracking()
                            .Include(x => x.Categoria)
                            .ToList();

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromServices] Contexto contexto, [FromRoute] int id)
        {
            var produto = contexto.Produtos
                            .AsNoTracking()
                            .Include(x => x.Categoria)
                            .FirstOrDefault(x => x.Id == id);

            return Ok(produto);
        }

        [HttpPost("criar")]
        public IActionResult Create([FromServices] Contexto contexto, [FromBody] Produto model)
        {
            var produto = new Produto
            {
                Nome = model.Nome,
                Estoque = model.Estoque,
                CategoriaId = model.CategoriaId
            };
            
            contexto.Produtos.Add(produto);
            contexto.SaveChanges();
            return CreatedAtAction("Get", new { id = produto.Id }, produto);
            // return CreatedAtAction("Get", new { id = categoria.Id }, categoria);
        }

        [HttpPut("")]
        public IActionResult Update([FromServices] Contexto contexto, [FromBody] Produto model)
        {
            if (!Existe(model.Id, contexto))
                return NotFound();

            try
            {
                var produto = new Produto
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Estoque = model.Estoque,
                    CategoriaId = model.CategoriaId
                };

                contexto.Produtos.Update(produto);
                // contexto.Categorias.(produto);
                contexto.SaveChanges();
                return Ok(produto);

            } catch (DbUpdateException error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromServices] Contexto contexto, [FromRoute] int id)
        {
            var produto = contexto.Produtos.FirstOrDefault(x => x.Id == id);
            if (produto == null)
                return NotFound("Produto não encontrado");

            contexto.Produtos.Remove(produto);
            contexto.SaveChanges();
            return Ok(produto);
        }

        private bool Existe(int id, [FromServices] Contexto contexto)
        {
            return contexto.Produtos.Any(x => x.Id == id);
        }
    }
}