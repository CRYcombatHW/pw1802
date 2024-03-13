using System.Data.SqlClient;
using System.Text;

namespace Task3
{
	internal class Program
	{
		static void Main(string[] args) {
			Console.OutputEncoding = Encoding.Unicode;
			
			string connectionStr = @"Data Source=CRYCOMBATPC\MSSQL2019;Integrated Security=True;Connect Timeout=30;";
			using SqlConnection connection = new SqlConnection(connectionStr);

			Console.WriteLine("Connecting...");
			try {
				connection.Open();
			}
			catch (Exception x) {
				Console.WriteLine("Cant connect");
				return;
			}
			Console.WriteLine("Connected to server");

			try {
				new SqlCommand("USE Storehouse", connection).ExecuteNonQuery();
			}
			catch {
				Console.WriteLine("Cant connect to Storehouse database");
				return;
			}

			Console.WriteLine("Connected to Storehouse database");

            Console.WriteLine(@"1 Відображення всієї інформації про товар
2 Відображення всіх типів товарів
3 Відображення всіх постачальників
4 Показати товар з максимальною кількістю
5 Показати товар з мінімальною кількістю
6 Показати товар з мінімальною собівартістю
7 Показати товар з максимальною собівартістю
");
			switch (Console.ReadKey(true).KeyChar) {
				case '1':
					ShowAll(connection);
					break;
				case '2':
					ShowDistinct(connection, "Type");
					break;
				case '3':
					ShowDistinct(connection, "Provider");
					break;
				case '4':
					ShowMinMaxAvg(connection, MinMaxAvg.Max, "Quantity");
					break;
				case '5':
					ShowMinMaxAvg(connection, MinMaxAvg.Min, "Quantity");
					break;
				case '6':
					ShowMinMaxAvg(connection, MinMaxAvg.Min, "PrimeCost");
					break;
				case '7':
					ShowMinMaxAvg(connection, MinMaxAvg.Max, "PrimeCost");
					break;
			}
        }

		private static void ShowAll(SqlConnection connection) {
			using SqlCommand command = new SqlCommand(@"USE Storehouse
SELECT * FROM Storage
", connection);
			command.ExecuteNonQuery();

			using SqlDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				WriteAll(reader);
			}
		}

		private static void ShowDistinct(SqlConnection connection, string distinct) {
			using SqlCommand command = new SqlCommand(@$"USE Storehouse
SELECT DISTINCT {distinct} FROM Storage
", connection);
			command.ExecuteNonQuery();

			using SqlDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				Console.WriteLine($"{reader[distinct]}");
			}
		}

		enum MinMaxAvg { Min, Max, Avg }
		private static void ShowMinMaxAvg(SqlConnection connection, MinMaxAvg function, string maxof) {
			using SqlCommand command = new SqlCommand(@$"USE Storehouse

SELECT *
FROM Storage
WHERE {maxof} = (
    SELECT {function}({maxof})
    FROM Storage
)
", connection);
			command.ExecuteNonQuery();

			using SqlDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				WriteAll(reader);
			}
		}

		private static void WriteAll(SqlDataReader reader) {
			Console.Write($"{reader["Id"]}: ");
			Console.Write($"{reader["Name"]} | ");
			Console.Write($"{reader["Type"]} | ");
			Console.Write($"{reader["Provider"]} | ");
			Console.Write($"{reader["Quantity"]} | ");
			Console.Write($"{reader["PrimeCost"]} | ");
			Console.WriteLine($"{reader["OrderDate"]} | ");
		}
	}
}
