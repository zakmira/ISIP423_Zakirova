using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prakktika8
{
    internal class Program
    {
        public class User
        {
            public User user {  get; set; }
            public int Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public bool SignUp(string email, string password) // регистрация
            {
                return true;
            }

            public bool SignIn(string email, string password) // вход в аккаунт
            {
                return true;
            }

            public void HandleSignUp()
            {
                Console.WriteLine("Введите email: ");
                var email = Console.ReadLine();
                Console.WriteLine("Введите пароль: ");
                var password = Console.ReadLine();
                Console.WriteLine("Повторите пароль: ");
                var passwordRepeat = Console.ReadLine();
                if (password != passwordRepeat)
                {
                    Console.WriteLine("Пароли не совпадают!");
                }

                if (user.SignUp(email, password))
                {
                    Console.WriteLine("Вы зарегистрированы!");
                }
                else
                {
                    Console.WriteLine("Пользователь уже существует");
                }
               

            }

            public void HandleSignIn()
            {
                Console.WriteLine("Введите email: ");
                var email = Console.ReadLine();
                Console.WriteLine("Введите пароль: ");
                var password = Console.ReadLine();

                if (user.SignIn(email, password))
                {
                    Console.WriteLine("Добро пожаловать!");
                }
                else
                {
                    Console.WriteLine("Неверный email или пароль!");
                }
            }
        }
       
        public class Marketplace
        {
            private List<Items> allItems;
            private List<PVZ> allPVZ;
            private User currentUser = new User();

            public Marketplace()
            {
                allItems = new List<Items>();
                allPVZ = new List<PVZ>();

                LoadDB();

            }

            public void LoadDB()
            {
                allItems = Core.Context.Items.ToList();
                allPVZ = Core.Context.PVZ.ToList();
            }

            public void LookItems()
            {
                // для незареганных тоже
            }

            public void AddToCart()
            {

            }

            public void LookCart()
            {

            }

            public void DeleteItem()
            {

            }

            public void Order()
            {
                // реализовать выбор: заказать всю корзину или только один предмет
            }
            
            public void ChoosePVZ()
            {
                foreach (var pvz in allPVZ)
                {

                }

            }

            public void LookOrders()
            {
                // сортировка по дате
            }
        }


        static void Main(string[] args)
        {
        }
    }
}


