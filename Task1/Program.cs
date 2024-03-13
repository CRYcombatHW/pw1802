using System.Data.SqlClient;

namespace Task1
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
            Console.WriteLine("Connected succesfuly");

			new SqlCommand(@"IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'Storehouse')
CREATE DATABASE Storehouse", connection).ExecuteNonQuery();
			new SqlCommand(@"USE Storehouse

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'Storage')

CREATE TABLE Storage(
	Id INT PRIMARY Key Identity(1, 1),
	Type nvarchar(Max) NOT NULL,
	Provider nvarchar(Max) NOT NULL,
	Quantity int NOT NULL,
	PrimeCost int NOT NULL,
	OrderDate DATE NOT NULL
)
", connection).ExecuteNonQuery();

			Console.WriteLine("Commands executed succesfuly");
		}
	}
}
