using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prakktika8
{
    internal class Program
    {

        public class Marketplace
        {
            public RegisteredUsers User { get; set; }
            private List<Items> allItems;
            private List<PVZ> allPVZ;
            private List<OrderItems> cart;

            public bool SignUp(string email, string password)
            {
                var userExist = Core.Context.RegisteredUsers.FirstOrDefault(u => u.Email == email);
                if (userExist != null)
                {
                    return false; // уже существует
                }

                var newUser = new RegisteredUsers { Email = email, Password = password };
                Core.Context.RegisteredUsers.Add(newUser);
                Core.Context.SaveChanges();

                User = newUser; // установить текущего пользователя
                return true;
            }

            public bool SignIn(string email, string password)
            {
                var userSignedUp = Core.Context.RegisteredUsers.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (userSignedUp != null)
                {
                    User = userSignedUp;
                    return true;
                }
                return false;
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
                    return;
                }

                if (SignUp(email, password))
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

                if (SignIn(email, password))
                {
                    Console.WriteLine("Добро пожаловать!");
                }
                else
                {
                    Console.WriteLine("Неверный email или пароль!");
                }
            }
        
            public Marketplace()
            {
                allItems = new List<Items>();
                allPVZ = new List<PVZ>();
                cart = new List<OrderItems>();

                LoadDB();

            }

            public void LoadDB()
            {
                allItems = Core.Context.Items.ToList();
                allPVZ = Core.Context.PVZ.ToList();
            }

            public void LookItems()
            {
                foreach (var item in allItems)
                {
                    Console.WriteLine($"ID {item.ItemID}: {item.ItemName} - {item.Price}");
                }
                // для незареганных тоже
            }

            public void AddToCart()
            {
                Console.WriteLine("Введите ID товара:");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    var item = allItems.FirstOrDefault(i => i.ItemID == itemId);
                    if (item == null)
                    {
                        Console.WriteLine("Товар не найден.");
                        return;
                    }

                    Console.WriteLine("Введите количество:");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        var existCartItem = cart.FirstOrDefault(c => c.ItemID == itemId);
                        if (existCartItem != null)
                        {
                            existCartItem.Quantity += quantity;
                        }
                        else
                        {
                            cart.Add(new OrderItems
                            {
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                Price = item.Price,
                                Quantity = quantity
                            });
                        }
                        Console.WriteLine("Товар добавлен в корзину!");
                    }
                    else
                    {
                        Console.WriteLine("Неверное кол-во!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ID товара!");
                }
            }

            public void LookCart()
            {
                if (cart.Count == 0)
                {
                    Console.WriteLine("Корзина пуста.");
                    return;
                }

                Console.WriteLine("Ваша корзина:");
                foreach (var item in cart)
                {
                    Console.WriteLine($"{item.ItemName} {item.Quantity} шт. — {item.Price * item.Quantity} руб.");
                }
            }
            public void DeleteItem()
            {
                Console.WriteLine("Введите ID товара для удаления: ");
                if (int.TryParse(Console.ReadLine(), out int itemId))
                {
                    var existCartItem = cart.FirstOrDefault(c => c.ItemID == itemId);
                    if (existCartItem == null)
                    {
                        Console.WriteLine("Товар не найден");
                        return;
                    }

                    Console.WriteLine("Введите кол-во для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        if (quantity >= existCartItem.Quantity)
                        {
                            cart.Remove(existCartItem);
                            Console.WriteLine("Товар удален из корзины");
                        }
                        else
                        {
                            existCartItem.Quantity -= quantity;
                            Console.WriteLine("Количество товара уменьшено");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверное количество!");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ID товара!");
                }
            }

            public void Order()
            {
                if (User == null)
                {
                    Console.WriteLine("Выполните вход в аккаунт");
                    return;
                }

                Console.WriteLine("Выберите ПВЗ: ");
                for (int i = 0; i < allPVZ.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {allPVZ[i].Address}");
                }

                if (!int.TryParse(Console.ReadLine(), out int pvzChoice) || pvzChoice < 1 || pvzChoice > allPVZ.Count)
                {
                    Console.WriteLine("Такого ПВЗ нет");
                    return;
                }

                var selectedPVZ = allPVZ[pvzChoice - 1];

                if (cart.Count == 0)
                {
                    Console.WriteLine("Корзина пуста");
                    return;
                }

                Console.WriteLine("Заказать всю корзину - 1; Заказать определенный товар из корзины - 2");
                if (!int.TryParse(Console.ReadLine(), out int orderChoice))
                {
                    Console.WriteLine("Неверный выбор");
                    return;
                }

                if (orderChoice == 1)
                {
                    // заказать всю корзину
                    CreateOrderFromCart(selectedPVZ);
                }
                else if (orderChoice == 2)
                {
                    // заказать один товар
                    OrderSingleItem(selectedPVZ);
                }
                else
                {
                    Console.WriteLine("Неверный выбор");
                }
            }

            private void CreateOrderFromCart(PVZ pvz)
            {
                var order = new Orders
                {
                    UserID = User.UserID,
                    PVZid = pvz.PVZid,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.Sum(c => c.Price * c.Quantity)
                };

                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                foreach (var item in cart)
                {
                    var orderItem = new OrderItems
                    {
                        OrderID = order.OrderID,      
                        ItemID = item.ItemID,         
                        Quantity = item.Quantity,     
                        Price = item.Price 
                    };
                    Core.Context.OrderItems.Add(orderItem);
                }

                Core.Context.SaveChanges();
                cart.Clear(); 

                Console.WriteLine("Заказ оформлен!");
            }

            private void OrderSingleItem(PVZ pvz)
            {
                Console.WriteLine("Введите ID товара из корзины: ");
                if (!int.TryParse(Console.ReadLine(), out int itemId))
                {
                    Console.WriteLine("Некорректный ID товара");
                    return;
                }

                var cartItem = cart.FirstOrDefault(c => c.ItemID == itemId);
                if (cartItem == null)
                {
                    Console.WriteLine("Товар не найден в корзине");
                    return;
                }

                Console.Write($"Введите нужное кол-во товара {cartItem.ItemName}: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0 || quantity > cartItem.Quantity)
                {
                    Console.WriteLine("Некорректное кол-во!");
                    return;
                }

                var order = new Orders
                {
                    UserID = User.UserID,
                    PVZid = pvz.PVZid,
                    OrderDate = DateTime.Now,
                    TotalAmount = cartItem.Price * quantity
                };

                Core.Context.Orders.Add(order);
                Core.Context.SaveChanges();

                var orderItem = new OrderItems
                {
                    OrderID = order.OrderID,
                    ItemID = cartItem.ItemID,
                    Quantity = quantity,
                    Price = cartItem.Price
                };
                Core.Context.OrderItems.Add(orderItem);
                Core.Context.SaveChanges();

                if (quantity == cartItem.Quantity)
                {
                    cart.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= quantity;
                }

                Console.WriteLine("Заказ оформлен!");
            }

            public void LookOrders()
            {
                if (User == null)
                {
                    Console.WriteLine("Войдите в аккаунт");
                    return;
                }

                var orders = Core.Context.Orders
                    .Where(o => o.UserID == User.UserID)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();

                if (!orders.Any())
                {
                    Console.WriteLine("У Вас нет заказов..");
                    return;
                }

                foreach (var order in orders)
                {
                    Console.WriteLine($"Заказ {order.OrderID} от {order.OrderDate:DD.MM.YYYY} на сумму {order.TotalAmount} руб.");

                    var orderItems = Core.Context.OrderItems.Where(oi => oi.OrderID == order.OrderID).ToList();
                    foreach (var item in orderItems)
                    {
                        var product = Core.Context.Items.FirstOrDefault(i => i.ItemID == item.ItemID);
                        Console.WriteLine($"{product.ItemName} {item.Quantity} шт. - {item.Price * item.Quantity} руб.");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var marketplace = new Marketplace();
        }
    }
}


