using MySql.Data.MySqlClient;
using System;

public class UserOperations {
    private Database database;

    public UserOperations(Database db) {
        database = db;
    }

    public void RedeemItem(int id, int jumlahItem, string username) {
        try {
            database.OpenConnection();
            
            // Mengecek Item ada atau tidak
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM redeemItem WHERE id = @Id", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Id", id);
            int itemCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (itemCount == 0) {
                Console.WriteLine("Item tidak ditemukan");
                return;
            }
            
            // Mendapatkan detail item
            MySqlCommand getItemCommand = new MySqlCommand("SELECT * FROM redeemItem WHERE id = @Id", database.GetConnection());
            getItemCommand.Parameters.AddWithValue("@Id", id);
            MySqlDataReader reader = getItemCommand.ExecuteReader();
            reader.Read();
            
            int harga = Convert.ToInt32(reader["harga"]);
            int totalHarga = harga * jumlahItem;
            reader.Close(); // Close the reader before executing another command
            
            // Get user details
            MySqlCommand getUserCommand = new MySqlCommand("SELECT * FROM users WHERE username = @Username", database.GetConnection());
            getUserCommand.Parameters.AddWithValue("@Username", username);
            reader = getUserCommand.ExecuteReader();
            if (!reader.Read()) {
                Console.WriteLine("Username tidak ditemukan");
                return;
            }
            
            int shellPoint = Convert.ToInt32(reader["shellPoint"]);
            reader.Close(); // Close the reader before executing another command
            
            if (shellPoint < totalHarga) {
                Console.WriteLine("Shell Point tidak cukup");
                return;
            }
            
            // Update user's shell points
            MySqlCommand updateShellPointCommand = new MySqlCommand("UPDATE users SET shellPoint = shellPoint - @TotalHarga WHERE username = @Username", database.GetConnection());
            updateShellPointCommand.Parameters.AddWithValue("@TotalHarga", totalHarga);
            updateShellPointCommand.Parameters.AddWithValue("@Username", username);
            updateShellPointCommand.ExecuteNonQuery();
            
            // Check item quantity
            MySqlCommand checkItemCommand = new MySqlCommand("SELECT * FROM redeemItem WHERE id = @Id", database.GetConnection());
            checkItemCommand.Parameters.AddWithValue("@Id", id);
            reader = checkItemCommand.ExecuteReader();
            reader.Read();
            int quantity = Convert.ToInt32(reader["quantity"]);
            reader.Close();
            if (quantity < jumlahItem) {
                Console.WriteLine("Item tidak cukup atau habis");
                return;
            }
            
            // Update item quantity
            MySqlCommand updateItemCommand = new MySqlCommand("UPDATE redeemItem SET quantity = quantity - @JumlahItem WHERE id = @Id", database.GetConnection());
            updateItemCommand.Parameters.AddWithValue("@JumlahItem", jumlahItem);
            updateItemCommand.Parameters.AddWithValue("@Id", id);
            updateItemCommand.ExecuteNonQuery();
            
            Console.WriteLine("Item berhasil ditukar");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }
    public string ShowUserPoint (string username) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE username = @Username", database.GetConnection());
            command.Parameters.AddWithValue("@Username", username);
            MySqlDataReader reader = command.ExecuteReader();
            
            if (!reader.HasRows) {
                Console.WriteLine("Username tidak ditemukan");
                return null;
            }
            
