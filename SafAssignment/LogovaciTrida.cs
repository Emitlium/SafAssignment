namespace SafAssignment
{
    public class LoggingOfTests
    {
        // Začínací Log pro [SetUp]
        public static void LogStartOfTest(List<string> f, int i)
        {
            string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {i}. test started.";
            f.Add(log);
        }

        // Konečný Log pro [TearDown]
        public static void LogEndOfTest(List<string> f, int i)
        {
            string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {i}. test finished.";
            f.Add(log);
        }

        // Možnost zapsání logu v [Test]
        public static void DuringTheTest(List<string> f, string cokoliv)
        {
            string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {cokoliv}";
            f.Add(log);
        }

        // Vytvoření souboru v [OneTimeTearDown]
        /*
         * Poznámka: když to běží, tak se to ukládá do:
         * SafAssignment\SafAssignment\bin\Debug\net6.0
         * asi proto, že to běží v ten moment tam
         * já to samozřejmě očekávám v SafAssignment\SafAssignment\, protože tam jsou .cs soubory
         * "..\\..\\.." to posune o tři složky nahoru
         * ...nechtějte vědět, jak dlouho jsem si myslela, že soubor co se celou dobu vytváří, někde mizí
         */
        public static void WriteLogsToFile(List<string> f, string a)
        {
            File.WriteAllLines(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\.."), a), f);
        }
    }
}
