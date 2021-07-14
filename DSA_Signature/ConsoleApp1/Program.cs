using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Укажите файл по которому будет строиться ЭЦП: ");
            string path = Console.ReadLine();
            string info = File.ReadAllText(path);

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            // Create a new instance of DSACryptoServiceProvider
            DSACryptoServiceProvider DSA = new DSACryptoServiceProvider();

            // Convert string to array of bytes
            byte[] text = Encoding.Unicode.GetBytes(info.ToCharArray());
            // Get hash code 
            byte[] textHash = new SHA1Managed().ComputeHash(text);


            // Create an DSASignatureFormatter object and pass it the 
            // DSACryptoServiceProvider to transfer the private key.
            DSASignatureFormatter DSAFormatter = new DSASignatureFormatter(DSA);
            // Set the hash algorithm
            DSAFormatter.SetHashAlgorithm("SHA1");
            // Create a signature for textHash
            byte[] signedHashValue = DSAFormatter.CreateSignature(textHash);
            
            // Convert array of bytes to string 
            string signed = BitConverter.ToString(signedHashValue);
            // Output signature

            Console.WriteLine("Укажите файл для сохранения ЭЦП: ");
            path = Console.ReadLine();
            File.WriteAllText(path, signed);

            //Проверка
            Console.WriteLine("Укажите файл для проверки ЭЦП: ");
            path = Console.ReadLine();
            info = File.ReadAllText(path);
            text = Encoding.Unicode.GetBytes(info.ToCharArray());
            textHash = new SHA1Managed().ComputeHash(text);


            DSAFormatter.SetHashAlgorithm("SHA1");
            DSASignatureDeformatter dsaDeformatter = new DSASignatureDeformatter(DSA);
            if (dsaDeformatter.VerifySignature(textHash, signedHashValue))
            {
                Console.WriteLine("The signature was verified");
            }
            else
            {
                Console.WriteLine("The signature was not verified");
            }

            Console.ReadLine();

        }
    }
}
