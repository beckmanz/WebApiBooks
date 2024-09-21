using WebApiBooks.Dto.Vinculo;
using WebApiBooks.Models;

namespace WebApiBooks.Dto.Livro;

public class LivroEdicaoDto
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public AutorVinculoDto Autor { get; set; }
}