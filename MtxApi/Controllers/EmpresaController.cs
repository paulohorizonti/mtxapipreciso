using MtxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Description;

namespace MtxApi.Controllers
{
    public class EmpresaController : ApiController
    {
        private MtxApiContext db = new MtxApiContext();

        // GET: api/Empresa
        public IQueryable<Empresa> GetEmpresas()
        {
            return db.Empresas;
        }

        //Retorna a empresa pelo ID
        // GET: api/EmpresaMtx/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult GetEmpresa(int id)
        {
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return BadRequest("EMPRESA NÃO ENCONTRADA");
            }

            return Ok(empresa);
        }


        //Retorna a empresa pelo CNPJ
        // GET: api/EmpresaMtx/5
        [Route("api/EmpresaCnpj/{cnpj}")]
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult GetEmpresaCnpj(string cnpj)
        {
            
            if (cnpj == null)
            {
                return BadRequest("FAVOR INFORMAR O CNPJ NO PARÂMETRO");
            }
            //formatando a string
            string cnpjFormatado = FormataCnpj.FormatarCNPJ(cnpj);

            //verificar a qtd de digitos
            if (cnpj.Length != 14)
            {
                return BadRequest("CNPJ PASSADO COMO PARÂMETRO ESTÁ INCORRETO");
            }
            Empresa empresa = db.Empresas.FirstOrDefault(x => x.cnpj.Equals(cnpjFormatado));

            if (empresa == null)
            {
                return BadRequest("EMPRESA NÃO ENCONTRADA");
            }

            return Ok(empresa);
        }

        /*Incluir registro de nova empresa*/
        [Route("api/EmpresaSalvar/{cnpj}")]
        public IHttpActionResult PostEmpresaSalvar(string cnpj, List<EmpresaJson>dados)
        {
            //verificar se cnpj é nulo
            if (cnpj == null)
            {
                return BadRequest("FAVOR INFORMAR O CNPJ NO PARÂMETRO");
            }
            else
            {
                //verificar a qtd de digitos
                if (cnpj.Length != 14)
                {
                    return BadRequest("CNPJ PASSADO COMO PARÂMETRO ESTÁ INCORRETO");
                }
                else
                {
                    bool cnp = FormataCnpj.ValidaCNPJ(cnpj);

                    if (!cnp)
                    {
                        return BadRequest("CNPJ PASSADO COMO PARÂMETRO ESTÁ INCORRETO");
                    }
                }
            }

            //verificar se os dados informados no json estão nulos
            if (dados == null)
            {
                return BadRequest("JSON SEM DADOS DA EMPRESA PARA CADASTRO");
            }

            //formatando a string
            string cnpjFormatado = FormataCnpj.FormatarCNPJ(cnpj);

            //objeto para procurar empresa no banco
            Empresa empresa = db.Empresas.FirstOrDefault(x => x.cnpj.Equals(cnpjFormatado));

            //objeto para ser salvo no banco
            Empresa empresaSalvar = new Empresa();
            
            //verifica se a empresa já está cadastrada
            if (empresa == null)
            {
               //percorrer os dados informados
                foreach (EmpresaJson dado in dados)
                {
                    //verifica se o cnpj informado no json está nulo
                    if (dado.CNPJ_EMPRESA == null)
                    {
                        return BadRequest("CNPJ NO JSON INVÁLIDO OU AUSENTE");
                    }
                    else
                    {
                        //compara o cnpj informado no json com o informado na url da requisição
                        if (dado.CNPJ_EMPRESA != cnpjFormatado)
                        {
                            if (dado.CNPJ_EMPRESA != cnpj)
                            {
                                return BadRequest("O CNPJ INFORMADO NO PARAMETRO DIFERE DO INFORMADO NO JSON. VERIFIQUE E TENTE NOVAMENTE");
                            }
                        }
                    }

                    //tenta salvar os dados
                    try
                    {
                        //vindos do json
                        empresaSalvar.cnpj = FormataCnpj.FormatarCNPJ(dado.CNPJ_EMPRESA);
                        empresaSalvar.razacaosocial = dado.RAZAO_SOCIAL;
                        empresaSalvar.fantasia = dado.FANTASIA;
                        empresaSalvar.logradouro = dado.LOGRADOURO;
                        empresaSalvar.numero = dado.NUMERO;
                        empresaSalvar.cep = dado.CEP;
                        empresaSalvar.complemento = dado.COMPLEMENTO;
                        empresaSalvar.cidade = dado.CIDADE;
                        empresaSalvar.estado = dado.ESTADO;
                        empresaSalvar.telefone = dado.TELEFONE;
                        empresaSalvar.email = dado.EMAIL;

                        //automaticos
                        empresaSalvar.datacad = DateTime.Now;
                        empresaSalvar.dataalt = DateTime.Now;
                        empresaSalvar.ativo = 1;

                        //adiciona o objeto ao contexto do banco
                        db.Empresas.Add(empresaSalvar);

                        //salva a empresa
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                          
                            //salvar usuario
                            //buscar empresa
                            //objeto para procurar empresa no banco
                            Empresa empresaUsu = db.Empresas.FirstOrDefault(x => x.cnpj.Equals(cnpjFormatado));
                            //objeto para ser salvo no banco
                            Usuario usuarioSalvar = new Usuario();
                            usuarioSalvar.idEmpresa = empresaUsu.id;
                            usuarioSalvar.nome = "adminempresatemp_" + cnpj; //nome do usuario adminEmpresa+cnpj
                            usuarioSalvar.senha = cnpj.ToString() + "adminempresatemp";//senha é o usuario invertido
                            usuarioSalvar.idNivel = 5; //nivel administrativo
                            usuarioSalvar.ativo = 1;
                            usuarioSalvar.dataAlt = DateTime.Now;
                            usuarioSalvar.dataCad = DateTime.Now;
                            usuarioSalvar.email = "admintemp_" + cnpj + "@precisomtx.com.br";//usuario
                            db.Usuarios.Add(usuarioSalvar);
                            db.SaveChanges();

                            //retornar a empresa com o usuário cadastradado
                            Empresa empresaUsuListar = db.Empresas.FirstOrDefault(x => x.cnpj.Equals(cnpjFormatado));

                            return Ok(empresaUsuListar);
                        }

                        
                    }
                    catch(Exception e)
                    {
                        return BadRequest("ERRO AO SALVAR A EMPRESA, VERIFIQUE OS DADOS E TENTE NOVAMENTE"+e.ToString());
                    }
                }//FIM FOREACH
            }
            else
            {
                return BadRequest("EMPRESA JÁ CADASTRADA NA BASE DE DADOS");

            }

            return Ok();

        }

        



    }
}
