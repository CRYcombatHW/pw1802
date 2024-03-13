using System.Data.SqlClient;

namespace Task4
{
	internal class Program
	{
		static void Main(string[] args) {
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

			Console.WriteLine(@"1 - Показати товари заданої категорії
2 - Показати товари заданого постачальника
3 - Показати товар, який знаходиться на складі найдовше з усіx
4 - Показати середню кількість товарів за кожним типом товару");
			switch (Console.ReadKey(true).KeyChar) {
				case '1':
					Console.WriteLine("Type to display: ");
					ShowAll(connection, $"Type = '{Console.ReadLine()}'");
					break;
				case '2':
					Console.WriteLine("Provider to display: ");
					ShowAll(connection, $"Provider = '{Console.ReadLine()}'");
					break;
				case '3':
					ShowMinMaxAvg(connection, MinMaxAvg.Min, "OrderDate");
					break;
				case '4':
					ShowMinMaxAvgOf(connection, MinMaxAvg.Avg, "Quantity", "Type");
					break;
			}
		}

		private static void ShowAll(SqlConnection connection, string? where = null) {
			using SqlCommand command = new SqlCommand(@$"USE Storehouse
SELECT * FROM Storage {((where is null) ? ("") : ($"WHERE {where}"))}
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

		enum MinMaxAvg { Min, Max, Avg }
		private static void ShowMinMaxAvg(SqlConnection connection, MinMaxAvg function, string of) {
			using SqlCommand command = new SqlCommand(@$"USE Storehouse
SELECT *
FROM Storage
WHERE {of} = (
    SELECT {function}({of})
    FROM Storage
)
", connection);
			command.ExecuteNonQuery();

			using SqlDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				WriteAll(reader);
			}
		}
		private static void ShowMinMaxAvgOf(SqlConnection connection, MinMaxAvg function, string name, string of) {
			using SqlCommand command = new SqlCommand(@$"USE Storehouse
SELECT {function}({name}) as {name}
FROM Storage
WHERE {of} = (
    SELECT DISTINCT {of}
    FROM Storage
)
", connection);
			command.ExecuteNonQuery();

			using SqlDataReader reader = command.ExecuteReader();
			while (reader.Read()) {
				Console.WriteLine(reader[name]);
            }
		}
	}
}