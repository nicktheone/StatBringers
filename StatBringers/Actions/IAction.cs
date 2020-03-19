namespace StatBringers.Actions
{
    interface IAction
    {
        string Description { get; }

        void Do();
    }
}