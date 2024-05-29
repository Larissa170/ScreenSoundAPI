using ScreenSound.Modelos;

namespace ScreenSound.Banco;

public class DAL<T> where T : class
{
    private readonly ScreenSoundContext context;

    public DAL(ScreenSoundContext context)
    {
        this.context = context;
    }

    public IEnumerable<T> Listar()
    {
        return context.Set<T>().ToList();
    }
    public void Adicionar(T obj)
    {
        context.Set<T>().Add(obj);
        context.SaveChanges();
    }
    public void Atualizar(T obj)
    {
        context.Set<T>().Update(obj);
        context.SaveChanges();
    }
    public void Deletar(T obj)
    {
        context.Set<T>().Remove(obj);
        context.SaveChanges();

    }

    public T? RecuperarPor(Func<T,bool> condicao)
    {
       return context.Set<T>().FirstOrDefault(condicao); // recupera o primeiro da condicao
    }

    public IEnumerable<T> ListarPor(Func<T,bool> condicao)// lista por uma condicao
    {
        return context.Set<T>().Where(condicao);
    }
}
