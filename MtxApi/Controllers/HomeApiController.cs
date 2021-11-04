using MtxApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MtxApi.Controllers
{
    public class HomeApiController : Controller
    {
        readonly private MtxApiContext db = new MtxApiContext();

        List<AnaliseTributaria> analise = new List<AnaliseTributaria>();

        Usuario usuario;
        Empresa empresa;

        // GET: HomeApi
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EmpresaTributacao(string cnpj)
        {
            if (cnpj.Length != 14)
            {
                ViewBag.ErroMensagem="CNPJ PASSADO COMO PARÂMETRO ESTÁ INCORRETO";
                return View();
            }
            else
            {
                string cnpjFormatado = FormataCnpj.FormatarCNPJ(cnpj);
                this.empresa = (from a in db.Empresas where a.cnpj == cnpjFormatado select a).FirstOrDefault(); //empresa
            }
           
            
            if(empresa == null)
            {
                ViewBag.ErroMensagem = "Não há empresa cadasrtrada para esse cnpj";
            }
            else
            {
                this.usuario = (from a in db.Usuarios where a.idEmpresa == empresa.id select a).FirstOrDefault();

                if (usuario == null)
                {
                    ViewBag.ErroMensagem = "Não há usuarios cadastrados para empresa";
                }
                else
                {
                    ViewBag.RazaoSocial = empresa.razacaosocial;
                    ViewBag.Fantasia = empresa.fantasia;
                    ViewBag.Cnpj = empresa.cnpj;
                    ViewBag.Cidade = empresa.cidade;
                    ViewBag.Estado = empresa.estado;

                    //Analise
                    this.analise = (from a in db.Analise_Tributaria where a.CNPJ_EMPRESA == empresa.cnpj select a).ToList();
                    ViewBag.TotalIcmsVarejoCF = this.analise.Count();
                    /*OBS: 22072021: ACERTADO COMPARAÇÃO DE IGUALDADE: RETIRAR OS NULOS*/
                    /*Aliq ICMS Venda Varejo Consumidor Final - ok*/
                    ViewBag.AlqICMSVarejoCFMaior = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL > a.Aliq_Icms_Venda_Varejo_Cons_Final_INTERNO);
                    ViewBag.AlqICMSVarejoCFMenor = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL < a.Aliq_Icms_Venda_Varejo_Cons_Final_INTERNO);
                    ViewBag.AlqICMSVarejoCFIgual = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL == a.Aliq_Icms_Venda_Varejo_Cons_Final_INTERNO && a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL != null);
                    ViewBag.AlqICMSVarejoCFNullaInterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL != null && a.Aliq_Icms_Venda_Varejo_Cons_Final_INTERNO == null); //onde nao for nulo no cliente mas no mtx sim
                    ViewBag.AlqICMSVarejoCFNullaExterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONS_FINAL == null);
                   


                    /*Aliq ICMS ST Venda Varejo Consumidor Final - ok*/
                    ViewBag.AlqICMSSTVarejoCFMaior = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL > a.Aliq_Icms_ST_Venda_Varejo_Cons_Final_INTERNO);
                    ViewBag.AlqICMSSTVarejoCFMenor = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL < a.Aliq_Icms_ST_Venda_Varejo_Cons_Final_INTERNO);
                    ViewBag.AlqICMSSTVarejoCFIgual = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL == a.Aliq_Icms_ST_Venda_Varejo_Cons_Final_INTERNO && a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL != null);
                    ViewBag.AlqICMSSTVarejoCFNulaInterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL != null && a.Aliq_Icms_ST_Venda_Varejo_Cons_Final_INTERNO == null);
                    ViewBag.AlqICMSSTVarejoCFNulaExterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONS_FINAL == null);

                    /*Aliq ICMS Venda Varejo Contribuinte - ok*/
                    ViewBag.AlqICMSVendaVContMaior = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONT > a.Aliq_Icms_Venda_Varejo_Cont_INTERNO);
                    ViewBag.AlqICMSVendaVContMenor = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONT < a.Aliq_Icms_Venda_Varejo_Cont_INTERNO);
                    ViewBag.AlqICMSVendaVContIguais = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONT == a.Aliq_Icms_Venda_Varejo_Cont_INTERNO && a.ALIQ_ICMS_VENDA_VAREJO_CONT != null);
                    ViewBag.AlqICMSVendaVContNulasInternos = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONT != null && a.Aliq_Icms_Venda_Varejo_Cont_INTERNO == null);
                    ViewBag.AlqICMSVendaVContNulasExternos = this.analise.Count(a => a.ALIQ_ICMS_VENDA_VAREJO_CONT == null);

                    /*Aliq ICMS ST Venda Varejo Contribuinte - ok*/
                    ViewBag.AlqICMSSTVendaVContMaior = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT > a.Aliq_Icms_ST_Venda_Varejo_Cont_INTERNO);
                    ViewBag.AlqICMSSTVendaVContMenor = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT < a.Aliq_Icms_ST_Venda_Varejo_Cont_INTERNO);
                    ViewBag.AlqICMSSTVendaVContIguais = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT == a.Aliq_Icms_ST_Venda_Varejo_Cont_INTERNO && a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT != null);
                    ViewBag.AlqICMSSTVendaVContNulasInternos = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT != null && a.Aliq_Icms_ST_Venda_Varejo_Cont_INTERNO == null);
                    ViewBag.AlqICMSSTVendaVContNulasExternos = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_VAREJO_CONT == null);


                    /*Aliq ICMS venda ATA - ok*/
                    ViewBag.AlqICMSVataMaior = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA > a.Aliq_Icms_Venda_Ata_Cont_INTERNO);
                    ViewBag.AlqICMSVataMenor = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA < a.Aliq_Icms_Venda_Ata_Cont_INTERNO);
                    ViewBag.AlqICMSVataIgual = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA == a.Aliq_Icms_Venda_Ata_Cont_INTERNO && a.ALIQ_ICMS_VENDA_ATA != null);
                    ViewBag.AlqICMSVataNulaInterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA != null && a.Aliq_Icms_Venda_Ata_Cont_INTERNO == null);
                    ViewBag.AlqICMSVataNulaExterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA == null);

                    /*Aliq ICMS ST venda ATA - ok*/
                    ViewBag.AlqICMSSTVataMaior = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA > a.Aliq_Icms_ST_Venda_Ata_Cont_INTERNO);
                    ViewBag.AlqICMSSTVataMenor = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA < a.Aliq_Icms_ST_Venda_Ata_Cont_INTERNO);
                    ViewBag.AlqICMSSTVataIgual = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA == a.Aliq_Icms_ST_Venda_Ata_Cont_INTERNO && a.ALIQ_ICMS_ST_VENDA_ATA != null);
                    ViewBag.AlqICMSSTVataNulaInterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA != null && a.Aliq_Icms_ST_Venda_Ata_Cont_INTERNO == null);
                    ViewBag.AlqICMSSTVataNulaExterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA == null);


                    /*Aliq ICMS Vendo no atacado para Simples Nacional - ok*/
                    ViewBag.AliqICMSVendaAtaSimpNacionalMaior = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL > a.Aliq_Icms_Venda_Ata_Simp_Nacional_INTERNO);
                    ViewBag.AliqICMSVendaAtaSimpNacionalMenor = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL < a.Aliq_Icms_Venda_Ata_Simp_Nacional_INTERNO);
                    ViewBag.AliqICMSVendaAtaSimpNacionalIgual = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL == a.Aliq_Icms_Venda_Ata_Simp_Nacional_INTERNO && a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL != null);
                    ViewBag.AliqICMSVendaAtaSimpNacionalNulaInterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL != null && a.Aliq_Icms_Venda_Ata_Simp_Nacional_INTERNO == null);
                    ViewBag.AliqICMSVendaAtaSimpNacionalNulaExterno = this.analise.Count(a => a.ALIQ_ICMS_VENDA_ATA_SIMP_NACIONAL == null);


                    /*Aliq ICMS ST Venda no atacado para Simples Nacional - ok*/
                    ViewBag.AliqICMSSTVendaAtaSimpNacionalMaior = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL > a.Aliq_Icms_ST_Venda_Ata_Simp_Nacional_INTERNO);
                    ViewBag.AliqICMSSTVendaAtaSimpNacionalMenor = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL < a.Aliq_Icms_ST_Venda_Ata_Simp_Nacional_INTERNO);
                    ViewBag.AliqICMSSTVendaAtaSimpNacionalIgual = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL == a.Aliq_Icms_ST_Venda_Ata_Simp_Nacional_INTERNO && a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL != null);
                    ViewBag.AliqICMSSTVendaAtaSimpNacionalNulaInterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL != null && a.Aliq_Icms_ST_Venda_Ata_Simp_Nacional_INTERNO == null);
                    ViewBag.AliqICMSVendaAtaSimpNacionalNulaExterno = this.analise.Count(a => a.ALIQ_ICMS_ST_VENDA_ATA_SIMP_NACIONAL == null);
                }
            }
            


            //this.analise = (from a in db.Analise_Tributaria where a.CNPJ_EMPRESA == empresa.cnpj select a).ToList();

            return View();
        }
    }
}