using Lampadas.Store;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;

namespace Lampadas.Controllers
{
    [RoutePrefix("api/equipe")]
    public class EquipeController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(PostData equipe)
        {
            if (Data.Equipes.ContainsKey(equipe.CodEquipe))
            {
                return base.BadRequest("CodEquipe já cadastrado");
            }

            Data.Equipes.TryAdd(equipe.CodEquipe, (EquipeData)equipe);

            return Ok(new { mensagem = "Equipe adicionada com sucesso" });
        }

        [HttpPut, Route("{codEquipe}")]
        public IHttpActionResult Put([FromUri]byte codEquipe, [FromBody]PutData equipe)
        {
            if (codEquipe == 0) return Unauthorized();

            equipe.CodEquipe = codEquipe;

            if (!Data.Equipes.ContainsKey(equipe.CodEquipe))
            {
                return base.BadRequest("CodEquipe não cadastrado");
            }

            if (equipe.Autorizada && !Data.Equipes[equipe.CodEquipe].Autorizada)
            {
                LampadasController.ApagarTodas();
                Data.EquipeAutorizada = equipe.CodEquipe;
            }
            else if (!equipe.Autorizada)
            {
                Data.EquipeAutorizada = 0;
            }

            Data.Equipes[equipe.CodEquipe] = (EquipeData)equipe;

            return Ok(new { mensagem = "Equipe atualizada com sucesso" });
        }

        [HttpDelete]
        public IHttpActionResult Delete(byte codEquipe)
        {
            if (codEquipe == 0) return Unauthorized();

            if (!Data.Equipes.ContainsKey(codEquipe))
            {
                return base.BadRequest("CodEquipe não cadastrado");
            }

            Data.Equipes.TryRemove(codEquipe, out EquipeData equipe);

            return Ok(new { mensagem = "Equipe removida com sucesso", equipe });
        }

        [HttpGet]
        public IHttpActionResult Get(byte? codEquipe = null)
        {
            var equipes = Data.Equipes.Select(e => e.Value);

            if (codEquipe.HasValue)
                equipes = equipes.Where(e => e.CodEquipe == codEquipe);
            else
                equipes = equipes.Where(e => e.CodEquipe > 0);

            return Ok(equipes);
        }


        [DataContract]
        public class PostData
        {
            [DataMember(Name = "cod-equipe")]
            public byte CodEquipe { get; set; }

            [DataMember(Name = "nome")]
            public string Nome { get; set; }

            public static explicit operator EquipeData(PostData v)
            {
                return new EquipeData
                {
                    CodEquipe = v.CodEquipe,
                    Nome = v.Nome
                };
            }
        }

        [DataContract]
        public class PutData
        {
            public byte CodEquipe { get; set; }

            [DataMember(Name = "nome")]
            public string Nome { get; set; }

            [DataMember(Name = "autorizada")]
            public bool Autorizada { get; set; }

            public static explicit operator EquipeData(PutData v)
            {
                return new EquipeData
                {
                    CodEquipe = v.CodEquipe,
                    Nome = v.Nome,
                    Autorizada = v.Autorizada
                };
            }
        }
    }
}