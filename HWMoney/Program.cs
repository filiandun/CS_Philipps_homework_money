namespace HWMoney
{
    public class Money
    {
        private ulong rubles;
        private ushort pennies;

        public Money()
        {
            this.rubles = 0;
            this.pennies = 0;
        }

        public Money(ulong rubles, ushort pennies)
        {
            try
            {
                if (pennies >= 100)
                {
                    throw new OverflowException($"ОШИБКА: копеек не может быть больше или равно 100! (Вы задали: {pennies})");
                }

                this.rubles = rubles;
                this.pennies = pennies;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static Money operator -(Money m, decimal money)
        {
            try
            {
                ulong rubles = (ulong)(m.rubles - Math.Round(money));
                ushort pennies = (ushort)(m.pennies - (money - Math.Round(money)) * 100);

                return new Money(rubles, pennies);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public static Money operator +(Money m, decimal money)
        {
            try
            {
                ulong rubles = (ulong)(m.rubles + Math.Round(money));
                ushort pennies = (ushort)(m.pennies + (money - Math.Round(money)) * 100);

                return new Money(rubles, pennies);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь Вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public static Money operator *(Money m, ulong num)
        {
            try
            {
                ulong rubles = m.rubles * num;

                return new Money(rubles, m.pennies);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь Вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public static Money operator /(Money m, ulong num)
        {
            try
            {
                ulong rubles = m.rubles / num;

                return new Money(rubles, m.pennies);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь Вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public static Money operator ++(Money m)
        {
            try
            {
                return new Money(m.rubles, (ushort) (m.pennies + 1));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь Вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public static Money operator --(Money m)
        {
            try
            {
                return new Money(m.rubles, (ushort) (m.pennies - 1));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("К сожалению, теперь Вы банкрот"); Console.ResetColor();
                return new Money(m.rubles, m.pennies);
            }
        }

        public override string ToString()
        {
            return $"{this.rubles} руб. {this.pennies} коп.";
        }
    }



    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}