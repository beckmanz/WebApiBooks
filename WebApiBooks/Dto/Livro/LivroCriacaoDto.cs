using WebApiBooks.Dto.Vinculo;
using WebApiBooks.Models;

namespace WebApiBooks.Dto.Livro;

public class LivroCriacaoDto
{
    public string Titulo { get; set; }
    public AutorVinculoDto Autor { get; set; }
}