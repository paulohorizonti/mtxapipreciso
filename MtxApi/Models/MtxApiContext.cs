using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MtxApi.Models
{
    public class MtxApiContext : DbContext
    {
        public MtxApiContext() : base("name=MtxApiContext")
        {
        }
        //acesso ao model Empresa do projeto Matriz
        public System.Data.Entity.DbSet<Models.Empresa> Empresas { get; set; }
        public System.Data.Entity.DbSet<Models.TributacaoEmpresa> TributacaoEmpresas { get; set; }
        public System.Data.Entity.DbSet<Models.CategoriaProduto> CategoriasProdutos { get; set; }
        public System.Data.Entity.DbSet<Models.Produto> Produtos { get; set; }
        public System.Data.Entity.DbSet<Models.Usuario> Usuarios { get; set; }
        public System.Data.Entity.DbSet<Models.Nivel> Niveis { get; set; }
        public virtual DbSet<Models.AnaliseTributaria> Analise_Tributaria { get; set; } //Vitor
    }
}