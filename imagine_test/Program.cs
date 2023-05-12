namespace imagine_test
{
    public interface IA
    {

    }
    public interface IB
    {

    }

    public class C:IA,IB
    {
        public string Name { get; set; }
        public C(string name)
        {
            Name = name;
        }
        public void ShowName()
        {
            Console.WriteLine(Name);
        }
    }
    internal class Program
    {
        
        static void Main(string[] args)
        {
            IB a = new C("hello");

            var b = a as C;
            b.ShowName();
        }
    }
}