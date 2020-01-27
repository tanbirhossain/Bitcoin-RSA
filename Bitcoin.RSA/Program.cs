using NBitcoin;
using System;

namespace Bitcoin.RSA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var wallet = RSA.KeyGenerate();
            Console.WriteLine($"Public Key : {wallet.PublicKey}");
            Console.WriteLine($"Private Key : {wallet.PrivateKey}");

            var message = "Chill message";
            var signData = RSA.Sign(wallet.PrivateKey, message);
            var IsOrginalData = RSA.Verify(wallet.PublicKey, message, signData);
            Console.ReadKey();

        }
    }

    public static class RSA
    {
        public static Wallet KeyGenerate()
        {
            Key privateKey = new Key();
            var v = privateKey.GetBitcoinSecret(Network.Main).GetAddress();
            var address = BitcoinAddress.Create(v.ToString(), Network.Main);

            return new Wallet { PublicKey = v.ToString(), PrivateKey = privateKey.GetBitcoinSecret(Network.Main).ToString() };

        }
        public static string Sign(string privateKey, string msgToSign)
        {
            var secret = Network.Main.CreateBitcoinSecret(privateKey);
            var singnature = secret.PrivateKey.SignMessage(msgToSign);
            var v = secret.PubKey.VerifyMessage(msgToSign, singnature);
            return singnature;
        }

        public static bool Verify(string publicKey, string originalMessage, string signedMessage)
        {
            var address = BitcoinAddress.Create(publicKey, Network.Main);
            var pkh = (address as IPubkeyHashUsable);
            var bol = pkh.VerifyMessage(originalMessage, signedMessage);
            return bol;
        }
    }


}

public class Wallet
{
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}