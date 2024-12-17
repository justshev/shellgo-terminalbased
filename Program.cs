using System;

class Program {
    static void Main(string[] args) {
        string[] menuAdmin_0501 = { "1. Create User", "2. Hapus Data User", "3. Melihat Data User ","4. Lihat Redeem Item", "5. Tambah Redeem Item", "6. Update Redeem Item", "7. Hapus Redeem Item", "8. Tambah Shell Point", "9. Exit" };
        string[] menuUser_0501 = { "1. Lihat Redeem Item", "2. Cari Item berdasarkan kategori", "3. Cari Item berdasarkan Nama", "4. Tukar Point","5. Ganti Password", "6. Exit" };
        Database db = new Database("Server=localhost;Database=shellredeem;Uid=root;password=Sxhvxa0707");
        UserOperations userOperations = new UserOperations(db);

        Console.Clear();
        Console.WriteLine("=========================================");
        Console.WriteLine("  Selamat Datang di Penukaran Shell Point");
        Console.WriteLine("=========================================");
        Console.WriteLine();
        Console.WriteLine("Sudah punya akun? (y/n)");
        string answer = Console.ReadLine();
       
        while(true ){
            if (answer.ToLower() == "y") {


                 Console.Write("Masukkan username: ");
            string usernameInput_0501 = Console.ReadLine();
            Console.Write("Masukkan password: ");
            
            string passwordInput_0501 = Console.ReadLine();
            while (string.IsNullOrEmpty(usernameInput_0501) || string.IsNullOrEmpty(passwordInput_0501)) {
                Console.WriteLine("Username dan password tidak boleh kosong");
                Console.Write("Masukkan username: ");
                usernameInput_0501 = Console.ReadLine();
                Console.Write("Masukkan password: ");
                passwordInput_0501 = Console.ReadLine();
            }
            bool? isAdmin_0501 = userOperations.CheckRoleisAdmin(usernameInput_0501);
            bool? LoginUser_0501 = userOperations.LoginUserOperations(usernameInput_0501,passwordInput_0501);

          
            while (LoginUser_0501 == false) {

                Console.Write("Masukkan username: ");
                usernameInput_0501 = Console.ReadLine();
                Console.Write("Masukkan password: ");
                passwordInput_0501 = Console.ReadLine();
                isAdmin_0501 = userOperations.CheckRoleisAdmin(usernameInput_0501);
                LoginUser_0501 = userOperations.LoginUserOperations(usernameInput_0501,passwordInput_0501);
            }

            if (isAdmin_0501 == false && LoginUser_0501 == true) {
                DisplayMenuUser_0501(userOperations, usernameInput_0501, menuUser_0501);
            } else if (isAdmin_0501 == true && LoginUser_0501 == true) {
                DisplaymenuAdmin_0501(userOperations, menuAdmin_0501);
            } else {
                return;
            }

            } else if (answer.ToLower() == "n") {
                 Console.Write("Masukkan username: ");
            string username_0501 = Console.ReadLine();
            Console.Write("Masukkan email: ");
            string email_0501 = Console.ReadLine();
            Console.Write("Masukkan password: ");
            string password_0501 = Console.ReadLine();
            while (string.IsNullOrEmpty(username_0501) || string.IsNullOrEmpty(email_0501) || string.IsNullOrEmpty(password_0501)) {
                Console.WriteLine("Username, email, dan password tidak boleh kosong");
                Console.Write("Masukkan username: ");
                username_0501 = Console.ReadLine();
                Console.Write("Masukkan email: ");
                email_0501 = Console.ReadLine();
                Console.Write("Masukkan password: ");
                password_0501 = Console.ReadLine();
            }
            if (username_0501.Length < 3 || username_0501.Length > 100) {
                Console.WriteLine("Username tidak boleh kurang dari 3 karakter dan lebih dari 100 karakter");
                return;
            }
            if (password_0501.Length < 3 ) {
                Console.WriteLine("Password tidak boleh kurang dari 3 karakter");
                return;
            }
            if (userOperations.CreateUser(username_0501, email_0501, password_0501) == true) {
                Console.WriteLine("User berhasil dibuat");
                if (userOperations.CheckRoleisAdmin(username_0501) == false) {
                    DisplayMenuUser_0501(userOperations, username_0501, menuUser_0501);
                } else {
                    DisplaymenuAdmin_0501(userOperations, menuAdmin_0501);
                }
            } else {
                Console.WriteLine("User gagal dibuat");
            }

            } else {
                Console.WriteLine("Jawaban tidak valid");
                Console.WriteLine("Sudah punya akun? (y/n)");
                answer = Console.ReadLine();
            }
        }
   
    }

