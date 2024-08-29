using MongoDB.Driver.Core.Authentication;
using ShopLibrary.Services;
namespace ShopApp
{
    class Program
    {
        async static Task Main(string[] args)
        {
           
            UserInterface UI = new UserInterface(new CustomerService(), new ItemService(), new SupplierService());
            Console.WriteLine("Welcome to our shop, I would like to request your identity! Who are you?");
            Console.WriteLine("1) Customer");
            Console.WriteLine("2) Supplier");
            Console.WriteLine("3) Manager");
            string choiceInput = Console.ReadLine() ?? "";
            int choice = 1;
            if(int.TryParse(choiceInput, out choice))
            {
                switch (choice)
                {
                    case 1:
                        await UI.customerNameInterface();
                        break;
                    case 2:
                        await UI.supplierNameInterface();
                        break;
                    case 3:
                        await UI.managerHomeInterface();
                        break;
                }
                        
                }
            }
        }
    }
