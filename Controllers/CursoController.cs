using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;


namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly EscolaContext _school;
        public CursoController(EscolaContext context)
        {
            //construtor
            _school = context;
        }
        [HttpGet]
        public ActionResult<List<Curso>> GetAll()
        {
            return _school.Curso.ToList();
        }

        [HttpGet("{CursoId}")]
        public ActionResult<List<Curso>> Get(int CursoId)
        {
            try
            { 
                var result = _school.Curso.Find(CursoId);
                if(result == null)
                {return NotFound();}
                
                return Ok(result);
            }

            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                "Falha no acesso ao banco de dados.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> post (Curso model)
        {
            try
            {
                _school.Curso.Add(model);
                if(await _school.SaveChangesAsync() == 1)
                {
                    return Created($"/api/Curso/{model.codCurso}", model);
                }
            }

            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }

        [HttpPut("{CursoId}")]
        public async Task<ActionResult> put(int CursoId, Curso dadosCursoAlt)
        {
            try
            {
                //verificar se 
                var result = await _school.Curso.FindAsync(CursoId);
                if(CursoId != result.id)
                {
                    return BadRequest();
                }
                result.nome = dadosCursoAlt.nome;
                result.codCurso = dadosCursoAlt.codCurso;

                await _school.SaveChangesAsync();
                return Created($"/api/curso/{dadosCursoAlt.codCurso}", dadosCursoAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                "Falha no acesso ao banco de dados.");
            }
        }

         [HttpDelete("{CursoId}")]
        public async Task<ActionResult> delete(int CursoId)
        {
            try
            {
                //verificar se existe curso a ser excluido
                var curso = await _school.Curso.FindAsync(CursoId);
                if(curso == null)
                {
                    return NotFound();
                }

                _school.Remove(curso);
                await _school.SaveChangesAsync();

                return NoContent();
            }
            catch 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                "Falha no acesso ao excluir curso.");
            }
        }

    }
}