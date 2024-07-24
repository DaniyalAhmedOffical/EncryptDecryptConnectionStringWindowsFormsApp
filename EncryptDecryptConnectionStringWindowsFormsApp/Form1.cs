using System; // System namespace ka istemal ho raha hai jo basic functions provide karta hai
using System.Security.Cryptography; // Cryptographic services ke liye namespace
using System.Text; // Text encoding ke liye namespace
using System.Windows.Forms; // Windows Forms Application ke liye namespace

namespace EncryptDecryptConnectionStringWindowsFormsApp
{
    public partial class Form1 : Form
    {
        // Encryption key define kar rahe hain, key ki length 16, 24 ya 32 characters honi chahiye AES ke liye
        private readonly string key = "32"; // "your-encryption-key"; 

        public Form1()
        {
            InitializeComponent(); // Form ko initialize karta hai
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string connectionString = txtInput.Text; // User se input liya ja raha hai
            string encryptedString = Encrypt(connectionString, key); // Input ko encrypt kar rahe hain
            txtOutput.Text = encryptedString; // Encrypted string ko output textbox mein display kar rahe hain
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string encryptedString = txtInput.Text; // User se input liya ja raha hai (encrypted string)
            string decryptedString = Decrypt(encryptedString, key); // Encrypted string ko decrypt kar rahe hain
            txtOutput.Text = decryptedString; // Decrypted string ko output textbox mein display kar rahe hain
        }

        // Encrypt method ko define kar rahe hain
        public static string Encrypt(string plainText, string key)
        {
            byte[] iv = new byte[16]; // Initialization vector (IV) 16 bytes ka
            using (Aes aes = Aes.Create()) // AES object create kar rahe hain
            {
                aes.Key = Encoding.UTF8.GetBytes(key); // Key ko UTF8 bytes mein convert kar rahe hain
                aes.IV = iv; // IV set kar rahe hain

                // Encryptor create kar rahe hain AES key aur IV se
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // MemoryStream ka use kar ke data ko memory mein store kar rahe hain
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    // CryptoStream ka use kar rahe hain encryption ke liye
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // StreamWriter ka use kar rahe hain data likhne ke liye
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(cs))
                        {
                            sw.Write(plainText); // Plain text ko write kar rahe hain
                        }
                        return Convert.ToBase64String(ms.ToArray());
                        // Encrypted bytes ko Base64 string mein convert kar ke return kar rahe hain
                    }
                }
            }
        }

        // Decrypt method ko define kar rahe hain
        public static string Decrypt(string cipherText, string key)
        {
            byte[] iv = new byte[16]; // Initialization vector (IV) 16 bytes ka
            byte[] buffer = Convert.FromBase64String(cipherText); 
            // Cipher text ko Base64 se bytes mein convert kar rahe hain

            using (Aes aes = Aes.Create()) // AES object create kar rahe hain
            {
                aes.Key = Encoding.UTF8.GetBytes(key); // Key ko UTF8 bytes mein convert kar rahe hain
                aes.IV = iv; // IV set kar rahe hain
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV); // Decryptor create kar rahe hain AES key aur IV se

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(buffer)) // MemoryStream ka use kar rahe hain encrypted data ko memory mein rakhne ke liye
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) // CryptoStream ka use kar rahe hain decryption ke liye
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(cs)) // StreamReader ka use kar rahe hain decrypted data ko read karne ke liye
                        {
                            return sr.ReadToEnd(); // Decrypted text ko read karke return kar rahe hain
                        }
                    }
                }
            }
        }
    }
}
