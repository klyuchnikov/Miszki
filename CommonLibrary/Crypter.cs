using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public abstract class CrypterBlocks
    {
        protected delegate void ComplitedHandler(string result, string key);
        protected event ComplitedHandler Complited;

        protected int MaxValueProcess { get; private set; }

        protected int CurrentValueProcess { get; private set; }

        protected virtual string Encrypt(string str, string key);

        protected virtual void EncryptAsync(string str, string key);

        protected virtual string Decrypt(string str, string key);

        protected virtual string DecryptAsync(string str, string key);

        protected virtual byte[] Encrypt(byte[] str, string key);

        protected virtual byte[] EncryptAsync(byte[] str, string key);

        protected virtual byte[] Decrypt(byte[] str, string key);

        protected virtual byte[] DecryptAsync(byte[] str, string key);

        protected virtual void EncryptAsync(string inputFile, string outputFile, string key);

        protected virtual void DecryptAsync(string inputFile, string outputFile, string key);

        protected virtual byte[] F(byte[] data, int[] key);

    }
}
