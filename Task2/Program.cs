using System.Data.SqlClient;

namespace Task2
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
		}
	}
}