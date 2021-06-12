using System;

namespace SamuraiApp.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            AddSamurais("Shimada", "Kikuchio", "Hayashiu");
        }
        
        public static void AddSamurais(params string[] names)
        {
            var bizData = new BizLogicData();
            var samuraisCreated = bizData.AddNewSamuraisByName(names);
        }
    }
}