            reader.Read();
            return reader["shellPoint"].ToString();
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
            return null;
        } finally {
            database.CloseConnection();
        }
    }
    public void ReadItemRedeem() {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM redeemItem", database.GetConnection());
            MySqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("=========================================");
            Console.WriteLine("            Redeem Item List             ");
            Console.WriteLine("=========================================");
            Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", "ID", "Produk", "Kategori", "Harga", "Quantity");
            Console.WriteLine(new string('-', 65));

            while (reader.Read()) {
                Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", reader["id"], reader["produk"], reader["kategori"], reader["harga"], reader["quantity"]);
            }

            Console.WriteLine(new string('-', 65));
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void AddItemRedeem(string produk, string kategori, int harga, int quantity) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO redeemItem (produk, kategori, harga, quantity) VALUES (@Produk, @Kategori, @Harga, @Quantity)", database.GetConnection());
            command.Parameters.AddWithValue("@Produk", produk);
            command.Parameters.AddWithValue("@Kategori", kategori);
            command.Parameters.AddWithValue("@Harga", harga);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.ExecuteNonQuery();
            Console.WriteLine("Item berhasil ditambahkan");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void ReadItemRedeemByCategory(string kategori) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM redeemItem WHERE kategori = @Kategori", database.GetConnection());
            command.Parameters.AddWithValue("@Kategori", kategori);
            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows) {
                Console.WriteLine("Kategori tidak ditemukan");
                return;
            }

            Console.WriteLine("=========================================");
            Console.WriteLine("         Redeem Item List by Category    ");
            Console.WriteLine("=========================================");
            Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", "ID", "Produk", "Kategori", "Harga", "Quantity");
            Console.WriteLine(new string('-', 65));

            while (reader.Read()) {
                Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", reader["id"], reader["produk"], reader["kategori"], reader["harga"], reader["quantity"]);
            }

            Console.WriteLine(new string('-', 65));
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void ReadItemRedeemByName(string produk) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM redeemItem WHERE produk LIKE @Produk", database.GetConnection());
            command.Parameters.AddWithValue("@Produk", "%" + produk + "%"); // Use wildcards for partial match
            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows) {
                Console.WriteLine("Produk tidak ditemukan");
                return;
            }

            Console.WriteLine("=========================================");
            Console.WriteLine("         Redeem Item List by Name        ");
            Console.WriteLine("=========================================");
            Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", "ID", "Produk", "Kategori", "Harga", "Quantity");
            Console.WriteLine(new string('-', 65));

            while (reader.Read()) {
                Console.WriteLine("{0,-5} | {1,-20} | {2,-15} | {3,-10} | {4,-10}", reader["id"], reader["produk"], reader["kategori"], reader["harga"], reader["quantity"]);
            }

            Console.WriteLine(new string('-', 65));
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void ReadCategoryItems() {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT kategori, COUNT(*) AS jumlah FROM redeemItem GROUP BY kategori", database.GetConnection());
            MySqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("=========================================");
            Console.WriteLine("         Redeem Item Categories          ");
            Console.WriteLine("=========================================");
            Console.WriteLine("{0,-20} | {1,-10}", "Kategori", "Jumlah");
            Console.WriteLine(new string('-', 35));

            while (reader.Read()) {
                Console.WriteLine("{0,-20} | {1,-10}", reader["kategori"], reader["jumlah"]);
            }

            Console.WriteLine(new string('-', 35));
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void UpdateItemRedeem(int id, string produk, string kategori, int harga,int quantity) {
        try {
            database.OpenConnection();
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM redeemItem WHERE id = @Id", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Id", id);
            int itemCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (itemCount == 0) {
                Console.WriteLine("Item tidak ditemukan");
                return;
            }
            
            MySqlCommand command = new MySqlCommand("UPDATE redeemItem SET produk = @Produk, kategori = @Kategori, harga = @Harga,quantity = @Quantity WHERE id = @Id", database.GetConnection());
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Produk", produk);
            command.Parameters.AddWithValue("@Kategori", kategori);
            command.Parameters.AddWithValue("@Harga", harga);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.ExecuteNonQuery();
            Console.WriteLine("Item berhasil diupdate");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void DeleteItemRedeem(int id) {
        try {
            database.OpenConnection();
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM redeemItem WHERE id = @Id", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Id", id);
            int itemCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (itemCount == 0) {
                Console.WriteLine("Item tidak ditemukan");
                return;
            }
            
            MySqlCommand command = new MySqlCommand("DELETE FROM redeemItem WHERE id = @Id", database.GetConnection());
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            Console.WriteLine("Item berhasil dihapus");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public bool? CheckRoleisAdmin(string username) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE username = @Username", database.GetConnection());
            command.Parameters.AddWithValue("@Username", username);
            MySqlDataReader reader = command.ExecuteReader();
            
            if (!reader.HasRows) {
                Console.WriteLine("Username tidak ditemukan");
                return null;
            }

            reader.Read();
            bool isAdmin = reader["role"].ToString() == "admin";
            return isAdmin;
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
            return null;
        } finally {
            database.CloseConnection();
        }
    }

    public bool? LoginUserOperations (string username, string password) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users WHERE username = @Username AND password = @Password", database.GetConnection());
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            MySqlDataReader reader = command.ExecuteReader();
            
            if (!reader.HasRows) {
                Console.WriteLine("Username atau password salah");
                return false;
            }

            reader.Read();
            Console.WriteLine("Login berhasil");
            return true;
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
            return null;
        } finally {
            database.CloseConnection();
        }
    }

    public bool? CreateUser(string username, string email,string password) {
        try {
            database.OpenConnection();
                // check username exists
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @Username", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Username", username);
            int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (userCount > 0) {
                Console.WriteLine("Username sudah ada");
                return false;
            }
            MySqlCommand command = new MySqlCommand("INSERT INTO users (username, email,password) VALUES (@Username, @Email,@Password)", database.GetConnection());
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password",password);
            command.ExecuteNonQuery();
            Console.WriteLine("User berhasil dibuat");
            return true;
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
            return null;
        } finally {
            database.CloseConnection();
        }
    }
   public void ChangePasswordUser(string username, string oldPassword, string newPassword) {
    try {
        database.OpenConnection();

        // Check if the old password is correct
        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @Username AND password = @OldPassword", database.GetConnection());
        checkCommand.Parameters.AddWithValue("@Username", username);
        checkCommand.Parameters.AddWithValue("@OldPassword", oldPassword);
        int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

        if (userCount == 0) {
            Console.WriteLine("Password lama salah");
            return;
        }

        // Update to the new password
        MySqlCommand command = new MySqlCommand("UPDATE users SET password = @NewPassword WHERE username = @Username", database.GetConnection());
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@NewPassword", newPassword);
        command.ExecuteNonQuery();
        Console.WriteLine("Password berhasil diubah");
    } catch (Exception ex) {
        Console.WriteLine("An error occurred: " + ex.Message);
    } finally {
        database.CloseConnection();
    }
}
    public void ReadUsers() {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("SELECT * FROM users", database.GetConnection());
            MySqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("=========================================");
            Console.WriteLine("               User List                 ");
            Console.WriteLine("=========================================");
            Console.WriteLine("{0,-5} | {1,-15} | {2,-25} | {3,-10} | {4,-10}", "ID", "Username", "Email", "Role", "ShellPoint");
            Console.WriteLine(new string('-', 75));

            while (reader.Read()) {
                Console.WriteLine("{0,-5} | {1,-15} | {2,-25} | {3,-10} | {4,-10}", reader["id"], reader["username"], reader["email"], reader["role"], reader["shellPoint"]);
            }

            Console.WriteLine(new string('-', 75));
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void UpdateUser(int id, string username, string email) {
        try {
            database.OpenConnection();
            MySqlCommand command = new MySqlCommand("UPDATE users SET username = @Username, email = @Email WHERE id = @Id", database.GetConnection());
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Email", email);
            command.ExecuteNonQuery();
            Console.WriteLine("User berhasil diupdate");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void AddShellPoint(string username, int shellPoint) {
        try {
            database.OpenConnection();
            
            // Check if the username exists
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @Username", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Username", username);
            int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (userCount == 0) {
                Console.WriteLine("Top Up gagal karena username tidak ditemukan");
                return;
            }
            
            // Update shell points jika username ada
            MySqlCommand command = new MySqlCommand("UPDATE users SET shellPoint = shellPoint + @ShellPoint WHERE username = @Username", database.GetConnection());
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@ShellPoint", shellPoint);
            command.ExecuteNonQuery();
            Console.WriteLine(shellPoint + " Point berhasil ditambahkan ke " + username);
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }

    public void DeleteUser(int id) {
        try {
            database.OpenConnection();
            MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM users WHERE id = @Id", database.GetConnection());
            checkCommand.Parameters.AddWithValue("@Id", id);
            int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());
            
            if (userCount == 0) {
                Console.WriteLine("username tidak ditemukan");
                return;
            }
            
            MySqlCommand command = new MySqlCommand("DELETE FROM users WHERE id = @Id", database.GetConnection());
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            Console.WriteLine("User berhasil dihapus");
        } catch (Exception ex) {
            Console.WriteLine("An error occurred: " + ex.Message);
        } finally {
            database.CloseConnection();
        }
    }
}