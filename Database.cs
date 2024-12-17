using MySql.Data.MySqlClient;
using System;

public class Database {
    private MySqlConnection Connection;
    public Database(string connectionString) {
        Connection = new MySqlConnection(connectionString);

    }
    public void OpenConnection(){
        try{
            Connection.Open();
        }
        catch (Exception ex){
            Console.WriteLine("Error :" + ex.Message);
        }
    }
    public void CloseConnection(){
        try{
            Connection.Close();
        }
        catch (Exception ex){
            Console.WriteLine("Error :" + ex.Message);
        }
    }
    public MySqlConnection GetConnection(){
        return Connection;
    }


}