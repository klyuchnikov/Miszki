using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace CommonLibrary
{
    /// <summary>
    /// Сеть Фейстеля - методов построения блочных шифров
    /// </summary>
    public class FeistelNetwork : CrypterBlocks
    {

        /// <summary>
        /// Количество иттераций
        /// </summary>
        public byte Rounds { get; private set; }

        /// <summary>
        /// Создание экземпляра
        /// </summary>
        /// <param name="BlockLenth">Длина блока</param>
        /// <param name="rounds">Количество иттераций</param>
        public FeistelNetwork(byte BlockLenth, byte subBlocks, string pass, byte rounds)
            : base(BlockLenth, subBlocks, pass)
        {
            this.Rounds = rounds;
        }

        protected override void GenKeys(string key)
        {
            var gen = new CongruentialGenerator(MaHash8v64.GetHashCode(key));
            keys = new byte[this.SubBlocks];
            for (var i = 0; i < keys.Length - 1; i++)
                keys[i] = (byte)gen.Next(1, this.BlockLenth / 2 - keys.Sum(a => a) - (this.SubBlocks - i - 1));
            keys[keys.Length - 1] = (byte)(this.BlockLenth / 2 - keys.Sum(a => a));
        }

        /// <summary>
        /// Функция преобразования данных, сеть Фейстеля, изменненной левой ^ правой части и присваивание к левой части
        /// </summary>
        /// <param name="data">массив данных</param>
        /// <param name="lk">преобразованный массив левой части</param>
        /// <param name="indL">индекс начала</param>
        private void XORl(ref byte[] data, byte[] lk, int indL)
        {
            byte temp;
            for (var i = 0; i < BlockLenth / 2; i++)
            {
                temp = data[i + indL + BlockLenth / 2];
                data[i + indL + BlockLenth / 2] = data[i + indL];
                data[i + indL] = (byte)(temp ^ lk[i]);
            }
        }

        /// <summary>
        /// Функция преобразования данных, сеть Фейстеля, изменненной левой ^ правой части и присваивание к правой части
        /// </summary>
        /// <param name="data">массив данных</param>
        /// <param name="lk">преобразованный массив левой части</param>
        /// <param name="indL">индекс начала</param>
        private void XORr(ref byte[] data, byte[] lk, int indL)
        {
            for (var i = 0; i < BlockLenth / 2; i++)
                data[i + indL + BlockLenth / 2] = (byte)(data[i + indL + BlockLenth / 2] ^ lk[i]);
        }

        /// <summary>
        /// Функция изменения блока данных - перестановка 
        /// </summary>
        /// <param name="data">массив данных</param>
        /// <param name="keys"массив ключей></param>
        /// <param name="ind">индекс начала</param>
        /// <returns></returns>
        private byte[] F(byte[] data, int ind)
        {
            var clone = new byte[BlockLenth / 2];
            for (var i = 0; i < BlockLenth / 2 - keys[0]; i++)
                clone[i] = data[ind + i + keys[0]];
            for (var i = 0; i < keys[0]; i++)
                clone[i + BlockLenth / 2 - keys[0]] = data[i + ind];
            return clone;
        }


        /// <summary>
        /// Основная функция изменения блока данных по данному методу
        /// </summary>
        /// <param name="isEncrypt">если true - шифрация, false - дешифрация</param>
        /// <param name="buffer">массив данных</param>
        /// <returns></returns>
        protected override byte[] CryptBlock(bool isEncrypt, byte[] buffer)
        {
            var round = isEncrypt ? this.Rounds : 1;
            for (byte k = 0; k < this.Rounds; k++)
            {
                if (k < this.Rounds - 1)
                    XORl(ref buffer, F(buffer, 0), 0);
                else
                    XORr(ref buffer, F(buffer, 0), 0);
                round += isEncrypt ? -1 : 1;
            }
            return buffer;
        }

    }
}
