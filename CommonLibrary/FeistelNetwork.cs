using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace CommonLibrary
{
    /// <summary>
    /// Сеть Фейстеля - методов построения блочных шифров
    /// </summary>
    public class FeistelNetwork
    {

        private delegate void CryptFunctionDelegate(string inputFile, string outputFile, string key);
        private CryptFunctionDelegate cfDelegate;

        public int MaxValueProcess { get; private set; }

        public int CurrentValueProcess { get; private set; }

        /// <summary>
        /// Количество иттераций
        /// </summary>
        public byte Rounds { get; private set; }
        /// <summary>
        /// Длина блока
        /// </summary>
        public byte BlockLenth { get; private set; }

        /// <summary>
        /// Создание экземпляра
        /// </summary>
        /// <param name="BlockLenth">Длина блока</param>
        /// <param name="rounds">Количество иттераций</param>
        public FeistelNetwork(byte BlockLenth, byte rounds)
        {
            this.BlockLenth = BlockLenth;
            this.Rounds = rounds;
            cfDelegate = new CryptFunctionDelegate(this.Crypt);
        }

        /// <summary>
        /// Функция преобразования данных, зависимая от ключа 
        /// </summary>
        /// <param name="data">Массив данных</param>
        /// <param name="key">Ключ</param>
        /// <param name="ind"> </param>
        /// <returns></returns>
        /* private void F(ref byte[] data, byte key, int ind)
         {
             for (var i = ind; i < ind + BlockLenth / 2; i++)
                 data[i] = (byte)(data[i] ^ key);
         }

         /// <summary>
         /// Функция ^ (xor) между двумя массивами байтов
         /// </summary>
         /// <param name="a">Массив данных</param>
         /// <param name="b">Массив данных</param>
         /// <returns>a ^ b</returns>
       */
        private void XORk(ref byte[] data, byte[] lk, int indL)
        {
            byte temp;
            for (var i = 0; i < BlockLenth / 2; i++)
            {
                temp = data[i + indL + BlockLenth / 2];
                data[i + indL + BlockLenth / 2] = data[i + indL];
                data[i + indL] = (byte)(temp ^ lk[i]);
            }
        }
        private byte[] F(byte[] data, byte key)
        {
            var clone = (byte[])data.Clone();
            for (var i = 0; i < clone.Length; i++)
                clone[i] = (byte)(data[i] ^ key);
            return clone;
        }
        private byte[] Fk(byte[] data, byte[] keys, int ind)
        {
            var clone = new byte[BlockLenth / 2];
            for (var i = 0; i < BlockLenth / 2 - keys[0]; i++)
                clone[i] = data[ind + i + keys[0]];
            for (var i = 0; i < keys[0]; i++)
                clone[i + BlockLenth / 2 - keys[0]] = data[i + ind];
            return clone;
        }


        private string Fx(string str, byte[] keys)
        {
            var res = "";
            for (var i = 1; i < keys.Length; i++)
                res += str.Substring(keys.Take(i).Sum(a => a), keys[i]);
            res = res + str.Substring(0, keys[0]);
            return res;
        }

        private string XOR(string a, string b)
        {
            var res = "";
            for (var i = 0; i < a.Length; i++)
                res += (a[i] ^ b[i]);
            return res;
        }
        /*
        private byte[] XOR(byte[] a, byte[] b)
        {
            var res = new byte[a.Length];
            for (var i = 0; i < a.Length; i++)
                res[i] = (byte)(a[i] ^ b[i]);
            return res;
        }*/

        /// <summary>
        /// Общая функция преобразования входного массива байтов 
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="reverse">если false - шифрация, true - дешифрация</param>
        /// <param name="key">ключ</param>
        private void Crypt(string inputFile, string outputFile, string key)
        {
            var subblockscount = 3;
            var file = File.ReadAllBytes(inputFile);
            var gen = new CongruentialGenerator(MaHash8v64.GetHashCode(key));
            var keys = new byte[subblockscount];
            for (var i = 0; i < keys.Length - 1; i++)
                keys[i] = (byte)gen.Next(1, BlockLenth / 2 - keys.Sum(a => a) - (subblockscount - i - 1));
            keys[keys.Length - 1] = (byte)(BlockLenth / 2 - keys.Sum(a => a));


            this.MaxValueProcess = (int)(file.Length * Rounds);



            var lenthBlocks = Math.Truncate((double)(file.Length / BlockLenth));
            for (byte k = 0; k < Rounds; k++)
            {
                for (var i = 0; i < lenthBlocks * BlockLenth; i += BlockLenth)
                {
                    XORk(ref file, Fk(file, keys, i), i);
                    this.CurrentValueProcess = i + k * Rounds;
                }
            }
            File.WriteAllBytes(outputFile, file);
        }

        /// <summary>
        /// Шифрование
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="key"> ключ</param>
        public void Encrypt(string inputFile, string outputFile, string key)
        {
            cfDelegate.BeginInvoke(inputFile, outputFile, key, null, null);
        }

        /// <summary>
        /// Дешифрование
        /// </summary>
        /// <param name="file">входной массив байтов</param>
        /// <param name="key">ключ</param>
        public void Decrypt(string inputFile, string outputFile, string key)
        {
            cfDelegate.BeginInvoke(inputFile, outputFile, key, null, null);
        }
    }
}