    static void DisplayMenuUser_0501(UserOperations userOperations, string usernameInput_0501, string[] menuUser) {
        while (true) {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("       Selamat Datang User Shell Point");
            Console.WriteLine("=========================================");
            Console.WriteLine("Menu:");
            foreach (string menu in menuUser) {
                Console.WriteLine(menu);
            }
            Console.WriteLine("=========================================");
            Console.Write("Masukan pilihan kamu: ");
            string choice = Console.ReadLine();
            switch (choice) {
                case "1":
                    userOperations.ReadItemRedeem();
                    break;
                case "2":
                    Console.WriteLine("Kategori yang tersedia: ");
                    userOperations.ReadCategoryItems();
                    Console.Write("Cari ketegori item: ");
                    string namaItem_0501 = Console.ReadLine();
                    userOperations.ReadItemRedeemByCategory(namaItem_0501);
                    break;
                case "3":
                    Console.Write("Cari nama item: ");
                    string namaItem2_0501 = Console.ReadLine();
                    userOperations.ReadItemRedeemByName(namaItem2_0501);
                    break;
                case "4":
                    userOperations.ReadItemRedeem();
                    string userPoints_0501 = userOperations.ShowUserPoint(usernameInput_0501);
                    Console.WriteLine("Point yang kamu miliki: " + userPoints_0501);
                    Console.WriteLine("=========================================");
                    Console.WriteLine("Tekan tombol Back untuk balik ke menu... dan tekan tombol apapun untuk melanjutkan");
                   if (Console.ReadKey().Key == ConsoleKey.Backspace) {
                        break;
                    }

                    Console.Write("Masukan id item: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) {
                        Console.WriteLine("Input id item harus berupa angka dan tidak boleh kosong.");
                    break;
                    }
                    Console.Write("Masukan jumlah item: ");
                    if (!int.TryParse(Console.ReadLine(), out int jumlahItem)) {
                        Console.WriteLine("Input jumlah item harus berupa angka dan tidak boleh kosong.");
break;
                    }
                    userOperations.RedeemItem(id, jumlahItem, usernameInput_0501);
                    break;
                case "5":
                    Console.Clear();
                    Console.Write("Masukkan password lama: ");
                    string oldPassword_0501 = Console.ReadLine();
                    Console.Write("Masukkan password baru: ");
                    string newPassword_0501 = Console.ReadLine();
                    if (string.IsNullOrEmpty(oldPassword_0501) || string.IsNullOrEmpty(newPassword_0501)) {
                        Console.WriteLine("Password lama dan password baru tidak boleh kosong");
                        break;
                    }
                    if (newPassword_0501.Length < 3) {
                        Console.WriteLine("Password baru tidak boleh kurang dari 3 karakter");
                        break;
                    }
                    userOperations.ChangePasswordUser(usernameInput_0501, oldPassword_0501, newPassword_0501);
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak ditemukan");
                    break;
            }
            Console.WriteLine("Tekan sembarang tombol untuk melanjutkan...");
            Console.ReadKey();
        }
    }

