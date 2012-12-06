﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public abstract class CrypterBlocks
    {

        public delegate void ComplitedHandler(string password);
        public delegate void ComplitedStringHandler(string result);
        public delegate void ComplitedBytesHandler(byte[] result);

        public event ComplitedStringHandler EncryptStringComplitedEvent;
        public event ComplitedStringHandler DecryptStringComplitedEvent;
        public event ComplitedBytesHandler EncryptBytesComplitedEvent;
        public event ComplitedBytesHandler DecryptBytesComplitedEvent;
        public event ComplitedHandler DecryptComplitedEvent;
        public event ComplitedHandler EncryptComplitedEvent;


        protected delegate void CryptFunctionDelegate(string inputFile, string outputFile, bool reverse);
        protected delegate void CryptStringFunctionDelegate(ref string resstr);
        protected delegate void CryptBytesFunctionDelegate(byte[] data);

        public int MaxValueProcess { get; protected set; }

        public int CurrentValueProcess { get; protected set; }

        public string Password { get; protected set; }

        /// <summary>
        /// Основная функция изменения блока данных по данному методу
        /// </summary>
        /// <param name="isEncrypt">если true - шифрация, false - дешифрация</param>
        /// <param name="buffer">массив данных</param>
        /// <returns></returns>
        protected abstract byte[] CryptBlock(bool isEncrypt, byte[] buffer);

        protected abstract void Crypt(string inputFile, string outputFile, bool isEncrypt);

        protected CryptStringFunctionDelegate GetCryptStringDelegate(bool isEncrypt)
        {
            return new CryptStringFunctionDelegate(delegate(ref string strr)
            {
                strr = Encrypt(strr);
            });
        }

        protected CryptBytesFunctionDelegate GetCryptBytesDelegate(bool isEncrypt)
        {
            return new CryptBytesFunctionDelegate(delegate(byte[] data)
            {
                Encrypt(data);
            });
        }

        public virtual void EncryptAsync(string inputFile, string outputFile)
        {
            IAsyncResult res = null;
            var cfDelegate = new CryptFunctionDelegate(this.Crypt);
            res = cfDelegate.BeginInvoke(inputFile, outputFile, true, delegate
            {
                cfDelegate.EndInvoke(res);
                if (EncryptComplitedEvent != null)
                    EncryptComplitedEvent(Password);
            }, null);
        }

        public virtual void DecryptAsync(string inputFile, string outputFile)
        {
            IAsyncResult res = null;
            var cfDelegate = new CryptFunctionDelegate(this.Crypt);
            res = cfDelegate.BeginInvoke(inputFile, outputFile, false, delegate
            {
                cfDelegate.EndInvoke(res);
                if (DecryptComplitedEvent != null)
                    DecryptComplitedEvent(Password);
            }, null);
        }


        public virtual string Encrypt(string str)
        {
            var data = Encoding.Unicode.GetBytes(str);
            var res = CryptBlock(true, data);
            return Encoding.Unicode.GetString(res);
        }

        public virtual void EncryptAsync(string str)
        {
            IAsyncResult res = null;
            var resstr = str + "";

            var deleg = GetCryptStringDelegate(true);

            res = deleg.BeginInvoke(ref resstr, delegate
            {
                deleg.EndInvoke(ref resstr, res);
                if (EncryptStringComplitedEvent != null)
                    EncryptStringComplitedEvent(resstr);
            }, null);
        }

        public virtual string Decrypt(string str)
        {
            var data = Encoding.Unicode.GetBytes(str);
            var res = CryptBlock(true, data);
            return Encoding.Unicode.GetString(res);
        }

        public virtual void DecryptAsync(string str)
        {
            IAsyncResult res = null;
            var resstr = str + "";

            var deleg = GetCryptStringDelegate(false);

            res = deleg.BeginInvoke(ref resstr, delegate
            {
                deleg.EndInvoke(ref resstr, res);
                if (DecryptStringComplitedEvent != null)
                    DecryptStringComplitedEvent(resstr);
            }, null);
        }

        public virtual byte[] Encrypt(byte[] data)
        {
            var clone = (byte[])data.Clone();
            return CryptBlock(true, clone);
        }

        public virtual void EncryptAsync(byte[] data)
        {
            IAsyncResult res = null;

            var deleg = GetCryptBytesDelegate(true);
            var clone = (byte[])data.Clone();
            res = deleg.BeginInvoke(clone, delegate
            {
                deleg.EndInvoke(res);
                if (EncryptBytesComplitedEvent != null)
                    EncryptBytesComplitedEvent(clone);
            }, null);
        }

        public virtual byte[] Decrypt(byte[] data)
        {
            var clone = (byte[])data.Clone();
            return CryptBlock(false, clone);
        }

        public virtual void DecryptAsync(byte[] data)
        {
            IAsyncResult res = null;

            var deleg = GetCryptBytesDelegate(false);
            var clone = (byte[])data.Clone();
            res = deleg.BeginInvoke(clone, delegate
            {
                deleg.EndInvoke(res);
                if (DecryptBytesComplitedEvent != null)
                    DecryptBytesComplitedEvent(clone);
            }, null);
        }


    }
}
