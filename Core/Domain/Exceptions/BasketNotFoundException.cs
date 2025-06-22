using Domain.Exceptions;

namespace Domain.ExceptionsAdd
{
    public class BasketNotFoundException(string basketKey)
                : NotFoundException($"Basket With This Id : {basketKey}  is Not Found")
    {
    }
}