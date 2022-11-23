namespace HotelListingSql.Core.Exceptions;

public class MyNotFoundException : ApplicationException
{
    public MyNotFoundException(string name, object key) : base($"{name} ({key}) was not found")
    {

    }
}
