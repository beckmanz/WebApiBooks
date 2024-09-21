using Microsoft.EntityFrameworkCore;
using WebApiBooks.Data;
using WebApiBooks.Dto.Livro;
using WebApiBooks.Models;
namespace WebApiBooks.Services.Livro;

public class LivroServices : ILivroInterface
{
    private readonly AppDbContext _context;

    public LivroServices(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ResponseModel<List<LivroModel>>> ListarLivros()
    {
        ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
        try
        {

            var livros = await _context.Livros.Include(a => a.Autor).ToListAsync();

            resposta.Dados = livros;
            resposta.Mensagem = "Todos os livros foram coletados!";
            return resposta;

        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }

    public async Task<ResponseModel<LivroModel>> BuscarLivroPorId(int idLivro)
    {
        ResponseModel<LivroModel> resposta = new ResponseModel<LivroModel>();
        try
        {
            var livro = await _context.Livros.Include(a => a.Autor).FirstOrDefaultAsync(bancoLivros => bancoLivros.Id == idLivro);
            if (livro == null)
            {
                resposta.Mensagem = "Livro não encontrado!";
                return resposta;
            }

            resposta.Dados = livro;
            resposta.Mensagem = "Livro encontrado!";
            return resposta;

        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }

    public async Task<ResponseModel<List<LivroModel>>> BuscarLivroPorIdAutor(int idAutor)
    {
        ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
        try
        {
            var livro = await _context.Livros
                .Include(a => a.Autor)
                .Where(livroBanco => livroBanco.Autor.Id == idAutor)
                .ToListAsync();

            if (livro == null)
            {
                resposta.Mensagem = "Nenhum registro encontrado encontrado!";
                return resposta;
            }

            resposta.Dados = livro;
            resposta.Mensagem = "Livros localizados!";
            return resposta;

        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }

    public async Task<ResponseModel<List<LivroModel>>> CriarLivro(LivroCriacaoDto livroCriacaoDto)
    {
        ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
        try
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(autorBanco =>
                autorBanco.Id == livroCriacaoDto.Autor.Id);
            if (autor == null)
            {
                resposta.Mensagem = "Nenhum autor localizado";
                return resposta;
            }
            
            var livro = new LivroModel()
            {
                Titulo = livroCriacaoDto.Titulo,
                Autor = autor
            };

            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
            
            resposta.Dados = await _context.Livros.Include(a => a.Autor).ToListAsync();
            resposta.Mensagem = "Livro criado com sucesso!";
            return resposta;
            
        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }

    public async Task<ResponseModel<List<LivroModel>>> EditarLivro(LivroEdicaoDto livroEdicaoDto)
    {
        ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
        try
        {
            var livro = await _context.Livros
                .Include(a => a.Autor)
                .FirstOrDefaultAsync(livroBanco => livroBanco.Id == livroEdicaoDto.Id);
            
            var autor = await _context.Autores.FirstOrDefaultAsync(autorbanco =>
                autorbanco.Id == livroEdicaoDto.Autor.Id);
            
            if (livro == null)
            {
                resposta.Mensagem = "Nenhum livro localizado";
                return resposta;
            }
            
            if (autor == null)
            {
                resposta.Mensagem = "Nenhum autor localizado";
                return resposta;
            }

            livro.Titulo = livroEdicaoDto.Titulo;
            livro.Autor = autor;

            _context.Update(livro);
            await _context.SaveChangesAsync();

            resposta.Dados = await _context.Livros.ToListAsync();
            return resposta;
        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }

    public async Task<ResponseModel<List<LivroModel>>> ExcluirLivro(int idLivro)
    {
        ResponseModel<List<LivroModel>> resposta = new ResponseModel<List<LivroModel>>();
        try
        {
            var livro = await _context.Livros.Include(a => a.Autor).FirstOrDefaultAsync(livroBanco => livroBanco.Id == idLivro);
            if (livro == null)
            {
                resposta.Mensagem = "Livro não encontrado!";
                return resposta;
            }

            _context.Remove(livro);
            await _context.SaveChangesAsync();

            resposta.Dados = await _context.Livros.ToListAsync();
            resposta.Mensagem = "Livro excluído com sucesso!";
            return resposta;
        }
        catch (Exception ex)
        {
            resposta.Mensagem = ex.Message;
            resposta.Status = false;
            return resposta;
        }
    }
}