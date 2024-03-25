using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2 || !File.Exists(args[0]) || !File.Exists(args[1]))
        {
            Console.WriteLine("Használat: program.exe <Kolcsonzok.csv útvonal> <Kolcsonzesek.csv útvonal>");
            return;
        }

        string connectionString = "Server=localhost;Database=library_db;User Id=yourUsername;Password=yourPassword;";

        int kolcsonzokCount = ImportKolcsonzok(args[0], connectionString);
        Console.WriteLine($"{kolcsonzokCount} sor sikeresen importálva a 'Kolcsonzok' táblába.");

        int kolcsonzesekCount = ImportKolcsonzesek(args[1], connectionString);
        Console.WriteLine($"{kolcsonzesekCount} sor sikeresen importálva a 'Kolcsonzesek' táblába.");
    }

    static int ImportKolcsonzok(string filePath, string connectionString)
    {
        var lines = File.ReadAllLines(filePath);
        var count = 0;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var line in lines)
            {
                var fields = line.Split(';');
                var cmdText = "INSERT INTO Kolcsonzok (Nev, SzuletesiDatum) VALUES (@Nev, @SzuletesiDatum)";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@Nev", fields[0]);
                    command.Parameters.AddWithValue("@SzuletesiDatum", fields[1]);

                    count += command.ExecuteNonQuery();
                }
            }
        }

        return count;
    }

    static int ImportKolcsonzesek(string filePath, string connectionString)
    {
        var lines = File.ReadAllLines(filePath);
        var count = 0;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            foreach (var line in lines)
            {
                var fields = line.Split(';');
                var cmdText = @"INSERT INTO Kolcsonzesek (KolcsonzoId, Iro, Mufaj, Cim) VALUES (@KolcsonzoId, @Iro, @Mufaj, @Cim)";
                using (var command = new SqlCommand(cmdText, connection))
                {
                    command.Parameters.AddWithValue("@KolcsonzoId", fields[0]);
                    command.Parameters.AddWithValue("@Iro", fields[1]);
                    command.Parameters.AddWithValue("@Mufaj", fields[2]);
                    command.Parameters.AddWithValue("@Cim", fields[3]);

                    count += command.ExecuteNonQuery();
                }
            }
        }

        return count;
    }
}
