namespace _Client
{
    public class Markers<TC1> : IMarks where TC1 : IComponent, new()
    {
        public void AddMarks(Entity entity)
        {
            entity.GetOrAddComponent<TC1>();
        }
    }
    
    public class Markers<TC1, TC2> : IMarks
        where TC1 : IComponent, new()
        where TC2 : IComponent, new()
    {
        public void AddMarks(Entity entity)
        {
            entity.GetOrAddComponent<TC1>();
            entity.GetOrAddComponent<TC2>();
        }
    }
}