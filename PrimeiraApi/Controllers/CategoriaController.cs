using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using PrimeiraApi.Data;
using PrimeiraApi.Models;

namespace PrimeiraApi.Controllers
{
    [ApiController]
    [Route("/api/categorias/")]
    public class CategoriaController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetAll([FromServices] Contexto contexto)
        {
            var categorias = contexto.Categorias
                            .AsNoTracking()
                            .ToList();

            return Ok(categorias);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get([FromServices] Contexto contexto, [FromRoute] int id)
        {
            if (!Existe(id, contexto))
                return NotFound("Categoria não encontrada");

            var categoria = contexto.Categorias
                            .AsNoTracking()
                            .FirstOrDefault(x => x.Id == id);            

            return Ok(categoria);
        }

        [HttpPost("criar")]
        public IActionResult Create([FromServices] Contexto contexto, [FromBody] Categoria model)
        {
            var categoria = new Categoria
            {
                Nome = model.Nome,
            };

            contexto.Categorias.Add(categoria);
            contexto.SaveChanges();
            return CreatedAtAction("Get", new { id = categoria.Id }, categoria);
        }

        [HttpPut("")]
        public IActionResult Update([FromServices] Contexto contexto, [FromBody] Categoria model)
        {
            if (!Existe(model.Id, contexto))
                return NotFound("Categoria não encontrada");

            var categoria = new Categoria
            {
                Id = model.Id,
                Nome = model.Nome,
            };

            contexto.Categorias.Update(categoria);
            contexto.SaveChanges();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromServices] Contexto contexto, [FromRoute] int id)
        {
            var categoria = contexto.Categorias.FirstOrDefault(x => x.Id == id);
            if (categoria == null)
                return NotFound("Categoria não encontrada");

            contexto.Categorias.Remove(categoria);
            contexto.SaveChanges();
            return Ok(categoria);
        }

        private bool Existe(int id, [FromServices] Contexto contexto)
        {
            return contexto.Categorias.Any(x => x.Id == id);
        }
    }
}