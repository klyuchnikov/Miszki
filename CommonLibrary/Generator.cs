using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    /// <summary>
    /// Линейный конгруэнтный генератор псевдослучайных чисел
    /// </summary>
    public class CongruentialGenerator
    {
        /// <summary>
        /// Последнее значение
        /// </summary>
        private ulong lastValue;

        /// <summary>
        /// Мультипликатор
        /// </summary>
        public ulong a = 6364136223846793005;

        /// <summary>
        /// Инкремент
        /// </summary>
        public ulong c = 1442695040888963407;

        /// <summary>
        /// Модуль
        /// </summary>
        public double m = Math.Pow(2, 32);

        /// <summary>
        /// Линейный конгруэнтный генератор псевдослучайных чисел
        /// </summary>
        /// <param name="firstValue">Начальное значение</param>
        public CongruentialGenerator(ulong firstValue)
        {
            this.lastValue = firstValue;
        }

        /// <summary>
        /// Получение очередного псевдослучайного числа
        /// </summary>
        /// <returns>Псевдослучайное число</returns>
        public ulong Next()
        {
            this.lastValue = (ulong)((this.a * this.lastValue + this.c) % this.m);
            return this.lastValue;
        }

        /// <summary>
        /// Получение очередного псевдослучайного числа (double)
        /// </summary>
        /// <returns>Псевдослучайное число (double)</returns>
        public double NextDouble()
        {
            var res = ((this.a * this.lastValue + this.c) % this.m);
            this.lastValue = (ulong)res;
            return res;
        }

        /// <summary>
        /// Получение очередного псевдослучайного числа
        /// </summary>
        /// <param name="maxValue">Максимальное значение результата</param>
        /// <returns>Псевдослучайное число</returns>
        public ulong Next(ulong maxValue)
        {
            return (ulong)(maxValue * (Next() / this.m));
        }

        /// <summary>
        /// Получение очередного псевдослучайного числа
        /// </summary>
        /// <param name="minValue">Минимальное значение результата</param>
        /// <param name="maxValue">Максимальное значение результата</param>
        /// <returns>Псевдослучайное число</returns>
        public ulong Next(ulong minValue, ulong maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("\"minValue\" can not be greater than \"maxValue\"");
            }
            return (ulong)(((maxValue - minValue) * (Next() / this.m)) + minValue);
        }

        /// <summary>
        /// Получение очередного псевдослучайного числа (double)
        /// </summary>
        /// <param name="minValue">Минимальное значение результата (double)</param>
        /// <param name="maxValue">Максимальное значение результата (double)</param>
        /// <returns>Псевдослучайное число (double)</returns>
        public double Next(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("\"minValue\" can not be greater than \"maxValue\"");
            }
            if (minValue < 0 || maxValue < 0)
            {
                throw new ArgumentException("Value can not be less than zero (\"minValue\" or \"maxValue\")");
            }
            return (((maxValue - minValue) * (NextDouble() / this.m)) + minValue);
        }
        /// <summary>
        /// Получение очередного псевдослучайного числа (double)
        /// </summary>
        /// <param name="max">Максимальное значение результата (double)</param>
        /// <returns>Псевдослучайное число (double)</returns>
        public double Next(double max)
        {
            if (max < 0)
            {
                throw new ArgumentException("\"maxValue\" can not be less than zero.");
            }

            return (max * (NextDouble() / this.m));
        }
    }
}
