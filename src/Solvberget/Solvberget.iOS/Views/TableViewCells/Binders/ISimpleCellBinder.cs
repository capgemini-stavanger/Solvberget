namespace Solvberget.iOS
{
    public interface ISimpleCellBinder<T> where T : class
    {
        void Bind(SimpleCell cell, T model);
    }
}
