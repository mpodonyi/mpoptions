namespace MPOptions
{
    public interface IGeneralFlow<T> where T : Command
    {
        T Add(params Option[] options);

        T Add(params Command[] commands);

        T Add(Argument argument);
    }
}