    static void DisplaymenuAdmin_0501(UserOperations userOperations, string[] menuAdmin_0501) {
        while (true) {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("       Selamat Datang Admin Shell Point");
            Console.WriteLine("=========================================");
            Console.WriteLine("Menu:");
            foreach (string menu in menuAdmin_0501) {
                Console.WriteLine(menu);
            }
            Console.WriteLine("=========================================");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            switch (choice) {
                case "1":
                    Console.Clear();
                    Console.Write("Masukan username: ");
                    string username_0501 = Console.ReadLine();
                    Console.Write("Masukan email: ");
                    string email_0501 = Console.ReadLine();
                    if (string.IsNullOrEmpty(username_0501) || string.IsNullOrEmpty(email_0501)) {
                        Console.WriteLine("Username dan email tidak boleh kosong");
                        break;
                    }
                    Console.Write("Masukan password: ");
                    string password_0501 = Console.ReadLine();
                    if (string.IsNullOrEmpty(password_0501)) {
                        Console.WriteLine("Password tidak boleh kosong");
                        break;
                    }
                    userOperations.CreateUser(username_0501, email_0501,password_0501);
                  
                    break;
                    case "2":
                    Console.Clear();

                    userOperations.ReadUsers();
                    Console.Write("Masukan id yang mau dihapus : ");
                    if (!int.TryParse(Console.ReadLine(), out int idUserDelete_0501)) {
                        Console.WriteLine("id harus berupa angka dan tidak boleh kosong.");
                        break;
                    }

                    userOperations.DeleteUser(idUserDelete_0501);
                    break;
                case "3":
                    userOperations.ReadUsers();
                    break;

                    
                case "4":
                    userOperations.ReadItemRedeem();
                    break;
                case "5":
                    Console.Clear();
                    Console.WriteLine("Add Redeem Item");
                    Console.Write("Masukan Nama Item: ");
                    string namaItem_0501 = Console.ReadLine();
                    Console.Write("Masukan Kategori Item : ");
                    string kategoriItem_0501 = Console.ReadLine().ToLower();
                    if (string.IsNullOrEmpty(namaItem_0501) || string.IsNullOrEmpty(kategoriItem_0501)) {
                        Console.WriteLine("Nama item dan kategori item tidak boleh kosong");
                        break;
                    }
                    if (namaItem_0501.Length < 3 || namaItem_0501.Length > 100) {
                        Console.WriteLine("Nama item tidak boleh kurang dari 3 karakter dan lebih dari 100 karakter");
                        break;
                    }
                    if (kategoriItem_0501.Length < 3 || kategoriItem_0501.Length > 100) {
                        Console.WriteLine("Kategori item tidak boleh kurang dari 3 karakter dan lebih dari 100 karakter");
                        break;
                    }
                    Console.Write("Masukan Point Item : ");
                    if (!int.TryParse(Console.ReadLine(), out int pointItem)) {
                        Console.WriteLine("Point item harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    Console.WriteLine("Masukan Quantity Item : ");
                    if (!int.TryParse(Console.ReadLine(), out int quantityItem)) {
                        Console.WriteLine("Quantity item harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    if (pointItem < 0) {
                        Console.WriteLine("Point item tidak boleh dibawah 0");
                        break;
                    }
                    if (quantityItem < 0) {
                        Console.WriteLine("Quantity item tidak boleh kosong atau kurang dari 0");
                        break;
                    }
                    userOperations.AddItemRedeem(namaItem_0501, kategoriItem_0501, pointItem, quantityItem);
                    break;
                case "6":
                    Console.Clear();
                    Console.WriteLine("Update Redeem Item");
                    userOperations.ReadItemRedeem();
                    Console.Write("Masukan id item: ");
                    if (!int.TryParse(Console.ReadLine(), out int id_0501)) {
                        Console.WriteLine("id harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    Console.Write("Update Nama Produk: ");
                    string namaProdukUpdate_0501 = Console.ReadLine();
                    
                    Console.Write("Update Kategori Produk: ");
                    string kategoriUpdate_0501 = Console.ReadLine();
                    if (string.IsNullOrEmpty(namaProdukUpdate_0501) || string.IsNullOrEmpty(kategoriUpdate_0501)) {
                        Console.WriteLine("nama produk dan kategori gaboleh kosong");
                        break;
                    }
                    if (namaProdukUpdate_0501.Length < 3 || namaProdukUpdate_0501.Length > 100) {
                        Console.WriteLine("Nama item tidak boleh kurang dari 3 karakter dan lebih dari 100 karakter");
                        break;
                    }
                    if (kategoriUpdate_0501.Length < 3 || kategoriUpdate_0501.Length > 100) {
                        Console.WriteLine("Kategori item tidak boleh kurang dari 3 karakter dan lebih dari 100 karakter");
                        break;
                    }
                    Console.Write("Point Produk: ");
                    if (!int.TryParse(Console.ReadLine(), out int pointUpdate_0501)) {
                        Console.WriteLine("Point item harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    if (pointUpdate_0501 < 0) {
                        Console.WriteLine("Point item tidak boleh dibawah 0");
                        break;
                    }
                     Console.Write("Quantity Produk: ");
                    if (!int.TryParse(Console.ReadLine(), out int quantity_0501)) {
                        Console.WriteLine("Quantity item harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    if (quantity_0501 < 0) {
                        Console.WriteLine("Quantity item tidak boleh dibawah 0");
                        break;
                    }
                    userOperations.UpdateItemRedeem(id_0501, namaProdukUpdate_0501, kategoriUpdate_0501, pointUpdate_0501,quantity_0501);
                    break;
                case "7":
                    Console.Clear();
                    userOperations.ReadItemRedeem();
                    Console.Write("Enter id: ");
                    if (!int.TryParse(Console.ReadLine(), out int idDelete_0501)) {
                        Console.WriteLine("id harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    userOperations.DeleteItemRedeem(idDelete_0501);
                    break;
                case "8":
                    Console.Clear();
                    Console.WriteLine("Add Shell Point");
                    Console.Write("Masukan Username: ");
                    string usernamePoint_0501 = Console.ReadLine();
                    if (string.IsNullOrEmpty(usernamePoint_0501)) {
                        Console.WriteLine("username tidak boleh kosong");
                        break;
                    }
                    Console.Write("Jumlah Pembelian User : ");
                    if (!int.TryParse(Console.ReadLine(), out int jumlahPembelian_0501)) {
                        Console.WriteLine("jumlah pembelian harus berupa angka dan tidak boleh kosong.");
                        break;
                    }
                    if (jumlahPembelian_0501 < 0) {
                        Console.WriteLine("Jumlah pembelian tidak boleh dibawah 0");
                        break;
                    }
                    double RoundedPoint = jumlahPembelian_0501 / 15000;
                    double shellpoint = Math.Round(RoundedPoint, MidpointRounding.ToEven);
                    if (RoundedPoint < 2) {
                        shellpoint = 1.0;
                    }
                    userOperations.AddShellPoint(usernamePoint_0501, (int)shellpoint);
                    break;
                case "9":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak ditemukan");
                    break;
            }
            Console.WriteLine("Tekan sembarang tombol untuk melanjutkan...");
            Console.ReadKey();
        }
    }
}