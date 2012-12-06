using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public delegate void ComplitedHandler(string key);

    public interface IComplited
    {
        event ComplitedHandler EncryptComplitedEvent;
        event ComplitedHandler DecryptComplitedEvent;
    }
    public abstract class CrypterBlocks
    {

        //public event ComplitedHandler ComplitedEvent;

        public int MaxValueProcess { get; protected set; }

        public int CurrentValueProcess { get; protected set; }

        public abstract string Encrypt(string str, string key);

        public abstract void EncryptAsync(string str, string key);

        public abstract string Decrypt(string str, string key);

        public abstract string DecryptAsync(string str, string key);

        public abstract byte[] Encrypt(byte[] str, string key);

        public abstract byte[] EncryptAsync(byte[] str, string key);

        public abstract byte[] Decrypt(byte[] str, string key);

        public abstract byte[] DecryptAsync(byte[] str, string key);

        public abstract void EncryptAsync(string inputFile, string outputFile, string key);

        public abstract void DecryptAsync(string inputFile, string outputFile, string key);

        public abstract byte[] F(byte[] data, int[] key);

    }
}
