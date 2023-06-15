namespace HWMoney
{
    public class Money
    {
        private long rubles;
        private short kopeyki;

        public Money()
        {
            this.rubles = 0;
            this.kopeyki = 0;
        }

        public Money(long rubles, short kopeyki)
        {
            try
            {
                if (kopeyki >= 100 || kopeyki < 0)
                {
                    throw new OverflowException($"ОШИБКА: копеек не может быть больше или равно 100 или меньше 0!");
                }

                this.rubles = rubles;
                this.kopeyki = kopeyki;
            }
            catch (OverflowException oe)
            {
                Console.WriteLine(oe.Message);
                this.rubles = 0;
                this.kopeyki = 0;
            }
        }

        public static Money operator -(Money m, decimal money)
        {
            try
            {
                long rubles = (long)(m.rubles - Math.Truncate(money));
                short kopeyki = (short)(m.kopeyki - (money - Math.Truncate(money)) * 100);

                if (rubles < 0 || kopeyki < 0)
                {
                    throw new OverflowException($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {rubles} руб. {kopeyki} коп.");
                }
                if (rubles > long.MaxValue || rubles < long.MinValue || kopeyki > short.MaxValue || kopeyki < short.MinValue)
                {
                    throw new OverflowException($"Архивы нашего банка не позволяют хранить столько.");
                }

                return new Money(rubles, kopeyki);
            }
            catch (OverflowException oe)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(oe.Message); Console.ResetColor();

                return new Money(0, 0);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine(ex.Data);

            //    Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
            //    return new Money(0, 0);
            //}
        }

        public static Money operator +(Money m, decimal money)
        {
            try
            {
                long rubles = (long)(m.rubles + Math.Truncate(money));
                short kopeyki = (short)(m.kopeyki + (money - Math.Truncate(money)) * 100);

                return new Money(rubles, kopeyki);
            }
            catch (OverflowException ofe)
            {
                Console.WriteLine("ОНО " + ofe.Message);

                return new Money(0, 0);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
                return new Money(m.rubles, m.kopeyki);
            }
        }

        public static Money operator *(Money m, long num)
        {
            try
            {
                long rubles = m.rubles * num;

                return new Money(rubles, m.kopeyki);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
                return new Money(m.rubles, m.kopeyki);
            }
        }

        public static Money operator /(Money m, long num)
        {
            try
            {
                long rubles = m.rubles / num;

                return new Money(rubles, m.kopeyki);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
                return new Money(m.rubles, m.kopeyki);
            }
        }

        public static Money operator ++(Money m)
        {
            try
            {
                return new Money(m.rubles, (short) (m.kopeyki + 1));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
                return new Money(m.rubles, m.kopeyki);
            }
        }

        public static Money operator --(Money m)
        {
            try
            {
                return new Money(m.rubles, (short) (m.kopeyki - 1));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"К сожалению, теперь Вы банкрот, ваша задолженность составляет {m.rubles} руб. {m.kopeyki - 1} коп."); Console.ResetColor();
                return new Money(0, 0);
            }
        }

        public override string ToString()
        {
            return $"{this.rubles} руб. {this.kopeyki} коп.";
        }
    }



    internal class Program
    {
        static void Main()
        {
            Money money = new Money(1, 0);
            Console.WriteLine($"Изначально денег: {money}");

            //money = money - 24.84m;
            money = money - 18446744073709551615;
            Console.WriteLine($"money - 3483: {money}");
        }
    }
}