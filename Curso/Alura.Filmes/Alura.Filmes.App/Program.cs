using Alura.Filmes.App.Dados;
using Alura.Filmes.App.Extensions;
using Alura.Filmes.App.Negocio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Alura.Filmes.App
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using(var contexto = new AluraFilmesContexto())
            {
                contexto.LogSQLToConsole();

                var idiomas = contexto.Idiomas.Include(i => i.FilmesFalados);

                foreach(var idioma in idiomas)
                {
                    Console.WriteLine(idioma);

                    foreach(var filme in idioma.FilmesFalados)
                    {
                        Console.WriteLine(filme);
                    }

                    Console.WriteLine();
                }

            }
        }

        private static void ExibeFilmeElenco(AluraFilmesContexto contexto)
        {
            var filme = contexto.Filmes
                                .Include(f => f.Atores)
                                .ThenInclude(fa => fa.Ator)
                                .First();

            Console.WriteLine(filme);
            Console.WriteLine("\nElenco: \n");

            foreach (var ator in filme.Atores)
            {
                Console.WriteLine(ator.Ator);
            }
        }

        private static void ExibeElenco(AluraFilmesContexto contexto)
        {
 
            foreach (var item in contexto.Elenco)
            {
                var entidade = contexto.Entry(item);
                var filmId = entidade.Property("film_id").CurrentValue;
                var actorId = entidade.Property("actor_id").CurrentValue;
                var lastUpd = entidade.Property("last_update").CurrentValue;
                               
                Console.WriteLine($"Filme {filmId}, Ator {actorId}, LastUpdate: {lastUpd}");
            }
        }

        private static void ExibeFilmes(AluraFilmesContexto contexto)
        {
            foreach (var filme in contexto.Filmes)
            {
                Console.WriteLine(filme);
            }
        }

        private static void ListarDezFilmesRecentesModificados(AluraFilmesContexto contexto)
        {
            // listar os 10 atores modificados recentemente
            var atores = contexto.Atores
                                 .OrderByDescending(a => EF.Property<DateTime>(a, "last_update"))
                                 .Take(10);
            foreach (var ator in atores)
            {
                Console.WriteLine(ator + " - " + contexto.Entry(ator).Property("last_update").CurrentValue);
            }
        }

        private static void SelecionaPrimeiroAtor(AluraFilmesContexto contexto)
        {
            var ator = contexto.Atores.First();
            Console.WriteLine(ator);
            Console.WriteLine(contexto.Entry(ator).Property("last_update").CurrentValue);
            Console.WriteLine();

            contexto.SaveChanges();
        }

        private static void InsereAtor(AluraFilmesContexto contexto, string primeiroNome = "Tom", string ultimoNome = "Hanks")
        {
            var ator = new Ator();
            ator.PrimeiroNome = primeiroNome;
            ator.UltimoNome = ultimoNome;

            //contexto.Entry(ator).Property("last_update").CurrentValue = DateTime.Now;

            contexto.Atores.Add(ator);

            contexto.SaveChanges();
        }

        private static void ExibeAtores(AluraFilmesContexto contexto)
        {
            foreach (var ator in contexto.Atores)
            {
                System.Console.WriteLine(ator);
            }
        }
    }
}