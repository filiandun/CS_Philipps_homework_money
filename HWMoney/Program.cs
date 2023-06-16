namespace HWMoney
{
    public class BankruptException : Exception
    {
        public BankruptException() { }
        public BankruptException(string message) : base(message) { }
    }


    public class Money
    {
        private long rubles;
        private short kopeyki;

        private bool bankrupt;



        public Money()
        {
            this.rubles = 0;
            this.kopeyki = 0;
        }

        public Money(long rubles, short kopeyki)
        {
            try
            {
                if (kopeyki < 0)
                {
                    throw new ArgumentException($"ОШИБКА: вы ввели {kopeyki} коп., но копеек не может быть меньше 0!");
                }

                if (rubles < 0)
                {
                    throw new ArgumentException($"ОШИБКА: вы ввели {rubles} руб., но рублей не может быть меньше 0!");
                }

                if (kopeyki >= 100)
                {
                    throw new ArgumentException($"ОШИБКА: вы ввели {kopeyki} коп., но копеек не может быть больше/равно 100!");
                }

                this.rubles = rubles;
                this.kopeyki = kopeyki;
                this.bankrupt = false;
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
                this.rubles = 0;
                this.kopeyki = 0;
            }
        }

        public Money(long rubles, short kopeyki, bool bankrupt) : this(rubles, kopeyki)
        {
            this.bankrupt = bankrupt;
        }



        public static Money operator -(Money m, decimal money)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                if (money < 0)
                {
                    throw new ArgumentException($"Банк не может работать с отрицательными суммами ({money})");
                }

                decimal bufMoney = (m.rubles + ((decimal)m.kopeyki / 100)) - money; // складываются имеющиеся рубли и копейки (m.rubles + ((decimal)m.kopeyki / 100)), а после из них вычитается money

                if (bufMoney < 0)
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Abs(Math.Truncate(bufMoney))} руб. {(long) ((Math.Abs(bufMoney) - Math.Abs(Math.Truncate(bufMoney))) * 100)} коп.");
                }

                return new Money((long)Math.Truncate(bufMoney), (short)((bufMoney - Math.Truncate(bufMoney)) * 100));
            }
            catch (ArgumentException ae)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ae.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (OverflowException)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Банк не может работать с такими суммами."); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }

        public static Money operator +(Money m, decimal money)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                if (money < 0)
                {
                    throw new ArgumentException($"Банк не может работать с отрицательными суммами ({money}).");
                }
                //if (DecimalDigitsCount()) // в идеале сделать учёт кол-ва цифр после запятой, где кидать ArgumentException, если символов больше 2.

                decimal bufMoney = (m.rubles + ((decimal)m.kopeyki / 100)) + money; // складываются имеющиеся рубли и копейки (m.rubles + ((decimal)m.kopeyki / 100)), а после к ним прибавляется money

                if (bufMoney < 0)
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Abs(Math.Truncate(bufMoney))} руб. {(long)((Math.Abs(bufMoney) - Math.Abs(Math.Truncate(bufMoney))) * 100)} коп.");
                }

                return new Money((long)Math.Truncate(bufMoney), (short)((bufMoney - Math.Truncate(bufMoney)) * 100));
            }
            catch (ArgumentException ae)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ae.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (OverflowException)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Банк не может работать с такими суммами."); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо просто передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }



        public static Money operator *(Money m, long num)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                if (num < 0)
                {
                    throw new ArgumentException($"Банк не может работать с отрицательными суммами ({num}).");
                }

                long rubles = m.rubles * num;

                if (rubles < 0) // наверное не особо смысл есть, так как как при умножении можно отрицательное число получить?
                                // При условии, что отрицательный счёт быть не может, так как это bankrupt == true и кидается исключение и при отрицательном num также кидается исключение 
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Truncate((decimal) rubles - m.kopeyki)} руб. 0 коп.");
                }

                return new Money(rubles, m.kopeyki);
            }
            catch (ArgumentException ae)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ae.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (OverflowException)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Банк не может работать с такими суммами."); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо просто передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }



        public static Money operator /(Money m, long num)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                if (num < 0)
                {
                    throw new ArgumentException($"Банк не может работать с отрицательными суммами ({num}).");
                }
                
                if (num == 0)
                {
                    throw new DivideByZeroException("На ноль делить нельзя!");
                }

                long rubles = m.rubles > num ? m.rubles / num : -(m.rubles * num); // если условия не будет, то при делении делимого (m.rubles) на делитель (num), который будет больше делителя, получится просто 0

                if (rubles < 0)
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Abs(Math.Truncate(rubles + ((decimal)m.kopeyki / 100)))} руб. {100 - m.kopeyki} коп.");
                }

                return new Money(rubles, m.kopeyki);
            }
            catch (ArgumentException ae)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ae.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (OverflowException)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Банк не может работать с такими суммами."); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (DivideByZeroException dbze)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(dbze.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо просто передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }



        public static Money operator ++(Money m)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                decimal bufMoney = m.rubles + ((decimal) m.kopeyki / 100) + 0.01m;

                if (bufMoney < 0)
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Abs(Math.Truncate(bufMoney))} руб. {(long)((Math.Abs(bufMoney) - Math.Abs(Math.Truncate(bufMoney))) * 100)} коп.");
                }

                return new Money((long)Math.Truncate(bufMoney), (short)((bufMoney - Math.Truncate(bufMoney)) * 100));
            }
            catch (OverflowException oe)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write(oe.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо просто передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }

        public static Money operator --(Money m)
        {
            try
            {
                if (m.bankrupt == true)
                {
                    throw new BankruptException("Ваш счёт был заморожен на период банкротства, вы не можете проводить какие-либо операции с ним.");
                }

                decimal bufMoney = m.rubles + ((decimal)m.kopeyki / 100) - 0.01m;

                if (bufMoney < 0)
                {
                    throw new BankruptException($"К сожалению, теперь Вы - банкрот, ваша задолженность составляет {Math.Abs(Math.Truncate(bufMoney))} руб. {(long)((Math.Abs(bufMoney) - Math.Abs(Math.Truncate(bufMoney))) * 100)} коп.");
                }

                return new Money((long)Math.Truncate(bufMoney), (short)((bufMoney - Math.Truncate(bufMoney)) * 100));
            }
            catch (OverflowException oe)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Write(oe.Message); Console.ResetColor();

                return new Money(m.rubles, m.kopeyki);
            }
            catch (BankruptException be)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(be.Message); Console.ResetColor();

                return new Money(0, 0, true); // помимо просто передачи рублей и копеек, передаётся true в bool bankrupt
            }
        }



        public static bool operator >(Money m1, Money m2)
        {
            if (m1.bankrupt && m2.bankrupt)
            {
                return false;
            }
            if (m1.bankrupt)
            {
                return false;
            }
            if (m2.bankrupt)
            {
                return true;
            }

            return (m1.rubles + ((decimal)m1.kopeyki / 100)) > (m2.rubles + ((decimal)m2.kopeyki / 100));
        }

        public static bool operator <(Money m1, Money m2)
        {
            if (m1.bankrupt && m2.bankrupt)
            {
                return false;
            }
            if (m1.bankrupt)
            {
                return true;
            }
            if (m2.bankrupt)
            {
                return false;
            }

            return (m1.rubles + ((decimal)m1.kopeyki / 100)) < (m2.rubles + ((decimal)m2.kopeyki / 100));
        }



        public static bool operator ==(Money m1, Money m2)
        {
            if (m1.bankrupt && m2.bankrupt)
            {
                return true;
            }

            return (m1.rubles + ((decimal)m1.kopeyki / 100)) == (m2.rubles + ((decimal)m2.kopeyki / 100));
        }

        public static bool operator !=(Money m1, Money m2)
        {
            if (m1.bankrupt && m2.bankrupt)
            {
                return false;
            }

            return (m1.rubles + ((decimal)m1.kopeyki / 100)) != (m2.rubles + ((decimal)m2.kopeyki / 100));
        }



        public override string ToString()
        {
            if (this.bankrupt == false)
            {
                return $"{this.rubles} руб. {this.kopeyki} коп.";
            }
            return $"банкрот, счёт обнулён и заморожен ({this.rubles} руб. {this.kopeyki} коп.)";
        }
    }



    internal class Program
    {
        static void Main()
        {
            Money money = new Money(1, 20);
            Console.WriteLine($"Изначально money: {money}\n");


            // ОПЕРАТОР+
            Console.WriteLine("[OPERATOR+]");

            money += 2;
            Console.WriteLine($"Money + 2: {money}\n");

            money += -2; // не получится, так как число отрицательное, будет исключение
            Console.WriteLine($"Money + (-2): {money}\n");


            money += 1.95m;
            Console.WriteLine($"Money + 1.95m: {money}\n");

            money += decimal.MaxValue; // не получится, будет кинуто исключение
            Console.WriteLine($"Money + decimal.MaxValue: {money}\n\n");



            // ОПЕРАТОР+
            Console.WriteLine("[OPERATOR-]");

            money -= 3;
            Console.WriteLine($"Money - 3: {money}\n");

            money -= -1; // не получится, так как число отрицательное, будет исключение
            Console.WriteLine($"Money - (-1): {money}\n");


            money -= 2.15m;
            Console.WriteLine($"Money - 2.15m: {money}\n");

            //money -= decimal.MaxValue; // получится, так как никак больше decimal.MaxValue не выйдет получить
            //Console.WriteLine($"Money - decimal.MaxValue: {money}\n");

            money -= 0.10m; // банкротство!
            Console.WriteLine($"Money - 0.10m: {money}\n\n");



            // ОПЕРАЦИИ ПРИ БАНКРОТСТВЕ
            Console.WriteLine("[ОПЕРАЦИИ ПРИ БАНКРОТСТВЕ]");

            money += 1000.20m;
            Console.WriteLine($"Money + 1000.20m: {money}\n");

            money -= 100m;
            Console.WriteLine($"Money - 100: {money}\n\n\n");




            Money money2 = new Money(1, 20);
            Console.WriteLine($"Изначально money2: {money2}\n");

            // ОПЕРАТОР *
            Console.WriteLine("[OPERATOR*]");

            money2 *= 12; // умножать можно только на целые числа
            Console.WriteLine($"Money2 * 3: {money2}\n");

            money2 *= -2; // не получится, так как число отрицательное, будет исключение
            Console.WriteLine($"Money2 * (-2): {money2}\n\n");



            // ОПЕРАТОР /
            Console.WriteLine("[OPERATOR/]");

            money2 /= 2; // делить тоже можно только на целые числа
            Console.WriteLine($"Money2 / 2: {money2}\n");

            money2 /= -2; // не получится, так как число отрицательное, будет исключение
            Console.WriteLine($"Money2 / (-2): {money2}\n");

            money2 /= 8; // банкрот!
            Console.WriteLine($"Money2 / 4: {money2}\n\n");



            // ОПЕРАЦИИ ПРИ БАНКРОТСТВЕ 2
            Console.WriteLine("[ОПЕРАЦИИ ПРИ БАНКРОТСТВЕ 2]");

            money2 *= 100;
            Console.WriteLine($"Money2 * 100: {money2}\n");

            money2 /= 10;
            Console.WriteLine($"Money2 / 10: {money2}\n\n");



            // ОПЕРАТОРЫ >, <, ==, !=
            Console.WriteLine("[ОПЕРАТОРЫ >, <, ==, !=]");

            Money money3 = new Money(20, 65);
            Console.WriteLine($"Money3: {money3}");

            Money money4 = new Money(19, 99);
            Console.WriteLine($"Money4: {money4}\n");


            Console.WriteLine($"Money3 > Money4: {money3 > money4}");
            Console.WriteLine($"Money3 < Money4: {money3 < money4}\n");

            Console.WriteLine($"Money < Money2: {money < money2}"); // это два банкрота
            Console.WriteLine($"Money > Money2: {money > money2}\n\n"); // это два банкрота


            Console.WriteLine($"Money3 == Money4: {money3 == money4}");
            Console.WriteLine($"Money3 != Money4: {money3 != money4}\n");

            Console.WriteLine($"Money == Money2: {money == money2}"); // это два банкрота
            Console.WriteLine($"Money != Money2: {money != money2}\n\n"); // это два банкрота




            //ЛУЧШЕ В ПОСЛЕДНЮЮ ОЧЕРЕДЬ ИХ ОТДЕЛЬНО ПРОВЕРИТЬ, ТАК КАК СТОИТ Console.Clear();
            //// ОПЕРАТОР++
            //for (int i = 0; i <= 100; ++i)
            //{
            //    Console.Clear();
            //    Console.WriteLine($"Money3: {money3++}");
            //    Thread.Sleep(100);
            //}

            //Thread.Sleep(1000);

            //// ОПЕРАТОР--
            //for (int i = 0; i <= 1000; ++i)
            //{
            //    Console.Clear();
            //    Console.WriteLine($"Money3: {--money3}");
            //    Thread.Sleep(10);
            //}
        }
    }
}