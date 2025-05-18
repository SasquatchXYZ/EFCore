namespace ServiceLayer.StatusGeneric;

public interface IStatusGeneric<out T> : IStatusGeneric
{
    T? Result { get; }
}
