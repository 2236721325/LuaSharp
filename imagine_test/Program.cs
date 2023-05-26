using System.Security.Cryptography.X509Certificates;

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
            //IB a = new C("hello");

            //var b = a as C;
            //b.ShowName();

            var list = new List<int>()
            {
                1,2,3,4,56
            };
            var list2 = new int[100];

            list.CopyTo(list2);
            foreach (var item in list2 )
            {
                list.Remove(item);
            }



            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

        }
    }
}